//namespace YCF_Admin, this is the common js
window.ycf_crm = window.ycf_crm || {};

ycf_crm.loadGrid = function (objTable, opts, fnCellClickCallBack, fnOnDbClickRow, evelEvent) {
    var _opts = {
        title: opts.title || '',
        fitColumns: opts.fitColumns || false,
        fit: opts.fit || false,
        nowrap: opts.nowrap || false,
        striped: opts.striped || false,
        url: opts.url || '',
        queryParams: opts.queryParams || '',
        loadMsg: opts.loadMsg || 'Loading...',
        sortName: opts.sortName || 'ID',
        sortOrder: opts.sortOrder || 'asc',
        remoteSort: opts.remoteSort || false,
        height: 500,
        idField: opts.idField || 'ID',
        columns: opts.columns || null,
        toolbar: opts.toolbar || null,
        pagination: opts.pagination || false,
        pageNumber: opts.pageNumber || 1,
        pageSize: opts.pageSize || 20,
        pageList: opts.pageList || [5, 10, 15, 20, 25, 50],
        singleSelect: opts.singleSelect || false,
        rownumbers: opts.rownumbers || false,
        remoteSort: true,
        onBeforeEdit: function (index, row) {
            row.editing = true;
            objTable.datagrid('refreshRow', index);
        },
        onAfterEdit: function (index, row) {
            row.editing = false;
            objTable.datagrid('refreshRow', index);
        },
        onCancelEdit: function (index, row) {
            row.editing = false;
            objTable.datagrid('refreshRow', index);
        },
        onSelect: function (rowIndex, rowData) {
            $($(this).parents()
                .find('.datagrid-body input:checkbox')[rowIndex])
                .closest('.checker > span')
                .addClass('checked');
        },
        onUnselect: function (rowIndex, rowData) {
            $($(this).parents()
            .find('.datagrid-body input:checkbox')[rowIndex])
            .closest('.checker > span')
            .removeClass('checked');
        },
        onLoadSuccess: function (data) {
            $('input[type=checkbox],input[type=radio],input[type=file]').uniform();
            $(".datagrid-header-check input:checkbox").click(function () {
                var checkedStatus = this.checked;
                var checkbox = $(this).parents().find('.datagrid-body input:checkbox');
                checkbox.each(function () {
                    this.checked = checkedStatus;
                    if (checkedStatus == this.checked) {
                        $(this).closest('.checker > span').removeClass('checked');
                    }
                    if (this.checked) {
                        $(this).closest('.checker > span').addClass('checked');
                    }
                });
            });
            if (evelEvent) {
                var fnStr = ycf_crm.getEvel(evelEvent);
                eval(fnStr);
            }
        }
    };
    if (typeof fnCellClickCallBack === 'function') {
        _opts.onClickCell = fnCellClickCallBack;
    }
    if (typeof fnOnDbClickRow === 'function') {
        _opts.onDblClickRow = fnOnDbClickRow;
    }
    objTable.datagrid(_opts);
};

ycf_crm.logout = function (fnBeforeSend, fnSuccess, fnError) {
    $.ajax({
        url: "/Account/Logout",
        type: "POST",
        dataType: "json",
        cache: false,
        beforeSend: function (jqXHR, settings) {
            if (fnBeforeSend) {
                fnBeforeSend.call(fnBeforeSend, jqXHR, settings);
            }
        },
        success: function (data) {
            fnSuccess.call(fnSuccess, data);
        },
        error: function (e) {
            fnError.call(fnError, e);
        }
    });
};

ycf_crm.getMenu = function (fnBeforeSend, fnSuccess, fnError) {
    $.ajax({
        url: "/Home/GetMenu",
        type: "POST",
        dataType: "json",
        cache: true,
        beforeSend: function (jqXHR, settings) {
            if (fnBeforeSend) {
                fnBeforeSend.call(fnBeforeSend, jqXHR, settings);
            }
        },
        success: function (data) {
            fnSuccess.call(fnSuccess, data);
        },
        error: function (e) {
            fnError.call(fnError, e);
        }
    });
};

