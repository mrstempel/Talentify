function loadFrage(f)
{
	$('#frage-container').load('/Talentecheck/Frage?f=' + f, function ()
	{
		$('#frage-counter').html(f + "/10");
		$('#frage-loading').hide();
		for (i = 1; i <= f; i++)
		{
			$('#progress' + i).removeClass('progress-empty');
			$('#progress' + i).addClass('progress-filled');
		}
		setUmfrageNavButtons(f);
	});
}

function setAnswer(f)
{
	var answerId = 0;
	// check if question is answered
	for (i = 1; i <= 4; i++)
	{
		if ($('#a' + i).prop('checked'))
		{
			answerId = i;
		}
	}

	if (answerId == 0)
	{
		// not answered
		alert("Bitte wähle eine Antwort aus um zur nächsten Frage zu kommen");
		return false;
	}

	$.ajax({
		url: '/TalenteCheck/SetAnswer',
		type: 'get',
		async: true,
		data: { f: f, a: answerId },
		success: function (data)
		{
			if (data)
			{
				if (f == 10)
				{
					location.href = "/Talentecheck/Result";
				}
				else
				{
					loadFrage(f + 1);
				}
			}
			else
			{
				alert("Speicherfehler");
			}
		},
		error: function (request, status, error)
		{
			alert("Speicherfehler");
		}
	});
}

function setUmfrageNavButtons(f)
{
	if (f == 1)
	{
		// hide back btn
		$('#col-back-btn').hide();
	}
	else
	{
		$('#col-back-btn').show();
		$('#col-back-btn').unbind("click");
		$('#col-back-btn').click(function()
		{
			loadFrage(f - 1);
		});
	}

	
	$('#col-next-btn').show();
	$('#col-next-btn').unbind("click");
	$('#col-next-btn').click(function ()
	{
		setAnswer(f);
	});
}

function shareFacebook(url, image, description)
{
	//alert("url: " + url);
	//FB.ui({
	//	method: 'share',
	//	href: 'https://developers.facebook.com/docs/',
	//}, function (response) { alert(response); });

	FB.ui({
		method: 'feed',
		link: url,
		picture: 'https://www.talentify.me/Images/talentecheck/' + image,
		name: 'Der große Talentecheck',
		caption: 'Der große Talentecheck',
		description: description
	},
	function (response)
	{
		if (response != null)
		{
			$.ajax({
				url: '/TalenteCheck/AddShareBonus',
				type: 'get',
				async: true,
				data: { network: 'fb' },
				success: function (data)
				{
				},
				error: function (request, status, error)
				{
				}
			});
		}
	});

	//window.open('https://www.facebook.com/sharer/sharer.php?u=' + encodeURIComponent(url), 'facebook-share-dialog', 'width=626,height=436');
	//return false;
}

function shareGoogle(url)
{
	//alert(url);
	window.open('https://plus.google.com/share?url=' + encodeURIComponent(url), '', 'menubar=no,toolbar=no,resizable=yes,scrollbars=yes,height=600,width=600');
	return false;
}

function shareTwitter(url)
{
	//alert(url);
	window.open('https://twitter.com/share?url=' + encodeURIComponent(url), '', 'menubar=no,toolbar=no,resizable=yes,scrollbars=yes,height=600,width=600');
	return false;
}