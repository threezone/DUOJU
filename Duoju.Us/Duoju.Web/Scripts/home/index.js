
$(document).ready(function () {

    // === jQuery Peity === //
    $.fn.peity.defaults.line = {
        strokeWidth: 1,
        delimeter: ",",
        height: 24,
        max: null,
        min: 0,
        width: 50
    };
    $.fn.peity.defaults.bar = {
        delimeter: ",",
        height: 24,
        max: null,
        min: 0,
        width: 50
    };
    $(".peity_line_good span").peity("line", {
        colour: "#B1FFA9",
        strokeColour: "#459D1C"
    });
    $(".peity_line_bad span").peity("line", {
        colour: "#FFC4C7",
        strokeColour: "#BA1E20"
    });
    $(".peity_line_neutral span").peity("line", {
        colour: "#CCCCCC",
        strokeColour: "#757575"
    });
    $(".peity_bar_good span").peity("bar", {
        colour: "#459D1C"
    });
    $(".peity_bar_bad span").peity("bar", {
        colour: "#BA1E20"
    });
    $(".peity_bar_neutral span").peity("bar", {
        colour: "#757575"
    });

    maruti.init();

    ycf_crm.getSupplierInWeek(null, function (data) {
        var sin = [], cos = [];
        var sSum = 0, cSum = 0;
        if (data.rows) {
            var start = Date.parse(data.start);
            var end = Date.parse(data.end);
            while (Date.compare(start, end) <= 0) {
                var dateStr = start.toString("yyyyMMdd");
                var sCount = 0;
                var cCount = 0;
                $.each(data.rows, function (i) {
                    if (dateStr == Date.parse(data.rows[i].CreateDate).toString("yyyyMMdd")) {
                        if (data.rows[i].SupplierType == 1) {
                            sCount++;
                            sSum++;
                        } else if (data.rows[i].SupplierType == 2) {
                            cCount++;
                            cSum++;
                        }
                    }
                });
                sin.push([dateStr, sCount]);
                cos.push([dateStr, cCount]);
                start.addDays(1);
            }

            $('#new_supplier').html(sSum);
            $('#potential_supplier').html(cSum);

            // === Prepare peity charts === //
            maruti.peity();
            
            // === Make chart === //
            var plot = $.plot($(".chart"),
                       [{ data: sin, label: "新增", color: "#ee7951" }, { data: cos, label: "潜在", color: "#4fb9f0"}], {
                           yaxis: { min: 0, tickDecimals: 0 },
                           xaxis: { tickDecimals: 0 },
                           series: {
                               lines: { show: true },
                               points: { show: true }
                           },
                           grid: { hoverable: true, clickable: true }
                       });

            // === Point hover in chart === //
            var previousPoint = null;
            $(".chart").bind("plothover", function (event, pos, item) {
                if (item) {
                    if (previousPoint != item.dataIndex) {
                        previousPoint = item.dataIndex;

                        $('#tooltip').fadeOut(200, function () {
                            $(this).remove();
                        });
                        var x = item.datapoint[0].toFixed(0),
					        y = item.datapoint[1].toFixed(0);

                        maruti.flot_tooltip(item.pageX, item.pageY, item.series.label + y + " 个");
                    }

                } else {
                    $('#tooltip').fadeOut(200, function () {
                        $(this).remove();
                    });
                    previousPoint = null;
                }
            });
        }
    }, function (e) {
        ycf_crm.errorHandler();
    });

});


maruti = {
    init: function () {
        var date = Date.today();
        $('#fullcalendar').fullCalendar({
            header: {
                left: 'prev,next',
                center: 'title',
                right: 'basicWeek,basicDay'
            },
            defaultView: 'basicWeek',
            monthNames: ['一月', '二月', '三月', '四月', '五月', '六月', '七月', '八月', '九月', '十月', '十一月', '十二月'],
            dayNamesShort: ['周日', '周一', '周二', '周三', '周四', '周五', '周六'],
            monthNamesShort: ["一月", "二月", "三月", "四月", "五月", "六月", "七月", "八月", "九月", "十月", "十一月", "十二月"],
            dayNames: ["周日", "周一", "周二", "周三", "周四", "周五", "周六"],
            titleFormat: {
                month: 'yyyy年MM月',
                week: 'yyyy年MM月dd日{ &#8212; yyyy年MM月dd日}',
                day: 'yyyy年MM月dd日, dddd'
            },
            buttonText: {
                today: '今天',
                month: '月',
                week: '周',
                day: '日',
                prev: '上一周',
                next: '下一周'
            },
            editable: false,
            droppable: false,
            eventClick: function (event, jsEvent, view) {
                $('#lbTaskTitle').html('<strong>' + event.title + '</strong> ');
                $('#taskStartDate').val(event.start.toString("yyyy/MM/dd hh:mm"));
                $('#editTaskContent').val(event.content);
                $('#taskEndDate').val((event.end == null ? event.start.toString("yyyy/MM/dd hh:mm") : event.end.toString("yyyy/MM/dd hh:mm")));
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
    },

    // === Peity charts === //
    peity: function () {
        $.fn.peity.defaults.line = {
            strokeWidth: 1,
            delimeter: ",",
            height: 24,
            max: null,
            min: 0,
            width: 50
        };
        $.fn.peity.defaults.bar = {
            delimeter: ",",
            height: 24,
            max: null,
            min: 0,
            width: 50
        };
        $(".peity_line_good span").peity("line", {
            colour: "#57a532",
            strokeColour: "#459D1C"
        });
        $(".peity_line_bad span").peity("line", {
            colour: "#FFC4C7",
            strokeColour: "#BA1E20"
        });
        $(".peity_line_neutral span").peity("line", {
            colour: "#CCCCCC",
            strokeColour: "#757575"
        });
        $(".peity_bar_good span").peity("bar", {
            colour: "#459D1C"
        });
        $(".peity_bar_bad span").peity("bar", {
            colour: "#BA1E20"
        });
        $(".peity_bar_neutral span").peity("bar", {
            colour: "#4fb9f0"
        });
    },

    // === Tooltip for flot charts === //
    flot_tooltip: function (x, y, contents) {
        $('<div id="tooltip">' + contents + '</div>').css({
            top: y + 5,
            left: x + 5
        }).appendTo("body").fadeIn(200);
    }
};