ycf_crm.getActionByGidAndUid = function (gid, uid, fnBeforeSend, fnSuccess, fnError) {
    $.ajax({
        url: "/Home/GetActionByGidAndUid",
        type: "POST",
        dataType: "json",
        cache: true,
        data: {
            gid: gid,
            uid: uid
        },
        beforeSend: function (jqXHR, settings) {
            if (fnBeforeSend) {
                fnBeforeSend.call(fnBeforeSend, jqXHR, settings);
            }
        },
        success: function (data) {
            fnSuccess.call(fnSuccess, data);
        },
        error: function (e) {
            fnError.call(fnError, e);
        }
    });
};

ycf_crm.getAllAction = function (fnBeforeSend, fnSuccess, fnError) {
    $.ajax({
        async: false,
        url: "/Home/GetAllAction",
        type: "POST",
        dataType: "json",
        cache: true,
        beforeSend: function (jqXHR, settings) {
            if (fnBeforeSend) {
                fnBeforeSend.call(fnBeforeSend, jqXHR, settings);
            }
        },
        success: function (data) {
            fnSuccess.call(fnSuccess, data);
        },
        error: function (e) {
            fnError.call(fnError, e);
        }
    });
};

ycf_crm.deleteSupplierInfo = function (sid, fnBeforeSend, fnSuccess, fnError) {
    $.ajax({
        url: "/Supplier/DeleteSupplierInfo",
        type: "POST",
        data: {
            sid: sid
        },
        dataType: "json",
        cache: false,
        beforeSend: function (jqXHR, settings) {
            if (fnBeforeSend) {
                fnBeforeSend.call(fnBeforeSend, jqXHR, settings);
            }
        },
        success: function (data) {
            fnSuccess.call(fnSuccess, data);
        },
        error: function (e) {
            fnError.call(fnError, e);
        }
    });
};

ycf_crm.deleteSupplierList = function (sidList, fnBeforeSend, fnSuccess, fnError) {
    $.ajax({
        url: "/Supplier/DeleteSupplierInfo",
        type: "POST",
        data: {
            sidList: sidList
        },
        dataType: "json",
        cache: false,
        beforeSend: function (jqXHR, settings) {
            if (fnBeforeSend) {
                fnBeforeSend.call(fnBeforeSend, jqXHR, settings);
            }
        },
        success: function (data) {
            fnSuccess.call(fnSuccess, data);
        },
        error: function (e) {
            fnError.call(fnError, e);
        }
    });
};

ycf_crm.getTaskList = function (fnBeforeSend, fnSuccess, fnError) {
    $.ajax({
        url: "/Task/GetTaskByUserId",
        type: "POST",
        dataType: "json",
        cache: true,
        beforeSend: function (jqXHR, settings) {
            if (fnBeforeSend) {
                fnBeforeSend.call(fnBeforeSend, jqXHR, settings);
            }
        },
        success: function (data) {
            fnSuccess.call(fnSuccess, data);
        },
        error: function (e) {
            fnError.call(fnError, e);
        }
    });
};

ycf_crm.readTask = function (tid, sid, fnBeforeSend, fnSuccess, fnError) {
    $.ajax({
        url: "/Task/ReadTask",
        type: "POST",
        dataType: "json",
        data: {
            tid: tid,
            sid: sid
        },
        cache: false,
        beforeSend: function (jqXHR, settings) {
            if (fnBeforeSend) {
                fnBeforeSend.call(fnBeforeSend, jqXHR, settings);
            }
        },
        success: function (data) {
            fnSuccess.call(fnSuccess, data);
        },
        error: function (e) {
            fnError.call(fnError, e);
        }
    });
};

ycf_crm.getSupplierInWeek = function (fnBeforeSend, fnSuccess, fnError) {
    $.ajax({
        url: "/Supplier/GetSupplierInWeek",
        type: "POST",
        dataType: "json",
        cache: true,
        beforeSend: function (jqXHR, settings) {
            if (fnBeforeSend) {
                fnBeforeSend.call(fnBeforeSend, jqXHR, settings);
            }
        },
        success: function (data) {
            fnSuccess.call(fnSuccess, data);
        },
        error: function (e) {
            fnError.call(fnError, e);
        }
    });
};

