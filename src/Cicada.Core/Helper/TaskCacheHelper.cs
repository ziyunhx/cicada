using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cicada.Core.Helper
{
    /// <summary>
    /// 缓存帮助类，用于控制任务执行过程中不会被重复启动，以及控制失败后的重试次数
    /// 在单节点中，使用系统缓存来控制，集群环境中，通过redis缓存
    /// </summary>
    public class TaskCacheHelper
    {
        private static IMemoryCache memoryCache = new MemoryCache(Options.Create(new MemoryCacheOptions()));

        public static bool IsRuning(string taskId)
        {
            string runStatus = memoryCache.Get<string>("runStatus_" + taskId);

            if (runStatus == "running")
            {
                return true;
            }
            else
            {
                memoryCache.Set("runStatus_" + taskId, "running");
                return false;
            }
        }

        public static void Stop(string taskId, bool isSucess)
        {
            if (isSucess)
            {
                memoryCache.Remove("runStatus_" + taskId);
            }
            else
            {
                memoryCache.Set("runStatus_" + taskId, "failed");
            }
        }

        public static bool RetryRunTask(string taskId, int maxTryCount)
        {
            string runStatus = memoryCache.Get<string>("runStatus_" + taskId);

            if (runStatus == "failed")
            {
                int runningCount = memoryCache.Get<int>(taskId);

                runningCount++;

                memoryCache.Set(taskId, runningCount);

                if (runningCount <= maxTryCount)
                {
                    return true;
                }
            }

            memoryCache.Set(taskId, 0);
            return false;
        }


    }
}
