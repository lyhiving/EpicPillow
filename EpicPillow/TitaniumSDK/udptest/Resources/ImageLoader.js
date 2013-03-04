exports.LoadRemoteImage = function (obj, url)
{
	var xhr = Titanium.Network.createHTTPClient();
	xhr.onload = function()
	{
		Ti.API.info('image data=' + this.responseData);
		obj.image = this.responseData;	
	};
	xhr.open('GET', url);
	
	xhr.send(); 	
	
}

