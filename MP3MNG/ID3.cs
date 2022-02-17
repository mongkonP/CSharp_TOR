using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MP3MNG
{
    class ID3
    {
        private string FilePath = "";
        private string _Title = "";
        private string _Artist = "";
        private string _Album = "";
        private string _Year = "";
        private string _Comment = "";
        private int _TitleNumber;
        private short _Genre;
        private bool _HasTag;
        private bool _HadTag;
        private byte[] buffer = new byte[129];
        public override string ToString() { return FilePath; }
        public ID3(string _FilePath)
        {
            if (!(System.IO.File.Exists(_FilePath)))
            {
                throw new System.IO.FileNotFoundException("File Not Found", _FilePath);
            }
            this.FilePath = _FilePath;
            ReadID3();
        }
        public void CollectData()
        {
            Array.Clear(buffer, 0, 128);
            System.Text.Encoding.Default.GetBytes("TAG".ToUpper().ToCharArray()).CopyTo(buffer, 0);
            System.Text.Encoding.Default.GetBytes(_Title.ToCharArray()).CopyTo(buffer, 3);
            System.Text.Encoding.Default.GetBytes(_Artist.ToCharArray()).CopyTo(buffer, 33);
            System.Text.Encoding.Default.GetBytes(_Album.ToCharArray()).CopyTo(buffer, 63);
            System.Text.Encoding.Default.GetBytes(_Year.ToCharArray()).CopyTo(buffer, 93);
            System.Text.Encoding.Default.GetBytes(_Comment.ToCharArray()).CopyTo(buffer, 97);
            buffer[126] = Convert.ToByte(_TitleNumber);
            buffer[127] = Convert.ToByte(_Genre);
        }
        public void WriteID3()
        {
            System.IO.FileInfo mp3File = new System.IO.FileInfo(FilePath);
            if (mp3File.Extension.ToLower() != ".mp3")
            {
                throw new Exception("File extension must be MP3");
            }
            CollectData();
            System.IO.FileStream mp3Writer = mp3File.OpenWrite();
            if (_HasTag & _HadTag)
            {
                mp3Writer.Seek(-128, System.IO.SeekOrigin.End); mp3Writer.Write(buffer, 0, 128);
            }
            else if ((!_HadTag) & _HasTag)
            {
                mp3Writer.Seek(0, System.IO.SeekOrigin.End); mp3Writer.Write(buffer, 0, 128); _HadTag = true;
            }
            else if (_HadTag & (!_HasTag))
            {
                _HadTag = false; mp3Writer.SetLength(mp3Writer.Length - 128);
            }
            mp3Writer.Close();
        }
        public void ReadID3()
        {
            System.IO.FileInfo mp3File = new System.IO.FileInfo(FilePath);
            if (mp3File.Extension.ToLower() != ".mp3") { throw new Exception("File extension must be MP3"); }
            if (mp3File.Length > 128) { System.IO.Stream mp3Reader = mp3File.OpenRead(); mp3Reader.Seek(-128, System.IO.SeekOrigin.End); int i = 0; for (i = 0; i <= 127; i++) { buffer[i] = Convert.ToByte(mp3Reader.ReadByte()); } mp3Reader.Close(); }
            if (Encoding.Default.GetString(buffer, 0, 3).Equals("TAG"))
            {
                _Title = Encoding.Default.GetString(buffer, 3, 30); _Artist = Encoding.Default.GetString(buffer, 33, 30);
                _Album = Encoding.Default.GetString(buffer, 63, 30); _Year = Encoding.Default.GetString(buffer, 93, 4); _Comment = Encoding.Default.GetString(buffer, 97, 28);
                if (Convert.ToInt32(buffer[126]) <= 147) { _TitleNumber = Convert.ToInt32(buffer[126].ToString()); }
                if (Convert.ToInt32(buffer[127]) > 0) { _Genre = Convert.ToInt16(buffer[127]); }

                _HasTag = true; _HadTag = true;

            }
            else { _HasTag = false; _HasTag = false; }
        }

        #region " Public Properties "

        public string Title { get { return _Title; } set { if (value.Length > 30) { _Title = value.Substring(0, 30); } else { _Title = value; } } }

        public string Artist { get { return _Artist; } set { if (value.Length > 30) { _Artist = value.Substring(0, 30); } else { _Artist = value; } } }

        public string Album { get { return _Album; } set { if (value.Length > 30) { _Album = value.Substring(0, 30); } else { _Album = value; } } }

        public string Year { get { return _Year; } set { if (value.Length > 4) { _Year = value.Substring(0, 4); } else { _Year = value; } } }

        public string Comment { get { return _Comment; } set { if (value.Length > 28) { _Comment = value.Substring(0, 28); } else { _Comment = value; } } }

        public int TitleNumber { get { return _TitleNumber; } set { if (value < 255) { _TitleNumber = value; } } }

        public short Genre { get { return _Genre; } set { if (value < 256) { _Genre = value; } } }

        public bool HasTag { get { return _HasTag; } set { _HasTag = value; } }

        #endregion

    }
}
