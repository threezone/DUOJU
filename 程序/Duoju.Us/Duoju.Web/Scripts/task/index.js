$(document).ready(function () {
    var __pageWidth = $(window).width();
    if (__pageWidth < 600) {
        $('.container-fluid').attr('style', 'padding:0;');
        $('.container-fluid .row-fluid:first-child').css('margin-top', '0');
        $('.widget-box').attr('style', 'margin-top: 0;');
        $('.widget-content').attr('style', 'padding:15px 0;');
    }
    
    $('input[type=checkbox],input[type=radio],input[type=file]').uniform();
    $('select').select2();

    maruti.init();

    $('#basic_validate').ajaxForm({
        dataType: 'json',
        success: function (data) {
            $('#modal-add-event').modal('hide');
            $('#myModal').modal();
            var _ev = ycf_async.handlerTask(data);
            ycf_async.__aysncTaskList.push(_ev);
            maruti.add_event(_ev);
            $('#basic_validate').clearForm();
        }
    });

    var checkin = $('#startDate').datetimepicker({ format: 'yyyy-mm-dd hh:ii',
        autoclose: true
    })
    .on('changeDate', function (ev) {
        var newDate = new Date(ev.date)
        newDate.setDate(newDate.getDate() - 1);
        $('#endDate').datetimepicker('setStartDate', newDate);
        $('#endDate')[0].focus();
    });

    var checkout = $('#endDate').datetimepicker({
        format: 'yyyy-mm-dd hh:ii',
        autoclose: true,
        onRender: function (date) {
            return date.valueOf() <= checkin.date.valueOf() ? 'disabled' : '';
        }
    });
});

maruti = {
    init: function () {
        var date = Date.today();
        $('#fullcalendar').fullCalendar({
            header: {
                left: 'prev,next',
                center: 'title',
                right: 'month,basicWeek,basicDay'
            },
            monthNames: ['一月', '二月', '三月', '四月', '五月', '六月', '七月', '八月', '九月', '十月', '十一月', '十二月'],
            dayNamesShort: ['周日', '周一', '周二', '周三', '周四', '周五', '周六'],
            monthNamesShort: ["一月", "二月", "三月", "四月", "五月", "六月", "七月", "八月", "九月", "十月", "十一月", "十二月"],
            dayNames: ["周日", "周一", "周二", "周三", "周四", "周五", "周六"],
            titleFormat: {
                month: 'yyyy年MM月' ,
                week: 'yyyy年MM月dd日{ &#8212; yyyy年MM月dd日}',
                day: 'yyyy年MM月dd日, dddd'
            },
            buttonText: {
                today: '本月',
                month: '月',
                week: '周',
                day: '日',
                prev: '上一月',
                next: '下一月'
            },  
            editable: false,
            droppable: false,
            dayClick: function (date, allDay, jsEvent, view) {
                var now = Date.today().setTimeToNow();
                var cDate = date.addHours(now.getHours()).addMinutes(now.getMinutes() + (5-now.getMinutes()%5));
                var dateStr = cDate.toString('yyyy-MM-dd HH:mm');
                $('#startDate').attr('data-date', dateStr);
                $('#StartTime').attr('value', dateStr);
                $('#modal-add-event').modal();
            },
            eventClick: function (event, jsEvent, view) {
                $('#lbTaskTitle').html('<strong>' + event.title + '</strong> ');
                $('#spanSupplierInfo').html("<strong>酒店/客户名：</strong>" + event.supplierName + "&nbsp;&nbsp;&nbsp;&nbsp;<strong>联系电话：</strong>" + event.telePhone);
                $('#spanTimeDiff').html('开始: ' + event.start.toString("yyyy.MM.dd hh:mm") + '&nbsp;&nbsp;&nbsp;&nbsp;结束:' + (event.end == null ? event.start.toString("yyyy.MM.dd hh:mm") : event.end.toString("yyyy.MM.dd hh:mm")));
                $('#lbTaskContent').html(event.content);
                $('#currentTaskId').val(event.id);
                $('#currentSupplierId').val(event.supplierId);
                if (event.taskState == 1) {
                    $('#FlagTask').removeAttr('style');
                    $('#CancelTask').find('i').removeClass('icon-ban-circle');
                    $('#CancelTask').find('i').addClass('icon-remove');
                    $('#CancelTask').find('i').removeAttr('style');
                    ycf_crm.readTask(event.id, 2, null, function (d) {
                        event.className = 'fc-event-skin-flag';
                        $('#fullcalendar').fullCalendar('updateEvent', event);
                        ycf_async.updateEventStateById(event.id, 2);
                    }, function (e) {
                        console.log(e);
                    });
                } else if (event.taskState == 3) {
                    $('#FlagTask').attr('style', 'color:#f74d4d;');
                    $('#CancelTask').find('i').removeClass('icon-ban-circle');
                    $('#CancelTask').find('i').addClass('icon-remove');
                    $('#CancelTask').find('i').removeAttr('style');
                } else if (event.taskState == 4) {
                    $('#CancelTask').find('i').removeClass('icon-remove');
                    $('#CancelTask').find('i').addClass('icon-ban-circle');
                    $('#CancelTask').find('i').attr('style', 'color:#f74d4d;');
                    $('#FlagTask').removeAttr('style');
                } else {
                    $('#CancelTask').find('i').removeClass('icon-ban-circle');
                    $('#CancelTask').find('i').addClass('icon-remove');
                    $('#FlagTask').removeAttr('style');
                    $('#CancelTask').find('i').removeAttr('style');
                }
                $('#modal-showTask').modal();
            }
        });
        this.external_events();

        //如果队列中存在任务，则不重新向后台获取
        if (!!ycf_async.__aysncTaskList && ycf_async.__aysncTaskList.length <= 0) {
            ycf_crm.getTaskList(null, function (data) {
                if (!!ycf_async.__aysncTaskList && ycf_async.__aysncTaskList.length <= 0) {
                    if (data && data.length > 0) {
                        $.each(data, function (i) {
                            ycf_async.__aysncTaskList.push(ycf_async.handlerTask(data[i]));
                        });
                        $('#fullcalendar').fullCalendar('addEventSource', ycf_async.__aysncTaskList, true);
                    }
                } else {
                    $('#fullcalendar').fullCalendar('addEventSource', ycf_async.__aysncTaskList, true);
                }
            }, function (e) {
                console.log(e);
            });
        } else {
            $('#fullcalendar').fullCalendar('addEventSource', ycf_async.__aysncTaskList, true);
        }
    },
    add_event: function (event) {
        if (event) {
            $('#fullcalendar').fullCalendar('renderEvent', event, true);
        } else {
            this.show_error();
        }
    },
    external_events: function () {
        $('#external-ycf_async.__aysncTaskList div.external-event').each(function () {
            var eventObject = {
                title: $.trim($(this).text())
            };
            $(this).data('eventObject', eventObject);
            $(this).draggable({
                zIndex: 999,
                revert: true,
                revertDuration: 0
            });
        });
    },
    show_error: function () {
        $('#modal-error').remove();
        $('<div style="border-radius: 5px; top: 70px; font-size:14px; left: 50%; margin-left: -70px; position: absolute;width: 140px; background-color: #f00; text-align: center; padding: 5px; color: #ffffff;" id="modal-error">Enter event name!</div>').appendTo('#modal-add-event .modal-body');
        $('#modal-error').delay('1500').fadeOut(700, function () {
            $(this).remove();
        });
    }
};
