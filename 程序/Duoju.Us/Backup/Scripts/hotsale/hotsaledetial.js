$(function () {
    var __pageWidth = $(window).width();
    if (__pageWidth < 600) {
        $('.container-fluid').attr('style', 'padding:0;');
        $('.container-fluid .row-fluid:first-child').css('margin-top', '0');
        $('.widget-box').attr('style', 'margin-top: 0;border:none;margin-bottom:0;');
        $('.widget-content').attr('style', 'padding:8px;border-bottom:none;');
        $('#divHotSaleTitle').remove();
        $('#tbHotSaleInfo tr td:first-child').attr('style', 'width:25%;');
        $('#tbHotSaleContentInfo tr td:first-child').attr('style', 'width:25%;');
        $('#content').attr('style', 'padding-bottom:0;');
    }
});