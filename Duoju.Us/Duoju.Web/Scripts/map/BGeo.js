/**
*Author:jary.zhang
*Date:2013-01-15
*/
/**
*使用前，需先引入 http://api.map.baidu.com/api?v=1.4
*BGeo.js is a javacript lib for baidu map, see: http://cnblogs.com/three-zone
*/

window.BMap = window.BMap || {};
BMap.Convertor = {};

var geo_map = function (elem, opts) {
    var myMap, _point;
    if (opts && elem)
        myMap = new BMap.Map(elem, opts);
    else if (elem)
        myMap = new BMap.Map(elem);
    var getZoom = function (z) {
        var mz = 8;
        if (typeof z === 'number') {
            if (z >= 3 && z <= 25) {
                mz = z;
            }
        }
        return mz;
    };
    return {
        setMapCenter: function (center) {
            if (myMap) {
                myMap.setCenter(center);
                _point = center;
            }
        },
        setMapTypeId: function (bMapType) {
            if (myMap) {
                if (typeof bMapType === 'string') {
                    if (bMapType == 'PERSPECTIVE_MAP') {
                        myMap.setMapType(BMAP_PERSPECTIVE_MAP);
                    } else if (bMapType == 'SATELLITE_MAP') {
                        myMap.setMapType(BMAP_SATELLITE_MAP);
                    } else if (bMapType == 'HYBRID_MAP') {
                        myMap.setMapType(BMAP_HYBRID_MAP);
                    } else {
                        myMap.setMapType(BMAP_NORMAL_MAP);
                    }
                }
            }
        },
        enableScrollWheelZoom: function (isEnable) {
            if (myMap) {
                if (isEnable)
                    myMap.enableScrollWheelZoom();
                else
                    myMap.disableScrollWheelZoom();
            }
        },
        getMapCenter: function () {
            if (myMap) {
                return myMap.getCenter();
            }
        },
        getMapTypeId: function () {
            if (myMap) {
                return myMap.getMapType();
            }
        },
        getMapZoom: function () {
            if (myMap) {
                return myMap.getZoom();
            }
        },
        getMap: function () {
            if (myMap) {
                return myMap;
            }
        },
        setZoom: function (z) {
            if (myMap) {
                myMap.setZoom(getZoom(z));
            }
        },
        initMap: function (node, point, bMapType, z) {
            if (typeof node === 'object' || typeof node === 'string') {
                _point = point;
                myMap = new BMap.Map(node);
                myMap.centerAndZoom(point, z);
                myMap.addControl(new BMap.NavigationControl());
                myMap.addControl(new BMap.ScaleControl());
                myMap.addControl(new BMap.OverviewMapControl({ isOpen: true, anchor: BMAP_ANCHOR_BOTTOM_RIGHT }));
                myMap.addControl(new BMap.MapTypeControl());
                var geocoder = new BMap.Geocoder();
                geocoder.getLocation(myMap.getCenter(), function (r) {
                    myMap.setCurrentCity(r.addressComponents['city']);
                });
                this.setMapTypeId(bMapType);

                function locationControl() {
                    this.defaultAnchor = BMAP_ANCHOR_TOP_LEFT;
                    this.defaultOffset = new BMap.Size(8, 210);
                }
                locationControl.prototype = new BMap.Control();
                locationControl.prototype.initialize = function (map) {
                    var imgUrl = '../Scripts/map/Images/maps-icon2.png';
                    var currentLocation = location.href;
                    var heard = currentLocation.replace(/(.+\/).*$/g, '$1');
                    var tail = currentLocation.replace(/(.+?:\/\/.+?\/).*$/g, '$1');
                    var locationArray = heard.replace(tail, '')
                        .replace(/(.*)\/$/g, '$1')
                        .split('/');
                    for (var i = 0; i < locationArray.length - 1; i++) {
                        imgUrl = '../' + imgUrl;
                    }
                    var div = document.createElement("div");
                    div.innerHTML = '<img title="Go Back To My Location" src="' + imgUrl + '" width="48" height="48"/>';
                    div.style.cursor = "pointer";
                    div.onclick = function (e) {
                        map.setZoom(myMap.setCenter(point));
                    }
                    map.getContainer().appendChild(div);
                    return div;
                }
                var myLocationCtrl = new locationControl();
                myMap.addControl(myLocationCtrl);
                this.zoomend();
            }
        },
        panTo: function (point, isAnimation) {
            if (isAnimation || typeof isAnimation === 'undefined')
                myMap.panTo(point, { noAnimation: true });
            else
                myMap.panTo(point, { noAnimation: false });
        },
        click: function (fn) {
            myMap.addEventListener('click', fn);
        },
        dblclick: function (fn) {
            myMap.addEventListener('dblclick', fn);
        },
        mouseover: function (fn) {
            myMap.addEventListener('mouseover', fn);
        },
        mouseout: function (fn) {
            myMap.addEventListener('mouseout', fn);
        },
        zoomend: function () {
            myMap.addEventListener('zoomend', function () {
                myMap.setZoom(myMap.setCenter(_point));
            });
        }
    };
};

