
$(document).ready(function () {
    $('#divTable').height($('#content').height() / 1.2);

    var __pageWidth = $(window).width();
    var cls_supplier = [];

    if (__pageWidth < 600) {
        $('.container-fluid').attr('style', 'padding:0px 5px;');
        $('.widget-box').attr('style', 'margin-top: 0px;');
        //定义跟进记录列表的列
        cls_supplier = [[
            { field: 'SupplierName', title: '客户名', width: 60, sortable: true, formatter: function (value, rowData, rowIndex) {
                return rowData.SupplierInfo.SupplierName;
            }
            },
            { field: 'Detial', title: '详情', width: 20, sortable: true, formatter: function (value, rowData, rowIndex) {
                return '<a class="a_Detial" href="/Supplier/SupplierDetial?sid=' + rowData.SupplierId + '">查看</a>';
            }
            }
        ]];
    } else if (__pageWidth < 1280 && __pageWidth >= 600) {
        //定义跟进记录列表的列
        cls_supplier = [[
            { field: 'SupplierName', title: '客户名', width: 60, sortable: true, formatter: function (value, rowData, rowIndex) {
                return rowData.SupplierInfo.SupplierName;
            }
            },
            { field: 'FollowState', title: '跟进进度', width: 60, sortable: true, formatter: function (value, rowData, rowIndex) {
                return rowData.FollowStateInfo.State;
            }
            },
            { field: 'CreateDate', title: '创建时间', width: 40, sortable: true },
            { field: 'Detial', title: '详情', width: 20, sortable: true, formatter: function (value, rowData, rowIndex) {
                return '<a class="a_Detial" href="/Supplier/SupplierDetial?sid=' + rowData.SupplierId + '">查看</a>';
            }
            }
        ]];
    } else if (__pageWidth >= 1280) {
        //定义跟进记录列表的列
        cls_supplier = [[
            {
                field: 'ck', checkbox: true
            },
            { field: 'FollowLogId', title: '编号', width: 20, sortable: true },
            { field: 'SupplierName', title: '客户名', width: 60, sortable: true, formatter: function (value, rowData, rowIndex) {
                return rowData.SupplierInfo.SupplierName;
            }
            },
            { field: 'FollowState', title: '跟进进度', width: 60, sortable: true, formatter: function (value, rowData, rowIndex) {
                return rowData.FollowStateInfo.State;
            }
            },
            { field: 'FollowName', title: '跟进记录', width: 100, sortable: true },
            { field: 'CreateDate', title: '创建时间', width: 40, sortable: true },
            { field: 'Detial', title: '详情', width: 20, sortable: true, formatter: function (value, rowData, rowIndex) {
                return '<a class="a_Detial" href="/Supplier/SupplierDetial?sid=' + rowData.SupplierId + '">查看</a>';
            }
            }
        ]];
    }

    $('input[type=checkbox],input[type=radio],input[type=file]').uniform();
    $('select').select2();
    var checkin = $('#startDate').datepicker()
    .on('changeDate', function (ev) {
        if (ev.date.valueOf() > checkout.date.valueOf()) {
            var newDate = new Date(ev.date)
            newDate.setDate(newDate.getDate() + 1);
            checkout.setValue(newDate);
        }
        checkin.hide();
        $('#endDate')[0].focus();
    }).data('datepicker');

    var checkout = $('#endDate').datepicker({
        onRender: function (date) {
            return date.valueOf() <= checkin.date.valueOf() ? 'disabled' : '';
        }
    }).on('changeDate', function (ev) {
        checkout.hide();
    }).data('datepicker');

    

    var tb_followLog = [{
        id: 'btnEdit',
        text: ' 编辑',
        iconCls: 'icon-pencil',
        //定义编辑操作的回调函数
        handler: function () {
            var selections = $('#tb_followLog').datagrid('getSelections');
            if (null == selections || '' == selections) {
                $('#myModalLabel').html('操作提示');
                $('.modal-body').html('先勾选要编辑的客户');
                $('#myModal').modal();
            } else {
                document.location.href = '/FollowLog/FollowLogDetial?lid=' + selections[0].FollowLogId;
            }
        }
    }, '-'];

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
            pageSize: 10,
            idField: 'FollowLogId',
            columns: cls_supplier,
            pagination: true,
            rownumbers: true,
            singleSelect: false,
            toolbar: tb_followLog
        }, null, null, null);
    };

    var options = {};
    options.url = "/FollowLog/GetFollowList";

    try {
        loadGrid(options);
    } catch (e) {
        ycf_crm.errorHandler();
    }

    $('#btnSearch').click(function () {
        try {
            options.url = "/FollowLog/SearchFollowLog";
            options.pars = {
                'startDate': $('#startDate').data('date'),
                'endDate': $('#endDate').data('date'),
                'keyWord': $('#txtSearch').val()
            };
            $('#tb_followLog').datagrid('clearSelections');
            loadGrid(options);
        } catch (e) {
            ycf_crm.errorHandler();
        }
    });
});