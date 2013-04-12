var UDP = require('ti.udp');
Titanium.UI.setBackgroundColor('orange');
var tabGroup = Titanium.UI.createTabGroup();
var u = Ti.Android != undefined ? 'dp' : 0;
var win = Ti.UI.createWindow({
	title : 'udpSendWin',
	backgroundColor : 'orange',
	layout : 'vertical'
});
var tab1 = Titanium.UI.createTab({
	icon : 'KS_nav_views.png',
	title : 'udpTab',
	window : win

});

/*
 Create our socket.
 */
var socket = UDP.createSocket();

/*
 Start the server...
 */
/*
 var startSocket = Ti.UI.createButton({
 title: 'Start Socket',
 top: 10 + u, left: 10 + u, right: 10 + u, height: 40 + u
 });
 startSocket.addEventListener('click', function () {
 socket.start({
 port: 1261
 });
 });
 win.add(startSocket);
 */
socket.start({
	port : 1261
});
var sendTo = Ti.UI.createTextField({
	height : 20 + u,
	top : 10 + u,
	left : 10 + u,
	right : 10 + u,
	//borderStyle : Ti.UI.INPUT_BORDERSTYLE_ROUNDED,
	//value: Ti.App.Properties.getString('SavedSendTo', '')
	//value: "192.168.0.2"
	value : "192.168.0.7"
});
win.add(sendTo);
var sendStuff = Ti.UI.createTextField({
	height : 20 + u,
	top : 10 + u,
	left : 10 + u,
	right : 10 + u,
	//borderStyle : Ti.UI.INPUT_BORDERSTYLE_ROUNDED,
	//value: Ti.App.Properties.getString('SavedSendTo', '')
	//value: "192.168.0.2"
	value : "nav(http://youtube.com)"
});
win.add(sendStuff);
/*
 Send a string...
 */
var sendString = Ti.UI.createButton({
	title : 'Send String to Specific',
	top : 10 + u,
	left : 10 + u,
	right : 10 + u,
	height : 20 + u
});
sendString.addEventListener('click', function() {
	Ti.App.Properties.setString('SavedSendTo', sendTo.value);
	socket.sendString({
		host : sendTo.value,
		data : sendStuff.value
		//data: sendVal.value
	});
});
win.add(sendString);
/*
 ... or send bytes.
 */
var sendBytes = Ti.UI.createButton({
	title : 'Send Bytes to Specific',
	top : 10 + u,
	left : 10 + u,
	right : 10 + u,
	height : 20 + u
});
sendBytes.addEventListener('click', function() {
	Ti.App.Properties.setString('SavedSendTo', sendTo.value);
	socket.sendBytes({
		host : sendTo.value,
		data : [181, 10, 0, 0]
	});
});
win.add(sendBytes);

/*
 Broadcast a string (notice we don't specify a host)...
 */
var broadcastString = Ti.UI.createButton({
	title : 'Broadcast String',
	top : 10 + u,
	left : 10 + u,
	right : 10 + u,
	height : 20 + u
});
broadcastString.addEventListener('click', function() {
	socket.sendString({
		//data: 'Hello, UDP!'
		data : 'nav(http://google.com)'
	});
});
win.add(broadcastString);

/*
 Listen for when the server or client is ready.
 */
socket.addEventListener('started', function(evt) {
	status.text = 'Started!';
});

/*
 Listen for data from network traffic.
 */
socket.addEventListener('data', function(evt) {
	status.text = JSON.stringify(evt);
	Ti.API.info(JSON.stringify(evt));
});

/*
 Listen for errors.
 */
socket.addEventListener('error', function(evt) {
	status.text = JSON.stringify(evt);
	Ti.API.info(JSON.stringify(evt));
});

/*
 Finally, stop the socket when you no longer need network traffic access.
 */
var stop = Ti.UI.createButton({
	title : 'Stop',
	top : 10 + u,
	left : 10 + u,
	right : 10 + u,
	height : 20 + u
});
stop.addEventListener('click', function() {
	socket.stop();
});
win.add(stop);

var win2 = Titanium.UI.createWindow({
	title : 'ViewWin',
	backgroundColor : '#fff'

});

