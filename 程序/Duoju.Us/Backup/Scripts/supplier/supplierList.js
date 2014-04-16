$(function () {
    var __pageWidth = $(window).width();
    var cls_supplier = [];

    if (__pageWidth < 600) {
        $('.container-fluid').attr('style', 'padding:0px 5px;');
        $('.widget-box').attr('style', 'margin-top: 0px;');
        $('.widget-title').attr('style', 'height:auto;padding-top: 5px;');
        $('.div_search').attr('style', 'position:static;margin-top:3px;margin-left:35px;');
        $('#txtSearch').attr('style', 'width:80%;');
        $('#btnToday').html('今天');
        $('#btnTwoDay').html('两天');
        $('#btnSevenDay').html('七天');
        $('#start').attr('style', 'margin-left:5px;');
        $('#start').width(100);
        $('#end').width(100);
        cls_supplier = [[
            { field: 'SupplierName', title: '酒店名/客户名', width: 120, sortable: true, formatter: function (value, rowData, rowIndex) {
                return '<a class="a_detial" href="/Supplier/SupplierDetial?sid=' + rowData.SupplierId + '">' + value + '</a>';
            }
            }
        ]];
    } else if (__pageWidth < 1280 && __pageWidth >= 600) {
        $('.container-fluid').attr('style', 'padding:0px 5px;');
        $('.widget-title').attr('style', 'height:auto;padding-top: 5px;');
        $('.div_search').attr('style', 'position:static;margin-top:3px;margin-left:35px;');
        $('#txtSearch').attr('style', 'width:80%;');
        $('#btnToday').html('今天');
        $('#btnTwoDay').html('两天');
        $('#btnSevenDay').html('七天');
        $('#start').width(105);
        $('#end').width(105);
        cls_supplier = [[
            { field: 'SupplierName', title: '酒店名/客户名', width: 120, sortable: true, formatter: function (value, rowData, rowIndex) {
                return '<a class="a_detial" href="/Supplier/SupplierDetial?sid=' + rowData.SupplierId + '">' + value + '</a>';
            }
            },
            { field: 'Release', title: '释放', width: 25, sortable: false, formatter: function (value, rowData, rowIndex) {
                return '<a class="a_release" sid="' + rowData.SupplierId + '" href="javascript:void(0);">释放</a>';
            }
            }
        ]];
    } else if (__pageWidth >= 1280) {
        //定义套餐列表的列
        cls_supplier = [[
            {
                field: 'ck', checkbox: true
            },
            { field: 'SupplierId', title: '编号', width: 20, sortable: true },
            { field: 'SupplierName', title: '酒店名/客户名', width: 120, sortable: true, formatter: function (value, rowData, rowIndex) {
                return '<a class="a_detial" href="/Supplier/SupplierDetial?sid=' + rowData.SupplierId + '">' + value + '</a>';
            }
            },
            { field: 'Contacts', title: '负责人', width: 30, sortable: true },
            { field: 'Position', title: '职位', width: 35, sortable: true },
            { field: 'TelePhone', title: '手机号', width: 60, sortable: true },
            { field: 'Email', title: '邮箱', width: 80, sortable: true },
            { field: 'Address', title: '地址', width: 135, sortable: true },
            { field: 'CreateDate', title: '创建时间', width: 45, sortable: true, formatter: function (value, rowData, rowIndex) {
                try {
                    return Date.parse(value).toString('yyyy-MM-dd');
                }
                catch (err) {
                    return '--';
                }
            }
            },
            { field: 'Detial', title: '详情', width: 25, sortable: false, formatter: function (value, rowData, rowIndex) {
                return '<a class="a_Detial" href="/Supplier/SupplierDetial?sid=' + rowData.SupplierId + '">查看</a>';
            }
            },
            { field: 'Operate', title: '拜访记录', width: 40, sortable: false, formatter: function (value, rowData, rowIndex) {
                return '<a class="a_Detial" href="/FollowLog/FollowLogDetial?sid=' + rowData.SupplierId + '">添加</a>';
            }
            },
            { field: 'Release', title: '释放', width: 25, sortable: false, formatter: function (value, rowData, rowIndex) {
                return '<a class="a_release" sid="' + rowData.SupplierId + '" href="javascript:void(0);">释放</a>';
            }
            }
        ]];
    }


    $('#divTable').height($('#content').height() / 1.5);

    var checkin = $('#start').datepicker({
        format: "yyyy-mm-dd",
        language: "zh-CN"
    })
    .on('changeDate', function (ev) {
        if (ev.date.valueOf() > checkout.date.valueOf()) {
            var newDate = new Date(ev.date)
            newDate.setDate(newDate.getDate() + 1);
            checkout.setValue(newDate);
        }
        checkin.hide();
        $('#endDate')[0].focus();
    }).data('datepicker');

    var checkout = $('#end').datepicker({
        format: "yyyy-mm-dd",
        language: "zh-CN",
        onRender: function (date) {
            return date.valueOf() <= checkin.date.valueOf() ? 'disabled' : '';
        }
    }).on('changeDate', function (ev) {
        checkout.hide();
    }).data('datepicker');



    var tb_supplier = [{
        id: 'btnAdd',
        text: ' 添加',
        iconCls: 'icon-folder-open',
        //定义添加操作的回调函数
        handler: function () {
            document.location.href = '/Supplier/SupplierDetial';
        }
    }, '-', {
        id: 'btnEdit',
        text: ' 编辑',
        iconCls: 'icon-pencil',
        //定义编辑操作的回调函数
        handler: function () {
            var selections = $('#tb_supplier').datagrid('getSelections');
            if (null == selections || '' == selections) {
                $('#myModalLabel').html('操作提示');
                $('.modal-body').html('先勾选要编辑的客户');
                $('#myModal').modal();
            } else {
                document.location.href = '/Supplier/SupplierDetial?sid=' + selections[0].SupplierId;
            }
        }
    }, '-'];

    var loadGrid = function (opts) {
        var releaseEvent = function () {
            $('.a_release').click(function () {
                var sid = $(this).attr('sid');
                ycf_crm.releaseSupplier(sid, null, function (data) {
                    $('#myModalLabel').html('操作提示');
                    $('.modal-body').html(data.Message);
                    $('#myModal').modal();
                    if (data.State == 'success') {
                        $('#tb_supplier').datagrid('reload');
                        $('#tb_supplier').datagrid('clearSelections');
                    }
                }, null);
            });
        };
        ycf_crm.loadGrid($('#tb_supplier'), {
            title: null,
            url: opts.url,
            loadMsg: 'Data Loading...',
            queryParams: opts.pars,
            sortName: 'SupplierId',
            fit: true,
            fitColumns: true,
            striped: true,
            height: 400,
            pageSize: 10,
            idField: 'SupplierId',
            columns: cls_supplier,
            pagination: true,
            rownumbers: true,
            singleSelect: false,
            toolbar: tb_supplier
        }, null, null, releaseEvent);
    };

    var options = {};
    options.url = "/Supplier/GetSupplierList";
    try {
        loadGrid(options);
    } catch (e) {
        ycf_crm.errorHandler();
    }

    $('#btnSearch').click(function () {
        try {
            options.url = "/Supplier/SearchSupplier";
            options.pars = {
                'keyWord': $('#txtSearch').val()
            };
            $('#tb_supplier').datagrid('clearSelections');
            loadGrid(options);
        } catch (e) {
            ycf_crm.errorHandler();
        }
    });

    $('#btnToday').click(function () {
        loadFollowSupplier(Date.today().toString('yyyy-MM-dd'), Date.today().toString('yyyy-MM-dd') + " 23:59:59");
    });

    $('#btnTwoDay').click(function () {
        loadFollowSupplier(Date.today().toString('yyyy-MM-dd'), Date.today().addDays(2).toString('yyyy-MM-dd') + " 23:59:59");
    });

    $('#btnSevenDay').click(function () {
        loadFollowSupplier(Date.today().toString('yyyy-MM-dd'), Date.today().addDays(7).toString('yyyy-MM-dd') + " 23:59:59");
    });

    $('#btnAllDay').click(function () {
        options.url = "/Supplier/GetSupplierList";
        try {
            loadGrid(options);
        } catch (e) {
            ycf_crm.errorHandler();
        }
    });

    $('#btnFilterDate').click(function () {
        if ($('#start').val() != "" && $('#end').val() != "") {
            loadFollowSupplier($('#start').val(), $('#end').val() + " 23:59:59");
        } else {
            $('#myModalLabel').html('操作提示');
            $('.modal-body').html('请填写要筛选的时间段');
            $('#myModal').modal();
        }
    });

    $(document).keydown(function (event) {
        if (event.keyCode == 13) {
            $('#btnSearch').trigger('click');
        }
    });

    var loadFollowSupplier = function (start, end) {
        try {
            options.url = "/Supplier/GetFollowLogListBySIdAndUId";
            options.pars = {
                start: start,
                end: end
            };
            $('#tb_supplier').datagrid('clearSelections');
            loadGrid(options);
        } catch (e) {
            ycf_crm.errorHandler();
        }
    };
});
