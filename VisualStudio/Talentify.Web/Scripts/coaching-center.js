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
				data: { coachingRequestId: coachingRequestId, val1: $('#wertRatingValue').val(), val2: $('#puenktlichRatingValue').val(), val3: $('#preisRatingValue').val(), date: date, duration: $('#duration').val() },
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
	});

	// hide date form
	$('#date-form').hide();

	// hide buttons
	$('#confirm-btn-container').hide();
	$('#cancel-btn-container').hide();

	// hide messaging form
	$('#new-message-form').hide();
}

function setMyCoachingRatings(myVal1, myVal2, myVal3)
{
	// disable form
	disableCoachingFeedbackForm();
	// show my values
	$('#link-wert-' + myVal1).addClass('selected');
	$('#link-puenktlich-' + myVal2).addClass('selected');
	$('#link-preis-' + myVal3).addClass('selected');
}

function setOtherCoachingRatings(myVal1, myVal2, myVal3)
{
	// show other values
	$('#span-wert-' + myVal1).addClass('selected');
	$('#span-puenktlich-' + myVal2).addClass('selected');
	$('#span-preis-' + myVal3).addClass('selected');
}

function cancelCoachingRequest(coachingRequestId, reason)
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

function cancelCoachingRequestWithReason(coachingRequestId, reason)
{
	if (validateMandatoryFields())
	{
		cancelCoachingRequest(coachingRequestId, reason);
	}
}