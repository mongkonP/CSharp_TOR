using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;


namespace TORServices.PathFileTor
{
  

    public static class FileTor
    {
        public static string videoFilter = ".avi .mpg .mpeg .mov .flv .3gp .rm .rmvb .mp4 .0gm .mkv .mov .wmv .vob";
        public static string audioFilter = ".mp3 .wav .ogg .mid .rm .wma m4a";
        public static string imagesFilter = ".jpg .jpeg .bmp .gif .ico .tga .png";
        public static string codeFilter = ".c .cpp .h .java .class .jar .cs .csproj .vbproj";
        public static string softwareFilter = ".exe .msi .rpm .bin .deb .iso .nrg .zip .rar";
        public static string documentsFilter = ".pdf .doc .htm .html .mht .txt .ppt .xl .pps .tex .dvi";

        public static string FilterFile = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png" +
                                    "|txt files (*.txt)|*.txt" +
                                    "|Document files (*.doc, *.docx, *.xls,  *.xlsx, *.ppt, *.pptx, *.pdf )|*.doc, *.docx, *.xls,  *.xlsx, *.ppt, *.pptx, *.pdf " +
                                    "|Audio/Vdio files (*.AVI, *.XVID, *.DivX *.3GP *.MKV *.MP4 *.FLV *.MP3 *.DAT)|*.AVI, *.XVID, *.DivX *.3GP *.MKV *.MP4 *.FLV *.MP3 *.DAT" +
                                    "|All files (*.*)|*.*";
        public static string SelectFile(string _path, string InitialDirectory = "", string ExtFile = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png" +
                                    "|txt files (*.txt)|*.txt" +
                                    "|Document files (*.doc, *.docx, *.xls,  *.xlsx, *.ppt, *.pptx, *.pdf )|*.doc, *.docx, *.xls,  *.xlsx, *.ppt, *.pptx, *.pdf " +
                                    "|Audio/Vdio files (*.AVI, *.XVID, *.DivX *.3GP *.MKV *.MP4 *.FLV *.MP3 *.DAT)|*.AVI, *.XVID, *.DivX *.3GP *.MKV *.MP4 *.FLV *.MP3 *.DAT" +
                                    "|All files (*.*)|*.*")
        {
            string _F = "";
            try
            {
                System.Windows.Forms.OpenFileDialog fb = new System.Windows.Forms.OpenFileDialog();
                if(!string.IsNullOrEmpty(InitialDirectory))
                {
                    fb.InitialDirectory = InitialDirectory;
                }
                fb.ShowDialog();
                _F = fb.FileName;
            }
            catch { }
            return _F;
        }
      public static  bool IsDirectoryEmpty(this string path)
        {
            return !Directory.EnumerateFileSystemEntries(path).Any();


           
        }
      public static void MoveFile(string s, string t)
      {
            // if (s == t) return;
            // t = RenameFileDup(t);
            if(!Directory.Exists( Path.GetDirectoryName(t)))
            Directory.CreateDirectory(Path.GetDirectoryName(t));
          try { File.Move(s, t); }
          catch { System.IO.File.Delete(s); }

      }
        public static string GetMD5HashFromFile(this string filename)
                {
                   // if (!System.IO.File.Exists(filename)) return "";
                    /*using (var md5 = new System.Security.Cryptography.MD5CryptoServiceProvider())
                    {
                        var buffer = md5.ComputeHash(System.IO.File.ReadAllBytes(filename));
                        var sb = new StringBuilder();
                        for (int i = 0; i < buffer.Length; i++)
                        {
                            sb.Append(buffer[i].ToString("x2"));
                        }
                        return sb.ToString();
                    }*/
                    using (var md5 = MD5.Create())
                    {
                        using (var stream = File.OpenRead(filename))
                        {
                            var hash = md5.ComputeHash(stream);
                            return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                        }
                    }

                }
        public static List<string> GetMD5HashFromFiles(this string Dir, string searchPattern = "*.*", System.IO.SearchOption searchOption = System.IO.SearchOption.TopDirectoryOnly)
        {
            if (!System.IO.Directory.Exists(Dir)) return null;
            List<string> result = new List<string>();
            using (var md5 = new System.Security.Cryptography.MD5CryptoServiceProvider())
            {
                System.IO.Directory.GetFiles(Dir, searchPattern, searchOption).ToList<string>()
                 .ForEach(f =>
                 {

                     var buffer = md5.ComputeHash(System.IO.File.ReadAllBytes(f));
                     var sb = new StringBuilder();
                     for (int i = 0; i < buffer.Length; i++)
                     {
                         sb.Append(buffer[i].ToString("x2"));
                     }
                     result.Add(sb.ToString());
                 });
            }
            return result;

        }
        public static System.Data.DataTable ToDatatableMD5Hash(this string Dir, string searchPattern = "*.*", System.IO.SearchOption searchOption = System.IO.SearchOption.TopDirectoryOnly)
        {

            if (string.IsNullOrEmpty(Dir)) return null;
            System.Data.DataTable dt = new System.Data.DataTable();
            dt.Columns.Add(new System.Data.DataColumn("File", typeof(string)));
            dt.Columns.Add(new System.Data.DataColumn("Path", typeof(string)));
            dt.Columns.Add(new System.Data.DataColumn("Size", typeof(string)));
            dt.Columns.Add(new System.Data.DataColumn("MD5", typeof(string)));
            dt.Columns.Add(new System.Data.DataColumn("Date Modified", typeof(string)));
            dt.Columns.Add(new System.Data.DataColumn("Selected", typeof(Boolean)));
            System.IO.Directory.GetFiles(Dir, searchPattern, searchOption).ToList<string>()
                .ForEach(
                f =>
                {
                    System.Data.DataRow dr = dt.NewRow();
                    dr["File"] = System.IO.Path.GetFileNameWithoutExtension(f);
                    dr["Path"] = f;
                    dr["Size"] = new System.IO.FileInfo(f).Length;
                    dr["MD5"] = FileTor.GetMD5HashFromFile(f);
                    dr["Date Modified"] = System.IO.File.GetCreationTime(f);
                    dr["Selected"] = false;
                    dt.Rows.Add(dr);
                });
            return dt;
        }


