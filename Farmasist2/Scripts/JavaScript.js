$(function () {

    var $active = true;

    $('.panel-title > a').click(function (e) {
        e.preventDefault();
    });

    $('.collapse-init').on('click', function () {
        if (!$active) {
            $active = true;
            $('.panel-title > a').attr('data-toggle', 'collapse');
            $('.panel-collapse').collapse({ 'toggle': true, 'parent': '#accordion' });
            $(this).html('Click to disable accordion behavior');
        } else {
            $active = false;
            $('.panel-collapse').collapse({ 'toggle': true, 'parent': '#accordion' });
            $('.panel-title > a').removeAttr('data-toggle');
            $(this).html('Click to enable accordion behavior');
        }
    });

});