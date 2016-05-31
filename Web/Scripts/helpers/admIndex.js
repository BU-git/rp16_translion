(function () {
    function makeActive(elem, actClass, inactClass) {
        elem.removeClass(inactClass).addClass(actClass);
    };

    function makeInactive(elem, actClass, inactClass, btn) {
        elem.removeClass(actClass).addClass(inactClass);
    };

    function switchAction(elem, href, text) {
        elem.attr('href', href);
        elem.children().last().text(text);
    };

    $(document).ready(function () {
        var active = 'tlr-usr-category-active';
        var inactive = 'tlr-usr-section-category';
        var advBtn = $('#advBtn');
        var emplBtn = $('#emplBtn').addClass(active);
        var advisors = $('#advisors').hide();
        var employers = $('#employers');
        var addBtn = $('#tlr-usr-section-hdr-adm-btn');



        advBtn.click(function () {
            advisors.show();
            employers.hide();
            makeActive(advBtn, active, inactive);
            makeInactive(emplBtn, active, inactive);
            switchAction(addBtn, '/Admin/AddAdvisor/', 'Adviseur toevoegen');
        });

        emplBtn.click(function () {
            advisors.hide();
            employers.show();
            makeActive(emplBtn, active, inactive);
            makeInactive(advBtn, active, inactive);
            switchAction(addBtn, '/Admin/RegisterEmployer/', 'Werkgever toevoegen');
        });
    });
}());