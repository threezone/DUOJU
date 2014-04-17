//zhangzezhi,2013.09.18
//desc:用于异步定时检查是否有需要提示的任务队列，
//ycf_async.__aysncTaskList 为全局的任务队列

window.ycf_async = window.ycf_async || {};

//定义任务队列
ycf_async.__aysncTaskList = [];

//定义任务处理程序
ycf_async.handlerTask = function (obj) {
    var event = new Array();
    event.id = obj.TaskId;
    event.supplierId = obj.SupplierId;
    event.start = Date.parse(obj.StartTime);
    event.end = Date.parse(obj.EndTime);
    event.fromId = obj.FromId;
    event.receiverId = obj.ReceiverId;
    event.title = obj.Title;
    event.content = obj.Content;
    event.taskState = obj.TaskState;
    event.remindType = obj.RemindType;
    event.remindMins = obj.RemindMins;
    event.allDay = false;
    if (obj.Supplier) {
        event.supplierId = obj.Supplier.SupplierId;
        event.supplierName = obj.Supplier.SupplierName;
        event.telePhone = obj.Supplier.TelePhone;
    }
    if (obj.TaskStateInfo) {
        if (obj.TaskStateInfo.State == '重要') {
            event.className = 'fc-event-skin-important';
        } else if (obj.TaskStateInfo.State == '已阅') {
            event.className = 'fc-event-skin-flag';
        } else if (obj.TaskStateInfo.State == '已取消') {
            event.className = 'fc-event-skin-cancel';
        } else {
            event.className = 'fc-event-skin-notread';
        }
    } else {
        event.className = 'fc-event-skin-notread';
    }
    return event;
};

ycf_async.findEventById = function (eId) {
    var events = [];
    if (ycf_async.__aysncTaskList && ycf_async.__aysncTaskList.length > 0) {
        $.each(ycf_async.__aysncTaskList, function (i) {
            if (ycf_async.__aysncTaskList[i].id == eId) {
                events.push(ycf_async.__aysncTaskList[i]);
            }
        });
    }
    return events;
};

ycf_async.updateEventStateById = function (eId, state) {
    if (ycf_async.__aysncTaskList && ycf_async.__aysncTaskList.length > 0) {
        $.each(ycf_async.__aysncTaskList, function (i) {
            if (ycf_async.__aysncTaskList[i].id == eId) {
                ycf_async.__aysncTaskList[i].taskState = state;
            }
        });
    }
};

ycf_async.findRemindTask = function () {
    var asyncNow = Date.today().setTimeToNow();
    var asyncTask = [];
    var taskCount = 0;
    if (ycf_async.__aysncTaskList && ycf_async.__aysncTaskList.length > 0) {
        $.each(ycf_async.__aysncTaskList, function (i) {
            if (ycf_async.__aysncTaskList[i].end == null) {
                ycf_async.__aysncTaskList[i].end = ycf_async.__aysncTaskList[i].start;
            }
            if (Date.compare(asyncNow, Date.parse(ycf_async.__aysncTaskList[i].start)) >= 0 &&
                Date.compare(asyncNow, Date.parse(ycf_async.__aysncTaskList[i].end)) <= 0) {
                taskCount++;
                if ((((asyncNow.getTime() - Date.parse(ycf_async.__aysncTaskList[i].start).getTime()) / 60000) % ycf_async.__aysncTaskList[i].remindMins) <= 1 &&
                    ycf_async.__aysncTaskList[i].taskState != 4 && (ycf_async.__aysncTaskList[i].remindType % 2 == 1)) {
                    asyncTask.push(ycf_async.__aysncTaskList[i]);
                }
            }
        });
        if (taskCount > 0) {
            $('#MyTaskCount').show();
            $('#MyTaskCount').html(taskCount);
            $('#my_task_count').show();
            $('#my_task_count').html(taskCount);
        } else {
            $('#MyTaskCount').hide();
            $('#my_task_count').hide();
        }
    }
    return asyncTask;
};

//页面首次加载时，后期任务列表，并保存到队列中
if (!!ycf_async.__aysncTaskList && ycf_async.__aysncTaskList.length <= 0) {
    ycf_crm.getTaskList(null, function (data) {
        if (!!ycf_async.__aysncTaskList && ycf_async.__aysncTaskList.length <= 0) {
            if (data && data.length > 0) {
                $.each(data, function (i) {
                    ycf_async.__aysncTaskList.push(ycf_async.handlerTask(data[i]));
                });
            }
        }
    }, function (e) {
        console.log(e);
    });
};

//任务开始，启动异步定时器，每隔59秒(1分钟)轮询队列中是否有需要弹框提醒的任务
$(function () {
    var isFirstLoad = false;

    var taskHand = function () {
        var _firstLoad = isFirstLoad;
        var asyncTask = ycf_async.findRemindTask();
        if (asyncTask && asyncTask.length > 0) {
            if (_firstLoad) {
                $.each(asyncTask, function (i) {
                    $.gritter.add({
                        title: asyncTask[i].title,
                        text: '<a href="#modal-showTask" role="button" class="sp_gritter" data-toggle="modal" eId="' + asyncTask[i].id + '">' + asyncTask[i].content + '</a>',
                        time: 590000,
                        image: '../../Content/images/envelope.png',
                        sticky: false
                    });
                });
            }
            isFirstLoad = true;
        }
    }
    var taskHandAsync = eval(Wind.compile("async", function (interval) {
        while (true) {
            taskHand();
            if (isFirstLoad) {
                $await(Wind.Async.sleep(interval));

            } else {
                $await(Wind.Async.sleep(1000));
            }
        }
    }));
    taskHandAsync(59000).start();

});