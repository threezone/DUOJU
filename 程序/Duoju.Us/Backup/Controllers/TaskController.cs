using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YCF.CRM.Models;
using YCF.CRM.Model.Entity;
using YCF.CRM.Model.Exception;
using YCF.CRM.DAO.Abstract;
using YCF.CRM.DAO.Utilities;

namespace YCF.CRM.Controllers
{
    [Authorize]
    public class TaskController : BaseController
    {
        private ITaskRepository SpTaskRepository { get; set; }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SaveOrUpdateTask(TaskInfo model)
        {
            model.FromId = base.CurrentUserId;
            model.CreateBy = base.CurrentUserId;
            model.CreateDate = System.DateTime.Now;
            model.TaskState = 1;

            var jsonStr = string.Empty;
            
            var newId = SpTaskRepository.AddTask(model);
            if (newId > 0)
            {
                TaskInfo task = SpTaskRepository.GetTaskByTaskId(newId);
                jsonStr = JSONUtility.ToJson(task, true);
            }
            else
            {
                MessageModel messageModel = new MessageModel();
                messageModel.State = "false";
                messageModel.Message = "Save Or Update false";
                jsonStr = JSONUtility.ToJson(messageModel, true);
            }
            
            return Content(jsonStr);
        }

        [HttpPost]
        public ActionResult GetTaskByUserId()
        {
            var taskList = SpTaskRepository.GetTaskByUserId(base.CurrentUserId);
            return Content(JSONUtility.ToJson(taskList, true));
        }

        [HttpPost]
        public ActionResult ReadTask(int tid, int sid)
        {
            var task = SpTaskRepository.GetTaskByTaskId(tid);
            task.TaskState = sid;
            MessageModel messageModel = new MessageModel();
            if (SpTaskRepository.UpdateTask(task))
            {
                messageModel.State = "success";
                messageModel.Message = "Update success";
            }
            else
            {
                messageModel.State = "false";
                messageModel.Message = "Update false";
            }
            var jsonStr = JSONUtility.ToJson(messageModel, true);
            return Content(jsonStr);
        }

        [HttpPost]
        public ActionResult GetTaskBySidAndFromId(int sid)
        {
            var today = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            var taskList = SpTaskRepository.GetTaskBySidAndFromIdAndDateDiff(CurrentUserId, sid, null, new List<int> { 5 }, today, today);
            return Content(JSONUtility.ToJson(taskList, true));
        }
    }
}
