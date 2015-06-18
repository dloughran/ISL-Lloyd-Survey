var $map;
var $latlng;
var $overlay;
var $mapDiv;
var $mapModel;
var $markers = [];
var $slimMarkers = [];
var $currentPinDiv = null;
var $currentPin = null;

$(document).ready(function () {
    //load the Google map script once document is ready
    loadMapScript();

    // DOM event handlers

    // Save marker
    $('#map-canvas').on('click', '.dft-sav-mkr', function () {
        var markerId = this.parentNode.attributes['markerInfoId'].nodeValue;
        var markerInfoDiv = document.getElementById('markerInfoDiv' + markerId);
        var markerType = markerInfoDiv.attributes['markerType'].nodeValue;
        var markerIssueType = '';
        var markerComments = document.getElementById('markerInfoComment' + markerId).value;
        console.log("Save marker: " + markerId);
        $markers[markerId].type = markerType;
        $markers[markerId].comment = markerComments;
        $markers[markerId].infoWin.close();
        // scroll back up in case onscreen keyboard hid map controls.
        window.scrollTo(0, 0);


    });

    // delete marker
    $('#map-canvas').on('click', '.dft-del-mkr', function (e, data) {
        var markerId = this.parentNode.attributes['markerInfoId'].nodeValue;
        var marker = $markers[markerId];
        var infoWindow = marker.infoWin;
        for (var i = 0; i < $markers.length; i++) {
            if ($markers[i] != undefined && $markers[i].uuid === marker.uuid) {
                delete $markers[i];
            }
        }
        infoWindow.close();
        marker.setMap(null);
        console.log('Delete marker');
    });

    // close map help window
    $('#dftMapHelp').click(function (e) {
        hideMapHelp();
    });
});


// load the google api after the document has finished loading
function loadMapScript() {
    var script = document.createElement('script');
    script.type = 'text/javascript';
    // note the callback to the initalize function in the following line.
    // are not done until the api has loaded.
    script.src = 'https://maps.google.com/maps/api/js?libraries=drawing&callback=initialize';
    document.body.appendChild(script);
}

// initalize the Google map, map overlay and custom controls. 
// this function is called by the google api once it finishes loading.
function initialize() {
    // Create and position a new map instance
    var $mapDiv = document.getElementById('map-canvas');
    var $latlng = new google.maps.LatLng(53.2750, -110.0150); // Lloydminster
    var mapOptions = {
        zoom: 13,
        center: $latlng,
        disableDefaultUI: true,
        zoomControl: true,
        zoomControlOptions: {
            style: google.maps.ZoomControlStyle.SMALL,
            position: google.maps.ControlPosition.RIGHT_CENTER
        }
    };

    // create map instance
    $map = new google.maps.Map($mapDiv, mapOptions);

    // create custom control div for back button and call control constructor passing this new DIV
    var surveyNavPrevMapControlDiv = document.createElement('div');
    var surveyNavPrevMapControl = new SurveyPrevNav(surveyNavPrevMapControlDiv, $map);
    surveyNavPrevMapControlDiv.index = 1;
    $map.controls[google.maps.ControlPosition.LEFT_BOTTOM].push(surveyNavPrevMapControlDiv);

    // create custom control div for continue button and call control constructor passing this new DIV
    var surveyNavNextMapControlDiv = document.createElement('div');
    var surveyNavNextMapControl = new SurveyNextNav(surveyNavNextMapControlDiv, $map);
    surveyNavNextMapControlDiv.index = 1;
    $map.controls[google.maps.ControlPosition.RIGHT_BOTTOM].push(surveyNavNextMapControlDiv);

    // create custom control div for pins and call control constructor passing this new DIV
    var pinShelfDiv = document.createElement('div');
    var pinShelf = new PinShelf(pinShelfDiv, $map);
    pinShelfDiv.index = 1;
    $map.controls[google.maps.ControlPosition.TOP_LEFT].push(pinShelfDiv);

    // Get existing markers from MVC MapModel (if any)
    if ($mapModel != null & $mapModel != undefined & $mapModel.Markers != null & $mapModel.Markers != undefined) {
        var m = JSON.parse($mapModel.Markers);
        for (var i = 0; i < m.length; i++) {
            createMarker(m[i]);
        }
    }

    // add map event listener for map click (creating a marker)
    google.maps.event.addListener($map, 'click', function (e) {
        placeMarker(e.latLng, $map);
    });
}

