using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CD_Player
{
    public partial class finalizeappsetup : Form
    {
        public finalizeappsetup()
        {
            InitializeComponent();
        }

        Point greenArrowLocation1 = new Point(11, 29);
        Point greenArrowLocation2 = new Point(11, 59);
        Point greenArrowLocation3 = new Point(11, 129);
        Point greenArrowLocation4 = new Point(11, 160);

        private void finalizeappsetup_Load(object sender, EventArgs e)
        {
            this.Text = formname.Text + ": Finalize Setup";
            label4.Text = "Light";
            doneLabel.Font = new Font("Microsoft YaHei", 11, FontStyle.Bold);
            greenArrow.Location = greenArrowLocation4;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            label4.Text = "Light";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            label4.Text = "Dark";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.DISPLAY_MODE_COLOUR = label4.Text;
            Properties.Settings.Default.IS_APP_SETUP_FINISHED = true;
            Properties.Settings.Default.Save();
            Application.Exit();
        }
    }
}
