using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ArduinoOLED_Sim
{
  public partial class MonoChromeOLED : UserControl
  {
    public MonoChromeOLED()
    {
      InitializeComponent();

      DoubleBuffered = true;
    }

    public delegate void OnGDIPRenderHandler(Graphics graphics);
    public event OnGDIPRenderHandler OnGDIPRender;

    public byte[] DisplayBuffer
    {
      get => _displayBuffer;
      //private set => _displayBuffer = value;
    }

    public enum OLED_PixelColor
    {
      Black = 0,
      White = 1,
      Inverse = 2
    }


    private int _width, _height;
    private byte[] _displayBuffer = null;
    private Bitmap _finalBmp = null;



    public bool Initialize(int width, int height)
    {
      if (width <= 0 || height <= 0)
        return false;

      // UserControl class size
      Width = width;
      Height = height;

      _width = width;
      _height = height;

      _finalBmp = new Bitmap(
        width, height);

      int bits = width * height;
      int bytes = bits / 8;

      _displayBuffer = new byte[bytes];
      _displayBuffer.Initialize();


      Invalidate();
      return true;
    }








    /*private funcs*/
    private bool _GetPixelBit(int xy)
    {
      int byteIndex = xy / 8;
      int byteBitIndex = xy % 8;

      byte @byte = _displayBuffer[byteIndex];
      //bool 

      return (@byte & (1 << byteBitIndex)) != 0;
    }

    private void _SetPixelBit(int xy, OLED_PixelColor bit)
    {
      if (bit == OLED_PixelColor.Black ||
        bit == OLED_PixelColor.White)
      {
        _SetPixelBit(xy, bit);
        return;
      }

      if (bit == OLED_PixelColor.Inverse)
      {
        _SetPixelBit(xy, !_GetPixelBit(xy));
      }
    }

    private void _SetPixelBit(int xy, bool bit)
    {
      int byteIndex = xy / 8;
      int byteBitIndex = xy % 8;

      if (bit)
      {
        _displayBuffer[byteIndex] |= (byte)(1 << byteBitIndex);
      }
      else
      {
        _displayBuffer[byteIndex] &= (byte)~(1 << byteBitIndex);
      }
    }

    private bool _RenderBitmap(ref Bitmap bmp)
    {
      if (bmp == null ||
        bmp.Width < _width ||
        bmp.Height < _height)
      {
        return false;
      }

      // reset buffer
      _displayBuffer.Initialize();

      // convert to monochrome image
      for (int y = 0; y < _height; y++)
        for (int x = 0; x < _width; x++)
        {
          // on/off bit -> 0-255 int -> Color class
          bool bit = _GetPixelBit((y * _width) + x);
          int val = bit ? 255 : 0;
          Color bmpColor = Color.FromArgb(val, val, val);


          /* slow function; (can use custom bitmap class to fix this.) */
          bmp.SetPixel(x, y, bmpColor);
        }

      return true;
    }

    protected override void OnPaint(PaintEventArgs e)
    {
      if (_displayBuffer == null ||
        _finalBmp == null)
      {
        return;
      }

      for (int i = 0; i < (_width * _height) / 8; i++)
      {
        _displayBuffer[i] = 0;
      }

      if (!GDIP_Render())
        return;

      Graphics graphics = e.Graphics;
      graphics.InterpolationMode =
        InterpolationMode.NearestNeighbor;

      if (_RenderBitmap(ref _finalBmp))
      {
        graphics.DrawImage(
          _finalBmp, 0, 0,
          _width, _height);
      }
    }

    private bool DrawGDIPBitmap(Bitmap bmp, int fx, int fy, bool inverse = false)
    {
      if (bmp == null)
        return false;

      for (int y = 0; y < bmp.Height; y++)
        for (int x = 0; x < bmp.Width; x++)
        {
          Color color = bmp.GetPixel(x, y);
          int avgVal = (color.R + color.G + color.B) / 3;
          bool bit = avgVal > 255 / 3;

          if (color.A < 255 / 2)
            continue; // transparent pixel

          if (inverse)
            _SetPixelBit((y * _width) + x, OLED_PixelColor.Inverse);
          else
            _SetPixelBit((y * _width) + x, bit);
        }

      return true;
    }

    private bool GDIP_Render()
    {
      if (_displayBuffer == null ||
        _finalBmp == null)
      {
        return false;
      }

      using (var dummyBmp = new Bitmap(
        _width, _height))
      {
        using (var gdip = Graphics.FromImage(dummyBmp))
        {
          gdip.SmoothingMode = SmoothingMode.None;
          gdip.TextRenderingHint = TextRenderingHint.SingleBitPerPixelGridFit;

          OnGDIPRender?.Invoke(gdip);
        }

        DrawGDIPBitmap(dummyBmp, 0, 0, false);
      }

      return true;
    }
  }
}
