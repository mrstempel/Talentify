var topMargin = 85;
var waypointOffset = topMargin;
var maximizeSections = true;
var hasWaypoints = false;

causeRepaintsOn = $("h1, h2, h3, p, div");

function getMaskSize()
{
	var size = new Object();
	size.width = $(window).innerWidth();
	size.height = $(window).innerHeight();
	return size;
}

function scrollToDiv(divId)
{
	$('html, body').animate({
			scrollTop: $("#" + divId).offset().top - 100
	}, 1000);
}

function initSectionSizes()
{
	if (maximizeSections)
	{
		causeRepaintsOn.css("z-index", 1);
		var maskSize = getMaskSize();
		//var minHeight = (maskSize.height - topMargin);
		//console.log('masksize height: ' + maskSize.height);
		//$('#page-container > section').css('min-height', minHeight + 'px');
		$('section.auto-height').css('min-height', '10px');
		//console.log('bg width: ' + $('.big-bg').width());
		var aspect = 622 / 1480;
		var bgHeight = $('.big-bg').width() * aspect;
		$('.big-bg').css('height', bgHeight + 'px');
		//console.log('.big-bg height: ' + $('.big-bg').height());
		var labelLeft = parseInt(($('.big-bg').width() - 1105) / 2);
		if (labelLeft < 0)
		{
			labelLeft = 0;
		}
		$('.big-bg > .label').css('left', labelLeft + 'px');
		if ($('.big-bg').width() < 1440)
		{
			var labelBottom = parseInt($('.big-bg').height() - bgHeight);
			$('.big-bg > .label').css('bottom', labelBottom + 'px');
		}
		else
		{
			$('.big-bg > .label').css('bottom', '0px');
		}
	}
}

function initWaypoints()
{
	$('#page-container > section').each(function ()
	{
		$(this).waypoint(function (direction)
		{
			if (direction === 'down')
				highlightNavigation($(this).prop('id'));
		},
		{ offset: topMargin - 19 });

		$(this).waypoint(function (direction)
		{
			if (direction === 'up')
				highlightNavigation($(this).prop('id'));
		},
		{ offset: topMargin - 30 });

	});
}

function highlightNavigation(id)
{
	$('nav').find('a').removeClass('selected');
	$('nav').find('li').removeClass('selected');
	$('#nav-' + id).addClass('selected');
}

function initMenuLinks()
{
	$('.nav-icon-link:not(.sticky)').mouseover(function()
	{
		var icon = $(this).find('img');
		var imgSrc = $(icon).attr('id') + '-active.png';
		$(icon).attr('src', '/Images/' + imgSrc);
	});
	$('.nav-icon-link:not(.sticky)').mouseleave(function ()
	{
		if (!$(this).hasClass('sticky'))
		{
			var menu = $(this).parent().find('.nav-menu');
			console.log($(menu).attr('id'));
			if ($(menu).attr('id') && $(menu).is(':hidden'))
			{
				var icon = $(this).find('img');
				var imgSrc = $(icon).attr('id') + '.png';
				$(icon).attr('src', '/Images/' + imgSrc);
			}
		}
	});
}

function initScreen()
{
	initMenuLinks();
	initForms();

	if (hasWaypoints)
	{
		initSectionSizes();
		initWaypoints();
	}
}

function hideModal(id)
{
	$('#' + id).find('.close-link').click();
}

function resetMenu(id)
{
	var menu = $('#' + id);
	var navLink = $(menu).parent().find('a');
	if (!$(navLink).hasClass('sticky'))
	{
		var icon = $(navLink).find('img');
		var imgSrc = $(icon).attr('id') + '.png';
		$(icon).attr('src', '/Images/' + imgSrc);
	}
	$(menu).hide();
}

function toggleMenu(id)
{
	var menu = $('#' + id);
	$('.nav-menu').not($(menu)).each(function()
	{
		resetMenu($(this).attr('id'));
	});

	if ($(menu).is(':visible'))
	{
		$(menu).slideUp(100);
	}
	else
	{
		var navLink = $(menu).parent().find('a');
		var icon = $(navLink).find('img');
		var imgSrc = $(icon).attr('id') + '-active.png';
		$(icon).attr('src', '/Images/' + imgSrc);
		$(menu).slideDown(100);

		if (id == 'nav-menu-notifications')
		{
			loadNotifactionlist();
		}
	}
}

function hoverImage(id)
{
	var image = $('#' + id);
	var src = image.attr('src');
	if (src.indexOf('hover') != -1)
	{
		image.attr('src', src.substring(0, src.lastIndexOf('-hover')) + '.png');
	}
	else
	{
		image.attr('src', src.substring(0, src.length - 4) + '-hover.png');
	}
}