(function (f, w) {
    var __s = 'http://cr.yaochufa.com';

    var metaElement = document.createElement("meta");
    metaElement.setAttribute("http-equiv", "Access-Control-Allow-Origin");
    metaElement.setAttribute("content", "*");
    document.getElementsByTagName('head')[0].appendChild(metaElement);

    var jqElement = document.createElement("script");
    jqElement.setAttribute("type", "text/javascript");
    jqElement.setAttribute("src", __s + "/Scripts/jquery.min.js");
    document.body.appendChild(jqElement);

    if (jqElement.readyState) {
        jqElement.onreadystatechange = function () {
            if (jqElement.readyState == "loaded" || jqElement.readyState == "complete") {
                jqElement.onreadystatechange = null;
                _callback();
            }
        };
    } else {
        jqElement.onload = function () {
            _callback();
        };
    }
    document.body.appendChild(jqElement);
    var _callback = function () {
        var hsElement = document.createElement("script");
        hsElement.setAttribute("type", "text/javascript");
        hsElement.setAttribute("src", __s + "/Scripts/plug-in/hotsale.min.js?" + Math.floor(new Date / 1E7));
        document.body.appendChild(hsElement);
    }
})(window, document);