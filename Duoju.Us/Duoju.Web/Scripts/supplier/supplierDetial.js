$(document).ready(function () {
    var currentSId = $.query.get('sid');

    var cls_contacts = [];
    var cls_supplier = [];
    var cls_task = [];

    var __pageWidth = $(window).width();

    ycf_crm.getProvince(null, function (data) {
        $.each(data, function (i) {
            $('#selectProvince').append('<option text="' + data[i].ProvinceName + '" code="' + data[i].ProvinceCode + '" value="' + data[i].ProvinceId + '">' + data[i].PinYinNameAbbr + '-' + data[i].ProvinceName + '</option>');
        });
        if ($('#hidPid').val() != 0 && $('#hidPid').val()) {
            $("#selectProvince").select2("val", $('#hidPid').val());
            $("#selectProvince").trigger("change");
        }
    }, function (e) {
        console.log(e);
    });

    $('#selectProvince').change(function () {
        var selectOption = $(this).find("option:selected");
        var code = $(selectOption).attr('code');
        $('#selectCity').html('<option value="0">请选择城市</option>');
        $("#selectCity").select2("val", "0");
        ycf_crm.getCityByProvinceCode(code, null, function (data) {
            $.each(data, function (i) {
                $('#selectCity').append('<option text="' + data[i].CityName + '" code="' + data[i].CityCode + '" value="' + data[i].CityId + '">' + data[i].PinYinNameAbbr + '-' + data[i].CityName + '</option>');
            });
            if ($('#hidCid').val() != 0 && $('#hidCid').val()) {
                $("#selectCity").select2("val", $('#hidCid').val());
                $("#selectCity").trigger("change");
            }
        }, function (e) {
            console.log(e);
        });
    });

    $('#selectCity').change(function () {
        var selectOption = $(this).find("option:selected");
        var code = $(selectOption).attr('code');
        $('#selectDistict').html('<option value="0">请选择区域</option>');
        $("#selectDistict").select2("val", "0");
        ycf_crm.getDistrictByCityCode(code, null, function (data) {
            $.each(data, function (i) {
                $('#selectDistict').append('<option text="' + data[i].DistrictName + '" code="' + data[i].DistrictCode + '" value="' + data[i].DistrictId + '">' + data[i].PinYinNameAbbr + '-' + data[i].DistrictName + '</option>');
            });
            if ($('#hidDid').val() != 0 && $('#hidDid').val()) {
                $("#selectDistict").select2("val", $('#hidDid').val());
            }
        }, function (e) {
            console.log(e);
        });
    });

    if (__pageWidth < 600) {
        $('.container-fluid').attr('style', 'padding:0px 5px;');
        $('.widget-box').attr('style', 'margin-top: 0px;');
        $('#spanProvinceName').html('省份');
        $('#spanCityName').html('城市');
        $('#spanDistrictName').html('地区');

        //start：定义联系人列表
        cls_contacts = [[
            { field: 'ContactsName', title: '联系人', width: 40, sortable: true },
            { field: 'TelePhone', title: '手机号', width: 60, sortable: true },
            { field: 'OfficePhone', title: '办公号码', width: 60, sortable: true }
        ]];

        //start：定义跟进记录列表
        cls_task = [[
            { field: 'Title', title: '标题', width: 60 },
            { field: 'EditTask', title: '任务详情', width: 25, sortable: true, formatter: function (value, rowData, rowIndex) {
                return '<a class="a_task_detial" tid=' + rowData.TaskId + '>详情</a>';
            }
            }
        ]];
        //start：定义跟进记录列表
        cls_supplier = [[
            { field: 'SupplierName', title: '客户名', width: 60, sortable: true, formatter: function (value, rowData, rowIndex) {
                return rowData.SupplierInfo.SupplierName;
            }
            },
            { field: 'FollowState', title: '跟进进度', width: 60, sortable: true, formatter: function (value, rowData, rowIndex) {
                return rowData.FollowStateInfo.State;
            }
            }
        ]];
    } else if (__pageWidth < 1280 && __pageWidth >= 600) {
        //start：定义联系人列表
        cls_contacts = [[
            { field: 'ContactsName', title: '联系人', width: 40, sortable: true },
            { field: 'TelePhone', title: '手机号', width: 60, sortable: true },
            { field: 'OfficePhone', title: '办公号码', width: 60, sortable: true },
            { field: 'QQ', title: 'QQ', width: 80, sortable: true }
        ]];

        //start：定义跟进记录列表
        cls_task = [[
            { field: 'EditTask', title: '任务详情', width: 25, sortable: true, formatter: function (value, rowData, rowIndex) {
                return '<a class="a_task_detial" tid=' + rowData.TaskId + '>详情</a>';
            }
            },
            { field: 'Title', title: '标题', width: 60 },
            { field: 'TaskState', title: '任务状态', width: 25, sortable: true, formatter: function (value, rowData, rowIndex) {
                if (rowData.TaskState == '1') {
                    return '新任务';
                } else if (rowData.TaskState == '2') {
                    return '已阅';
                } else if (rowData.TaskState == '3') {
                    return '重要';
                } else if (rowData.TaskState == '4') {
                    return '已取消';
                } else if (rowData.TaskState == '5') {
                    return '已完成';
                }
            }
            },
            { field: 'StartTime', title: '开始时间', width: 50, sortable: true },
            { field: 'EndTime', title: '结束时间', width: 50, sortable: true }
        ]];

        //start：定义跟进记录列表
        cls_supplier = [[
            { field: 'SupplierName', title: '客户名', width: 60, sortable: true, formatter: function (value, rowData, rowIndex) {
                return rowData.SupplierInfo.SupplierName;
            }
            },
            { field: 'FollowState', title: '跟进进度', width: 60, sortable: true, formatter: function (value, rowData, rowIndex) {
                return rowData.FollowStateInfo.State;
            }
            },
            { field: 'FollowName', title: '跟进记录', width: 100, sortable: true },
            { field: 'CreateDate', title: '创建时间', width: 40, sortable: true }
        ]];
    } else if (__pageWidth >= 1280) {
        //start：定义联系人列表
        cls_contacts = [[
            { field: 'ContactsId', title: '编号', width: 30, hidden: true, sortable: true },
            { field: 'ContactsName', title: '联系人', width: 40, sortable: true },
            { field: 'Position', title: '职位', width: 45, sortable: true },
            { field: 'TelePhone', title: '手机号', width: 60, sortable: true },
            { field: 'OfficePhone', title: '办公号码', width: 60, sortable: true },
            { field: 'Email', title: '邮箱', width: 80, sortable: true },
            { field: 'QQ', title: 'QQ', width: 80, sortable: true },
        //{ field: 'Address', title: '地址', width: 135, sortable: true },
            {field: 'Remark', title: '备注', width: 135, sortable: true }
        ]];

        //start：定义跟进记录列表
        cls_task = [[
            { field: 'TaskId', title: '编号', width: 20, hidden: true, sortable: true },
            { field: 'EditTask', title: '任务详情', width: 25, sortable: true, formatter: function (value, rowData, rowIndex) {
                return '<a class="a_task_detial" tid=' + rowData.TaskId + '>详情</a>';
            }
            },
            { field: 'Title', title: '标题', width: 60 },
            { field: 'Content', title: '内容', width: 120 },
            { field: 'TaskState', title: '任务状态', width: 25, sortable: true, formatter: function (value, rowData, rowIndex) {
                if (rowData.TaskState == '1') {
                    return '新任务';
                } else if (rowData.TaskState == '2') {
                    return '已阅';
                } else if (rowData.TaskState == '3') {
                    return '重要';
                } else if (rowData.TaskState == '4') {
                    return '已取消';
                } else if (rowData.TaskState == '5') {
                    return '已完成';
                }
            }
            },
            { field: 'StartTime', title: '开始时间', width: 50, sortable: true },
            { field: 'EndTime', title: '结束时间', width: 50, sortable: true }
        ]];

        //start：定义跟进记录列表
        cls_supplier = [[
            { field: 'FollowLogId', title: '编号', width: 20, hidden: true, sortable: true },
            { field: 'SupplierName', title: '客户名', width: 60, sortable: true, formatter: function (value, rowData, rowIndex) {
                return rowData.SupplierInfo.SupplierName;
            }
            },
            { field: 'FollowState', title: '跟进进度', width: 60, sortable: true, formatter: function (value, rowData, rowIndex) {
                return rowData.FollowStateInfo.State;
            }
            },
            { field: 'FollowName', title: '跟进记录', width: 100, sortable: true },
            { field: 'CreateDate', title: '创建时间', width: 40, sortable: true }
        ]];
    }

    $('#basic_validate').ajaxForm({
        dataType: 'json',
        success: function (data) {
            if (!isNaN(data.Message)) {
                $('#supplierId').val(data.Message);
                currentSId = data.Message;
                $('#tip_supplierName').hide();
                loadContacts();
                loadFollowLog();
                loadTask();
            }
            $('#div_EditPanel').hide();
            $('#myModal').modal();
        }
    });

    $('#contactsInfoForm').ajaxForm({
        dataType: 'json',
        success: function (data) {
            $('#tb_contacts').datagrid('reload');
            $('#contactsInfoForm').clearForm();
            $('#modal-contacts').modal('hide');
            $('#myModal').modal();
        }
    });

    $('#add-event-submit').click(function () {
        if ($('#txtStartDate').val() == '') {
            alert('请选择任务开始时间！');
            return false;
        }
        if ($('#txtEndDate').val() == '') {
            alert('请选择任务结束时间！');
            return false;
        }
        $('#hidSupplierId').val(currentSId);
        return true;
    });

    $('#btnSaveEditPosition').click(function () {
        $('#supplierId_position').val(currentSId);
        if ($('#selectProvince').val() == 0) {
            alert('请选择省份！');
            return false;
        }
        if ($('#selectCity').val() == 0) {
            alert('请选择城市！');
            return false;
        }
        $('#spanProvinceName').html($('#selectProvince').find('option:selected').attr('text'));
        $('#hidPid').val($('#selectProvince').find('option:selected').val());
        $('#spanCityName').html($('#selectCity').find('option:selected').attr('text'));
        $('#hidCid').val($('#selectCity').find('option:selected').val());
        $('#spanDistrictName').html($('#selectDistict').find('option:selected').attr('text'));
        $('#hidDid').val($('#selectDistict').find('option:selected').val());
        $('#modal-container-449303').modal('hide');
    });

    $('#formTask').ajaxForm({
        dataType: 'json',
        success: function (data) {
            var _ev = ycf_async.handlerTask(data);
            ycf_async.__aysncTaskList.push(_ev);
            $('#tb_task').datagrid('reload');
            $('#formTask').clearForm();
            $('#modal-add-event').modal('hide');
            $('#myModal').modal();
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

    $('#btnCancel').click(function () {
        $('#div_EditPanel').hide();
    });



    var tb_contacts = [{
        id: 'btnAdd',
        text: ' 添加',
        iconCls: 'icon-folder-open',
        //定义添加操作的回调函数
        handler: function () {
            $('#contactsInfo_SupplierId').val(currentSId);
            $('#modal-contacts').modal();
            $("#modal-contacts").draggable({
                handle: ".modal-header"

            });
        }
    }, '-', {
        id: 'btnEdit',
        text: ' 编辑',
        iconCls: 'icon-pencil',
        //定义编辑操作的回调函数
        handler: function () {

        }
    }, '-', {
        id: 'btndelete',
        text: ' 删除',
        iconCls: 'icon-remove',
        //定义删除的回调函数
        handler: function () {
        }
    }, '-'];
    //end：定义联系人列表

    //    var tb_followLog = [{
    //        id: 'btnEdit',
    //        text: ' 编辑',
    //        iconCls: 'icon-pencil',
    //        //定义编辑操作的回调函数
    //        handler: function () {
    //            var selections = $('#tb_followLog').datagrid('getSelections');
    //            if (null == selections || '' == selections) {
    //                $('#myModalLabel').html('操作提示');
    //                $('.modal-body').html('先勾选要编辑的客户');
    //                $('#myModal').modal();
    //            } else {
    //                document.location.href = '/FollowLog/FollowLogDetial?lid=' + selections[0].FollowLogId;
    //            }
    //        }
    //    }, '-'];
    //end：定义跟进记录列表



    //    var tb_task = [
    //    {
    //        id: 'btnAdd',
    //        text: ' 添加',
    //        iconCls: 'icon-folder-open',
    //        //定义添加操作的回调函数
    //        handler: function () {
    //        }
    //    }, '-', 
    //    {
    //    id: 'btnEdit',
    //    text: ' 编辑',
    //    iconCls: 'icon-pencil',
    //    //定义编辑操作的回调函数
    //    handler: function () {
    //        var taskList = $('#tb_task').datagrid('getSelections');
    //        if (taskList.length > 0) {
    //            var evs = ycf_async.findEventById(taskList[0].TaskId);
    //            $('#currentTaskId').val(taskList[0].TaskId);
    //            $('#currentSupplierId').val(evs[0].supplierId);
    //            $('#lbTaskTitle').html('<strong>' + evs[0].title + '</strong> ');
    //            $('#spanTimeDiff').html('开始: ' + evs[0].start.toString("yyyy.MM.dd hh:mm") + '&nbsp;&nbsp;&nbsp;&nbsp;结束:' + (evs[0].end == null ? evs[0].start.toString("yyyy.MM.dd hh:mm") : evs[0].end.toString("yyyy.MM.dd hh:mm")));
    //            $('#lbTaskContent').html(evs[0].content);
    //            $('#currentTaskId').val(evs[0].id);
    //            if (evs[0].taskState == 1) {
    //                $('#FlagTask').removeAttr('style');
    //                $('#CancelTask').find('i').removeClass('icon-ban-circle');
    //                $('#CancelTask').find('i').addClass('icon-remove');
    //                $('#CancelTask').find('i').removeAttr('style');
    //                ycf_crm.readTask(evs[0].id, 2, null, function (d) {
    //                    evs[0].className = 'fc-event-skin-flag';
    //                    if ($('#fullcalendar').length > 0) {
    //                        $('#fullcalendar').fullCalendar('updateEvent', event);
    //                    }
    //                    ycf_async.updateEventStateById(evs[0].id, 2);
    //                }, function (e) {
    //                    console.log(e);
    //                });
    //            } else if (evs[0].taskState == 3) {
    //                $('#FlagTask').attr('style', 'color:#f74d4d;');
    //                $('#CancelTask').find('i').removeClass('icon-ban-circle');
    //                $('#CancelTask').find('i').addClass('icon-remove');
    //                $('#CancelTask').find('i').removeAttr('style');
    //            } else if (evs[0].taskState == 4) {
    //                $('#CancelTask').find('i').removeClass('icon-remove');
    //                $('#CancelTask').find('i').addClass('icon-ban-circle');
    //                $('#CancelTask').find('i').attr('style', 'color:#f74d4d;');
    //                $('#FlagTask').removeAttr('style');
    //            } else {
    //                $('#CancelTask').find('i').removeClass('icon-ban-circle');
    //                $('#CancelTask').find('i').addClass('icon-remove');
    //                $('#FlagTask').removeAttr('style');
    //                $('#CancelTask').find('i').removeAttr('style');
    //            }
    //            $('#modal-showTask').modal();
    //        }
    //    }
    //}, '-']
    //end：定义跟进记录列表

    var isSupplierNameExit = false;
    $("#txtSupplierName").blur(function () {
        if ($(this).val() !== '' && currentSId == 0) {
            ycf_crm.getSupplierByName($(this).val(), null, function (data) {
                if (data.State === 'Exist') {
                    isSupplierNameExit = true;
                    var count = data.Message.split(';').length;
                    var top = 22 - count * 9;
                    $('#tip_supplierName').css('top', top);
                    $('#popoverContent').html('系统中已存在：</br>' + data.Message.replace(/;/g, '</br>'));
                    $('#tip_supplierName').show();
                } else {
                    isSupplierNameExit = false;
                    $('#tip_supplierName').hide();
                }
            }, null);
        } else {
            isSupplierNameExit = false;
        }
    });

    $('#btnSave').click(function () {
        if (isSupplierNameExit) {
            if (confirm("系统中已存在名字相似的客户，是否确定添加？")) {
                return true;
            } else {
                return false;
            }
        }
    });

    $('#add-event').click(function () {
        $('#hidSupplierId').val(currentSId);
    });

    $('#btnShowPanel').click(function () {
        $("#div_EditPanel").toggle();
    });


    var loadContacts = function () {
        if (currentSId == 0) {
            $('#div_contacts_msg').show();
        } else {
            $('#div_EditPanel').hide();
            $('#div_contacts_msg').hide();
            var loadGrid = function (opts) {
                ycf_crm.loadGrid($('#tb_contacts'), {
                    title: null,
                    url: opts.url,
                    loadMsg: 'Data Loading...',
                    queryParams: opts.pars,
                    sortName: 'ContactsId',
                    fit: true,
                    fitColumns: true,
                    striped: true,
                    height: 400,
                    pageSize: 10,
                    idField: 'ContactsId',
                    columns: cls_contacts,
                    pagination: false,
                    rownumbers: true,
                    singleSelect: false,
                    toolbar: tb_contacts
                }, null, null, null);
            };
            try {
                var options = {};
                options.url = "/Supplier/GetContactsBySupplierId";
                options.pars = { sid: currentSId };
                loadGrid(options);

                isLoadContacts = true;
            } catch (e) {
                ycf_crm.errorHandler();
            }
        }
    };

    loadContacts();

    var loadFollowLog = function () {
        if (currentSId == 0) {
            $('#div_followLog_msg').show();
        } else {
            $('#div_followLog_msg').hide();
            var loadGrid = function (opts) {
                ycf_crm.loadGrid($('#tb_followLog'), {
                    title: null,
                    url: opts.url,
                    loadMsg: 'Data Loading...',
                    queryParams: opts.pars,
                    sortName: 'FollowLogId',
                    fit: true,
                    fitColumns: true,
                    striped: true,
                    height: 400,
                    pageSize: 5,
                    idField: 'FollowLogId',
                    columns: cls_supplier,
                    pagination: true,
                    rownumbers: true,
                    singleSelect: false,
                    toolbar: null
                }, null, null, null);
            };
            try {
                var options = {};
                options.url = "/FollowLog/GetFollowLogListBySIdAndUId";
                options.pars = { sid: currentSId };
                loadGrid(options);
            } catch (e) {
                ycf_crm.errorHandler();
            }
        }
    };

    loadFollowLog();

    var loadTask = function () {
        if (currentSId == 0) {
            $('#div_task_msg').show();
        } else {
            $('#div_task_msg').hide();
            var eventClickFn = function () {
                $('.a_task_detial').click(function () {
                    var ctid = $(this).attr('tid');
                    var evs = ycf_async.findEventById(ctid);
                    $('#currentTaskId').val(ctid);
                    if (evs[0].supplierId) {
                        $('#currentSupplierId').val(evs[0].supplierId);
                    }
                    $('#lbTaskTitle').html('<strong>' + evs[0].title + '</strong> ');
                    $('#taskStartDate').val(evs[0].start.toString("yyyy/MM/dd hh:mm"));
                    $('#editTaskContent').val(evs[0].content);
                    $('#taskEndDate').val((evs[0].end == null ? evs[0].start.toString("yyyy/MM/dd hh:mm") : evs[0].end.toString("yyyy/MM/dd hh:mm")));
                    if (evs[0].supplierName || evs[0].telePhone) {
                        $('#spanSupplierInfo').html("<strong>酒店/客户名：</strong>" + evs[0].supplierName + "&nbsp;&nbsp;&nbsp;&nbsp;<strong>联系电话：</strong>" + evs[0].telePhone);
                    }
                    $('#spanTimeDiff').html('开始: ' + evs[0].start.toString("yyyy.MM.dd hh:mm") + '&nbsp;&nbsp;&nbsp;&nbsp;结束:' + (evs[0].end == null ? evs[0].start.toString("yyyy.MM.dd hh:mm") : evs[0].end.toString("yyyy.MM.dd hh:mm")));
                    $('#lbTaskContent').html(evs[0].content);
                    $('#currentTaskId').val(evs[0].id);
                    if (evs[0].taskState == 1) {
                        $('#FlagTask').removeAttr('style');
                        $('#CancelTask').find('i').removeClass('icon-ban-circle');
                        $('#CancelTask').find('i').addClass('icon-remove');
                        $('#CancelTask').find('i').removeAttr('style');
                        ycf_crm.readTask(evs[0].id, 2, null, function (d) {
                            evs[0].className = 'fc-event-skin-flag';
                            if ($('#fullcalendar').length > 0) {
                                $('#fullcalendar').fullCalendar('updateEvent', event);
                            }
                            ycf_async.updateEventStateById(evs[0].id, 2);
                            $("#tb_task").datagrid("reload");
                        }, function (e) {
                            console.log(e);
                        });
                    } else if (evs[0].taskState == 3) {
                        $('#FlagTask').attr('style', 'color:#f74d4d;');
                        $('#CancelTask').find('i').removeClass('icon-ban-circle');
                        $('#CancelTask').find('i').addClass('icon-remove');
                        $('#CancelTask').find('i').removeAttr('style');
                    } else if (evs[0].taskState == 4) {
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
                });
            };
            var loadGrid = function (opts) {
                ycf_crm.loadGrid($('#tb_task'), {
                    title: null,
                    url: opts.url,
                    loadMsg: 'Data Loading...',
                    queryParams: opts.pars,
                    sortName: 'TaskId',
                    fit: true,
                    fitColumns: true,
                    striped: true,
                    height: 400,
                    pageSize: 5,
                    idField: 'TaskId',
                    columns: cls_task,
                    pagination: false,
                    rownumbers: true,
                    singleSelect: true,
                    toolbar: null
                }, null, null, eventClickFn);
            };
            try {
                var options = {};
                options.url = "/Task/GetTaskBySidAndFromId";
                options.pars = { sid: currentSId };
                loadGrid(options);
            } catch (e) {
                ycf_crm.errorHandler();
            }
        }
    };

    loadTask();
}); 