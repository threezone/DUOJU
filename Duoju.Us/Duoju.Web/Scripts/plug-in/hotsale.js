(function (f, w) {
    $('#_div_loading').hide();
    var __ServerUrl = 'http://cr.yaochufa.com';
    var regUrl = new RegExp(/http:\/\/[\s\S]+.meituan.com\/deal\/\d+.html/);
    if (regUrl.test(window.location.href)) {
        var url = __ServerUrl + "/Account/GetUser?callback=ycfCrmUserInfo";
        var script = document.createElement('script');
        if (script.readyState) {
            //IE 
            script.onreadystatechange = function () {
                if (script.readyState == "loaded" || script.readyState == "complete") {
                    script.onreadystatechange = null;
                    callback();
                }
            };
        } else {
            //其他浏览器 
            script.onload = function () {
                callback();
            };
        }
        script.setAttribute('src', url);
        document.getElementsByTagName('head')[0].appendChild(script);

        var callback = function () {
            if ($('#_crm_userId').val() > 0) {
                //获取当前页面非iframe的body
                var _body = document.getElementsByTagName('body')[0];

                //定义插件的样式
                var _cssElement = document.createElement('style'),
		        _cssText = '._div_modal{position:fixed;top:7%;right:15%;z-index:1050;width:70%;height:75%;margin-left:-280px;background-color:#fff;border:1px solid #999;border:1px solid #999;border:1px solid rgba(0,0,0,0.3);outline:0;-webkit-box-shadow:0 3px 7px rgba(0,0,0,0.3);-moz-box-shadow:0 3px 7px rgba(0,0,0,0.3);box-shadow:0 3px 7px rgba(0,0,0,0.3);-webkit-background-clip:padding-box;-moz-background-clip:padding-box;background-clip:padding-box;}'
                            + '._div_title{height:auto;padding:8px 15px 5px;background:#efefef;border-bottom:1px solid #CDCDCD;}'
                            + '._div_title_close{padding:0;cursor:pointer;background:transparent;border:0;-webkit-appearance:none;float:right;font-size:20px;font-weight:bold;line-height:20px;color:#000;text-shadow:0 1px 0 #fff;opacity:.2;filter:alpha(opacity=20);margin-top:2px;}'
                            + '._div_title_h3{font-size: 12px;text-shadow: 0 1px 0 #ffffff;color:#333;}'
                            + '._div_foot{padding:10px 15px;margin-bottom:0;text-align:right;background-color:#f5f5f5;border-top:1px solid #ddd;-webkit-box-shadow:inset 0 1px 0 #fff;-moz-box-shadow:inset 0 1px 0 #fff;box-shadow:inset 0 1px 0 #fff;}'
                            + '._div_body{position:relative;height:85%;padding:15px;overflow-y:auto;}'
                            + '._div_widget_box{background:none repeat scroll 0 0 #F9F9F9;border-left:1px solid #CDCDCD;border-top:1px solid #CDCDCD;border-right:1px solid #CDCDCD;clear:both;margin-bottom:16px;}'
                            + '._div_widget_title{background:#efefef;border-bottom:1px solid #CDCDCD;height:36px;}'
                            + '._div_widget_title h5{color:#666;float:left;font-size:12px;font-weight:bold;padding:12px;line-height:12px;margin:0}'
                            + 'select, textarea, input[type="text"]{display:inline-block;height:20px;padding:4px 6px;font-size:14px;line-height:20px;color:#555;vertical-align:middle;background-color:#fff;border:1px solid #ccc;-webkit-box-shadow:inset 0 1px 1px rgba(0,0,0,0.075);-moz-box-shadow:inset 0 1px 1px rgba(0,0,0,0.075);box-shadow:inset 0 1px 1px rgba(0,0,0,0.075);-webkit-transition:border linear .2s,box-shadow linear .2s;-moz-transition:border linear .2s,box-shadow linear .2s;-o-transition:border linear .2s,box-shadow linear .2s;transition:border linear .2s,box-shadow linear .2s}'
                            + '._div_widget_content{padding:15px;border-bottom:1px solid #cdcdcd;}'
                            + '._div_control_group{margin-bottom:0;}'
                            + '._div_control_label{float:left;width:140px;padding-top:13px;text-align:right;}'
                            + '._div_controls{margin-left:160px;padding:10px 0}'
                            + '._div_button{position:fixed;width:81px;height:81px;border:1px solid #ccc;top:7%;right:15%;-moz-border-radius:50%; -webkit-border-radius:50%; border-radius:50%;filter:alpha(Opacity=80);-moz-opacity:0.5;opacity: 0.5;background:#000;text-align:center;line-height:81px;color:white;}'
                            + '._div_estop{position:fixed;top:7%;right:15%;z-index:1055;width:70%;height:77%;margin-left:-280px; opacity:0.75; filter: alpha(opacity=30); background-color:#fff;}'
		        ;

                var _div_main = document.createElement("div");
                _div_main.setAttribute("id", "_div_main");
                _div_main.setAttribute("class", "_div_modal");

                var _div_title = document.createElement("div");
                _div_title.setAttribute("id", "_div_title");
                _div_title.setAttribute("class", "_div_title");
                _div_title.innerHTML = '<button type="button" class="_div_title_close">×</button><h3 class="_div_title_h3">要出发CRM-爆单库采集插件</h3>';

                var meituanImg = $('.deal-buy-cover-img').find('img')[0];
                var meituanPoi = eval($('#J-biz-pos').attr('data-poi'));
                var meituanContentObj = $('.deal-menu').find('tr');
                var meituanContent = '';
                var j = 0;
                $.each(meituanContentObj, function (i) {
                    if (j > 0) {
                        var obj = $(meituanContentObj[i]).find('.name');
                        if (obj.length > 0) {
                            var tdEle = $(meituanContentObj[i]).find('td');
                            meituanContent += $(tdEle[0]).html() + 'x' + $(tdEle[2]).html() + '&&';
                        }
                    }
                    j++;
                });

                var meituanProductName = $($('div.deal-brand').find('h1')).html();

                var meituanProvide = null;
                var meituanProduct = null;

                $.each(meituanPoi, function (i) {
                    if (meituanPoi[i].name.substring(0, 3).indexOf(meituanProductName.substring(0, 2)) >= 0) {
                        meituanProduct = meituanPoi[i];
                    } else {
                        meituanProvide = meituanPoi[i];
                    }
                });
                if (meituanProvide == null) {
                    meituanProvide = meituanProduct;
                }
                if (meituanProduct == null) {
                    meituanProduct = meituanProvide;
                }

                var meituanLat = '', meituanLng = '';
                if (meituanProduct.latlng) {
                    var latlng = eval(meituanProduct.latlng);
                    meituanLat = latlng[0];
                    meituanLng = latlng[1];
                } else {
                    var latlng = eval(meituanPoi[0].latlng);
                    meituanLat = latlng[0];
                    meituanLng = latlng[1];
                }

                var meituanStar = '', meituanEnd = '';
                var meituanDateObj = $('div.deal-term').find('dl').find('dd');

                $.each(meituanDateObj, function (k) {
                    var objs = eval($(meituanDateObj[k]).html().match(/\d{4}[\.-]\d{1,2}[\.-]\d{1,2}/g));
                    if (objs) {
                        meituanStar = objs[0];
                        meituanEnd = objs[1];
                        return false;
                    }
                });


                var meituanTraff = meituanProduct.trafficinfo.replace(/<\/?br?>/g, '');

                if (meituanTraff == '') {
                    var meituanPList = $('div.standard-content').find('p');
                    $.each(meituanPList, function (i) {
                        if ($(meituanPList[i]).html().indexOf('交通指南') >= 0 || $(meituanPList[i]).html().indexOf('交通路线') >= 0) {
                            meituanTraff = $(meituanPList[i]).html().replace(/<[^>]*>/g, " ");
                        }
                    });
                }

                var meituanDesc = $($('.standard-content').find('p')[1]).html();
                var _div_body = document.createElement("div");
                _div_body.setAttribute("id", "_div_body");
                _div_body.setAttribute("class", "_div_body");
                _div_body.innerHTML = '<div style="width:35%;height:auto; text-align:center;padding:10px 0; border:1px solid #ccc;float:left;">'
                            + '<img id="_crm_imgUrl" style="border:1px solid #ccc;" width="96%" src="' + $(meituanImg).attr('src') + '">'
                            + '<div style="text-align:left;margin-left:10px;margin-bottom:10px;">产品名：<input type="text" id="_crm_productName" style="width:75%;" value="' + meituanProductName + '" /></div>'
                            + '<div style="width:100%;">'
                            + '<input type="hidden" id="_crm_shopId" value="' + $('.J-add-cart').attr('data-dealid') + '" />'
                            + '<input type="hidden" id="_crm_rate" value="' + $($('a.score-info').find('em')).html() + '" />'
                            + '<div style="width:30%;float:left;margin-left:10px;margin-bottom:10px;text-align:left;">美团价：<input id="_crm_tuanPrice" type="text" style="width:30%;" value="' + $('.deal-discount').find('.price').html().replace(/<[\s\S]+>/g, '') + '" /></div>'
                            + '<div style="width:30%;float:left;margin-left:10px;margin-bottom:10px;text-align:left;">门市价：<input id="_crm_price" type="text" style="width:30%;" value="' + $('.discount-price').html().replace(/¥/g, '') + '" /></div>'
                            + '<div style="width:30%;float:left;margin-left:10px;margin-bottom:10px;text-align:left;">总销量：<input id="_crm_volume" type="text" style="width:30%;" value="' + $('.deal-status-count').find('strong').html() + '" /></div>'
                            + '</div>'
                            + '</div>'
                            + '<div style="width:64%;float:right;height:auto;">'
                            + '<div class="_div_widget_box">'
                            + '<div class="_div_widget_title">'
                            + '<h5>本单详情</h5>'
                            + '</div>'
                            + '<div class="_div_widget_content">'
                            + '<div class="_div_control_group">'
                            + '<label class="_div_control_label">供应商名：</label>'
                            + '<div class="_div_controls">'
                            + '<input style="width:85%;" readonly="readonly" id="_crm_provider" type="text" value="' + meituanProvide.name + '">'
                            + '</div>'
                            + '</div>'
                            + '<div class="_div_control_group">'
                            + '<label class="_div_control_label">本单内容：</label>'
                            + '<div class="_div_controls">'
                            + '<input style="width:85%;" readonly="readonly" id="_crm_content" type="text" value="' + meituanContent + '">'
                            + '</div>'
                            + '</div>'
                            + '<div class="_div_control_group">'
                            + '<label class="_div_control_label">有效时间：</label>'
                            + '<div class="_div_controls">'
                            + '<input style="width:40%;" id="_crm_start" type="text" value="' + meituanStar + '">'
                            + '<input style="width:40%;margin-left:15px;" id="_crm_end" type="text" value="' + meituanEnd + '">'
                            + '</div>'
                            + '</div>'
                            + '<div class="_div_control_group">'
                            + '<label class="_div_control_label">联系方式：</label>'
                            + '<div class="_div_controls">'
                            + '<input style="width:56%;" id="_crm_address" type="text" value="' + meituanProduct.address + '">'
                            + '<input style="width:24%;margin-left:15px;" id="_crm_phone" type="text" value="' + meituanProduct.phone + '">'
                            + '</div>'
                            + '</div>'
                            + '<div class="_div_control_group">'
                            + '<label class="_div_control_label">区域信息：</label>'
                            + '<div class="_div_controls">'
                            + '<input style="width:25%;" id="_crm_city" type="text" placeholder="城市" value="' + meituanProduct.cityname + '">'
                            + '<input style="width:25%;margin-left:16px;" placeholder="区域" id="_crm_district" type="text" value="' + meituanProduct.disname + '">'
                            + '<input style="width:25%;margin-left:16px;" placeholder="地铁" id="_crm_subwayname" type="text" value="' + meituanProduct.subwayname + '">'
                            + '</div>'
                            + '</div>'
                            + '<div class="_div_control_group">'
                            + '<label class="_div_control_label">交通路线：</label>'
                            + '<div class="_div_controls">'
                            + '<textarea id="_crm_traffic" style="width:85%;height:80px;">' + meituanTraff + '</textarea>'
                            + '</div>'
                            + '</div>'
                            + '<div class="_div_control_group">'
                            + '<label class="_div_control_label">产品简介：</label>'
                            + '<div class="_div_controls">'
                            + '<textarea id="_crm_desc" style="width:85%;height:100px;">' + (meituanDesc == null ? "" : meituanDesc.replace(/<[^>]*>/g, "")) + '</textarea>'
                            + '</div>'
                            + '</div>'
                             + '<div class="_div_control_group">'
                            + '<label class="_div_control_label">个性评价：</label>'
                            + '<div class="_div_controls">'
                            + '<input style="width:85%;" id="_crm_evaluate" type="text" value="" placeholder="填写自己对产品的个性评价">'
                            + '</div>'
                            + '</div>'
                            + '</div>'
                            + '</div>'
                            + '</div>';

                var _div_foot = document.createElement("div");
                _div_foot.setAttribute("id", "_div_foot");
                _div_foot.setAttribute("class", "_div_foot");
                _div_foot.innerHTML = '<input id="_btnUploadToYCF" type="button" value="确定" class="btn btn-success"><span style="margin-left: 20px;"></span><input id="btnCancel" type="button" class="btn" data-dismiss="modal" value="取消">';

                _cssElement.setAttribute('id', 'css_Slect');
                _cssElement.setAttribute('type', 'text/css');
                if (_cssElement.styleSheet) {
                    _cssElement.styleSheet.cssText = _cssText;
                }
                else {
                    _cssElement.appendChild(document.createTextNode(_cssText));
                }

                var _div_button = document.createElement("div");
                _div_button.setAttribute("id", "_div_button");
                _div_button.setAttribute("class", "_div_button");
                _div_button.innerHTML = "<h2 id='_div_button_h2'>+CRM</h2>";
                _body.appendChild(_div_button);

                _body.appendChild(_cssElement);
                _div_main.appendChild(_div_title);
                _div_main.appendChild(_div_body);
                _div_main.appendChild(_div_foot);
                _body.appendChild(_div_main);

                $('#_btnUploadToYCF').click(function () {
                    var _div_estop = document.createElement("div");
                    _div_estop.setAttribute("id", "_div_estop");
                    _div_estop.setAttribute("class", "_div_estop");
                    _div_estop.innerHTML = '<div id="_div_loading_d" style="width: 64px; position: fixed; left: 50%; top: 40%;"><img src="' + __ServerUrl + '/Content/images/loading_b.gif" style="width: 64px;" /></div>';
                    _body.appendChild(_div_estop);
                    $.ajax({
                        url: __ServerUrl + "/PlugIn/GetHotSale",
                        type: "POST",
                        dataType: "text",
                        data: {
                            productId: $('#_crm_shopId').val(),
                            productName: $('#_crm_productName').val(),
                            fromType: 'meituan',
                            productType: $($('div.bread-nav').find('a')[2]).html(),
                            providerName: $('#_crm_provider').val(),
                            tuanPrice: $('#_crm_tuanPrice').val(),
                            price: $('#_crm_price').val(),
                            volume: $('#_crm_volume').val(),
                            content: $('#_crm_content').val(),
                            start: $('#_crm_start').val(),
                            end: $('#_crm_end').val(),
                            address: $('#_crm_address').val(),
                            phone: $('#_crm_phone').val(),
                            city: $('#_crm_city').val(),
                            district: $('#_crm_district').val(),
                            metro: $('#_crm_subwayname').val(),
                            traffic: $('#_crm_traffic').val(),
                            desc: $('#_crm_desc').val(),
                            url: window.location.href,
                            followBy: $('#_crm_userId').val(),
                            imgUrl: $('#_crm_imgUrl').attr('src'),
                            lat: meituanLat,
                            lng: meituanLng,
                            evaluate: $('#evaluate').val(),
                            rate: $('#_crm_rate').val(),
                            channelCity: $('.city-info__name').html()+'-美团'
                        },
                        cache: false,
                        success: function (data) {
                            if (data == 'success') {
                                alert('产品收录到CRM成功！');
                                $('#_div_button_h2').html('已收录');
                                hideDialog();
                            } else {
                                alert(data);
                                $('#_div_button_h2').html('已收录');
                                hideDialog();
                            }
                        },
                        error: function (e) {
                            console.log(e);
                        }
                    });
                });

                $('#btnCancel').click(function () {
                    hideDialog();
                });

                $('._div_title_close').click(function () {
                    hideDialog();
                });

                var hideDialog = function () {
                    $('#_div_estop').hide();
                    $('#_div_main').hide('slow');
                    $('#_div_bg').hide();
                    $('#_div_button').show('slow');
                };


                $('#_div_button').live('click', function () {
                    $('#_div_main').show('slow');
                    $('#_div_bg').show();
                    $('#_div_button').hide('slow');
                });
            } else {
                alert("未检测到您登录到CRM，请先登录！");
                $('#_div_bg').hide();
            }
        }
    } else {
        $('#_div_bg').hide();
        alert('当前页面非美团产品内页，请到美团产品内页使用该插件！');
    }
})(window, document);