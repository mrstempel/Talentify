var map;
var geocoder;
var latitude;
var longitude;
var schoolLatLngs;
var schoolInfoWindow;

var mapStylesArray = [
	{
		featureType: 'all',
		elementType: 'geometry',
		stylers:
		[
			{ visibility: "simplified" },
			{ saturation: -70 },
			{ gamma: 0.90 }
		]
	},
	{
		featureType: 'road',
		elementType: 'geometry',
		stylers:
		[
			{ visibility: "simplified" },
			{ lightness: 100 }
		]
	},
	{
		featureType: 'road',
		elementType: 'labels.icon',
		stylers:
		[
			{ visibility: "off" }
		]
	},
	{
		featureType: 'geometry',
		elementType: 'labels.text',
		stylers:
		[
			{ color: '#666666' },
			{ weight: 0.1 }
		]
	},
	{
		featureType: "administrative",
		elementType: "geometry.fill",
		stylers: [
		   { visibility: "off" }
		]
	},
	{
		featureType: "poi",
		stylers: [
		   { visibility: "off" }
		]
	},
	{
		featureType: "transit",
		stylers: [
		   { visibility: "off" }
		]
	}
];

function map_recenter(latlng, offsetx, offsety)
{
	var point1 = map.getProjection().fromLatLngToPoint(
        (latlng instanceof google.maps.LatLng) ? latlng : map.getCenter()
    );
	var point2 = new google.maps.Point(
        ((typeof (offsetx) == 'number' ? offsetx : 0) / Math.pow(2, map.getZoom())) || 0,
        ((typeof (offsety) == 'number' ? offsety : 0) / Math.pow(2, map.getZoom())) || 0
    );
	map.setCenter(map.getProjection().fromPointToLatLng(new google.maps.Point(
        point1.x - point2.x,
        point1.y + point2.y
    )));
	console.log("map recentered");
}

function initEventMap()
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

function initBackgroundMap()
{
	geocoder = new google.maps.Geocoder();
	var centerLatlng = new google.maps.LatLng(latitude, longitude);
	var styledMap = new google.maps.StyledMapType(mapStylesArray, { name: "talentify Map" });
	var mapOptions =
	{
		backgroundColor: 'transparent',
		minZoom: 3,
		zoom: 16,
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

	map = new google.maps.Map(document.getElementById('map-container'), mapOptions);
	map.mapTypes.set('map_style', styledMap);
	map.setMapTypeId('map_style');
	loadSchoolMarkers();
}

function loadSchoolMarkers()
{
	schoolInfoWindow = new google.maps.InfoWindow({ content: "" });

	for (i = 0; i < schoolLatLngs.length; i++)
	{
		var marker = new google.maps.Marker({
			position: new google.maps.LatLng(schoolLatLngs[i].lat, schoolLatLngs[i].lng),
			title: "SCHULE",
			map: map
		});

		google.maps.event.addListener(marker, 'click', (function (marker, i)
		{
			return function ()
			{
				map_recenter(marker.getPosition(), 0, -100);
				var content = '<div class="map-school-info">';
				content += '<h2>' + schoolLatLngs[i].name + '</h2>';
				content += '<p>' + schoolLatLngs[i].address + ', ' + schoolLatLngs[i].zipCode + ' ' + schoolLatLngs[i].city + '</p>';
				content += '<p><a href="' + schoolLatLngs[i].website + '" target="_blank">' + schoolLatLngs[i].website + '</a></p>';
				content += '<p>' + schoolLatLngs[i].coachingStudentCount + ' Lernhilfeangebote in ' + schoolLatLngs[i].coachingSubjectCount + ' Fächern</p>';
				content += '</div>';

				schoolInfoWindow.setContent(content);
				schoolInfoWindow.open(map, marker);
			};
		})(marker, i));
	}
}