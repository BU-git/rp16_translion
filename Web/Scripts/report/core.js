function Page() {
    this.Id = 0;
    this.Name = "";
    this.Order = 0;
    this.Questions = [];
    this.GetHTMLMarkup = function () {
        var htmlPage = $("<div>");

        for (var question in this.Questions)
            htmlPage.append(this.Questions[question].GetHTMLMarkup());

        return htmlPage;
    }
}

function RadioButtonsAnswersToHTML(answers, questionId, pageId) {
    var htmlAnswers = [];
    for (var ans in answers) {
        //var newContainer = $("<div>", {class: "containerAnswers"});
        var newRadio = $("<div>", { class: "radio" });
        var newRadioBtn = $("<input>", { name: "page" + pageId + "qa" + questionId, value: answers[ans].Name, type: "radio" });
        var newLabel = $("<label>", {}).append(newRadioBtn).append(answers[ans].Name);
        newRadio.append(newLabel);
        //newContainer.append(newRadio);
        htmlAnswers.push(newRadio);
    }

    return htmlAnswers;
}

function CheckboxesAnswersToHTML(answers, questionId, pageId) {
    var htmlAnswers = [];
    for (var ans in answers) {
        //var newContainer = $("<div>", {class: "containerAnswers"});
        var newCheckbox = $("<div>", { class: "checkbox" });
        var newCheckboxBtn = $("<input>", { name: "page" + pageId + "qa" + questionId, value: answers[ans].Name, type: "checkbox" });
        var newLabel = $("<label>", {}).append(newCheckboxBtn).append(answers[ans].Name);
        newCheckbox.append(newLabel);
        //newContainer.append(newCheckbox);
        htmlAnswers.push(newCheckbox);
    }

    return htmlAnswers;
}

function CommentToHTML(questionId, pageId) {
    var htmlAnswers = [];
    //var newContainer = $("<div>", {class: "containerAnswers"});
    var newTextarea = $("<textarea>", { class: "form-control textareaAnswer", name: "page" + pageId + "qa" + questionId, rows: 3 });
    //newContainer.append(newTextarea);
    htmlAnswers.push(newTextarea);
    return htmlAnswers;
}

function CreateAnswWrapperForCbRb() {
    var container = $('.containerAnswersModal');
    var newWrapper = $("<div>", { class: "input-group wrapperAnswer" });
    var newInput = $("<input>", { type: "text", class: "form-control" });
    var btnDel = $('<button>',
	{
	    type: "button",
	    class: "btn btn-default",
	    click: function () {
	        $(this).parent().parent().remove();
	    }
	});

    btnDel.append('X');

    var newSpan = $('<span>', { class: "input-group-btn" }).append(btnDel);
    newWrapper.append(newInput).append(newSpan);
    container.append(newWrapper);
}



function CreateAnswWrapperForCompType1() {
    var container = $('.containerAnswersModal');
    var wrapper = $('<div>', { class: "container-fluid wrapperAnswer" });
    var wrapperDatepicker = $('<div>', { class: "col-xs-3 wrapperDatepicker" });
    var inpDatePick = $('<input>', { type: "text", class: "form-control datepicker col-xs-6 compType1Datepicker" });

    wrapperDatepicker.append(inpDatePick);
    var inp = $('<input>', { type: "text", class: "form-control col-xs-2 compType1Input" });
    var cb1 = GenerateSelect("col-xs-2 compType1Select",
		["Ongeschoold", "Basisschool", "Praktijkonderwijs",
		"Mavo", "Havo", "Vwo", "MBO", "HBO", "WO"]);
    var cb2 = GenerateSelect("col-xs-2 compType1Select",
		["In afwachting", "Afgewezen", "Proefperiode", "Aangenomen"]);

    var btnDel = $('<button>',
	{
	    type: "button",
	    class: "btn btn-default",
	    click: function () {
	        $(this).parent().parent().remove();
	    }
	});

    btnDel.append('X');

    var newSpan = $('<span>').append(btnDel);

    wrapper.append(wrapperDatepicker).append(inp).append(cb1).append(inp).append(cb2);
    container.append(wrapper);
    UpdDatePickers(inpDatePick);

}


