var map;
var geocoder;
var latitude;
var longitude;

function initMap()
{
	geocoder = new google.maps.Geocoder();
	var centerLatlng = new google.maps.LatLng(latitude, longitude);
	var mapOptions =
	{
		backgroundColor: 'transparent',
		minZoom: 3,
		zoom: 18,
		center: centerLatlng,
		mapTypeId: google.maps.MapTypeId.ROADMAP,
		mapTypeControl: false,
		overviewMapControl: false,
		streetViewControl: false,
		scaleControl: false,
		panControl: false,
		rotateControl: false,
		zoomControl: false
	};

	var image = '/Images/map-eumel.png';
	var marker = new google.maps.Marker({
		position: centerLatlng,
		title: '',
		icon: image
	});
	
	map = new google.maps.Map(document.getElementById('map-container'), mapOptions);
	marker.setMap(map);
}