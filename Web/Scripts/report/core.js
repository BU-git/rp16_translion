function Page()
{
	this.Id = 0;
	this.Name = "";
	this.Order = 0;
	this.Questions = [];
	this.GetHTMLMarkup = function()
	{
		var htmlPage = $("<div>");

		for(var question in this.Questions)
			htmlPage.append(this.Questions[question].GetHTMLMarkup());

		return htmlPage;
	}
}

function RadioButtonsAnswersToHTML(answers, questionId, pageId)
{
	var htmlAnswers = [];
	for(var ans in answers)
	{
		var newContainer = $("<div>", {class: "containerAnswers"});
		var newRadio = $("<div>", {class: "radio"});
		var newRadioBtn = $("<input>", {name: "page" + pageId + "qa" + questionId, value: answers[ans].Name, type: "radio"});
		var newLabel = $("<label>", {}).append(newRadioBtn).append(answers[ans].Name);
		newRadio.append(newLabel);
		newContainer.append(newRadio);
		htmlAnswers.push(newContainer);
	}

	return htmlAnswers;
}

function CheckboxesAnswersToHTML(answers, questionId, pageId)
{
	var htmlAnswers = [];
	for(var ans in answers)
	{
		var newContainer = $("<div>", {class: "containerAnswers"});
		var newCheckbox = $("<div>", {class: "checkbox"});
		var newCheckboxBtn = $("<input>", {name: "page" + pageId + "qa" + questionId, value: answers[ans].Name, type: "checkbox"});
		var newLabel = $("<label>", {}).append(newCheckboxBtn).append(answers[ans].Name);
		newCheckbox.append(newLabel);
		newContainer.append(newCheckbox);
		htmlAnswers.push(newContainer);
	}

	return htmlAnswers;
}

function CommentToHTML(questionId, pageId)
{
	var htmlAnswers = [];
	var newContainer = $("<div>", {class: "containerAnswers"});
	var newTextarea = $("<textarea>", {class: "form-control textareaAnswer", name: "page" + pageId + "qa" + questionId, rows: 3});
	newContainer.append(newTextarea);
	htmlAnswers.push(newContainer);
	return htmlAnswers;
}


