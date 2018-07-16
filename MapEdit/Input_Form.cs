using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MapEdit
{
	public partial class Input_Form : Form
	{
		private new Main_Form ParentForm;
		public Vec2 CanvasSize { get; set; }

		public Input_Form(Main_Form f)
		{
			InitializeComponent();
			CanvasSize = new Vec2();
			ParentForm = f;
		}

		private void Input_Form_Load(object sender, EventArgs e)
		{
			// 表示する位置
			Left = ParentForm.Left + 30;
			Top = ParentForm.Top + 30;

			CanvasSize.Clear();

			textBox1.Text = Global.CANVAS_INIT_SIZE.x.ToString();
			textBox2.Text = Global.CANVAS_INIT_SIZE.y.ToString();

			textBox1.Select();
		}

		// 決定をクリック
		private void OKClick(object sender, EventArgs e)
		{
			if (textBox1.Text == "" || textBox2.Text == "") { return; }
			if (!int.TryParse(textBox1.Text, out int x) || !int.TryParse(textBox2.Text, out int y)) { return; }

			CanvasSize.Set(x, y);
			DialogResult = DialogResult.OK;
			Close();
		}

		// 取消をクリック
		private void CancelClick(object sender, EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
			Close();
		}
	}
}
