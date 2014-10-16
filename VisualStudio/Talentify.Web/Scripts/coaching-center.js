function filterCoachingCenter(requestId)
{
	$('#coaching-center-results').hide();
	$('#coaching-center-loading').show();
	$('#coaching-center-results').load('/CoachingCenter/Filter?statusType=' + $('#StatusType').val() + '&requestId=' + requestId, function ()
	{
		$('#coaching-center-loading').hide();
		$('#coaching-center-results').fadeIn('medium');
	});
}

function loadCoachingRequestTimeline(requestId)
{
	$('.request-list').find('.item').each(function () { $(this).removeClass('active'); });
	$('#request-item-' + requestId).addClass('active');
	$('#request-stream').hide();
	$('#request-stream-loading').show();
	$('#request-stream').load('/CoachingCenter/Stream?coachingRequestId=' + requestId, function ()
	{
		$('#request-stream-loading').hide();
		$('#request-stream').fadeIn('medium');
	});
}

function updateCoachingRequestStatus(coachingRequestId, status)
{
	$.ajax({
		url: '/CoachingCenter/UpdateStatus',
		type: 'get',
		async: true,
		data: { coachingRequestId: coachingRequestId, status: status },
		success: function (data)
		{
			if (data && !data.error)
			{
				$('#status-update-error').hide();
				// implemented in coaching-center.js
				addCoachingRequestStatusToTimeline(data.status);
			}
			else
			{
				$('#status-update-error').fadeIn();
			}
		},
		error: function (request, status, error)
		{
		}
	});
}

function addCoachingRequestStatusToTimeline(status)
{
	// add status to timeline
	$('#timeline-container').append('<div class="item ' + status.type + '"><div class="profile-img"><a href="/Profile/Index/' + status.userId + '" class="image-link" style="background: url(\'' + status.image + '\');"></a></div><div class="content"><h3>' + status.username + '</h3><p>' + status.message + '</p></div></div>');

	// show new status options
	if (status.type == 'confirm')
	{
		$('#status-update-confirm').hide();
	}
}

function loadCoachingRequestCompleteForm(coachingRequestId, status)
{
	$('#status-update-complete').hide();
	$('#status-update-complete-loading').show();
	$('#status-update-complete').load('/CoachingCenter/CompleteCoachingRequest?coachingRequestId=' + coachingRequestId + '&status=' + status, function ()
	{
		$('#status-update-complete-loading').hide();
		$('#status-update-complete').fadeIn('medium');
	});
}

function submitCoachingConfirmForm(coachingRequestId, validateDate)
{
	
	if (validateCoachingConfirmForm(validateDate))
	{
		if (validateDate)
		{
			var day = $('#coachingdate_day').val();
			var month = $('#coachingdate_month').val();
			var year = $('#coachingdate_year').val();
			var date = year + "-" + month + "-" + day;
			
			$.ajax({
				url: '/CoachingCenter/SetCoachingRequestRatingWithDate',
				type: 'get',
				async: true,
				data: { coachingRequestId: coachingRequestId, val1: $('#wertRatingValue').val(), val2: $('#puenktlichRatingValue').val(), val3: $('#preisRatingValue').val(), date: date, duration: $('#duration').val(), payedPrice: $('#payedPrice').val() },
				success: function (data)
				{
					if (data)
					{
						updateCoachingRequestStatus(coachingRequestId, 2);
						disableCoachingFeedbackForm();
					}
					else
					{
						alert("Fehler");
					}
				},
				error: function (request, status, error) { }
			});
		}
		else
		{
			$.ajax({
				url: '/CoachingCenter/SetCoachingRequestRating',
				type: 'get',
				async: true,
				data: { coachingRequestId: coachingRequestId, val1: $('#wertRatingValue').val(), val2: $('#puenktlichRatingValue').val(), val3: $('#preisRatingValue').val() },
				success: function (data)
				{
					if (data)
					{
						updateCoachingRequestStatus(coachingRequestId, 2);
						disableCoachingFeedbackForm();
					}
					else
					{
						alert("Fehler");
					}
				},
				error: function (request, status, error) { }
			});
		}
	}
}

