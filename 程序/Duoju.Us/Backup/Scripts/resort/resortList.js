$(function () {
    var __pageWidth = $(window).width();
    var cls_resort = [];

    if (__pageWidth < 600) {
        $('.container-fluid').attr('style', 'padding:0px 5px;');
        $('.widget-box').attr('style', 'margin-top: 0px;');
        $('.widget-title').attr('style', 'height:auto;padding-top: 5px;');
        $('.div_search').attr('style', 'position:static;margin-top:3px;margin-left:35px;');
        cls_resort = [
            [
                {
                    field: 'ResortName',
                    title: '酒店名',
                    width: 120,
                    sortable: true,
                    formatter: function (value, rowData, rowIndex) {
                        return '<a class="a_detial" href="/Resort/ProductManager?sid=' + rowData.ResortId + '">' + value + '</a>';
                    }
                }
            ]
        ];
    } else if (__pageWidth < 1280 && __pageWidth >= 600) {
        $('.container-fluid').attr('style', 'padding:0px 5px;');
        $('.widget-title').attr('style', 'height:auto;padding-top: 5px;');
        $('.div_search').attr('style', 'position:static;margin-top:3px;margin-left:35px;');
        cls_resort = [
            [
                {
                    field: 'ResortName',
                    title: '酒店名',
                    width: 120,
                    sortable: true,
                    formatter: function (value, rowData, rowIndex) {
                        return '<a class="a_detial" href="/Resort/ProductManager?sid=' + rowData.ResortId + '">' + value + '</a>';
                    }
                },
                { field: 'Telephone', title: '电话', width: 60, sortable: true }
            ]
        ];
    } else if (__pageWidth >= 1280) {
        //定义套餐列表的列
        cls_resort = [
            [
                {
                    field: 'ck',
                    checkbox: true
                },
                { field: 'ResortId', title: '编号', width: 20, sortable: true },
                {
                    field: 'ResortName',
                    title: '酒店名',
                    width: 120,
                    sortable: true,
                    formatter: function (value, rowData, rowIndex) {
                        return '<a class="a_detial" href="/Resort/ProductManager?sid=' + rowData.ResortId + '">' + value + '</a>';
                    }
                },
                { field: 'Telephone', title: '电话', width: 60, sortable: true },
                { field: 'Fax', title: '传真', width: 60, sortable: true },
                { field: 'Address', title: '地址', width: 135, sortable: true },
                { field: 'CreatedBy', title: '创建人', width: 40, sortable: true },
                {
                    field: 'CreatedDate',
                    title: '创建时间',
                    width: 45,
                    sortable: true,
                    formatter: function (value, rowData, rowIndex) {
                        try {
                            return Date.parse(value).toString('yyyy-MM-dd');
                        } catch (err) {
                            return '--';
                        }
                    }
                },
                {
                    field: 'Detial',
                    title: '详情',
                    width: 25,
                    sortable: false,
                    formatter: function (value, rowData, rowIndex) {
                        return '<a class="a_Detial" href="/Resort/ProductManager?sid=' + rowData.ResortId + '">查看</a>';
                    }
                }
            ]
        ];
    }
    var loadGrid = function (opts) {
        ycf_crm.loadGrid($('#tb_resort'), {
            title: null,
            url: opts.url,
            loadMsg: 'Data Loading...',
            queryParams: opts.pars,
            sortName: 'ResortId',
            fit: true,
            fitColumns: true,
            striped: true,
            height: 400,
            pageSize: 10,
            idField: 'ResortId',
            columns: cls_resort,
            pagination: true,
            rownumbers: true,
            singleSelect: false,
            toolbar: null
        }, null, null, null);
    };
    $('#divTable').height($('#content').height() / 1.5);

    var options = {};
    options.pars = {
        'keyWord': $('#txtResortName').val(),
        'rid': $('#txtResortId').val()
    };
    options.url = "/Resort/GetResortListBySearch";
    try {
        loadGrid(options);
    } catch (e) {
        ycf_crm.errorHandler();
    }

    $('#btnSearch').click(function () {
        options.pars = {
            'keyWord': $('#txtResortName').val(),
            'rid': $('#txtResortId').val()
        };
        try {
            $('#tb_resort').datagrid('clearSelections');
            loadGrid(options);
        } catch (e) {
            ycf_crm.errorHandler();
        }
    });

});