ycf_crm.getCurrentUser = function (fnBeforeSend, fnSuccess, fnError) {
    $.ajax({
        url: "/Account/GetCurrentUser",
        type: "POST",
        dataType: "json",
        cache: true,
        beforeSend: function (jqXHR, settings) {
            if (fnBeforeSend) {
                fnBeforeSend.call(fnBeforeSend, jqXHR, settings);
            }
        },
        success: function (data) {
            fnSuccess.call(fnSuccess, data);
        },
        error: function (e) {
            fnError.call(fnError, e);
        }
    });
};

ycf_crm.followSupplier = function (sid, fnBeforeSend, fnSuccess, fnError) {
    $.ajax({
        url: "/Supplier/FollowSupplier",
        type: "POST",
        dataType: "json",
        cache: false,
        data: {
            sid: sid
        },
        beforeSend: function (jqXHR, settings) {
            if (fnBeforeSend) {
                fnBeforeSend.call(fnBeforeSend, jqXHR, settings);
            }
        },
        success: function (data) {
            fnSuccess.call(fnSuccess, data);
        },
        error: function (e) {
            fnError.call(fnError, e);
        }
    });
};

ycf_crm.releaseSupplier = function (sid, fnBeforeSend, fnSuccess, fnError) {
    $.ajax({
        url: "/Supplier/ReleaseSupplier",
        type: "POST",
        dataType: "json",
        cache: false,
        data: {
            sid: sid
        },
        beforeSend: function (jqXHR, settings) {
            if (fnBeforeSend) {
                fnBeforeSend.call(fnBeforeSend, jqXHR, settings);
            }
        },
        success: function (data) {
            fnSuccess.call(fnSuccess, data);
        },
        error: function (e) {
            fnError.call(fnError, e);
        }
    });
};

ycf_crm.getSupplierByName = function (name, fnBeforeSend, fnSuccess, fnError) {
    $.ajax({
        url: "/Supplier/CheckSupplierByName",
        type: "POST",
        dataType: "json",
        cache: false,
        data: {
            name: name
        },
        beforeSend: function (jqXHR, settings) {
            if (fnBeforeSend) {
                fnBeforeSend.call(fnBeforeSend, jqXHR, settings);
            }
        },
        success: function (data) {
            fnSuccess.call(fnSuccess, data);
        },
        error: function (e) {
            fnError.call(fnError, e);
        }
    });
};

ycf_crm.getFollowState = function (fnBeforeSend, fnSuccess, fnError) {
    $.ajax({
        url: "/FollowLog/GetFollowState",
        type: "POST",
        dataType: "json",
        cache: false,
        beforeSend: function (jqXHR, settings) {
            if (fnBeforeSend) {
                fnBeforeSend.call(fnBeforeSend, jqXHR, settings);
            }
        },
        success: function (data) {
            fnSuccess.call(fnSuccess, data);
        },
        error: function (e) {
            fnError.call(fnError, e);
        }
    });
};

ycf_crm.getArea = function (fnBeforeSend, fnSuccess, fnError) {
    $.ajax({
        url: "/Position/GeAllAreas",
        type: "POST",
        dataType: "json",
        cache: true,
        beforeSend: function (jqXHR, settings) {
            if (fnBeforeSend) {
                fnBeforeSend.call(fnBeforeSend, jqXHR, settings);
            }
        },
        success: function (data) {
            fnSuccess.call(fnSuccess, data);
        },
        error: function (e) {
            fnError.call(fnError, e);
        }
    });
};

