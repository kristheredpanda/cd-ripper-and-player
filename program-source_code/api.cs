using NAudio.Lame;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CD_Player
{
    public class api
    {
        [DllImport("winmm.dll", EntryPoint = "mciSendStringA")]
        public static extern int mciSendString(string lpstrCommand,
               string lpstrReturnString, int uReturnLength, int hwndCallback);
        
        [DllImport("kernel32.dll", EntryPoint = "GetVolumeInformationA")]
        public static extern int GetVolumeInformation(string lpRootPathName,
               StringBuilder lpVolumeNameBuffer, int nVolumeNameSize,
               int lpVolumeSerialNumber, int lpMaximumComponentLength,
               int lpFileSystemFlags, string lpFileSystemNameBuffer,
               int nFileSystemNameSize);

        [DllImport("kernel32.dll", EntryPoint = "GetDriveTypeA")]
        public static extern int GetDriveType(string nDrive);

        public static void ConvertWavToMp3(string inputWavPath, string outputMp3Path)
        {
            using (var reader = new AudioFileReader(inputWavPath))
            using (var writer = new LameMP3FileWriter(outputMp3Path, reader.WaveFormat, LAMEPreset.ABR_128)) // Adjust LamePreset for desired quality/bitrate
            {
                reader.CopyTo(writer);
            }
        }

        public static Image ByteArrayToImage(byte[] byteArrayIn)
        {
            if (byteArrayIn == null || byteArrayIn.Length == 0)
            {
                return null; // Or throw an exception, depending on your error handling strategy
            }

            using (MemoryStream ms = new MemoryStream(byteArrayIn))
            {
                try
                {
                    Image returnImage = Image.FromStream(ms);
                    return returnImage;
                }
                catch (ArgumentException ex)
                {
                    // Handle cases where the byte array does not represent a valid image format
                    Console.WriteLine($"Error converting byte array to image: {ex.Message}");
                    return null;
                }
            }
        }

        public static void changeListBoxColour(ListBox targetListBox, int backR, int backG, int backB, int foreR, int foreG, int foreB)
        {
            targetListBox.BackColor = Color.FromArgb(backR, backG, backB);
            targetListBox.ForeColor = Color.FromArgb(foreR, foreG, foreB);
        }
    }
}