var geo_marker = function (map, p, opts) {
    var myMarker;
    if (p && opts)
        myMarker = new BMap.Marker(p, opts);
    else if (p)
        myMarker = new BMap.Marker(p);
    var markerOptions = {};
    return {
        initMarker: function (map, point, isClickable, isDraggable, icon, title, shadow, offsetX, offsetY) {
            markerOptions.enableClicking = isClickable || false;
            markerOptions.enableDragging = isDraggable || false;
            if (icon)
                markerOptions.icon = icon;
            markerOptions.title = title;
            if (shadow)
                markerOptions.shadow = shadow;
            markerOptions.offset = { width: offsetX || 0, height: offsetY || 0 };
            myMarker = new BMap.Marker(point, markerOptions);
            map.addOverlay(myMarker);
        },
        setDraggable: function (isDraggable) {
            if (myMarker) {
                if (isDraggable)
                    myMarker.enableDragging();
                else
                    myMarker.disableDragging();
            }
        },
        setPosition: function (postion) {
            if (myMarker) {
                myMarker.setPosition(postion);
            }
        },
        setTitle: function (title) {
            if (myMarker) {
                myMarker.setTitle(title);
            }
        },
        setIcon: function (icon) {
            if (myMarker) {
                myMarker.setIcon(icon);
            }
        },
        setShadow: function (shadow) {
            if (myMarker) {
                myMarker.setShadow(shadow);
            }
        },
        setAnimation: function (animation) {
            if (myMarker) {
                if (typeof animation === 'string') {
                    if (animation == 'DROP')
                        myMarker.setAnimation(BMAP_ANIMATION_DROP);
                    else if (animation == 'BOUNCE')
                        myMarker.setAnimation(BMAP_ANIMATION_BOUNCE);
                }
            }
        },
        setTop: function (isTop) {
            if (myMarker) {
                myMarker.setTop(isTop);
            }
        },
        getMap: function () {
            if (myMarker) {
                return myMarker.getMap();
            }
        },
        getIcon: function () {
            if (myMarker) {
                return myMarker.getIcon();
            }
        },
        getPosition: function () {
            if (myMarker) {
                return myMarker.getPosition();
            }
        },
        getMarker: function () {
            if (myMarker) {
                return myMarker;
            }
        },
        click: function (fn) {
            myMarker.addEventListener('click', fn);
        },
        dragstart: function (fn) {
            myMarker.addEventListener('dragstart', fn);
        },
        dragging: function (fn) {
            myMarker.addEventListener('dragging', fn);
        },
        dragend: function (fn) {
            myMarker.addEventListener('dragend', fn);
        },
        mousedown: function (fn) {
            myMarker.addEventListener('mousedown', fn);
        },
        mouseout: function (fn) {
            myMarker.addEventListener('mouseout', fn);
        },
        mouseover: function (fn) {
            myMarker.addEventListener('mouseover', fn);
        },
        mouseup: function (fn) {
            myMarker.addEventListener('mouseup', fn);
        },
        remove: function (fn) {
            myMarker.addEventListener('remove', fn);
        },
        clearMarker: function () {
            if (myMarker) {
                this.getMap().removeOverlay(myMarker);
            }
        }
    };
};

var geo_geocoder = function () {
    var myGeocoder = new BMap.Geocoder();
    return {
        GetGeoCoder: function (position, fnCallback, opts) {
            if (typeof position === 'string') {
                myGeocoder.getPoint(position, function (r) {
                    if (r) {
                        myGeocoder.getLocation(r, fnCallback);
                    } else {
                        r = 'ZERO_RESULT';
                        fnCallback.call(fnCallback, r);
                    }
                }, opts);
            } else if (typeof position === 'object') {
                myGeocoder.getLocation(position, fnCallback, opts);
            }
        }
    };
};

//routeType:[0:步行；1:公共交通；2.驾车；]
//renderOptions:{
//    map：map,
//    panel:string|htmlElem,
//    selectFirstResult:boolean,
//    autoViewport:boolean,
//    highlightMode:highlightMode
//}
var geo_routeService = function (map, routeType, renderOptions) {
    var myRoute;
    var myRouteOptions = {};
    var handleResult = function (e) {
        var myRouteResult = {};
        myRouteResult.resolved = 'true';
        myRouteResult.numOfPlan = e.getNumPlans();
        myRouteResult.start = e.getStart();
        myRouteResult.end = e.getEnd();
        myRouteResult.plans = new Array();
        for (var i = 0; i < myRouteResult.numOfPlan; i++) {
            var plan = e.getPlan(i);
            var routResult = new Array();
            routResult.duration = plan.getDuration(true);
            routResult.distance = plan.getDistance(false) / 1000;
            myRouteResult.plans.push(routResult);
        }
        return myRouteResult;
    };
    if (renderOptions || typeof renderOptions === 'object')
        myRouteOptions.renderOptions = renderOptions;
    switch (routeType) {
        case 0:
            myRoute = new BMap.WalkingRoute(map, myRouteOptions);
            break;
        case 1:
            myRoute = new BMap.TransitRoute(map, myRouteOptions);
            break;
        case 2:
            myRoute = new BMap.DrivingRoute(map, myRouteOptions);
            break;
    }
    return {
        search: function (start, end, onSearchComplete) {
            myRoute.search(start, end);
            if (typeof onSearchComplete === 'function') {
                myRoute.setSearchCompleteCallback(function (e) {
                    var r = {};
                    if (myRoute.getStatus() != BMAP_STATUS_SUCCESS) {
                        r.resolved = 'false';
                        r.msg = 'The Route Can\'t be resolved';
                    } else {
                        r = handleResult(e);
                    }
                    onSearchComplete.call(onSearchComplete, r);
                });
            }
        }
    };
};

