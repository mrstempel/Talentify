causeRepaintsOn = $("h1, h2, h3, p, div");

function getMaskSize()
{
	var size = new Object();
	size.width = $(window).innerWidth();
	size.height = $(window).innerHeight();
	return size;
}

function initPage()
{
	initSectionSizes();
	initTeamRollovers();
	initNewsletterAnmeldung();
}

function initSectionSizes()
{
	causeRepaintsOn.css("z-index", 1);
	var maskSize = getMaskSize();
	$('section.auto-height').css('min-height', '10px');
	var aspect = 385 / 1280;
	var bgHeight = ($('.big-bg').width() * aspect) - 5;
	$('.big-bg').css('height', bgHeight + 'px');
	$('.big-bg').find('article').css('height', bgHeight + 'px');
}

function initTeamRollovers()
{
	$('.team > .item').mouseenter(function()
	{
		$(this).find('img').fadeTo(0.5, 0.2, function()
		{
			$(this).parent().find('.info').fadeIn('fast');
		});
	});

	$('.team > .item').mouseleave(function ()
	{
		$(this).find('img').fadeTo(0.2, 1.0);
		$(this).find('.info').fadeOut(0.2);
	});
}

function initNewsletterAnmeldung()
{
	$(".fancybox").fancybox({
		maxWidth: 430,
		maxHeight: 400,
		fitToView: false,
		width: '70%',
		height: '70%',
		autoSize: false,
		closeClick: false,
		openEffect: 'none',
		closeEffect: 'none',
		padding: 10,
		helpers: {
			overlay: {
				locked: false
			}
		}
	});

	$(".fancybox-anmeldung").fancybox({
		maxWidth: 530,
		maxHeight: 750,
		fitToView: false,
		width: '70%',
		height: '70%',
		autoSize: false,
		closeClick: false,
		openEffect: 'none',
		closeEffect: 'none',
		padding: 10,
		helpers: {
			overlay: {
				locked: false
			}
		}
	});
}