using System;
using System.Windows.Forms;

namespace WinFormsConsoleReadonly
{
    public partial class Form2 : Form
    {
        private int PW;
        private bool hidden;
        public Form2()
        {

            InitializeComponent();
            PW = SlidingPanel.Width;
            hidden = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (hidden == true)
            {
                toolStripMenuItem1.Text = ">";
            }
            else
            {
                toolStripMenuItem1.Text = "<";
            }
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (hidden)
            {
                SlidingPanel.Width += 20;
                if (SlidingPanel.Width >= PW)
                {
                    timer1.Stop();
                    hidden = false;
                    this.Refresh();
                }
            }
            else
            {
                SlidingPanel.Width -= 20;
                if (SlidingPanel.Width <= 0)
                {
                    timer1.Stop();
                    hidden = true;
                    this.Refresh();
                }
            }
        }
 

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (hidden == true)
            {
                toolStripMenuItem1.Text = ">";
            }
            else
            {
                toolStripMenuItem1.Text = "<";
            }
            timer1.Start();
        }
    }
}
