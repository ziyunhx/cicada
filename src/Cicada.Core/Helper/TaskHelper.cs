using Cicada.Core.Enums;
using Cicada.Core.Interfaces;
using Cicada.Core.Notication;
using Cicada.EFCore.Shared;
using Cicada.EFCore.Shared.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Cicada.Core.Helper
{
    public class TaskHelper
    {
        /// <summary>
        /// 执行任务
        /// </summary>
        /// <param name="taskInfo"></param>
        /// <param name="cancel"></param>
        /// <returns></returns>
        public static Task<CheckResult> DoTask(TaskInfo taskInfo, CancellationToken cancel)
        {
            Task<CheckResult> T = new Task<CheckResult>(() =>
            {
                CheckResult CheckResult = new CheckResult();

                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                try
                {
                    Task.Factory.StartNew(() =>
                    {
                        ProcessManager process = new ProcessManager();
                        process.DoCommand(taskInfo, ref CheckResult);
                    }).Wait(cancel);
                }
                catch (Exception ex)
                {
                    //判断任务是否是因为超时被取消而导致的异常
                    if (cancel.IsCancellationRequested)
                    {
                        CheckResult.Status = 504;
                        CheckResult.MessageLevel = (int)Enums.EventLevel.Critical;
                        CheckResult.MessageInfo = "Task Timeout!";
                    }
                    else
                    {
                        CheckResult.Status = 500;
                        CheckResult.MessageLevel = (int)Enums.EventLevel.Critical;
                        CheckResult.MessageInfo = ex.ToString();
                    }
                }
                stopwatch.Stop();
                CheckResult.CostMillisecond = stopwatch.ElapsedMilliseconds;

                CheckResult.TaskId = taskInfo.TaskId;

                CheckResult.CheckId = Guid.NewGuid().ToString("N");
                CheckResult.EndTime = DateTime.Now;

                return CheckResult;
            });
            return T;
        }

        /// <summary>
        /// 启动任务进行循环
        /// </summary>
        /// <returns></returns>
        public static Task<Boolean> StartTasksLoopAsync()
        {
            Task<Boolean> Task = new Task<Boolean>(() =>
            {
                var cache = new List<TaskInfo>();//对象缓存
                DateTime lastRefreshTime = DateTime.MinValue;
                int MaxThreadPool = 2000;//最大任务数
                int ThreadCount = 0;//任务计数器
                object locker = new object();
                do
                {
                    try
                    {
                        //每分钟刷新全部任务，放入缓存
                        if (lastRefreshTime <= DateTime.Now.AddMinutes(-1))//每分钟从数据库取一次
                        {
                            //using (CicadaDbContext entity = new CicadaDbContext())
                            //{
                            //    var tasks = entity.TaskInfo.Where(i => i.Disabled == false && (i.EndTime == null || i.EndTime < DateTime.Now)).ToArray();
                            //    cache.Clear();
                            //    foreach (var task in tasks)
                            //    {
                            //        if (!cache.Any(i => i.TaskId == task.TaskId))
                            //        {
                            //            cache.Add(task);
                            //        }
                            //    }
                            //}
                            lastRefreshTime = DateTime.Now;
                        }

                        //从缓存获取当前时间需要执行的任务，并通过异步线程池进行执行，注意限制线程池的最大数量
                        var _tinfos = cache.Where(i => i.NextRunTime <= DateTime.Now).OrderBy(i => i.NextRunTime);//拿出执行时间小于等于当前时间的任务
                        if (_tinfos != null && _tinfos.Count() > 0)
                        {
                            int startTaskNum = 0;

                            foreach (TaskInfo _tinfo in _tinfos)
                            {
                                if (TaskCacheHelper.IsRuning(_tinfo.TaskId))
                                    continue;

                                startTaskNum++;

                                if (ThreadCount < MaxThreadPool)
                                {
                                    #region 线程计数器
                                    Interlocked.Increment(ref ThreadCount);
                                    //cache.Remove(_tinfo);//将任务从缓存中删除
                                    #endregion

                                    #region 任务取消相关
                                    CancellationTokenSource cancel = null;//
                                    if (_tinfo.Timeout > 0)
                                    {
                                        TimeSpan timeSpan = new TimeSpan(_tinfo.Timeout * 10000);
                                        cancel = new CancellationTokenSource(timeSpan);
                                    }
                                    else
                                    {
                                        cancel = new CancellationTokenSource();
                                    }
                                    #endregion

                                    #region 计算下次执行时间相关
                                    try
                                    {
                                        _tinfo.NextRunTime = TaskHelper.CalNextRunTime(_tinfo);//计算下次执行时间
                                        Console.WriteLine($"{DateTime.Now}: {_tinfo.Name} Running. Next run at {_tinfo.NextRunTime}.");
                                    }
                                    catch (Exception ex)
                                    {
                                        _tinfo.NextRunTime = DateTime.MaxValue;
                                        //写日志操作
                                        Console.WriteLine(ex.ToString());
                                    }
                                    #endregion

                                    #region 任务执行及回调
                                    Task<CheckResult> T = DoTask(_tinfo, cancel.Token);
                                    T.ContinueWith(i =>
                                    {
                                        var checkResult = i.Result;

                                        #region 通知相关
                                        //单节点模式使用数据库记录上次检查状态，集群模式通过Redis记录
                                        if (checkResult.Status != _tinfo.LastCheckStatus)
                                        {
                                            TaskHelper.PushNotice(_tinfo, checkResult);
                                        }
                                        #endregion
                                        #region 任务状态改变相关
                                        _tinfo.LastCheckStatus = checkResult.Status;
                                        #endregion

                                        #region 数据库写入操作
                                        //using (CicadaDbContext entity = new CicadaDbContext())
                                        //{
                                        //    entity.CheckResult.Add(checkResult);
                                        //    entity.TaskInfo.Update(_tinfo);
                                        //    entity.SaveChanges();
                                        //}
                                        #endregion

                                        #region 线程计数器
                                        Interlocked.Decrement(ref ThreadCount);
                                        #endregion

                                        if (checkResult.Status < 200 || checkResult.Status >= 300)
                                        {
                                            TaskCacheHelper.Stop(_tinfo.TaskId, false);

                                            if (TaskCacheHelper.RetryRunTask(_tinfo.TaskId, _tinfo.Retries))
                                            {
                                                //todo 计算下次重试时间，加入到队列
                                            }
                                        }
                                        else
                                        {
                                            TaskCacheHelper.Stop(_tinfo.TaskId, true);
                                        }

                                    });
                                    T.Start();//任务启动
                                    #endregion
                                }
                            }

                            if (startTaskNum > 0)
                                Logger.Info(string.Format("Finish check tasks! {0} Tasks is started. {1} Thread is running.", startTaskNum, ThreadCount));
                        }
                        Thread.Sleep(5);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                }
                while (true);
            });

            return Task;
        }


        public static DateTime CalNextRunTime(TaskInfo taskInfo)
        {
            DateTime sourcedt = DateTime.Now;
            DateTime endTime = taskInfo.EndTime == null ? DateTime.MaxValue : taskInfo.EndTime.Value;
            DateTime source = GetNextRunTimeWithSchedule(sourcedt, taskInfo.Schedule);
            //DateTime source = GetNextRunTimeWithSceduleFromNow(taskInfo.Schedule);
            while (source < endTime)
            {
                if (IsOnWorkDay(source, taskInfo.WorkDays, taskInfo.SleepDays) &&
                    IsOnWorkTime(source.TimeOfDay, taskInfo.WorkTimes, taskInfo.SleepTimes))
                {
                    return source;
                }
                source = GetNextRunTimeWithSchedule(source, taskInfo.Schedule);
            }
            return source;
        }
        /// <summary>
        /// 根据Schedule 规则计算下次运行时间
        /// </summary>
        /// <param name="source">起始时间</param>
        /// <param name="value">Schedule规则</param>
        /// <returns></returns>
        private static DateTime GetNextRunTimeWithSchedule(DateTime source, string schedules)
        {
            if (!string.IsNullOrWhiteSpace(schedules))
            {
                List<DateTime> times = new List<DateTime>();

                string[] _schedules = schedules.Split(';');

                foreach (string value in _schedules)
                {
                    int Rcount = 0;
                    string[] strs = value.Split('/');
                    if (strs[0].Substring(0, 1).ToUpper() != "R" || strs.Count() != 3)
                    {
                        throw new Exception($"[{value}] is Wrong Value. Schedule not formatted correctly. Should look like: R/2014-03-08T20:00:00Z/PT2H");
                    }
                    else if (strs[0].Length > 1)
                    {
                        if (!int.TryParse(strs[0].Substring(1), out Rcount))
                        {
                            throw new Exception($"[{value}] is Wrong Value. Schedule not formatted correctly. Should look like: R/2014-03-08T20:00:00Z/PT2H");
                        }
                    }
                    //stime = stime == null ? DateTime.Parse(strs[1]) : stime;
                    DateTime stime = DateTime.Parse(strs[1]);
                    if (source < stime)
                    {
                        times.Add(stime);
                    }
                    else
                    {
                        long ticks = GetRepeatTicks(strs[2]);
                        TimeSpan dtime = source - stime;
                        int count = Convert.ToInt32(dtime.Ticks / ticks) + 1;
                        if (Rcount != 0 && count > Rcount)
                        {
                            //throw new Exception($"[{value}] is Wrong Value. Above the repeat count");
                        }
                        else
                        {
                            times.Add(stime.AddTicks(ticks * count));
                        }
                    }
                }

                if (times != null && times.Count > 0)
                {
                    return times.Min();
                }
            }

            return DateTime.MaxValue;
        }

        /// <summary>
        /// 根据特定字符计算时间Ticks
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private static long GetRepeatTicks(string value)
        {
            DateTime source = new DateTime();
            value = value.ToUpper();
            //删除第一个字符
            if (value.Substring(0, 1) != "P")
            {
                throw new Exception($"[{value}]is Wrong Value");
            }
            else
            {
                value = value.Remove(0, 1);
            }
            //分割时间与日期
            string[] daytime = value.Split('T');

            if (daytime.Count() > 2)
            {
                throw new Exception($"[{value}]is Wrong Value");
            }
            Regex rg = new Regex(@"\d+[A-Z]");
            foreach (Match i in rg.Matches(daytime[0]))
            {
                source = AddDatetime(source, i.Value, DateType.day);
            }
            if (daytime.Count() == 2)
            {
                foreach (Match i in rg.Matches(daytime[1]))
                {
                    source = AddDatetime(source, i.Value, DateType.time);
                }
            }
            return source.Ticks;
        }

        private static DateTime AddDatetime(DateTime source, string value, DateType dtype)
        {
            value = value.ToUpper();
            char key = value.Last();
            int dayvalue = Convert.ToInt32(value.Remove(value.Length - 1));
            DateTime result = source;
            if (dtype == DateType.day)
            {
                switch (key)
                {
                    case 'Y':
                        result = source.AddYears(dayvalue);
                        break;
                    case 'M':
                        result = source.AddMonths(dayvalue);
                        break;
                    case 'W':
                        result = source.AddDays(dayvalue * 7);
                        break;
                    case 'D':
                        result = source.AddDays(dayvalue);
                        break;
                    default: break;
                }
            }
            else
            {
                switch (key)
                {
                    case 'H':
                        result = source.AddHours(dayvalue);
                        break;
                    case 'M':
                        result = source.AddMinutes(dayvalue);
                        break;
                    case 'S':
                        result = source.AddSeconds(dayvalue);
                        break;
                    default: break;
                }
            }
            return result;
        }
        private static bool IsOnWorkDay(DateTime source, string workDays, string sleepDays)
        {
            if (string.IsNullOrEmpty(workDays) ? true : IsOnDay(source, workDays))
            {
                if (string.IsNullOrEmpty(sleepDays) ? true : !IsOnDay(source, sleepDays))
                {
                    return true;
                }
            }
            return false;
        }
        private static bool IsOnWorkTime(TimeSpan source, string workTimes, string sleepTimes)
        {
            if (string.IsNullOrEmpty(workTimes) || IsOnTime(source, workTimes))
            {
                if (string.IsNullOrEmpty(sleepTimes) || !IsOnTime(source, sleepTimes))
                {
                    return true;
                }
            }
            return false;
        }
        private static bool IsOnDay(DateTime source, string str)
        {
            string[] datestrs = str.Split('|');
            foreach (string i in datestrs)
            {
                //判断是否是关键词日期
                if (KeyOfdate(source, i) != null)
                {
                    return KeyOfdate(source, i).Value;
                }
                //如果不是抛出异常
                throw new Exception("Wrong Value");
            }
            return false;
        }
        private static bool IsOnTime(TimeSpan source, string str)
        {
            DateTime dateSource = new DateTime().Add(source);
            string[] datestrs = str.Split('|');
            foreach (string i in datestrs)
            {
                //必须包含"+"说明是个时间段 如 9H30M+11H
                if (i.ToUpper().Contains("+"))
                {
                    string[] times = i.Split('+');
                    if (times.Count() != 2)
                    {
                        throw new Exception("Wrong Value");
                    }
                    else
                    {
                        Regex rg = new Regex(@"\d+[A-Z]");

                        DateTime stime = new DateTime();
                        foreach (Match matchitem in rg.Matches(times[0]))
                        {
                            stime = AddDatetime(stime, matchitem.Value, DateType.time);
                        }

                        DateTime etime = new DateTime();
                        foreach (Match matchitem in rg.Matches(times[1]))
                        {
                            etime = AddDatetime(etime, matchitem.Value, DateType.time);
                        }

                        if (dateSource > stime && dateSource < etime)
                        {
                            return true;
                        }
                    }
                }
                else
                {
                    throw new Exception($"[{str}]is Wrong Value");
                }
            }
            return false;
        }
        private static bool? KeyOfdate(DateTime source, string str)
        {
            switch (str.ToUpper())
            {
                case "MONDAY":
                    if (source.DayOfWeek == DayOfWeek.Monday)
                    {
                        return true;
                    }
                    break;
                case "TUESDAY":
                    if (source.DayOfWeek == DayOfWeek.Tuesday)
                    {
                        return true;
                    }
                    break;
                case "WEDNESDAY":
                    if (source.DayOfWeek == DayOfWeek.Wednesday)
                    {
                        return true;
                    }
                    break;
                case "THURSDAY":
                    if (source.DayOfWeek == DayOfWeek.Thursday)
                    {
                        return true;
                    }
                    break;
                case "FRIDAY":
                    if (source.DayOfWeek == DayOfWeek.Friday)
                    {
                        return true;
                    }
                    break;
                case "SATURDAY":
                    if (source.DayOfWeek == DayOfWeek.Saturday)
                    {
                        return true;
                    }
                    break;
                case "SUNDAY":
                    if (source.DayOfWeek == DayOfWeek.Sunday)
                    {
                        return true;
                    }
                    break;
                default:
                    return null;
            }
            return false;
        }
        enum DateType
        {
            day,
            time
        }

        /// <summary>
        /// 推送通知
        /// </summary>
        /// <param name="taskInfo"></param>
        /// <param name="checkResult"></param>
        public static void PushNotice(TaskInfo taskInfo, CheckResult checkResult)
        {
            string messageLevel = Convert.ToInt32(checkResult.MessageLevel).ToString();
            //using (CicadaDbContext entity = new CicadaDbContext())
            //{
            //    List<INotication> Notications = new List<INotication>();
            //    Notications.Add(new WeChatNotication());//添加微信通知

            //    var notices = entity.Notice.Where(i => i.TaskId == taskInfo.TaskId).ToArray();
            //    if (notices.Count() == 0) return;//如果没有通知方式就不推送
            //    foreach (var item in notices)
            //    {
            //        if (string.IsNullOrEmpty(item.IgnoreTime) ? false : IsOnTime(DateTime.Now.TimeOfDay, item.IgnoreTime))
            //        {
            //            return;
            //        }
            //        string[] Levels = item.EffectLevels.Split('|');
            //        if (Levels.Contains(messageLevel))
            //        {
            //            #region 通知操作

            //            string[] _members = string.IsNullOrEmpty(item.ToMembers) ? null : item.ToMembers.Split('|');
            //            string[] _tags = string.IsNullOrEmpty(item.ToTags) ? null : item.ToTags.Split('|');
            //            string[] _partys = string.IsNullOrEmpty(item.ToPartys) ? null : item.ToPartys.Split('|');

            //            string ResultMessage = MessageResult(checkResult.MessageInfo, taskInfo.Name, (EventLevel)checkResult.MessageLevel, DateTime.Now);

            //            //发送通知
            //            foreach (var NoticatItem in Notications)
            //            {
            //                try
            //                {
            //                    bool IsNoticateSuccess = NoticatItem.Notify(ResultMessage, _members, _partys, _tags, (EventLevel)checkResult.MessageLevel);
            //                }
            //                catch (Exception ex)
            //                {
            //                }
            //            }
            //            #endregion

            //            #region 创建通知记录并写入
            //            NotifyRecode nRecode = new NotifyRecode()
            //            {
            //                CreateTime = DateTime.Now,
            //                NotifyRecodeId = Md5Helper.getMd5Hash(DateTime.Now.ToString() + checkResult.CheckId),
            //                Status = 0,
            //                TaskId = taskInfo.TaskId
            //            };
            //            entity.NotifyRecode.Add(nRecode);
            //            entity.SaveChanges();
            //            #endregion
            //        }
            //    }
            //}
        }

        private static string MessageResult(string msg, string TaskName, EventLevel MessageLevel, DateTime? Time, string Service = null, string Host = null, string Adress = null, string State = null)
        {
            if (Time == null) Time = DateTime.Now;
            StringBuilder MessageResult = new StringBuilder();
            MessageResult.Append($"{MessageLevel.ToString()} - [Cicada]");
            MessageResult.AppendLine();
            MessageResult.Append($"Task: {TaskName}");
            MessageResult.AppendLine();
            if (Service != null)
            {
                MessageResult.Append($"Service: {Service}");
                MessageResult.AppendLine();
            }
            if (Host != null)
            {
                MessageResult.Append($"Host: {Host}");
                MessageResult.AppendLine();
            }
            if (Adress != null)
            {
                MessageResult.Append($"Address: { Adress}");
                MessageResult.AppendLine();
            }
            if (State != null)
            {
                MessageResult.Append($"State: {State}");
                MessageResult.AppendLine();
            }
            MessageResult.Append($"Time: {Time}");
            MessageResult.AppendLine();
            MessageResult.Append($"Msg: {msg}");
            return MessageResult.ToString();
        }
    }
}
