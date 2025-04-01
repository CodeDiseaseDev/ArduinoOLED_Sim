#include <SPI.h>
#include <Wire.h>
#include <Adafruit_GFX.h>
#include <Adafruit_SSD1306.h>

#define SCREEN_WIDTH 128
#define SCREEN_HEIGHT 32

Adafruit_SSD1306 display(SCREEN_WIDTH, SCREEN_HEIGHT, &Wire, -1);

#define DISP_BUF_SIZE (SCREEN_WIDTH * SCREEN_HEIGHT / 8)

uint8_t load_buffer[DISP_BUF_SIZE];
uint8_t double_buffer[DISP_BUF_SIZE];

void setup()
{
  Serial.begin(115200);

  if (!display.begin(SSD1306_SWITCHCAPVCC, 0x3C))
  {
    Serial.println("SSD1306 allocation failed");
    while(1);
  }

  memset(double_buffer, 255, DISP_BUF_SIZE);
}

int _index = 0;
int percent = 0;
void buffer_tick()
{
  while (Serial.available())
  {
    int d = Serial.read();

    int p = ((float)_index / (float)DISP_BUF_SIZE) * 100;
    if (p != percent)
    {
      // Serial.print(p);
      // Serial.print("-");
    }
    percent = p;
    
    load_buffer[_index++] = d;

    if (_index >= DISP_BUF_SIZE)
    {
      _index = 0;

      memcpy(
        double_buffer,
        load_buffer,
        DISP_BUF_SIZE
      );

      // Serial.println("received image!");
    }
  }
}

bool get_bit(uint8_t byte, int i)
{
  return (byte & (1 << i)) != 0;
}

void draw_bmp()
{
  display.startWrite();

  for (int y = 0; y < SCREEN_HEIGHT; y++)
    for (int x = 0; x < SCREEN_WIDTH; x++)
    {
      int xy = (y * SCREEN_WIDTH) + x;
      int byteIndex = xy / 8;
      int byteBitIndex = xy % 8;

      bool pixel = get_bit(
        double_buffer[byteIndex],
        byteBitIndex);

      display.writePixel(x, y, pixel);
    }

  display.endWrite();
}

void loop()
{
  buffer_tick();

  display.clearDisplay();
  // display.drawBitmap(
  //   0, 0, double_buffer,
  //   SCREEN_WIDTH,
  //   SCREEN_HEIGHT,
  //   SSD1306_WHITE
  // );
  draw_bmp();

  display.display();

}
