$(document).ready(function () {
    $('#divTable').height($('#content').height() / 1.25);
    //定义跟进记录列表的列
    var cls_supplier = [[
        { field: 'FollowLogId', title: '编号', width: 20, hidden: true },
        { field: 'UserName', title: '跟进者', width: 20, align: 'center', formatter: function (value, rowData, rowIndex) {
            return rowData.UserInfo.Name;
        }
        },
        { field: 'SupplierName', title: '客户名', width: 60, sortable: true, formatter: function (value, rowData, rowIndex) {
            return rowData.SupplierInfo.SupplierName;
        }
        },
        { field: 'FollowState', title: '跟进进度', width: 60, sortable: true, formatter: function (value, rowData, rowIndex) {
            if (rowData.FollowStateInfo) {
                return rowData.FollowStateInfo.State;
            } else {
                return '--';
            }
        }
        },
        { field: 'FollowName', title: '跟进记录', width: 100, sortable: true },
        { field: 'CreateDate', title: '最新跟进时间', width: 40, sortable: true }
    ]];


    $('input[type=checkbox],input[type=radio],input[type=file]').uniform();
    $('select').select2();

    ycf_crm.getSimpleUserInfoListByGroupId(null, function (data) {
        $.each(data, function (i) {
            $('#selectFollowBy').append("<option value='" + data[i].CRMUserId + "'>" + data[i].Name + "</option>");
        });
        $("#selectFollowBy").select2("val", "");
    }, function (e) {
        console.log(e);
    });

    ycf_crm.getProvince(null, function (data) {
        $.each(data, function (i) {
            $('#selectProvince').append('<option text="' + data[i].ProvinceName + '" code="' + data[i].ProvinceCode + '" value="' + data[i].ProvinceId + '">' + data[i].PinYinNameAbbr + '-' + data[i].ProvinceName + '</option>');
        });
        $("#selectProvince").select2("val", "");
        $("#selectCity").select2("val", "");
        $("#selectDistict").select2("val", "");
    }, function (e) {
        console.log(e);
    });

    $('#selectProvince').change(function () {
        var selectOption = $(this).find("option:selected");
        var code = $(selectOption).attr('code');
        $('#selectCity').html('<option value="">请选择城市</option>');
        $("#selectCity").select2("val", "");
        ycf_crm.getCityByProvinceCode(code, null, function (data) {
            $.each(data, function (i) {
                $('#selectCity').append('<option text="' + data[i].CityName + '" code="' + data[i].CityCode + '" value="' + data[i].CityId + '">' + data[i].PinYinNameAbbr + '-' + data[i].CityName + '</option>');
            });
        }, function (e) {
            console.log(e);
        });
    });

    $('#selectCity').change(function () {
        var selectOption = $(this).find("option:selected");
        var code = $(selectOption).attr('code');
        $('#selectDistict').html('<option value="">请选择区域</option>');
        $("#selectDistict").select2("val", "");
        ycf_crm.getDistrictByCityCode(code, null, function (data) {
            $.each(data, function (i) {
                $('#selectDistict').append('<option text="' + data[i].DistrictName + '" code="' + data[i].DistrictCode + '" value="' + data[i].DistrictId + '">' + data[i].PinYinNameAbbr + '-' + data[i].DistrictName + '</option>');
            });
        }, function (e) {
            console.log(e);
        });
    });

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

    var loadGrid = function (opts) {
        var fn = function () {
            var rows = $('#tb_fllowLogGroup').datagrid("getRows"); //获取行的数据
            for (var i = 0; i < rows.length; i++) {
                var name = rows[i].UserInfo.Name;
                var rowspan = 0;
                for (var j = 0; j < rows.length; j++) {
                    if (name == rows[j].UserInfo.Name) {
                        rowspan++;
                    }
                }
                if (rowspan != 0) {
                    //mergeCells这个方法是合并单元格，index表示标示号就是第几行开始，field表示要合并的字段，rowspan合并行数，colspan:合并列
                    $('#tb_fllowLogGroup').datagrid('mergeCells', { index: i, field: 'UserName', rowspan: rowspan });
                    i = i + rowspan - 1;
                }
            }
        };
        ycf_crm.loadGrid($('#tb_fllowLogGroup'), {
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
            pagination: false,
            rownumbers: true,
            singleSelect: false,
            toolbar: null
        }, null, null, fn);
    };

    var options = {};
    options.url = "/FollowLog/FollowLogCollect";

    try {
        loadGrid(options);
    } catch (e) {
        ycf_crm.errorHandler();
    }

    $('#selectFollowBy').change(function () {
        options.pars = { start: $('#start').val(), end: $('#end').val(), userId: $("#selectFollowBy").val(), province: $('#selectProvince').val(), city: $('#selectCity').val(), district: $('#selectDistict').val() };
        loadGrid(options);
    });

    $('#btnFilterDate').click(function () {
        options.pars = { start: $('#start').val(), end: $('#end').val(), userId: $("#selectFollowBy").val(), province: $('#selectProvince').val(), city: $('#selectCity').val(), district: $('#selectDistict').val() };
        loadGrid(options);
    });

    $('#btnLoadPosition').click(function () {
        options.pars = { start: $('#start').val(), end: $('#end').val(), userId: $("#selectFollowBy").val(), province: $('#selectProvince').val(), city: $('#selectCity').val(), district: $('#selectDistict').val() };
        $('#modal-container-449304').modal('hide');
        loadGrid(options);
    });
});