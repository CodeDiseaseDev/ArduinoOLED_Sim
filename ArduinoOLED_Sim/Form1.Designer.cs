namespace ArduinoOLED_Sim
{
  partial class Form1
  {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.RenderTestButton = new System.Windows.Forms.Button();
      this.richTextBox1 = new System.Windows.Forms.RichTextBox();
      this.OledDisplay128x32 = new ArduinoOLED_Sim.MonoChromeOLED();
      this.SuspendLayout();
      // 
      // RenderTestButton
      // 
      this.RenderTestButton.Location = new System.Drawing.Point(40, 140);
      this.RenderTestButton.Name = "RenderTestButton";
      this.RenderTestButton.Size = new System.Drawing.Size(75, 23);
      this.RenderTestButton.TabIndex = 2;
      this.RenderTestButton.Text = "Test Render";
      this.RenderTestButton.UseVisualStyleBackColor = true;
      this.RenderTestButton.Click += new System.EventHandler(this.TestRender_Click);
      // 
      // richTextBox1
      // 
      this.richTextBox1.Location = new System.Drawing.Point(163, 12);
      this.richTextBox1.Name = "richTextBox1";
      this.richTextBox1.Size = new System.Drawing.Size(382, 229);
      this.richTextBox1.TabIndex = 4;
      this.richTextBox1.Text = "";
      // 
      // OledDisplay128x32
      // 
      this.OledDisplay128x32.BackColor = System.Drawing.Color.Black;
      this.OledDisplay128x32.Location = new System.Drawing.Point(12, 12);
      this.OledDisplay128x32.Name = "OledDisplay128x32";
      this.OledDisplay128x32.Size = new System.Drawing.Size(128, 32);
      this.OledDisplay128x32.TabIndex = 3;
      // 
      // Form1
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.BackColor = System.Drawing.Color.Gray;
      this.ClientSize = new System.Drawing.Size(557, 253);
      this.Controls.Add(this.richTextBox1);
      this.Controls.Add(this.OledDisplay128x32);
      this.Controls.Add(this.RenderTestButton);
      this.Name = "Form1";
      this.Text = "Form1";
      this.ResumeLayout(false);

    }

    #endregion
    private System.Windows.Forms.Button RenderTestButton;
    private MonoChromeOLED OledDisplay128x32;
    private System.Windows.Forms.RichTextBox richTextBox1;
  }
}

