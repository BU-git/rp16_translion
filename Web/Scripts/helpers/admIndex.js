(function () {
    function makeActive(elem, actClass, inactClass) {
        elem.removeClass(inactClass).addClass(actClass);
    };

    function makeInactive(elem, actClass, inactClass) {
        elem.removeClass(actClass).addClass(inactClass);
    };
    $(document).ready(function () {
        var active = 'tlr-usr-category-active';
        var inactive = 'tlr-usr-section-category';
        var advBtn = $('#advBtn');
        var emplBtn = $('#emplBtn').addClass(active);
        var advisors = $('#advisors').hide();
        var employers = $('#employers');
        


        advBtn.click(function () {
            advisors.show();
            employers.hide();
            makeActive(advBtn, active, inactive);
            makeInactive(emplBtn, active, inactive);
        });

        emplBtn.click(function () {
            advisors.hide();
            employers.show();
            makeActive(emplBtn, active, inactive);
            makeInactive(advBtn, active, inactive);
        });
    });
}());