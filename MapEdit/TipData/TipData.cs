using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Windows.Forms;
using System.Drawing;

namespace MapEdit.TipData
{
	public enum TipInfoType
	{
		none,
		slope,//坂
		through,//透過ブロック
		light,//投影機
		_End,//これが最後
	}

	//入力する値に制限を設けたときに使用する
	public class LimitStr
	{
		public string Name;//ComboBoxに表示される値
		public object Value;

		public LimitStr(string name, object value)
		{
			Name = name;
			Value = value;
		}
		public LimitStr()
		{
			Name = "";
			Value = null;
		}

		public override string ToString()
		{
			return Name;
		}
	}

	public class LimitStrList
	{
		public List<LimitStr> Data;

		public LimitStrList(params LimitStr[] data)
		{
			Data = new List<LimitStr>();

			foreach (var v in data)
			{
				Data.Add(v);
			}
		}
		public LimitStrList()
		{
			Data = new List<LimitStr>();
		}

		public bool IsEnableTrue => Data.Count != 0;
	}

	//読込・出力の際に使用する型
	[XmlInclude(typeof(TipSlopeData))]
	[XmlInclude(typeof(TipThroughData))]
	[XmlInclude(typeof(TipLightData))]
	public class TipData
	{
		public int ID;
		public TipInfoType Type;
		[XmlIgnore]
		public LimitStrList[] StringLimit;
		public bool FripUpDownSide = false;
		public bool FripRightLeftSide = false;

		public TipData()
		{
			Init();
		}
		public TipData(int iD)
		{
			Init();

			this.ID = iD;
		}
		public virtual void Init()
		{
			Type = TipInfoType.none;

			StringLimit = new LimitStrList[0];
		}

		public virtual object Clone()
		{
			var tmp = new TipData();

			tmp.ID = this.ID;
			tmp.FripRightLeftSide = FripRightLeftSide;
			tmp.FripUpDownSide = FripUpDownSide;

			return tmp;
		}
		/// <summary>
		/// 同一ならばTrue
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		public virtual bool IsSame(TipData data)
		{
			return this.ID == data.ID;
		}

		public virtual void ChangeValue(ControlTxtCheCom ctl, LimitStrList strLimit, int index)
		{
			switch (index)
			{
				default:
					break;
			}
		}
		// コントロールに表示されている値が変更されたときに実際にデータを変更する
		public virtual void SetValue(ControlTxtCheCom ctl, LimitStrList strLimit, int index)
		{
			switch (index)
			{
				default:
					break;
			}
		}
		// どのコントロールを有効にするか ex)textbox or combobox etc...
		public virtual void CtlControl(ControlTxtCheCom ctl, LimitStrList strLimit, int index)
		{
			switch (index)
			{
				default:
					break;
			}
		}
		// TipData が選択された際に List に表示する文字を設定する
		public virtual void NameValue(ListBox list)
		{
			list.Items.Clear();
		}
		// 描画を行う
		public virtual void Draw(Graphics graphics, Rectangle rect, Image img)
		{
			if (this.FripRightLeftSide) { img.RotateFlip(RotateFlipType.Rotate180FlipX); }
			if (this.FripUpDownSide) { img.RotateFlip(RotateFlipType.Rotate180FlipY); }

			graphics.DrawImage(img, rect);
		}
	}


	public class TipSlopeData : TipData
	{
		public enum SlopeIndex
		{
			rot,    // vec3
			_End
		}

		public Vec3 Rot;

		public TipSlopeData()
		{
			Init();
		}
		public TipSlopeData(int id)
		{
			Init();

			this.ID = id;
		}
		public override void Init()
		{
			Rot = new Vec3(0, 0, 0);

			Type = TipInfoType.slope;

			StringLimit = new LimitStrList[(int)SlopeIndex._End]{
				new LimitStrList(new LimitStr("左向き(0,180,0)",new Vec3(0,180,0)),
								 new LimitStr("右向き(0,0,0)",new Vec3(0,0,0)),
								 new LimitStr("左向き(0,-90,0)",new Vec3(0,-90,0)),
								 new LimitStr("右向き(0,90,0)",new Vec3(0,90,0)))
			};
		}

