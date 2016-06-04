/*Changes size of current slick after adding answer*/
function expandSlick(elem, slick) {
    var elemHeight = elem.outerHeight(true);
    slick.css('height', slick.height() + elemHeight + 'px');
}


jQuery.fn.outerHTML = function () {
    return jQuery('<div />').append(this.eq(0).clone()).html();
};

$(document).ready(function () {
    $('.carousel').slick({
        infinite: false,
        slidesToShow: 1,
        adaptiveHeight: true
    });
});

$(document).ready(function () {
    $('.addAnswerLineBtn').click(function () {
        var regAnswerName = /p[0-9]+q[0-9]+t[0-9]l[0-9]+/g;
        var html = $("<div />").append($(this).closest('.questionComplicated').find('.answerLine:last').clone()).html();
       
        function nameReplacer(match) {
            var reg = /l(.*)/;
            var len = match.length;

            var curLineId = reg.exec(match)[1];
            var newLineId = (parseInt(curLineId) + 1).toString();

            var name = match.slice(0, len - curLineId.length) + newLineId;
            return name;
        }

        html = html.replace(regAnswerName, nameReplacer);

        $(this).closest('.questionComplicated').find('#answersContainer').append(html);

        var newAnswer = $(this).closest('.questionComplicated').find('.answerLine:last');
        expandSlick(newAnswer, $('.slick-list'));

        $('.ui-datepicker-trigger').each(function() {
                $(this).remove();
            }
        );

        $('.datepicker').each(function () {
            $(this).removeClass('hasDatepicker');
            $(this).removeAttr('id');
            $(this).datepicker({
                showOn: "both",
                buttonImage: "../../Content/Images/dateImg.png",
                buttonImageOnly: true
            });
        });
    });

    $('.delAnswerLineBtn').click(function () {
        var nChildren = $(this).closest('.questionComplicated').find('#answersContainer').children().length;
        if (nChildren > 1)
            $(this).closest('.questionComplicated').find('.answerLine:last').remove();
    });

    $('.datepicker').datepicker({
        showOn: "both",
        buttonImage: "../../Content/Images/dateImg.png",
        buttonImageOnly: true
    });

    var pageNum = parseInt(document.getElementById("PageCounterId").getAttribute("value")) + 3;
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
                                    element.innerHTML += value + ' ';
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

                    $('input, select', $(this)).each(function() {
                        var question = $(this).attr('placeholder');
                        var value = $(this).val();

                        if ($.inArray(question, names) == -1) {
                            names.push(question);
                        }
                        values.push(value);
                    });

                    var element = document.getElementById(labelId);

                    var insertHtml = '<table class="table table-striped myTable"><thead><tr>';
     
                    for (var j = 0; j < names.length; j++) {
                        insertHtml += '<th>' + names[j] + '</th>';
                    }
                    insertHtml += '</tr></thead><tbody><tr>';

                    for (j = 0; j < values.length; j++) {
                        insertHtml += '<td>' + values[j] + '</td>';
                        if ((j + 1) % names.length == 0) {
                            insertHtml += '</tr><tr>';
                        }
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