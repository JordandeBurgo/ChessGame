using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CollegeProject
{
    public partial class MainMenu : Form
    {
        public MainMenu()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form1 form = new Form1("S1S");
            form.Show();
            this.Hide();
            //If "Single Player" is clicked, open form1 with game code "S1S"
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form1 form = new Form1("M1B");
            form.Show();
            this.Hide();
            //If "2 Player Blitz" is clicked, open form1 with game code "M1B"
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form1 form = new Form1("M2B");
            form.Show();
            this.Hide();
            //If "2 Player Bullet" is clicked, open form1 with game code "M2B"
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Form1 form = new Form1("M3S");
            form.Show();
            this.Hide();
            //If "2 Player Suicide" is clicked, open form1 with game code "M3S"
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Help form = new Help();
            form.Show();
            //If "?" is clicked, open the "help" form (form3).
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Rules form = new Rules();
            form.Show();
            //If "Rules" is clicked, open the "rules" form (form4).
        }
    }
}