// Create Survey Previous and Next Navigation Controls to add to Google Map Controls Collection
function SurveyPrevNav(el, map) {
    var isiPhone = navigator.userAgent.match(/iPhone/i);
    var surveyPreviousPageDiv = document.createElement('div');
    surveyPreviousPageDiv.id = "dftSurveyPreviousPage";
    surveyPreviousPageDiv.innerHTML = "<< BACK";
    surveyPreviousPageDiv.classList.add('btn');
    surveyPreviousPageDiv.classList.add('btn-primary');
    surveyPreviousPageDiv.classList.add('btn-lg');
    if (isiPhone) {
        surveyPreviousPageDiv.innerHTML = "<<";
        surveyPreviousPageDiv.classList.add('btn-sm');
    }
    surveyPreviousPageDiv.classList.add('dft-gm-lb-fix');
    el.appendChild(surveyPreviousPageDiv);
    google.maps.event.addDomListener(surveyPreviousPageDiv, 'click', function (e) {
        serializeMarkers("One"); //Passing target view so the MVC controller can redirect properly
    });
}

function SurveyNextNav(el,map) {
    var isiPhone = navigator.userAgent.match(/iPhone/i);
    var surveyNextPageDiv = document.createElement('div');
    surveyNextPageDiv.id = "dftContinueSurvey";
    surveyNextPageDiv.innerHTML = 'Continue >>';
    surveyNextPageDiv.classList.add('btn');
    surveyNextPageDiv.classList.add('btn-primary');
    surveyNextPageDiv.classList.add('btn-lg');
    if (isiPhone) {
        surveyNextPageDiv.innerHTML = '>>';
        surveyNextPageDiv.classList.add('btn-sm');
    }
    surveyNextPageDiv.classList.add('dft-gm-rb-fix');
    el.appendChild(surveyNextPageDiv);

    google.maps.event.addDomListener(surveyNextPageDiv, 'click', function (e) {
        serializeMarkers("Two"); //Passing target view so the MVC controller can redirect properly
    });
}

// Serialize all relevant marker information and put in form data field "hidmapdata"
// submit the form
function serializeMarkers(nextView) {
    $slimMarkers = [];
    for (var i = 0; i < $markers.length; i++) {
        if ($markers[i] != null & $markers[i] != undefined) {
            var slimMarker = {};
            slimMarker.lat = $markers[i].position.A;
            slimMarker.lng = $markers[i].position.F;
            //slimMarker.position = $markers[i].position;
            slimMarker.type = $markers[i].type;
            slimMarker.icon = $markers[i].icon;
            slimMarker.id = $markers[i].id;
            slimMarker.uuid = $markers[i].uuid;
            slimMarker.comment = $markers[i].comment;
            $slimMarkers.push(slimMarker);
        }
    }
    $('#hidmapdata').val(JSON.stringify($slimMarkers));
    $('#hidredirect').val(nextView);
    $('form#formMap').submit();
}

