﻿// Newsletter functionailty
function submitNewsletter()
{
	resetNewsletterForm();
	$('#mce-EMAIL').val($('#newsletter-email').val());
	$('#mc-embedded-subscribe-form').submit();
}

function resetNewsletterForm()
{
	//mce-EMAIL
	$('#mce-EMAIL').val('');
	//mce-FNAME
	$('#mce-FNAME').val('');
	//mce-LNAME
	$('#mce-LNAME').val('');
}

// Index - Make the thour mouseover function (only Desktop)
function isMobile()
{
	return /Android|webOS|iPhone|iPad|iPod|BlackBerry|IEMobile|Opera Mini/i.test(navigator.userAgent);
}

function getUrlParam(name)
{
	name = name.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
	var regexS = "[\\?&]" + name + "=([^&#]*)";
	var regex = new RegExp(regexS);
	var results = regex.exec(window.location.href);
	if (results == null)
		return "";
	else
		return results[1];
}

// Navbar Index
$(document).ready(function () {
    $(window).scroll(function () {
        //if you hard code, then use console
        //.log to determine when you want the 
        //nav bar to stick.  
        console.log($(window).scrollTop())
        if ($(window).scrollTop() < 372) {
            $('#indexmenubg').slideUp();
        }
        if ($(window).scrollTop() > 372) {
            $('#indexmenubg').slideDown();
        }
    });
});

//Navbar Tour, Voice and About
$(document).ready(function () {
    $(window).scroll(function () {
        //if you hard code, then use console
        //.log to determine when you want the 
        //nav bar to stick.  
        console.log($(window).scrollTop())
        if ($(window).scrollTop() < 360) {
            $('#menubg').slideUp();
        }
        if ($(window).scrollTop() > 360) {
            $('#menubg').slideDown();
        }
    });
});