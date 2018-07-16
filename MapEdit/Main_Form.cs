using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace MapEdit
{
	public partial class Main_Form : Form
	{
		private Input_Form InputForm;
		private List<Canvas.Canvas_Form> FormList;
		private TipData.TipInfo_Form TipInfoForm;
		public ImageMng ImgMng { get; }
		public TipData.TipData SelectTipData { get; private set; }
		private string LastOpenFile = "";
		//選択されたチップに応じてテキストボックス等の表示を変更する
		private TipData.ControlTxtCheCom CtlTextCheckBox;


		public Main_Form()
		{
			InitializeComponent();

			InputForm = new Input_Form(this);
			FormList = new List<Canvas.Canvas_Form>();
			TipInfoForm = new TipData.TipInfo_Form(this);
			ImgMng = new ImageMng();
			SelectTipData = new TipData.TipData();

			CtlTextCheckBox = new TipData.ControlTxtCheCom(SelectTipData,
				textBox1, textBox2, textBox3, textBox4, checkBox1, comboBox1);

			InitDialog();
			ImgMng.InitLoad(TipInfoForm);
			AddTempDataBoxItems();

			Canvas.Canvas_Form.ClosingFormEvent += DeleteCanvasWindowForm;
			MouseWheel += FormMouseWheel;
		}
		// Dialog系の初期化
		public void InitDialog()
		{
			Global.Ofd = openFileDialog1;
			Global.Ofd.InitialDirectory = Directory.GetCurrentDirectory();

			Global.Sfd = saveFileDialog1;
			Global.Sfd.InitialDirectory = Directory.GetCurrentDirectory();

			Global.OfdPic = openFileDialog2;
			Global.OfdPic.InitialDirectory = Directory.GetCurrentDirectory() + "\\Data\\Img\\";
		}
		// TempBoxにデータを追加していく
		private void AddTempDataBoxItems()
		{
			TempDataBox.Items.Clear();

			foreach (var v in TipInfoForm.InfoDataList)
			{
				foreach (var v2 in v.TempDataList.DataList)
				{
					TempDataBox.Items.Add(v2.Name);
				}
			}
		}
		private void Main_Form_Load(object sender, EventArgs e)
		{
		}
		private void 新規作成NToolStripMenuItem_Click(object sender, EventArgs e)
		{
			var tmp = TopMost;
			TopMost = false;
			if (InputForm.ShowDialog() != DialogResult.OK) { TopMost = tmp; return; }
			TopMost = tmp;

			var f = new Canvas.Canvas_Form(this, InputForm.CanvasSize);
			f.MouseWheel += FormMouseWheel;
			f.Show();
			f.ParentsListIndex = FormList.Count;

			FormList.Add(f);
		}
		private void 開くOToolStripMenuItem_Click(object sender, EventArgs e)
		{
			openFileDialog1.InitialDirectory = LastOpenFile;
			if (string.IsNullOrEmpty(LastOpenFile))
			{
				openFileDialog1.InitialDirectory = Directory.GetCurrentDirectory();
			}

			if (openFileDialog1.ShowDialog() == DialogResult.Cancel) { return; }

			LastOpenFile = Path.GetDirectoryName(openFileDialog1.FileName);

			var tmp = new Canvas.CanvasData();
			tmp.Load(openFileDialog1.FileName);

			var form = new Canvas.Canvas_Form(this, tmp);
			// Canvas_Form がアクティブ時にでもホイールで
			// TempBoxの選択アイテムが上下移動してほしため追加
			form.MouseWheel += FormMouseWheel;
			form.Show();

			// Canvas_Form がクローズするときに、削除する対象が
			// リストのどこにいるか知る必要があるため
			form.ParentsListIndex = FormList.Count;

			FormList.Add(form);
		}
		private void マップチップの追加DToolStripMenuItem_Click(object sender, EventArgs e)
		{
			TipInfoForm.ShowDialog();
			AddTempDataBoxItems();
			ImgMng.InitLoad(TipInfoForm);
		}
		private void 終了XToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Close();
		}
		private void 最前面に設定ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			TopMost = !TopMost;

			if (TopMost)
			{
				var v = (ToolStripMenuItem)menuStrip1.Items[1];
				v.DropDownItems[0].Text = "最前面を解除(&T)";
				menuStrip1.Items.RemoveAt(1);
				menuStrip1.Items.Add(v);
			}
			else
			{
				var v = (ToolStripMenuItem)menuStrip1.Items[1];
				v.DropDownItems[0].Text = "最前面に設定(&T)";
				menuStrip1.Items.RemoveAt(1);
				menuStrip1.Items.Add(v);
			}
		}
		// Canvas_Form が存在する場合はそれぞれの Close() を呼び出す
		private void Main_Form_FormClosing(object sender, FormClosingEventArgs e)
		{
			int loop = 0;
			// Canvas_Form が閉じるときに For で回している List から削除されるため
			// ループ変数をずらさないと実行されないインスタンスが出てくるため
			Action closed = () =>
			{
				loop--;
			};
			// Canvas_Form が終了時のメッセージボックスでキャンセルを押した場合に
			// Main_Form もクローズをキャンセルする
			Action cancel = () =>
			{
				e.Cancel = true;
			};

			Canvas.Canvas_Form.ClosedFormEvent += closed;
			Canvas.Canvas_Form.ClosingCancelEvent += cancel;

			for (loop = 0; loop < FormList.Count; loop++)
			{
				if (FormList[loop].SavedFileFlag) { continue; }
				FormList[loop].Activate();
				FormList[loop].Close();
			}

			Canvas.Canvas_Form.ClosedFormEvent -= closed;
			Canvas.Canvas_Form.ClosingCancelEvent -= cancel;
		}
		// 指定された index のデータを FormList から削除する
		private void DeleteCanvasWindowForm(int index)
		{
			FormList.RemoveAt(index);
			AllocateIndexCanvasWindowForm();
		}
		// それぞれのフォームが FormList のどの位置にいるのかを知らせる
		private void AllocateIndexCanvasWindowForm()
		{
			int index = 0;
			foreach (var v in FormList)
			{
				v.ParentsListIndex = index;
				index++;
			}
		}
		// TempBox の Selectedindex をホイールで制御する
		private void FormMouseWheel(object sender, MouseEventArgs e)
		{
			if (0 < e.Delta)
			{
				TempDataBox.SelectedIndex = Math.Max(TempDataBox.SelectedIndex - 1, 0);
			}
			else
			{
				TempDataBox.SelectedIndex = Math.Min(TempDataBox.SelectedIndex + 1, TempDataBox.Items.Count - 1);
			}
		}
		// TempBox で選択した TipData を SelectTipData に Clone する
		private void TempDataBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (IsTmpDataUnSelected) { return; }

			SelectTipData = (GetTempDataSelectedItem().Data.Clone()) as TipData.TipData;
			CtlTextCheckBox.ChangeTipDate(SelectTipData, TempDataBox.SelectedIndex);
			CtlTextCheckBox.AddListBoxNames(listBox2);
		}
		// 
		private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
		{
			// listbox2 が変更された際に描画する TipData を変更する
			CtlTextCheckBox.ChangeViewText(sender, e);
		}

		private bool IsTmpDataUnSelected => TempDataBox.SelectedIndex == -1;
		// 選択した TipData を TipInfoForm.InfoDataList から抽出する
		private TipData.TempTipData GetTempDataSelectedItem()
		{
			int index = TempDataBox.SelectedIndex;
			int cnt = 0;
			foreach (var v in TipInfoForm.InfoDataList)
			{
				foreach (var v2 in v.TempDataList.DataList)
				{
					if (index == cnt) { return v2; }
					cnt++;
				}
			}

			return null;
		}
	}
}
