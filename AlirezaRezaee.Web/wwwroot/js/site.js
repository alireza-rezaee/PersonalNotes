$(document).ready(function () {
    $('<div class="mt-5"></div>').css("height", $('#footer').outerHeight()).appendTo('body>.wrapper');
    $("#gallery").lightGallery();
    $('.js-tooltip').tooltip();
    $('.js-peyvandha-compact').mCustomScrollbar({
        axis: "x" // horizontal scrollbar
    });
    $('.js-gotop').click(function () {
        $('html, body').animate({ scrollTop: 0 });
    });
    $('#js-nav-shortcuts-trigger').click(function () {
        $(this).siblings('#js-nav-shortcuts').toggleClass('open').hide().fadeIn();
    });
});