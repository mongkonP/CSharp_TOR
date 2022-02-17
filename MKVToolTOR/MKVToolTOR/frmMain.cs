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
       // string strmkvmerge = @"Progress: (\d+)%";
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
                Directory.GetDirectories(textBox1.Text, "*", SearchOption.TopDirectoryOnly)
                .ToList<string>()
                .ForEach(fol =>
                {
                    var lstMKV = Directory.GetFiles(fol, "*.mkv", SearchOption.TopDirectoryOnly);
                    if (lstMKV.Length == 1)
                    {
                        try
                        {
                            File.Move(lstMKV[0], textBox1.Text + "\\" + Path.GetFileName(lstMKV[0]));
                            Directory.Delete(fol,true);
                        }
                        catch { }
                    
                    }


                });
                List<string> cris = new List<string>() { "[Mini HD]", "[MINI Super-HQ]_", "[MINI]",
           "{MINI}","{MINI Super-HQ}_","[Mini Super-1080p.HQ]","TT ","TT_","{MINI Super-HQ}",
               "[MINI-HQ]_","[Mini-hidef]","[Mini-HD]","[HQ]","{MINI-HQ}_"};
                string fl = textBox1.Text;

                cris.ForEach(cri =>
                {
                    System.IO.Directory.GetFiles(fl, "*" + cri + "*", System.IO.SearchOption.TopDirectoryOnly).ToList<string>()
                   .ForEach(f =>
                   {
                       string _f = fl + "\\" + System.IO.Path.GetFileName(f).Replace(cri, "").Trim();
                       try
                       {
                           System.IO.File.Move(f, _f);
                       }
                       catch { }
                   });

                });


                MessageBox.Show("complete...");

                dataGridView1.Invoke(new Action(() => dataGridView1.Rows.Clear()));
                Directory.GetFiles(textBox1.Text, "*.mkv", SearchOption.AllDirectories)
                     .Where(f => !Path.GetFileNameWithoutExtension(f).ToLower().Contains(".mkv"))
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

            Task.Run(() =>
            {
                System.IO.Directory.GetFiles(textBox1.Text, "*" + textBox4.Text + "*", SearchOption.AllDirectories).ToList<string>()
                     .ForEach(f =>
                     {
                         string _f = Path.GetDirectoryName(f) + "\\" + Path.GetFileNameWithoutExtension(f).Replace(textBox4.Text, "").Trim() + Path.GetExtension(f);
                         txtStatus.Text = "Move File:" + f;
                         File.Move(f, _f);
                     }
                     );
            txtStatus.Text = "Move File Complete";
           
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

            if (File.Exists(file))
            {
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

                process.Exited += new EventHandler((object _s, EventArgs _e) =>
                {

                    process.Dispose();

                    if (checkBox1.Checked)
                    {
                        try
                        {
                            System.Threading.Thread.Sleep(1000);
                            File.Delete(file);
                            System.Threading.Thread.Sleep(1000);
                            File.Move(_file, file);
                        }
                        catch { }

                    }
                    if (checkBox2.Checked)
                    {
                        dataGridView1.Invoke(new Action(() => dataGridView1[1, index].Value = 30));
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
                            MessageBox.Show("ConvertMKV All Complete");
                            return;
                        }
                        backgroundWorker1.Invoke(new Action(() => backgroundWorker1.Value++));
                        ConvertMKVItem();
                    }

                });
                dataGridView1.Invoke(new Action(() => dataGridView1[1, index].Value = 10));

                process.Start();
            }
            else
            {
                dataGridView1.Invoke(new Action(() => dataGridView1[1, index].Value = 100));
                index++;
                if (index >= dataGridView1.Rows.Count)
                {
                    backgroundWorker1.Invoke(new Action(() => backgroundWorker1.Value++));
                    checkRun = false;
                    MessageBox.Show("ConvertMKV All Complete");
                    return;
                }
                backgroundWorker1.Invoke(new Action(() => backgroundWorker1.Value++));
                ConvertMKVItem();
            }

             


         


        }
        void ConvertffmpegItem(string file)
        {

            string str = "",str1 = "";
            Task.Run(() =>
            {
                Regex myRegex = new Regex(@"Output #0.*?\n\s{0,}.*?\n\s{0,}.*?\n\s{0,}.*?\n\s{0,}.*?\n\s{0,}.*?\n\s{0,}.*?\n\s{0,}NUMBER_OF_FRAMES.*?:\s{1,}(\d+)\n", RegexOptions.None);
                Regex myRegex_ = new Regex(@"frame=\s{0,}(\d+)", RegexOptions.None);
                long frameAll = 0;
                Process proc = new Process();
                proc.StartInfo.FileName = ffmpeg_file;
                proc.StartInfo.Arguments = string.Format(ffmpeg_query, file, Path.GetDirectoryName(file) + Path.GetFileNameWithoutExtension(file) + ".mp4" , display);
                proc.StartInfo.RedirectStandardError = true;
                proc.StartInfo.UseShellExecute = false;
                proc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                if (!proc.Start())
                {

                    return;
                }
                StreamReader reader = proc.StandardError;
          
                string line = "";


                while ((line = reader.ReadLine()) != null)
                {
                    str += "\n" + line;
                    
                        str1 = line;

                        if (str1.Contains("frame="))
                        {
                            if (frameAll <= 0)
                            {
                                Match match = myRegex.Match(str);
                                if (match.Length > 0 && match.Groups.Count >= 1)
                                {
                                    frameAll = long.Parse(match.Groups[1].Value);

                                }

                            }
                            Match match_ = myRegex_.Match(str1);
                            if (match_.Length > 0 && match_.Groups.Count >= 1)
                            {
                                long frame = long.Parse(match_.Groups[1].Value);
                                if (frame >= 0)
                                {
                                dataGridView1.Invoke(new Action(() => dataGridView1[1, index].Value =30d+ (Convert.ToDouble(frame) / Convert.ToDouble(frameAll) * 100d)*70d));
                                //  this.Invoke(new Action(() => this.Text = "Frame:" + frame + "/" + frameAll + "  " + (Convert.ToDouble(frame) / Convert.ToDouble(frameAll) * 100d).ToString("0.000") + " %"));
                            }

                            }
                        }

                   
                
                }
                proc.Close();



            }).Wait();
            dataGridView1.Invoke(new Action(() => dataGridView1[1, index].Value = 100));

        }


        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            textBox1.Text = "H:\\New folder";// Properties.Settings.Default.strPath;
            textBox4.Text = Properties.Settings.Default.strReplace;
            comboBox1.Text = Properties.Settings.Default.strDisplay;

           /* string dir = @"J:\tt_Load\24.Douluo.Dalu.(Soul Land).ตำนานจอมยุทธ์ภูตถังซาน ตอนที่ 1-142";
            Directory.GetFiles(dir, "*.mkv").ToList<string>()
                .ForEach(f =>
                {

                    string _f = dir + "\\" + int.Parse(Path.GetFileNameWithoutExtension(f)).ToString("000") + ".mkv";
                    File.Move(f, _f);
                });*/
        }
    }
}
