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


namespace MapEdit.TipData
{
	public partial class TipInfo_Form : Form
	{
		public new Main_Form ParentForm;
		public TipInfoData InfoData { get; private set; }

		public int TipInfoCount => InfoData.TipInfoList.Count;
		public IReadOnlyCollection<TipInfo> InfoDataList => InfoData.TipInfoList;
		public string GetTipInfoDataPath(int index)
		{
			return InfoData.GetFilePath(index);
		}


		public TipInfo_Form(Main_Form f)
		{
			InitializeComponent();
			ParentForm = f;

			InfoData = new TipInfoData();

			LoadData();

			TmpDataBox.DoubleClick += button10_Click;

			comboBox1.Items.Clear();
			foreach (var val in Enum.GetValues(typeof(TipInfoType)))
			{
				comboBox1.Items.Add(Enum.GetName(typeof(TipInfoType), val));
			}
			// enum の末端は _End なので削除
			comboBox1.Items.RemoveAt(comboBox1.Items.Count - 1);
			comboBox1.SelectedIndex = 0;
		}

		private void TipInfo_Form_Load(object sender, EventArgs e)
		{
			Left = ParentForm.Left + 30;
			Top = ParentForm.Top + 30;

			
		}
		private void LoadData()
		{
			InfoData.Load();
			AddTipInfoBoxItem();
		}
		private void SaveData()
		{
			InfoData.Save();
		}

		private void AddTipInfoBoxItem()
		{
			TipInfoBox.Items.Clear();

			for (int i = 1; i < InfoData.TipInfoList.Count; i++)
			{
				TipInfoBox.Items.Add(InfoData.TipInfoList[i].FilePath);
			}
		}
		private void AddTmpDataBoxItem()
		{
			if (IsTipInfoBoxUnSelected) { return; }

			TmpDataBox.Items.Clear();

			int index = TipInfoBox.SelectedIndex + 1;

			foreach (var v in InfoData.TipInfoList[index].TempDataList.DataList)
			{
				TmpDataBox.Items.Add(v.Name);
			}
		}

		private void TipInfoBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			AddTmpDataBoxItem();
		}

		private void ChangeID()
		{
			for (int i = 0; i < TipInfoBox.Items.Count; i++)
			{
				InfoData.TipInfoList[i + 1].ChangeID(i + 1);
			}
		}

		private void AddInfoBoxButton(object sender, EventArgs e)
		{
			if (Global.OfdPic.ShowDialog() != DialogResult.OK) { return; }

			string path = Helper.AbsoluteToRelativePath(Global.OfdPic.FileName, Directory.GetCurrentDirectory() + "\\");

			//末端に追加
			if (IsTipInfoBoxUnSelected)
			{
				TipInfo info = new TipInfo(InfoData.TipInfoList.Count, path, (TipInfoType)comboBox1.SelectedIndex);
				InfoData.Add(info);
				TipInfoBox.Items.Add(path);
			}
			//選択の下に追加
			else
			{
				TipInfo info = new TipInfo(TipInfoBox.SelectedIndex + 1, path, (TipInfoType)comboBox1.SelectedIndex);
				TipInfoBox.Items.Insert(TipInfoBox.SelectedIndex, path);
				InfoData.Insert(TipInfoBox.SelectedIndex, info);
			}

			ChangeID();
		}
		private void ChangeInfoBoxButton(object sender, EventArgs e)
		{
			if (IsTipInfoBoxUnSelected) { return; }
			//選択中のものを FileName に
			Global.OfdPic.FileName = Path.GetFullPath(InfoData.TipInfoList[TipInfoBox.SelectedIndex + 1].FilePath);
			if (Global.OfdPic.ShowDialog() != DialogResult.OK) { return; }

			string path = Helper.AbsoluteToRelativePath(Global.OfdPic.FileName, Directory.GetCurrentDirectory() + "\\");
			TipInfo info = new TipInfo(TipInfoBox.SelectedIndex + 1, path, (int)TipInfoType.none);

			InfoData.Change(TipInfoBox.SelectedIndex, info);
			TipInfoBox.Items[TipInfoBox.SelectedIndex] = path;

			ChangeID();
		}
		private void DeleteInfoBoxButton(object sender, EventArgs e)
		{
			if (IsTipInfoBoxUnSelected) { return; }

			InfoData.RemoveAt(TipInfoBox.SelectedIndex + 1);
			TipInfoBox.Items.RemoveAt(TipInfoBox.SelectedIndex);

			ChangeID();

			TmpDataBox.Items.Clear();
			TmpDataTextBox.Text = "";
		}
		//選択しているアイテムを１つ上にずらす
		private void UpInfoBoxButton(object sender, EventArgs e)
		{
			if (IsTipInfoBoxUnSelected) { return; }
			//選択アイテムを上にずらすことができない
			if (TipInfoBox.SelectedIndex - 1 == -1) { return; }

			int index = TipInfoBox.SelectedIndex--;
			InfoData.Swap(index, index + 1);
			Helper.SwapListBoxData(TipInfoBox, index, index - 1);

			ChangeID();
		}
		//選択しているアイテムを１つ下にずらす
		private void DownInfoBoxButton(object sender, EventArgs e)
		{
			if (IsTipInfoBoxUnSelected) { return; }
			//選択アイテムを下にずらすことができない
			if (TipInfoBox.SelectedIndex + 1 == TipInfoBox.Items.Count) { return; }

			int index = TipInfoBox.SelectedIndex++;
			InfoData.Swap(index + 1, index + 2);
			Helper.SwapListBoxData(TipInfoBox, index, index + 1);

			ChangeID();
		}
		//
		private void MoveTextInfoBoxButton(object sender, EventArgs e)
		{
			if (IsTipInfoBoxUnSelected) { return; }
			if (comboBox1.Items.Count <= (int)InfoBoxSelectedItem.TipType) { return; }

			comboBox1.SelectedIndex = (int)InfoBoxSelectedItem.TipType;
		}


