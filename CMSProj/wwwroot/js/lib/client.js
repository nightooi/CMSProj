$(document).ready(() => {
    $(document).on('click', '.menu-list-header', function () {
        $(this).next('.menu-list').toggleClass('d-none');
    });
    $(document).on('click', '.menu-trigger', function (e) {
        e.stopPropagation();
        const $list = $(this).find('.menu-list').first();
        if ($list.hasClass('collapsed')) {
            $list.removeClass('d-none');
        } else {
            $list.addClass('d-none');
        }
    });
})