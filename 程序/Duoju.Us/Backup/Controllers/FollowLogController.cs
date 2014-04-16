using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YCF.CRM.Models;
using YCF.CRM.Model.Entity;
using YCF.CRM.DAO.Abstract;
using YCF.CRM.DAO.Utilities;

namespace YCF.CRM.Controllers
{
    public class FollowLogController : BaseController
    {
        private ISupplierInfoRepository SpSupplierInfoRepository { get; set; }
        private IFollowLogRepository SpFollowLogRepository { get; set; }
        private IFollowStateRepository SpFollowStateRepository { get; set; }
        private ITaskRepository SpTaskRepository { get; set; }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult FollowLogGroup()
        {
            return View();
        }

        public ActionResult FollowLogDetial(string sid, string lid)
        {
            FollowLogModel fModel = new FollowLogModel();
            int supId = 0;
            int logId = 0;

            if (int.TryParse(lid, out logId))
            {
                fModel.FLogInfo = SpFollowLogRepository.GetFollowLogByLogId(logId);
                if (fModel.FLogInfo == null)
                    fModel.FLogInfo = new FollowLog();
            }

            if (int.TryParse(sid, out supId))
            {
                fModel.SupInfo = SpSupplierInfoRepository.GetSupplierBySupplierId(supId);
            }
            else
            {
                fModel.SupInfo = fModel.FLogInfo.SupplierInfo;
            }
            fModel.FStateList = SpFollowStateRepository.GetFollowStateList();
            return View(fModel);
        }