		public override object Clone()
		{
			var tmp = new TipSlopeData();

			tmp.ID = this.ID;
			tmp.Type = Type;

			tmp.FripRightLeftSide = FripRightLeftSide;
			tmp.FripUpDownSide = FripUpDownSide;

			tmp.Rot = new Vec3(Rot);

			return tmp;
		}
		public override bool IsSame(TipData data)
		{
			if (!(data is TipSlopeData)) { return false; }
			var slope = data as TipSlopeData;
			return ID == data.ID &&
				   Rot.Equals(slope.Rot);
		}

		public override void ChangeValue(ControlTxtCheCom ctl, LimitStrList strLimit, int index)
		{
			switch ((SlopeIndex)index)
			{
				case SlopeIndex.rot:
					ctl.ChangeComboBox(strLimit, (ctl.SelectedTipData as TipSlopeData).Rot, ctl.Change3Vec);
					break;
				default:
					break;
			}
		}
		public override void SetValue(ControlTxtCheCom ctl, LimitStrList strLimit, int index)
		{
			switch ((SlopeIndex)index)
			{
				case SlopeIndex.rot:
					ctl.SetComboBox(strLimit, ref (ctl.SelectedTipData as TipSlopeData).Rot, ctl.Set3Vec);
					break;
				default:
					break;
			}
		}
		public override void CtlControl(ControlTxtCheCom ctl, LimitStrList strLimit, int index)
		{
			switch ((SlopeIndex)index)
			{
				case SlopeIndex.rot:
					ctl.CtlComboBox(strLimit, () => ctl.CtlText(3));
					break;
				default:
					break;
			}
		}
		public override void NameValue(ListBox list)
		{
			list.Items.Clear();

			list.Items.Add(Helper.GetEnumName(TipSlopeData.SlopeIndex.rot));
		}
	}
	public class TipThroughData : TipData
	{
		public enum ThroughIndex
		{
			throughColor,   // enum
			_End
		}

		public enum ThroughColor
		{
			red,
			green,
			blue,
			yellow,
			cyan,
			magenta,
			white,
		}

		public ThroughColor ThroughBlockColor;
		// 単色の画像を定義する
		private static readonly List<Bitmap> TrustImage;


		public TipThroughData()
		{
			Init();
		}
		public TipThroughData(int id)
		{
			Init();

			ID = id;
		}
		static TipThroughData()
		{
			TrustImage = new List<Bitmap>();

			Graphics g;

			void Func(Brush b)
			{
				var img = new Bitmap(Global.ONE_COLOR_SIZE, Global.ONE_COLOR_SIZE);
				g = Graphics.FromImage(img);
				g.FillRectangle(b, g.VisibleClipBounds);
				TrustImage.Add(img);
			}

			Func(Brushes.Red);
			Func(Brushes.Green);
			Func(Brushes.Blue);
			Func(Brushes.Yellow);
			Func(Brushes.Cyan);
			Func(Brushes.Magenta);
			Func(Brushes.White);

			g.Dispose();
		}
		public override void Init()
		{
			ThroughBlockColor = ThroughColor.red;

			Type = TipInfoType.through;

			StringLimit = new LimitStrList[(int)ThroughIndex._End] {
				 new LimitStrList(new LimitStr("赤",ThroughColor.red),
								  new LimitStr("緑",ThroughColor.green),
								  new LimitStr("青",ThroughColor.blue),
								  new LimitStr("黄",ThroughColor.yellow),
								  new LimitStr("シアン",ThroughColor.cyan),
								  new LimitStr("マゼンタ",ThroughColor.magenta),
								  new LimitStr("白",ThroughColor.white)),
			};
		}
		public override object Clone()
		{
			TipThroughData tmp = new TipThroughData();

			tmp.ID = ID;
			tmp.Type = Type;

			tmp.FripRightLeftSide = FripRightLeftSide;
			tmp.FripUpDownSide = FripUpDownSide;

			tmp.ThroughBlockColor = ThroughBlockColor;

			return tmp;
		}
		public override bool IsSame(TipData data)
		{
			if (!(data is TipThroughData)) { return false; }
			var through = data as TipThroughData;
			return ID == data.ID &&
				   ThroughBlockColor == through.ThroughBlockColor;
		}
		public override void ChangeValue(ControlTxtCheCom ctl, LimitStrList strLimit, int index)
		{
			switch ((ThroughIndex)index)
			{
				case ThroughIndex.throughColor:
					ctl.ChangeComboBox(strLimit, (ctl.SelectedTipData as TipThroughData).ThroughBlockColor);
					break;
				default:
					break;
			}
		}
		public override void SetValue(ControlTxtCheCom ctl, LimitStrList strLimit, int index)
		{
			switch ((ThroughIndex)index)
			{
				case ThroughIndex.throughColor:
					ctl.SetComboBox(strLimit, ref ((ctl.SelectedTipData as TipThroughData).ThroughBlockColor));
					break;
				default:
					break;
			}
		}
		public override void CtlControl(ControlTxtCheCom ctl, LimitStrList strLimit, int index)
		{
			switch ((ThroughIndex)index)
			{
				case ThroughIndex.throughColor:
					ctl.CtlComboBox(strLimit, () => ctl.CtlText(1));
					break;
				default:
					break;
			}
		}
		public override void NameValue(ListBox list)
		{
			list.Items.Clear();

			list.Items.Add(Helper.GetEnumName(TipThroughData.ThroughIndex.throughColor));
		}
		public override void Draw(Graphics graphics, Rectangle rect, Image img)
		{
			img = TrustImage[(int)ThroughBlockColor];
			graphics.DrawImage(img, rect);
		}
	}
	public class TipLightData : TipData
	{
		public enum LightIndex
		{
			rot,            // vec3
			defaultColor,   // enum
			headAngle,      // enum
			defaultSwitch,  // bool
			canMove,        // bool
			possibleCol,    // enum
			_End
		}

