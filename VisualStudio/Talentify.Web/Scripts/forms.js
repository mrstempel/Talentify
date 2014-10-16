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
		if ($(field).nodeName && $(field).nodeName.toLowerCase() === 'input')
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
function validateDate(prefix)
{
	var day = $('#' + prefix + '_day').val();
	var month = $('#' + prefix + '_month').val();
	var year = $('#' + prefix + '_year').val();
	var date = day + "." + month + "." + year;

	var matches = /^(\d{2})[.\/](\d{2})[.\/](\d{4})$/.exec(date);
	if (matches == null) return false;
	var m = matches[2] - 1;
	var d = matches[1];
	var y = matches[3];

	var composedDate = new Date(y, m, d);

	return composedDate.getDate() == d &&
            composedDate.getMonth() == m &&
            composedDate.getFullYear() == y;
}

function validateMandatoryOnlyForm()
{
	var isValid = true;

	// check mandatory fields
	if (!validateMandatoryFields())
	{
		return false;
	}

	return isValid;
}

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
	if ($('#SchoolSelectName').val() == "")
	{
		$('#SchoolSelect').addClass('error');
		if (errorText.length > 0)
			errorText += "<br/>";
		errorText += "Bitte wähle eine Schule aus.";
		isValid = false;
	}

	if (!isValid)
	{
		showErrorMsg(errorHeadline, errorText, false);
	}

	return isValid;
}

function validateTeacherRegisterForm()
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
	if ($('#SchoolSelectName').val() == "")
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

function validateSurvey()
{
	// check mandatory fields
	if (!validateMandatoryFields())
	{
		return false;
	}

	if ($('#HearedOfTalentifyOption').val() == "Sonstige" && $('#HearedOfTalentifyText').val() == "")
	{
		$('#HearedOfTalentifyText').addClass('error');
		return false;
	}
	else
	{
		$('#HearedOfTalentifyText').removeClass('error');
	}

	return true;
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

function validateCoachingConfirmForm(doValidateDate)
{
	var isValid = true;
	var errorMsg = 'Bitte bewerte den Termin vollständig!';
	
	if ($('#wertRatingValue').val() == '' || $('#puenktlichRatingValue').val() == '' || $('#preisRatingValue').val() == '')
		isValid = false;

	if (isValid && doValidateDate && !validateDate('coachingdate'))
	{
		isValid = false;
		errorMsg = 'Bitte trage das Datum und die Dauer des Termins ein!';
	}


	if (!isValid)
		showErrorMsg(errorMsg, '', false);
	else
		$('#msg-container').hide();

	return isValid;
}