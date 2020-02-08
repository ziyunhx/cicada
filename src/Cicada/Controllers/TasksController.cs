using Cicada.EFCore.Shared.DBContexts;
using Cicada.EFCore.Shared.Models;
using Cicada.Data.Extensions;
using Cicada.Services;
using Cicada.ViewModels.Common;
using Cicada.ViewModels.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Cicada.Controllers
{
    [Authorize]
    public class TasksController : Controller
    {
        private readonly CicadaDbContext _context;
        private readonly ITaskService _taskService;

        public TasksController(CicadaDbContext context, ITaskService taskService)
        {
            _context = context;
            _taskService = taskService;
        }

        // GET: Tasks
        public async Task<IActionResult> Index(int page = 1, string search = null, int pageSize = 10)
        {
            var pagedList = new PagedList<TaskInfo>();
            Expression<Func<TaskInfo, bool>> searchCondition = x => x.Name.Contains(search) || x.TaskId.Contains(search) || x.Owner.Contains(search);

            var tasks = await _context.TaskInfos.WhereIf(!string.IsNullOrEmpty(search), searchCondition).PageBy(x => x.TaskId, page, pageSize).ToListAsync();

            pagedList.Data.AddRange(tasks);
            pagedList.TotalCount = await _context.TaskInfos.WhereIf(!string.IsNullOrEmpty(search), searchCondition).CountAsync();
            pagedList.PageSize = pageSize;

            return View(pagedList);
        }

        public async Task<IActionResult> Status(int page = 1, int pageSize = 10)
        {
            var pagedList = new PagedList<TaskStatusDto>();
            var tasks = await (from task in _context.TaskInfos
                               let check = _context.CheckResults.Where(f => task.TaskId == f.TaskId).OrderByDescending(f => f.EndTime).FirstOrDefault()
                               orderby task.TaskId descending
                               select new TaskStatusDto
                               {
                                   TaskId = task.TaskId,
                                   TaskName = task.Name,
                                   Status = task.Status,
                                   LastCheckStatus = check == null ? 0 : check.Status,
                                   LastCheckDateTime = check != null ? (check.EndTime ?? DateTime.MinValue) : DateTime.MinValue,
                                   CostMillisecond = check == null ? 0 : check.CostMillisecond,
                                   MessageLevel = check == null ? 0 : check.MessageLevel,
                                   MessageInfo = check == null ? "" : check.MessageInfo
                               }).PageBy(x => x.TaskId, page, pageSize).ToListAsync();

            pagedList.Data.AddRange(tasks);
            pagedList.TotalCount = await _context.TaskInfos.CountAsync();
            pagedList.PageSize = pageSize;

            return View(pagedList);
        }

        public async Task<IActionResult> Notications(int page = 1, int pageSize = 10)
        {
            var pagedList = new PagedList<TaskNoticationDto>();

            var tasks = await
                (from nr in _context.NotifyRecodes
                 where nr.CreateTime >= DateTime.Now.AddMonths(-3)
                 join task in _context.TaskInfos on nr.TaskId equals task.TaskId
                 join ck in _context.CheckResults on nr.CheckId equals ck.CheckId
                 select new TaskNoticationDto()
                 {
                     TaskId = nr.TaskId,
                     CreateTime = nr.CreateTime,
                     Status = nr.Status,
                     CheckStatus = ck.Status,
                     MessageInfo = ck.MessageInfo,
                     MessageLevel = ck.MessageLevel,
                     NotifyRecodeId = nr.NotifyRecodeId,
                     ReplyMsg = nr.ReplyMsg,
                     ReplyTime = nr.ReplyTime,
                     TaskName = task.Name
                 }).PageBy(x => x.CreateTime, page, pageSize).ToListAsync();

            pagedList.Data.AddRange(tasks);
            pagedList.TotalCount = await _context.NotifyRecodes.Where(f => f.CreateTime >= DateTime.Now.AddMonths(-3)).CountAsync();
            pagedList.PageSize = pageSize;

            return View(pagedList);
        }

        // GET: Tasks/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Tasks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TaskId,Name,Command,Params,Type,Owner,Disabled,Schedule,Retries,Timeout,Epsilon,EndTime,WorkDays,SleepDays,WorkTimes,SleepTimes")] TaskInfo taskInfo)
        {
            if (ModelState.IsValid)
            {
                _taskService.AddOrUpdateTask(taskInfo);

                return RedirectToAction(nameof(Index));
            }
            return View(taskInfo);
        }

        // GET: Tasks/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taskInfo = await _context.TaskInfos.FindAsync(id);
            if (taskInfo == null)
            {
                return NotFound();
            }
            return View(taskInfo);
        }

        // POST: Tasks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("TaskId,Name,Command,Params,Type,Owner,Disabled,Schedule,Retries,Timeout,Epsilon,EndTime,WorkDays,SleepDays,WorkTimes,SleepTimes,LastCheckStatus,NextRunTime")] TaskInfo taskInfo)
        {
            if (id != taskInfo.TaskId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _taskService.AddOrUpdateTask(taskInfo);
                return RedirectToAction(nameof(Index));
            }
            return View(taskInfo);
        }

        // GET: Tasks/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taskInfo = await _context.TaskInfos
                .FirstOrDefaultAsync(m => m.TaskId == id);
            if (taskInfo == null)
            {
                return NotFound();
            }

            return View(taskInfo);
        }

        // POST: Tasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var taskInfo = await _context.TaskInfos.FindAsync(id);
            _context.TaskInfos.Remove(taskInfo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TaskInfoExists(string id)
        {
            return _context.TaskInfos.Any(e => e.TaskId == id);
        }
    }
}