var tab2 = Titanium.UI.createTab({
	icon : 'KS_nav_ui.png',
	title : 'ViewTab',
	window : win2
});
var label2 = Titanium.UI.createLabel({
	color : '#999',
	text : 'testWin2',
	font : {
		fontSize : 20,
		fontFamily : 'Helvetica Neue'
	},
	textAlign : 'center',
	width : 'auto'
});
var dimensionOffset = 50;
var pWidth = Ti.Platform.displayCaps.platformWidth - dimensionOffset;
var pHeight = Ti.Platform.displayCaps.platformHeight - dimensionOffset;
//var vidsrc = 'http://217.197.122.134/axis-cgi/mjpg/video.cgi';
var vidsrc = 'http://192.168.0.7:1262';
var playerHTML = '<img src=' + vidsrc + ' />';
var webBox = Titanium.UI.createWebView({
	width: Ti.UI.SIZE,
	height: Ti.UI.SIZE,
	html : playerHTML
	//url: vidsrc,
	//width : 'auto',
	//height : 'auto'
});
webBox.addEventListener('click', function(e) {
	var xCoord = Math.round(e.x);
	var yCoord = Math.round(e.y);
	var sendDat = 'lclick(' + xCoord + ',' + yCoord + ')';
	Ti.API.info(sendDat);
	socket.sendString({
		host : sendTo.value,
		data : sendDat
	});
});
/*
 var webBox = Titanium.UI.createWebView({
 url: vidsrc
 });
 */
var scrollView = Titanium.UI.createScrollView({
	//contentWidth: 'auto',
	//contentHeight: 'auto',
	maxZoomScale : 1,
	minZoomScale : 0.1,
	//contentWidth : Ti.UI.SIZE,
	//contentHeight : Ti.UI.SIZE,
	showVerticalScrollIndicator : true,
	showHorizontalScrollIndicator : true
});
win2.add(label2);
scrollView.add(webBox);
var vidsource = Ti.UI.createTextField({
	height : 20 + u,
	top : 10 + u,
	left : 10 + u,
	right : 10 + u,
	//borderStyle : Ti.UI.INPUT_BORDERSTYLE_ROUNDED,
	//value: Ti.App.Properties.getString('SavedSendTo', '')
	//value: "192.168.0.2"
	value : "http://192.168.0.7:1262"
});
win.add(vidsource);
var reloadsource = Ti.UI.createButton({
	title : 'reloadsource',
	top : 10 + u,
	left : 10 + u,
	right : 10 + u,
	height : 20 + u
});
reloadsource.addEventListener('click', function() {
	webBox.stopLoading(true); 
	webBox.html = '<img src=' + vidsource.value + ' />';
	webBox.reload(); 
});
win.add(reloadsource);
webBox.show();
var mode = 1;
//scrollView.add(imageBox);
win2.add(scrollView);
//win2.add(imageBox);

var status = Ti.UI.createLabel({
	text : 'Press Start Socket to Begin',
	top : 10 + u,
	left : 10 + u,
	right : 10 + u,
	height : 'auto'
});
var memstatus = Ti.UI.createLabel({
	text : 'Available Memory',
	top : 10 + u,
	left : 10 + u,
	right : 10 + u,
	height : 'auto'
});

win.add(status);
win2.add(memstatus);
tabGroup.addTab(tab1);
tabGroup.addTab(tab2);
tabGroup.open();
function updateView() {
	try {

		Titanium.API.error("start update");
		//checkreload();
		if (mode == 1) {
			//webBox.repaint();
		} else if (mode == 2) {
			//webBox2.repaint();
		}
		//webBox.html = null;

		//webBox.reload();
		//webBox.release();
		webBox.width = Ti.UI.SIZE;
		webBox.height = Ti.UI.SIZE; 
		//webBox.repaint();
		Titanium.API.error(Titanium.Platform.availableMemory);
		memstatus.text = Titanium.Platform.availableMemory.toString();
		//Titanium.API.error(webBox.loading);
		//webBox.release();
	} catch(e) {

	}
}

var oldWidth = 800;
var oldHeight = 600;
function checkreload() {
	var browseSize = webBox.size;
	var newWidth = browseSize.width;
	var newHeight = browseSize.height;
	if (newWidth != oldWidth || newHeight != oldHeight) {
		Ti.API.info("dimensions changed");
		webBox.reload();
		Ti.API.info("webbox reloaded");
		oldWidth = newWidth;
		oldHeight = newHeight;
		Ti.API.info("olddimensions set");
	} else {
		Ti.API.info("dimensions the same");
		Ti.API.info("no action taken");
	}

}

function releaseView() {
	try {
		//Ti.API.info(webBox.loading.toString());
		//webBox.reload();
		//Ti.API.info("reloaded");
	} catch(e) {

	}
}

setInterval(updateView, 5000);
setInterval(releaseView, 5000);