function CreateAnswWrapperForCompType2() {
    var container = $('.containerAnswersModal');
    var wrapper = $('<div>', { class: "container-fluid wrapperAnswer" });
    var wrapperDatepicker = $('<div>', { class: "col-xs-3 wrapperDatepicker" });
    var inpDatePick = $('<input>', { type: "text", class: "form-control datepicker col-xs-6 compType1Datepicker" });

    wrapperDatepicker.append(inpDatePick);
    var inp = $('<input>', { type: "text", class: "form-control col-xs-2 compType1Input" });
    var inp2 = $('<input>', { type: "text", class: "form-control col-xs-2 compType1Input" });
    var cb1 = GenerateSelect("col-xs-2 compType1Select",
		["Ongeschoold", "Basisschool", "Praktijkonderwijs",
		"Mavo", "Havo", "Vwo", "MBO", "HBO", "WO"]);

    var btnDel = $('<button>',
	{
	    type: "button",
	    class: "btn btn-default",
	    click: function () {
	        $(this).parent().parent().remove();
	    }
	});

    btnDel.append('X');

    var newSpan = $('<span>').append(btnDel);

    wrapper.append(wrapperDatepicker).append(inp).append(inp2).append(cb1);
    container.append(wrapper);
    UpdDatePickers(inpDatePick);

}

function CreateAnswWrapperForCompType3() {
    var container = $('.containerAnswersModal');
    var wrapper = $('<div>', { class: "container-fluid wrapperAnswer" });
    var wrapperDatepicker = $('<div>', { class: "col-xs-3 wrapperDatepicker" });
    var inpDatePick = $('<input>', { type: "text", class: "form-control datepicker col-xs-6 compType1Datepicker" });

    wrapperDatepicker.append(inpDatePick);
    var inp = $('<input>', { type: "text", class: "form-control col-xs-2 compType1Input" });

    var btnDel = $('<button>',
	{
	    type: "button",
	    class: "btn btn-default",
	    click: function () {
	        $(this).parent().parent().remove();
	    }
	});

    btnDel.append('X');

    var newSpan = $('<span>').append(btnDel);

    wrapper.append(inp).append(wrapperDatepicker);
    container.append(wrapper);
    UpdDatePickers(inpDatePick);

}

function GenerateSelect(classSelect, optionsText) {
    var select = $('<select>', { class: classSelect });
    for (var i = 0; i < optionsText.length; i++) {
        var option = $('<option>').append(optionsText[i]);
        select.append(option);
    }

    return select;
}

function UpdDatePickers(inpDatePick) {
    $(inpDatePick).datepicker({
        showOn: "both",
        buttonImage: "../Content/Images/dateImg.png",
        buttonImageOnly: true
    });
}

function CompType1ToHTML(asnwerCount) {
    var htmlAnswers = [];
    for (var i = 0; i < asnwerCount; i++) {
        var wrapper = $('<div>', { class: "container-fluid wrapperAnswer row" });
        var wrapperDatepicker = $('<div>', { class: "col-xs-3 wrapperDatepicker" });
        var inpDatePick = $('<input>', { type: "text", class: "form-control datepicker col-xs-6 compType1Datepicker" });

        wrapperDatepicker.append(inpDatePick);
        var inp = $('<input>', { type: "text", class: "form-control col-xs-2 compType1Input" });
        var cb1 = GenerateSelect("col-xs-2 compType1Select",
			["Ongeschoold", "Basisschool", "Praktijkonderwijs",
			"Mavo", "Havo", "Vwo", "MBO", "HBO", "WO"]);
        var cb2 = GenerateSelect("col-xs-2 compType1Select",
			["In afwachting", "Afgewezen", "Proefperiode", "Aangenomen"]);
        wrapper.append(wrapperDatepicker).append(inp).append(cb1).append(inp).append(cb2);
        UpdDatePickers(inpDatePick);
        htmlAnswers.push(wrapper);
    }

    return htmlAnswers;




}