        [HttpPost]
        public ActionResult FollowLogCollect(string start, string end, string userId, string province, string city, string district)
        {
            IList<FollowLog> followLogCollect = new List<FollowLog>();
            if (!string.IsNullOrEmpty(start) && !string.IsNullOrEmpty(end))
            {
                if (string.IsNullOrEmpty(userId))
                {
                    if (!string.IsNullOrEmpty(province))
                    {
                        if (string.IsNullOrEmpty(city))
                        {
                            followLogCollect = SpFollowLogRepository.GetFollowLogByGroup(Convert.ToDateTime(start), Convert.ToDateTime(end), null, Convert.ToInt32(province), null, null);
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(district))
                            {
                                followLogCollect = SpFollowLogRepository.GetFollowLogByGroup(Convert.ToDateTime(start), Convert.ToDateTime(end), null, Convert.ToInt32(province), Convert.ToInt32(city), null);
                            }
                            else
                            {
                                followLogCollect = SpFollowLogRepository.GetFollowLogByGroup(Convert.ToDateTime(start), Convert.ToDateTime(end), null, Convert.ToInt32(province), Convert.ToInt32(city), Convert.ToInt32(district));
                            }
                        }
                    }
                    else
                    {
                        followLogCollect = SpFollowLogRepository.GetFollowLogByGroup(Convert.ToDateTime(start), Convert.ToDateTime(end), null, null, null, null);
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(province))
                    {
                        if (string.IsNullOrEmpty(city))
                        {
                            followLogCollect = SpFollowLogRepository.GetFollowLogByGroup(Convert.ToDateTime(start), Convert.ToDateTime(end), Convert.ToInt32(userId), Convert.ToInt32(province), null, null);
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(district))
                            {
                                followLogCollect = SpFollowLogRepository.GetFollowLogByGroup(Convert.ToDateTime(start), Convert.ToDateTime(end), Convert.ToInt32(userId), Convert.ToInt32(province), Convert.ToInt32(city), null);
                            }
                            else
                            {
                                followLogCollect = SpFollowLogRepository.GetFollowLogByGroup(Convert.ToDateTime(start), Convert.ToDateTime(end), Convert.ToInt32(userId), Convert.ToInt32(province), Convert.ToInt32(city), Convert.ToInt32(district));
                            }
                        }
                    }
                    else
                    {
                        followLogCollect = SpFollowLogRepository.GetFollowLogByGroup(Convert.ToDateTime(start), Convert.ToDateTime(end), Convert.ToInt32(userId), null, null, null);
                    }
                }
            }
            else if (!string.IsNullOrEmpty(userId))
            {
                if (!string.IsNullOrEmpty(province))
                {
                    if (string.IsNullOrEmpty(city))
                    {
                        followLogCollect = SpFollowLogRepository.GetFollowLogByGroup(null, null, Convert.ToInt32(userId), Convert.ToInt32(province), null, null);
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(district))
                        {
                            followLogCollect = SpFollowLogRepository.GetFollowLogByGroup(null, null, Convert.ToInt32(userId), Convert.ToInt32(province), Convert.ToInt32(city), null);
                        }
                        else
                        {
                            followLogCollect = SpFollowLogRepository.GetFollowLogByGroup(null, null, Convert.ToInt32(userId), Convert.ToInt32(province), Convert.ToInt32(city), Convert.ToInt32(district));
                        }
                    }
                }
                else
                {
                    followLogCollect = SpFollowLogRepository.GetFollowLogByGroup(null, null, Convert.ToInt32(userId), null, null, null);
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(province))
                {
                    if (string.IsNullOrEmpty(city))
                    {
                        followLogCollect = SpFollowLogRepository.GetFollowLogByGroup(null, null, null, Convert.ToInt32(province), null, null);
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(district))
                        {
                            followLogCollect = SpFollowLogRepository.GetFollowLogByGroup(null, null, null, Convert.ToInt32(province), Convert.ToInt32(city), null);
                        }
                        else
                        {
                            followLogCollect = SpFollowLogRepository.GetFollowLogByGroup(null, null, null, Convert.ToInt32(province), Convert.ToInt32(city), Convert.ToInt32(district));
                        }
                    }
                }
                else
                {
                    followLogCollect = SpFollowLogRepository.GetFollowLogByGroup(null, null, null, null, null, null);
                }
            }
            GridModel<IList<FollowLog>> grid = new GridModel<IList<FollowLog>>();
            grid.rows = followLogCollect;
            grid.total = followLogCollect.Count();
            var jsonStr = JSONUtility.ToJson(grid, true);
            return Content(jsonStr);
        }

        [HttpPost]
        public ActionResult GetFollowState()
        {
            var stateList = SpFollowStateRepository.GetFollowStateList();
            return Content(JSONUtility.ToJson(stateList, true));
        }

        [HttpPost]
        public ActionResult SaveOrUpdateFollowLog(FollowLog FLogInfo, SupplierInfo SupInfo)
        {
            FLogInfo.UserId = base.CurrentUserId;
            FLogInfo.CreateBy = base.CurrentUserId;
            FLogInfo.CreateDate = System.DateTime.Now;
            FLogInfo.SupplierId = SupInfo.SupplierId;

            MessageModel messageModel = new MessageModel();
            if (SpFollowLogRepository.SaveOrUpdateFollowLog(FLogInfo))
            {
                messageModel.State = "success";
                messageModel.Message = "Save Or Update Success";
            }
            else
            {
                messageModel.State = "false";
                messageModel.Message = "Save Or Update false";
            }
            var jsonStr = JSONUtility.ToJson(messageModel, true);
            return Content(jsonStr);
        }

        [HttpPost]
        public ActionResult AddFollowLogAndEditTask(int taskId, int supplierId, int? taskState, string followName, int? followState, string remark, string start, string end, string editTaskContent)
        {
            MessageModel messageModel = new MessageModel();
            try
            {
                var task = SpTaskRepository.GetTaskByTaskId(taskId);
                if (taskState.HasValue && taskState.Value != 0)
                {
                    task.TaskState = taskState;
                }
                if (!string.IsNullOrEmpty(start))
                {
                    task.StartTime = Convert.ToDateTime(start);
                }
                if (!string.IsNullOrEmpty(end))
                {
                    task.EndTime = Convert.ToDateTime(end);
                }
                task.Content = editTaskContent;
                SpTaskRepository.UpdateTask(task);
                if (followState.HasValue && followState.Value != 0)
                {
                    var followLog = new FollowLog();
                    followLog.SupplierId = supplierId;
                    followLog.FollowName = followName;
                    followLog.FollowState = followState;
                    followLog.Remark = remark;
                    followLog.UserId = task.FromId;
                    followLog.CreateDate = System.DateTime.Now;
                    followLog.CreateBy = task.FromId;
                    SpFollowLogRepository.SaveOrUpdateFollowLog(followLog);
                }
                messageModel.Message = "Success";
                messageModel.State = "success";
            }
            catch (Exception ex)
            {
                messageModel.Message = "Failed";
                messageModel.State = "false";
            }
            var jsonStr = JSONUtility.ToJson(messageModel, true);
            return Content(jsonStr);
        }

        [HttpPost]
        public ActionResult GetFollowList(int page, int rows)
        {
            int total = 0;
            var followLogList = SpFollowLogRepository.GetFollowLogListByUserId(base.CurrentUserId, (page - 1) * rows, rows, out total);
            GridModel<IList<FollowLog>> grid = new GridModel<IList<FollowLog>>();
            grid.rows = followLogList;
            grid.total = total;
            var jsonStr = JSONUtility.ToJson(grid, true);
            return Content(jsonStr);
        }

        [HttpPost]
        public ActionResult SearchFollowLog(string startDate, string endDate, string keyWord, int page, int rows)
        {
            int total = 0;
            IList<FollowLog> followLogList = new List<FollowLog>();
            if (!string.IsNullOrEmpty(startDate) && !string.IsNullOrEmpty(endDate))
            {
                followLogList = SpFollowLogRepository.SearchFollowLog(base.CurrentUserId, Convert.ToDateTime(startDate), Convert.ToDateTime(endDate), keyWord, (page - 1) * rows, rows, out total);
            }
            else
            {
                followLogList = SpFollowLogRepository.SearchFollowLog(base.CurrentUserId, null, null, keyWord, (page - 1) * rows, rows, out total);
            }
            GridModel<IList<FollowLog>> grid = new GridModel<IList<FollowLog>>();
            grid.rows = followLogList;
            grid.total = total;
            var jsonStr = JSONUtility.ToJson(grid, true);
            return Content(jsonStr);
        }

        [HttpPost]
        public ActionResult GetFollowLogListBySIdAndUId(int sid, int page, int rows)
        {
            int total = 0;
            IList<FollowLog> followLogList = new List<FollowLog>();
            followLogList = SpFollowLogRepository.GetFollowLogListBySIdAndUId(base.CurrentUserId, sid, (page - 1) * rows, rows, out total);
            GridModel<IList<FollowLog>> grid = new GridModel<IList<FollowLog>>();
            grid.rows = followLogList;
            grid.total = total;
            var jsonStr = JSONUtility.ToJson(grid, true);
            return Content(jsonStr);
        }
    }
}
