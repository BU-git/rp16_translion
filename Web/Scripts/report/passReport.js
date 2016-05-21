$(document).ready(function () {
    $('.carousel').slick({
        infinite: false,
        slidesToShow: 1,
        adaptiveHeight: true
    });
});

$(document).ready(function () {
    $('.datepicker').datepicker({
        showOn: "both",
        buttonImage: "../../Content/Images/dateImg.png",
        buttonImageOnly: true
    });

    var pageNum = parseInt(document.getElementById("PageCounterId").getAttribute("value")) + 2;
    var pageArray = [];

    for (var i = 0; i <= pageNum; ++i) {
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
                    $('input, textarea', $(this)).each(function () {
                        var id = '#' + $(this).attr('id');
                        var name = $(this).attr('name');
                        var type = $(this).attr('type');
                        var value = $(this).val();
                        var labelId = "view" + name;

                        var element = document.getElementById(labelId);

                        if (element != null) {
                            if (type === 'checkbox' || type === 'radio') {
                                if ($(id).is(":checked")) {
                                    element.innerHTML += value + ', ';
                                }
                            } else {
                                element.innerHTML = value;
                            }
                        }
                    });
                });
                $('.questionComplicated').each(function () {
                    var labelId = "view" + $(this).attr('id');
                    var names = [];
                    var values = [];

                    $(this).children('input, select').each(function() {
                        var question = $(this).attr('placeholder');
                        var value = $(this).val();
                        names.push(question);
                        values.push(value);
                    });

                    var element = document.getElementById(labelId);

                    var insertHtml = '<table class="table table-striped myTable"><thead><tr>';
     
                    for (var j = 0; j < names.length; j++) {
                        insertHtml += '<th>' + names[j] + '</th>';
                    }
                    insertHtml += '</tr></thead><tbody><tr>';

                    for (j = 0; j < names.length; j++) {
                        insertHtml += '<td>' + values[j] + '</td>';
                    }
                    insertHtml += '</tr></tbody><table>';

                    if (element != null) {
                        element.innerHTML = insertHtml;
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