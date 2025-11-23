using IWshRuntimeLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace cdRipnPlayInstaller
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        Point greenArrowLocation1 = new Point(11, 29);
        Point greenArrowLocation2 = new Point(11, 59);
        Point greenArrowLocation3 = new Point(11, 129);
        Point greenArrowLocation4 = new Point(11, 160);

        string defaultInstallLocation = "C:\\Program Files\\CD Ripper and Player";
        private void Form1_Load(object sender, EventArgs e)
        {
            string appLocation = defaultInstallLocation + "\\app.exe";

            if (!System.IO.File.Exists(appLocation))
            {
                this.Text = formname.Text + ": Setup";
                label1.Text = "Welcome to the setup";
                label2.Text = "This will install the program: CD Ripper and Player\n\nClick \"Next\" to move to the next page";
                button1.Text = "Next";
                button2.Visible = false;

                welcomeLabel.Font = new Font("Microsoft YaHei", 11, FontStyle.Bold);
                greenArrow.Location = greenArrowLocation1;

                Directory.CreateDirectory(defaultInstallLocation);
            }
            else if (System.IO.File.Exists(appLocation))
            {
                this.Text = formname.Text + ": Setup";
                label1.Text = "Welcome to the setup";
                label2.Text = "It seems you already have \"CD Ripper and Player\" installed.\n\nWould you like to uninstall the program or update it?";
                button1.Text = "Update";
                button2.Visible = true;

                welcomeLabel.Font = new Font("Microsoft YaHei", 11, FontStyle.Bold);
                greenArrow.Location = greenArrowLocation1;
            }
        }

        public void CreateDesktopShortcut(string appPath, string shortcutName, string description, string iconPath)
        {
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            string shortcutLocation = Path.Combine(desktopPath, shortcutName + ".lnk");

            WshShell shell = new WshShell();

            IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutLocation);

            shortcut.Description = description;
            shortcut.TargetPath = appPath;
            shortcut.WorkingDirectory = Path.GetDirectoryName(appPath);

            if (!string.IsNullOrEmpty(iconPath) && System.IO.File.Exists(iconPath))
            {
                shortcut.IconLocation = iconPath;
            }

            shortcut.Save();
        }

        public void extractFiles()
        {
            System.IO.File.WriteAllBytes(defaultInstallLocation + "\\app.exe", Properties.Resources.CD_Player);
            progressBar1.Value = progressBar1.Value + 1;
            System.IO.File.WriteAllBytes(defaultInstallLocation + "\\DarkNet.dll", Properties.Resources.DarkNet);
            progressBar1.Value = progressBar1.Value + 1;
            System.IO.File.WriteAllBytes(defaultInstallLocation + "\\DarkNet.xml", Properties.Resources.DarkNet1);
            progressBar1.Value = progressBar1.Value + 1;
            System.IO.File.WriteAllBytes(defaultInstallLocation + "\\libmp3lame.32.dll", Properties.Resources.libmp3lamex32);
            progressBar1.Value = progressBar1.Value + 1;
            System.IO.File.WriteAllBytes(defaultInstallLocation + "\\libmp3lame.64.dll", Properties.Resources.libmp3lamex64);
            progressBar1.Value = progressBar1.Value + 1;
            System.IO.File.WriteAllBytes(defaultInstallLocation + "\\Microsoft.Win32.Registry.dll", Properties.Resources.Microsoft_Win32_Registry);
            progressBar1.Value = progressBar1.Value + 1;
            System.IO.File.WriteAllBytes(defaultInstallLocation + "\\Microsoft.Win32.Registry.xml", Properties.Resources.Microsoft_Win32_Registry1);
            progressBar1.Value = progressBar1.Value + 1;
            System.IO.File.WriteAllBytes(defaultInstallLocation + "\\NAudio.Asio.dll", Properties.Resources.NAudio_Asio);
            progressBar1.Value = progressBar1.Value + 1;
            System.IO.File.WriteAllBytes(defaultInstallLocation + "\\NAudio.Asio.xml", Properties.Resources.NAudio_Asio1);
            progressBar1.Value = progressBar1.Value + 1;
            System.IO.File.WriteAllBytes(defaultInstallLocation + "\\NAudio.Core.dll", Properties.Resources.NAudio_Core);
            progressBar1.Value = progressBar1.Value + 1;
            System.IO.File.WriteAllBytes(defaultInstallLocation + "\\NAudio.Core.xml", Properties.Resources.NAudio_Core1);
            progressBar1.Value = progressBar1.Value + 1;
            System.IO.File.WriteAllBytes(defaultInstallLocation + "\\NAudio.dll", Properties.Resources.NAudio);
            progressBar1.Value = progressBar1.Value + 1;
            System.IO.File.WriteAllBytes(defaultInstallLocation + "\\NAudio.Lame.dll", Properties.Resources.NAudio_Lame);
            progressBar1.Value = progressBar1.Value + 1;
            System.IO.File.WriteAllBytes(defaultInstallLocation + "\\NAudio.Midi.dll", Properties.Resources.NAudio_Midi);
            progressBar1.Value = progressBar1.Value + 1;
            System.IO.File.WriteAllBytes(defaultInstallLocation + "\\NAudio.Midi.xml", Properties.Resources.NAudio_Midi1);
            progressBar1.Value = progressBar1.Value + 1;
            System.IO.File.WriteAllBytes(defaultInstallLocation + "\\NAudio.Wasapi.dll", Properties.Resources.NAudio_Wasapi);
            progressBar1.Value = progressBar1.Value + 1;
            System.IO.File.WriteAllBytes(defaultInstallLocation + "\\NAudio.Wasapi.xml", Properties.Resources.NAudio_Wasapi1);
            progressBar1.Value = progressBar1.Value + 1;
            System.IO.File.WriteAllBytes(defaultInstallLocation + "\\NAudio.WinForms.dll", Properties.Resources.NAudio_WinForms);
            progressBar1.Value = progressBar1.Value + 1;
            System.IO.File.WriteAllBytes(defaultInstallLocation + "\\NAudio.WinForms.xml", Properties.Resources.NAudio_WinForms1);
            progressBar1.Value = progressBar1.Value + 1;
            System.IO.File.WriteAllBytes(defaultInstallLocation + "\\NAudio.WinMM.dll", Properties.Resources.NAudio_WinMM);
            progressBar1.Value = progressBar1.Value + 1;
            System.IO.File.WriteAllBytes(defaultInstallLocation + "\\NAudio.WinMM.xml", Properties.Resources.NAudio_WinMM1);
            progressBar1.Value = progressBar1.Value + 1;
            System.IO.File.WriteAllBytes(defaultInstallLocation + "\\NAudio.xml", Properties.Resources.NAudio1);
            progressBar1.Value = progressBar1.Value + 1;
            System.IO.File.WriteAllBytes(defaultInstallLocation + "\\System.Security.AccessControl.dll", Properties.Resources.System_Security_AccessControl);
            progressBar1.Value = progressBar1.Value + 1;
            System.IO.File.WriteAllBytes(defaultInstallLocation + "\\System.Security.AccessControl.xml", Properties.Resources.System_Security_AccessControl1);
            progressBar1.Value = progressBar1.Value + 1;
            System.IO.File.WriteAllBytes(defaultInstallLocation + "\\System.Security.Principal.Windows.dll", Properties.Resources.System_Security_Principal_Windows);
            progressBar1.Value = progressBar1.Value + 1;
            System.IO.File.WriteAllBytes(defaultInstallLocation + "\\System.Security.Principal.Windows.xml", Properties.Resources.System_Security_Principal_Windows1);
            progressBar1.Value = progressBar1.Value + 1;
            System.IO.File.WriteAllBytes(defaultInstallLocation + "\\TagLibSharp.dll", Properties.Resources.TagLibSharp);
            progressBar1.Value = progressBar1.Value + 1;
            System.IO.File.WriteAllBytes(defaultInstallLocation + "\\TagLibSharp.xml", Properties.Resources.TagLibSharp1);
            progressBar1.Value = progressBar1.Value + 1;

            timer2.Start();
        }

        public void deleteFiles()
        {
            if (label1.Text == "Uninstalling")
            {
                System.IO.File.Delete(defaultInstallLocation + "\\app.exe");
                progressBar1.Value = progressBar1.Value + 1;
                System.IO.File.Delete(defaultInstallLocation + "\\DarkNet.dll");
                progressBar1.Value = progressBar1.Value + 1;
                System.IO.File.Delete(defaultInstallLocation + "\\DarkNet.xml");
                progressBar1.Value = progressBar1.Value + 1;
                System.IO.File.Delete(defaultInstallLocation + "\\libmp3lame.32.dll");
                progressBar1.Value = progressBar1.Value + 1;
                System.IO.File.Delete(defaultInstallLocation + "\\libmp3lame.64.dll");
                progressBar1.Value = progressBar1.Value + 1;
                System.IO.File.Delete(defaultInstallLocation + "\\Microsoft.Win32.Registry.dll");
                progressBar1.Value = progressBar1.Value + 1;
                System.IO.File.Delete(defaultInstallLocation + "\\Microsoft.Win32.Registry.xml");
                progressBar1.Value = progressBar1.Value + 1;
                System.IO.File.Delete(defaultInstallLocation + "\\NAudio.Asio.dll");
                progressBar1.Value = progressBar1.Value + 1;
                System.IO.File.Delete(defaultInstallLocation + "\\NAudio.Asio.xml");
                progressBar1.Value = progressBar1.Value + 1;
                System.IO.File.Delete(defaultInstallLocation + "\\NAudio.Core.dll");
                progressBar1.Value = progressBar1.Value + 1;
                System.IO.File.Delete(defaultInstallLocation + "\\NAudio.Core.xml");
                progressBar1.Value = progressBar1.Value + 1;
                System.IO.File.Delete(defaultInstallLocation + "\\NAudio.dll");
                progressBar1.Value = progressBar1.Value + 1;
                System.IO.File.Delete(defaultInstallLocation + "\\NAudio.Lame.dll");
                progressBar1.Value = progressBar1.Value + 1;
                System.IO.File.Delete(defaultInstallLocation + "\\NAudio.Midi.dll");
                progressBar1.Value = progressBar1.Value + 1;
                System.IO.File.Delete(defaultInstallLocation + "\\NAudio.Midi.xml");
                progressBar1.Value = progressBar1.Value + 1;
                System.IO.File.Delete(defaultInstallLocation + "\\NAudio.Wasapi.dll");
                progressBar1.Value = progressBar1.Value + 1;
                System.IO.File.Delete(defaultInstallLocation + "\\NAudio.Wasapi.xml");
                progressBar1.Value = progressBar1.Value + 1;
                System.IO.File.Delete(defaultInstallLocation + "\\NAudio.WinForms.dll");
                progressBar1.Value = progressBar1.Value + 1;
                System.IO.File.Delete(defaultInstallLocation + "\\NAudio.WinForms.xml");
                progressBar1.Value = progressBar1.Value + 1;
                System.IO.File.Delete(defaultInstallLocation + "\\NAudio.WinMM.dll");
                progressBar1.Value = progressBar1.Value + 1;
                System.IO.File.Delete(defaultInstallLocation + "\\NAudio.WinMM.xml");
                progressBar1.Value = progressBar1.Value + 1;
                System.IO.File.Delete(defaultInstallLocation + "\\NAudio.xml");
                progressBar1.Value = progressBar1.Value + 1;
                System.IO.File.Delete(defaultInstallLocation + "\\System.Security.AccessControl.dll");
                progressBar1.Value = progressBar1.Value + 1;
                System.IO.File.Delete(defaultInstallLocation + "\\System.Security.AccessControl.xml");
                progressBar1.Value = progressBar1.Value + 1;
                System.IO.File.Delete(defaultInstallLocation + "\\System.Security.Principal.Windows.dll");
                progressBar1.Value = progressBar1.Value + 1;
                System.IO.File.Delete(defaultInstallLocation + "\\System.Security.Principal.Windows.xml");
                progressBar1.Value = progressBar1.Value + 1;
                System.IO.File.Delete(defaultInstallLocation + "\\TagLibSharp.dll");
                progressBar1.Value = progressBar1.Value + 1;
                System.IO.File.Delete(defaultInstallLocation + "\\TagLibSharp.xml");
                progressBar1.Value = progressBar1.Value + 1;
                System.IO.Directory.Delete(defaultInstallLocation, true);
                progressBar1.Value = progressBar1.Value + 1;
                System.IO.File.Delete("C:\\Users\\" + Environment.UserName + "\\Desktop\\CD Ripper & Player.lnk");
                progressBar1.Value = progressBar1.Value + 1;
            }
            else if (label1.Text == "Updating")
            {
                System.IO.File.Delete(defaultInstallLocation + "\\app.exe");
                progressBar1.Value = progressBar1.Value + 1;
                System.IO.File.Delete(defaultInstallLocation + "\\DarkNet.dll");
                progressBar1.Value = progressBar1.Value + 1;
                System.IO.File.Delete(defaultInstallLocation + "\\DarkNet.xml");
                progressBar1.Value = progressBar1.Value + 1;
                System.IO.File.Delete(defaultInstallLocation + "\\libmp3lame.32.dll");
                progressBar1.Value = progressBar1.Value + 1;
                System.IO.File.Delete(defaultInstallLocation + "\\libmp3lame.64.dll");
                progressBar1.Value = progressBar1.Value + 1;
                System.IO.File.Delete(defaultInstallLocation + "\\Microsoft.Win32.Registry.dll");
                progressBar1.Value = progressBar1.Value + 1;
                System.IO.File.Delete(defaultInstallLocation + "\\Microsoft.Win32.Registry.xml");
                progressBar1.Value = progressBar1.Value + 1;
                System.IO.File.Delete(defaultInstallLocation + "\\NAudio.Asio.dll");
                progressBar1.Value = progressBar1.Value + 1;
                System.IO.File.Delete(defaultInstallLocation + "\\NAudio.Asio.xml");
                progressBar1.Value = progressBar1.Value + 1;
                System.IO.File.Delete(defaultInstallLocation + "\\NAudio.Core.dll");
                progressBar1.Value = progressBar1.Value + 1;
                System.IO.File.Delete(defaultInstallLocation + "\\NAudio.Core.xml");
                progressBar1.Value = progressBar1.Value + 1;
                System.IO.File.Delete(defaultInstallLocation + "\\NAudio.dll");
                progressBar1.Value = progressBar1.Value + 1;
                System.IO.File.Delete(defaultInstallLocation + "\\NAudio.Lame.dll");
                progressBar1.Value = progressBar1.Value + 1;
                System.IO.File.Delete(defaultInstallLocation + "\\NAudio.Midi.dll");
                progressBar1.Value = progressBar1.Value + 1;
                System.IO.File.Delete(defaultInstallLocation + "\\NAudio.Midi.xml");
                progressBar1.Value = progressBar1.Value + 1;
                System.IO.File.Delete(defaultInstallLocation + "\\NAudio.Wasapi.dll");
                progressBar1.Value = progressBar1.Value + 1;
                System.IO.File.Delete(defaultInstallLocation + "\\NAudio.Wasapi.xml");
                progressBar1.Value = progressBar1.Value + 1;
                System.IO.File.Delete(defaultInstallLocation + "\\NAudio.WinForms.dll");
                progressBar1.Value = progressBar1.Value + 1;
                System.IO.File.Delete(defaultInstallLocation + "\\NAudio.WinForms.xml");
                progressBar1.Value = progressBar1.Value + 1;
                System.IO.File.Delete(defaultInstallLocation + "\\NAudio.WinMM.dll");
                progressBar1.Value = progressBar1.Value + 1;
                System.IO.File.Delete(defaultInstallLocation + "\\NAudio.WinMM.xml");
                progressBar1.Value = progressBar1.Value + 1;
                System.IO.File.Delete(defaultInstallLocation + "\\NAudio.xml");
                progressBar1.Value = progressBar1.Value + 1;
                System.IO.File.Delete(defaultInstallLocation + "\\System.Security.AccessControl.dll");
                progressBar1.Value = progressBar1.Value + 1;
                System.IO.File.Delete(defaultInstallLocation + "\\System.Security.AccessControl.xml");
                progressBar1.Value = progressBar1.Value + 1;
                System.IO.File.Delete(defaultInstallLocation + "\\System.Security.Principal.Windows.dll");
                progressBar1.Value = progressBar1.Value + 1;
                System.IO.File.Delete(defaultInstallLocation + "\\System.Security.Principal.Windows.xml");
                progressBar1.Value = progressBar1.Value + 1;
                System.IO.File.Delete(defaultInstallLocation + "\\TagLibSharp.dll");
                progressBar1.Value = progressBar1.Value + 1;
                System.IO.File.Delete(defaultInstallLocation + "\\TagLibSharp.xml");
                progressBar1.Value = progressBar1.Value + 1;
                System.IO.File.Delete("C:\\Users\\" + Environment.UserName + "\\Desktop\\CD Ripper & Player.lnk");
                progressBar1.Value = progressBar1.Value + 1;

                timer4.Start();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (button1.Text == "Next")
            {
                if (greenArrow.Location == greenArrowLocation1)
                {
                    greenArrow.Location = greenArrowLocation2;
                    welcomeLabel.Font = new Font("Microsoft YaHei", 11, FontStyle.Regular);
                    PiLLabel.Font = new Font("Microsoft YaHei", 11, FontStyle.Bold);
                    label1.Text = "Program install location";
                    label2.Text = "By default, this program will be installed to the directory below:\n" + defaultInstallLocation;
                }
                else if (greenArrow.Location == greenArrowLocation2)
                {
                    greenArrow.Location = greenArrowLocation3;
                    PiLLabel.Font = new Font("Microsoft YaHei", 11, FontStyle.Regular);
                    installLabel.Font = new Font("Microsoft YaHei", 11, FontStyle.Bold);
                    installLabel.Text = "Installing";
                    label1.Text = "Installing";
                    label2.Text = "Extracting files.";
                    progressBar1.Minimum = 0;
                    progressBar1.Maximum = 28;
                    progressBar1.Value = 0;
                    progressBar1.Visible = true;
                    button1.Enabled = false;
                    extractFiles();
                    Application.DoEvents();
                }
                else if (label1.Text == "Installation complete." || label1.Text == "Update complete.")
                {
                    this.Hide();
                    timer3.Start();
                }
            }
            else if (button1.Text == "Update")
            {
                greenArrow.Location = greenArrowLocation3;
                welcomeLabel.Font = new Font("Microsoft YaHei", 11, FontStyle.Regular);
                installLabel.Font = new Font("Microsoft YaHei", 11, FontStyle.Bold);
                installLabel.Text = "Updating";
                label1.Text = "Updating";
                label2.Text = "Uninstalling current program version files.";
                progressBar1.Minimum = 0;
                progressBar1.Maximum = 29;
                progressBar1.Value = 0;
                progressBar1.Visible = true;
                button1.Enabled = false;
                button2.Enabled = false;
                deleteFiles();
                Application.DoEvents();
                Directory.CreateDirectory(defaultInstallLocation);
                progressBar1.Value = 0;
                label2.Text = "Installing new program version files.";
                extractFiles();
                Application.DoEvents();
                button1.Text = "Next";
            }
            else if (button1.Text == "Exit")
            {
                Application.Exit();
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            CreateDesktopShortcut(defaultInstallLocation + "\\app.exe", "CD Ripper & Player", "A program that rips music from CD's and saves it to your computer, and plays them too.", this.Icon.ToString());
            progressBar1.Visible = false;
            
            if (label1.Text == "Installing")
            {
                installLabel.Text = "Install";
                label1.Text = "Installation complete.";
            }
            else if (label1.Text == "Updating")
            {
                installLabel.Text = "Update";
                label1.Text = "Update complete.";
            }

            label2.Text = "You can now click \"Next\" to proceed to finalizing this setup.\n\nIf you want to add metadata to the .mp3 files once they're ripped from the CD,\nuse a program called MP3Tag to do it manually.\nhttps://www.mp3tag.de/en/download.html";
            button1.Enabled = true;
            greenArrow.Location = greenArrowLocation3;
            installLabel.Font = new Font("Microsoft YaHei", 11, FontStyle.Bold);
            timer2.Stop();
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(defaultInstallLocation + "\\app.exe");
            Application.Exit();
            timer3.Stop();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            greenArrow.Location = greenArrowLocation3;
            welcomeLabel.Font = new Font("Microsoft YaHei", 11, FontStyle.Regular);
            installLabel.Font = new Font("Microsoft YaHei", 11, FontStyle.Bold);
            installLabel.Text = "Uninstalling";
            label1.Text = "Uninstalling";
            label2.Text = "Uninstalling program files.";
            progressBar1.Minimum = 0;
            progressBar1.Maximum = 30;
            progressBar1.Value = 0;
            progressBar1.Visible = true;
            button1.Enabled = false;
            button2.Enabled = false;
            deleteFiles();
            Application.DoEvents();
            installLabel.Text = "Uninstall";
            label1.Text = "Uninstall";
            label2.Text = "Uninstall complete, you can now exit the setup.";
            button1.Text = "Exit";
            button1.Enabled = true;
            button2.Enabled = false;
        }

        private void timer4_Tick(object sender, EventArgs e)
        {
            greenArrow.Location = greenArrowLocation3;
            installLabel.Font = new Font("Microsoft YaHei", 11, FontStyle.Bold);
            installLabel.Text = "Update";
            label1.Text = "Update complete.";
            label2.Text = "You can now click \"Next\" to proceed to finalizing this setup.\n\nIf you want to add metadata to the .mp3 files once they're ripped from the CD,\nuse a program called MP3Tag to do it manually.\nhttps://www.mp3tag.de/en/download.html";
            progressBar1.Value = 0;
            progressBar1.Visible = false;
            button1.Text = "Next";
            button1.Enabled = true;
            button2.Visible = false;
            timer4.Stop();
        }
    }
}
