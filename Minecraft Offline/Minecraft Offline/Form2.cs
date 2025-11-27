using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Windows.Forms;

namespace Minecraft_Offline
{
    public partial class Form2 : Form
    {
        private WebClient client; // For async download

        public Form2()
        {
            InitializeComponent();
        }

        // DOWNLOAD BUTTON
        private void button1_Click(object sender, EventArgs e)
        {
            string url = "";
            string version = "";

            // Determine selected version
            if (checkBox1.Checked) { url = "https://github.com/Clicker-games-Studio/Minecraft-Offline/releases/download/MC1.2.5/mc1.2.5.zip"; version = "1.2.5"; }
            else if (checkBox2.Checked) { url = "https://github.com/Clicker-games-Studio/Minecraft-Offline/releases/download/MC1.8.7/MC1.8.7.zip"; version = "1.8.7"; }
            else if (checkBox3.Checked) { url = "https://github.com/Clicker-games-Studio/Minecraft-Offline/releases/download/MC1.8.8/MC1.8.8.zip"; version = "1.8.8"; }
            else if (checkBox4.Checked) { url = "https://github.com/Clicker-games-Studio/Minecraft-Offline/releases/download/MC1.8.9/Mc1.8.9.zip"; version = "1.8.9"; }
            else if (checkBox5.Checked) { url = "https://github.com/Clicker-games-Studio/Minecraft-Offline/releases/download/MC1.12/Mc1.12.zip"; version = "1.12"; }
            else if (checkBox6.Checked) { url = "https://github.com/Clicker-games-Studio/Minecraft-Offline/releases/download/1.17.1/MC1.17.1BETA1.zip"; version = "1.17.1"; } // ✅ New version
            else
            {
                MessageBox.Show("Please select a version before downloading.");
                return;
            }

            try
            {
                string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                string targetDir = Path.Combine(appData, "minecraft-offline", version);

                // Delete target folder if it exists to allow overwrite
                if (Directory.Exists(targetDir))
                    Directory.Delete(targetDir, true);

                Directory.CreateDirectory(targetDir);

                string tempZipPath = Path.Combine(Path.GetTempPath(), $"{version}.zip");

                // Disable buttons while downloading
                button1.Enabled = false;
                button2.Enabled = false;

                client = new WebClient();

                // Update label on progress
                client.DownloadProgressChanged += (s, ev) =>
                {
                    download_label.Text = $"Downloading... {ev.ProgressPercentage}%";
                };

                // When download completes
                client.DownloadFileCompleted += (s, ev) =>
                {
                    try
                    {
                        // Change label before extraction
                        download_label.Text = "Unzipping... This can take a while";

                        // Extract zip
                        ZipFile.ExtractToDirectory(tempZipPath, targetDir);
                        File.Delete(tempZipPath);

                        download_label.Text = $"Download complete: {version}";
                        MessageBox.Show($"Version {version} downloaded and extracted to:\n{targetDir}");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error: {ex.Message}");
                    }
                    finally
                    {
                        // Re-enable buttons
                        button1.Enabled = true;
                        button2.Enabled = true;
                    }
                };

                // Start async download
                client.DownloadFileAsync(new Uri(url), tempZipPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
                button1.Enabled = true;
                button2.Enabled = true;
            }
        }

        // START BUTTON
        private void button2_Click(object sender, EventArgs e)
        {
            string version = "";

            // Determine selected version
            if (checkBox1.Checked) version = "1.2.5";
            else if (checkBox2.Checked) version = "1.8.7";
            else if (checkBox3.Checked) version = "1.8.8";
            else if (checkBox4.Checked) version = "1.8.9";
            else if (checkBox5.Checked) version = "1.12";
            else if (checkBox6.Checked) version = "1.17.1"; // ✅ New version
            else
            {
                MessageBox.Show("Please select a version to start.");
                return;
            }

            string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string targetDir = Path.Combine(appData, "minecraft-offline", version);
            string batFile = "";

            // Map version to correct .bat file
            if (version == "1.2.5")
                batFile = Path.Combine(targetDir, "mc125-launcher.bat");
            else
                batFile = Path.Combine(targetDir, "start.bat"); // All other versions

            if (!File.Exists(batFile))
            {
                MessageBox.Show($"The file {batFile} does not exist. Make sure the version is downloaded.");
                return;
            }

            try
            {
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = batFile,
                    WorkingDirectory = targetDir,
                    UseShellExecute = true
                };
                Process.Start(psi);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error starting the game: {ex.Message}");
            }
        }
    }
}