ycf_crm.getProvinceByAreaId = function (aId, fnBeforeSend, fnSuccess, fnError) {
    $.ajax({
        url: "/Position/GetProvinceByAreaId",
        type: "POST",
        dataType: "json",
        cache: true,
        data: {
            areaId: aId
        },
        beforeSend: function (jqXHR, settings) {
            if (fnBeforeSend) {
                fnBeforeSend.call(fnBeforeSend, jqXHR, settings);
            }
        },
        success: function (data) {
            fnSuccess.call(fnSuccess, data);
        },
        error: function (e) {
            fnError.call(fnError, e);
        }
    });
};

ycf_crm.getProvince = function (fnBeforeSend, fnSuccess, fnError) {
    $.ajax({
        url: "/Position/GetAllProvince",
        type: "POST",
        dataType: "json",
        cache: true,
        beforeSend: function (jqXHR, settings) {
            if (fnBeforeSend) {
                fnBeforeSend.call(fnBeforeSend, jqXHR, settings);
            }
        },
        success: function (data) {
            fnSuccess.call(fnSuccess, data);
        },
        error: function (e) {
            fnError.call(fnError, e);
        }
    });
};

ycf_crm.getCityByProvinceCode = function (code, fnBeforeSend, fnSuccess, fnError) {
    $.ajax({
        url: "/Position/GetCityByProvinceCode",
        type: "POST",
        dataType: "json",
        cache: true,
        data: {
            code: code
        },
        beforeSend: function (jqXHR, settings) {
            if (fnBeforeSend) {
                fnBeforeSend.call(fnBeforeSend, jqXHR, settings);
            }
        },
        success: function (data) {
            fnSuccess.call(fnSuccess, data);
        },
        error: function (e) {
            fnError.call(fnError, e);
        }
    });
};

ycf_crm.getDistrictByCityCode = function (code, fnBeforeSend, fnSuccess, fnError) {
    $.ajax({
        url: "/Position/GetDistrictByCityCode",
        type: "POST",
        dataType: "json",
        cache: true,
        data: {
            code: code
        },
        beforeSend: function (jqXHR, settings) {
            if (fnBeforeSend) {
                fnBeforeSend.call(fnBeforeSend, jqXHR, settings);
            }
        },
        success: function (data) {
            fnSuccess.call(fnSuccess, data);
        },
        error: function (e) {
            fnError.call(fnError, e);
        }
    });
};

ycf_crm.getScenicsMappingByResortId = function (resortId, fnSuccess, fnError) {
    $.ajax({
        url: "/Position/GetScenicsMappingByResortId",
        type: "POST",
        dataType: "json",
        cache: true,
        data: {
            resortId: resortId
        },
        success: function (data) {
            fnSuccess.call(fnSuccess, data);
        },
        error: function (e) {
            fnError.call(fnError, e);
        }
    });
};

ycf_crm.getThemesMappingByResortId = function (resortId, fnSuccess, fnError) {
    $.ajax({
        url: "/Position/GetThemesMappingByResortId",
        type: "POST",
        dataType: "json",
        cache: true,
        data: {
            resortId: resortId
        },
        success: function (data) {
            fnSuccess.call(fnSuccess, data);
        },
        error: function (e) {
            fnError.call(fnError, e);
        }
    });
};

ycf_crm.getScenicsByCityCode = function (code, fnSuccess, fnError) {
    $.ajax({
        url: "/Position/GetScenicsByCityCode",
        type: "POST",
        dataType: "json",
        cache: true,
        data: {
            code: code
        },
        success: function (data) {
            fnSuccess.call(fnSuccess, data);
        },
        error: function (e) {
            fnError.call(fnError, e);
        }
    });
};

ycf_crm.getThemeByCityCode = function (code, fnSuccess, fnError) {
    $.ajax({
        url: "/Position/GetThemeByCityCode",
        type: "POST",
        dataType: "json",
        cache: true,
        data: {
            code: code
        },
        success: function (data) {
            fnSuccess.call(fnSuccess, data);
        },
        error: function (e) {
            fnError.call(fnError, e);
        }
    });
};

