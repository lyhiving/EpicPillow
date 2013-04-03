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
	height : 44 + u,
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
	height : 44 + u,
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
	height : 40 + u
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
	height : 40 + u
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
	height : 40 + u
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
	height : 40 + u
});
stop.addEventListener('click', function() {
	socket.stop();
});
win.add(stop);

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
win.add(memstatus);
win.add(status);
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
var vidsrc = 'http://192.168.0.7:1262';
var playerHTML = '<img src=' + vidsrc + ' />';
var webBox = Titanium.UI.createWebView({
	html: playerHTML,
	width: Ti.UI.SIZE,
	height: Ti.UI.SIZE
});
webBox.addEventListener('longpress', function(e) {
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
	maxZoomScale: 1,
	minZoomScale: 0.1,
	contentWidth: Ti.UI.SIZE,
	contentHeight: Ti.UI.SIZE,
	showVerticalScrollIndicator: true,
	showHorizontalScrollIndicator: true
});
win2.add(label2);
scrollView.add(webBox);
webBox.show();
var mode = 1;
//scrollView.add(imageBox);
win2.add(scrollView); 
//win2.add(imageBox);
tabGroup.addTab(tab1);
tabGroup.addTab(tab2);
tabGroup.open();
function updateView()
{
	try
	{
		
		Titanium.API.error("start update");
	 
		if (mode == 1)
		{
			//webBox.repaint(); 
		}
		else if (mode == 2)
		{
			//webBox2.repaint();
		}
		//webBox.html = null;
		
		//webBox.reload();
		//webBox.release();
		
		//webBox.repaint();
		Titanium.API.error(Titanium.Platform.availableMemory);
		memstatus.text = Titanium.Platform.availableMemory.toString();
		//Titanium.API.error(webBox.loading);
		//webBox.release();
	}
	catch(e)
	{
		
	}
}
function releaseView()
{
	try
	{
		webBox.stopLoading(true);
<<<<<<< HEAD
		webBox.reload();
=======
		webBox.reload(); 
>>>>>>> memory issue a tiny bit better
	}
	catch(e)
	{
		
	}
}
setInterval(updateView, 2500); 
setInterval(releaseView, 15000);
