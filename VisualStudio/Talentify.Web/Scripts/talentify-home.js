var HomePage = function ()
{
	var handleMobileMenu = function ()
	{
		$("#mobilemenutrigger").click(function ()
		{
			$("#mobilemenu").slideToggle("slow");
		});
	}
		

	return {
		//main function to initiate the module
		init: function ()
		{
			handleMobileMenu();
			initAcademyHover();
		}
	};
}();

function initAcademyHover()
{
	// Index - Academy mouseover function
	$('.hover').hover(
		function ()
		{
			$(this).find('.caption').show().css('display', 'inline-block');
		},
		function ()
		{
			$(this).find('.caption').hide();
		}
	);
}