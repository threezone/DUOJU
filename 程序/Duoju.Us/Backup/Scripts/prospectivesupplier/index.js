$(function () {
    $.ajax({
        url: "http://www.haojiudian.com/ajax/hotel",
        type: "GET",
        dataType: "jsonp",
        cache: true,
        data: {
            valued: 2,
            districtid: 41346,
            classid: 0
        },
        success: function (data) {
            console.log(data);
        }
    });
});