function CompType2ToHTML(asnwerCount) {
    var htmlAnswers = [];
    for (var i = 0; i < asnwerCount; i++) {
        var wrapper = $('<div>', { class: "container-fluid wrapperAnswer row" });
        var wrapperDatepicker = $('<div>', { class: "col-xs-3 wrapperDatepicker" });
        var inpDatePick = $('<input>', { type: "text", class: "form-control datepicker col-xs-6 compType1Datepicker" });

        wrapperDatepicker.append(inpDatePick);
        var inp = $('<input>', { type: "text", class: "form-control col-xs-2 compType1Input" });
        var inp2 = $('<input>', { type: "text", class: "form-control col-xs-2 compType1Input" });
        var cb1 = GenerateSelect("col-xs-2 compType1Select",
			["Ongeschoold", "Basisschool", "Praktijkonderwijs",
			"Mavo", "Havo", "Vwo", "MBO", "HBO", "WO"]);
        wrapper.append(wrapperDatepicker).append(inp).append(inp2).append(cb1);
        UpdDatePickers(inpDatePick);
        htmlAnswers.push(wrapper);
    }

    return htmlAnswers;




}


function CompType3ToHTML(asnwerCount) {
    var htmlAnswers = [];
    for (var i = 0; i < asnwerCount; i++) {
        var wrapper = $('<div>', { class: "container-fluid wrapperAnswer row" });
        var wrapperDatepicker = $('<div>', { class: "col-xs-3 wrapperDatepicker" });
        var inpDatePick = $('<input>', { type: "text", class: "form-control datepicker col-xs-6 compType1Datepicker" });

        wrapperDatepicker.append(inpDatePick);
        var inp = $('<input>', { type: "text", class: "form-control col-xs-2 compType1Input" });
        wrapper.append(inp).append(wrapperDatepicker);
        UpdDatePickers(inpDatePick);
        htmlAnswers.push(wrapper);
    }

    return htmlAnswers;




}


