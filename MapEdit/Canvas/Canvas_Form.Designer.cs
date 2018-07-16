namespace MapEdit.Canvas
{
	partial class Canvas_Form
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
			this.vScrollBar1 = new System.Windows.Forms.VScrollBar();
			this.hScrollBar1 = new System.Windows.Forms.HScrollBar();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.ファイルFToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.上書き保存SToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.名前を付けて保存ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.表示VToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.最前面に表示ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.仮描画をするToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.menuStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// vScrollBar1
			// 
			this.vScrollBar1.Location = new System.Drawing.Point(232, 94);
			this.vScrollBar1.Name = "vScrollBar1";
			this.vScrollBar1.Size = new System.Drawing.Size(17, 80);
			this.vScrollBar1.TabIndex = 7;
			this.vScrollBar1.ValueChanged += new System.EventHandler(this.ScrollValueChanged);
			// 
			// hScrollBar1
			// 
			this.hScrollBar1.Location = new System.Drawing.Point(39, 252);
			this.hScrollBar1.Name = "hScrollBar1";
			this.hScrollBar1.Size = new System.Drawing.Size(80, 17);
			this.hScrollBar1.TabIndex = 6;
			this.hScrollBar1.ValueChanged += new System.EventHandler(this.ScrollValueChanged);
			// 
			// pictureBox1
			// 
			this.pictureBox1.Location = new System.Drawing.Point(29, 42);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(150, 155);
			this.pictureBox1.TabIndex = 4;
			this.pictureBox1.TabStop = false;
			this.pictureBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseDown);
			this.pictureBox1.MouseLeave += new System.EventHandler(this.pictureBox1_MouseLeave);
			this.pictureBox1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseMove);
			this.pictureBox1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseUp);
			// 
			// menuStrip1
			// 
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ファイルFToolStripMenuItem,
            this.表示VToolStripMenuItem});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(370, 24);
			this.menuStrip1.TabIndex = 5;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// ファイルFToolStripMenuItem
			// 
			this.ファイルFToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.上書き保存SToolStripMenuItem,
            this.名前を付けて保存ToolStripMenuItem});
			this.ファイルFToolStripMenuItem.Name = "ファイルFToolStripMenuItem";
			this.ファイルFToolStripMenuItem.Size = new System.Drawing.Size(67, 20);
			this.ファイルFToolStripMenuItem.Text = "ファイル(&F)";
			// 
			// 上書き保存SToolStripMenuItem
			// 
			this.上書き保存SToolStripMenuItem.Name = "上書き保存SToolStripMenuItem";
			this.上書き保存SToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
			this.上書き保存SToolStripMenuItem.Text = "上書き保存(&S)";
			// 
			// 名前を付けて保存ToolStripMenuItem
			// 
			this.名前を付けて保存ToolStripMenuItem.Name = "名前を付けて保存ToolStripMenuItem";
			this.名前を付けて保存ToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
			this.名前を付けて保存ToolStripMenuItem.Text = "名前を付けて保存";
			this.名前を付けて保存ToolStripMenuItem.Click += new System.EventHandler(this.名前を付けて保存ToolStripMenuItem_Click_1);
			// 
			// 表示VToolStripMenuItem
			// 
			this.表示VToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.最前面に表示ToolStripMenuItem,
            this.仮描画をするToolStripMenuItem});
			this.表示VToolStripMenuItem.Name = "表示VToolStripMenuItem";
			this.表示VToolStripMenuItem.Size = new System.Drawing.Size(58, 20);
			this.表示VToolStripMenuItem.Text = "表示(&V)";
			// 
			// 最前面に表示ToolStripMenuItem
			// 
			this.最前面に表示ToolStripMenuItem.Name = "最前面に表示ToolStripMenuItem";
			this.最前面に表示ToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
			this.最前面に表示ToolStripMenuItem.Text = "最前面に表示";
			this.最前面に表示ToolStripMenuItem.Click += new System.EventHandler(this.最前面に表示ToolStripMenuItem_Click);
			// 
			// 仮描画をするToolStripMenuItem
			// 
			this.仮描画をするToolStripMenuItem.Name = "仮描画をするToolStripMenuItem";
			this.仮描画をするToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
			this.仮描画をするToolStripMenuItem.Text = "仮描画をする";
			this.仮描画をするToolStripMenuItem.Click += new System.EventHandler(this.仮描画をするToolStripMenuItem_Click);
			// 
			// Canvas_Form
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(370, 307);
			this.Controls.Add(this.vScrollBar1);
			this.Controls.Add(this.hScrollBar1);
			this.Controls.Add(this.pictureBox1);
			this.Controls.Add(this.menuStrip1);
			this.Name = "Canvas_Form";
			this.Text = "Canvas_Form";
			this.Deactivate += new System.EventHandler(this.Canvas_Form_Deactivate);
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CanvasWindow_Form_FormClosing);
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.CanvasWindow_Form_FormClosed);
			this.Load += new System.EventHandler(this.CanvasWnd_Form_Load);
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.CanvasWindow_Form_KeyDown);
			this.Resize += new System.EventHandler(this.CanvasWindow_Form_Resize);
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.VScrollBar vScrollBar1;
		private System.Windows.Forms.HScrollBar hScrollBar1;
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem ファイルFToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem 上書き保存SToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem 名前を付けて保存ToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem 表示VToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem 最前面に表示ToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem 仮描画をするToolStripMenuItem;
	}
}