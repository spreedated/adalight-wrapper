[![Donate](https://img.shields.io/badge/Donate-PayPal-green.svg)](https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=35WE5NU48AUMA&source=url)

Adalight Wrapper
================
Simple wrapper written in Visual Basic .NET (.NET Framework 4.5.2)

Simple Usage:
__VB__
~~~~
Dim AdalightDevices As List(Of String) = adalight_wrapper.Adalight.GetAdalightDevices
Dim LEDCount As Integer = 50

Using Ada = New adalight_wrapper.Adalight(AdalightDevices(0), LEDCount)
	Ada.OpenConn()
	For i = 0 To LEDCount - 1
		Ada.LEDMatrix(i) = {255, 255, 255}
	Next
	Ada.Send()
End Using
~~~~

__C#__
~~~~
List<string> AdalightDevices = adalight_wrapper.Adalight.GetAdalightDevices();
int LEDCount = 50;
using (var Ada = new adalight_wrapper.Adalight(AdalightDevices[0], 50))
{
	Ada.OpenConn();
	for (int i = 0; i < LEDCount; i++)
	{
		Ada.LEDMatrix[i] = new[] { 255, 255, 255 };
	}
	Ada.Send();
}
~~~~

#LEDMatrix
This is an array containing the RBG (note: not RGB) information for each LED.
Send RED to LED ##21 would like like this (VB example):
~~~~
Ada.LEDMatrix(21) = {255,0,0}
~~~~

### Enjoying this?
Just star the repo or make a donation.

[![Donate0](https://img.shields.io/badge/Donate-PayPal-green.svg)](https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=35WE5NU48AUMA&source=url)

Your help is valuable since this is a hobby project for all of us: we do development during out-of-office hours.

### Contribution
Pull requests are very welcome.

### Copyrights
Adalight Wrapper was initially written by **Markus Karl Wackermann**.
