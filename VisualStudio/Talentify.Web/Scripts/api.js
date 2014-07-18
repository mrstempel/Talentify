function login()
{
	$.ajax({
		url: '/Auth/Login',
		type: 'get',
		async: true,
		data: { email: $('#login-email').val(), password: $('#login-password').val() },
		success: function (data)
		{
			if (data)
			{
				location.href = "/Start";
			}
			else
			{
				location.href = "/Login/Error";
			}
		},
		error: function (request, status, error)
		{
			location.href = "/Login/Error";
		}
	});
}

function submitLoginScreenForm()
{
	var isValid = true;

	// check mandatory fields
	if (!validateMandatoryFields())
	{
		return false;
	}

	$.ajax({
		url: '/Auth/Login',
		type: 'get',
		async: true,
		data: { email: $('#login-email-screen').val(), password: $('#login-password-screen').val() },
		success: function (data)
		{
			if (data)
			{
				location.href = "/Start";
			}
			else
			{
				location.href = "/Login/Error";
			}
		},
		error: function (request, status, error)
		{
			location.href = "/Login/Error";
		}
	});
}

function logout()
{
	$.ajax({
		url: '/Auth/Logout',
		type: 'get',
		async: true,
		success: function (data)
		{
			location.href = "/Home";
		},
		error: function (request, status, error)
		{
			location.href = "/Home";
		}
	});
}

function setInputSwitch(id, url)
{
	changeInputSwitch(id);
	var switchElement = $('#' + id);
	var feedbackElement = $('#' + id + '-feedback');
	var isEnabled = $(switchElement).hasClass('on');
	$.ajax({
		url: url,
		type: 'get',
		async: true,
		data: { isEnabled: isEnabled },
		success: function (data)
		{
			if (data)
			{
				$(feedbackElement).html('Eingabe gespeichert');
				$(feedbackElement).css('display', 'inline-block');
				$(feedbackElement).fadeOut(1000);
			}
			else
			{
				changeInputSwitch(id);
				$(feedbackElement).html('Fehler beim speichern');
				$(feedbackElement).css('display', 'inline-block');
				$(feedbackElement).fadeOut(1000);
			}
		},
		error: function (request, status, error)
		{
			changeInputSwitch(id);
			$(feedbackElement).html('Fehler beim speichern');
			$(feedbackElement).css('display', 'inline-block');
			$(feedbackElement).fadeOut(1000);
		}
	});
}

function setCoachingEnabled()
{
	changeInputSwitch('enable-coaching');
	var isEnabled = $('#enable-coaching').hasClass('on');
	$.ajax({
		url: '/Profile/SetCoachingEnabled',
		type: 'get',
		async: true,
		data: { isEnabled: isEnabled },
		success: function (data)
		{
			if (data)
			{
				if (isEnabled)
				{
					$('.lernhilfe-enabled').fadeIn('fast');
				}
				else
				{
					$('.lernhilfe-enabled').fadeOut('fast');
				}
				$('#enable-coaching-feedback').html('Eingabe gespeichert');
				$('#enable-coaching-feedback').css('display', 'inline-block');
				$('#enable-coaching-feedback').fadeOut(1000);
			}
			else
			{
				changeInputSwitch('enable-coaching');
				$('#enable-coaching-feedback').html('Fehler beim speichern');
				$('#enable-coaching-feedback').css('display', 'inline-block');
				$('#enable-coaching-feedback').fadeOut(1000);
			}
		},
		error: function (request, status, error)
		{
			changeInputSwitch('enable-coaching');
			$('#enable-coaching-feedback').html('Fehler beim speichern');
			$('#enable-coaching-feedback').css('display', 'inline-block');
			$('#enable-coaching-feedback').fadeOut(1000);
		}
	});
}

function setClassDropdown(schoolListId, classListId, selectedClass)
{
	$('#' + classListId).attr("enabled", false);
	$("#" + classListId + " option[value!='0']").remove();
	$.ajax({
		url: '/FormHelper/SchoolClasses',
		type: 'get',
		async: true,
		data: { schoolId: $('#' + schoolListId).val() },
		success: function (data)
		{
			$.each(data, function(i, option)
			{
				$('#' + classListId).append($('<option/>').attr("value", option).text(option + ". Schulstufe"));
			});
			if (selectedClass != 0)
			{
				$('#' + classListId).val(selectedClass);
			}
			$('#' + classListId).attr("enabled", true);
		},
		error: function (request, status, error)
		{
		}
	});
}

function loadStream()
{
	$('#stream-container').load('/Start/Stream', function ()
	{
		$('#stream-container-loading').hide();
		$('#stream-container').fadeIn('medium');
	});
}

function loadEvents()
{
	$('#events-container').load('/Start/Events', function ()
	{
		$('#events-container-loading').hide();
		$('#events-container').fadeIn('medium');
	});
}

