using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Рисовашки
{
    public partial class Form_size_pen : Form
    {
        public Form_size_pen(int s)
        {
            InitializeComponent();
            numericUpDown1.Value = s;
        }

        public int Size_Pen;
        private void Button1_Click(object sender, EventArgs e)
        {
             Size_Pen = Convert.ToInt32(numericUpDown1.Value);
        }
    }
}
