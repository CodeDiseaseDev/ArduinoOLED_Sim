using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using GlobalLowLevelHooks;

namespace ArduinoOLED_Sim
{
  public partial class Form1 : Form
  {
    public Form1()
    {
      InitializeComponent();

      OledDisplay128x32.Initialize(128, 32);

      OledDisplay128x32.OnGDIPRender += OledDisplay128x32_OnGDIPRender;

      this.FormClosing += Form1_FormClosing;

      mouseHook.MouseMove += MouseHook_MouseMove;
      mouseHook.Install();

      port.DataReceived += Port_DataReceived;
      port.Open();

      thread = new Thread(() =>
      {
        while (true)
        {
          Rerender();
          Thread.Sleep(10);
        }
      });
      thread.Start();
    }

    private void Form1_FormClosing(object sender, FormClosingEventArgs e)
    {
      mouseHook.Uninstall();
    }

    Thread thread;

    int x, y;

    private void OledDisplay128x32_OnGDIPRender(Graphics graphics)
    {
      graphics.CopyFromScreen(
        new Point(x - 64, y - 16),
        new Point(0, 0), new Size(128, 32));
    }

    private void MouseHook_MouseMove(MouseHook.MSLLHOOKSTRUCT mouseStruct)
    {
      x = mouseStruct.pt.x;
      y = mouseStruct.pt.y;
      //Rerender();
    }

    private void Port_DataReceived(object sender, SerialDataReceivedEventArgs e)
    {
      richTextBox1.Text += port.ReadExisting();
    }

    private SerialPort port = new SerialPort("COM5", 115200);
    MouseHook mouseHook = new MouseHook();

    void Rerender()
    {
      OledDisplay128x32.Invalidate();

      if (port.IsOpen)
      {
        port.Write(
          OledDisplay128x32.DisplayBuffer,
          0, OledDisplay128x32.DisplayBuffer.Length);
      }
    }

    private void timer1_Tick(object sender, EventArgs e)
    {
      Rerender();
    }

    private void TestRender_Click(object sender, EventArgs e)
    {
      Rerender();
    }
  }
}
