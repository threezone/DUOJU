$(function () {
    var resortId = $.query.get('sid');

    var _init = true;

    var initScenics = function() {
        ycf_crm.getScenicsByCityCode($('#cityCode').val(), function(data) {
            var _data = [];
            $.each(data, function(i) {
                var _d = {};
                _d.id = data[i].ScenicId;
                _d.text = data[i].Name;
                _data.push(_d);
            });
            $('#selectScenic').select2({
                tags: _data,
                multiple: true
            });
            var _sdate = [];
            var _initDate = '';
            ycf_crm.getScenicsMappingByResortId(resortId, function(sdata) {
                $.each(sdata, function(j) {
                    _sdate.push(sdata[j].ScenicId);
                    _initDate += sdata[j].ScenicId + ';';
                });
                $('#selectScenic').select2('val', _sdate);
                $('#scenicIds').val(_initDate);
            }, function(e) {
                console.log(e);
            });
        }, function(e) {
            console.log(e);
        });
    };
    initScenics();

    var initThemes = function () {
        ycf_crm.getThemeByCityCode($('#cityCode').val(), function (data) {
            var _data = [];
            $.each(data, function (i) {
                var _d = {};
                _d.id = data[i].ThemeId;
                _d.text = data[i].Name;
                _data.push(_d);
            });
            $('#selectTheme').select2({
                tags: _data,
                multiple: true
            });
            var _sdate = [];
            var _initDate = '';
            ycf_crm.getThemesMappingByResortId(resortId, function (sdata) {
                $.each(sdata, function (j) {
                    _sdate.push(sdata[j].ThemeId);
                    _initDate += sdata[j].ThemeId + ';';
                });
                $('#selectTheme').select2('val', _sdate);
                $('#themeIds').val(_initDate);
            }, function (e) {
                console.log(e);
            });
        }, function (e) {
            console.log(e);
        });
    };
    initThemes();

    $('#selectScenic').change(function () {
        var selections = $('#selectScenic').select2('data');
        //console.log('Selected options: ' + selections);
        var selects = '';
        for (var i = 0; i < selections.length; i++) {
            selects += selections[i].id+';';
        }
        $('#scenicIds').val(selects);
    });

    $('#selectTheme').change(function () {
        var selections = $('#selectTheme').select2('data');
        //console.log('Selected options: ' + selections);
        var selects = '';
        for (var i = 0; i < selections.length; i++) {
            selects += selections[i].id + ';';
        }
        $('#themeIds').val(selects);
    });

    ycf_crm.getArea(null, function (data) {
        $.each(data, function (i) {
            $('#selectArea').append('<option text="' + data[i].Name + '" value="' + data[i].AreaId + '">' + data[i].PinYinNameAbbr + '-' + data[i].Name + '</option>');
        });
        if ($('#hidAid').val() != 0 && $('#hidAid').val()) {
            $("#selectArea").select2("val", $('#hidAid').val());
            $("#selectArea").trigger("change");
        }
    }, function (e) {
        console.log(e);
    });

    $('#selectArea').change(function () {
        var selectOption = $(this).find("option:selected");
        var aId = $(selectOption).attr('value');
        $('#selectProvince').html('<option value="0">请选择省份</option>');
        $("#selectProvince").select2("val", "0");
        ycf_crm.getProvinceByAreaId(aId, null, function (data) {
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
    });

    $('#selectProvince').change(function () {
        var selectOption = $(this).find("option:selected");
        var code = $(selectOption).attr('code');
        $('#selectCity').html('<option value="0">请选择城市</option>');
        $("#selectCity").select2("val", "0");
        $('#cityCode').val("0")
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
        $('#cityCode').val(code);
        $('#selectDistict').html('<option value="0">请选择区域</option>');
        $("#selectDistict").select2("val", "0");
        ycf_crm.getDistrictByCityCode(code, null, function (data) {
            $.each(data, function (i) {
                $('#selectDistict').append('<option text="' + data[i].DistrictName + '" code="' + data[i].DistrictCode + '" value="' + data[i].DistrictId + '">' + data[i].PinYinNameAbbr + '-' + data[i].DistrictName + '</option>');
            });
            if ($('#hidDid').val() != 0 && $('#hidDid').val()) {
                $("#selectDistict").select2("val", $('#hidDid').val());
            }
            if (!_init) {
                initScenics()
            } else {
                _init = false;
            };
        }, function (e) {
            console.log(e);
        });
    });

    $('#btnSaveEditPosition').click(function () {
        if ($('#selectProvince').val() == 0) {
            alert('请选择省份！');
            return false;
        }
        if ($('#selectCity').val() == 0) {
            alert('请选择城市！');
            return false;
        }
        $('#spanAreaName').html($('#selectArea').find('option:selected').attr('text'));
        $('#hidAid').val($('#selectArea').find('option:selected').val());
        $('#spanProvinceName').html($('#selectProvince').find('option:selected').attr('text'));
        $('#hidPid').val($('#selectProvince').find('option:selected').val());
        $('#spanCityName').html($('#selectCity').find('option:selected').attr('text'));
        $('#hidCid').val($('#selectCity').find('option:selected').val());
        $('#spanDistrictName').html($('#selectDistict').find('option:selected').attr('text'));
        $('#hidDid').val($('#selectDistict').find('option:selected').val());
        $('#modal-container-449303').modal('hide');
    });

    $('.checker').live('click', function () {
        if ($(this).find("input[type='checkbox']").attr("value").toLowerCase() == 'false') {
            $(this).find("input[type='checkbox']").attr("value", 'true');
        } else {
            $(this).find("input[type='checkbox']").attr("value", 'false');
        }
    });

    $('input[type=checkbox],input[type=radio],input[type=file]').uniform();
    $('select').select2();

    $('#basic_validate').ajaxForm({
        dataType: 'json',
        success: function (data) {
            $('#myModal').modal();
        }
    });

    var checkin = $('#start').datepicker({
        format: "yyyy-mm-dd",
        language: "zh-CN"
    })
    .on('changeDate', function (ev) {
        if (ev.date.valueOf() > checkout.date.valueOf()) {
            var newDate = new Date(ev.date);
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


    var saleStart = $('#saleStart').datepicker({
        format: "yyyy-mm-dd",
        language: "zh-CN"
    })
    .on('changeDate', function (ev) {
        if (ev.date.valueOf() > checkout.date.valueOf()) {
            var newDate = new Date(ev.date);
            newDate.setDate(newDate.getDate() + 1);
            saleEnd.setValue(newDate);
        }
        saleStart.hide();
        $('#saleEnd')[0].focus();
    }).data('datepicker');

    var saleEnd = $('#saleEnd').datepicker({
        format: "yyyy-mm-dd",
        language: "zh-CN",
        onRender: function (date) {
            return date.valueOf() <= saleStart.date.valueOf() ? 'disabled' : '';
        }
    }).on('changeDate', function (ev) {
        saleEnd.hide();
    }).data('datepicker');

    $('.thumbnails').sortable();

    //定义套餐列表的列
    var cls_roomList = [[
        { field: 'ThumImg', title: '房型缩略图', width: 20, align: 'center', formatter: function (value, rowData, rowIndex) {
            return '<img style="margin:5px 0;" src="/upload/hl_5_r_1456_1.jpg" />';
        }
        },
        { field: 'RoomId', title: '房型编号', width: 20, sortable: true },
        { field: 'IsActive', title: '是否激活', width: 10, align: 'center', formatter: function (value, rowData, rowIndex) {
            if (value) {
                return "是";
            } else {
                return "否";
            }
        }
        },
        { field: 'Name', title: '房型名', width: 60 },
        { field: 'StartDate', title: '开始时间', width: 40, sortable: true },
        { field: 'EndDate', title: '结束时间', width: 40, sortable: true },
        { field: 'CreatedBy', title: '创建人', width: 30 },
        { field: 'EditRoom', title: '编辑', width: 10, align: 'center', formatter: function (value, rowData, rowIndex) {
            return '<a class="a_release" roomId="' + rowData.RoomId + '" href="javascript:void(0);">编辑</a>';
        }
        }
    ]];

    var loadGrid = function (opts) {
        ycf_crm.loadGrid($('#tbRoomList'), {
            title: null,
            url: opts.url,
            loadMsg: 'Data Loading...',
            queryParams: opts.pars,
            sortName: 'RoomId',
            fit: true,
            fitColumns: true,
            striped: true,
            height: 400,
            pageSize: 15,
            idField: 'RoomId',
            columns: cls_roomList,
            pagination: false,
            rownumbers: false,
            singleSelect: true,
            toolbar: null
        }, null, null, null);

    };

    var options = {};
    try {
        options.url = "/Resort/GetRoomListByResortId";
        options.pars = {
            'rid': resortId
        };
        loadGrid(options);
    } catch (e) {
        ycf_crm.errorHandler();
    }

    var sort = 1;
    var listSort = 1;
    ycf_crm.getImageListByResortId(resortId, function (data) {
        $.each(data, function (i) {
            var img = data[i].Url.substr(data[i].Url.lastIndexOf('/') + 1);
            if (data[i].ImageCategoryId == 13) {
                sort++;
                $('#divBroadcasting').append('<li imgId="' + data[i].ImageId + '" class="span5"><a href="#" class="thumbnail">' +
                    '<img src="../upload/thumb/' + img + '" alt="' + data[i].Title + '">' +
                    '<input type="text" style="width:50%;margin-top:10px;" placeholder="标题(选填)" value="' + data[i].Title + '" />' +
                    '<span class="btn btn-success btn-mini btnFileUpdate">更新</span>' +
                    '<span delType="delBro" class="btn btn-danger btn-mini btnFileDelete">删除</span>' +
                    '</a></li>');
            } else {
                listSort++;
                $('#divPictureList').append('<li imgId="' + data[i].ImageId + '" class="span5"><a href="#" class="thumbnail">' +
                    '<img src="../upload/thumb/' + img + '" alt="' + data[i].Title + '">' +
                    '<input type="text" style="width:50%;margin-top:10px;" placeholder="标题(选填)" value="' + data[i].Title + '" />' +
                    '<span class="btn btn-success btn-mini btnFileUpdate">更新</span>' +
                    '<span delType="delList" class="btn btn-danger btn-mini btnFileDelete">删除</span>' +
                    '</a></li>');
            }
        });
    }, function (e) { });

    $('#btnSaveBroad').click(function () {
        var imgList = $('#divBroadcasting').find('li');
        var imgIdListStr = '';
        $.each(imgList, function (i) {
            imgIdListStr += $(imgList[i]).attr('imgId') + ',';
        });
        ycf_crm.postImageSort(resortId, 13, imgIdListStr, function (data) {
            $('#myModal').modal();
        }, function (e) {
        });
    });

    $('#btnSaveList').click(function () {
        var imgList = $('#divPictureList').find('li');
        var imgIdListStr = '';
        $.each(imgList, function (i) {
            imgIdListStr += $(imgList[i]).attr('imgId') + ',';
        });
        ycf_crm.postImageSort(resortId, 23, imgIdListStr, function (data) {
            $('#myModal').modal();
        }, function (e) {
        });
    });

    $('.btnFileUpdate').live('click', function () {
        var imgId = $(this).parent().parent().attr('imgId');
        var title = $(this).prev().val();
        ycf_crm.updateImageTitle(imgId, title, function (data) {
            $('#myModal').modal();
        }, function (e) { });
    });

    $('.btnFileDelete').live('click', function () {
        if (window.confirm('你确定要删除该图片？')) {
            var imgId = $(this).parent().parent().attr('imgId');
            var delType = $(this).attr('delType');
            $(this).parent().parent().remove();
            console.log(delType);
            ycf_crm.deleteImage(imgId, function (data) {
                if (delType == 'delBro') {
                    $('#btnSaveBroad').trigger("click");
                } else {
                    $('#btnSaveList').trigger("click");
                }
            }, function (e) { });
        }
    });

    $('#file_upload').uploadify({
        'auto': true,
        'buttonText': '请选择上传文件',
        'fileTypeDesc': '图片文件',
        'fileTypeExts': '*.gif; *.jpg; *.png; *.bmp; *.jpeg',
        'swf': "/Scripts/uploadify-v3.1/uploadify.swf",
        'uploader': '/Image/Upload',
        'multi': true,
        'onUploadStart': function (file) {
            $("#file_upload").uploadify("settings", 'formData', {
                sort: sort,
                resortId: resortId,
                imageCategory: 13,
                imageSizeType: 1,
                adminUserId: $('#hidAdminUserId').val()
            });
        },
        'onUploadSuccess': function (file, data, response) {
            sort += 1;
            var d = data.split('&');
            $('#divBroadcasting').append('<li imgId="' + d[0] + '" class="span5"><a href="#" class="thumbnail">' +
                                         '<img src="' + d[1] + '" alt="' + file.name + '">' +
                                         '<input type="text" style="width:50%;margin-top:10px;" placeholder="标题(选填)" value="" />' +
                                         '<span class="btn btn-success btn-mini btnFileUpdate">更新</span>' +
                                         '<span class="btn btn-danger btn-mini btnFileDelete">删除</span>' +
                                         '</a></li>');
        }
    });

    $('#file_upload_list').uploadify({
        'auto': true,
        'buttonText': '请选择上传文件',
        'fileTypeDesc': '图片文件',
        'fileTypeExts': '*.gif; *.jpg; *.png; *.bmp; *.jpeg',
        'swf': "/Scripts/uploadify-v3.1/uploadify.swf",
        'uploader': '/Image/Upload',
        'multi': true,
        'onUploadStart': function (file) {
            $("#file_upload_list").uploadify("settings", 'formData', {
                sort: listSort,
                resortId: resortId,
                imageCategory: 23,
                imageSizeType: 1,
                adminUserId: $('#hidAdminUserId').val()
            });
        },
        'onUploadSuccess': function (file, data, response) {
            sort += 1;
            var d = data.split('&');
            $('#divPictureList').append('<li imgId="' + d[0] + '"  class="span5"><a href="#" class="thumbnail">' +
                                         '<img src="' + d[1] + '" alt="' + file.name + '">' +
                                         '<input type="text" style="width:50%;margin-top:10px;" placeholder="标题(选填)" value="" />' +
                                         '<span class="btn btn-success btn-mini btnFileUpdate">更新</span>' +
                                         '<span class="btn btn-danger btn-mini btnFileDelete">删除</span>' +
                                         '</a></li>');
        }
    });

    var isFirstLoadEditor = true;
    var editPackageDetial, editBookNotice, editVendorInfo, editYcfComment, editRecommendTrip, editTrafficGuide;

    //Tab第一次点击时，加载编辑器
    $('#tab2li').click(function () {
        if (isFirstLoadEditor) {
            editPackageDetial = CKEDITOR.replace('ResortInfo.PackageDetail');
            editBookNotice = CKEDITOR.replace('ResortInfo.BookNotice');
            editVendorInfo = CKEDITOR.replace('ResortInfo.VendorInfo');
            editYcfComment = CKEDITOR.replace('ResortInfo.YcfComment');
            editRecommendTrip = CKEDITOR.replace('ResortInfo.RecommendTrip');
            editTrafficGuide = CKEDITOR.replace('ResortInfo.TrafficGuide');
            isFirstLoadEditor = false;
        }
    });

    $('.btnSaveEditor').click(function () {
        var editType = $(this).attr('editType');
        var content = '';
        switch (editType) {
            case "1":
                content = editPackageDetial.getData();
                break;
            case "2":
                content = editBookNotice.getData();
                break;
            case "3":
                content = editVendorInfo.getData();
                break;
            case "4":
                content = editYcfComment.getData();
                break;
            case "5":
                content = editRecommendTrip.getData();
                break;
            case "6":
                content = editTrafficGuide.getData();
                break;
        }
        ycf_crm.updateResortInfoContent(resortId, content, editType, function (data) {
            $('#myModal').modal();
        }, function (e) { });
    });

    $('#btnSaveTitleInfo').click(function () {
        alert(123);
        var mainTitle = $('#txtMainTitla').val();
        var subTitle = $('#txtSubTitle').val();
        var appMainTitle = $('#txtAppMainTitle').val();
        var appSubTitle = $('#txtAppSubTitle').val();
        ycf_crm.updateResortTitleInfo(resortId, mainTitle, subTitle, appMainTitle, appSubTitle, function (data) {
            $('#myModal').modal();
        }, function (e) { });
    });

    var map = geo_map();
    var coder = geo_geocoder();
    coder.GetGeoCoder($('#txtAddress').val(), function (r) {
        console.log(r);
        if ($('#txtLatitude').val() == '') {
            $('#txtLatitude').val(r.point.lat);
        }
        if ($('#txtLongitude').val() == '') {
            $('#txtLongitude').val(r.point.lng);
        }
        map.initMap('map_canvas', r.point, 'NORMAL_MAP', 15);
        map.setMapCenter(r.point);
        var marker = geo_marker();
        marker.initMarker(map.getMap(), r.point, true, true, null, 'marker', null, 1, 2);
        marker.setAnimation('DROP');
    });
});
    