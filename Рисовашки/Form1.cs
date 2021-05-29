using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Рисовашки
{
    public partial class Form1 : Form
    {
        private readonly Graphics gr;
        private int x = -1, y = -1;
        private bool moving = false;
        private Pen pen;
        private Color Color_Pen = Color.Black;
        private int This_size_pen = 5;

        public Form1()
        {
            InitializeComponent();
            gr = panel1.CreateGraphics();
            gr.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            pen = new Pen(Color.Black, This_size_pen);
            pen.StartCap = pen.EndCap = System.Drawing.Drawing2D.LineCap.Round;
        }

        private readonly string File_Locate = @"C:\ProgramData\Рисовашки\Locate.txt";

        public static int x_temp, y_temp;

        private void File_diagnostics()
        {
            if (!(Directory.Exists(@"C:\ProgramData")))
                Directory.CreateDirectory(@"C:\ProgramData");

            if (!(Directory.Exists(@"C:\ProgramData\Рисовашки")))
                Directory.CreateDirectory(@"C:\ProgramData\Рисовашки");
            
            if (!(File.Exists(File_Locate)))
            {
                using (StreamWriter sw = new StreamWriter(File_Locate))
                {
                    sw.Write("0; 0");
                    sw.Dispose();
                    sw.Close();
                }
            }
        }

        private void Music_click()
        {
            System.Media.SoundPlayer Cl = new System.Media.SoundPlayer(Properties.Resources.Click);
            Cl.Load();
            Cl.Play();
            Cl.Dispose();
        }

        private void Set_Locate()
        {
            int x, y;
            using (StreamReader sr = new StreamReader(File_Locate))
            {
                string Size = sr.ReadToEnd(), str = "";
                int i = 0;
                if (Size != "")
                    while (Size[i] != Size.Length)
                    {
                        if (Size[i] != ';')
                            str += Size[i];
                        else
                        {
                            x = Convert.ToInt16(str);
                            str = "";
                            for (int j = i + 1; j < Size.Length; j++)
                            {
                                str += Size[j];
                            }
                            y = Convert.ToInt16(str);
                            this.StartPosition = FormStartPosition.Manual;
                            this.Location = new Point(x, y);
                            break;
                        }
                        i++;
                    }
                else
                    this.StartPosition = FormStartPosition.CenterScreen;
                sr.Dispose();
                sr.Close();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            File_diagnostics();
            Set_Locate();
            notifyIcon1.BalloonTipTitle = "Имя программы";
            notifyIcon1.BalloonTipText = "Приложение свернуто";
            notifyIcon1.Text = "Имя приложения";
        }

        private void ВыходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (StreamWriter sw = new StreamWriter(File_Locate))
            {
                string size = x_temp + "; " + y_temp;
                sw.Write(size);
            }
            this.Close();
        }

        private void NotifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Music_click();
            this.Show();
            notifyIcon1.Visible = false;
            WindowState = FormWindowState.Normal;
        }

        private void Panel1_MouseDown(object sender, MouseEventArgs e)
        {
            moving = true;
            x = e.X;
            y = e.Y;
        }

        private void Panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (moving && x != -1 && y != -1)
            {
                gr.DrawLine(pen, new Point(x, y), e.Location);
                x = e.X; y = e.Y;
            }
        }

        private void Panel1_MouseUp(object sender, MouseEventArgs e)
        {
            moving = false;
            x = -1;
            y = -1;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            button4.BackColor = Color.White;
            ColorDialog colorDialog = new ColorDialog();
            DialogResult dialogResultColor = colorDialog.ShowDialog();
            if (dialogResultColor == DialogResult.OK)
            {
                panel1.BackColor = colorDialog.Color;
                panel2.BackColor = colorDialog.Color;
            }
            colorDialog.Dispose();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            button4.BackColor = Color.White;
            ColorDialog colorDialog = new ColorDialog();
            DialogResult dialogResultColor = colorDialog.ShowDialog();
            if (dialogResultColor == DialogResult.OK)
            {
                Color_Pen = colorDialog.Color;
                pen = new Pen(Color_Pen, This_size_pen);
                pen.StartCap = pen.EndCap = System.Drawing.Drawing2D.LineCap.Round;
            }
            colorDialog.Dispose();
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            button4.BackColor = Color.White;
            using (Form_size_pen F = new Form_size_pen(This_size_pen))
            {
                if (F.ShowDialog() == DialogResult.OK)
                {
                    This_size_pen = F.Size_Pen;
                    pen = new Pen(Color_Pen, This_size_pen);
                    pen.StartCap = pen.EndCap = System.Drawing.Drawing2D.LineCap.Round;
                }
            }
        }

        private void Button5_Click(object sender, EventArgs e)
        {
            button4.BackColor = Color.White;
            panel1.Refresh();
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            if (button4.BackColor == Color.White)
            {
                button4.BackColor = Color.LightGreen;
                pen = new Pen(panel1.BackColor, 25);
            }
            else
            {
                button4.BackColor = Color.White;
                pen = new Pen(Color_Pen, This_size_pen);
            }
            pen.StartCap = pen.EndCap = System.Drawing.Drawing2D.LineCap.Round;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.WindowState == FormWindowState.Normal)
                using (StreamWriter sw = new StreamWriter(File_Locate))
                {
                    string size = this.Location.X.ToString() + "; " + this.Location.Y.ToString();
                    sw.Write(size);
                }
            this.Cursor = Cursors.AppStarting;
            Music_click();
            gr.Dispose();
            pen.Dispose();
            System.Threading.Thread.Sleep(250);
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                x_temp = this.Location.X;
                y_temp = this.Location.Y;
                this.Hide();
                notifyIcon1.Visible = true;
                notifyIcon1.ShowBalloonTip(1000);
            }
            else
            if (FormWindowState.Normal == this.WindowState)
            {
                this.StartPosition = FormStartPosition.Manual;
                this.Location = new Point(x_temp, y_temp);
                notifyIcon1.Visible = false;
            }
        }
    }
}