function Question()
{
	this.Id = 0;
	this.QuestionName = "";
	this.PageId = 0;
	this.TypeAnswer = "";
	this.Answers = [];
	this.AnswersCount = 0;
	this.GetHTMLMarkup = function()
	{

		var htmlAnswers = [];

		if(this.TypeAnswer == "Radiobuttons")
			htmlAnswers = RadioButtonsAnswersToHTML(this.Answers, this.Id, this.PageId);
		if(this.TypeAnswer == "Checkboxes")
			htmlAnswers = CheckboxesAnswersToHTML(this.Answers, this.Id, this.PageId);
		if(this.TypeAnswer == "Comment")
			htmlAnswers = CommentToHTML(this.Id, this.PageId);

		var newDeleteIcon = $("<img>", 
		{
			class: "deleteQuestion", 
			src: "/Content/Images/5.1 Report.png",
			click: function()
			{
				var wrapper = $(this).parent();
				var wrapperId = wrapper.attr('id');
				var modal = $('#deleteQuestionModal');
				modal.attr('wrapQue',wrapperId);
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
			src: "/Content/Images/pen.png",
			click: function()
			{
				var wrapper = $(this).parent();
				var wrapperId = wrapper.attr('id');
				var qIndex = wrapperId.indexOf('q');
				var pageId = wrapperId.substring(4,qIndex);
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
				if(question.TypeAnswer != "Comment")
				{
					for(var i=0; i<question.Answers.length; i++)
					{
						var container = $('.containerAnswersModal');
						var newWrapper = $("<div>", {class: "input-group wrapperAnswer"});
						var newInput = $("<input>", {type: "text", class: "form-control", value: question.Answers[i].Name});
						var btnDel = $('<button>', 
						{
							type: "button", 
							class: "btn btn-default",
							click: function()
							{
								$(this).parent().parent().remove();
							}
						});

						btnDel.append('X');

						var newSpan = $('<span>', {class: "input-group-btn"}).append(btnDel);
						newWrapper.append(newInput).append(newSpan);
						container.append(newWrapper);
					}

				}

				$('#myModal').modal();
			}
		});		
		var newNameQuestion = $("<span>", {class: "nameOfQuestion"});
		newNameQuestion.append(this.QuestionName);
		var newWrapper = $("<div>", {class: "wrapperQuestion", id: "page" + this.PageId + "q" + this.Id});
		newWrapper.append(newDeleteIcon, newEditIcon, newNameQuestion, htmlAnswers);

		return newWrapper;
	}
}

function UpdateNumericForPages()
{
	var pagesInMarkup = $('.sortablePages').children('li');
	for(var i = 0; i<pagesInMarkup.length; i++)
	{
		var btnChild = $(pagesInMarkup[i]).children('button')[0];
		var indexPage = IndexOfObjectArray(pages, "Id", $(btnChild).attr('id').substring(4));
		pages[indexPage].Order = i + 1;
		
		$(btnChild).html('Page ' + (i + 1).toString());
	}

}

function IndexOfObjectArray(arr, property, valueToFind)
{
	for(var i = 0; arr.length; i++)
		if(arr[i][property] == valueToFind) return i;

	return -1;
}

function UpdateSelectedPage(btn)
{
	if(currentSelectedPage)
		$(currentSelectedPage).removeClass('selectedPage');
	$(btn).addClass('selectedPage');
	
	currentSelectedPage = btn;
}



var pages = [];
var currentPageId = 0;
var currentQuestionId = 0;
var currentSelectedPage;

function SetPageToPreview(pageIndex)
{
	$('#namePageGrid').empty().append(pages[pageIndex].Name);
	var container = $('.containerQuestions').empty();
	container.attr('id', "page"+pages[pageIndex].Id);
	$('.containerQuestions').append(pages[pageIndex].GetHTMLMarkup());
}

$(document).ready(function() {

	$(".sortablePages").sortable({ 
		handle: 'button',
		cancel: '',
		stop: function(event, ui)
		{
			UpdateNumericForPages();
		}
	});
  //$(".pageContainer").disableSelection();
	$('#btnAddPage').click(function()
	{
		$('#myModalPage').modal();
	});

    

	$("#btnSaveTest").click(function() {
	    var dataToSerialize = { pages: pages };
		$.ajax({
            type: 'POST',
            url: '/Report/AddPages',
            data: JSON.stringify(dataToSerialize),
            contentType: 'application/json; charset=utf-8',
            beforeSend: function() {
                $.LoadingOverlay("show", {
                    size: "50%",
                    image: "/Content/Images/loading.gif"
                });
            },
            complete: function(jqxhr, status) {
                $.LoadingOverlay("hide");
                var message = status === "success" ?
                    "Successfully added new report pattern" :
                    "Error occured, please, check all pages, or try later";
                $('#ajaxStatsMessage').text(message);
                $('#ajaxResultModal').modal('show');
            },

		});
	});
	
	$('#btnDeleteQuestionModalOK').click(function(){

		var modal = $('#deleteQuestionModal');
		var wrapperId = modal.attr('wrapQue');
		var wrapper = $('.wrapperQuestion#'+ wrapperId);

		var qIndex = wrapperId.indexOf('q');
		var pageId = wrapperId.substring(4,qIndex);
		var questionId = wrapperId.substring(qIndex + 1);

		var pageIndex = IndexOfObjectArray(pages, "Id", pageId);
		var page = pages[pageIndex];
		var questionIndex = IndexOfObjectArray(page.Questions, "Id", questionId);
		page.Questions.splice(questionIndex, 1);

	    wrapper.remove();

		modal.modal('hide');
	});


	$('#btnMyModalPageOK').click(function()
	{

		var namePage = $('#pageNameModal').val();

		if(!namePage || namePage.trim().length == 0)
		{
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
			click: function()
			{
				UpdateSelectedPage($(this));
				SetPageToPreview(page.Order - 1);
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
		if(!$('.containerQuestions').attr('id'))
		{
			alert("Please choice a page");
			return;
		}

		$('#myModal').modal();

	});

	$('#btnMyModalOK').click(function(e){
		var editable = $('#myModal').attr('editQuestion');
		var question, pageId, pageIndex, page, questionIndex;

		if(!editable)
		{
			question = new Question();
			pageId = $('.containerQuestions').attr('id').substring(4);
				pageIndex = IndexOfObjectArray(pages, "Id", pageId);//pages[parseInt(pageId) - 1];
				page = pages[pageIndex];
				currentQuestionId++;
				question.Id = currentQuestionId;
			}
			else
			{
				var qIndex = editable.indexOf('q');
				pageIndex = parseInt(editable.substring(1,qIndex));
				page = pages[pageIndex];
				pageId = page.Id;
				questionIndex = parseInt(editable.substring(qIndex+1));
				question = page.Questions[questionIndex];
			}
			
			question.QuestionName = $('#questionName').val();
			question.PageId = pageId;
			question.TypeAnswer = $('#cbTypeAnswers').find("option:selected").text();
			question.Answers = [];
			question.AnswersCount = 0;

			if(question.TypeAnswer != 'Comment')
			{
				var container = $('.containerAnswersModal').children('.wrapperAnswer');
				for(var i = 0; i<container.length; i++)
				{
					question.Answers.push({ Name: $(container[i]).find("input").val() });
				}
			}
			
			if(!editable)
				page.Questions.push(question);
			else
				page.Questions[questionIndex] = question;

			SetPageToPreview(pageIndex);

			$('.containerAnswersModal').empty();
			$('#questionName').val('');

			$('#myModal').modal('hide');
		});

	$('#btnDeletePage').click(function(){
		$('#deletePageModal').modal();
	});

	$('#btnDeletePageModalOK').click(function(){
		var container = $('.containerQuestions'); 
		var pageId = container.attr('id');
		if(!pageId)
		{
			alert("Please choice a page");
			return;
		}
		var indexObj = IndexOfObjectArray(pages, "Id", parseInt(pageId.substring(4)));
		if(indexObj == -1)
		{
			alert('Internal error. Try again.')
			return;
		}
		//pages = pages[parseInt(pageId.substring(4))];
		pages.splice(indexObj, 1);
		container.empty();
		container.removeAttr('id');
		$('.pageContainer').find('#'+pageId).parent().remove();
		UpdateNumericForPages();
		$('#deletePageModal').modal('hide');

	});

	$('#cbTypeAnswers').change(function (e) {
		var optionSelected = $(this).find("option:selected");
		var typeAnswer = optionSelected.text();
		var btnAddAnswer = $('.addAnswer');
		if(typeAnswer == "Comment")
		{
			btnAddAnswer.css('visibility', 'hidden');
			$('.containerAnswersModal').empty();
		}     	
		else
			btnAddAnswer.css('visibility', 'visible');
	});


	$('#btnAddAnswer').click(function(e){
		var container = $('.containerAnswersModal');
		var newWrapper = $("<div>", {class: "input-group wrapperAnswer"});
		var newInput = $("<input>", {type: "text", class: "form-control"});
		var btnDel = $('<button>', 
		{
			type: "button", 
			class: "btn btn-default",
			click: function()
			{
				$(this).parent().parent().remove();
			}
		});

		btnDel.append('X');

		var newSpan = $('<span>', {class: "input-group-btn"}).append(btnDel);
		newWrapper.append(newInput).append(newSpan);
		container.append(newWrapper);

	});

	

	

	




});