var GetBGeoLocation = function (ops, fnSuccess, fnFaile) {
    var options = {
        'enableHighAccuracy': true,
        'timeout': 10000,
        'maximumAge': 0
    };
    if (typeof ops == 'object' && ops) {
        options = ops;
    }
    var geolocation = new BMap.Geolocation();
    geolocation.getCurrentPosition(function (r) {
        if (this.getStatus() == BMAP_STATUS_SUCCESS) {
            fnSuccess.call(fnSuccess, r);
        }
        else {
            var c = {};
            var myCity = new BMap.LocalCity();
            myCity.get(function (myResult) {
                if (myResult.level === 12) {
                    c.point = myResult.center;
                    c.accuracy = 100000;
                    fnSuccess.call(fnSuccess, c);
                } else if (typeof fnFaile !== 'function')
                    alert('failed' + this.getStatus());
                else {
                    fnFaile.call(fnFaile, r);
                }
            });
        }
    }, options)
};

var GetGeoLocation = function (ops, fnSuccess, fnFaile) {
    var options = {
        'enableHighAccuracy': true,
        'timeout': 10000,
        'maximumAge': 0
    };
    if (typeof ops == 'object' && ops) {
        options = ops;
    }
    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(function (r) {
            var result = {};
            var point = geo_getGLatLng(r.coords.latitude, r.coords.longitude);
            geo_translate(point, 0, function (p) {
                result.point = p;
                result.accuracy = r.coords.accuracy;
                fnSuccess.call(fnSuccess, result);
            });

        }, function (e) {
            var c = {};
            var myCity = new BMap.LocalCity();
            myCity.get(function (myResult) {
                if (myResult.level === 12) {
                    c.point = myResult.center;
                    c.accuracy = 100000;
                    fnSuccess.call(fnSuccess, c);
                } else if (typeof fnFaile !== 'function')
                    alert('failed' + this.getStatus());
                else {
                    fnFaile.call(fnFaile, e);
                }
            });
        }, options);
    } else {
        // Browser doesn't support Geolocation
        alert(handleNoGeolocation(false));
    }
};

var handleNoGeolocation = function (errorFlag) {
    var content = '';
    if (errorFlag) {
        content = 'Error: The Geolocation service failed.';
    } else {
        content = 'Error: Your browser doesn\'t support geolocation.';
    }
    return content;
};

var geo_loadScript = function (xyUrl, callback) {
    var head = document.getElementsByTagName('head')[0];
    var script = document.createElement('script');
    script.type = 'text/javascript';
    script.src = xyUrl;
    script.onload = script.onreadystatechange = function () {
        if ((!this.readyState || this.readyState === "loaded" || this.readyState === "complete")) {
            callback && callback();
            script.onload = script.onreadystatechange = null;
            if (head && script.parentNode) {
                head.removeChild(script);
            }
        }
    };
    head.insertBefore(script, head.firstChild);
};

var geo_translate = function (point, type, callback) {
    var callbackName = 'cbk_' + Math.round(Math.random() * 10000);
    var xyUrl = "http://api.map.baidu.com/ag/coord/convert?from=" + type + "&to=4&x=" + point.lng + "&y=" + point.lat + "&callback=BMap.Convertor." + callbackName;
    geo_loadScript(xyUrl);
    BMap.Convertor[callbackName] = function (xyResult) {
        delete BMap.Convertor[callbackName];
        var point = new BMap.Point(xyResult.x, xyResult.y);
        callback && callback(point);
    }
};

var geo_getGLatLng = function (lat, lng) {
    if (typeof lat === 'number' && typeof lng === 'number') {
        return new BMap.Point(lng, lat);
    }
    throw { 'exception_type': 'FORMAT_ERROR', 'exception_msg': 'can\'t convert to number' };
};

var geo_getBIcon = function (url, width, height) {
    if (typeof url === 'string') {
        return new BMap.Icon(url, new BMap.Size(width || 0, height || 0));
    }
    throw { 'exception_type': 'FORMAT_ERROR', 'exception_msg': 'url format error' };
};

