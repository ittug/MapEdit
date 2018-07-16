using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MapEdit.TipData
{
	public partial class TempData_Form : Form
	{
		private new TipInfo_Form ParentForm;
		private ControlTxtCheCom CtlTextCheckBox;

		public TempData_Form(TipData data, TipInfo_Form f)
		{
			InitializeComponent();

			CtlTextCheckBox = new ControlTxtCheCom(data, textBox1, textBox2, textBox3, textBox4, checkBox1, comboBox1);
			CtlTextCheckBox.AddListBoxNames(listBox1);
			listBox1.SelectedIndex = 0;
			ParentForm = f;

			listBox1.SelectedIndexChanged += CtlTextCheckBox.ChangeViewText;
		}

		private void TempData_Form_Load(object sender, EventArgs e)
		{
			//表示する位置
			Left = ParentForm.Left + 30;
			Top = ParentForm.Top + 30;
		}

		//---------------------------------

		private void TempData_Form_FormClosed(object sender, FormClosedEventArgs e)
		{
			//データが１つだけだった場合に現在の方法では保存されないので
			//ChangeValues(listBox1.SelectedIndex);
		}

		private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
		{
			CtlTextCheckBox.ChangeViewText(sender, e);
		}
	}

}
