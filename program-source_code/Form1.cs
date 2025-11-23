using Dark.Net;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Media;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TagLib;
using static System.Collections.Specialized.BitVector32;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrackBar;

namespace CD_Player
{
    public partial class Form1 : Form
    {
        #region Kernel32 stuff
        [System.Runtime.InteropServices.DllImport("Kernel32.dll")]
        public extern static int FlushFileBuffers(IntPtr FileHandle);

        [System.Runtime.InteropServices.DllImport("Kernel32.dll")]
        public extern static DriveTypes GetDriveType(string drive);

        [System.Runtime.InteropServices.DllImport("Kernel32.dll", SetLastError = true)]
        public extern static IntPtr CreateFile(string FileName, uint DesiredAccess,
          uint ShareMode, IntPtr lpSecurityAttributes,
          uint CreationDisposition, uint dwFlagsAndAttributes,
          IntPtr hTemplateFile);

        [DllImport("kernel32.dll")]
        public static extern int WriteFile(IntPtr hFile,
        [MarshalAs(UnmanagedType.LPArray)] byte[] lpBuffer, // also tried this.
        uint nNumberOfBytesToWrite,
        out uint lpNumberOfBytesWritten,
        IntPtr lpOverlapped);

        [System.Runtime.InteropServices.DllImport("Kernel32.dll", SetLastError = true)]
        public extern static int DeviceIoControl(IntPtr hDevice, uint IoControlCode,
          IntPtr lpInBuffer, uint InBufferSize,
          IntPtr lpOutBuffer, uint nOutBufferSize,
          ref uint lpBytesReturned,
          IntPtr lpOverlapped);

        [System.Runtime.InteropServices.DllImport("Kernel32.dll", SetLastError = true)]
        public extern static int CloseHandle(IntPtr hObject);
        public enum TRACK_MODE_TYPE { YellowMode2, XAForm2, CDDA }

        [StructLayout(LayoutKind.Sequential)]
        public class RAW_READ_INFO
        {
            public long DiskOffset = 0;
            public uint SectorCount = 0;
            public TRACK_MODE_TYPE TrackMode = TRACK_MODE_TYPE.CDDA;
        }

        public enum DriveTypes : uint
        {
            DRIVE_UNKNOWN = 0,
            DRIVE_NO_ROOT_DIR,
            DRIVE_REMOVABLE,
            DRIVE_FIXED,
            DRIVE_REMOTE,
            DRIVE_CDROM,
            DRIVE_RAMDISK
        };

        [StructLayout(LayoutKind.Sequential)]
        public class PREVENT_MEDIA_REMOVAL
        {
            public byte PreventMediaRemoval = 0;
        }

        public struct TRACK_DATA
        {
            public byte Reserved;
            private byte BitMapped;

            public byte Control
            {
                get
                {
                    return (byte)(BitMapped & 0x0F);
                }
                set
                {
                    BitMapped = (byte)((BitMapped & 0xF0) | (value & (byte)0x0F));
                }
            }

            public byte Adr
            {
                get
                {
                    return (byte)((BitMapped & (byte)0xF0) >> 4);
                }
                set
                {
                    BitMapped = (byte)((BitMapped & (byte)0x0F) | (value << 4));
                }
            }

            public byte TrackNumber;
            public byte Reserved1;
            public byte Address_0;
            public byte Address_1;
            public byte Address_2;
            public byte Address_3;
        };