function loadMyProfileCoachings(isMyProfile, id)
{
	$('#my-offers').html('lade Lernhilfen ...');
	$.ajax({
		url: '/FormHelper/MyCoachingOffers',
		type: 'get',
		async: true,
		data: { userId: id },
		success: function (data)
		{
			if (data && data.length > 0)
			{
				$('#my-offers').html('');
				jQuery.each(data, function (key, val)
				{
					$('#my-offers').html($('#my-offers').html() + '<a href="javascript:void(0);" class="link-button" onclick="loadEditCoachingForm(' + val['Id'] + ')" data-ot="' + val['Comments'] + '">' + val['SubjectCategory']['Name'] + '</a>');
				});
				$("#my-offers a").each(function ()
				{
					if ($(this).attr('data-ot') != "null")
					{
						$(this).opentip($(this).attr('data-ot'), {
							extends: "alert",
							color: "#000000",
							background: "#ffffff",
							shadow: true,
							borderColor: "#e5e5e5",
							tipJoint: "bottom",
							target: true,
							hideOn: "mouseout",
							hideTriggers: ["target", "tip"]
						});
					}
				});
				$('#my-offers').show();
				$('#choaching-request-btn').show();
			}
			else
			{
				$('#my-offers').hide();
			}
		},
		error: function (request, status, error)
		{
			$('#my-offers').hide();
		}
	});
}

function loadAddCoachingForm()
{
	$('#add-coaching-frame').attr('src', '/Profile/AddCoaching');
	$('#modal-add-coaching').modal('show');
}

function loadEditCoachingForm(id)
{
	$('#edit-coaching-frame').attr('src', '/Profile/EditCoaching?id=' + id);
	$('#modal-edit-coaching').modal('show');
}

function loadEditPriceForm()
{
	$('#edit-price-frame').attr('src', '/Profile/EditCoachingPrice');
	$('#modal-edit-price').modal('show');
}

function loadRequestCoachingForm(toId, searchClass, subjectCategoryId)
{
	var requestUrl = '/Profile/CoachingRequest?toUserId=' + toId + '&searchClass=' + searchClass + '&subjectCategoryId=' + subjectCategoryId;
	$('#request-coaching-frame').attr('src', requestUrl);
	$('#modal-request-coaching').modal('show');
}

function searchCoaching()
{
	$('#search-results').hide();
	$('#search-loading').show();
	$('#search-results').load('/Search/Search?Class=' + $('#Class').val() + '&SubjectCategoryId=' + $('#SubjectCategoryId').val(), function ()
	{
		$('#search-loading').hide();
		$('#search-results').fadeIn('medium');
	});
}

function loadEventRegistrationForm(id)
{
	$('#event-register-frame').attr('src', '/Events/Register/' + id);
	$('#modal-event-register').modal('show');
}

function loadEventOpenSeats(id)
{
	$('#event-open-seats').html('lade freie Plätze ...');
	$.ajax({
		url: '/FormHelper/GetEventOpenSeats',
		type: 'get',
		async: true,
		data: { eventId: id },
		success: function (data)
		{
			if (data)
			{
				$('#event-open-seats').html(data);
			}
			else
			{
				$('#event-open-seats').html('0');
			}
		},
		error: function (request, status, error)
		{
			$('#event-open-seats').html('0');
		}
	});
}

function sendMessage(conversationId, fromUserId, toUserId, targetId, text)
{
	$.ajax({
		url: '/FormHelper/SendMessage',
		type: 'get',
		async: true,
		data: { conversationId: conversationId, fromUserId: fromUserId, toUserId: toUserId, targetId: targetId, text: text },
		success: function (data)
		{
			if (data)
			{
				var item = document.createElement('div');
				$(item).addClass('item');
				$(item).html('<div class="profile-img"><a href="/Profile/Index/' + data['UserId'] + '" class="image-link" style="background: url(\'' + data['UserImage'] + '\');"></a></div><div class="content"><h3>' + data['Username'] + '<span>' + data['CreatedDate'] + '</span></h3><p>' + data['Text'] + '</p></div></div>');
				$(item).appendTo($('#timeline-container'));
				$('#coaching-new-message').val('');
				$('#coaching-new-message-btn').hide();
			}
		},
		error: function (request, status, error)
		{
		}
	});
}

function checkNotifications()
{
	$.ajax({
		url: '/Notification/Count',
		type: 'get',
		async: true,
		success: function (data)
		{
			setNotificationCount(data, true);
		},
		error: function (request, status, error)
		{
		}
	});
}

function setNotificationCount(count, doFadeIn)
{
	var navLink = $('#notification-link');
	var icon = $(navLink).find('img');
	var imgSrc = $(icon).attr('id') + '-active.png';
	var lastKlammer = document.title.lastIndexOf("(");

	if (count && count != "0")
	{
		$('#notification-count').html(count);
		$(icon).attr('src', '/Images/' + imgSrc);
		if (doFadeIn)
		{
			$('#notification-count').fadeIn('fast');
		}
		else
		{
			$('#notification-count').show();
		}

		if (lastKlammer != -1)
			document.title = document.title.substring(0, lastKlammer) + ' (' + count + ')';
		else
			document.title = document.title + ' (' + count + ')';

		$(navLink).addClass('sticky');
	}
	else
	{
		imgSrc = $(icon).attr('id') + '.png';
		$(icon).attr('src', '/Images/' + imgSrc);
		$('#notification-count').hide();
		if (lastKlammer != -1)
			document.title = document.title.substring(0, lastKlammer);

		$(navLink).removeClass('sticky');
	}
}

function loadNotifactionlist()
{
	$('#notification-list').hide();
	$('#notification-list-loading').show();
	$('#notification-list').load('/Notification/PopupList', function ()
	{
		$('#notification-list-loading').hide();
		$('#notification-list').fadeIn('medium');
	});
}