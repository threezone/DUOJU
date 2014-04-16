$(function () {
    $('#divTable').height($('#content').height() / 1.3);

    var __pageWidth = $(window).width();
    var cls_user = [];

    if (__pageWidth < 600) {
        $('.container-fluid').attr('style', 'padding:0px 5px;');
        $('.widget-box').attr('style', 'margin-top: 0px;');
        //定义套餐列表的列
        cls_user = [[
            { field: 'UserName', title: '用户名', width: 60, sortable: true, formatter: function (value, rowData, rowIndex) {
                return rowData.AdminUserInfo.UserName;
            }
            },
            { field: 'UserState', title: '账户状态', width: 40, sortable: true, formatter: function (value, rowData, rowIndex) {
                if (rowData.AdminUserInfo.Status == 1) {
                    return "启用";
                } else {
                    return "禁用";
                }
            }
            },
            { field: 'Release', title: '权限', width: 25, sortable: false, formatter: function (value, rowData, rowIndex) {
                return '<a class="a_authority" gid="' + rowData.CrmUserInfo.GroupId + '" uid="' + rowData.CrmUserInfo.CRMUserId + '" href="javascript:void(0);">权限</a>';
            }
            }
        ]];
    } else {
        //定义套餐列表的列
        cls_user = [[
            { field: 'CRMUserId', title: '编号', width: 20, sortable: true, formatter: function (value, rowData, rowIndex) {
                return rowData.CrmUserInfo.CRMUserId;
            }
            },
            { field: 'LoginId', title: '登录账户', width: 60, sortable: true, formatter: function (value, rowData, rowIndex) {
                return rowData.AdminUserInfo.LoginId;
            }
            },
            { field: 'UserName', title: '用户名', width: 60, sortable: true, formatter: function (value, rowData, rowIndex) {
                return rowData.AdminUserInfo.UserName;
            }
            },
            { field: 'Email', title: '邮箱', width: 100, sortable: true, formatter: function (value, rowData, rowIndex) {
                return rowData.AdminUserInfo.Email;
            }
            },
            { field: 'UserState', title: '账户状态', width: 40, sortable: true, formatter: function (value, rowData, rowIndex) {
                if (rowData.AdminUserInfo.Status == 1) {
                    return "启用";
                } else {
                    return "禁用";
                }
            }
            },
            { field: 'Release', title: '权限', width: 25, sortable: false, formatter: function (value, rowData, rowIndex) {
                return '<a class="a_authority" gid="' + rowData.CrmUserInfo.GroupId + '" uid="' + rowData.CrmUserInfo.CRMUserId + '" href="javascript:void(0);">权限</a>';
            }
            }
        ]];
    }

    

    var loadGrid = function (opts) {
        var authorityEvent = function () {
            $('.a_authority').click(function () {
                var gid = $(this).attr('gid');
                var uid = $(this).attr('uid');
                var setting = {
                    view: {
                        selectedMulti: false
                    },
                    key: {
                        name: "ActionName"
                    },
                    check: {
                        enable: true
                    },
                    data: {
                        simpleData: {
                            enable: true
                        }
                    },
                    callback: {
                    //onCheck: onCheck
                }
            };
            $('#hidUserId').val(uid);
            var zNodes = [];
            ycf_crm.getAllAction(null, function (data) {
                zNodes = data;
            }, function () {
            });
            var createTree = function () {
                $.fn.zTree.init($("#treeAuthority"), setting, zNodes);
            };
            $('#hidCurrentAuthority').val('');
            $('#hidAddAuthority').val('');
            $('#hidRemoveAuthority').val('');
            ycf_crm.getActionByGidAndUid(gid, uid, null, function (data) {
                var currentAuthority = '';
                $.each(data, function (i) {
                    currentAuthority += data[i].id + ',' + (data[i].isGroupAuthority ? 't' : 'f') + ';';
                    $.each(zNodes, function (j) {
                        if (data[i].id == zNodes[j].id) {
                            zNodes[j].checked = true;
                            zNodes[j].open = true;
                        }
                    });
                });
                $('#hidCurrentAuthority').val(currentAuthority);
                createTree();
                $('#modal-authority').modal();
            }, null);
        });
    };
    ycf_crm.loadGrid($('#tb_user_authority'), {
        title: null,
        url: opts.url,
        loadMsg: 'Data Loading...',
        queryParams: opts.pars,
        sortName: 'CRMUserId',
        fit: true,
        fitColumns: true,
        striped: true,
        height: 400,
        pageSize: 10,
        idField: 'CRMUserId',
        columns: cls_user,
        pagination: false,
        rownumbers: true,
        singleSelect: true,
        toolbar: null
    }, null, null, authorityEvent);
};

var options = {};
options.url = "/Account/GetUserListByGroupId";

loadGrid(options);

$('#btnSave').click(function () {
    var zTree = $.fn.zTree.getZTreeObj("treeAuthority"),
	nodes = zTree.getChangeCheckedNodes();
    var authorityArray = $('#hidCurrentAuthority').val().replace(/^[\;]|[\;*]$/g, '').split(';');
    var authorityStack = [];
    $.each(authorityArray, function (k) {
        var tmpAuthority = authorityArray[k].replace(/^[\,]|[\,*]$/g, '').split(',');
        authorityStack.push({ id: tmpAuthority[0], isG: tmpAuthority[1] });
    });
    var addAuthority = '';
    var removeAuthority = '';
    var groupAuthority = '';
    $.each(nodes, function (i) {
        var flag = 0;
        $.each(authorityStack, function (j) {
            flag = flag + 1;
            if (nodes[i].id == authorityStack[j].id) {
                removeAuthority += nodes[i].id + ',';
                if (authorityStack[j].isG == 't') {
                    groupAuthority += nodes[i].name + '，';
                }
                return false;
            } else if (flag == authorityStack.length) {
                addAuthority += nodes[i].id + ',';
            }
        });
    });
    if (addAuthority == '' && removeAuthority == '') {
        $('#modal-authority').modal('hide');
        return false;
    } else {
        if (groupAuthority != '') {
            if (confirm('您选择删除的权限中，' + groupAuthority + ' 是组权限，删除将使得全组的去掉该权限，您确定删除吗？')) {
                $('#hidAddAuthority').val(addAuthority);
                $('#hidRemoveAuthority').val(removeAuthority);
            } else {
                return false;
            }
        } else {
            $('#hidAddAuthority').val(addAuthority);
            $('#hidRemoveAuthority').val(removeAuthority);
        }
    }
});

$('#formAuthority').ajaxForm({
    success: function (data) {
        $('#modal-authority').modal('hide');
        $('#myModal').modal();
    }
});
});