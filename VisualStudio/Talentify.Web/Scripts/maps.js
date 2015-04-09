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
		scaleControl: true,
		panControl: false,
		rotateControl: false,
		zoomControl: true,
		scrollwheel: false
	};

	map = new google.maps.Map(document.getElementById('map-container'), mapOptions);
	map.mapTypes.set('map_style', styledMap);
	map.setMapTypeId('map_style');
	loadSchoolMarkers();
}

function loadSchoolMarkers()
{
	console.log('lade schulmap dinger ...');
	$.ajax({
		url: '/Start/SchoolLatLng',
		type: 'get',
		async: true,
		success: function (data)
		{
			if (data != null && data.length > 0)
			{
				console.log('schulmap dinger geladen');
				schoolInfoWindow = new google.maps.InfoWindow({ content: "" });

				/*{
					name: '@schoolInfo.School.Name', 
					address: '@schoolInfo.School.Address', 
					zipCode: '@schoolInfo.School.ZipCode', 
					city: '@schoolInfo.School.City', 
					website: '@schoolInfo.School.Website', 
					lat: @schoolInfo.School.Latitude, 
					lng: @schoolInfo.School.Longitude, 
					coachingStudentCount: @schoolInfo.CoachingStudentCount, 
					coachingSubjectCount: @schoolInfo.CoachingSubjectCount
				}*/

				//alert(data[0]['School']['Name']);
				var schoolMarkers = new Array();

				for (i = 0; i < data.length; i++)
				{
					var marker = new google.maps.Marker({
						position: new google.maps.LatLng(data[i]['School']['Latitude'], data[i]['School']['Longitude']),
						title: "Details anzeigen",
						map: map
					});
					schoolMarkers.push(marker);

					google.maps.event.addListener(marker, 'click', (function (marker, i)
					{
						return function ()
						{
							map_recenter(marker.getPosition(), 0, -100);
							var content = '<div class="map-school-info">';
							content += '<h2>' + data[i]['School']['Name'] + '</h2>';
							content += '<p>' + data[i]['School']['Address'] + ', ' + data[i]['School']['ZipCode'] + ' ' + data[i]['School']['City'] + '</p>';
							content += '<p><a href="' + data[i]['School']['Website'] + '" target="_blank">' + data[i]['School']['Website'] + '</a></p>';
							content += '<p>' + data[i]['CoachingStudentCount'] + ' Lernhilfeangebote in ' + data[i]['CoachingSubjectCount'] + ' Fächern</p>';
							content += '</div>';

							schoolInfoWindow.setContent(content);
							schoolInfoWindow.open(map, marker);
						};
					})(marker, i));
				}
				console.log('schulmap marker gesetzt');

				var schoolMarkerCluster = new MarkerClusterer(map, schoolMarkers, { gridSize: 50 });
				console.log('schulmap cluster gesetzt');
			}
		}
	});
}