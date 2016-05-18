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
                    $(this).children('input, textarea').each(function () {
                        var id = '#' + $(this).attr('id');
                        var name = $(this).attr('name');
                        var type = $(this).attr('type');
                        var value = $(this).val();
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
                $('.questionComplicated').each(function () {
                    var labelId = null;
                    var names = [];
                    var values = [];

                    $(this).children('input, select').each(function() {
                        var question = $(this).attr('placeholder');
                        var value = $(this).val();
                        names.push(question);
                        values.push(value);

                        var name = $(this).attr('name');
                        labelId = "view" + name.slice(0, 4);
                    });

                    var element = document.getElementById(labelId);

                    var insertHtml = '';
                    for (var i = 0; i < names.length; i++) {
                        var add = names[i] + ": " + values[i] + "\n";
                        insertHtml += add;
                    }

                    if (element != null) {
                        element.innerHTML = insertHtml;
                    }
                    //var date = new Date($(this).find('input[type="date"]').val());
                });
            }
        }
    });
    $('.slick-prev').click(function () {
        if (pages.currentValue > 1)
            $('#progressbar').stepProgressBar('setCurrentValue', --pages.currentValue);
    });
});