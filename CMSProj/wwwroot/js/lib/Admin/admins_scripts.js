$(document).ready(function () {

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
    function hide(ele) {
        $(ele).addClass("d-none");
    }
    function show(ele) {
        $(ele).removeClass("d-none");
    }
    function linkEdOp(c) {
        const text = prompt("text for navigation item")
        const link = prompt("link for navigation item")
        $t = fTarget(c);
        if ($t.is('a')) {
            $t.text(text)
            $t.attr('href', link)
        }
        $llink = $t.find('a');
        $llink.attr('href', link)
        $llink.text(text)
    }
    function delOp(c) {
        $(c).remove()
    }
    function imgeEdOp(c) {
        const text = prompt("text for image")
        const link = prompt("src for image")
        $t = fTarget(c);
        if ($t.is('img')) {
            $t.attr('src', link)
            $t.attr('alt', text)
        }
        $img = $t.find('img')

        $img.attr('src', link);
        $img.attr('src', link);

    }
    function fTarget(c) {
        if ($(c).hasClass('target')) return $(c);
        return $(c).find('.target').first()
    }
    function parEdOp(c) {
        const text = prompt("paragraph text")
        $t = fTarget(c)
        if ($t.is('p')) { $t.text(text); return; }

        $t.find('p').text(text)
    }
    function headingEdOp(c) {

        const hval = prompt("header num, 1-6")
        const parsed = parseInt(hval, 10)
        if (Number.isNaN(parsed) || parsed < 1 || parsed > 6) {
            alert('wrong input, aborting')
            return
        }
        const text = prompt("header text")
        const $t = fTarget(c);

        if ($t.length == 0) throw new DOMException("shit aint be working")

        const cls = ($t[0].className || "")
        const style = $t.attr("style") || ""

        const $new = $(`<h${parsed}>`)
            .addClass(cls)
            .attr("style", style)
            .text(text)
        
        $t.replaceWith($new)
        attachAdminEvHandler($new.parent(), $new.parent().parent().find('.nav-item-opts').first())
    }
    function cssOp(a) {
        const $t = ($(a).hasClass('target') ? $(a) : $(a).find('.target').first());
        if (!$t.length) {
            console.warn('no .target found'); return;
        }

        if (confirm('(OK = classes, Cancel = inline styles)')) {
            const cls = prompt("Enter CSS classes to add (space-separated)", "");
            if (cls && cls.trim()) $t.addClass(cls.trim());
            return;
        }

        const current = ($t.attr('style') || "").trim();
        const input = prompt("(e.g. 'color:red; margin:4px;')", current);
        if (input == null)
            return;

        $t.attr('style', input)
    }
    const opItem = {
        'A': linkEdOp,
        'IMG': imgeEdOp,
        'H': headingEdOp,
        'P': parEdOp,
    }
    function AdminHandlers(del, css, ops) {
        this.delop = del,
            this.cssop = css,
            this.edop = ops
    }
    function c$(v) {
        if ((typeof v == 'string' || v instanceof String) && v.startsWith('#'))
            return $(cl).find("." + v.slice(1).trim()).first()

        return $(cl).find(v).first();
    }
    function attachClientItem(elem_id, cb_mutate_placed) {
        placed = placeElem($(cl), $(elem_id))
        denotation(placed, 'client')
        attachAdminElement(placed)
        cb_mutate_placed(placed)
    }
    const ops = new AdminHandlers(delOp, cssOp, opItem)
    function cm$(v) {

        if ((typeof v == 'string' || v instanceof String) && v.startsWith('#'))
            return $(cl).find("." + v.slice(1).trim());

        return $(cl).find(v);
    }
    function CHook(o, f) {
        this.obj = o,
            this.fun = f
    }
    function CPlace(p, c) {
        this.parent = p,
            this.child = c,
            this.id = "";
    }
    let deletedTemplateComponentIds = [];
    let _timer;
    let elemList = []
    let hookList = []
    let placeList = []
    const nvitemOp = '#nav-item-opts'
    const nvItem = '#h-nav-item'
    const nviAB = '#add-item'
    const iAB = "#add-ima"
    const lAB = '#add-link'
    const teAB = '#add-text'
    const haAB = '#add-heading'
    const nvAB = '#add-nav'
    const cl = '#clientArea';
    const blm = '#nav-item-opts';
    const cmpm = '#component-manager';
    const h = 'header'
    const hUl = '#header-ul'
    const neA = '#nav-edit-activate'
    const cmpmParent = $(cmpm).parent();
    //created content is umanaged

    function denotation(wrapped_elem, type) {
        switch (type) {
            case 'admin':
                wrapped_elem.addClass('adminelem')
                break;
            case 'client':
                wrapped_elem.addClass('clientelem')
                break;
            default:
                throw new DOMException('WRONG TYPE')
                break;
        }
    }
    function addNavItem() {
        navitem()
    }
    function navitem() {
        item = placeElem(c$('ul'), $('#h-nav-item'))
        console.log(item);
        denotation(item, 'client')
        attachAdminElement(item)
        ops.edop['A'](item)
    }

    function placeElem(elemp, elemc) {
        function moveIdToClass($el) {
            const id = ($el.attr('id') || '').trim()
            if (id) {
                $el.addClass(id).removeAttr('id');
            }
        }
        function moveIntoPlace(elemp, elemc) {
            function move(elemp, elemc) {

                $elemp = elemp
                $elemc = elemc
                $cl = $(cl).first()
                $clone = $(elemc).clone(false)

                $($clone).find("*").each((i, x) => {
                    at = $(x).attr('id')
                    if (at != undefined || at != null)
                        moveIdToClass($(x))
                })
                if (elemp == undefined) {
                    moveIdToClass($clone)
                    return;
                }

                moveIdToClass($clone)

                if ($elemp && $elemp.attr('id') === $cl.attr('id')) {
                    $clone.appendTo($cl)
                    return $clone;
                }
                $clone.appendTo($elemp)
                return $clone;
            }
            return move(elemp, elemc)
        }
        function removeFromPlace() {
            function moveOut(o) {
                $(o.parent).remove($(o.child))
            }
            placeList.map(moveOut)
        }

        if (elemp && elemc) {
            return moveIntoPlace(elemp, elemc);
        }
        placeList.map((x) => moveIntoPlace(x.parent, x.child))
        placeList = []
        $('.adminelem').each((i, x) => {
            placeList.push(new CPlace(undefined, $(x)))
        })
        moveIntoPlace(elemp, elemc)
    }
    //
    // Abstraction here is: Do the attachment into the client area with a method responsible for attaching only client elements.
    // then ask the method responsible for attaching interactive managing elements (admin elemens) to attach the admin elems
    // on the client elems
    //
    function attachAdminElement(client_item) {
        if (client_item === undefined || $(client_item).length == 0)
            return
        client_parent = $(client_item).find('.target').first().parent();
        aElem = placeElem(client_parent, $('#nav-item-opts'))
        denotation(aElem, 'admin')
        attachAdminEvHandler(client_item, aElem)
    }
    function attachAdminEvHandler(client_item, admin_elem) {
        elem = admin_elem
        if (elem == undefined)
            return;
        const t = $(client_item).find('.target').first();
        if (!t.length) return;

        target = t[0].tagName;
        target = target.startsWith('H') ? target.slice(0, 1) : target;
        admin_elem.find('button').each((i, x) => {
            type = $(x).attr('data-type')
            if (!type)
                return;

            $elem = $(x);
            switch (type) {
                case 'style':
                    $elem.off('click').on('click', () => ops.cssop(t))
                    break;
                case 'remove':
                    $elem.off('click').on('click', () => ops.delop(client_item))
                    break;
                case 'edit':
                    $elem.off('click').on('click', () => ops.edop[target](t))
                    break;
                default:
                    noop();
                    break;
            }
        })
    }
    function addHeaderNav(cb) {
        if (!cb()) return;

        function asserHeaderExists() {
            return $(cl).find(h).children().length > 0;
        }
        function buildheader() {
            if (asserHeaderExists()) {
                function clearPrev() {
                    $(cl).children(h).empty();
                    hookList = new Array()
                    placeList = new Array()
                }
                clearPrev();
                const sH = $($(cl).children(h).first());
                const hUlC = $($(hUl).clone());
                hUlC.addClass(hUl.slice(1).trim())
                hUlC.prependTo(sH);
                placeList.push(new CPlace(c$(hUlC), $(neA)))
                placeElem();
                console.log($(c$(hUl).find('button')));
                hookList.push(new CHook($(c$(neA).find('button')), addNewMenuList));
                siteBuilderLoop();
                addNewMenuList()
            }
            else {
                const $header = $('<header>');
                $(cl).prepend($header);
                buildheader();
            }
        }
        buildheader();
    }
    function registerHook(hl) {
        hl.forEach((e) => {
            console.log($(cl).find(e.obj));
        })
    }

    function siteBuilderManager(state) {
        $(".adminelem").each((i, x) => {
            stateInvoker(state,
                () => show(x),
                () => hide(x))
        })
    }
    function siteBuilderLoop() {
        function cR(e) {
            $(e.obj).click(e.fun)
        }
        hookList.map(cR)
        hookList = new Array()
    }

    function stateInvoker(state, cb_OnState, cb_OffState) {
        switch (state) {
            case 'enable':
                cb_OnState()
                break;
            case 'disable':
                cb_OffState()
                break;
        }
    }
    function adminElemSanitizer() {
        const clC = $(cl).clone(false)

        clC.children().each((i, x) => {
        })
    }

    function enableEdit(cb) {
        $('#page-edit-menu >').children().children().each((i, x) => {
            if (x.id != 'de') $(x).addClass("d-none")
            switch (x.id) {

                case 'ap':
                    $(x).click(() => {
                        //invoke add page api
                        console.log("ad page invoked");
                    });
                    break;
                case 'dp':
                    $(x).click(() => {
                        console.log("delete page invoked");
                    });
                    break;
                case 'ce':
                    $(x).click(() => {
                        console.log("commit edit invoked");
                    })
                    break;
                case 'de':
                    $(x).click(() => {
                        $(x).siblings().each((i, y) => {
                            if (cb != null)
                                cb("enable")
                            if (($(y).hasClass("d-none"))) {
                                show(y)
                                return;
                            }
                            cb("disable")
                            hide($(y))
                        })
                    })
                    break;
            }
        })
    }
    function addHeading() {
        attachClientItem($('#heading-def'), ops.edop['H'])
    }
    function addText() {
        attachClientItem($('#paragraph-def'), ops.edop['P'])
    }
    function addImage() {
        attachClientItem($('#image-def'), ops.edop['IMG'])
    }
    function addLink() {
        attachClientItem($('#url-def'), ops.edop['A'])
    }

    const run = (function () {
        function s(i) {
            return "."+i.slice(1)
        }
        placeList.push(new CPlace($('header').first(), $(cmpm)));
        placeElem();
        hookList.push(new CHook($(s(nvAB)), () => addHeaderNav(() => confirm("current header will be overridden"))));
        hookList.push(new CHook($(s(haAB)), addHeading));
        hookList.push(new CHook($(s(teAB)), addText));
        hookList.push(new CHook($(s(lAB)), addLink));
        hookList.push(new CHook($(s(iAB)), addImage));
        $(cl).attr('ran', 'no')
    })

    function elementReAttachment() {

        $(cl).find('*[data-cmsrootcompguid').each(function () {
            if ($(this).find('.adminelement').length == 0)
                attachAdminElement($(this))
        })

        hUlC = $(cl).find('.' + hUl.slice(1))
        if (hUlC && (hUlC).find('.' + neA.slice(1)).length == 0) {
            placeList.push(new CPlace(c$(hUlC), $(neA)))
            placeElem();
            console.log($(c$(hUl).find('button')));
            hookList.push(new CHook($(c$(neA).find('button')), addNewMenuList));
            siteBuilderLoop();
            reattachMenuList();
           
        }
    }

    function clearTemplates() {

        $elems = $(cl).children('[data-cmsrootcompguid]')
            .filter(function () { return $(this)[0].tagName != 'HEADER' })
        $elems.each(function() {
            if ($(this).find('.target').length > 0) return;
            deletedTemplateComponentIds.push($(this).attr('data-cmsrootcompguid'))
            $(this).remove()
        })
    }
    let __menuSeq = 0;

    function ensureListId($list) {
        if (!$list.attr('id')) $list.attr('id', 'menu_list_' + (++__menuSeq));
        return $list.attr('id');
    }

    function initSortableList(el) {
        if (!el || el.__sorted) return;
        new Sortable(el, {
            group: 'menus',
            animation: 150,
            draggable: '.list-group-item',
            ghostClass: 'drag-ghost',
        });
        el.__sorted = true;
    }

    function addListItem($list) {
        const $li = $('<div class="list-group-item flex-grow p-0 d-flex align-items-center"><a class="flex-grow-1 d-block py-2 px-3 text-decoration-none target" href="#">New Item</a></div>');
        attachAdminElement($li);
        $list.append($li);
        denotation($li, 'client');
    }

    function wireWrapperButtons($wrapper) {
        const $list = $wrapper.find('.menu-list').first();
        ensureListId($list);
        attachAdminElement($wrapper)
        const $btnAddItem = placeElem($wrapper.find('.menu-list-header').first(), $('#nav-edit-activate'));
        denotation($btnAddItem, 'admin');
        $btnAddItem.find('button').attr('title', 'Add Menu Item').text('+Item').off('click').on('click', function (e) {
            e.stopPropagation();
            addListItem($list);
        });
    }
    function reattachMenuList() {
        document.querySelectorAll('.menu-list').forEach(el => {
            $(el).parent()
            initSortableList(el)
        });
        li = $(cl).find('.menu-list-wrapper');
        li.each(function () {
            wireWrapperButtons($(this))
            $(this).find('.menu-list > .list-group-item').each((i, x) => attachAdminElement(x))
        })
    }
    function addNewMenuList() {
        const $host = $(cl).find('.header-ul').first();
        if ($host.length == 0) return;
        const $wrap = $('#lili-wrapper').clone(false, false).removeAttr('id');
        const $list = $wrap.find('.menu-list').first();
        ensureListId($list);
        $host.children().first().append($wrap);
        wireWrapperButtons($wrap);
        initSortableList($list[0]);
        $wrap.find('.list-group-item').each(function () {
            const $it = $(this);
            denotation($it, 'client');
            attachAdminElement(fTarget($it));
        });
    }
    function pureSlug(url = undefined) {
        let surl = String(url).toLowerCase();
        if (url == undefined)
            surl = window.location.pathname.toLowerCase();

        u = surl.startsWith('admin/') || surl.startsWith('/admin/') ? surl.slice(6) : surl;
        return u.startsWith('/') ? u.slice(1) : u;
    }
    function showStatus(ok, code, text) {
        const $b = $('#cms-status');
        const $t = $('#cms-status-text');
        $b.removeClass('d-none ok err');
        $b.addClass(ok ? 'ok' : 'err');
        $t.text((ok ? 'OK ' : 'ERR ') + (code ?? '') + (text ? (' - ' + text) : ''));
        clearTimeout($b.data('hidet'));
        const hidet = setTimeout(() => $b.addClass('d-none'), 4500);
        $b.data('hidet', hidet);
    }

    // payload builders
    function buildDeletes() {
        dels = []
        if (deletedTemplateComponentIds.length == 0) {
            $(cl).children("[data-cmsrootcompguid]").each((i, x) => {
                dels.push(
                    {
                        choice: 1,
                        contentType: 0,
                        value: "string",
                        pageOrder: 0,
                        componentGuid: $(x).attr("data-cmsrootcompguid")
                    }
                )
            })
            return dels;
        }
        dels = (deletedTemplateComponentIds || []).map(g => ({
            choice: 1,
            contentType: 0,
            value: "string",
            pageOrder: 0,
            componentGuid: g
        }));
        deletedTemplateComponentIds = []
        return dels;
    }
    function buildAdds() {
        adds = []
        $(cl).children().filter(function () {
            return $(this)[0].tagName != 'HEADER'
        }).each((i, x) => {
            adds.push({
                choice: 0,
                contentType: 0,
                value: serializeOuter(x),
                pageOrder: i,
                componentGuid: "00000000-0000-0000-0000-000000000000"
            })
        })
        return adds
    }
    function serializeOuter(el) {
        const $c = $(el).clone(true, false);
        $c.find('.adminelem, #nav-item-opts, #component-manager,'
            + ' .page-edit-menu, .nav-item-opts, .component-manager, .nav-edit-activate').remove();

        $c.find('.adminelem').removeClass('adminelem');

        $c.find('.drag-ghost').removeClass('drag-ghost');
        $c.find('[draggable]').removeAttr('draggable');
        if ($c.is('header')) return $c[0].outerHTML

        return $c[0].outerHTML;
    }
    // calls
    async function apiCreatePage() {
        const pg = prompt('url')
        const body = { Url: pureSlug(pg) };
        try {
            const res = await fetch('/api/ContentManager', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(body)
            });
            showStatus(res.ok, res.status);
        } catch (e) { showStatus(false, 0, 'network'); }
    }

    async function apiDeletePage() {
        if (!confirm("delete this page?")) return;

        const body = { Url: pureSlug() };
        try {
            const res = await fetch('/api/ContentManager', {
                method: 'DELETE',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(body)
            });
            showStatus(res.ok, res.status);
        } catch (e) { showStatus(false, 0, 'network'); }
    }
    async function apiUpdateMenu() {
        $header = $($(cl).find("header").first());
        guid = $header.attr('data-cmsrootcompguid')
        const body = {
            url: pureSlug(),
            cms_root_component: guid,
            cmsedit_subcomponent: guid,
            choice: 3,
            value: serializeOuter($header.children().first())
        };
        try {
            const res = await fetch('/api/ContentManager/SubComp', {
                method: 'PATCH',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(body)
            })
            showStatus(res.ok, res.status);
        } catch (e) {
            showStatus(false, 0, res.text().json())
        }
    }

    async function apiUpdatePageAdds() {
        await apiUpdateCall(buildAdds())
    }
    async function apiUpdateCall(items) {

        const payload = {
            url: pureSlug(),
            contentEdits: [...items]
        };
        console.log(payload.contentEdits)
        try {
            console.log(payload)
            const res = await fetch('/api/ContentManager', {
                method: 'PUT',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(payload)
            });
            showStatus(res.ok, res.status);
            return res;

        } catch (e) { showStatus(false, 0, 'network'); }
    }
    async function apiUpdatePageDeletes() {
        await apiUpdateCall(buildDeletes())
    }
    async function apiUpdatePage() {
        var adds = buildAdds();
        console.log(`adds ::: ${adds}`, adds);
        var del = buildDeletes();
        let res = {}
        if (del.length > 0)
            res = await apiUpdateCall(del)

        await apiUpdateCall(adds)
        await apiUpdateMenu();
    }

    $(document).on('click', '#ap', function (e) { e.stopPropagation(); apiCreatePage(); });
    $(document).on('click', '#dp', function (e) { e.stopPropagation(); apiDeletePage(); });
    $(document).on('click', '#ce', function (e) { e.stopPropagation(); apiUpdatePage(); });
    clearTemplates()
    run();
    enableEdit(siteBuilderManager);
    siteBuilderLoop();
    attachAdminElement()
    elementReAttachment();

   });

