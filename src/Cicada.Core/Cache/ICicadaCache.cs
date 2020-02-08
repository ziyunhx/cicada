using Cicada.EFCore.Shared.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cicada.Core.Cache
{
    /// <summary>
    /// Cicada 缓存接口
    /// </summary>
    public interface ICicadaCache
    {
        /// <summary>
        /// 增加需要执行的任务
        /// </summary>
        /// <param name="task">任务信息</param>
        /// <returns>是否成功</returns>
        bool AddTask(TaskInfo task);
        /// <summary>
        /// 从缓存中移除一个任务
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        bool RemoveTask(string taskId);
        /// <summary>
        /// 缓存某次任务执行中的数据
        /// </summary>
        /// <param name="checkId">执行编号</param>
        /// <param name="data">数据</param>
        /// <param name="timeSpan">缓存周期</param>
        void SetTaskCache(string checkId, Dictionary<string, object> data, TimeSpan? timeSpan = null);
        /// <summary>
        /// 获取任务执行中的缓存数据，有可能为nukk
        /// </summary>
        /// <param name="checkId">执行编号</param>
        /// <returns></returns>
        Dictionary<string, object> GetTaskCacheData(string checkId);
        /// <summary>
        /// 增加一批需要执行的任务
        /// </summary>
        /// <param name="tasks">任务信息集合</param>
        /// <returns>是否成功</returns>
        bool AddTasks(IEnumerable<TaskInfo> tasks);
        /// <summary>
        /// 任务执行中的数量
        /// </summary>
        /// <param name="TaskId">任务编号</param>
        /// <returns>执行中的进程数</returns>
        int TaskExecutingCount(string taskId);
        /// <summary>
        /// 获取一个Task执行，该任务将被标记为执行中，直到任务
        /// </summary>
        /// <returns></returns>
        TaskInfo GetTask(string clientId);
        /// <summary>
        /// 获取指定数量的任务，这些任务不会被马上标记为执行中，但会锁定5s不被其它客户端获取；
        /// 拿到任务后需要立即更新状态确保不会被其它客户端再次获取到。
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        List<TaskInfo> GetTasks(string clientId, int count);
        /// <summary>
        /// 设置任务状态
        /// </summary>
        /// <param name="taskId">任务编号</param>
        /// <param name="checkId">执行编号</param>
        /// <param name="status">状态，0 等待执行；1 准备执行；2 执行中；3 执行成功；-1 执行失败</param>
        /// <param name="processId">进程ID，非必填</param>
        void SetTaskStatus(string taskId, string checkId, int status, string processId = "");
    }
}
