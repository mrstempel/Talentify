var topMargin = 85;
var waypointOffset = topMargin;
var maximizeSections = true;

function getMaskSize()
{
	var size = new Object();
	size.width = $(window).innerWidth();
	size.height = $(window).innerHeight();
	return size;
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

function initScreen()
{
	initSectionSizes();
}