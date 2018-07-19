/// <reference path="jquery-1.10.2.min.js" />


$(document).ready(function(){
    //Set click events for tiles
    $('.tile').click(function () {
        var controller = $(this).attr("controller");
        var action = $(this).attr("action") || "index";
        window.location.href = "/" + controller + "/" + action
    });
    $(".tile").mousedown(function () {
        $(".tile").removeClass("tile-mousedown");
        $(this).addClass("tile-mousedown");
    })
    $(".tile").mouseup(function () {
        $(this).removeClass("tile-mousedown");
    })

});