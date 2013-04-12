EpicPillow
==========

An attempt to make a image-based web proxy and eventually allow for a desktop browsing experience on mobile devices. 

Original HTML2Image Code from MSDN Sample here: http://code.msdn.microsoft.com/wpapps/How-to-render-or-convert-7064e4e2
by Matteo Migliore

Original MJPEG Streaming Server code from here: http://www.codeproject.com/Articles/371955/Motion-JPEG-Streaming-Server
by Ragheed Al-Tayeb

Default render mode is IE7
If you want to render web page with IE9, go to the registry editor 
32-bit
HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Internet Explorer\MAIN\FeatureControl\FEATURE_BROWSER_EMULATION

64-bit 
HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\Microsoft\Internet Explorer\MAIN\FeatureControl\FEATURE_BROWSER_EMULATION

DWORD value 
name - "EpicPillow.exe" (and/or "EpicPillow.vshost.exe") Decimal value - "9999"

http://www.west-wind.com/weblog/posts/2011/May/21/Web-Browser-Control-Specifying-the-IE-Version