function Question() {
    this.Id = 0;
    this.QuestionName = "";
    this.PageId = 0;
    this.TypeAnswer = "";
    this.Answers = [];
    this.AnswersCount = 0;
    this.GetHTMLMarkup = function () {

        var htmlAnswers = [];

        if (this.TypeAnswer == "Radiobuttons")
            htmlAnswers = RadioButtonsAnswersToHTML(this.Answers, this.Id, this.PageId);
        if (this.TypeAnswer == "Checkboxes")
            htmlAnswers = CheckboxesAnswersToHTML(this.Answers, this.Id, this.PageId);
        if (this.TypeAnswer == "Comment")
            htmlAnswers = CommentToHTML(this.Id, this.PageId);
        if (this.TypeAnswer == "CompType1")
            htmlAnswers = CompType1ToHTML(this.AnswersCount);
        if (this.TypeAnswer == "CompType2")
            htmlAnswers = CompType2ToHTML(this.AnswersCount);
        if (this.TypeAnswer == "CompType3")
            htmlAnswers = CompType3ToHTML(this.AnswersCount);

        var newDeleteIcon = $("<img>",
		{
		    class: "deleteQuestion",
		    src: "../Content/Images/deleteQuestion.png",
		    click: function () {
		        var wrapper = $(this).parent();
		        var wrapperId = wrapper.attr('id');
		        var modal = $('#deleteQuestionModal');
		        modal.attr('wrapQue', wrapperId);
		        modal.modal();
		        /*
				var qIndex = wrapperId.indexOf('q');
				var pageId = wrapperId.substring(4,qIndex);
				var questionId = wrapperId.substring(qIndex + 1);

				var pageIndex = IndexOfObjectArray(pages, "Id", pageId);
				var page = pages[pageIndex];
				var questionIndex = IndexOfObjectArray(page.Questions, "Id", questionId);
				page.Questions.splice(questionIndex, 1);

				wrapper.remove();*/
		    }
		});
        var newEditIcon = $("<img>",
		{
		    class: "editQuestion",
		    src: "../Content/Images/pen.png",
		    click: function () {
		        ClearModalQuestion();
		        var wrapper = $(this).parent();
		        var wrapperId = wrapper.attr('id');
		        var qIndex = wrapperId.indexOf('q');
		        var pageId = wrapperId.substring(4, qIndex);
		        var questionId = wrapperId.substring(qIndex + 1);

		        var pageIndex = IndexOfObjectArray(pages, "Id", pageId);
		        var page = pages[pageIndex];
		        var questionIndex = IndexOfObjectArray(page.Questions, "Id", questionId);

		        var modal = $('#myModal');
		        modal.attr('editQuestion', 'p' + pageIndex + 'q' + questionIndex);
		        var question = pages[pageIndex].Questions[questionIndex];

		        modal.find('#questionName').val(question.QuestionName);
		        $("#cbTypeAnswers").val(question.TypeAnswer);
		        $('#cbTypeAnswers').change();
		        if (question.TypeAnswer == "Radiobuttons" || question.TypeAnswer == "Checkboxes") {
		            for (var i = 0; i < question.Answers.length; i++) {
		                var container = $('.containerAnswersModal');
		                var newWrapper = $("<div>", { class: "input-group wrapperAnswer" });
		                var newInput = $("<input>", { type: "text", class: "form-control", value: question.Answers[i].Name });
		                var btnDel = $('<button>',
						{
						    type: "button",
						    class: "btn btn-default",
						    click: function () {
						        $(this).parent().parent().remove();
						    }
						});

		                btnDel.append('X');

		                var newSpan = $('<span>', { class: "input-group-btn" }).append(btnDel);
		                newWrapper.append(newInput).append(newSpan);
		                container.append(newWrapper);
		            }

		        }

		        if (question.TypeAnswer == "CompType1") {
		            for (var i = 0; i < question.AnswersCount; i++) {
		                CreateAnswWrapperForCompType1();
		            }
		        }

		        if (question.TypeAnswer == "CompType2") {
		            for (var i = 0; i < question.AnswersCount; i++) {
		                CreateAnswWrapperForCompType2();
		            }
		        }

		        if (question.TypeAnswer == "CompType3") {
		            for (var i = 0; i < question.AnswersCount; i++) {
		                CreateAnswWrapperForCompType3();
		            }
		        }

		        $('#myModal').modal();
		    }
		});
        var newNameQuestion = $("<span>", { class: "nameOfQuestion" });
        newNameQuestion.append(this.QuestionName);
        var newWrapper = $("<div>", { class: "wrapperQuestion", id: "page" + this.PageId + "q" + this.Id });
        var newContainerAns = $("<div>", { class: "containerAnswers" }).append(htmlAnswers);
        newWrapper.append(newDeleteIcon, newEditIcon, newNameQuestion, newContainerAns);

        return newWrapper;
    }
}

function UpdateNumericForPages() {
    var pagesInMarkup = $('.sortablePages').children('li');
    for (var i = 0; i < pagesInMarkup.length; i++) {
        var btnChild = $(pagesInMarkup[i]).children('button')[0];
        var indexPage = IndexOfObjectArray(pages, "Id", $(btnChild).attr('id').substring(4));
        pages[indexPage].Order = i + 1;

        //$(btnChild).html('Page ' + (i + 1).toString());
    }

}

function IndexOfObjectArray(arr, property, valueToFind) {
    for (var i = 0; i < arr.length; i++)
        if (arr[i][property] == valueToFind) return i;

    return -1;
}

function UpdateSelectedPage(btn) {
    if (currentSelectedPage)
        $(currentSelectedPage).removeClass('selectedPage');
    $(btn).addClass('selectedPage');

    currentSelectedPage = btn;
}

function AddAnswerToContainer() {
    var typeAnswer = $('#cbTypeAnswers').val()
    if (typeAnswer == 'Checkboxes' || typeAnswer == 'Radiobuttons')
        CreateAnswWrapperForCbRb();
    if (typeAnswer == 'CompType1')
        CreateAnswWrapperForCompType1();
    if (typeAnswer == 'CompType2')
        CreateAnswWrapperForCompType2();
    if (typeAnswer == 'CompType3')
        CreateAnswWrapperForCompType3();
}