function disableCoachingFeedbackForm()
{
	// disable feedback links
	$('.values').find('a').each(function ()
	{
		$(this).removeClass('selectable');
		$(this).addClass('disabled');
		$(this).removeAttr('onclick');
		$(this).removeAttr('onmouseover');
		$(this).removeAttr('onmouseout');
	});

	$('.switch').find('a').each(function ()
	{
		$(this).removeClass('selectable');
		$(this).addClass('disabled');
		$(this).removeAttr('onclick');
		$(this).removeAttr('onmouseover');
		$(this).removeAttr('onmouseout');
	});

	// hide date form
	$('#date-form').hide();

	// hide buttons
	$('#confirm-btn-container').hide();
	$('#cancel-btn-container').hide();

	// hide messaging form
	$('#new-message-form').hide();
}

function setCoachingInOutFilter(requestId)
{
	var filter = $('#in-out-select').val();
	var streamId = $('.request-list').find('.item:first').attr('id');
	if (filter == 'both')
	{
		$('#selected-in-out-filter').attr('src', '/Images/inandout.png');
		$('.request-list').find('.item.in').show('fast');
		$('.request-list').find('.item.out').show('fast');
		console.log("both");
	}

	if (filter == "in")
	{
		$('#selected-in-out-filter').attr('src', '/Images/icon-links.png');
		$('.request-list').find('.item.out').slideUp('fast');
		$('.request-list').find('.item.in').slideDown('fast');
		streamId = $('.request-list').find('.item.in:first').attr('id');
		console.log("in");
	}

	if (filter == "out")
	{
		$('#selected-in-out-filter').attr('src', '/Images/icon-rechts.png');
		$('.request-list').find('.item.in').slideUp('fast');
		$('.request-list').find('.item.out').slideDown('fast');
		streamId = $('.request-list').find('.item.out:first').attr('id');
		console.log("out");
	}

	loadCoachingRequestTimeline(streamId.substring(13));
}

function setMyCoachingRatings(myVal1, myVal2, myVal3)
{
	// disable form
	disableCoachingFeedbackForm();
	// show my values
	$('#link-wert-' + myVal1).prevAll().addClass('selected');
	$('#link-wert-' + myVal1).addClass('selected');

	$('#puenktlich-switch').removeClass('on');
	var puenktlichStatus = myVal2 != 0 ? "on" : "off";
	$('#puenktlich-switch').addClass(puenktlichStatus);

	$('#preis-switch').removeClass('on');
	var preisStatus = myVal3 != 0 ? "on" : "off";
	$('#preis-switch').addClass(preisStatus);
}

function setOtherCoachingRatings(myVal1, myVal2, myVal3)
{
	// show other values
	$('#span-wert-' + myVal1).prevAll().addClass('selected');
	$('#span-wert-' + myVal1).addClass('selected');
	
	var puenktlichStatus = myVal2 != 0 ? "on" : "off";
	$('#puenktlich-other-switch').addClass(puenktlichStatus);

	var preisStatus = myVal3 != 0 ? "on" : "off";
	$('#preis-other-switch').addClass(preisStatus);
}

function hideOtherCoachingRatings(requestId)
{
	$('#other-rating-' + requestId).hide();
}

function rejectCoachingRequest(coachingRequestId)
{
	$.ajax({
		url: '/CoachingCenter/CancelRequest',
		type: 'get',
		async: true,
		data: { coachingRequestId: coachingRequestId, reason: null },
		success: function (data)
		{
			if (data && !data.error)
			{
				$('#status-update-error').hide();
				// hide confirm options
				$('#status-update-confirm').hide();
				// hide messaging form
				$('#new-message-form').hide();
				addCoachingRequestStatusToTimeline(data.status);
			}
			else
			{
				$('#status-update-error').fadeIn();
			}
		},
		error: function (request, status, error) { }
	});
}

function cancelCoachingRequest(coachingRequestId, reason)
{
	if (validateMandatoryFields())
	{
		$.ajax({
			url: '/CoachingCenter/CancelRequest',
			type: 'get',
			async: true,
			data: { coachingRequestId: coachingRequestId, reason: reason },
			success: function (data)
			{
				if (data && !data.error)
				{
					$('#status-update-error').hide();
					disableCoachingFeedbackForm();
					addCoachingRequestStatusToTimeline(data.status);
				}
				else
				{
					$('#status-update-error').fadeIn();
				}
			},
			error: function (request, status, error) { }
		});
	}
}