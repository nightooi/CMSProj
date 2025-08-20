(function () {
    'use strict';
    $(function () {
        var $forms = $('.needs-validation');
        $forms.on('submit', function (e) {
            var form = this;
            if (form.checkValidity() === false) {
                e.preventDefault();
                e.stopPropagation();
            }
            $(form).addClass('was-validated');
        });
    });
})();

/*  Tiny jQuery Mobile demo: show an alert after collapsible is opened  */
$(document).on('collapsibleexpand', '[data-role="collapsible"]', function () {
    var heading = $(this).find('h3').text();
    console.log('Expanded section: ' + heading);
});