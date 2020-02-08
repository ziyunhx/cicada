using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cicada.Core.Helper;
using Cicada.Core.Models;
using Cicada.EFCore.Shared.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace Cicada.Core.Cache
{
    /// <summary>
    /// Cicada 任务内存缓存类
    /// </summary>
    public class MemoryCicadaCache : ICicadaCache
    {
        //存储任务列表
        private static ConcurrentDictionary<string, TaskInfo> _tasks = new ConcurrentDictionary<string, TaskInfo>();

        //存储任务数据
        private static IMemoryCache memoryCache = new MemoryCache(Options.Create(new MemoryCacheOptions()));

        //存储待执行的任务信息
        private static SortedSet<NextRunTask> _needRunTasks = new SortedSet<NextRunTask>();

        private static object _runTaskLock = new object();

        //存储执行中的任务信息
        private static ConcurrentDictionary<string, List<TaskCheckModel>> _runningTasks = new ConcurrentDictionary<string, List<TaskCheckModel>>();

        public bool AddTask(TaskInfo task)
        {
            if (task.NextRunTime == null)
            {
                task.NextRunTime = TaskHelper.CalNextRunTime(task);
            }

            _tasks[task.TaskId] = task;

            lock(_runTaskLock)
            {
                int removeCount = _needRunTasks.RemoveWhere(f => f.TaskId == task.TaskId);
                NextRunTask taskStatus = new NextRunTask();
                taskStatus.TaskId = task.TaskId;
                taskStatus.NextRunTime = task.NextRunTime ?? DateTime.Now;

                _needRunTasks.Add(taskStatus);
            }
            return true;
        }

        public bool AddTasks(IEnumerable<TaskInfo> tasks)
        {
            lock (_runTaskLock)
            {
                foreach (var task in tasks)
                {
                    _tasks[task.TaskId] = task;

                    if (task.NextRunTime == null)
                    {
                        task.NextRunTime = TaskHelper.CalNextRunTime(task);
                    }

                    int removeCount = _needRunTasks.RemoveWhere(f => f.TaskId == task.TaskId);
                    NextRunTask taskStatus = new NextRunTask();
                    taskStatus.TaskId = task.TaskId;
                    taskStatus.NextRunTime = task.NextRunTime ?? DateTime.Now;

                    _needRunTasks.Add(taskStatus);
                }
            }
            return true;
        }

        public TaskInfo GetTask(string clientId)
        {
            lock (_runTaskLock)
            {
                do
                {
                    var _task = _needRunTasks.TakeOne(f => f.NextRunTime <= DateTime.Now);
                    if (_task != null && !string.IsNullOrWhiteSpace(_task.TaskId))
                    {
                        var _taskInfo = _tasks[_task.TaskId];

                        if (_taskInfo != null)
                        {
                            //判断当前任务是否执行超过最大正在执行数，部分任务在一台机器仅允许单个实例的也需要判断
                            var runningChecks = _runningTasks[_taskInfo.TaskId];
                            if (runningChecks != null)
                            {
                                if (runningChecks.Count >= _taskInfo.MaxRuningThread)
                                {
                                    continue;
                                }
                                
                                if (_taskInfo.MaxThreadInOneClient > 0 && runningChecks.Count(f => f.ClientId == clientId) >= _taskInfo.MaxThreadInOneClient)
                                {
                                    continue;
                                }
                            }

                            return _taskInfo;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
                while (true);
            }

            return null;
        }

        public Dictionary<string, object> GetTaskCacheData(string checkId)
        {
            Dictionary<string, object> cacheDatas = new Dictionary<string, object>();
            memoryCache.TryGetValue<Dictionary<string, object>>("cache_" + checkId, out cacheDatas);

            return cacheDatas;
        }

        public List<TaskInfo> GetTasks(string clientId, int count)
        {
            throw new NotImplementedException();
        }

        public bool RemoveTask(string taskId)
        {
            throw new NotImplementedException();
        }

        public void SetTaskCache(string checkId, Dictionary<string, object> data, TimeSpan? timeSpan = null)
        {
            throw new NotImplementedException();
        }

        public void SetTaskStatus(string taskId, string checkId, int status, string processId = "")
        {
            throw new NotImplementedException();
        }

        public int TaskExecutingCount(string taskId)
        {
            throw new NotImplementedException();
        }
    }
}
