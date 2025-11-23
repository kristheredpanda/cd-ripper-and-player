using Dark.Net;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CD_Player
{
    public partial class about : Form
    {
        public about()
        {
            InitializeComponent();
        }

        public void changeButtonsStyle()
        {
            foreach (Control control in this.Controls)
            {
                if (control is System.Windows.Forms.Button button)
                {
                    button.BackColor = Color.FromArgb(45, 45, 45);
                    button.ForeColor = Color.FromArgb(255, 255, 255);
                    button.FlatStyle = FlatStyle.Flat;
                    button.FlatAppearance.BorderColor = Color.FromArgb(77, 77, 77);
                }
            }
        }

        public void checkSettings()
        {
            if (Properties.Settings.Default.DISPLAY_MODE_COLOUR == "Light")
            {

            }
            else if (Properties.Settings.Default.DISPLAY_MODE_COLOUR == "Dark")
            {
                changeButtonsStyle();
                DarkNet.Instance.SetWindowThemeForms(this, Theme.Auto);
                this.BackColor = Color.FromArgb(33, 33, 33);
                this.ForeColor = Color.FromArgb(255, 255, 255);
            }
        }

        private void about_Load(object sender, EventArgs e)
        {
            checkSettings();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://ktrp.free.nf");
        }

        private const string GitHubVersionFileUrl = "https://raw.githubusercontent.com/kristheredpanda/cd-ripper-and-player/main/version.txt";
        private async void button2_Click(object sender, EventArgs e)
        {
            try
            {
                Version currentVersion = Assembly.GetExecutingAssembly().GetName().Version;
                Version latestVersion = await GetLatestVersionFromGitHub();

                if (latestVersion != null && latestVersion > currentVersion)
                {
                    MessageBox.Show($"A new version ({latestVersion}) is available! Current version: {currentVersion}. Please update.", "Update Available", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    System.Diagnostics.Process.Start("https://github.com/kristheredpanda/cd-ripper-and-player/releases");
                }
                else
                {
                    MessageBox.Show($"Your application is up to date.\nVersion: {currentVersion}", "No Update", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error checking for updates: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task<Version> GetLatestVersionFromGitHub()
        {
            using (HttpClient client = new HttpClient())
            {
                string versionString = await client.GetStringAsync(GitHubVersionFileUrl);
                return new Version(versionString.Trim());
            }
        }
    }
}
