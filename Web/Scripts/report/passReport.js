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
                $('input, textarea').each(function() {
                    var name = $(this).attr('name');
                    var type = $(this).attr('type');
                    var value = $(this).val();
                    var labelId = "view" + name;

                    var element = document.getElementById(labelId);

                    if (element != null) {
                        if (type === 'checkbox') {
                            $("input:checkbox[name=" + name + "]:checked").each(function() {
                                element.innerHTML += value;
                            });
                        } else {
                            element.innerHTML = value;
                        }

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