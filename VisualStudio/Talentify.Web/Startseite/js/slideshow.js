$(document).ready(function() {	
	$('.center').slick({
	  centerMode: true,
	  autoplay: true,
	  autoplaySpeed: 3000,
	  pauseOnHover: true,
	  prevArrow: $('.prev'),
	  nextArrow: $('.next'), 
	  centerPadding: '0px',
	  slidesToShow: 5,
	  responsive: [
		{
		  breakpoint: 768,
		  settings: {
			arrows: false,
			centerMode: true,
			autoplay: true,
			autoplaySpeed: 3000,
			centerPadding: '60px',
			slidesToShow: 3
		  }
		},
		{
		  breakpoint: 480,
		  settings: {
			arrows: false,
			autoplay: true,
			autoplaySpeed: 3000,
			centerMode: true,
			centerPadding: '25px',
			slidesToShow: 1
		  }
		}
	  ]
	});
});