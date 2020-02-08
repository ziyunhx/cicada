using System;
using System.Collections.Generic;
using System.Text;
using Cicada.EFCore.Shared.Models;

namespace Cicada.Core.Cache
{
    public class RedisCicadaCache : ICicadaCache
    {
        public bool AddTask(TaskInfo task)
        {
            throw new NotImplementedException();
        }

        public bool AddTasks(IEnumerable<TaskInfo> tasks)
        {
            throw new NotImplementedException();
        }

        public TaskInfo GetTask(string clientId)
        {
            throw new NotImplementedException();
        }

        public Dictionary<string, object> GetTaskCacheData(string checkId)
        {
            throw new NotImplementedException();
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

        public int TaskExecutingCount(string TaskId)
        {
            throw new NotImplementedException();
        }
    }
}
