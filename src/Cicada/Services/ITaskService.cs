using Cicada.Core.Models;
using Cicada.EFCore.Shared.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cicada.Services
{
    public interface ITaskService
    {
        ResultModel AddOrUpdateTask(TaskInfo taskInfo);
        ResultModel AddOrUpdateNotice(Notice notice);
        ResultModel<TaskInfo[]> GetTasks(int count = -1, int skip = 0, string owner = null, string name = null);
        ResultModel<Notice[]> GetNotices(string taskId);
        ResultModel<TaskInfo> GetTask(string taskId);
        ResultModel DeleteTask(string taskId);
        ResultModel StartTask(string taskId, long nextRunTime = 0);
        ResultModel StopTask(string taskId);
        ResultModel<CheckResult[]> GetCheckResult(string taskId, long fromTime = 0, long endTime = 0, int count = -1, int skip = 0);
    }
}
