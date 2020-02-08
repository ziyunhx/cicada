using Cicada.Core.Helper;
using Cicada.Core.Models;
using Cicada.EFCore.Shared.Constants;
using Cicada.EFCore.Shared.DBContexts;
using Cicada.EFCore.Shared.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cicada.Services
{
    public class TaskService : ITaskService
    {
        private readonly ILogger<TaskService> _logger;
        private readonly CicadaDbContext _cicadaDbContext;

        public TaskService(ILogger<TaskService> logger, CicadaDbContext cicadaDbContext)
        {
            _logger = logger;
            _cicadaDbContext = cicadaDbContext;
        }

        /// <summary>
        /// 增加或更新一个任务
        /// </summary>
        /// <param name="taskInfo"></param>
        /// <returns></returns>
        public ResultModel AddOrUpdateTask(TaskInfo taskInfo)
        {
            ResultModel result = new ResultModel();
            try
            {
                if (string.IsNullOrEmpty(taskInfo.TaskId))
                    taskInfo.TaskId = Guid.NewGuid().ToString("N");//如果没有ID自动生成
                taskInfo.NextRunTime = TaskHelper.CalNextRunTime(taskInfo);//如果时间数据定义不合理会抛出异常并返回消息

                if (string.IsNullOrWhiteSpace(taskInfo.Command))//检查命令是否存在
                {
                    result.Code = 404;
                    result.Msg = "The task command is null!";
                }
                else
                {
                    _cicadaDbContext.TaskInfos.AddOrUpdate(taskInfo);

                    if (taskInfo.Notices != null && taskInfo.Notices.Count > 0)
                    {
                        foreach (Notice notice in taskInfo.Notices)
                        {
                            notice.TaskInfo = taskInfo;

                            if (string.IsNullOrEmpty(notice.NoticeId))
                                notice.NoticeId = Guid.NewGuid().ToString("N");

                            _cicadaDbContext.Notices.AddOrUpdate(notice);
                        }
                    }

                    _cicadaDbContext.SaveChanges();
                    result.Code = 200;
                    result.Msg = taskInfo.TaskId;
                }
            }
            catch (Exception ex)
            {
                result.Code = 500;
                result.Msg = ex.ToString();
            }
            return result;
        }

        /// <summary>
        /// 增加或更新通知
        /// </summary>
        /// <param name="notice"></param>
        /// <returns></returns>
        public ResultModel AddOrUpdateNotice(Notice notice)
        {
            ResultModel result = new ResultModel();
            try
            {
                if (string.IsNullOrWhiteSpace(notice.NoticeId))
                {
                    notice.NoticeId = Md5Helper.getMd5Hash(notice.TaskId);//如果没有ID自动生成
                }

                _cicadaDbContext.Notices.AddOrUpdate(notice);
                _cicadaDbContext.SaveChanges();

                result.Code = 200;
                result.Msg = notice.NoticeId;
            }
            catch (Exception ex)
            {
                result.Code = 500;
                result.Msg = ex.Message;
            }
            return result;
        }

        /// <summary>
        /// 获取任务列表
        /// </summary>
        /// <param name="count"></param>
        /// <param name="skip"></param>
        /// <returns></returns>
        public ResultModel<TaskInfo[]> GetTasks(int count = -1, int skip = 0, string owner = null, string name = null)
        {
            //todo 实现获取指定条数
            ResultModel<TaskInfo[]> result = new ResultModel<TaskInfo[]>();
            IQueryable<TaskInfo> TaskInfoList;
            try
            {
                TaskInfoList = _cicadaDbContext.TaskInfos.OrderBy(f => f.TaskId);
                if (!string.IsNullOrEmpty(name))
                {
                    TaskInfoList = TaskInfoList.Where(i => i.Name == name);
                }
                if (!string.IsNullOrEmpty(owner))
                {
                    TaskInfoList = TaskInfoList.Where(i => i.Owner == owner);
                }
                if (count == -1)
                {
                    TaskInfoList = TaskInfoList.Skip(skip);
                }
                else
                {
                    TaskInfoList = TaskInfoList.Take(count).Skip(skip);
                }
                result.Data = TaskInfoList.ToArray();
                result.Code = 200;

            }
            catch (Exception ex)
            {
                result.Code = 500;
                result.Msg = ex.Message;
            }
            return result;
        }

        /// <summary>
        /// 获取单个任务的通知信息
        /// </summary>
        /// <param name="taskId">任务编号</param>
        /// <returns></returns>
        public ResultModel<Notice[]> GetNotices(string taskId)
        {
            ResultModel<Notice[]> result = new ResultModel<Notice[]>();
            try
            {
                if (!string.IsNullOrWhiteSpace(taskId))
                {
                    result.Data = _cicadaDbContext.Notices.Where(f => f.TaskId == taskId).ToArray();

                    result.Code = 200;
                }
            }
            catch (Exception ex)
            {
                result.Code = 500;
                result.Msg = ex.Message;
            }
            return result;
        }

        /// <summary>
        /// 获取单个任务
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        public ResultModel<TaskInfo> GetTask(string taskId)
        {
            ResultModel<TaskInfo> result = new ResultModel<TaskInfo>();
            try
            {
                TaskInfo taskInfo = _cicadaDbContext.TaskInfos.Where(i => i.TaskId == taskId).FirstOrDefault();
                //taskInfo.Notices = entity.Notices.Where(f => f.Task.TaskId == taskId).ToList();
                result.Data = taskInfo;
                result.Code = 200;

            }
            catch (Exception ex)
            {
                result.Code = 500;
                result.Msg = ex.Message;
            }
            return result;
        }

        /// <summary>
        /// 删除单个任务
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        public ResultModel DeleteTask(string taskId)
        {
            ResultModel result = new ResultModel();
            try
            {
                #region 删除TASKINFO记录
                var _data = _cicadaDbContext.TaskInfos.Where(i => i.TaskId == taskId).FirstOrDefault();
                if (_data == null)
                {
                    result.Code = 404;
                    result.Msg = "No Record";
                }
                else
                {
                    _cicadaDbContext.TaskInfos.Remove(_data);
                    result.Code = 200;
                }
                #endregion
                #region 删除notice记录
                var _notice = _cicadaDbContext.Notices.Where(i => i.TaskId == taskId).ToArray();
                if (_notice.Count() > 0)
                {
                    _cicadaDbContext.Notices.RemoveRange(_notice);
                }
                #endregion
                _cicadaDbContext.SaveChanges();

                //return true;
            }
            catch (Exception ex)
            {
                result.Code = 500;
                result.Msg = ex.Message;
            }
            return result;
        }

        /// <summary>
        /// 启动一个任务
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="nextRunTime"></param>
        /// <returns></returns>
        public ResultModel StartTask(string taskId, long nextRunTime = 0)
        {
            ResultModel result = new ResultModel();
            try
            {
                var _data = _cicadaDbContext.TaskInfos.Where(i => i.TaskId == taskId).FirstOrDefault();
                if (_data == null)
                {
                    result.Code = 404;
                    result.Msg = "No Record";
                }
                else if (_data.Status == 0)
                {
                    result.Code = 501;
                    result.Msg = "This task is already start.";
                }
                else
                {
                    result.Code = 200;
                    if (nextRunTime != 0)
                    {
                        _data.NextRunTime = DateTimeHelper.GetTime(nextRunTime);
                    }
                    else
                    {
                        _data.NextRunTime = TaskHelper.CalNextRunTime(_data);
                    }

                    _data.Status = 0;
                    _cicadaDbContext.SaveChanges();
                }

            }
            catch (Exception ex)
            {
                result.Code = 500;
                result.Msg = ex.Message;
            }
            return result;
        }

        /// <summary>
        /// 停止一个任务
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        public ResultModel StopTask(string taskId)
        {
            ResultModel result = new ResultModel();
            try
            {
                var _data = _cicadaDbContext.TaskInfos.Where(i => i.TaskId == taskId).FirstOrDefault();
                if (_data == null)
                {
                    result.Code = 404;
                    result.Msg = "No Record";
                }
                else if (_data.Status == 1)
                {
                    result.Code = 501;
                    result.Msg = "This task is already stoped.";
                }
                else
                {
                    result.Code = 200;
                    _data.Status = 1;
                    _cicadaDbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                result.Code = 500;
                result.Msg = ex.Message;
            }
            return result;
        }

        /// <summary>
        /// 获取任务执行结果
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="fromTime"></param>
        /// <param name="endTime"></param>
        /// <param name="count"></param>
        /// <param name="skip"></param>
        /// <returns></returns>
        public ResultModel<CheckResult[]> GetCheckResult(string taskId, long fromTime = 0, long endTime = 0, int count = -1, int skip = 0)
        {
            ResultModel<CheckResult[]> result = new ResultModel<CheckResult[]>();
            try
            {
                List<CheckResult> CRList = new List<CheckResult>();
                var _data = _cicadaDbContext.CheckResults.Where(i => i.TaskId == taskId);
                if (fromTime != 0)
                {
                    DateTime dtFromTime = DateTimeHelper.GetTime(fromTime);
                    _data = _data.Where(i => i.EndTime > dtFromTime);
                }
                if (endTime != 0)
                {
                    DateTime dtEndTime = DateTimeHelper.GetTime(endTime);
                    _data = _data.Where(i => i.EndTime < dtEndTime);
                }
                if (count != -1)
                {
                    _data = _data.Take(count);
                }
                _data = _data.Skip(skip);
                CRList = _data.ToList();
                result.Data = CRList.ToArray();
                result.Code = 200;

            }
            catch (Exception ex)
            {
                result.Code = 500;
                result.Msg = ex.Message;
            }

            return result;
        }
    }
}