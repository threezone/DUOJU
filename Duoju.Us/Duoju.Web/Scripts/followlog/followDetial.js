$(document).ready(function () {
    $('#basic_validate').ajaxForm({
        success: function (data) {
            $('#myModal').modal();
        }
    });

    $('#btnCancel').click(function () {
        history.go(-1);
    });
}); 