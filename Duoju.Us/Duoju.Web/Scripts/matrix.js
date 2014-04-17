
$(document).ready(function () {
    $('#header_nav').click(function () {
        window.document.location.href = '/';
    });

    var __masterWidth = $(window).width();

    ycf_crm.getCurrentUser(null, function (data) {
        $('#welcome_msg').html(' Welcome ' + data.UserName);
        $('#hidAdminUserId').val(data.Usid);
    }, null);
    ycf_crm.getMenu(null, function (data) {
        if (data.length) {
            var currentPath = window.document.location.pathname;
            var _html = '';
            //console.log(data);
            $.each(data, function (i) {
                var _breadcrumb = ycf_crm.stringFormat('<a href="{0}" title="{1}" class="tip-bottom"><i class="{2}"></i>{3}</a>', data[i].UrlPath, '返回' + data[i].ActionName, data[i].Icon, data[i].ActionName);
                _breadcrumbSub = '';
                if (data[i].SubMenu != '') {
                    var isOpen = false;
                    var isSub = false;
                    var isBre = true;
                    _html += ycf_crm.stringFormat('<li class="submenu __isOpen__"><a href="#"><i class="icon {0}"></i><span>{1}</span></a><ul>', data[i].Icon, data[i].ActionName);
                    $.each(data[i].SubMenu, function (j) {
                        if (currentPath == data[i].SubMenu[j].UrlPath) {
                            isOpen = true;
                            _breadcrumbSub = '';
                            _breadcrumb += ycf_crm.stringFormat('<a href="{0}" title="{1}" class="tip-bottom"><i class="{2}"></i>{3}</a>', data[i].SubMenu[j].UrlPath, '返回' + data[i].SubMenu[j].ActionName, data[i].SubMenu[j].Icon, data[i].SubMenu[j].ActionName);
                            if (__masterWidth > 480) {
                                _html += ycf_crm.stringFormat('<li {0}><a href="{1}">{2}</a></li>', 'class="active"', data[i].SubMenu[j].UrlPath, data[i].SubMenu[j].ActionName);
                            } else {
                                _html += ycf_crm.stringFormat('<li {0}><a href="{1}">{2}</a></li>', '', data[i].SubMenu[j].UrlPath, data[i].SubMenu[j].ActionName);
                            }
                            isBre = false;
                        } else {
                            $.each(data[i].SubMenu[j].SubMenu, function (k) {
                                if (data[i].SubMenu[j].SubMenu[k].UrlPath == currentPath) {
                                    isSub = true;
                                    _breadcrumb += ycf_crm.stringFormat('<a href="{0}" title="{1}" class="tip-bottom"><i class="{2}"></i>{3}</a>', data[i].SubMenu[j].UrlPath, '返回' + data[i].SubMenu[j].ActionName, data[i].SubMenu[j].Icon, data[i].SubMenu[j].ActionName);
                                    _breadcrumbSub = ycf_crm.stringFormat('<a href="{0}" title="{1}" class="tip-bottom"><i class="{2}"></i>{3}</a>', data[i].SubMenu[j].SubMenu[k].UrlPath, '返回' + data[i].SubMenu[j].SubMenu[k].ActionName, data[i].SubMenu[j].SubMenu[k].Icon, data[i].SubMenu[j].SubMenu[k].ActionName);
                                }
                            });
                            _html += ycf_crm.stringFormat('<li><a href="{0}">{1}</a></li>', data[i].SubMenu[j].UrlPath, data[i].SubMenu[j].ActionName);
                        }
                    });
                    if (isOpen || isSub) {
                        $('#breadcrumb').html(_breadcrumb + _breadcrumbSub);
                        _breadcrumb = _breadcrumbSub = '';
                        if (__masterWidth > 480) {
                            _html = _html.replace(/__isOpen__/g, 'open');
                        } else {
                            _html = _html.replace(/__isOpen__/g, '');
                        }
                    }
                    else
                        _html = _html.replace(/__isOpen__/g, '');
                    _html += '</ul></li>';
                } else {
                    var isActive = (currentPath == data[i].UrlPath || currentPath == '/');
                    if (__masterWidth > 480) {
                        _html += ycf_crm.stringFormat('<li {0}><a href="{1}"><i class="icon {2}"></i><span>{3}</span></a></li>', (isActive ? 'class="active"' : ''), data[i].UrlPath, data[i].Icon, data[i].ActionName);
                    } else {
                        _html += ycf_crm.stringFormat('<li {0}><a href="{1}"><i class="icon {2}"></i><span>{3}</span></a></li>', '', data[i].UrlPath, data[i].Icon, data[i].ActionName);
                    }
                    if (isActive) {
                        $('#breadcrumb').html(_breadcrumb);
                    }
                }

            });
            $('#MainMenu').html(_html);

            // === Tooltips === //
            $('.tip').tooltip();
            $('.tip-left').tooltip({ placement: 'left' });
            $('.tip-right').tooltip({ placement: 'right' });
            $('.tip-top').tooltip({ placement: 'top' });
            $('.tip-bottom').tooltip({ placement: 'bottom' });
        }
    }, function (e) {
        console.log(e);
    });

    // === Sidebar navigation === //

    $('.submenu > a').live('click', function (e) {
        e.preventDefault();
        var submenu = $(this).siblings('ul');
        var li = $(this).parents('li');
        var submenus = $('#sidebar li.submenu ul');
        var submenus_parents = $('#sidebar li.submenu');
        if (li.hasClass('open')) {
            if (($(window).width() > 768) || ($(window).width() < 479)) {
                submenu.slideUp();
            } else {
                submenu.fadeOut(250);
            }
            li.removeClass('open');
        } else {
            if (($(window).width() > 768) || ($(window).width() < 479)) {
                submenus.slideUp();
                submenu.slideDown();
            } else {
                submenus.fadeOut(250);
                submenu.fadeIn(250);
            }
            submenus_parents.removeClass('open');
            li.addClass('open');
        }
    });

    var ul = $('#sidebar > ul');

    $('#sidebar > a').live('click', function (e) {
        e.preventDefault();
        var sidebar = $('#sidebar');
        if (sidebar.hasClass('open')) {
            sidebar.removeClass('open');
            ul.slideUp(250);
        } else {
            sidebar.addClass('open');
            ul.slideDown(250);
        }
    });

    // === Resize window related === //
    $(window).resize(function () {
        if ($(window).width() > 479) {
            ul.css({ 'display': 'block' });
            $('#content-header .btn-group').css({ width: 'auto' });
        }
        if ($(window).width() < 479) {
            ul.css({ 'display': 'none' });
            fix_position();
        }
        if ($(window).width() > 768) {
            $('#user-nav > ul').css({ width: 'auto', margin: '0' });
            $('#content-header .btn-group').css({ width: 'auto' });
        }
    });

    if ($(window).width() < 468) {
        ul.css({ 'display': 'none' });
        fix_position();
    }

    if ($(window).width() > 479) {
        $('#content-header .btn-group').css({ width: 'auto' });
        ul.css({ 'display': 'block' });
    }

    // === Search input typeahead === //
    $('#search input[type=text]').typeahead({
        source: ['Dashboard', 'Form elements', 'Common Elements', 'Validation', 'Wizard', 'Buttons', 'Icons', 'Interface elements', 'Support', 'Calendar', 'Gallery', 'Reports', 'Charts', 'Graphs', 'Widgets'],
        items: 4
    });

    // === Fixes the position of buttons group in content header and top user navigation === //
    function fix_position() {
        var uwidth = $('#user-nav > ul').width();
        $('#user-nav > ul').css({ width: uwidth, 'margin-left': '-' + uwidth / 2 + 'px' });

        var cwidth = $('#content-header .btn-group').width();
        $('#content-header .btn-group').css({ width: cwidth, 'margin-left': '-' + uwidth / 2 + 'px' });
    }

    // === Style switcher === //
    $('#style-switcher i').click(function () {
        if ($(this).hasClass('open')) {
            $(this).parent().animate({ marginRight: '-=190' });
            $(this).removeClass('open');
        } else {
            $(this).parent().animate({ marginRight: '+=190' });
            $(this).addClass('open');
        }
        $(this).toggleClass('icon-arrow-left');
        $(this).toggleClass('icon-arrow-right');
    });

    $('#style-switcher a').click(function () {
        var style = $(this).attr('href').replace('#', '');
        $('.skin-color').attr('href', 'css/maruti.' + style + '.css');
        $(this).siblings('a').css({ 'border-color': 'transparent' });
        $(this).css({ 'border-color': '#aaaaaa' });
    });

    $('.lightbox_trigger').click(function (e) {

        e.preventDefault();

        var image_href = $(this).attr("href");

        if ($('#lightbox').length > 0) {

            $('#imgbox').html('<img src="' + image_href + '" /><p><i class="icon-remove icon-white"></i></p>');

            $('#lightbox').slideDown(500);
        }

        else {
            var lightbox =
			'<div id="lightbox" style="display:none;">' +
				'<div id="imgbox"><img src="' + image_href + '" />' +
					'<p><i class="icon-remove icon-white"></i></p>' +
				'</div>' +
			'</div>';

            $('body').append(lightbox);
            $('#lightbox').slideDown(500);
        }

    });


    $('#lightbox').live('click', function () {
        $('#lightbox').hide(200);
    });

    $('#EditTask').click(function () {
        $('#TaskEditPanel').show();
        $('#TaskEditInfo').show();
    });

    $('#btnCancelEditTask').click(function () {
        $('#TaskEditPanel').hide();
        $('#TaskEditInfo').hide();
    });

    ycf_crm.getFollowState(null, function (data) {
        if (data && data.length > 0) {
            $.each(data, function (i) {
                var option = ycf_crm.stringFormat('<option value="{0}">{1}</option>', data[i].StateId, data[i].State);
                $('#taskFollowLogState').append(option);
            });
        }
    }, function (e) {
        console.log(e);
    });

    var taskcheckin = $('#divTaskStartDate').datetimepicker({ format: 'yyyy-mm-dd hh:ii',
        autoclose: true
    })
    .on('changeDate', function (ev) {
        var newDate = new Date(ev.date)
        newDate.setDate(newDate.getDate() - 1);
        $('#divTaskEndDate').datetimepicker('setStartDate', newDate);
        $('#divTaskEndDate')[0].focus();
    });

    var taskcheckout = $('#divTaskEndDate').datetimepicker({
        format: 'yyyy-mm-dd hh:ii',
        autoclose: true,
        onRender: function (date) {
            return date.valueOf() <= taskcheckin.date.valueOf() ? 'disabled' : '';
        }
    });

    $('#FlagTask').click(function () {
        var evs = {};
        if ($('#fullcalendar').length > 0) {
            evs = $('#fullcalendar').fullCalendar('clientEvents', $('#currentTaskId').val());
        } else {
            evs = ycf_async.findEventById($('#currentTaskId').val());
        }
        if (evs[0].taskState != 3) {
            ycf_crm.readTask($('#currentTaskId').val(), 3, null, function (d) {
                evs[0].className = 'fc-event-skin-important';
                evs[0].taskState = 3;
                $('#FlagTask').attr('style', 'color:#f74d4d;');
                if ($('#fullcalendar').length > 0) {
                    $('#fullcalendar').fullCalendar('updateEvent', evs[0], true);
                }
                $('#CancelTask').find('i').removeAttr('style');
                ycf_async.updateEventStateById($('#currentTaskId').val(), 3);
            }, function (e) {
                console.log(e);
            });
        }
    });

    $('#CancelTask').click(function () {
        var evs = {};
        if ($('#fullcalendar').length > 0) {
            evs = $('#fullcalendar').fullCalendar('clientEvents', $('#currentTaskId').val());
        } else {
            evs = ycf_async.findEventById($('#currentTaskId').val());

        }
        if (evs[0].taskState != 4) {
            ycf_crm.readTask($('#currentTaskId').val(), 4, null, function (d) {
                evs[0].className = 'fc-event-skin-cancel';
                evs[0].taskState = 4;
                $('#CancelTask').find('i').attr('style', 'color:#f74d4d;');
                $('#CancelTask').find('i').removeClass('icon-remove');
                $('#CancelTask').find('i').addClass('icon-ban-circle');
                $('#FlagTask').removeAttr('style');
                if ($('#fullcalendar').length > 0) {
                    $('#fullcalendar').fullCalendar('updateEvent', evs[0], true);
                }
                ycf_async.updateEventStateById($('#currentTaskId').val(), 4);
            }, function (e) {
                console.log(e);
            });
        }
    });

    $('.sp_gritter').live('click', function () {
        var evs = ycf_async.findEventById($(this).attr('eid'));
        $('#currentTaskId').val($(this).attr('eid'));
        $('#currentSupplierId').val(evs[0].supplierId);
        $('#lbTaskTitle').html('<strong>' + evs[0].title + '</strong> ');
        $('#taskStartDate').val(evs[0].start.toString("yyyy/MM/dd hh:mm"));
        $('#taskEndDate').val((evs[0].end == null ? evs[0].start.toString("yyyy/MM/dd hh:mm") : evs[0].end.toString("yyyy.MM.dd hh:mm")));
        $('#editTaskContent').val(evs[0].content);
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
    });

    $('#followLogAndTaskForm').ajaxForm({
        dataType: 'json',
        success: function (data) {
            ycf_async.updateEventStateById($('#currentTaskId').val(), $('#taskState').val());
            $('#TaskEditPanel').hide();
            $('#TaskEditInfo').hide();
            $('#modal-showTask').modal('hide');
            $('#myModal').modal();
            $('#followLogAndTaskForm').clearForm();
            if ($("#tb_followLog").length > 0) {
                $("#tb_followLog").datagrid("reload");
            }
            if ($("#tb_task").length > 0) {
                $("#tb_task").datagrid("reload");
            }
        }
    });
});

