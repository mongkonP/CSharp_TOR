using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MP3MNG
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
		
		 private void Form1_Load(object sender, EventArgs e)
        {
            txtCri.Text = Properties.Settings.Default.Cri;
            txtPathS.Text = Properties.Settings.Default.PathS;
            txtPathT.Text = Properties.Settings.Default.PathT;
            txtPathF.Text = Properties.Settings.Default.PathF;
            // MessageBox.Show(TORServices.PathFileTor.FileTor.FileName("@##&%GLKF$^#*"));
            /* TagLib.File file = TagLib.File.Create(@"J:\_เธฃเธงเธกเน€เธเธฅเธ\..ยคยนร ยดร”ยนยถยนยน\เธเธฅเธเธฅ - ..เธเธเน€เธ”เธดเธเธ–เธเธ -03- เธซเนเธงเธเนเธข.mp3");
             String title = file.Tag.Title;
             String album = file.Tag.Album;
             String length = file.Properties.Duration.ToString();*/
            /* var mp3 = new ID3(@"J:\_เธฃเธงเธกเน€เธเธฅเธ\ยกรยณร”ยกร’รรฌ - Single\เธเธฃเธ“เธดเธเธฒเธฃเน - เธกเธเธ•เนเนเธเธ เนเธเนเธเธเธนเธ.mp3");

               MessageBox.Show(mp3.Album);*/

         /*  Directory.GetFiles(@"H:\New folder", "*", SearchOption.AllDirectories).ToList<string>()
                      .ForEach(f =>
                      {

                          this.Invoke(new Action(() => this.Text = "Check:" + f));

                          string _f = @"H:\New folder\" + Path.GetFileName(f).Replace("_เธเธนเธเธน", "");
                          if (f != _f)
                              TORServices.PathFileTor.FileTor.MoveFile(f, _f);
                      });*/


        }
        #region _Function

     async   void DelEmptyDirectory(string startLocation)
        {
            await Task.Factory.StartNew(() =>
            {
                foreach (var directory in Directory.GetDirectories(startLocation))
                {
                    DelEmptyDirectory(directory);
                    if (Directory.GetFiles(directory).Length == 0 &&
                        Directory.GetDirectories(directory).Length == 0)
                    {
                        Directory.Delete(directory, false);
                    }
                }
            });
        }
        string SelectPath()
        {
            string path = "";
            using (FolderBrowserDialog fol = new FolderBrowserDialog())
            {
                if (fol.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                   path = fol.SelectedPath;

                }
            }

            return path;
        
        }

      async  void MoveFilebyCri(string pathS, string pathT, string cri )
        {
            await Task.Factory.StartNew(() =>
            {
                Directory.GetFiles(pathS, "*" + cri + "*", SearchOption.AllDirectories).ToList<string>()
                       .ForEach(f =>
                       {


                           this.Invoke(new Action(() => this.Text = "Check:" + f));

                           string _f = pathT + "\\" + ((cri != "*" && !string.IsNullOrEmpty(cri)) ? cri + "\\" : "") + Path.GetFileName(f);
                           if (f != _f)
                               TORServices.PathFileTor.FileTor.MoveFile(f, _f);
                       });
                this.Invoke(new Action(() => this.Text = "Check: complete"));
            });

        }

       async void  ReplaceByRegex(string pathS, string regex)
        {
            await Task.Factory.StartNew(() =>
            {

                Directory.GetFiles(pathS, "*", SearchOption.AllDirectories).ToList<string>()
                       .ForEach(f =>
                       {

                           this.Invoke(new Action(() => this.Text = "Check:" + f));
                           string _f = pathS + "\\" + new Regex(regex, RegexOptions.Compiled).Replace(Path.GetFileNameWithoutExtension(f), "").Trim() + Path.GetExtension(f);
                           if (f != _f)
                           {
                               try
                               {
                                   if (!Directory.Exists(Path.GetDirectoryName(_f)))
                                       Directory.CreateDirectory(Path.GetDirectoryName(_f));
                                   File.Move(f, _f);
                               }
                               catch { File.Delete(f); }
                           }
                       });
                this.Invoke(new Action(() => this.Text = "Check: complete"));
            });
            
        }
        async void ReplaceByCri(string pathS, string cri)
        {
            await Task.Factory.StartNew(() =>
            {

                Directory.GetFiles(pathS, "*" + cri + "*", SearchOption.AllDirectories).ToList<string>()
                       .ForEach(f =>
                       {

                           this.Invoke(new Action(() => this.Text = "Check:" + f));
                           string _f = pathS + "\\" + Path.GetFileNameWithoutExtension(f).Replace(cri,"").Trim() + Path.GetExtension(f);
                           if (f != _f)
                           { 
                           try
                               {
                                   if (!Directory.Exists(Path.GetDirectoryName(_f)))
                                       Directory.CreateDirectory(Path.GetDirectoryName(_f));
                                   File.Move(f, _f);
                               }
                               catch { File.Delete(f); }
                           }
                               
                       });
                this.Invoke(new Action(() => this.Text = "Check: complete"));
            });

        }
        string TagAlbum(string f)
        {
            string s = "";
            try
            {
                 var mp3 = new ID3(f);
                 s = "" + mp3.Album;
            }
            catch { }

            return s;
        }
        async void MoveFileHaveAlbum(string pathS)
        {
            await Task.Factory.StartNew(() =>
            {
                this.Invoke(new Action(() => this.Text = "get File have Album..."));
                Directory.GetFiles(pathS, "*.mp3", SearchOption.TopDirectoryOnly)
                
                .ToList<string>()
                       .ForEach(f =>
                       {
                          
                           this.Invoke(new Action(() => this.Text = "Check:" + f));
                           
                           String album = TagAlbum(f).Trim();
                         
                           if (!string.IsNullOrEmpty(album.Trim()))
                           {
                               string _f = pathS +"\\" + TORServices.PathFileTor.FileTor.FileName( album )+ "\\" + Path.GetFileName(f);
                               {
                                   try
                                   {
                                       if (!Directory.Exists(Path.GetDirectoryName(_f)))
                                           Directory.CreateDirectory(Path.GetDirectoryName(_f));
                                       File.Move(f, _f);
                                   }
                                   catch { File.Delete(f); }
                               }
                           }
                           
                       });
                this.Invoke(new Action(() => this.Text = "Check: complete"));
            });

        }

        async void AddCriinFile(string pathS, string cri,bool back = false)
        {
            await Task.Factory.StartNew(() =>
            {
                

                    Directory.GetFiles(pathS, "*", SearchOption.TopDirectoryOnly).ToList<string>()
                        .ForEach(f =>
                        {

                            this.Invoke(new Action(() => this.Text = "Check:" + f));

                            string _f = pathS + "\\" + ((back)?Path.GetFileNameWithoutExtension(f)  + "_" + cri + Path.GetExtension(f) : cri + "_" + Path.GetFileName(f));
                            if (f != _f)
                                TORServices.PathFileTor.FileTor.MoveFile(f, _f);
                        });
                    this.Invoke(new Action(() => this.Text = "Check: complete"));
              
            });
        }
        async  void ReplaceSymbol(string pathS)
        {
          await  Task.Factory.StartNew(()=> ReplaceByRegex(pathS, @"(^[\d\.\s-_\)\(]+)"));
        }
        async void DeleteMp4File(string pathS)
        {
            await Task.Factory.StartNew(() =>
            {


                var files = (from fl in Directory.GetFiles(pathS, "*.mp4", SearchOption.TopDirectoryOnly)
                             where File.Exists(fl.Replace(".mp4", ".mp3"))
                             select fl)
                   .ToList<string>();
                if (files != null && files.Count > 0)
                {
                    files.ForEach(f =>
                    {

                        this.Invoke(new Action(() => this.Text = "Check:" + f));
                        try { File.Delete(f); } catch { }
                    });
                    this.Invoke(new Action(() => this.Text = "Check: complete"));
                }

                files = (from fl in Directory.GetFiles(pathS, "*.mkv", SearchOption.TopDirectoryOnly)
                         where File.Exists(fl.Replace(".mkv", ".mp3"))
                         select fl)
                   .ToList<string>();
                if (files != null && files.Count > 0)
                {
                    files.ForEach(f =>
                    {

                        this.Invoke(new Action(() => this.Text = "Check:" + f));
                        try { File.Delete(f); } catch { }
                    });
                    this.Invoke(new Action(() => this.Text = "Check: complete"));
                }
            });

        }
            #endregion
            #region _SetProperties
            private void button5_Click(object sender, EventArgs e)
        {
            txtPathS.Text = SelectPath();
            Properties.Settings.Default.PathS = txtPathS.Text;
            Properties.Settings.Default.Save();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            txtPathT.Text = SelectPath();
            Properties.Settings.Default.PathT = txtPathT.Text;
            Properties.Settings.Default.Save();

        }

        private void txtCri_Leave(object sender, EventArgs e)
        {
            Properties.Settings.Default.Cri =txtCri.Text;
            Properties.Settings.Default.Save();
        }
        private void txtPathS_Leave(object sender, EventArgs e)
        {
            Properties.Settings.Default.PathS = txtPathS.Text;
            Properties.Settings.Default.Save();
        }
        private void txtPathT_Leave(object sender, EventArgs e)
        {
            Properties.Settings.Default.PathT = txtPathT.Text;
            Properties.Settings.Default.Save();
        }

        private void txtPathF_Leave(object sender, EventArgs e)
        {
            Properties.Settings.Default.PathF = txtPathF.Text;
            Properties.Settings.Default.Save();
        }
        #endregion


        #region _Action
        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtPathS.Text.Trim()) || string.IsNullOrEmpty(txtPathT.Text.Trim()) || string.IsNullOrEmpty(txtCri.Text.Trim())) return;
            Task.Run(() => MoveFilebyCri(txtPathS.Text.Trim(), txtPathT.Text.Trim(), txtCri.Text.Trim()));
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtPathS.Text.Trim()) || 
                string.IsNullOrEmpty(txtPathT.Text.Trim()) ||
                string.IsNullOrEmpty(txtPathF.Text.Trim())
                ) return;
            Task.Run(() =>
            {
                List<Task> tasks = new List<Task>(); 
                Directory.GetFiles(txtPathF.Text, "*.mp3", SearchOption.TopDirectoryOnly).ToList<string>()
                  .ForEach(f =>
                  {
                      string art = "";
                      try
                      {
                          art = new Regex(@".*[_-](.*)", RegexOptions.None).Matches(Path.GetFileNameWithoutExtension(f))[0].Groups[1].Value;
                      }
                      catch { }

                      if (!string.IsNullOrEmpty(art))
                      {
                          tasks.Add( Task.Run(() => MoveFilebyCri(txtPathS.Text.Trim(), txtPathT.Text.Trim(), art.Trim())));
                      }
                     
                  });

                Task.WaitAll(tasks.ToArray());
                DelEmptyDirectory(txtPathS.Text.Trim());
            });
        }


        #endregion

        private void button3_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrEmpty(txtPathS.Text.Trim()) || string.IsNullOrEmpty(txtPathT.Text.Trim())) return;
            Task.Run(() => MoveFilebyCri(txtPathS.Text, txtPathT.Text.Trim(),""));
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtPathS.Text.Trim())) return;
          Task.Factory.StartNew(()=>  ReplaceSymbol(txtPathS.Text.Trim()));
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtPathS.Text.Trim()) || string.IsNullOrEmpty(txtCri.Text.Trim())) return;
            Task.Factory.StartNew(() => ReplaceByCri(txtPathS.Text.Trim(),txtCri.Text.Trim()));
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtPathS.Text.Trim()) || string.IsNullOrEmpty(txtCri.Text.Trim())) return;
            Task.Factory.StartNew(() => AddCriinFile(txtPathS.Text.Trim(), txtCri.Text.Trim()));
        }

        private void button11_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtPathS.Text.Trim())) return;
            Task.Factory.StartNew(() => MoveFileHaveAlbum(txtPathS.Text.Trim()));
        }

        private void button12_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtPathS.Text.Trim())) return;
            Task.Factory.StartNew(() => DeleteMp4File(txtPathS.Text.Trim()));
          
        }

        private void button13_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtPathS.Text.Trim()) || string.IsNullOrEmpty(txtCri.Text.Trim())) return;
            Task.Factory.StartNew(() => AddCriinFile(txtPathS.Text.Trim(), txtCri.Text.Trim(),true));
        }

        private void button14_Click(object sender, EventArgs e)
        {
            dupAllCheck f = new dupAllCheck(txtPathS.Text);
            f.ShowDialog();
        }
    }
}
