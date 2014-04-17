javascript: void ((function () {
    var d = document;
    var b = document.getElementsByTagName('body')[0];
    var s = "http://cr.yaochufa.com";
    if (!document.getElementById("_div_bg")) {
        var _div_bg = d.createElement("div");
        _div_bg.setAttribute("id", "_div_bg");
        _div_bg.setAttribute("class", "_div_bg _div_opacity");
        _div_bg.innerHTML = '<div id="_div_loading" style="width: 64px; position: fixed; left: 50%; top: 40%;"><img src='+s+'"/Content/images/loading_b.gif" style="width: 64px;" /></div>';

        var c = document.createElement('style'),
		ct = '._div_bg {height:100%; width:100%; position: fixed; _position:absolute; top:0; z-index:999; }' + '._div_opacity{ opacity:0.75; filter: alpha(opacity=30); background-color:#000; }' + '._div_modal{position:fixed;top:7%;right:15%;z-index:1050;width:70%;height:75%;margin-left:-280px;background-color:#fff;border:1px solid #999;border:1px solid #999;border:1px solid rgba(0,0,0,0.3);outline:0;-webkit-box-shadow:0 3px 7px rgba(0,0,0,0.3);-moz-box-shadow:0 3px 7px rgba(0,0,0,0.3);box-shadow:0 3px 7px rgba(0,0,0,0.3);-webkit-background-clip:padding-box;-moz-background-clip:padding-box;background-clip:padding-box;}';
        c.setAttribute('id', 'css_c');
        c.setAttribute('type', 'text/css');

        if (c.styleSheet) {
            c.styleSheet.cssText = ct;
        } else {
            c.appendChild(document.createTextNode(ct));
        }

        b.appendChild(c);
        b.appendChild(_div_bg);

        var e = d.createElement("script");
        e.setAttribute("charset", "UTF-8");
        e.setAttribute("src", s+"/Scripts/plug-in/plug-in-meituan.js?" + Math.floor(new Date / 1E7));
        b.appendChild(e);
    } else {
        alert('插件已成功加载到页面中');
    }
})());