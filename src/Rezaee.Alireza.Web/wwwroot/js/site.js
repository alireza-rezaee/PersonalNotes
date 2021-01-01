const englishNumbersToPersian = s => String(s).replace(/\d/g, d => '۰۱۲۳۴۵۶۷۸۹'[d]);

$(document).ready(function () {
    $('<div class="mt-5"></div>').css("height", $('#footer').outerHeight()).appendTo('body>.wrapper');
    $('.js-tooltip').tooltip();
    $('.js-gotop').click(function () {
        $('html, body').animate({ scrollTop: 0 });
    });
    $('#js-nav-shortcuts-trigger').click(function () {
        $(this).siblings('#js-nav-shortcuts').toggleClass('open').hide().fadeIn();
    });
});