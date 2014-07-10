var topMargin = 85;
var waypointOffset = topMargin;
var maximizeSections = true;
var hasWaypoints = false;

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
		var maskSize = getMaskSize();
		$('#page-container > section').css('min-height', (maskSize.height - topMargin) + 'px');
		$('section.auto-height').css('min-height', '10px');
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
		{ offset: topMargin });

		$(this).waypoint(function (direction)
		{
			if (direction === 'up')
				highlightNavigation($(this).prop('id'));
		},
		{ offset: topMargin - 10 });

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
	}
}