namespace MP3MNG
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label3 = new System.Windows.Forms.Label();
            this.button9 = new System.Windows.Forms.Button();
            this.txtPathT = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.button5 = new System.Windows.Forms.Button();
            this.txtPathS = new System.Windows.Forms.TextBox();
            this.txtCri = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.button6 = new System.Windows.Forms.Button();
            this.txtPathF = new System.Windows.Forms.TextBox();
            this.button10 = new System.Windows.Forms.Button();
            this.button14 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.button8 = new System.Windows.Forms.Button();
            this.button11 = new System.Windows.Forms.Button();
            this.button12 = new System.Windows.Forms.Button();
            this.button13 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(21, 60);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(39, 13);
            this.label3.TabIndex = 29;
            this.label3.Text = "PathT:";
            // 
            // button9
            // 
            this.button9.Location = new System.Drawing.Point(716, 54);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(52, 23);
            this.button9.TabIndex = 28;
            this.button9.Text = "...";
            this.button9.UseVisualStyleBackColor = true;
            this.button9.Click += new System.EventHandler(this.button9_Click);
            // 
            // txtPathT
            // 
            this.txtPathT.Location = new System.Drawing.Point(59, 57);
            this.txtPathT.Name = "txtPathT";
            this.txtPathT.Size = new System.Drawing.Size(650, 20);
            this.txtPathT.TabIndex = 27;
            this.txtPathT.Leave += new System.EventHandler(this.txtPathT_Leave);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(31, 112);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(22, 13);
            this.label2.TabIndex = 25;
            this.label2.Text = "Cri:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(21, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(39, 13);
            this.label1.TabIndex = 24;
            this.label1.Text = "PathS:";
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(716, 28);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(52, 23);
            this.button5.TabIndex = 21;
            this.button5.Text = "...";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // txtPathS
            // 
            this.txtPathS.Location = new System.Drawing.Point(59, 31);
            this.txtPathS.Name = "txtPathS";
            this.txtPathS.Size = new System.Drawing.Size(650, 20);
            this.txtPathS.TabIndex = 20;
            this.txtPathS.Leave += new System.EventHandler(this.txtPathS_Leave);
            // 
            // txtCri
            // 
            this.txtCri.Location = new System.Drawing.Point(59, 109);
            this.txtCri.Name = "txtCri";
            this.txtCri.Size = new System.Drawing.Size(318, 20);
            this.txtCri.TabIndex = 15;
            this.txtCri.Leave += new System.EventHandler(this.txtCri_Leave);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(21, 86);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(38, 13);
            this.label4.TabIndex = 32;
            this.label4.Text = "PathF:";
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(716, 80);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(52, 23);
            this.button6.TabIndex = 31;
            this.button6.Text = "...";
            this.button6.UseVisualStyleBackColor = true;
            // 
            // txtPathF
            // 
            this.txtPathF.Location = new System.Drawing.Point(59, 83);
            this.txtPathF.Name = "txtPathF";
            this.txtPathF.Size = new System.Drawing.Size(650, 20);
            this.txtPathF.TabIndex = 30;
            this.txtPathF.Leave += new System.EventHandler(this.txtPathF_Leave);
            // 
            // button10
            // 
            this.button10.Location = new System.Drawing.Point(776, 81);
            this.button10.Name = "button10";
            this.button10.Size = new System.Drawing.Size(156, 23);
            this.button10.TabIndex = 33;
            this.button10.Text = "Create Folder";
            this.button10.UseVisualStyleBackColor = true;
            // 
            // button14
            // 
            this.button14.Location = new System.Drawing.Point(776, 55);
            this.button14.Name = "button14";
            this.button14.Size = new System.Drawing.Size(156, 23);
            this.button14.TabIndex = 37;
            this.button14.Text = "check file dup";
            this.button14.UseVisualStyleBackColor = true;
            this.button14.Click += new System.EventHandler(this.button14_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(59, 144);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(157, 23);
            this.button1.TabIndex = 38;
            this.button1.Text = "Move File by Artist";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(418, 144);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(157, 23);
            this.button2.TabIndex = 39;
            this.button2.Text = "Move File by Artist in folder";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(241, 144);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(157, 23);
            this.button3.TabIndex = 40;
            this.button3.Text = "Move File All to Folder";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(59, 226);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(157, 23);
            this.button4.TabIndex = 41;
            this.button4.Text = "Remove Symbol";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(241, 226);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(157, 23);
            this.button7.TabIndex = 42;
            this.button7.Text = "Remove Cri";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // button8
            // 
            this.button8.Location = new System.Drawing.Point(59, 304);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(157, 23);
            this.button8.TabIndex = 43;
            this.button8.Text = "Add Cri in File @ front";
            this.button8.UseVisualStyleBackColor = true;
            this.button8.Click += new System.EventHandler(this.button8_Click);
            // 
            // button11
            // 
            this.button11.Location = new System.Drawing.Point(59, 173);
            this.button11.Name = "button11";
            this.button11.Size = new System.Drawing.Size(157, 23);
            this.button11.TabIndex = 44;
            this.button11.Text = "Move File Have Album";
            this.button11.UseVisualStyleBackColor = true;
            this.button11.Click += new System.EventHandler(this.button11_Click);
            // 
            // button12
            // 
            this.button12.Location = new System.Drawing.Point(59, 432);
            this.button12.Name = "button12";
            this.button12.Size = new System.Drawing.Size(157, 23);
            this.button12.TabIndex = 45;
            this.button12.Text = "Delete mp4 File";
            this.button12.UseVisualStyleBackColor = true;
            this.button12.Click += new System.EventHandler(this.button12_Click);
            // 
            // button13
            // 
            this.button13.Location = new System.Drawing.Point(241, 304);
            this.button13.Name = "button13";
            this.button13.Size = new System.Drawing.Size(157, 23);
            this.button13.TabIndex = 46;
            this.button13.Text = "Add Cri in File @ Back";
            this.button13.UseVisualStyleBackColor = true;
            this.button13.Click += new System.EventHandler(this.button13_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(944, 605);
            this.Controls.Add(this.button13);
            this.Controls.Add(this.button12);
            this.Controls.Add(this.button11);
            this.Controls.Add(this.button8);
            this.Controls.Add(this.button7);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.button14);
            this.Controls.Add(this.button10);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.txtPathF);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.button9);
            this.Controls.Add(this.txtPathT);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.txtPathS);
            this.Controls.Add(this.txtCri);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button9;
        private System.Windows.Forms.TextBox txtPathT;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.TextBox txtPathS;
        private System.Windows.Forms.TextBox txtCri;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.TextBox txtPathF;
        private System.Windows.Forms.Button button10;
        private System.Windows.Forms.Button button14;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.Button button11;
        private System.Windows.Forms.Button button12;
        private System.Windows.Forms.Button button13;
    }
}

