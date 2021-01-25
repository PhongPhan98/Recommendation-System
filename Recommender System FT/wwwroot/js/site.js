$(document).ready(function () {
    $(".bnt-train").click(function () {
        $(".main").hide();
        $(".popup-data").show();
    })
    $(".bnt-topten").click(function () {
        $(".popup-data").hide();
        $(".popup-topten").show();
    })
    $(".bnt-topten").click(function () {
        $(".popup-data").hide();
        $(".popup-topten").show();
    })
    $(".done").click(function () {
        $(".popup-topten").hide();
        $(".main").show();
    })

})
