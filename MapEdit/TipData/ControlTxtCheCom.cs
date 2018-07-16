using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace MapEdit.TipData
{
	public class ControlTxtCheCom
	{
		private TextBox[] TxtBox;
		private CheckBox CheBox;
		private ComboBox ComBox;
		//それぞれのアクションで反映させるデータ
		private TipData ChangeTipData;
		private int TempboxSelectedIndex { get; set; } = 1;
		private int ListBox2SelectedIndex { get; set; }
		public TipData SelectedTipData => ChangeTipData;
		private LimitStrList SelectStrLimit => ChangeTipData.StringLimit[ListBox2SelectedIndex];


		public ControlTxtCheCom(TipData tip, TextBox t1, TextBox t2, TextBox t3, TextBox t4, CheckBox che, ComboBox com)
		{
			TxtBox = new TextBox[4] { t1, t2, t3, t4 };
			CheBox = che;
			ComBox = com;
			ChangeTipData = tip;

			Init();
		}
		private void Init()
		{
			for (int i = 0; i < 4; i++)
			{
				TxtBox[i].TextChanged += (object sender, EventArgs e) =>
				{
					SetTextCheckValue();
				};
			}

			CheBox.CheckedChanged += (object sender, EventArgs e) =>
			{
				if (CheBox.Checked)
				{
					CheBox.Text = "True";
				}
				else
				{
					CheBox.Text = "False";
				}

				SetTextCheckValue();
			};

			ComBox.SelectedIndexChanged += (object sender, EventArgs e) =>
			{
				SetTextCheckValue();
			};
		}

		// ListBoxに名前を追加する
		public void AddListBoxNames(ListBox list)
		{
			SelectedTipData.NameValue(list);
		}
		// 引数のデータを変更するのでCloneしたほうがいい
		public void ChangeTipDate(TipData tip, int index = 1)
		{
			ChangeTipData = tip;

			TempboxSelectedIndex = index;
			ListBox2SelectedIndex = 0;

			ControlTextCheckBox();
			ChangeTextCheckValue();
		}
		// TipData の各情報が追加されている ListBox のアイテムが変更された場合に呼ぶ
		public void ChangeViewText(object sender, EventArgs args)
		{
			var list = (ListBox)sender;

			ListBox2SelectedIndex = list.SelectedIndex;

			ControlTextCheckBox();
			ChangeTextCheckValue();
		}

		// 各コントロールにセットされた値をSelectedTipDataに反映させる
		private void SetTextCheckValue()
		{
			if (TempboxSelectedIndex < 1) { return; }
			if (ListBox2SelectedIndex == -1) { return; }
			if (SelectedTipData.StringLimit.Length == 0) { return; }
			SelectedTipData.SetValue(this, SelectStrLimit, ListBox2SelectedIndex);
		}
		// 選択されたタイプに応じて、コントロールの有効・無効を切り替える
		private void ControlTextCheckBox()
		{
			for (int i = 0; i < 4; i++)
			{
				TxtBox[i].Text = "";
			}
			//CheBox.Checked = false;
			ComBox.Items.Clear();

			if (TempboxSelectedIndex < 1) { return; }
			if (ListBox2SelectedIndex == -1) { return; }
			if (SelectedTipData.StringLimit.Length == 0) { CtlText(0); return; }
			SelectedTipData.CtlControl(this, SelectStrLimit, ListBox2SelectedIndex);
		}
		// 選択されたインデックスに応じて、値をコントロールに反映させる
		private void ChangeTextCheckValue()
		{
			if (TempboxSelectedIndex < 1) { return; }
			if (ListBox2SelectedIndex == -1) { return; }
			if (SelectedTipData.StringLimit.Length == 0) { return; }
			SelectedTipData.ChangeValue(this, SelectStrLimit, ListBox2SelectedIndex);
		}

		public void CtlComboBox(LimitStrList str, Action action)
		{
			if (str.IsEnableTrue)
			{
				CtlCombo();
			}
			else
			{
				action();
			}
		}
		public void CtlText(int value)
		{
			int i = 0;
			for (; i < value; i++)
			{
				TxtBox[i].Enabled = true;
			}
			for (; i < 4; i++)
			{
				TxtBox[i].Enabled = false;
			}

			CheBox.Enabled = false;
			ComBox.Enabled = false;
		}
		public void CtlCheck()
		{
			for (int i = 0; i < 4; i++)
			{
				TxtBox[i].Enabled = false;
			}

			CheBox.Enabled = true;
			ComBox.Enabled = false;
		}
		public void CtlCombo()
		{
			for (int i = 0; i < 4; i++)
			{
				TxtBox[i].Enabled = false;
			}

			CheBox.Enabled = false;
			ComBox.Enabled = true;
		}

		//---------------------------------
		//各ボックス等の値で変更

		public void SetComboBox<T>(LimitStrList str, ref T value, Func<T, object> action = null)
		{
			Debug.WriteLineIf(!typeof(T).IsEnum && action != null, "T が enum でないにもかかわらず action が null でした");

			if (str.IsEnableTrue)
			{
				if (ComBox.SelectedIndex == -1) { return; }

				if (typeof(T).IsClass)
				{
					value = (T)(str.Data[ComBox.SelectedIndex].Value as ICloneable).Clone();
				}
				else
				{
					value = (T)str.Data[ComBox.SelectedIndex].Value;
				}
			}
			else
			{
				if (typeof(T).IsEnum) { return; }
				value = (T)action(value);
			}
		}

		public object Set1Text(string s)
		{
			return TxtBox[0].Text;
		}
		public object Set1Int(int i)
		{
			if (!int.TryParse(TxtBox[0].Text, out int value)) { return i; }

			return value;
		}
		public object Set1Bool(bool b)
		{
			return CheBox.Checked;
		}
		public object Set2Vec(Vec2 v)
		{
			if (!int.TryParse(TxtBox[0].Text, out int x)) { return v; }
			if (!int.TryParse(TxtBox[1].Text, out int y)) { return v; }

			return new Vec2(x, y);
		}
		public object Set3Vec(Vec3 v)
		{
			if (!int.TryParse(TxtBox[0].Text, out int x)) { return v; }
			if (!int.TryParse(TxtBox[1].Text, out int y)) { return v; }
			if (!int.TryParse(TxtBox[2].Text, out int z)) { return v; }

			return new Vec3(x, y, z);
		}

		//各ボックス等の値を変更

		public void ChangeComboBox<T>(LimitStrList str, T value, Action<T> action = null)
		{
			ComBox.Items.Clear();

			if (str.IsEnableTrue)
			{
				foreach (var v in str.Data)
				{
					ComBox.Items.Add(v);
				}

				var index = str.Data.Select((s, i) => new { value = s.Value, index = i })
									.Where(s => s.value.Equals(value))
									.FirstOrDefault();

				ComBox.SelectedIndex = index.index;
			}
			else
			{
				if (typeof(T).IsEnum) { return; }
				action(value);
			}
		}

		public void Change1Text(string s)
		{
			TxtBox[0].Text = s;
		}
		public void Change1Int(int i)
		{
			TxtBox[0].Text = i.ToString();
		}

		public void Change1Bool(bool b)
		{
			CheBox.Checked = b;
		}
		public void Change2Vec(Vec2 v)
		{
			TxtBox[0].Text = v.x.ToString();
			TxtBox[0].Text = v.y.ToString();
		}
		public void Change3Vec(Vec3 v)
		{
			TxtBox[0].Text = v.x.ToString();
			TxtBox[1].Text = v.y.ToString();
			TxtBox[2].Text = v.z.ToString();
		}

		//---------------------------------
	}
}