var pages = [];
var currentPageId = 0;
var currentQuestionId = 0;
var currentSelectedPage;
var nameQuestionStateError = false;

function SetPageToPreview(pageIndex) {
    $('#namePageGrid').empty().append(pages[pageIndex].Name);
    var container = $('.containerQuestions').empty();
    container.attr('id', "page" + pages[pageIndex].Id);
    $('.containerQuestions').append(pages[pageIndex].GetHTMLMarkup());
}

function ClearModalQuestion() {
    var modal = $("#myModal");
    $('.containerAnswersModal').empty();
    $('#questionName').val('');
    $('#myModal').removeAttr('editQuestion');

    $('#cbTypeAnswers').val('Comment');
    CheckTypeAnswer();
}

function CheckTypeAnswer() {


    var typeAnswer = $('#cbTypeAnswers').val();
    var btnAddAnswer = $('.addAnswer');
    $('.containerAnswersModal').empty();
    if (typeAnswer != "Radiobuttons" && typeAnswer != "Checkboxes") {
        if (!$('#myModal').attr('editQuestion'))
            AddAnswerToContainer();
        btnAddAnswer.css('visibility', 'hidden');
    }
    else
        btnAddAnswer.css('visibility', 'visible');
}

$(document).ready(function () {

    function nameQuestionErrorRaise() {
        var questionName = $("#questionName");
        if (questionName && questionName.val().trim() != '')
            return true;
        $("#questionName").addClass('input-validation-error');
        $('.nameQuestionError').css({visibility: "visible", display: "block"});
        
        return false;
    }

    function clearQuestionError() {
        $("#questionName").removeClass('input-validation-error');
        $('.nameQuestionError').css({ visibility: "hidden", display: "inline" });
        nameQuestionStateError = false;
    }

    $(".sortablePages").sortable({
        handle: 'button',
        cancel: '',
        stop: function (event, ui) {
            UpdateNumericForPages();
        }
    });
    //$(".pageContainer").disableSelection();
    $('#btnAddPage').click(function () {

        $('#myModalPage').modal();
    });

    //$("#questionName").change(function () {
    //    if (nameQuestionStateError)
    //        clearQuestionError();
    //});

    $('#questionName').on('input', function () {
        if (nameQuestionStateError)
            clearQuestionError();
    });


    $("#btnSaveTest").click(function () {
        var dataToSerialize = { pages: pages };
        $.ajax({
            type: 'POST',
            url: '/Report/AddPages',
            data: JSON.stringify(dataToSerialize),
            contentType: 'application/json; charset=utf-8',
            beforeSend: function () {
                $.LoadingOverlay("show", {
                    size: "50%",
                    image: "/Content/Images/loading.gif"
                });
            },
            complete: function (jqxhr, status) {
                $.LoadingOverlay("hide");
                var message = status === "success" ?
                "Successfully added new report pattern" :
                "Error occured, please, check all pages, or try later";
                $('#ajaxStatsMessage').text(message);
                $('#ajaxResultModal').modal('show');
            },

        });
    });

    $('#btnDeleteQuestionModalOK').click(function () {

        var modal = $('#deleteQuestionModal');
        var wrapperId = modal.attr('wrapQue');
        var wrapper = $('.wrapperQuestion#' + wrapperId);

        var qIndex = wrapperId.indexOf('q');
        var pageId = wrapperId.substring(4, qIndex);
        var questionId = wrapperId.substring(qIndex + 1);

        var pageIndex = IndexOfObjectArray(pages, "Id", pageId);
        var page = pages[pageIndex];
        var questionIndex = IndexOfObjectArray(page.Questions, "Id", questionId);
        page.Questions.splice(questionIndex, 1);

        wrapper.remove();

        modal.modal('hide');
    });


    $('#btnMyModalPageOK').click(function () {

        var namePage = $('#pageNameModal').val();

        if (!namePage || namePage.trim().length == 0) {
            alert('Please enter name of page');
            return;
        }


        currentPageId++;
        var page = new Page();
        page.Id = currentPageId;
        page.Order = pages.length + 1;
        //page.Name = "Page" + currentPageId;
        page.Name = namePage;
        var newBtn = $("<button>",
		{
		    id: "page" + page.Id,
		    class: "btn btn-default btn-nav",
		    click: function () {
		        UpdateSelectedPage($(this));
		        var index = IndexOfObjectArray(pages, "Id", page.Id);
		        SetPageToPreview(index);
		    }
		});

        pages.push(page);
        newBtn.append("Page " + pages.length);
        var newLi = $("<li>").append(newBtn);
        $('#pageNameModal').val('');
        $('#myModalPage').modal('hide');

        $('.sortablePages').append(newLi).sortable('refresh');

    });


    $('#btnAddQuestion').click(function (e) {
        if (!$('.containerQuestions').attr('id')) {
            alert("Please choice a page");
            return;
        }
        ClearModalQuestion();
        //$('#myModal').removeAttr('editQuestion');
        $('#myModal').modal();

    });

    $('#btnMyModalCancel').click(function (e) {
        clearQuestionError();
        $('#myModal').modal('hide');
    });

    $('#btnMyModalOK').click(function (e) {
        var result = nameQuestionErrorRaise();
        if (!result) {
            nameQuestionStateError = true;
            return;
        }
            
        var editable = $('#myModal').attr('editQuestion');
        var question, pageId, pageIndex, page, questionIndex;

        if (!editable) {
            question = new Question();
            pageId = $('.containerQuestions').attr('id').substring(4);
            pageIndex = IndexOfObjectArray(pages, "Id", pageId);//pages[parseInt(pageId) - 1];
            page = pages[pageIndex];
            currentQuestionId++;
            question.Id = currentQuestionId;
        }
        else {
            var qIndex = editable.indexOf('q');
            pageIndex = parseInt(editable.substring(1, qIndex));
            page = pages[pageIndex];
            pageId = page.Id;
            questionIndex = parseInt(editable.substring(qIndex + 1));
            question = page.Questions[questionIndex];
        }

        question.QuestionName = $('#questionName').val();
        question.PageId = pageId;
        question.TypeAnswer = $('#cbTypeAnswers').val();
        question.Answers = [];
        question.AnswersCount = 0;

        if (question.TypeAnswer == 'Checkboxes' || question.TypeAnswer == 'Radiobuttons') {
            var container = $('.containerAnswersModal').children('.wrapperAnswer');
            for (var i = 0; i < container.length; i++) {
                question.Answers.push({ Name: $(container[i]).find("input").val() });
            }
        }

        if (question.TypeAnswer == 'CompType1' || question.TypeAnswer == 'CompType2' || question.TypeAnswer == 'CompType3') {
            var container = $('.containerAnswersModal').children('.wrapperAnswer');
            question.AnswersCount = container.length;
        }

        if (!editable)
            page.Questions.push(question);
        else
            page.Questions[questionIndex] = question;

        SetPageToPreview(pageIndex);

        $('.containerAnswersModal').empty();
        $('#questionName').val('');
        $('#myModal').removeAttr('editQuestion');
        clearQuestionError();
        $('#myModal').modal('hide');
    });

    $('#btnDeletePage').click(function () {
        $('#deletePageModal').modal();
    });

    $('#btnDeletePageModalOK').click(function () {
        var container = $('.containerQuestions');
        var pageId = container.attr('id');
        if (!pageId) {
            alert("Please choice a page");
            return;
        }
        var indexObj = IndexOfObjectArray(pages, "Id", parseInt(pageId.substring(4)));
        if (indexObj == -1) {
            alert('Internal error. Try again.')
            return;
        }
        //pages = pages[parseInt(pageId.substring(4))];
        pages.splice(indexObj, 1);
        container.empty();
        container.removeAttr('id');
        $('.pageContainer').find('#' + pageId).parent().remove();
        $('#namePageGrid').empty();
        UpdateNumericForPages();
        $('#deletePageModal').modal('hide');

    });

    $('#cbTypeAnswers').change(function (e) {

        CheckTypeAnswer();
    });






    $('#btnAddAnswer').click(function (e) {
        AddAnswerToContainer();
    });










});