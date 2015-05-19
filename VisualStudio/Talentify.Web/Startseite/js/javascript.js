// Index - Header Slide Down Menu
$(document).ready(function(){
    $("#mobilemenutrigger").click(function(){
		$("#mobilemenugreen").slideToggle("slow");
    });
});
$(document).ready(function(){
    $("#mobilemenutrigger").click(function(){
		$("#mobilemenuyellow").slideToggle("slow");
    });
});
/*
$(document).ready(function() {
	$('#mobilemenutrigger').click(
		function(){
			$("#mobilemenutrigger").css("background", "#01cba3");
		},
		function(){
			$("#mobilemenutrigger").css("background", "#00a77f");
		}
	);
});
*/

// Index - Academy mouseover function
$(document).ready(function() {
	$('.hover').hover(
		function(){
			$(this).find('.caption').show();
		},
		function(){
			$(this).find('.caption').hide();
		}
	);
});

// Index - Make the thour mouseover function (only Desktop)
function isMobile() {
return /Android|webOS|iPhone|iPad|iPod|BlackBerry|IEMobile|Opera Mini/i.test(navigator.userAgent);
}

if(!isMobile()){
	$(document).ready(function() {
		$('#makethetourbg').hover(
			function(){
				$("#makethetourbg").css("background-color", "#f3516e");
				$("#makethetourbg h1").css({
					'margin-left': '50px',
					'transition': 'margin-left .5s',
					'-moz-transition': 'margin-left .5s',
					'-webkit-transition': 'margin-left .5s',
					'-o-transition': 'margin-left .5s'
				});
				$("#makethetourbg img").css({
					'margin-left': '15px',
					'transition': 'margin-left .5s',
					'-moz-transition': 'margin-left .5s',
					'-webkit-transition': 'margin-left .5s',
					'-o-transition': 'margin-left .5s'
				});	
			},
			function(){
				$("#makethetourbg").css("background-color", "#e64461");
				$("#makethetourbg h1").css({
					'margin-left': '0',
					'transition': 'margin-left .5s',
					'-moz-transition': 'margin-left .5s',
					'-webkit-transition': 'margin-left .5s',
					'-o-transition': 'margin-left .5s'
				});
				$("#makethetourbg img").css({
					'margin-left': '-50px',
					'transition': 'margin-left .5s',
					'-moz-transition': 'margin-left .5s',
					'-webkit-transition': 'margin-left .5s',
					'-o-transition': 'margin-left .5s'
				});	
			}
		);
	});
}	

// Newsletter functionailty
function submitNewsletter(){
	resetNewsletterForm();
	$('#mce-EMAIL').val($('#newsletter-email').val());
	$('#mc-embedded-subscribe-form').submit();
}

function resetNewsletterForm() {
	//mce-EMAIL
	$('#mce-EMAIL').val('');
	//mce-FNAME
	$('#mce-FNAME').val('');
	//mce-LNAME
	$('#mce-LNAME').val('');
}