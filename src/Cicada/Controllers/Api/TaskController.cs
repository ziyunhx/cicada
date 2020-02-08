using Cicada.Core.Models;
using Cicada.EFCore.Shared.Models;
using Cicada.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Cicada.Controllers
{
    /// <summary>
    /// 任务管理接口
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    [EnableCors("CicadaApi")]
    public class TaskController : ControllerBase
    {
        private readonly ILogger<TasksController> _logger;
        private readonly ITaskService _taskService;

        public TaskController(ILogger<TasksController> logger, ITaskService taskService) : base()
        {
            _logger = logger;
            _taskService = taskService;
        }

        /// <summary>
        /// 新增或更新一个任务
        /// </summary>
        /// <param name="taskInfo">任务信息</param>
        /// <returns></returns>
        [HttpPost]
        public ResultModel AddOrUpdateTask(TaskInfo taskInfo)
        {
            return _taskService.AddOrUpdateTask(taskInfo);
        }

        /// <summary>
        /// 新增或更新一个通知
        /// </summary>
        /// <param name="notice">通知信息</param>
        /// <returns></returns>
        [HttpPost]
        public ResultModel AddOrUpdateNotice(Notice notice)
        {
            return _taskService.AddOrUpdateNotice(notice);
        }

        /// <summary>
        /// 获取符合条件的任务列表，按创建时间排序
        /// </summary>
        /// <param name="count">请求数量</param>
        /// <param name="skip">跳过数量</param>
        /// <param name="owner">所有者</param>
        /// <param name="name">名称</param>
        /// <returns></returns>
        [HttpGet]
        public ResultModel<TaskInfo[]> GetTasks(int count = -1, int skip = 0, string owner = null, string name = null)
        {
            return _taskService.GetTasks(count, skip, owner, name);
        }

        /// <summary>
        /// 获取单个任务的通知信息
        /// </summary>
        /// <param name="taskId">任务编号</param>
        /// <returns></returns>
        [HttpGet]
        public ResultModel<Notice[]> GetNotices(string taskId)
        {
            return _taskService.GetNotices(taskId);
        }

        /// <summary>
        /// 获取单个任务
        /// </summary>
        /// <param name="taskId">任务编号</param>
        /// <returns></returns>
        [HttpGet]
        public ResultModel<TaskInfo> GetTask(string taskId)
        {
            return _taskService.GetTask(taskId);
        }

        /// <summary>
        /// 删除单个任务
        /// </summary>
        /// <param name="taskId">人物编号</param>
        /// <returns></returns>
        [HttpDelete]
        public ResultModel DeleteTask(string taskId)
        {
            return _taskService.DeleteTask(taskId);
        }

        /// <summary>
        /// 启用一个任务
        /// </summary>
        /// <param name="taskId">任务编号</param>
        /// <param name="nextRunTime">下次执行时间的Unix时间戳，如果为0则其下次执行时间根据规则计算</param>
        /// <returns></returns>
        [HttpGet]
        public ResultModel StartTask(string taskId, long nextRunTime = 0)
        {
            return _taskService.StartTask(taskId, nextRunTime);
        }

        /// <summary>
        /// 停止一个任务
        /// </summary>
        /// <param name="taskId">任务编号</param>
        /// <returns></returns>
        [HttpGet]
        public ResultModel StopTask(string taskId)
        {
            return _taskService.StopTask(taskId);
        }

        /// <summary>
        /// 获取任务执行结果，结果按时间排序
        /// </summary>
        /// <param name="taskId">任务编号</param>
        /// <param name="fromTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="count">获取数量</param>
        /// <param name="skip">跳过数量</param>
        /// <returns></returns>
        [HttpGet]
        public ResultModel<CheckResult[]> GetCheckResult(string taskId, long fromTime = 0, long endTime = 0, int count = -1, int skip = 0)
        {
            return _taskService.GetCheckResult(taskId, fromTime, endTime, count, skip);
        }
    }

}
