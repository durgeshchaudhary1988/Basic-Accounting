$(document).ready(function () {
    $(".childEntry").hide();
    $("#btnDetailView").click(function () {
        if ($(this).val()=='Detail') {
            $(this).val('Summary');
            $(".childEntry").show();
        } else {
            $(this).val('Detail');
            $(".childEntry").hide();
        }
    });
});