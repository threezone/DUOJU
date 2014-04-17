$(function () {
    var __pageWidth = $(window).width();
    var cls_supplier = [];

    if (__pageWidth < 600) {
        $('.container-fluid').attr('style', 'padding:0px 5px;');
        $('.widget-box').attr('style', 'margin-top: 0px;');
        //定义套餐列表的列
        cls_supplier = [[
            { field: 'SupplierName', title: '酒店名/客户名', width: 120, sortable: true, formatter: function (value, rowData, rowIndex) {
                if (rowData.FollowById == 0 || rowData.FollowById == null) {
                    return '<a class="a_detial" href="/Supplier/SupplierDetial?sid=' + rowData.SupplierId + '">' + value + '</a>';
                } else {
                    return value;
                }
            }
            },
            { field: 'Operate', title: '申请跟进', width: 40, sortable: false, formatter: function (value, rowData, rowIndex) {
                if (rowData.FollowById == 0 || rowData.FollowById == null) {
                    return '<a class="a_follow" href="javascript:void(0);" sid="' + rowData.SupplierId + '">跟进</a>';
                } else {
                    return '已被跟进';
                }
            }
            }
        ]];
    } else if (__pageWidth < 1280 && __pageWidth >= 600) {
        //定义套餐列表的列
        cls_supplier = [[
            { field: 'SupplierName', title: '酒店名/客户名', width: 120, sortable: true, formatter: function (value, rowData, rowIndex) {
                if (rowData.FollowById == 0 || rowData.FollowById == null) {
                    return '<a class="a_detial" href="/Supplier/SupplierDetial?sid=' + rowData.SupplierId + '">' + value + '</a>';
                } else {
                    return value;
                }
            }
            },
            { field: 'TelePhone', title: '手机号', width: 110, sortable: true },
            { field: 'FollowByName', title: '当前跟进人', width: 40, sortable: false, formatter: function (value, rowData, rowIndex) {
                if (rowData.FollowBy) {
                    if (rowData.FollowBy.CRMUserId == 0) {
                        return '--';
                    } else {
                        return rowData.FollowBy.Name;
                    }
                } else {
                    return '--';
                }
            }
            },
            { field: 'Operate', title: '申请跟进', width: 40, sortable: false, formatter: function (value, rowData, rowIndex) {
                if (rowData.FollowById == 0 || rowData.FollowById == null) {
                    return '<a class="a_follow" href="javascript:void(0);" sid="' + rowData.SupplierId + '">跟进</a>';
                } else {
                    return '已被跟进';
                }
            }
            }
        ]];
    } else if (__pageWidth >= 1280) {
        //定义套餐列表的列
        cls_supplier = [[
            { field: 'SupplierId', title: '编号', width: 20, sortable: true },
            { field: 'SupplierName', title: '酒店名/客户名', width: 120, sortable: true, formatter: function (value, rowData, rowIndex) {
                if (rowData.FollowById == 0 || rowData.FollowById == null) {
                    return '<a class="a_detial" href="/Supplier/SupplierDetial?sid=' + rowData.SupplierId + '">' + value + '</a>';
                } else {
                    return value;
                }
            }
            },
            { field: 'Contacts', title: '负责人', width: 40, sortable: true },
            { field: 'Position', title: '职位', width: 35, sortable: true },
            { field: 'TelePhone', title: '手机号', width: 110, sortable: true },
            { field: 'Email', title: '邮箱', width: 80, sortable: true },
            { field: 'Address', title: '地址', width: 135, sortable: true },
            { field: 'FollowByName', title: '当前跟进人', width: 40, sortable: false, formatter: function (value, rowData, rowIndex) {
                if (rowData.FollowBy) {
                    if (rowData.FollowBy.CRMUserId == 0) {
                        return '--';
                    } else {
                        return rowData.FollowBy.Name;
                    }
                } else {
                    return '--';
                }
            }
            },
            { field: 'Operate', title: '申请跟进', width: 40, sortable: false, formatter: function (value, rowData, rowIndex) {
                if (rowData.FollowById == 0 || rowData.FollowById == null) {
                    return '<a class="a_follow" href="javascript:void(0);" sid="' + rowData.SupplierId + '">跟进</a>';
                } else {
                    return '已被跟进';
                }
            }
            }
        ]];
    }

    $('#divTable').height($('#content').height() / 1.4);

    

    var loadGrid = function (opts) {
        //绑定到表格生成的DOM上的事件，一定要为一个function对象
        var fnGrideCall = function () {
            $('.a_follow').click(function () {
                var sid = $(this).attr('sid');
                ycf_crm.followSupplier(sid, null, function (data) {
                    $('#myModalLabel').html('操作提示');
                    $('.modal-body').html(data.Message);
                    $('#myModal').modal();
                    if (data.State == 'success') {
                        $('#tb_publish_supplier').datagrid('reload');
                        $('#tb_publish_supplier').datagrid('clearSelections');
                    }
                }, null);
            });
        };
        ycf_crm.loadGrid($('#tb_publish_supplier'), {
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
            singleSelect: true,
            toolbar: null
        }, null, null, fnGrideCall);

    };

    var options = {};
    options.url = "/Supplier/GetPublishSupplierList";
    try {
        loadGrid(options);
    } catch (e) {
        ycf_crm.errorHandler();
    }


    $('#btnSearch').click(function () {
        try {
            options.url = "/Supplier/SearchPublishSupplier";
            options.pars = {
                'keyWord': $('#txtSearch').val()
            };
            $('#tb_supplier').datagrid('clearSelections');
            loadGrid(options);
        } catch (e) {
            ycf_crm.errorHandler();
        }
    });

    $(document).keydown(function (event) {
        if (event.keyCode == 13) {
            $('#btnSearch').trigger('click');
        }
    });

});
