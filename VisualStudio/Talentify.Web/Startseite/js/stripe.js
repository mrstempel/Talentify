var handler = StripeCheckout.configure({
	key: 'pk_test_6pRNASCoBOKtIshFeQd4XMUh',
	image: 'img/header_star.png',
	currency: 'EUR',
	token: function(token) {
	  // Use the token to create the charge with a server-side script.
	  // You can access the token ID with `token.id`
	}
});

$('.renewButton').on('click', function(e) {
	// Open Checkout with further options
	handler.open({
		name: 'Talentify.me',
		description: 'Premium-Mitgliedschaft',
		amount: 5000
	});
	e.preventDefault();
});

// Close Checkout on page navigation
$(window).on('popstate', function() {
	handler.close();
});