function PinShelf(shelfDiv, map) {
    // Create individual controls for the pins
    var pinDiv1 = document.createElement('div');
    pinDiv1.id = "pinDiv1";
    pinDiv1.style.cssFloat = "left";
    pinDiv1.innerHTML = '<div>Traffic</div><div><img id="dftpin1" class="dft-pin-40" type="Traffic Congestion" src="/Content/img/congestionpin_64.png"></div><div>Congestion</div>';
    shelfDiv.appendChild(pinDiv1);

    var pinDiv2 = document.createElement('div');
    pinDiv2.id = "pinDiv2";
    pinDiv2.style.cssFloat = "left";
    pinDiv2.innerHTML = '<div>Goods</div><div><img id="dftpin2" class="dft-pin-40" type="Goods Movement" src="/Content/img/goodspin_64.png"></div><div>Movement</div>';
    shelfDiv.appendChild(pinDiv2);

    var pinDiv3 = document.createElement('div');
    pinDiv3.id = "pinDiv3";
    pinDiv3.style.cssFloat = "left";
    pinDiv3.innerHTML = '<div>Other</div><div><img id="dftpin3" class="dft-pin-40" type="Other Issue" src="/Content/img/otherpin_64.png"></div><div>Issue</div>';
    shelfDiv.appendChild(pinDiv3);

    var pinDiv4 = document.createElement('div');
    pinDiv4.id = "pinDiv4";
    pinDiv4.style.cssFloat = "left";
    pinDiv4.innerHTML = '<div>Walk/Cycle</div><div><img id="dftpin4" class="dft-pin-40" type="Walk/Cycle Connections" src="/Content/img/pedcyclepin_64.png"></div><div>Connections</div>';
    shelfDiv.appendChild(pinDiv4);

    var pinDiv5 = document.createElement('div');
    pinDiv5.id = "pinDiv5";
    pinDiv5.style.cssFloat = "left";
    pinDiv5.innerHTML = '<div>Missing Road</div><div><img id="dftpin5" class="dft-pin-40" type="Missing Road Connections" src="/Content/img/roadcircpin_64.png"></div><div>Connections</div>';
    shelfDiv.appendChild(pinDiv5);

    var pinDiv6 = document.createElement('div');
    pinDiv6.id = "pinDiv6";
    pinDiv6.style.cssFloat = "left";
    pinDiv6.innerHTML = '<div>Traffic</div><div><img id="dftpin6" class="dft-pin-40" type="Traffic Safety" src="/Content/img/trafficsafetypin_64.png"></div><div>Safety</div>';
    shelfDiv.appendChild(pinDiv6);

    var pinDivHelp = document.createElement('div');
    pinDivHelp.id = "pinHelp";
    pinDivHelp.style.cssFloat = "left";
    pinDivHelp.innerHTML = 'HELP';
    shelfDiv.appendChild(pinDivHelp);

    // Add event listeners for the pins
    google.maps.event.addDomListener(pinDiv1, 'click', function (e) {
        pinClicked(1, pinDiv1);
        console.log("pin 1 clicked");
    });
    google.maps.event.addDomListener(pinDiv2, 'click', function (e) {
        pinClicked(2, pinDiv2);
        console.log("pin 2 clicked");
    });
    google.maps.event.addDomListener(pinDiv3, 'click', function (e) {
        pinClicked(3, pinDiv3);
        console.log("pin 3 clicked");
    });
    google.maps.event.addDomListener(pinDiv4, 'click', function (e) {
        pinClicked(4, pinDiv4);
        console.log("pin 4 clicked");
    });
    google.maps.event.addDomListener(pinDiv5, 'click', function (e) {
        pinClicked(5, pinDiv5);
        console.log("pin 5 clicked");
    });
    google.maps.event.addDomListener(pinDiv6, 'click', function (e) {
        pinClicked(6, pinDiv6);
        console.log("pin 6 clicked");
    });

    // Help clicked on pin control.
    google.maps.event.addDomListener(pinDivHelp, 'click', function (e) {
        $('#dftMapHelp').css('z-index', 1);
        $('#dftMapHelp').show('slow');
        console.log("help clicked");
    });

}

// When pin is clicked...
function pinClicked(pin, pinDiv) {
    var pinId = '#dftpin' + pin;
    var pinDivId = '#' + pinDiv.id;

    // remove selected pin class from current pin's div in the control ui
    if ($currentPinDiv != null & $currentPinDiv != undefined) {
        $('#' + $currentPinDiv.id).removeClass('dft-sel-pin');
    }
    // add the selected pin class to the currently selected pin's div
    $(pinDivId).addClass('dft-sel-pin');
    // set the current pin div
    $currentPinDiv = pinDiv;
    // set the current pin
    $currentPin = $(pinId);
    $map.setOptions({ draggableCursor: 'crosshair' });
    // hide the help window if it's open
    hideMapHelp();
}

// Hide the map help 
function hideMapHelp() {
    //alert("hideMapHelp");
    $('#dftMapHelp').hide('slow', function () {
        $("#dftMapHelp").css('z-index', -1);
    });

}