        [StructLayout(LayoutKind.Sequential)]
        public class TrackDataList
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = MAXIMUM_NUMBER_TRACKS * 8)]
            private byte[] Data;
            public TRACK_DATA this[int Index]
            {
                get
                {
                    if ((Index < 0) | (Index >= MAXIMUM_NUMBER_TRACKS))
                    {
                        throw new IndexOutOfRangeException();
                    }
                    TRACK_DATA res;
                    GCHandle handle = GCHandle.Alloc(Data, GCHandleType.Pinned);
                    try
                    {
                        IntPtr buffer = handle.AddrOfPinnedObject();
                        buffer = (IntPtr)(buffer.ToInt32() + (Index * Marshal.SizeOf(typeof(TRACK_DATA))));
                        res = (TRACK_DATA)Marshal.PtrToStructure(buffer, typeof(TRACK_DATA));
                    }
                    finally
                    {
                        handle.Free();
                    }
                    return res;
                }
            }
            public TrackDataList()
            {
                Data = new byte[MAXIMUM_NUMBER_TRACKS * Marshal.SizeOf(typeof(TRACK_DATA))];
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public class CDROM_TOC
        {
            public ushort Length;
            public byte FirstTrack = 0;
            public byte LastTrack = 0;
            public TrackDataList TrackData;

            public CDROM_TOC()
            {
                TrackData = new TrackDataList();
                Length = (ushort)Marshal.SizeOf(this);
            }
        }

        protected const int NSECTORS = 1;
        protected const int UNDERSAMPLING = 1;
        protected const int CB_CDDASECTOR = 2368;
        protected const int CB_QSUBCHANNEL = 16;
        protected const int CB_CDROMSECTOR = 2048;
        protected const int CB_AUDIO = (CB_CDDASECTOR - CB_QSUBCHANNEL);

        public const uint IOCTL_CDROM_READ_TOC = 0x00024000;
        public const uint IOCTL_STORAGE_CHECK_VERIFY = 0x002D4800;
        public const uint IOCTL_CDROM_RAW_READ = 0x0002403E;
        public const uint IOCTL_STORAGE_MEDIA_REMOVAL = 0x002D4804;
        public const uint IOCTL_STORAGE_EJECT_MEDIA = 0x002D4808;
        public const uint IOCTL_STORAGE_LOAD_MEDIA = 0x002D480C;
        public const uint GENERIC_READ = 0x80000000;
        public const uint GENERIC_WRITE = 0x40000000;
        public const uint GENERIC_EXECUTE = 0x20000000;
        public const uint GENERIC_ALL = 0x10000000;

        public const uint FILE_SHARE_READ = 0x00000001;
        public const uint FILE_SHARE_WRITE = 0x00000002;
        public const uint FILE_SHARE_DELETE = 0x00000004;

        public const uint CREATE_NEW = 1;
        public const uint CREATE_ALWAYS = 2;
        public const uint OPEN_EXISTING = 3;
        public const uint OPEN_ALWAYS = 4;
        public const uint TRUNCATE_EXISTING = 5;
        public const uint FILE_ATTRIBUTE_NORMAL = 0x80;
        public const int MAXIMUM_NUMBER_TRACKS = 100;
        #endregion

        byte[][] TrackData;

        private AudioFileReader audioFileReader;

        public Form1()
        {
            InitializeComponent();
        }

        SoundPlayer player = new SoundPlayer();

        private void checkForCD_Tick(object sender, EventArgs e)
        {
            string[] logDrives = System.IO.Directory.GetLogicalDrives();

            string s = "";
            StringBuilder volumeName = new StringBuilder(256);
            int srNum = new int();
            int comLen = new int();
            string sysName = "";
            int sysFlags = new int();
            int result;

            for (int i = 0; i < logDrives.Length; i++)
            {
                if (api.GetDriveType(logDrives[i]) == 5)
                {
                    s += "Your CD ROM is on drive : " +
                              logDrives[i].ToString() + "\n";
                    result = api.GetVolumeInformation(logDrives[i].ToString(),
                            volumeName, 256, srNum, comLen, sysFlags, sysName, 256);
                    if (result == 0)
                    {
                        s += "there is NO CD in ur CD ROM";
                        button3.Enabled = false;
                        label5.Text = "No CD found in drive " + logDrives[i].ToString() + "\n";
                    }
                    else
                    {
                        s += "There is a CD inside ur CD ROM and its name is " + volumeName;
                        button3.Enabled = true;
                        label5.Text = "CD in drive " + logDrives[i].ToString() + "\n";
                    }
                }
            }
        }

        private void PopulateListBox(ListBox lsb, string Folder, string FileType)
        {
            DirectoryInfo dinfo = new DirectoryInfo(Folder);
            FileInfo[] Files = dinfo.GetFiles(FileType);
            foreach (FileInfo file in Files)
            {
                lsb.Items.Add(file.Name);
            }
        }

        public void FillListBoxWAV()
        {
            listBox1.Items.Clear();
            PopulateListBox(listBox1, Application.StartupPath + "\\songs", "*.mp3");
        }

        public void picBoxShadow(object sender, PaintEventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                int shadowOffset = 4;
                Color shadowColor = Color.FromArgb(100, 0, 0, 0);

                Bitmap shadowImage = new Bitmap(pictureBox1.Width, pictureBox1.Height, PixelFormat.Format32bppArgb);
                using (Graphics gShadow = Graphics.FromImage(shadowImage))
                {
                    gShadow.FillRectangle(new SolidBrush(shadowColor), 0, 0, shadowImage.Width, shadowImage.Height);
                }

                Point shadowLocation = new Point(
                    pictureBox1.Location.X + shadowOffset,
                    pictureBox1.Location.Y + shadowOffset
                );

                e.Graphics.DrawImage(shadowImage, shadowLocation);
            }
            else if (pictureBox1.Image == null)
            {
                int shadowOffset = 4;
                Color shadowColor = Color.FromArgb(100, 0, 0, 0);

                Bitmap shadowImage = new Bitmap(pictureBox1.Width, pictureBox1.Height, PixelFormat.Format32bppArgb);
                using (Graphics gShadow = Graphics.FromImage(shadowImage))
                {
                    gShadow.FillRectangle(new SolidBrush(shadowColor), 0, 0, shadowImage.Width, shadowImage.Height);
                }

                Point shadowLocation = new Point(
                    pictureBox1.Location.X + shadowOffset,
                    pictureBox1.Location.Y + shadowOffset
                );

                e.Graphics.DrawImage(shadowImage, shadowLocation);
            }
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            if (pictureBox1.Visible == false)
            {

            }
            else if (pictureBox1.Visible == true)
            {
                picBoxShadow(sender, e);
            }
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

            foreach (Control control in panel1.Controls)
            {
                if (control is System.Windows.Forms.Button button)
                {
                    button.BackColor = Color.FromArgb(45, 45, 45);
                    button.ForeColor = Color.FromArgb(255, 255, 255);
                    panel1.BackColor = Color.FromArgb(55, 55, 55);
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
                DarkNet.Instance.SetWindowThemeForms(this, Theme.Auto);
                this.BackColor = Color.FromArgb(33, 33, 33);
                this.ForeColor = Color.FromArgb(255, 255, 255);
                cmbDrives.BackColor = Color.FromArgb(33, 33, 33);
                api.changeListBoxColour(listBox1, 33, 33, 33, 255, 255, 255);
                changeButtonsStyle();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (Directory.Exists(Application.StartupPath + "\\songs"))
            {
                button3.Text = "Refresh List";
                cmbDrives.Visible = false;
                label1.Visible = true;
                label2.Visible = true;
                label3.Visible = true;
                label4.Visible = false;
                label5.Visible = false;
                label6.Visible = true;
                label7.ForeColor = Color.FromArgb(0, 0, 0);
                label8.Visible = true;
                pictureBox1.Visible = true;
                progressBar2.Visible = true;
                panel1.Visible = true;
                FillListBoxWAV();
                listBox1.SelectedIndex = 0;
                string filePath = Application.StartupPath + "\\songs\\" + listBox1.SelectedItem.ToString();
                audioFileReader = new AudioFileReader(filePath);
                string songMinutes = audioFileReader.TotalTime.Minutes.ToString();
                string songSeconds = audioFileReader.TotalTime.Seconds.ToString();
                label8.Text = songMinutes + ":" + songSeconds;
                checkSettings();

                if (listBox1.Items.Count > 0)
                {
                    label9.Text = listBox1.Items[listBox1.Items.Count - 1].ToString();
                }

                label10.Text = listBox1.SelectedItem.ToString();
            }
            else if (!Directory.Exists(Application.StartupPath + "\\songs"))
            {
                checkSettings();
                button3.Text = "Start CD";
                label5.Text = "";
                checkForCD.Start();

                String[] Drives = Directory.GetLogicalDrives();
                foreach (String Drive in Drives)
                {
                    DriveInfo di = new DriveInfo(Drive);
                    if (di.DriveType == DriveType.CDRom)
                        cmbDrives.Items.Add(Drive);
                }

                if (cmbDrives.Items.Count > 0)
                    cmbDrives.SelectedIndex = 0;
            }
        }

        #region CD Ripping stuff
        private void button3_Click(object sender, EventArgs e)
        {
            if (button3.Text == "Start CD")
            {
                string[] logDrives = System.IO.Directory.GetLogicalDrives();

                //string s = "";
                StringBuilder volumeName = new StringBuilder(256);
                int srNum = new int();
                int comLen = new int();
                string sysName = "";
                int sysFlags = new int();
                int result;

                for (int i = 0; i < logDrives.Length; i++)
                {
                    if (api.GetDriveType(logDrives[i]) == 5)
                    {
                        //s += "Your CD ROM is on drive : " +
                                  //logDrives[i].ToString() + "\n";
                        result = api.GetVolumeInformation(logDrives[i].ToString(),
                                volumeName, 256, srNum, comLen, sysFlags, sysName, 256);
                        if (result == 0)
                        {
                            
                        }
                        else
                        {
                            if (MessageBox.Show("The program may take a while as to make the Audio CD playable, it must be converted from .CDA to .MP3\n\nAfterwards, the .MP3 files will be in the List Box and you'll be able to listen to your music.", formname.Text, MessageBoxButtons.OK, MessageBoxIcon.Information) == DialogResult.OK)
                            {
                                checkForCD.Stop();
                                startCDAtoWAV();
                            }
                        }
                    }
                }
            }
            else if (button3.Text == "Refresh List")
            {
                FillListBoxWAV();
            }
        }

        public void startCDAtoWAV()
        {
            button3.Enabled = false;
            readcd();
            this.Text = formname.Text + ": CD Rip complete";
            MessageBox.Show("Done.", this.Text + ": CD Rip complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
            label4.Text = "Done";
        }

        void readcd()
        {
            bool TocValid = false;
            IntPtr cdHandle = IntPtr.Zero;
            CDROM_TOC Toc = null;
            int track, StartSector, EndSector;
            BinaryWriter bw;
            bool CDReady;
            uint uiTrackCount, uiTrackSize, uiDataSize;
            int i;
            uint BytesRead, Dummy;
            char Drive = (char)cmbDrives.Text[0];
            TRACK_DATA td;
            int sector;
            byte[] SectorData;
            IntPtr pnt;
            Int64 Offset;
            Dummy = 0;
            BytesRead = 0;
            CDReady = false;
            Toc = new CDROM_TOC();
            IntPtr ip = Marshal.AllocHGlobal((IntPtr)(Marshal.SizeOf(Toc)));
            Marshal.StructureToPtr(Toc, ip, false);

            DriveTypes dt = GetDriveType(Drive + ":\\");

            Directory.CreateDirectory(Application.StartupPath + "\\temporary-files");

            if (dt == DriveTypes.DRIVE_CDROM)
            {
                cdHandle = CreateFile("\\\\.\\" + Drive + ':', GENERIC_READ, FILE_SHARE_READ, IntPtr.Zero, OPEN_EXISTING, 0, IntPtr.Zero);
                CDReady = DeviceIoControl(cdHandle, IOCTL_STORAGE_CHECK_VERIFY, IntPtr.Zero, 0, IntPtr.Zero, 0, ref Dummy, IntPtr.Zero) == 1;

                if (!CDReady)
                {
                    MessageBox.Show("Drive Not Ready", "Drive Not Ready", MessageBoxButtons.OK);
                }
                else
                {
                    uiTrackCount = 0;

                    TocValid = DeviceIoControl(cdHandle, IOCTL_CDROM_READ_TOC, IntPtr.Zero, 0, ip, (uint)Marshal.SizeOf(Toc), ref BytesRead, IntPtr.Zero) != 0;

                    Marshal.PtrToStructure(ip, Toc);

                    if (!TocValid)
                    {
                        MessageBox.Show("Invalid Table of Content ", "Invalid Table of Content ", MessageBoxButtons.OK);
                    }
                    else
                    {
                        uiTrackCount = Toc.LastTrack;
                        
                        TrackData = new byte[uiTrackCount][];

                        if (!System.IO.File.Exists(Application.StartupPath + "\\temporary-files\\number_of_tracks.tmp"))
                        {
                            System.IO.File.Create(Application.StartupPath + "\\temporary-files\\number_of_tracks.tmp").Close();

                            StreamWriter sw = new StreamWriter(Application.StartupPath + "\\temporary-files\\number_of_tracks.tmp");
                            sw.WriteLine(uiTrackCount - 1);
                            sw.Close();
                        }
                        else
                        {

                        }

                        for (track = 1; track <= uiTrackCount; track++)
                        {
                            Offset = 0;
                            label4.Visible = true;
                            progressBar1.Visible = true;
                            label4.Text = "Reading Track " + track.ToString() + " of " + uiTrackCount.ToString();
                            this.Text = formname.Text + ": Reading Track " + track.ToString() + " of " + uiTrackCount.ToString();
                            Application.DoEvents();

                            #region checking if track number is below 10 or at/above
                            if (track < 10)
                            {
                                bw = new BinaryWriter(System.IO.File.Open(Application.StartupPath + "\\temporary-files\\Track0" + track.ToString() + ".tmp", FileMode.Create));

                                td = Toc.TrackData[track - 1];

                                StartSector = (td.Address_1 * 60 * 75 + td.Address_2 * 75 + td.Address_3) - 150;
                                td = Toc.TrackData[track];
                                EndSector = (td.Address_1 * 60 * 75 + td.Address_2 * 75 + td.Address_3) - 151;
                                progressBar1.Minimum = StartSector;
                                progressBar1.Maximum = EndSector;
                                uiTrackSize = (uint)(EndSector - StartSector) * CB_AUDIO;

                                uiDataSize = (uint)uiTrackSize;

                                TrackData[track - 1] = new byte[uiDataSize];
                                SectorData = new byte[CB_AUDIO * NSECTORS];

                                for (sector = StartSector; (sector < EndSector); sector += NSECTORS)
                                {
                                    Debug.Print(sector.ToString("X2"));
                                    RAW_READ_INFO rri = new RAW_READ_INFO();
                                    rri.TrackMode = TRACK_MODE_TYPE.CDDA;
                                    rri.SectorCount = (uint)1;
                                    rri.DiskOffset = sector * CB_CDROMSECTOR;

                                    Marshal.StructureToPtr(rri, ip, false);

                                    int size = Marshal.SizeOf(SectorData[0]) * SectorData.Length;
                                    pnt = Marshal.AllocHGlobal(size);

                                    SectorData.Initialize();

                                    i = DeviceIoControl(cdHandle, IOCTL_CDROM_RAW_READ, ip, (uint)Marshal.SizeOf(rri), pnt, (uint)NSECTORS * CB_AUDIO, ref BytesRead, IntPtr.Zero);

                                    //if (i == 0)
                                    //{
                                    //MessageBox.Show("Bad Sector Read", "Bad Sector Read from sector " + sector.ToString("X2"), MessageBoxButtons.OK);
                                    //break;
                                    //}

                                    progressBar1.Value = sector;

                                    Marshal.PtrToStructure(ip, rri);
                                    Marshal.Copy(pnt, SectorData, 0, SectorData.Length);
                                    Marshal.FreeHGlobal(pnt);
                                    Array.Copy(SectorData, 0, TrackData[track - 1], Offset, BytesRead);
                                    Offset += BytesRead;
                                }

                                bw.Write(TrackData[track - 1]);
                                bw.Close();
                            }
                            else if (track == 10 || track > 10)
                            {
                                bw = new BinaryWriter(System.IO.File.Open(Application.StartupPath + "\\temporary-files\\Track" + track.ToString() + ".tmp", FileMode.Create));

                                td = Toc.TrackData[track - 1];

                                StartSector = (td.Address_1 * 60 * 75 + td.Address_2 * 75 + td.Address_3) - 150;
                                td = Toc.TrackData[track];
                                EndSector = (td.Address_1 * 60 * 75 + td.Address_2 * 75 + td.Address_3) - 151;
                                progressBar1.Minimum = StartSector;
                                progressBar1.Maximum = EndSector;
                                uiTrackSize = (uint)(EndSector - StartSector) * CB_AUDIO;

                                uiDataSize = (uint)uiTrackSize;

                                TrackData[track - 1] = new byte[uiDataSize];
                                SectorData = new byte[CB_AUDIO * NSECTORS];

                                for (sector = StartSector; (sector < EndSector); sector += NSECTORS)
                                {
                                    Debug.Print(sector.ToString("X2"));
                                    RAW_READ_INFO rri = new RAW_READ_INFO();
                                    rri.TrackMode = TRACK_MODE_TYPE.CDDA;
                                    rri.SectorCount = (uint)1;
                                    rri.DiskOffset = sector * CB_CDROMSECTOR;

                                    Marshal.StructureToPtr(rri, ip, false);

                                    int size = Marshal.SizeOf(SectorData[0]) * SectorData.Length;
                                    pnt = Marshal.AllocHGlobal(size);

                                    SectorData.Initialize();

                                    i = DeviceIoControl(cdHandle, IOCTL_CDROM_RAW_READ, ip, (uint)Marshal.SizeOf(rri), pnt, (uint)NSECTORS * CB_AUDIO, ref BytesRead, IntPtr.Zero);

                                    //if (i == 0)
                                    //{
                                    //MessageBox.Show("Bad Sector Read", "Bad Sector Read from sector " + sector.ToString("X2"), MessageBoxButtons.OK);
                                    //break;
                                    //}

                                    progressBar1.Value = sector;

                                    Marshal.PtrToStructure(ip, rri);
                                    Marshal.Copy(pnt, SectorData, 0, SectorData.Length);
                                    Marshal.FreeHGlobal(pnt);
                                    Array.Copy(SectorData, 0, TrackData[track - 1], Offset, BytesRead);
                                    Offset += BytesRead;
                                }

                                bw.Write(TrackData[track - 1]);
                                bw.Close();
                            }
                            #endregion
                        }

                        PREVENT_MEDIA_REMOVAL pmr = new PREVENT_MEDIA_REMOVAL();
                        pmr.PreventMediaRemoval = 0;
                        ip = Marshal.AllocHGlobal((IntPtr)(Marshal.SizeOf(pmr)));
                        Marshal.StructureToPtr(pmr, ip, false);
                        DeviceIoControl(cdHandle, IOCTL_STORAGE_MEDIA_REMOVAL, ip, (uint)Marshal.SizeOf(pmr), IntPtr.Zero, 0, ref Dummy, IntPtr.Zero);
                        Marshal.PtrToStructure(ip, pmr);
                        Marshal.FreeHGlobal(ip);
                    }
                }
            }

            CloseHandle(cdHandle);

            if (!Directory.Exists(Application.StartupPath + "\\songs"))
            {
                Directory.CreateDirectory(Application.StartupPath + "\\songs");
            }

            ConvertToWav();
        }
        
        private void ConvertToWav()
        {
            int i, track, tracks;
            byte[] b;
            char[] riffchunk = { 'R', 'I', 'F', 'F' };
            char[] wavechunk = { 'W', 'A', 'V', 'E' };
            char[] datachunk = { 'd', 'a', 't', 'a' };
            char[] fmtchunk = { 'f', 'm', 't', ' ' };
            Int32 riffsize, datasize, fmtsize, extrabits;
            Int32 DI, SampleRate, ByteRate;
            uint BytesWritten;
            Int16 BlockAlign, Format, NumChannels, BitsPerSample;
            Byte[] Image;
            IntPtr FileHandle;
            Format = 1;
            NumChannels = 2;
            SampleRate = 44100;
            BitsPerSample = 16;
            ByteRate = SampleRate * NumChannels * BitsPerSample / 8;
            BlockAlign = 4;
            fmtsize = 0x12;

            tracks = TrackData.GetUpperBound(0);

            progressBar1.Maximum = tracks;
            progressBar1.Minimum = 0;

            for (track = 0; track <= tracks; track++)
            {
                DI = 0;
                progressBar1.Value = track;
                label4.Text = "Writing Track " + (track + 1).ToString() + ".wav";
                this.Text = formname.Text + ": Writing Track " + (track + 1).ToString() + ".wav";
                Application.DoEvents();
                datasize = TrackData[track].Length;

                #region checking if track number is below 10 or at/above
                if (track < 10)
                {
                    FileHandle = CreateFile(Application.StartupPath + "\\temporary-files\\Track0" + (track + 1).ToString() + ".wav", GENERIC_WRITE, 0, IntPtr.Zero, OPEN_ALWAYS, FILE_ATTRIBUTE_NORMAL, IntPtr.Zero);

                    riffsize = datasize;
                    riffsize += 4;
                    riffsize += 4;
                    riffsize += 4;
                    riffsize += fmtsize;
                    riffsize += 4;
                    riffsize += 4;
                    extrabits = 0;

                    Image = new Byte[riffsize + 8];
                    b = Encoding.ASCII.GetBytes(riffchunk);
                    Array.Copy(b, 0, Image, DI, 4);
                    DI += 4;
                    b = BitConverter.GetBytes(riffsize);

                    if (!BitConverter.IsLittleEndian)
                        Array.Reverse(b);

                    Array.Copy(b, 0, Image, DI, 4);
                    DI += 4;
                    b = Encoding.ASCII.GetBytes(wavechunk);
                    Array.Copy(b, 0, Image, DI, 4);
                    DI += 4;
                    b = Encoding.ASCII.GetBytes(fmtchunk);

                    if (!BitConverter.IsLittleEndian)
                        Array.Reverse(b);

                    Array.Copy(b, 0, Image, DI, 4);
                    DI += 4;
                    b = BitConverter.GetBytes(fmtsize);

                    if (!BitConverter.IsLittleEndian)
                        Array.Reverse(b);

                    Array.Copy(b, 0, Image, DI, 4);
                    DI += 4;
                    b = BitConverter.GetBytes(Format);

                    if (!BitConverter.IsLittleEndian)
                        Array.Reverse(b);

                    Array.Copy(b, 0, Image, DI, 2);
                    DI += 2;
                    b = BitConverter.GetBytes(NumChannels);

                    if (!BitConverter.IsLittleEndian)
                        Array.Reverse(b);

                    Array.Copy(b, 0, Image, DI, 2);
                    DI += 2;
                    b = BitConverter.GetBytes(SampleRate);

                    if (!BitConverter.IsLittleEndian)
                        Array.Reverse(b);

                    Array.Copy(b, 0, Image, DI, 4);
                    DI += 4;
                    b = BitConverter.GetBytes(ByteRate);

                    if (!BitConverter.IsLittleEndian)
                        Array.Reverse(b);

                    Array.Copy(b, 0, Image, DI, 4);
                    DI += 4;
                    b = BitConverter.GetBytes(BlockAlign);

                    if (!BitConverter.IsLittleEndian)
                        Array.Reverse(b);

                    Array.Copy(b, 0, Image, DI, 2);
                    DI += 2;
                    b = BitConverter.GetBytes(BitsPerSample);

                    if (!BitConverter.IsLittleEndian)
                        Array.Reverse(b);

                    Array.Copy(b, 0, Image, DI, 2);
                    DI += 2;
                    b = BitConverter.GetBytes(extrabits);

                    if (!BitConverter.IsLittleEndian)
                        Array.Reverse(b);

                    Array.Copy(b, 0, Image, DI, 2);
                    DI += 2;
                    b = Encoding.ASCII.GetBytes(datachunk);

                    Array.Copy(b, 0, Image, DI, 4);
                    DI += 4;
                    b = BitConverter.GetBytes(datasize);

                    if (!BitConverter.IsLittleEndian)
                        Array.Reverse(b);

                    Array.Copy(b, 0, Image, DI, 4);
                    DI += 4;

                    Array.Copy(TrackData[track], 0, Image, DI, TrackData[track].Length);

                    i = WriteFile(FileHandle, Image, (uint)Image.Length, out BytesWritten, IntPtr.Zero);

                    if (i != 0)
                    {
                        i = FlushFileBuffers(FileHandle);

                        i = CloseHandle(FileHandle);
                    }

                    Image = null;
                    progressBar1.Value = track;
                }
                else if (track == 10 || track > 10)
                {
                    FileHandle = CreateFile(Application.StartupPath + "\\temporary-files\\Track" + (track + 1).ToString() + ".wav", GENERIC_WRITE, 0, IntPtr.Zero, OPEN_ALWAYS, FILE_ATTRIBUTE_NORMAL, IntPtr.Zero);

                    riffsize = datasize;
                    riffsize += 4;
                    riffsize += 4;
                    riffsize += 4;
                    riffsize += fmtsize;
                    riffsize += 4;
                    riffsize += 4;
                    extrabits = 0;

                    Image = new Byte[riffsize + 8];
                    b = Encoding.ASCII.GetBytes(riffchunk);
                    Array.Copy(b, 0, Image, DI, 4);
                    DI += 4;
                    b = BitConverter.GetBytes(riffsize);

                    if (!BitConverter.IsLittleEndian)
                        Array.Reverse(b);

                    Array.Copy(b, 0, Image, DI, 4);
                    DI += 4;
                    b = Encoding.ASCII.GetBytes(wavechunk);
                    Array.Copy(b, 0, Image, DI, 4);
                    DI += 4;
                    b = Encoding.ASCII.GetBytes(fmtchunk);

                    if (!BitConverter.IsLittleEndian)
                        Array.Reverse(b);

                    Array.Copy(b, 0, Image, DI, 4);
                    DI += 4;
                    b = BitConverter.GetBytes(fmtsize);

                    if (!BitConverter.IsLittleEndian)
                        Array.Reverse(b);

                    Array.Copy(b, 0, Image, DI, 4);
                    DI += 4;
                    b = BitConverter.GetBytes(Format);

                    if (!BitConverter.IsLittleEndian)
                        Array.Reverse(b);

                    Array.Copy(b, 0, Image, DI, 2);
                    DI += 2;
                    b = BitConverter.GetBytes(NumChannels);

                    if (!BitConverter.IsLittleEndian)
                        Array.Reverse(b);

                    Array.Copy(b, 0, Image, DI, 2);
                    DI += 2;
                    b = BitConverter.GetBytes(SampleRate);

                    if (!BitConverter.IsLittleEndian)
                        Array.Reverse(b);

                    Array.Copy(b, 0, Image, DI, 4);
                    DI += 4;
                    b = BitConverter.GetBytes(ByteRate);

                    if (!BitConverter.IsLittleEndian)
                        Array.Reverse(b);

                    Array.Copy(b, 0, Image, DI, 4);
                    DI += 4;
                    b = BitConverter.GetBytes(BlockAlign);

                    if (!BitConverter.IsLittleEndian)
                        Array.Reverse(b);

                    Array.Copy(b, 0, Image, DI, 2);
                    DI += 2;
                    b = BitConverter.GetBytes(BitsPerSample);

                    if (!BitConverter.IsLittleEndian)
                        Array.Reverse(b);

                    Array.Copy(b, 0, Image, DI, 2);
                    DI += 2;
                    b = BitConverter.GetBytes(extrabits);

                    if (!BitConverter.IsLittleEndian)
                        Array.Reverse(b);

                    Array.Copy(b, 0, Image, DI, 2);
                    DI += 2;
                    b = Encoding.ASCII.GetBytes(datachunk);

                    Array.Copy(b, 0, Image, DI, 4);
                    DI += 4;
                    b = BitConverter.GetBytes(datasize);

                    if (!BitConverter.IsLittleEndian)
                        Array.Reverse(b);

                    Array.Copy(b, 0, Image, DI, 4);
                    DI += 4;

                    Array.Copy(TrackData[track], 0, Image, DI, TrackData[track].Length);

                    i = WriteFile(FileHandle, Image, (uint)Image.Length, out BytesWritten, IntPtr.Zero);

                    if (i != 0)
                    {
                        i = FlushFileBuffers(FileHandle);

                        i = CloseHandle(FileHandle);
                    }

                    Image = null;
                    progressBar1.Value = track;
                }
                #endregion
            }

            ConvertWAVtoMP3();
        }

        public void ConvertWAVtoMP3()
        {
            int track, tracks;

            string wavlocation = Application.StartupPath + "\\temporary-files";
            string mp3location = Application.StartupPath + "\\songs";

            tracks = TrackData.GetUpperBound(0);

            progressBar1.Maximum = tracks;
            progressBar1.Minimum = 0;

            for (track = 0; track <= tracks; track++)
            {
                #region checking if track number is below 10 or at/above
                if (track < 10)
                {
                    progressBar1.Value = track;
                    label4.Text = "Converting Track0" + (track + 1).ToString() + ".wav to Track0" + (track + 1).ToString() + ".mp3";
                    this.Text = formname.Text + ": Converting Track0" + (track + 1).ToString() + ".wav to Track0" + (track + 1).ToString() + ".mp3";
                    ifCDAtoMP3isDone.Start();
                    Application.DoEvents();
                    api.ConvertWavToMp3(wavlocation + "\\Track0" + (track + 1).ToString() + ".wav", mp3location + "\\Track0" + (track + 1).ToString() + ".mp3");
                    progressBar1.Value = track;
                }
                else if (track == 10 || track > 10)
                {
                    progressBar1.Value = track;
                    label4.Text = "Converting Track" + (track + 1).ToString() + ".wav to Track" + (track + 1).ToString() + ".mp3";
                    this.Text = formname.Text + ": Converting Track" + (track + 1).ToString() + ".wav to Track" + (track + 1).ToString() + ".mp3";
                    ifCDAtoMP3isDone.Start();
                    Application.DoEvents();
                    api.ConvertWavToMp3(wavlocation + "\\Track" + (track + 1).ToString() + ".wav", mp3location + "\\Track" + (track + 1).ToString() + ".mp3");
                    progressBar1.Value = track;
                }
                #endregion
            }

            label4.Text = "Getting rid of temporary files.";
            System.IO.File.Copy(mp3location + "\\Track010.mp3", mp3location + "\\Track10.mp3");
            System.IO.File.Delete(mp3location + "\\Track010.mp3");
        }

        public void DeleteTemporaryFiles()
        {
            if (Directory.Exists(Application.StartupPath + "\\temporary-files"))
            {
                Directory.Delete(Application.StartupPath + "\\temporary-files", true);
            }
        }

        private void ifCDAtoMP3isDone_Tick(object sender, EventArgs e)
        {
            if (label4.Text == "Done")
            {
                System.IO.File.Move(Application.StartupPath + "\\temporary-files\\number_of_tracks.tmp", Application.StartupPath + "\\songs\\number_of_tracks.txt");
                api.mciSendString("set CDAudio door open", null, 127, 0);
                this.Text = formname.Text + ": drive opened";
                DeleteTemporaryFiles();
                label1.Visible = true;
                label2.Visible = true;
                label3.Visible = true;
                panel1.Visible = true;
                label4.Visible = false;
                label5.Visible = false;
                label6.Visible = true;
                pictureBox1.Visible = true;
                label7.ForeColor = Color.FromArgb(0, 0, 0);
                label7.Visible = true;
                label8.Visible = true;
                button3.Text = "Refresh List"; 
                button3.Enabled = true;
                progressBar1.Visible = false;
                progressBar2.Visible = true;
                cmbDrives.Visible = false;
                FillListBoxWAV();
                listBox1.SelectedIndex = 0;
                string filePath = Application.StartupPath + "\\songs\\" + listBox1.SelectedItem.ToString();
                audioFileReader = new AudioFileReader(filePath);
                string songMinutes = audioFileReader.TotalTime.Minutes.ToString();
                string songSeconds = audioFileReader.TotalTime.Seconds.ToString();
                label8.Text = songMinutes + ":" + songSeconds;
                api.changeListBoxColour(listBox1, 33, 33, 33, 255, 255, 255);
                changeButtonsStyle();
                checkSettings();
                ifCDAtoMP3isDone.Stop();
            }
            else
            {

            }
        }
        #endregion

        private void button1_Click(object sender, EventArgs e)
        {
            api.mciSendString("set CDAudio door open", null, 127, 0);
            this.Text = formname.Text + ": drive opened";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            api.mciSendString("set CDAudio door closed", null, 127, 0);
            this.Text = formname.Text;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                string filePath = Application.StartupPath + "\\songs\\" + listBox1.SelectedItem.ToString();

                try
                {
                    using (TagLib.File file = TagLib.File.Create(filePath))
                    {
                        #region metadata check
                        if (file.Tag.Title == null)
                        {
                            label1.Text = listBox1.SelectedItem.ToString();
                        }
                        else
                        {
                            label1.Text = file.Tag.Title;
                        }

                        if (file.Tag.FirstPerformer == null)
                        {
                            label2.Text = "Unknown Artist";
                        }
                        else
                        {
                            label2.Text = file.Tag.FirstPerformer;
                        }

                        if (file.Tag.Album == null)
                        {
                            label3.Text = "Unknown Album";
                        }
                        else
                        {
                            label3.Text = file.Tag.Album;
                        }

                        if (file.Tag.Pictures.Length > 0)
                        {
                            Console.WriteLine($"Contains Album Art: Yes");
                            pictureBox1.Image = api.ByteArrayToImage(file.Tag.Pictures[0].Data.Data);
                            label7.Visible = false;
                        }
                        else
                        {
                            Console.WriteLine($"Contains Album Art: No");
                            pictureBox1.Image = null;
                            label7.Visible = true;
                        }
                        #endregion

                        //Console.WriteLine($"Title: {file.Tag.Title}");
                        //Console.WriteLine($"Artist: {file.Tag.FirstPerformer}");
                        //Console.WriteLine($"Album: {file.Tag.Album}");
                        //Console.WriteLine($"Year: {file.Tag.Year}");
                        //Console.WriteLine($"Genre: {file.Tag.FirstGenre}");
                        //Console.WriteLine($"Duration: {file.Properties.Duration}");
                        //Console.WriteLine($"Channels: {file.Properties.AudioChannels}");
                        //Console.WriteLine($"Sample Rate: {file.Properties.AudioSampleRate} Hz");
                        //Console.WriteLine($"Bits Per Sample: {file.Properties.BitsPerSample}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error reading WAV metadata: {ex.Message}");
                }
            }
        }

        WMPLib.WindowsMediaPlayer wplayer = new WMPLib.WindowsMediaPlayer();
        string title = "";
        string artist = "";
        string album = "";

        public void metadataCheckOnPlay()
        {
            string filePath = Application.StartupPath + "\\songs\\" + listBox1.SelectedItem.ToString();

            try
            {
                using (TagLib.File file = TagLib.File.Create(filePath))
                {
                    if (file.Tag.Title == null)
                    {
                        label1.Text = listBox1.SelectedItem.ToString();
                        title = listBox1.SelectedItem.ToString();
                    }
                    else
                    {
                        label1.Text = file.Tag.Title;
                        title = file.Tag.Title;
                    }

                    if (file.Tag.FirstPerformer == null)
                    {
                        label2.Text = "Unknown Artist";
                        artist = "Unknown Artist";
                    }
                    else
                    {
                        label2.Text = file.Tag.FirstPerformer;
                        artist = file.Tag.FirstPerformer;
                    }

                    if (file.Tag.Album == null)
                    {
                        label3.Text = "Unknown Album";
                        album = "Unknown Album";
                    }
                    else
                    {
                        label3.Text = file.Tag.Album;
                        album = file.Tag.Album;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading MP3 metadata: {ex.Message}");
            }
        }

        private void progressBarSongUpdate_Tick(object sender, EventArgs e)
        {
            if (wplayer.playState == WMPLib.WMPPlayState.wmppsPlaying ||
                    wplayer.playState == WMPLib.WMPPlayState.wmppsPaused)
            {
                if (wplayer.currentMedia != null && wplayer.currentMedia.duration > 0)
                {
                    double currentPosition = wplayer.controls.currentPosition;
                    double duration = wplayer.currentMedia.duration;
                    int progressPercentage = (int)((currentPosition / duration) * 100);
                    progressBar2.Value = progressPercentage;
                }
            }
            else
            {
                progressBar2.Value = 0;
                progressBarSongUpdate.Stop();
            }
        }

        private void updateSongTime_Tick(object sender, EventArgs e)
        {
            string filePath = Application.StartupPath + "\\songs\\" + listBox1.SelectedItem.ToString();
            audioFileReader = new AudioFileReader(filePath);
            string songMinutes = audioFileReader.TotalTime.Minutes.ToString();
            string songSeconds = audioFileReader.TotalTime.Seconds.ToString();

            songSecondsPB.Value = songSecondsPB.Value + 1;

            if (songSecondsPB.Value < 10)
            {
                label6.Text = songMinutesPB.Value.ToString() + ":0" + songSecondsPB.Value.ToString();
            }
            else if (songSecondsPB.Value == 10 | songSecondsPB.Value > 10)
            {
                label6.Text = songMinutesPB.Value.ToString() + ":" + songSecondsPB.Value.ToString();
            }

            if (songSecondsPB.Value == 60)
            {
                songMinutesPB.Value = songMinutesPB.Value + 1;
                songSecondsPB.Value = 0;
                label6.Text = songMinutesPB.Value.ToString() + ":0" + songSecondsPB.Value.ToString();
            }

            if (songMinutesPB.Value == Convert.ToInt32(songMinutes) && songSecondsPB.Value == Convert.ToInt32(songSeconds))
            {
                label6.Text = "0:00";
                updateSongTime.Stop();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string filePath = Application.StartupPath + "\\songs\\" + listBox1.SelectedItem.ToString();

            metadataCheckOnPlay();

            this.Text = formname.Text + ": Playing " + title + " by " + artist + " on " + album;
            wplayer.URL = filePath;
            wplayer.controls.play();
            progressBarSongUpdate.Start();
            updateSongTime.Start();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Text = formname.Text + ": Stopped";
            wplayer.controls.stop();
            updateSongTime.Stop();
            label6.Text = "0:00";
            songMinutesPB.Value = 0;
            songSecondsPB.Value = 0;
        }

        private void button6_Click_1(object sender, EventArgs e)
        {
            about a = new about();
            a.Show();
        }

        string songMinutes;
        string songSeconds;
        int songTotalSeconds;
        private void button7_Click(object sender, EventArgs e)
        {
            string nextSong;

            if (listBox1.SelectedItem.ToString() == label9.Text)
            {
                nextSong = listBox1.SelectedIndex.ToString();
            }
            else
            {
                nextSong = listBox1.SelectedIndex++.ToString();
            }

            string filePath = Application.StartupPath + "\\songs\\" + nextSong;
            string ns2 = listBox1.SelectedItem.ToString();
            string actualFilePath = Application.StartupPath + "\\songs\\" + ns2;

            StreamReader sr = new StreamReader(Application.StartupPath + "\\songs\\number_of_tracks.txt");
            string lastTrack = sr.ReadLine();
            sr.Close();

            if (Convert.ToInt32(nextSong) == Convert.ToInt32(lastTrack) - 1)
            {
                MessageBox.Show("Cannot go any further, you are at the end of the playlist.", formname.Text + ": Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                try
                {
                    using (TagLib.File file = TagLib.File.Create(actualFilePath))
                    {
                        if (file.Tag.Title == null)
                        {
                            label1.Text = listBox1.SelectedItem.ToString();
                            title = listBox1.SelectedItem.ToString();
                        }
                        else
                        {
                            label1.Text = file.Tag.Title;
                            title = file.Tag.Title;
                        }

                        if (file.Tag.FirstPerformer == null)
                        {
                            label2.Text = "Unknown Artist";
                            artist = "Unknown Artist";
                        }
                        else
                        {
                            label2.Text = file.Tag.FirstPerformer;
                            artist = file.Tag.FirstPerformer;
                        }

                        if (file.Tag.Album == null)
                        {
                            label3.Text = "Unknown Album";
                            album = "Unknown Album";
                        }
                        else
                        {
                            label3.Text = file.Tag.Album;
                            album = file.Tag.Album;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error reading MP3 metadata: {ex.Message}");
                }

                progressBarSongUpdate.Stop();
                updateSongTime.Stop();
                audioFileReader = new AudioFileReader(actualFilePath);
                songMinutes = audioFileReader.TotalTime.Minutes.ToString();
                songSeconds = audioFileReader.TotalTime.Seconds.ToString();
                songTotalSeconds = Convert.ToInt32(audioFileReader.TotalTime.TotalSeconds);
                this.Text = formname.Text + ": Playing " + title + " by " + artist + " on " + album;
                wplayer.URL = actualFilePath;
                wplayer.controls.next();
                label6.Text = "0:00";

                if (Convert.ToInt32(songSeconds) < 10)
                {
                    label8.Text = songMinutes + ":0" + songSeconds;
                }
                else if (Convert.ToInt32(songSeconds) == 10 | Convert.ToInt32(songSeconds) > 10)
                {
                    label8.Text = songMinutes + ":" + songSeconds;
                }

                songMinutesPB.Value = 0;
                songSecondsPB.Value = 0;
                progressBarSongUpdate.Start();
                updateSongTime.Start();
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            string previousSong;

            if (listBox1.SelectedItem.ToString() == label10.Text)
            {
                previousSong = listBox1.SelectedIndex.ToString();
            }
            else
            {
                previousSong = listBox1.SelectedIndex--.ToString();
            }
            
            string filePath = Application.StartupPath + "\\songs\\" + previousSong;
            string prs2 = listBox1.SelectedItem.ToString();
            string actualFilePath = Application.StartupPath + "\\songs\\" + prs2;

            StreamReader sr = new StreamReader(Application.StartupPath + "\\songs\\number_of_tracks.txt");
            string lastTrack = sr.ReadLine();
            sr.Close();

            if (Convert.ToInt32(previousSong) == Convert.ToInt32(lastTrack) - 19)
            {
                MessageBox.Show("Cannot go back, you are at the start of the playlist.", formname.Text + ": Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                try
                {
                    using (TagLib.File file = TagLib.File.Create(actualFilePath))
                    {
                        if (file.Tag.Title == null)
                        {
                            label1.Text = listBox1.SelectedItem.ToString();
                            title = listBox1.SelectedItem.ToString();
                        }
                        else
                        {
                            label1.Text = file.Tag.Title;
                            title = file.Tag.Title;
                        }

                        if (file.Tag.FirstPerformer == null)
                        {
                            label2.Text = "Unknown Artist";
                            artist = "Unknown Artist";
                        }
                        else
                        {
                            label2.Text = file.Tag.FirstPerformer;
                            artist = file.Tag.FirstPerformer;
                        }

                        if (file.Tag.Album == null)
                        {
                            label3.Text = "Unknown Album";
                            album = "Unknown Album";
                        }
                        else
                        {
                            label3.Text = file.Tag.Album;
                            album = file.Tag.Album;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error reading MP3 metadata: {ex.Message}");
                }

                progressBarSongUpdate.Stop();
                updateSongTime.Stop();
                audioFileReader = new AudioFileReader(actualFilePath);
                songMinutes = audioFileReader.TotalTime.Minutes.ToString();
                songSeconds = audioFileReader.TotalTime.Seconds.ToString();
                songTotalSeconds = Convert.ToInt32(audioFileReader.TotalTime.TotalSeconds);
                this.Text = formname.Text + ": Playing " + title + " by " + artist + " on " + album;
                wplayer.URL = actualFilePath;
                wplayer.controls.previous();
                label6.Text = "0:00";

                if (Convert.ToInt32(songSeconds) < 10)
                {
                    label8.Text = songMinutes + ":0" + songSeconds;
                }
                else if (Convert.ToInt32(songSeconds) == 10 | Convert.ToInt32(songSeconds) > 10)
                {
                    label8.Text = songMinutes + ":" + songSeconds;
                }

                songMinutesPB.Value = 0;
                songSecondsPB.Value = 0;
                progressBarSongUpdate.Start();
                updateSongTime.Start();
            }
        }
    }
}
