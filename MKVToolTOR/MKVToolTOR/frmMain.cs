using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MKVToolTOR
{
    public partial class frmMain : Form
    {
        string mkvmerge_query = " -o \"{1}\"  --audio-tracks th  -S   \"{0}\"";
        string ffmpeg_query = "   -i \"{0}\" -s {2}  \"{1}\"";
        string mkvmerge_file =Application.StartupPath +   @"\MKVToolNix\mkvmerge.exe";
        string ffmpeg_file = Application.StartupPath +  @"\ffmpeg\ffmpeg.exe";
        string strmkvmerge = @"Progress: (\d+)%";
        double _progress = 0; int index;

        string display="";
        public frmMain()
        {
            InitializeComponent();
            dataGridView1.SetDefaultCellStyle();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog fol = new FolderBrowserDialog())
            {
                fol.ShowDialog();
                textBox1.Text = fol.SelectedPath;

                
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBox1.Text))
                Properties.Settings.Default.strPath = textBox1.Text;
            Properties.Settings.Default.Save();
            if (string.IsNullOrEmpty(textBox1.Text) || !Directory.Exists(textBox1.Text)) return;
            Task.Run(() =>
            {
                dataGridView1.Invoke(new Action(() => dataGridView1.Rows.Clear()));
                Directory.GetFiles(textBox1.Text, "*.mkv", SearchOption.AllDirectories)
                     .Where(f => !Path.GetFileNameWithoutExtension(f).ToLower().Contains("_.mkv"))
                     .ToList<string>()
                     .ForEach(f =>
                    {
                        txtStatus.Text = "Add File:" + f;
                        dataGridView1.Invoke(new Action(() => dataGridView1.Rows.Add(f)));

                    });

                txtStatus.Text = "Add File Complete";

            });
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text) || string.IsNullOrEmpty(textBox4.Text)) return;


            System.IO.Directory.GetFiles(textBox1.Text, "*" + textBox4.Text + "*", SearchOption.AllDirectories).ToList<string>()
                     .ForEach(f =>
                     {
                         string _f = Path.GetDirectoryName(f) + "\\" + Path.GetFileNameWithoutExtension(f).Replace(textBox4.Text, "").Trim() + Path.GetExtension(f);
                         txtStatus.Text = "Move File:" + f;
                         File.Move(f, _f);
                     }
                     );
            txtStatus.Text = "Move File Complete";

        }
      
        private void button4_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text)) return;
             display = (comboBox1.Text == "")?"1280x720":comboBox1.Text;
            if (dataGridView1.Rows.Count <= 0)
                MessageBox.Show("ConvertMKV All  Complete");
            backgroundWorker1.Maximum = dataGridView1.Rows.Count;
            backgroundWorker1.Minimum = 0;
            backgroundWorker1.Value = 0;
            backgroundWorker1.WorkerReportsProgress = true;
           
            backgroundWorker1.RunWorkerAsync();
        }

        private void textBox4_Leave(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBox4.Text))
                Properties.Settings.Default.strReplace = textBox4.Text;
            Properties.Settings.Default.Save();
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBox1.Text))
                Properties.Settings.Default.strPath = textBox1.Text;
            Properties.Settings.Default.Save();
        }

        private void comboBox1_Leave(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBox1.Text))
                Properties.Settings.Default.strDisplay = comboBox1.Text;
            Properties.Settings.Default.Save();
        }
        bool checkRun = true;
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            if (dataGridView1.Rows.Count > 0)
            {
                checkRun = true;
                index = 0;
                ConvertMKVItem();
                do { } while (checkRun == true);
            }
        }
       
        private void ConvertMKVItem()
        {
            string file = dataGridView1[0, index].Value.ToString();
            string _file = Path.GetDirectoryName(file) + "\\" + Path.GetFileName(file).Replace(".mkv", "_.mkv");
            dataGridView1.Invoke(new Action(() => dataGridView1[ 1,index].Value = 5));

            System.Threading.Thread.Sleep(1000);
            try
            {
                txtStatus.Text = "ConvertMKVItem:" + file;
            }
            catch { }
           
            Process process = new Process
            {
                StartInfo =
               {
                   FileName = mkvmerge_file,
                   Arguments = string.Format(mkvmerge_query,file, _file),
                   CreateNoWindow = true,
                   WindowStyle =  ProcessWindowStyle.Hidden


               },
                EnableRaisingEvents = true
            };

             process.OutputDataReceived += (object sender, DataReceivedEventArgs e) =>
            {
                string consoleLine = e.Data;
                if (!string.IsNullOrWhiteSpace(consoleLine))
                    ParsemkvmergeProgress(consoleLine);
                

                
            };

            process.ErrorDataReceived += (object sender, DataReceivedEventArgs e) =>
            {
                string consoleLine = e.Data;

                if (!string.IsNullOrWhiteSpace(consoleLine))
                    ParsemkvmergeProgress(consoleLine);
                

               
            };


            process.Exited += new EventHandler((object _s, EventArgs _e) =>
            {

                process.Dispose();
              
                if (checkBox1.Checked)
                {
                    try
                    {
                        File.Delete(file);
                        File.Move(_file, file);
                    }
                    catch { }

                }
                if (checkBox2.Checked)
                {
                    if (checkBox1.Checked)
                    {
                        ConvertffmpegItem(file);
                    }
                    else
                    {
                        ConvertffmpegItem(_file);
                    }

                }
                else
                {
                    dataGridView1.Invoke(new Action(() => dataGridView1[1, index].Value = 100));
                    index++;
                    if (index >= dataGridView1.Rows.Count)
                    {
                        backgroundWorker1.Invoke(new Action(() => backgroundWorker1.Value++));
                        checkRun = false;
                        MessageBox.Show("ConvertMKV All Complete" ); 
                        return;
                    }
                    backgroundWorker1.Invoke(new Action(()=> backgroundWorker1.Value++));
                    ConvertMKVItem( );
                }
               
            });
            dataGridView1.Invoke(new Action(() => dataGridView1[ 1,index].Value = 10));
            process.Start();


        }
         private void ParsemkvmergeProgress(string consoleLine)
                {
           
                    Regex extractDownloadProgress = new Regex(@"Progress:\s{0,}(\d+)%");
                    Match match = extractDownloadProgress.Match(consoleLine);
                    if (match.Length > 0 && match.Groups.Count >= 1)
                    {
                        double downloadProgress = double.Parse("0"+ match.Groups[1].Value);
                       /* if (double.TryParse(match.Groups[1].Value, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out double downloadProgress))
                        {*/
                   
                            if (downloadProgress > 100 || _progress > 100)
                            {
                                Debugger.Break();
                            }
                            if (checkBox2.Checked)
                            {
                                dataGridView1.Invoke(new Action(() => dataGridView1[ 1,index].Value = (int)(10 + downloadProgress / 100 * 30)));
                            }
                            else
                            {
                                dataGridView1.Invoke(new Action(() => dataGridView1[1, index].Value = (int)(10 + downloadProgress / 100*90)));
                            }
                       
                           // item.DownloadProgress = (int)(10.0 + downloadProgress / 100 * 60);
                            //OnProgressChanged(new ProgressChangedEventArgs(_progress * 100 / _totalSongs, null));
                        //}
                    }
                }

        private void ConvertffmpegItem(string f)
        {
            string file = f;
            string _file = Path.GetDirectoryName(file) + "\\" + Path.GetFileNameWithoutExtension(file) + ".mp4";
            txtStatus.Text = "ConvertffmpegItem:" + f;
            Process process = new Process
            {
                StartInfo =
               {
                   FileName = ffmpeg_file,
                   Arguments = string.Format(ffmpeg_query,file, _file,display),
                   CreateNoWindow = true,
                   WindowStyle =  ProcessWindowStyle.Normal


               },
                EnableRaisingEvents = true
            };

            process.OutputDataReceived += (object sender, DataReceivedEventArgs e) =>
            {
                string consoleLine = e.Data;

              //  richTextBox1.Invoke(new Action(() => richTextBox1.Text = consoleLine));
                if (!string.IsNullOrWhiteSpace(consoleLine))
                    ParseffmpegProgress(consoleLine);
            };

            process.ErrorDataReceived += (object sender, DataReceivedEventArgs e) =>
            {
                string consoleLine = e.Data;
              //  richTextBox1.Invoke(new Action(() => richTextBox1.Text = consoleLine));
                if (!string.IsNullOrWhiteSpace(consoleLine))
                    ParseffmpegProgress(consoleLine);

            };


            process.Exited += new EventHandler((object _s, EventArgs _e) =>
            {

                process.Dispose();
                backgroundWorker1.Invoke(new Action(() => backgroundWorker1.Value++));
              

                index++;
                if (index >= dataGridView1.Rows.Count)
                {
                    checkRun = false;
                    MessageBox.Show("ConvertMKV All Complete"); 
                    return; 
                }

                ConvertMKVItem();
            });
          
            process.Start();


        }


        private void ParseffmpegProgress(string consoleLine)
        {
            // Duration: 00:30:58.39, start: 0.023021, bitrate: 106 kb/s
            // size=     105kB time=00:00:06.69 bitrate= 128.7kbits/s speed=13.4x 
            /*
             Past duration 0.995995 too large
            frame=  624 fps= 60 q=28.0 size=    4241kB time=00:00:26.11 bitrate=1330.7kbits/s dup=1 drop=0 speed=2.51x
             */
            Regex extractDuration = new Regex(@"Duration:\s*([0-9]{2}:[0-9]{2}:[0-9]{2}\.[0-9]{2}),\sstart:");

            double Duration=0d;
            Match match = extractDuration.Match(consoleLine);
            txtStatus.Text = "Duration:" + match.Groups[1].Value;
            if (match.Length > 0 && match.Groups.Count >= 1)
            {
                Duration = TimeSpan.Parse(match.Groups[1].Value).TotalSeconds;
                if (Duration == 0d)
                {
                    return;
                }

                Regex extractProgressDuration = new Regex(@"time=([0-9]{2}:[0-9]{2}:[0-9]{2}\.[0-9]{2})\s*bitrate=");
                match = extractProgressDuration.Match(consoleLine);
                if (match.Length > 0 && match.Groups.Count >= 1)
                {

                    dataGridView1.Invoke(new Action(() => dataGridView1[1, index].Value = (int)(70 + TimeSpan.Parse(match.Groups[1].Value).TotalSeconds / Duration * 60)));

                }
            }

           
        }
       

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            textBox1.Text = Properties.Settings.Default.strPath;
            textBox4.Text = Properties.Settings.Default.strReplace;
            comboBox1.Text = Properties.Settings.Default.strDisplay;
        }
    }
}
