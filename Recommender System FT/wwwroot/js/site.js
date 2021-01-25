$(document).ready(function () {
    
    $(".bnt-train").click(function () {
        setTimeout(function () { $(".loader").hide(); $(".topten").show(); $(".success").show(); }, 3000);  
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
$body = $("body");


