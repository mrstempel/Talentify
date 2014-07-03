var showErrorMarkers = false;

function validateEmail(email)
{
	var re = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
	return re.test(email);
}

function initMandatoryFields()
{
	$('.mandatory').keyup(function() { checkMandatoryField($(this)); });
	$('.mandatory').change(function() { checkMandatoryField($(this)); });
}

function checkMandatoryField(field)
{
	if ($(field).val().length > 0)
	{
		$(field).css('background-image', 'none');
		$(field).removeClass('error');
	} else
	{
		if ($(field).nodeName.toLowerCase() === 'input')
			$(field).css('background-image', 'url(/Images/mandatory.png)');

		if (showErrorMarkers)
			$(field).addClass('error');
	}
}

function changeInputSwitch(switchId)
{
	var inputSwitch = $('#' + switchId);
	
	if ($(inputSwitch).hasClass('on'))
	{
		$(inputSwitch).removeClass('on');
		$(inputSwitch).addClass('off');
	}
	else
	{
		$(inputSwitch).removeClass('off');
		$(inputSwitch).addClass('on');
	}
}

function initForms()
{
	console.log("init form");
	$("textarea[maxlength]").bind('input propertychange', function ()
	{
		var maxLength = $(this).attr('maxlength');
		if ($(this).val().length > maxLength)
		{
			$(this).val($(this).val().substring(0, maxLength));
		}
	});

	initMandatoryFields();
}

function validateMandatoryFields()
{
	showErrorMarkers = true;
	var isValid = true;
	$('.mandatory').each(function ()
	{
		if ($(this).val().length > 0)
		{
			$(this).removeClass('error');
		}
		else
		{
			$(this).addClass('error');
			isValid = false;
		}
	});

	return isValid;
}

function showErrorMsg(msgHeadline, msgText, autoclose)
{
	if (msgHeadline.length > 0)
		$('#msg-error h2').html(msgHeadline);

	if (msgText.length > 0)
		$('#msg-error p').html(msgText);

	$('#msg-error').show();
	$('#msg-container').fadeIn('fast');
	scrollToDiv('msg-container');

	if (autoclose)
	{
		window.setTimeout("$('#msg-container').fadeOut('slow');", 2000);
	}
}

function showSuccessMsg(msgHeadline, msgText, autoclose)
{
	if (msgHeadline.length > 0)
		$('#msg-success h2').html(msgHeadline);

	if (msgText.length > 0)
		$('#msg-success p').html(msgText);

	$('#msg-success').show();
	$('#msg-container').fadeIn('fast');
	scrollToDiv('msg-container');

	if (autoclose)
	{
		window.setTimeout("$('#msg-container').fadeOut('slow');", 2000);
	}
}

/* specific form validation */
function validateRegisterForm()
{
	var isValid = true;
	var errorHeadline = "Fehler bei Anmeldung";
	var errorText = "";

	// check mandatory fields
	if (!validateMandatoryFields())
	{
		return false;
	}

	// check e-mail syntax
	if (!validateEmail($('#Email').val()))
	{
		$('#Email').addClass('error');
		errorText += "Bitte gib eine gültige E-Mail Adresse an.";
		isValid = false;
	}

	// check password length
	if ($('#Password').val().length < 6)
	{
		$('#Password').addClass('error');
		if (errorText.length > 0)
			errorText += "<br/>";
		errorText += "Dein Passwort muss mind. 6 Zeichen lang sein.";
		isValid = false;
	}

	// check if correct school was selected
	if ($('#SchoolId').val() == "0" || $('#SchoolSelectName').val() == "")
	{
		$('#SchoolSelect').addClass('error');
		if (errorText.length > 0)
			errorText += "<br/>";
		errorText += "Bitte wähle eine vorgeschlagene Schule aus.";
		isValid = false;
	}

	if (!isValid)
	{
		showErrorMsg(errorHeadline, errorText, false);
	}

	return isValid;
}

function validateProfileForm()
{
	var isValid = true;
	var errorHeadline = "Fehler beim Speichern";
	var errorText = "";

	// check mandatory fields
	if (!validateMandatoryFields())
	{
		return false;
	}

	// check e-mail syntax
	if (!validateEmail($('#Email').val()))
	{
		$('#Email').addClass('error');
		errorText += "Bitte gib eine gültige E-Mail Adresse an.";
		isValid = false;
	}

	// check if correct school was selected
	if ($('#SchoolId').val() == "0" || $('#SchoolSelectName').val() == "")
	{
		$('#SchoolSelect').addClass('error');
		if (errorText.length > 0)
			errorText += "<br/>";
		errorText += "Bitte wähle eine vorgeschlagene Schule aus.";
		isValid = false;
	}

	if (!isValid)
	{
		showErrorMsg(errorHeadline, errorText, false);
	}

	return isValid;
}

function validateCoachingForm()
{
	var isValid = true;

	// check mandatory fields
	if (!validateMandatoryFields())
	{
		return false;
	}

	return isValid;
}

function validateCoachingPrice()
{
	var isValid = true;
	var errorHeadline = "Fehler beim Speichern";
	var errorText = "Bitte gib einen gültigen Preis an (max. 10€)";

	var checkPrice = parseFloat($('#price').val());
	if (isNaN(checkPrice) || checkPrice <= 0 || checkPrice > 10)
	{
		isValid = false;
	}

	if (!isValid)
	{
		showErrorMsg(errorHeadline, errorText, false);
	}

	return isValid;
}

function checkDeleteCoachingOffer()
{
	if (confirm('Willst du diese Lernhilfe wirklich löschen?'))
	{
		$('#deleteOffer').val('true');
		return true;
	}

	return false;
}

function validatePasswordForm()
{
	var isValid = true;
	var errorHeadline = "Fehler beim Speichern";
	var errorText = "Deine Passwortwiederholung ist nicht korrekt!";

	// check mandatory fields
	if (!validateMandatoryFields())
	{
		return false;
	}

	if ($('#NewPassword1').val().length < 6)
	{
		$('#NewPassword1').addClass('error');
		$('#NewPassword2').addClass('error');
		errorText = "Dein Passwort muss mindestens 6 Zeichen lang sein!";
		isValid = false;
	}

	if ($('#NewPassword1').val() != $('#NewPassword2').val())
	{
		$('#NewPassword2').addClass('error');
		isValid = false;
	}

	if (!isValid)
	{
		showErrorMsg(errorHeadline, errorText, false);
	}

	return isValid;
}