        public static System.Diagnostics.Process IsProcessOpen(this string name)
        {
            foreach (System.Diagnostics.Process clsProcess in System.Diagnostics.Process.GetProcesses())
            {
                if (clsProcess.ProcessName.Contains(name))
                {
                    return clsProcess;
                }
            }
            return null;
        }
        public static void OpenFile(string _Path, Boolean _Edit = false)
        {
            string FileOnTemp;
            if (_Edit)
            {
                FileOnTemp = _Path;
            }
            else
            {
                FileOnTemp = System.IO.Path.GetTempPath() + "FileOnTemp" + string.Format("{0:ddMMyyyhhmmss}", DateTime.Now) + System.IO.Path.GetExtension(_Path);
                System.IO.File.Copy(_Path, FileOnTemp);
            }
            System.Diagnostics.Process.Start(FileOnTemp);
            try
            {
                System.Diagnostics.Process.Start(FileOnTemp);

            }
            catch
            {
                if (!System.IO.File.Exists(FileOnTemp))
                {
                    System.Windows.Forms.MessageBox.Show("Can't Find File " + FileOnTemp);
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("Can't Open File " + FileOnTemp);

                }

            }

        }
        public static string RenameFileDup(string _File)
        {
            string stg = _File;
            if (System.IO.File.Exists(_File))
            {
                int i = 1;
                do
                {
                    stg = System.IO.Path.GetDirectoryName(_File) + "\\" + System.IO.Path.GetFileNameWithoutExtension(_File) + "_" + i + System.IO.Path.GetExtension(_File);
                    i++;
                } while (System.IO.File.Exists(stg));
            }
            return stg;
        }
      
