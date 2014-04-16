$(function () {
    var __pageWidth = $(window).width();
    var cls_hotSale = [];

    if (__pageWidth < 600) {
        $('.container-fluid').attr('style', 'padding:0px 5px;');
        $('.container-fluid .row-fluid:first-child').css('margin-top', '10px');
        $('.widget-box').attr('style', 'margin-top: 0px;');
        $($('.widget-title').find('h5')).html('');
        //定义套餐列表的列
        cls_hotSale = [[
            { field: 'ChannelCity', title: '渠道城市', width: 40, sortable: true },
            { field: 'ProductName', title: '产品名', width: 110, sortable: true, formatter: function (value, rowData, rowIndex) {
                return '<a href="/HotSale/HotSaleDetial?hid=' + rowData.HotSaleId + '">' + value + '</a>';
            }
            }
        ]];
    } else if (__pageWidth < 1280 && __pageWidth >= 600) {
        //定义套餐列表的列
        cls_hotSale = [[
            { field: 'ChannelCity', title: '渠道城市', width: 40, sortable: true },
            { field: 'ProductName', title: '产品名', width: 100, sortable: true, formatter: function (value, rowData, rowIndex) {
                return '<a href="/HotSale/HotSaleDetial?hid=' + rowData.HotSaleId + '">' + value + '</a>';
            }
            },
            { field: 'City', title: '城市', width: 30, sortable: true, formatter: function (value, rowData, rowIndex) {
                if (rowData.City) {
                    return rowData.City.CityName;
                } else {
                    return "暂无";
                }
            }
            },
            { field: 'TuanPrice', title: '价格', width: 25, sortable: true },
            { field: 'SaleVolume', title: '销量', width: 25, sortable: true }
        ]];
    } else if (__pageWidth >= 1280) {
        //定义套餐列表的列
        cls_hotSale = [[
            { field: 'HotSaleId', title: '编号', width: 20, sortable: true, hidden: true },
            { field: 'ChannelCity', title: '渠道城市', width: 30, sortable: true },
            { field: 'ProductName', title: '产品名', width: 120, sortable: true, formatter: function (value, rowData, rowIndex) {
                return '<a href="/HotSale/HotSaleDetial?hid=' + rowData.HotSaleId + '">' + value + '</a>';
            }
            },
            { field: 'City', title: '产品所在城市', width: 30, sortable: true, formatter: function (value, rowData, rowIndex) {
                if (rowData.City) {
                    return rowData.City.CityName;
                } else {
                    return "暂无";
                }
            }
            },
            { field: 'TuanPrice', title: '价格', width: 20, sortable: true },
            { field: 'SaleVolume', title: '销量', width: 20, sortable: true },
            { field: 'Url', title: '原站链接', align: 'center', width: 25, sortable: true, formatter: function (value, rowData, rowIndex) {
                return '<a target="_blank" href="' + value + '">原站链接</a>'
            }
            },
			{ field: 'CreateDate', title: '录入时间', width: 45, sortable: true, formatter: function (value, rowData, rowIndex) {
                try {
                    return Date.parse(value).toString('yyyy-MM-dd');
                }
                catch (err) {
                    return '--';
                }
            }
            },
            { field: 'StartDate', title: '开始时间', width: 45, sortable: true, formatter: function (value, rowData, rowIndex) {
                try {
                    return Date.parse(value).toString('yyyy-MM-dd');
                }
                catch (err) {
                    return '--';
                }
            }
            },
            { field: 'EndDate', title: '结束时间', width: 45, sortable: true, formatter: function (value, rowData, rowIndex) {
                try {
                    return Date.parse(value).toString('yyyy-MM-dd');
                }
                catch (err) {
                    return '--';
                }
            }
            }
        ]];
    }

    $('#divTable').height($('#content').height() / 1.3);

    var loadGrid = function (opts) {
        ycf_crm.loadGrid($('#tb_hotSale'), {
            title: null,
            url: opts.url,
            loadMsg: 'Data Loading...',
            queryParams: opts.pars,
            sortName: 'HotSaleId',
            fit: true,
            fitColumns: true,
            striped: true,
            height: 400,
            pageSize: 15,
            idField: 'HotSaleId',
            columns: cls_hotSale,
            pagination: true,
            rownumbers: true,
            singleSelect: true,
            toolbar: null
        }, null, null, null);

    };

    var options = {};
    options.url = "/HotSale/GetHotSaleList";
    try {
        loadGrid(options);
    } catch (e) {
        ycf_crm.errorHandler();
    }


    $('#btnSearch').click(function () {
        try {
            options.url = "/HotSale/SearchHotSaleByProductName";
            options.pars = {
                'keyWord': $('#txtSearch').val()
            };
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
