using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Minecraft_Offline
{
    public partial class Form1 : Form
    {
        private int progressValue = 0;

        public Form1()
        {
            InitializeComponent();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (progressValue < 100)
            {
                progressValue++;
                progressBar1.Value = progressValue;
            }
            else
            {
                timer1.Stop();

                // Open Form2
                Form2 form2 = new Form2();
                form2.Show();

                // Hide Form1 (so the program doesn't close)
                this.Hide();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            progressBar1.Minimum = 0;
            progressBar1.Maximum = 100;
            progressBar1.Value = 0;

            timer1.Interval = 100; // 0.1 second
            timer1.Start();
        }

        private void progressBar1_Click(object sender, EventArgs e)
        {

        }
    }
}
