namespace MapEdit.TipData
{
	partial class TipInfo_Form
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
			this.button10 = new System.Windows.Forms.Button();
			this.checkBox2 = new System.Windows.Forms.CheckBox();
			this.checkBox1 = new System.Windows.Forms.CheckBox();
			this.button7 = new System.Windows.Forms.Button();
			this.button8 = new System.Windows.Forms.Button();
			this.button9 = new System.Windows.Forms.Button();
			this.TmpDataTextBox = new System.Windows.Forms.TextBox();
			this.button6 = new System.Windows.Forms.Button();
			this.comboBox1 = new System.Windows.Forms.ComboBox();
			this.TmpDataBox = new System.Windows.Forms.ListBox();
			this.button5 = new System.Windows.Forms.Button();
			this.button4 = new System.Windows.Forms.Button();
			this.button3 = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			this.button1 = new System.Windows.Forms.Button();
			this.TipInfoBox = new System.Windows.Forms.ListBox();
			this.SuspendLayout();
			// 
			// button10
			// 
			this.button10.Location = new System.Drawing.Point(418, 99);
			this.button10.Name = "button10";
			this.button10.Size = new System.Drawing.Size(49, 23);
			this.button10.TabIndex = 54;
			this.button10.Text = "詳細";
			this.button10.UseVisualStyleBackColor = true;
			this.button10.Click += new System.EventHandler(this.button10_Click);
			// 
			// checkBox2
			// 
			this.checkBox2.Appearance = System.Windows.Forms.Appearance.Button;
			this.checkBox2.AutoSize = true;
			this.checkBox2.Location = new System.Drawing.Point(418, 210);
			this.checkBox2.Name = "checkBox2";
			this.checkBox2.Size = new System.Drawing.Size(82, 22);
			this.checkBox2.TabIndex = 53;
			this.checkBox2.Text = "左右反転する";
			this.checkBox2.UseVisualStyleBackColor = true;
			this.checkBox2.CheckedChanged += new System.EventHandler(this.checkBox2_CheckedChanged);
			// 
			// checkBox1
			// 
			this.checkBox1.Appearance = System.Windows.Forms.Appearance.Button;
			this.checkBox1.AutoSize = true;
			this.checkBox1.Location = new System.Drawing.Point(418, 180);
			this.checkBox1.Name = "checkBox1";
			this.checkBox1.Size = new System.Drawing.Size(82, 22);
			this.checkBox1.TabIndex = 52;
			this.checkBox1.Text = "左右反転する";
			this.checkBox1.UseVisualStyleBackColor = true;
			this.checkBox1.Visible = false;
			this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
			// 
			// button7
			// 
			this.button7.Location = new System.Drawing.Point(418, 70);
			this.button7.Name = "button7";
			this.button7.Size = new System.Drawing.Size(49, 23);
			this.button7.TabIndex = 51;
			this.button7.Text = "削除";
			this.button7.UseVisualStyleBackColor = true;
			this.button7.Click += new System.EventHandler(this.DeleteTmpDataBoxButton);
			// 
			// button8
			// 
			this.button8.Location = new System.Drawing.Point(418, 41);
			this.button8.Name = "button8";
			this.button8.Size = new System.Drawing.Size(49, 23);
			this.button8.TabIndex = 50;
			this.button8.Text = "変更";
			this.button8.UseVisualStyleBackColor = true;
			this.button8.Click += new System.EventHandler(this.ChangeTmpDataBoxButton);
			// 
			// button9
			// 
			this.button9.Location = new System.Drawing.Point(418, 12);
			this.button9.Name = "button9";
			this.button9.Size = new System.Drawing.Size(49, 23);
			this.button9.TabIndex = 49;
			this.button9.Text = "追加";
			this.button9.UseVisualStyleBackColor = true;
			this.button9.Click += new System.EventHandler(this.AddTmpDataBoxButton);
			// 
			// TmpDataTextBox
			// 
			this.TmpDataTextBox.Location = new System.Drawing.Point(263, 248);
			this.TmpDataTextBox.Name = "TmpDataTextBox";
			this.TmpDataTextBox.Size = new System.Drawing.Size(149, 19);
			this.TmpDataTextBox.TabIndex = 48;
			// 
			// button6
			// 
			this.button6.Location = new System.Drawing.Point(208, 157);
			this.button6.Name = "button6";
			this.button6.Size = new System.Drawing.Size(49, 23);
			this.button6.TabIndex = 47;
			this.button6.Text = "転写";
			this.button6.UseVisualStyleBackColor = true;
			this.button6.Click += new System.EventHandler(this.MoveTextInfoBoxButton);
			// 
			// comboBox1
			// 
			this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBox1.FormattingEnabled = true;
			this.comboBox1.Location = new System.Drawing.Point(12, 248);
			this.comboBox1.Name = "comboBox1";
			this.comboBox1.Size = new System.Drawing.Size(190, 20);
			this.comboBox1.TabIndex = 46;
			// 
			// TmpDataBox
			// 
			this.TmpDataBox.FormattingEnabled = true;
			this.TmpDataBox.ItemHeight = 12;
			this.TmpDataBox.Location = new System.Drawing.Point(263, 12);
			this.TmpDataBox.Name = "TmpDataBox";
			this.TmpDataBox.Size = new System.Drawing.Size(149, 232);
			this.TmpDataBox.TabIndex = 45;
			this.TmpDataBox.SelectedIndexChanged += new System.EventHandler(this.TmpDataBox_SelectedIndexChanged);
			// 
			// button5
			// 
			this.button5.Location = new System.Drawing.Point(208, 128);
			this.button5.Name = "button5";
			this.button5.Size = new System.Drawing.Size(49, 23);
			this.button5.TabIndex = 44;
			this.button5.Text = "下へ";
			this.button5.UseVisualStyleBackColor = true;
			this.button5.Click += new System.EventHandler(this.DownInfoBoxButton);
			// 
			// button4
			// 
			this.button4.Location = new System.Drawing.Point(208, 99);
			this.button4.Name = "button4";
			this.button4.Size = new System.Drawing.Size(49, 23);
			this.button4.TabIndex = 43;
			this.button4.Text = "上へ";
			this.button4.UseVisualStyleBackColor = true;
			this.button4.Click += new System.EventHandler(this.UpInfoBoxButton);
			// 
			// button3
			// 
			this.button3.Location = new System.Drawing.Point(208, 70);
			this.button3.Name = "button3";
			this.button3.Size = new System.Drawing.Size(49, 23);
			this.button3.TabIndex = 42;
			this.button3.Text = "削除";
			this.button3.UseVisualStyleBackColor = true;
			this.button3.Click += new System.EventHandler(this.DeleteInfoBoxButton);
			// 
			// button2
			// 
			this.button2.Location = new System.Drawing.Point(208, 41);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(49, 23);
			this.button2.TabIndex = 41;
			this.button2.Text = "変更";
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new System.EventHandler(this.ChangeInfoBoxButton);
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(208, 12);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(49, 23);
			this.button1.TabIndex = 40;
			this.button1.Text = "追加";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.AddInfoBoxButton);
			// 
			// TipInfoBox
			// 
			this.TipInfoBox.FormattingEnabled = true;
			this.TipInfoBox.ItemHeight = 12;
			this.TipInfoBox.Location = new System.Drawing.Point(12, 12);
			this.TipInfoBox.Name = "TipInfoBox";
			this.TipInfoBox.Size = new System.Drawing.Size(190, 232);
			this.TipInfoBox.TabIndex = 39;
			this.TipInfoBox.SelectedIndexChanged += new System.EventHandler(this.TipInfoBox_SelectedIndexChanged);
			// 
			// TipInfo_Form
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(510, 278);
			this.Controls.Add(this.button10);
			this.Controls.Add(this.checkBox2);
			this.Controls.Add(this.checkBox1);
			this.Controls.Add(this.button7);
			this.Controls.Add(this.button8);
			this.Controls.Add(this.button9);
			this.Controls.Add(this.TmpDataTextBox);
			this.Controls.Add(this.button6);
			this.Controls.Add(this.comboBox1);
			this.Controls.Add(this.TmpDataBox);
			this.Controls.Add(this.button5);
			this.Controls.Add(this.button4);
			this.Controls.Add(this.button3);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.TipInfoBox);
			this.Name = "TipInfo_Form";
			this.Text = "TipInfo_Form";
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.TipInfo_Form_FormClosed);
			this.Load += new System.EventHandler(this.TipInfo_Form_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button button10;
		private System.Windows.Forms.CheckBox checkBox2;
		private System.Windows.Forms.CheckBox checkBox1;
		private System.Windows.Forms.Button button7;
		private System.Windows.Forms.Button button8;
		private System.Windows.Forms.Button button9;
		private System.Windows.Forms.TextBox TmpDataTextBox;
		private System.Windows.Forms.Button button6;
		private System.Windows.Forms.ComboBox comboBox1;
		private System.Windows.Forms.ListBox TmpDataBox;
		private System.Windows.Forms.Button button5;
		private System.Windows.Forms.Button button4;
		private System.Windows.Forms.Button button3;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.ListBox TipInfoBox;
	}
}