        public static string FileName(this string file)
        {
            string _file;

            _file = file.Replace("/", "_").Replace(@"\", "_").Replace("'", "")
                .Replace("\"", "").Replace("*", "").Replace(":", "").Replace(";", "")
                .Replace("[", "").Replace("]", "").Replace("(", "").Replace(")", "")
                .Replace("~", "").Replace("+", "").Replace("@", "").Replace("$", "")
                .Replace("%", "").Replace("^", "").Replace("฿", "").Replace("|", "")
                .Replace("{", "").Replace("}", "").Replace("?", "").Replace("<", "")
                .Replace(">", "");

            return   _file ;
        }








        public static string SelectFolder()
        {
            string _p = "";
            try
            {
                System.Windows.Forms.FolderBrowserDialog fb = new System.Windows.Forms.FolderBrowserDialog();
                fb.ShowDialog();
                _p = fb.SelectedPath;

                if (_p.Substring(_p.Length - 1) != "\\")
                {
                    _p = _p + "\\";
                }
            }
            catch { _p = ""; }
            return _p;
        }
       
        public static bool RenameFileDup(string _Dir, string filters = "*.*", System.IO.SearchOption searchOption = System.IO.SearchOption.TopDirectoryOnly)
        {
            if (!System.IO.Directory.Exists(_Dir)) return false;
            System.IO.Directory.GetFiles(_Dir, filters, searchOption).ToList<string>()
                .ForEach(f => FileTor.RenameFileDup(f));
            return true;
        }
        private static IEnumerable<string> GetFiles(string _Dir, string filter, System.IO.SearchOption searchOption = System.IO.SearchOption.TopDirectoryOnly)
        {
            return System.IO.Directory.GetFiles(_Dir, filter, searchOption).ToList<string>();
        }
        private static IEnumerable<string> GetFiles(string _Dir, IEnumerable<string> filters, System.IO.SearchOption searchOption = System.IO.SearchOption.TopDirectoryOnly)
        {
            return filters.SelectMany(filter => System.IO.Directory.GetFiles(_Dir, filter, searchOption)).ToList<string>();
        }
        private static IEnumerable<string> GetFiles(string _Dir, string[] filters, System.IO.SearchOption searchOption = System.IO.SearchOption.TopDirectoryOnly)
        {
            return filters.SelectMany(filter => System.IO.Directory.GetFiles(_Dir, filter, searchOption)).ToList<string>();
        }
       

public static void DeleteDirectory(string path, bool recursive)
		{
			if (recursive)
			{
				var subfolders = Directory.GetDirectories(path);
				foreach (var s in subfolders)
				{
					DeleteDirectory(s, recursive);
				}
			}
			var files = Directory.GetFiles(path);
			foreach (var f in files)
			{
				try
				{
					var attr = File.GetAttributes(f);
					if ((attr & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
					{
						File.SetAttributes(f, attr ^ FileAttributes.ReadOnly);
					}
					File.Delete(f);
				}
				catch(IOException)
				{
					//IOErrorOnDelete = true;
				}
			}
		 
			// At this point, all the files and sub-folders have been deleted.
			// So we delete the empty folder using the OOTB Directory.Delete method.
			Directory.Delete(path);
		}
        public static void DeleteFile(string _Dir,
            string filter = "*.*",
            System.IO.SearchOption searchOption = System.IO.SearchOption.TopDirectoryOnly,
            double size = 0)
        {
            System.IO.Directory.EnumerateFiles(_Dir, filter, searchOption)
                               .ToList<string>()
                               .ForEach(file =>
                               {
                                   if (new System.IO.FileInfo(file).Length == size)
                                       try { System.IO.File.Delete(file); }
                                       catch { }
                               });
        }
        public static void DeleteFile(string _Dir,
            string filter = "*.*",
            System.IO.SearchOption searchOption = System.IO.SearchOption.TopDirectoryOnly,
            double sizeMin = 0, double sizeMax = 100)
        {
            System.IO.Directory.EnumerateFiles(_Dir, filter, searchOption)
                               .ToList<string>()
                               .ForEach(file =>
                               {
                                   if (new System.IO.FileInfo(file).Length >= sizeMin && new System.IO.FileInfo(file).Length <= sizeMax)
                                       try { System.IO.File.Delete(file); }
                                       catch { }
                               });
        }

    }


   
}
