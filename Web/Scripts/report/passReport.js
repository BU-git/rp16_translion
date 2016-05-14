$(document).ready(function () {
    $('.carousel').slick({
        infinite: false,
        slidesToShow: 1,
        adaptiveHeight: true
    });
});

$(document).ready(function () {
    var pageNum = parseInt(document.getElementById("PageCounterId").getAttribute("value")) + 2;
    var pageArray = [];

    //pageArray[0] = { topLabel: ' ', value: 0 }

    for (i = 0; i <= pageNum; ++i) {
        pageArray[i] = { topLabel: ' ', value: i };
    }

    var pages = {
        currentValue: 1,
        steps: pageArray
    }

    $('#progressbar').stepProgressBar(pages);

    $('.slick-next').click(function () {
        if (pages.currentValue < pageNum) {
            $('#progressbar').stepProgressBar('setCurrentValue', ++pages.currentValue);

            if (pages.currentValue === pageNum) {
                $('.question').each(function() {
                    $('input, textarea').each(function () {
                        var id = '#' + $(this).attr('id');
                        var name = $(this).attr('name');
                        var type = $(this).attr('type');
                        var value = $(this).attr('placeholder');
                        var labelId = "view" + name;

                        var element = document.getElementById(labelId);

                        if (element != null) {
                            if (type === 'checkbox' || type === 'radio') {
                                if ($(id).is(":checked")) {
                                    element.innerHTML += value + '\n';
                                }
                            } else {
                                element.innerHTML = value;
                            }
                        }
                    });
                });
                $('.questionComplicated').each(function() {
                    var date = new Date($(this).find('input[type="date"]').val());
                    var id = $(this).attr('id');
                    var labelId = "view" + id;

                    var element = document.getElementById(labelId);

                    if (element != null) {
                        element.innerHTML = "Date: " + date.toISOString().slice(0, 10);
                    }
                });
            }
        }
    });
    $('.slick-prev').click(function () {
        if (pages.currentValue > 1)
            $('#progressbar').stepProgressBar('setCurrentValue', --pages.currentValue);
    });
});