		public enum DefaultColor
		{
			red,
			green,
			blue,
		}

		public enum HeadAngle
		{
			up,
			middle,
			down,
		}

		// 基本の色
		public enum PossiColor
		{
			r = 0x4,
			g = 0x2,
			b = 0x1,
		}
		// 切り替え可能な色を定義する
		public enum PossibleColor
		{
			r = PossiColor.r,
			rg = PossiColor.r | PossiColor.g,
			rgb = PossiColor.r | PossiColor.g | PossiColor.b,
			g = PossiColor.g,
			gb = PossiColor.g | PossiColor.b,
			b = PossiColor.b,
		}

		public Vec3 Rot;
		public DefaultColor DefaultBlockColor;
		public HeadAngle LightHeadAngle;
		public bool DefaultSwitch;
		public bool CanMove;
		public PossibleColor PossibleCol;

		// 単色の画像を定義する
		private static readonly List<Bitmap> TrustImage;
		// このライト（射影器）が変更可能な色を視覚的に描画するためもの
		private static readonly Dictionary<PossibleColor, Bitmap> CanChangeColorImage;

		public TipLightData()
		{
			Init();
		}
		public TipLightData(int id)
		{
			Init();

			this.ID = id;
		}
		static TipLightData()
		{
			TrustImage = new List<Bitmap>();
			CanChangeColorImage = new Dictionary<PossibleColor, Bitmap>();

			CreateTrustImage();
			CreateCanChangeColorImage();
		}
		private static void CreateTrustImage()
		{
			Graphics g;

			void Func(Brush b)
			{
				var img = new Bitmap(Global.ONE_COLOR_SIZE, Global.ONE_COLOR_SIZE);
				g = Graphics.FromImage(img);
				g.FillRectangle(b, g.VisibleClipBounds);
				TrustImage.Add(img);
			}

			Func(Brushes.Red);
			Func(Brushes.Green);
			Func(Brushes.Blue);

			g.Dispose();
		}
		private static void CreateCanChangeColorImage()
		{
			//各色の画像を生成

			var R = new Bitmap(10, 3);
			var g = Graphics.FromImage(R);
			g.FillRectangle(Brushes.Red, g.VisibleClipBounds);

			var G = new Bitmap(10, 3);
			g = Graphics.FromImage(G);
			g.FillRectangle(Brushes.Green, g.VisibleClipBounds);

			var B = new Bitmap(10, 3);
			g = Graphics.FromImage(B);
			g.FillRectangle(Brushes.Blue, g.VisibleClipBounds);

			g.Dispose();

			var rRect = new Rectangle(0, 0, 10, 4);
			var gRect = new Rectangle(0, 4, 10, 4);
			var bRect = new Rectangle(0, 8, 10, 4);

			Graphics g2;
			// 100・・赤、010・・緑、001・・青のように各ビットが描画可能な色を表している
			// そして各ビットが立っているならばそれぞれの位置に画像を描画する
			void func(PossibleColor color)
			{
				var img = new Bitmap(10, 12);
				img.MakeTransparent();
				g2 = Graphics.FromImage(img);

				if (Helper.BitAnd((int)color, (int)PossiColor.r).ToBool())
				{
					g2.DrawImage(R, rRect);
				}
				if (Helper.BitAnd((int)color, (int)PossiColor.g).ToBool())
				{
					g2.DrawImage(G, gRect);
				}
				if (Helper.BitAnd((int)color, (int)PossiColor.b).ToBool())
				{
					g2.DrawImageUnscaledAndClipped(B, bRect);
				}

				CanChangeColorImage[color] = img;
			};

			func(PossibleColor.r);
			func(PossibleColor.rg);
			func(PossibleColor.rgb);
			func(PossibleColor.g);
			func(PossibleColor.gb);
			func(PossibleColor.b);

			g2.Dispose();
		}
		public override void Init()
		{
			Rot = new Vec3(0, 90, 0);
			DefaultBlockColor = DefaultColor.red;
			LightHeadAngle = HeadAngle.middle;
			DefaultSwitch = false;
			CanMove = false;

			Type = TipInfoType.light;

			PossibleCol = PossibleColor.rgb;

			StringLimit = new LimitStrList[(int)LightIndex._End] {
					new LimitStrList(new LimitStr("左向き",new Vec3(0,-90,0)),
									 new LimitStr("右向き",new Vec3(0,90,0))),
					new LimitStrList(new LimitStr("赤",DefaultColor.red),
									 new LimitStr("緑",DefaultColor.green),
									 new LimitStr("青",DefaultColor.blue)),
					new LimitStrList(new LimitStr("上向き",HeadAngle.up),
									 new LimitStr("右向き",HeadAngle.middle),
									 new LimitStr("下向き",HeadAngle.down)),
					new LimitStrList(),
					new LimitStrList(),
					new LimitStrList(new LimitStr("赤",PossibleColor.r),
									 new LimitStr("赤緑",PossibleColor.rg),
									 new LimitStr("赤緑青",PossibleColor.rgb),
									 new LimitStr("緑",PossibleColor.g),
									 new LimitStr("緑青",PossibleColor.gb),
									 new LimitStr("青",PossibleColor.b))
			};
		}
		public override object Clone()
		{
			var tmp = new TipLightData();

			tmp.ID = ID;
			tmp.Type = Type;

			tmp.FripRightLeftSide = FripRightLeftSide;
			tmp.FripUpDownSide = FripUpDownSide;

			tmp.Rot = new Vec3(Rot);
			tmp.DefaultBlockColor = DefaultBlockColor;
			tmp.LightHeadAngle = LightHeadAngle;
			tmp.DefaultSwitch = DefaultSwitch;
			tmp.CanMove = CanMove;

			tmp.PossibleCol = PossibleCol;

			return tmp;
		}
		public override bool IsSame(TipData data)
		{
			if (!(data is TipLightData)) { return false; }
			var light = data as TipLightData;
			return ID == data.ID &&
				   Rot.Equals(light.Rot) &&
				   DefaultBlockColor == light.DefaultBlockColor &&
				   LightHeadAngle == light.LightHeadAngle &&
				   DefaultSwitch == light.DefaultSwitch &&
				   CanMove == light.CanMove &&
				   PossibleCol == light.PossibleCol;
		}
		public override void ChangeValue(ControlTxtCheCom ctl, LimitStrList strLimit, int index)
		{
			switch ((LightIndex)index)
			{
				case LightIndex.rot:
					ctl.ChangeComboBox(strLimit, (ctl.SelectedTipData as TipLightData).Rot, ctl.Change3Vec);
					break;
				case LightIndex.defaultColor:
					ctl.ChangeComboBox(strLimit, (ctl.SelectedTipData as TipLightData).DefaultBlockColor);
					break;
				case LightIndex.headAngle:
					ctl.ChangeComboBox(strLimit, (ctl.SelectedTipData as TipLightData).LightHeadAngle);
					break;
				case LightIndex.defaultSwitch:
					ctl.ChangeComboBox(strLimit, (ctl.SelectedTipData as TipLightData).DefaultSwitch, ctl.Change1Bool);
					break;
				case LightIndex.canMove:
					ctl.ChangeComboBox(strLimit, (ctl.SelectedTipData as TipLightData).CanMove, ctl.Change1Bool);
					break;
				case LightIndex.possibleCol:
					ctl.ChangeComboBox(strLimit, (ctl.SelectedTipData as TipLightData).PossibleCol);
					break;
				default:
					break;
			}
		}
		public override void SetValue(ControlTxtCheCom ctl, LimitStrList strLimit, int index)
		{
			switch ((LightIndex)index)
			{
				case LightIndex.rot:
					ctl.SetComboBox(strLimit, ref (ctl.SelectedTipData as TipLightData).Rot, ctl.Set3Vec);
					break;
				case LightIndex.defaultColor:
					ctl.SetComboBox(strLimit, ref ((ctl.SelectedTipData as TipLightData).DefaultBlockColor));
					break;
				case LightIndex.headAngle:
					ctl.SetComboBox(strLimit, ref ((ctl.SelectedTipData as TipLightData).LightHeadAngle));
					break;
				case LightIndex.defaultSwitch:
					ctl.SetComboBox(strLimit, ref (ctl.SelectedTipData as TipLightData).DefaultSwitch,ctl.Set1Bool);
					break;
				case LightIndex.canMove:
					ctl.SetComboBox(strLimit, ref (ctl.SelectedTipData as TipLightData).CanMove, ctl.Set1Bool);
					break;
				case LightIndex.possibleCol:
					ctl.SetComboBox(strLimit, ref (ctl.SelectedTipData as TipLightData).PossibleCol);
					break;
				default:
					break;
			}

		}
		public override void CtlControl(ControlTxtCheCom ctl, LimitStrList strLimit, int index)
		{
			switch ((LightIndex)index)
			{
				case LightIndex.rot:
					ctl.CtlComboBox(ctl.SelectedTipData.StringLimit[index], () => ctl.CtlText(3));
					break;
				case LightIndex.defaultColor:
					ctl.CtlComboBox(ctl.SelectedTipData.StringLimit[index], () => ctl.CtlText(1));
					break;
				case LightIndex.headAngle:
					ctl.CtlComboBox(ctl.SelectedTipData.StringLimit[index], () => ctl.CtlText(1));
					break;
				case LightIndex.defaultSwitch:
					ctl.CtlComboBox(ctl.SelectedTipData.StringLimit[index], () => ctl.CtlCheck());
					break;
				case LightIndex.canMove:
					ctl.CtlComboBox(ctl.SelectedTipData.StringLimit[index], () => ctl.CtlCheck());
					break;
				case LightIndex.possibleCol:
					ctl.CtlComboBox(ctl.SelectedTipData.StringLimit[index], () => ctl.CtlText(1));
					break;
				default:
					break;
			}

		}
		public override void NameValue(ListBox list)
		{
			list.Items.Clear();

			list.Items.Add(Helper.GetEnumName(TipLightData.LightIndex.rot));
			list.Items.Add(Helper.GetEnumName(TipLightData.LightIndex.defaultColor));
			list.Items.Add(Helper.GetEnumName(TipLightData.LightIndex.headAngle));
			list.Items.Add(Helper.GetEnumName(TipLightData.LightIndex.defaultSwitch));
			list.Items.Add(Helper.GetEnumName(TipLightData.LightIndex.canMove));
			list.Items.Add(Helper.GetEnumName(TipLightData.LightIndex.possibleCol));
		}
		public override void Draw(Graphics graphics, Rectangle rect, Image img)
		{
			base.Draw(graphics, rect, img);

			var trustRect = new Rectangle(rect.Location, new Size(12, 12));
			var trustImg = TrustImage[(int)DefaultBlockColor];
			graphics.DrawImage(trustImg, trustRect);

			var canChangeColRect = new Rectangle(rect.Location.X + 19, rect.Location.Y, 10, 12);
			var canChangeImg = CanChangeColorImage[PossibleCol];
			graphics.DrawImage(canChangeImg, canChangeColRect);
		}
	}
}
