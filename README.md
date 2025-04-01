Visual Studio 2022 & .net framework are required to run the windows program

# the software and the arduino/esp communicate over usb serial, so make sure the COM port is set correctly in your code




this program is designed for a 128x32 oled display, and must be connected with the i2c pins.
you can also connect a 128x64 oled display the same way, but you will need to change the resolution in the C# code AND the arduino code.



the arduino code is included in `computer_to_arduino_oled.ino`, it requires the `Adafruit_SSD1306` library in the library menu from the arduino IDE.



code needs a tidy, but the main juicy part (`MonoChromeOLED.cs`) is clean and easy to read and change.