ycf_crm.getSimpleUserInfoListByGroupId = function (fnBeforeSend, fnSuccess, fnError) {
    $.ajax({
        url: "/Account/GetSimpleUserInfoListByGroupId",
        type: "POST",
        dataType: "json",
        cache: true,
        beforeSend: function (jqXHR, settings) {
            if (fnBeforeSend) {
                fnBeforeSend.call(fnBeforeSend, jqXHR, settings);
            }
        },
        success: function (data) {
            fnSuccess.call(fnSuccess, data);
        },
        error: function (e) {
            fnError.call(fnError, e);
        }
    });
};

ycf_crm.getImageListByResortId = function (resortId, fnSuccess, fnError) {
    $.ajax({
        url: "/Image/GetImageListByResortId",
        type: "POST",
        dataType: "json",
        cache: true,
        data: {
            resortId: resortId
        },
        success: function (data) {
            fnSuccess.call(fnSuccess, data);
        },
        error: function (e) {
            fnError.call(fnError, e);
        }
    });
};

ycf_crm.postImageSort = function (resortId, imageCategoryId, imgIdListStr, fnSuccess, fnError) {
    $.ajax({
        url: "/Image/PostImageSort",
        type: "POST",
        dataType: "json",
        cache: true,
        data: {
            resortId: resortId,
            imageCategoryId: imageCategoryId,
            imgIdListStr: imgIdListStr
        },
        success: function (data) {
            fnSuccess.call(fnSuccess, data);
        },
        error: function (e) {
            fnError.call(fnError, e);
        }
    });
};

ycf_crm.updateImageTitle = function (imgId, title, fnSuccess, fnError) {
    $.ajax({
        url: "/Image/UpdateImageTitle",
        type: "POST",
        dataType: "json",
        cache: true,
        data: {
            imgId: imgId,
            title: title
        },
        success: function (data) {
            fnSuccess.call(fnSuccess, data);
        },
        error: function (e) {
            fnError.call(fnError, e);
        }
    });
};

ycf_crm.updateResortInfoContent = function (resortId, content, contentType, fnSuccess, fnError) {
    $.ajax({
        url: "/Resort/UpdateResortContent",
        type: "POST",
        dataType: "json",
        cache: true,
        data: {
            resortId: resortId,
            content: content,
            contentType: contentType
        },
        success: function (data) {
            fnSuccess.call(fnSuccess, data);
        },
        error: function (e) {
            fnError.call(fnError, e);
        }
    });
};

ycf_crm.updateResortTitleInfo = function (resortId, mainTitle, subTitle, appMainTitle, appSubTitle, fnSuccess, fnError) {
    $.ajax({
        url: "/Resort/UpdateResortTitleInfo",
        type: "POST",
        dataType: "json",
        cache: true,
        data: {
            resortId: resortId,
            mainTitle: mainTitle,
            subTitle: subTitle,
            appMainTitle: appMainTitle,
            appSubTitle: appSubTitle
        },
        success: function (data) {
            fnSuccess.call(fnSuccess, data);
        },
        error: function (e) {
            fnError.call(fnError, e);
        }
    });
};

ycf_crm.deleteImage = function (imgId, fnSuccess, fnError) {
    $.ajax({
        url: "/Image/DeleteImage",
        type: "POST",
        dataType: "json",
        cache: true,
        data: {
            imgId: imgId
        },
        success: function (data) {
            fnSuccess.call(fnSuccess, data);
        },
        error: function (e) {
            fnError.call(fnError, e);
        }
    });
};

ycf_crm.stringFormat = function () {
    var s = arguments[0];
    for (var i = 0; i < arguments.length - 1; i++) {
        var reg = new RegExp("\\{" + i + "\\}", "gm");
        s = s.replace(reg, arguments[i + 1]);
    }
    return s;
};

ycf_crm.getEvel = function (fn) {
    var entire = fn.toString();
    var body = entire.substring(entire.indexOf("{") + 1, entire.lastIndexOf("}"));
    return body;
};

ycf_crm.errorHandler = function (errorCode) {
    if (!isNaN(errorCode)) {
        document.location.href = "/error?errorCode=" + errorCode;
    } else {
        document.location.href = "/error";
    }
};