		private void TipInfo_Form_FormClosed(object sender, FormClosedEventArgs e)
		{
			SaveData();
		}

		private TempTipData CreateTempTipData()
		{
			TempTipData data = new TempTipData();

			data.Name = TmpDataTextBox.Text;
			data.FripUpDownSide = checkBox1.Checked;
			data.FripRightLeftSide = checkBox2.Checked;

			data.Data = TempTipData.InstanceTipData[(int)InfoBoxSelectedItem.TipType]();
			data.Data.ID = InfoBoxSelectedItem.TempDataList.ID;

			data.FripRightLeftSide = checkBox1.Checked;
			data.FripUpDownSide = checkBox2.Checked;

			return data;
		}
		private void AddTmpDataBoxButton(object sender, EventArgs e)
		{
			if (IsTipInfoBoxUnSelected) { return; }
			if (TmpDataTextBox.Text == "") { MessageBox.Show("名前を入力してください"); return; }

			var data = CreateTempTipData();

			InfoBoxSelectedItem.TempDataList.DataList.Add(data);
			TmpDataBox.Items.Add(data.Name);
		}

		private void ChangeTmpDataBoxButton(object sender, EventArgs e)
		{
			if (IsTipInfoBoxUnSelected) { return; }
			if (TmpDataTextBox.Text == "") { MessageBox.Show("名前を入力してください"); return; }

			var data = CreateTempTipData();

			InfoBoxSelectedItem.TempDataList.DataList[TmpDataBox.SelectedIndex] = data;
			TmpDataBox.Items[TmpDataBox.SelectedIndex] = data.Name;
		}

		private void DeleteTmpDataBoxButton(object sender, EventArgs e)
		{
			if (IsTipInfoBoxUnSelected) { return; }
			if (IsTmpDataBoxUnSelected) { return; }

			int TmpDataIndex = TmpDataBox.SelectedIndex;

			InfoBoxSelectedItem.TempDataList.DataList.RemoveAt(TmpDataIndex);
			TmpDataBox.Items.RemoveAt(TmpDataIndex);
		}

		private void checkBox1_CheckedChanged(object sender, EventArgs e)
		{
			if (checkBox1.Checked)
			{
				checkBox1.Text = "左右反転をやめる";
			}
			else
			{
				checkBox1.Text = "左右反転する";
			}
		}

		private void checkBox2_CheckedChanged(object sender, EventArgs e)
		{
			if (checkBox2.Checked)
			{
				checkBox2.Text = "左右反転をやめる";
			}
			else
			{
				checkBox2.Text = "左右反転する";
			}
		}

		private void button10_Click(object sender, EventArgs e)
		{
			if (IsTipInfoBoxUnSelected) { return; }
			if (IsTmpDataBoxUnSelected) { return; }
			if (InfoBoxSelectedItem.TipType == TipInfoType.none) { return; }

			TempData_Form f = new TempData_Form(
				InfoBoxSelectedItem.TempDataList.DataList[TmpDataBox.SelectedIndex].Data, this);
			f.ShowDialog();
		}


		private bool IsTipInfoBoxUnSelected => TipInfoBox.SelectedIndex == -1;
		private bool IsTmpDataBoxUnSelected => TmpDataBox.SelectedIndex == -1;
		private TipInfo InfoBoxSelectedItem => GetTipInfo(TipInfoBox.SelectedIndex + 1);
		private TipInfo GetTipInfo(int ID)
		{
			return InfoData.TipInfoList[ID];
		}

		private void TmpDataBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (IsTipInfoBoxUnSelected) { return; }
			if (IsTmpDataBoxUnSelected) { return; }

			checkBox1.Checked = InfoBoxSelectedItem.TempDataList.DataList[TmpDataBox.SelectedIndex].FripRightLeftSide;
			checkBox2.Checked = InfoBoxSelectedItem.TempDataList.DataList[TmpDataBox.SelectedIndex].FripUpDownSide;
		}
	}

}
