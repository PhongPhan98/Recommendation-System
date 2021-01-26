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
        $(".data-topten").hide();
        $(".popup-data").hide();
        $(".popup-topten").show();
        $(".loadtop10").show();
        timerId = window.setInterval(function () {
            x = $("tbody").children();
            if (x.length >= 1) {
                $(".loadtop10").hide();
                $(".data-topten").show();
            } else { }
        }, 2000);
        setTimeout(() => { clearInterval(timerId);}, 30000);
       
       
    })
   
    $(".done").click(function () {
        $(".popup-topten").hide();
        $(".main").show();
    })

})
$body = $("body");


