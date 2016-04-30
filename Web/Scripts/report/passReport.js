$(document).ready(function () {
    $('.carousel').slick({
        infinite: false,
        slidesToShow: 1,
        adaptiveHeight: true
    });
});

$(document).ready(function () {
    var pageNum = parseInt(document.getElementById("PageCounterId").getAttribute("value")) + 1;
    var pageArray = [];

    pageArray[0] = { topLabel: ' ', value: 0 }

    for (i = 1; i <= pageNum; ++i) {
        pageArray[i] = { topLabel: i, value: i };
    }

    var pages = {
        currentValue: 1,
        steps: pageArray
    }

    $('#progressbar').stepProgressBar(pages);

    $('.slick-next').click(function () {
        if (pages.currentValue < pageNum) {
            $('#progressbar').stepProgressBar('setCurrentValue', ++pages.currentValue);
        }
    });
    $('.slick-prev').click(function () {
        if (pages.currentValue > 1)
            $('#progressbar').stepProgressBar('setCurrentValue', --pages.currentValue);
    });
});