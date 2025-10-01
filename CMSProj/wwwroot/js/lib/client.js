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


    (function () {
        loc = window.location.pathname.slice(1).trim();
        const bod = {
            magic: '7b3f9b7f-6b0c-4a8a-9b1a-0e5b2d1a9f11',
            'key': loc == "" ? "home" : loc
        }
        console.log(bod.key)
        fetch('https://localhost:7250/api/Counter', {
            method: 'PUT',
            headers: {'Content-Type':'application/json'},
            body: JSON.stringify(bod)
        })
        .then(res => res.text().then(x => {
            if (!res.ok) throw new Error(x.status);
            return x;
        }).then(y => JSON.parse(y)))
        .then(x => console.info(x.key))
        .catch(err => { console.error(err); console.log(err.message||err) })
    })();
})