// Create a marker from the information derived from the session
function createMarker(m) {
    var loc = new google.maps.LatLng(parseFloat(m.lat), parseFloat(m.lng));
    var newMarker = new google.maps.Marker({
        position: loc,
        type: m.type,
        icon: m.icon,
        id: Number(m.id),
        uuid: m.uuid,
        comment: m.comment,
        map: $map,
        draggable:true
    });
    // add marker event listener
    google.maps.event.addListener(newMarker, 'click', function () {
        console.log('newMarker clicked');
        showInfoWindow(newMarker, true);
    });

    $markers.push(newMarker);
}

// Create a new map marker based on where the user click on the map surface
function placeMarker(position, map) {
    //limit the number of markers placed to 50
    if ($markers.length < 50) {
        var markerIcon;
        if ($currentPin != null & $currentPin != undefined) {
            markerIcon = $currentPin.attr('src');
            var marker = new google.maps.Marker({
                position: position,
                map: map,
                draggable: true,
                icon: markerIcon,
                type: $currentPin.attr('type'),
                issue: "",
                comment: "",
                id: $markers.length,
                uuid: generateUUID(),
                //infoWin: new google.maps.InfoWindow()
            });
            // save new marker to the $markers array
            $markers.push(marker);

            // add marker event listeners
            google.maps.event.addListener(marker, 'click', function () {
                console.log('Marker clicked');
                showInfoWindow(marker, true);
            });

            // display the marker's info window
            showInfoWindow(marker, true);
        }
    }
}

// Create and show the info window for a marker
// NOTE: isNew is always true and, as a result, this code 
// could be refactored to eliminate the isNew parameter
function showInfoWindow(mkr, isNew) {
    var infoWindow;
    // if the maker is new create it's content, otherwise use existing marker information
    if (isNew) {
        var ih = '';
        ih += '<div id="markerInfoDiv' + mkr.id + '" markerInfoId="' + mkr.id + '" uuid="' + mkr.uuid + '"' + 'class="dft-mkr-info-win"';
        ih += ' markerType="' + mkr.type + '" class="text-center dftMarkerInfoDiv">';
        ih += '  <div class="markerInfoTitle">' + mkr.type + '</div>';
        ih += '  <textarea id="markerInfoComment' + mkr.id + '" class="dft-mkr-textarea" maxlength="255" placeholder="Enter your comments...">' + mkr.comment + '</textarea>';
        ih += '  <br />';
        ih += '  <div class="dft-del-mkr btn btn-danger btn-sm pull-left">Delete</div>';
        ih += '  <div class="dft-sav-mkr btn btn-primary btn-sm pull-right">Save</div>';
        ih += '</div>';

        infoWindow = new google.maps.InfoWindow({
            content: ih
        });
    }
    else
    {
        infoWindow = mkr.infoWin;
    }
    // set focus into marker text area
    $('#markerInfoComment' + mkr.id).focus();

    // add infoWindow listener for infoWindow close click
    google.maps.event.addListener(infoWindow, 'closeclick', function () {
        console.log('infowindow closed');
        $(window.scrollTo(0, 0));
    });

    // store the updated infoWindow in the marker
    mkr.infoWin = infoWindow;

    //open the infoWindow
    infoWindow.open(mkr.map, mkr);
}

// Generate a UUID for a marker
function generateUUID() {
    var d = new Date().getTime();
    var uuid = 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
        var r = (d + Math.random() * 16) % 16 | 0;
        d = Math.floor(d / 16);
        return (c == 'x' ? r : (r & 0x3 | 0x8)).toString(16);
    });
    return uuid;
};

// The following are Google Maps utility methods

// Set the map property of all markers (shows them on the map)
function showMarkers() {
    setAllMap($map);
}

// Clears the map property of all markers (removes them from the map) but keep them in the array.
function clearMarkers() {
    setAllMap(null);
}

// Displays all markers by setting the map on all markers in the $markers array
function setAllMap(map) {
    for (var i = 0; i < $markers.length; i++) {
        $markers[i].setMap(map)
    }
}
