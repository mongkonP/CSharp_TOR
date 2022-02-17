using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MP3MNG
{
    public partial class dupAllCheck : Form
    {
        public dupAllCheck(string path)
        {
            InitializeComponent();
            dir = path;
        }
        string dir;
        string CalculateMD5(string filename)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(filename))
                {
                    var hash = md5.ComputeHash(stream);
                    return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                }
            }
        }
        private void dupAllCheck_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(dir) || !Directory.Exists(dir)) return;
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("File", typeof(string)));
            dt.Columns.Add(new DataColumn("Size", typeof(string)));
            dt.Columns.Add(new DataColumn("MD5", typeof(string)));
            string sizeMD5 = "";
           
            Task.Run(() =>
            {
                this.Invoke(new Action(() => this.Text = "Checking File"));
                Directory.GetFiles(dir, "*.mp3", SearchOption.AllDirectories).ToList<string>()
                    .ForEach(f =>
                    {
                        if (f.Length < 250)
                        {
                            this.Invoke(new Action(() => this.Text = "Add File:" + f));
                            DataRow dr = dt.NewRow();
                            dr["File"] = f;
                            dr["Size"] = new FileInfo(f).Length;
                            dr["MD5"] = CalculateMD5(f);

                            dt.Rows.Add(dr);
                        }
                       
                    });
                dataGridView1.Invoke(new Action(() =>
                {
                    dataGridView1.DataSource = dt;
                    dataGridView1.Sort(dataGridView1.Columns[2], ListSortDirection.Ascending);
                }));
                for (int i = 0; i < dataGridView1.RowCount - 1; i++)
                {
                    this.Invoke(new Action(() => this.Text = "Checking File:" + dataGridView1[0, i].Value.ToString().Trim()));
                    if (sizeMD5 != dataGridView1[1, i].Value.ToString().Trim() + dataGridView1[2, i].Value.ToString().Trim())
                    {

                        sizeMD5 = dataGridView1[1, i].Value.ToString().Trim() + dataGridView1[2, i].Value.ToString().Trim();
                    }
                    else
                    {
                        try { File.Delete(dataGridView1[0, i].Value.ToString().Trim()); }
                        catch { }
                    }

                }

                this.Invoke(new Action(() => this.Text = "Checking Dup Complete"));

            });
        }
    }
}
