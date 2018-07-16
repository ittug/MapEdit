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


namespace MapEdit.Canvas
{
	public partial class Canvas_Form : Form
	{
		private CanvasData CanvasData;
		//todo　要名前改変
		private Bitmap Buffer;
		private CanvasHistory CanvasHistory;
		//マウスを押すと true、離すと false
		private bool DrawFlag = false;

		private string LastSaveFile = "";
		//Parent_Form.FormList で自分が何番目のインデックスなのか
		public int ParentsListIndex { get; set; }
		public bool SavedFileFlag
		{
			get
			{
				return CanvasData.SavedFileFlag;
			}
		}
		private new static Main_Form ParentForm;
		private Vec2 WindowPicBoxTroutSize;
		private TraslucentStruct TraslucentObj;

		public static event Action<int> ClosingFormEvent;
		public static event Action ClosedFormEvent;
		public static event Action ClosingCancelEvent;


		public Canvas_Form(Main_Form parent, Vec2 size)
		{
			InitializeComponent();
			ParentForm = parent;

			CanvasData = new CanvasData(size);

			Init();
			WindowPicBoxTroutSize = new Vec2();
			TraslucentObj = new TraslucentStruct();
			MouseWheel += DrawTraslucent;
		}
		public Canvas_Form(Main_Form parent, CanvasData data)
		{
			InitializeComponent();
			ParentForm = parent;

			CanvasData = data;

			Init();
			WindowPicBoxTroutSize = new Vec2();
			TraslucentObj = new TraslucentStruct();
			//MouseWheel += DrawTraslucent;
		}
		private void Init()
		{
			WindowPicBoxTroutSize = Global.CANVAS_INIT_SIZE;

			Buffer = new Bitmap(Global.IMG_SIZE * CanvasData.CanvasSize.x,
			Global.IMG_SIZE * CanvasData.CanvasSize.y);

			var clientSize = new Vec2(Math.Min(CanvasData.CanvasSize.x, Global.CANVAS_INIT_SIZE.x),
				Math.Min(CanvasData.CanvasSize.y, Global.CANVAS_INIT_SIZE.y));

			ClientSize = new Size(Global.IMG_SIZE * (clientSize.x + 2), Global.IMG_SIZE * (clientSize.y + 2));

			CanvasHistory = new CanvasHistory();

			ScrollBarInit();
			PicBoxInit();

			Draw();

			SetFormText();
		}
		// ウィンドウの名前をセットする
		private void SetFormText()
		{
			string name = "無題";
			if (!string.IsNullOrEmpty(CanvasData.SavePath)) { name = CanvasData.SavePath; }

			this.Text = string.Format("(w:{0},h:{1})  {2}", CanvasData.CanvasSize.x.ToString(),
				CanvasData.CanvasSize.y.ToString(), name);
		}
		//--------------------------------
		//スクロールバー

		private void ScrollBarInit()
		{
			InitHScrollValue();
			InithVScrollValue();
		}
		private void InitHScrollValue()
		{
			hScrollBar1.Left = Global.IMG_SIZE;
			hScrollBar1.Top = ClientSize.Height - Global.IMG_SIZE;
			hScrollBar1.Width = CanvasData.NowCanvasSize.x * Global.IMG_SIZE;
			hScrollBar1.Maximum = CanvasData.CanvasSize.x;
			hScrollBar1.LargeChange = CanvasData.CanvasSize.x + 1;
			hScrollBar1.Minimum = 0;

			if (hScrollBar1.Maximum <= CanvasData.NowCanvasSize.x)
			{
				hScrollBar1.LargeChange = hScrollBar1.Maximum + 1;
				hScrollBar1.Visible = false;
			}
			else
			{
				hScrollBar1.LargeChange -= CanvasData.CanvasSize.x - CanvasData.NowCanvasSize.x;
				hScrollBar1.Visible = true;
			}

			if (hScrollBar1.Maximum - (hScrollBar1.LargeChange - 1) <= hScrollBar1.Value)
			{
				hScrollBar1.Value = hScrollBar1.Maximum - (hScrollBar1.LargeChange - 1);
			}
		}
		private void InithVScrollValue()
		{
			vScrollBar1.Left = ClientSize.Width - Global.IMG_SIZE;
			vScrollBar1.Top = Global.IMG_SIZE;
			vScrollBar1.Height = CanvasData.NowCanvasSize.y * Global.IMG_SIZE;
			vScrollBar1.Maximum = CanvasData.CanvasSize.y;
			vScrollBar1.LargeChange = CanvasData.CanvasSize.y + 1;
			vScrollBar1.Minimum = 0;

			if (vScrollBar1.Maximum <= CanvasData.NowCanvasSize.y)
			{
				vScrollBar1.LargeChange = vScrollBar1.Maximum + 1;
				vScrollBar1.Visible = false;
			}
			else
			{
				vScrollBar1.LargeChange -= CanvasData.CanvasSize.y - CanvasData.NowCanvasSize.y;
				vScrollBar1.Visible = true;
			}

			if (vScrollBar1.Maximum - (vScrollBar1.LargeChange - 1) <= vScrollBar1.Value)
			{
				vScrollBar1.Value = vScrollBar1.Maximum - (vScrollBar1.LargeChange - 1);
			}
		}
		//--------------------------------
		//ピクチャボックス

		private void PicBoxInit()
		{
			pictureBox1.Location = new Point(32, 32);

			Vec2 size = new Vec2(Math.Min(CanvasData.CanvasSize.x, Global.CANVAS_INIT_SIZE.x),
				Math.Min(CanvasData.CanvasSize.y, Global.CANVAS_INIT_SIZE.y));

			pictureBox1.Size = new Size(size.x * Global.IMG_SIZE, size.y * Global.IMG_SIZE);
		}
		private void PicResize()
		{
			pictureBox1.Size = new Size(CanvasData.NowCanvasSize.x * Global.IMG_SIZE, CanvasData.NowCanvasSize.y * Global.IMG_SIZE);
		}
		// 一括描画
		private void Draw()
		{
			Graphics graphics = Graphics.FromImage(Buffer);

			var loop = new Vec2(CanvasData.NowCanvasSize);

			Rectangle drawPos;

			for (int y = 0; y < loop.y; y++)
			{
				for (int x = 0; x < loop.x; x++)
				{
					var img = ParentForm.ImgMng.GetImage(
						CanvasData.MapData.GetTipData(new Vec2(x + CanvasData.LeftTopPos.x, y + CanvasData.LeftTopPos.y)).ID);

					if (img == null) { continue; }

					var tipData = CanvasData.MapData.GetTipData(new Vec2(x + CanvasData.LeftTopPos.x, y + CanvasData.LeftTopPos.y));
					drawPos = new Rectangle(x * Global.IMG_SIZE, y * Global.IMG_SIZE, 32, 32);

					graphics.DrawImage(ParentForm.ImgMng.GetImage(0), drawPos);
					tipData.Draw(graphics, drawPos, img);

					//if (tipData.FripRightLeftSide) { img.RotateFlip(RotateFlipType.Rotate180FlipY); }
					//if (tipData.FripUpDownSide) { img.RotateFlip(RotateFlipType.Rotate180FlipX); }

					//graphics.DrawImage(img, drawPos);
				}
			}
			graphics.Dispose();
			pictureBox1.Image = Buffer;
		}
		// 変更されたデータのみ再描画を行う
		private void Draw(Vec2 pos, TipData.TipData data)
		{
			Graphics graphics = Graphics.FromImage(Buffer);

			var img = ParentForm.ImgMng.GetImage(data.ID);
			if (img == null) { return; }

			Rectangle drawRect = new Rectangle(pos.x * Global.IMG_SIZE, pos.y * Global.IMG_SIZE, 32, 32);

			data.Draw(graphics, drawRect, img);

			graphics.Dispose();
			pictureBox1.Image = Buffer;
		}
		// 特定の ID の画像のみを描画する
		private void Draw(Vec2 pos, int id)
		{
			Graphics bg = Graphics.FromImage(Buffer);

			var tmp = ParentForm.ImgMng.GetImage(id);
			if (tmp == null) { return; }

			Rectangle drawRect = new Rectangle(pos.x * Global.IMG_SIZE, pos.y * Global.IMG_SIZE, 32, 32);
			bg.DrawImage(tmp, drawRect);

			bg.Dispose();
			pictureBox1.Image = Buffer;
		}
		// 半透明で描画する
		private void DrawTraslucentImg(Vec2 pos, TipData.TipData tipData)
		{
			Graphics graphics = Graphics.FromImage(Buffer);

			var img = ParentForm.ImgMng.GetImage(tipData.ID);
			if (img == null) { return; }

			// 一旦本来画面に描画される画像を生成し、それをもとに半透明の画像を描画する
			img = CreateTipDataImage(img, tipData);

			Rectangle drawRect = new Rectangle(pos.x * Global.IMG_SIZE, pos.y * Global.IMG_SIZE, Global.IMG_SIZE, Global.IMG_SIZE);

			System.Drawing.Imaging.ColorMatrix cm = new System.Drawing.Imaging.ColorMatrix();
			// ColorMatrixの行列の値を変更して、アルファ値が0.2変更されるようにする
			cm.Matrix00 = 1;
			cm.Matrix11 = 1;
			cm.Matrix22 = 1;
			cm.Matrix33 = 0.2f;
			cm.Matrix44 = 1;

			// ImageAttributesオブジェクトの作成
			System.Drawing.Imaging.ImageAttributes ia = new System.Drawing.Imaging.ImageAttributes();
			// ColorMatrixを設定する
			ia.SetColorMatrix(cm);

			// ImageAttributesを使用して画像を描画
			graphics.DrawImage(img, drawRect, 0, 0, drawRect.Width, drawRect.Height, GraphicsUnit.Pixel, ia);

			graphics.Dispose();
			pictureBox1.Image = Buffer;
		}
		// tipData の Draw を用いて 32*32 の画像を生成する
		private Image CreateTipDataImage(Image img, TipData.TipData tipData)
		{
			var tipImage = new Bitmap(Global.IMG_SIZE, Global.IMG_SIZE);
			Graphics g = Graphics.FromImage(tipImage);
			tipData.Draw(g, new Rectangle(0, 0, Global.IMG_SIZE, Global.IMG_SIZE), img);
			return tipImage;
		}

		private void CanvasWnd_Form_Load(object sender, EventArgs e)
		{

		}

		private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
		{
			DrawFlag = true;

			ChangeImage(e);
		}
		private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
		{
			DrawTraslucent(sender, e);

			if (!DrawFlag) { return; }

			ChangeImage(e);
		}
		private void pictureBox1_MouseLeave(object sender, EventArgs e)
		{
			BeforeIDDrawNormal();
			TraslucentObj.Clear();
		}
		private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
		{
			DrawFlag = false;
		}
		private void ScrollValueChanged(object sender, EventArgs e)
		{
			CanvasData.LeftTopPos.Set(hScrollBar1.Value, vScrollBar1.Value);
			Draw();
		}

		// キャンバスがクリックされ情報が更新されたときに呼ばれる関数
		// ctl-z のために
		private void PushHistory(Vec2 troutPos)
		{
			HistoryObj obj = new HistoryObj();

			obj.beforeID = (TipData.TipData)CanvasData.MapData.GetTipData(troutPos + CanvasData.LeftTopPos).Clone();
			obj.afterID = (TipData.TipData)ParentForm.SelectTipData.Clone();
			obj.Pos.Set(troutPos.x + CanvasData.LeftTopPos.x, troutPos.y + CanvasData.LeftTopPos.y);

			CanvasHistory.Push(obj);
		}
		// ctl-y が押されたときに呼ばれる
		private void CanvasHistoryPop(Vec2 pos, TipData.TipData data)
		{
			CanvasData.SetData(pos, data);

			//描画範囲内にあるのか
			if (!(CanvasData.LeftTopPos.x <= pos.x && pos.x < CanvasData.LeftTopPos.x + CanvasData.NowCanvasSize.x &&
				CanvasData.LeftTopPos.y <= pos.y && pos.y < CanvasData.LeftTopPos.y + CanvasData.NowCanvasSize.y)) { return; }

			var troutPos = pos - CanvasData.LeftTopPos;

			Draw(troutPos, 0);
			Draw(troutPos, data);
		}
		// マウスがクリックされてデータが変更されるのかどうか
		private void ChangeImage(MouseEventArgs e)
		{
			//現在のマウスのマス目の取得
			var trout = GetTroutPos();
			if (trout == null) { return; }

			// 右クリックだった場合はデフォルトの 0 に変更する
			var selectTipData = ParentForm.SelectTipData.Clone() as TipData.TipData;
			if (e.Button == MouseButtons.Right)
			{
				selectTipData = new TipData.TipData();
			}

			// 同じものなら描画しない
			if (selectTipData.IsSame(CanvasData.MapData.GetTipData(trout + CanvasData.LeftTopPos))) { return; }

			//push していく―
			PushHistory(trout);

			//変更
			CanvasData.SetData(trout + CanvasData.LeftTopPos, selectTipData.Clone() as TipData.TipData);

			//描画
			//Draw(trout, 0);
			Draw(trout, selectTipData);
		}
		private void DrawTraslucent(object sender, MouseEventArgs e)
		{
			if (!TraslucentObj.DrawFlag) { return; }
			var NowPos = GetTroutPos();
			if (NowPos != TraslucentObj.NowPos)
			{
				TraslucentObj.BeforePos = TraslucentObj.NowPos;
				TraslucentObj.NowPos = NowPos;

				BeforeIDDrawNormal();
				SelectIDDrawTraslucent();
			}
		}
		// どのキーが入力されたかで処理を分岐
		private void CanvasWindow_Form_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Control)
			{
				HistoryObj obj;
				switch (e.KeyCode)
				{
					case Keys.S:
						SaveData();
						break;
					case Keys.Z:
						obj = CanvasHistory.CtrlZ();
						if (obj == null) { break; }
						CanvasHistoryPop(obj.Pos, obj.beforeID);
						break;
					case Keys.Y:
						obj = CanvasHistory.CtrlY();
						if (obj == null) { break; }
						CanvasHistoryPop(obj.Pos, obj.afterID);
						break;
				}
			}
		}

		private void 上書き保存FToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SaveData();
		}
		/// <summary>
		/// 直ちにデータを保存する
		/// </summary>
		/// <returns></returns>
		private bool SaveData()
		{
			if (string.IsNullOrEmpty(CanvasData.SavePath))
			{
				Global.Sfd.FileName = "hoge";
				Global.Sfd.InitialDirectory = LastSaveFile;
				if (string.IsNullOrEmpty(LastSaveFile))
				{
					Global.Sfd.InitialDirectory = Directory.GetCurrentDirectory();
				}

				if (Global.Sfd.ShowDialog() == DialogResult.Cancel) { return false; }

				LastSaveFile = Global.Sfd.FileName;

				CanvasData.Save(Global.Sfd.FileName);

				SetFormText();
			}
			else
			{
				CanvasData.Save();
			}
			return true;
		}
		private void 名前を付けて保存ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (Global.Sfd.ShowDialog() == DialogResult.Cancel) { return; }

			CanvasData.Save(Global.Sfd.FileName);

			SetFormText();
		}

		/// <summary>
		/// <para>すでに１度どこかに保存していたら、上書き保存をするかを表示</para>
		/// <para>１度も保存していないなら、名前を付けて保存するかを表示</para>
		/// </summary>
		/// <param name="e"></param>
		/// <returns>Closed へと移行する == true</returns>
		public bool SaveDataOverwriteOrSaveAs(FormClosingEventArgs e)
		{
			if (SavedFileFlag) { ClosingFormEvent?.Invoke(ParentsListIndex); return true; }

			if (string.IsNullOrEmpty(CanvasData.SavePath))
			{
				switch (MessageBox.Show("無題　変更内容を保存しますか？", "確認", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question))
				{
					case DialogResult.Yes:
						if (!SaveData())
						{
							ClosingCancelEvent?.Invoke();
							e.Cancel = true; return false;
						}
						ClosingFormEvent?.Invoke(ParentsListIndex);
						return true;
					case DialogResult.Cancel:
						ClosingCancelEvent?.Invoke();
						e.Cancel = true;
						return false;
					default:
						ClosingFormEvent?.Invoke(ParentsListIndex);
						return true;
				}
			}
			else
			{
				switch (MessageBox.Show(CanvasData.SavePath + "への変更内容を保存しますか？", "確認", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question))
				{
					case DialogResult.Yes:
						CanvasData.Save(CanvasData.SavePath);
						ClosingFormEvent?.Invoke(ParentsListIndex);
						return true;
					case DialogResult.Cancel:
						ClosingCancelEvent?.Invoke();
						e.Cancel = true;
						return false;
					default:
						ClosingFormEvent?.Invoke(ParentsListIndex);
						return true;
				}
			}
		}

		private void CanvasWindow_Form_FormClosing(object sender, FormClosingEventArgs e)
		{
			SaveDataOverwriteOrSaveAs(e);
		}
		private void CanvasWindow_Form_FormClosed(object sender, FormClosedEventArgs e)
		{
			ClosedFormEvent?.Invoke();
		}

		private void CanvasWindow_Form_Resize(object sender, EventArgs e)
		{
			CanvasData.NowCanvasSize.Set(
				Math.Min(ClientSize.Width / Global.IMG_SIZE - 2, CanvasData.CanvasSize.x),
				Math.Min(ClientSize.Height / Global.IMG_SIZE - 2, CanvasData.CanvasSize.y));
			if (CanvasData.NowCanvasSize.x <= 0) { CanvasData.NowCanvasSize.x = 1; }
			if (CanvasData.NowCanvasSize.y <= 0) { CanvasData.NowCanvasSize.y = 1; }

			Buffer.Dispose();
			Buffer = new Bitmap(CanvasData.NowCanvasSize.x * Global.IMG_SIZE,
				CanvasData.NowCanvasSize.y * Global.IMG_SIZE);

			PicResize();
			ScrollBarInit();

			CanvasData.LeftTopPos.Set(hScrollBar1.Value, vScrollBar1.Value);
			Draw();
		}

		private Vec2 GetTroutPos()
		{
			//マウスの座標
			var mousePos = pictureBox1.PointToClient(Cursor.Position);

			if (mousePos.X < 0 || mousePos.Y < 0 ||
				CanvasData.CanvasSize.x * Global.IMG_SIZE <= mousePos.X ||
				CanvasData.CanvasSize.y * Global.IMG_SIZE <= mousePos.Y) { return null; }

			var troutPos = new Vec2(mousePos.X / Global.IMG_SIZE,
				mousePos.Y / Global.IMG_SIZE);

			if (CanvasData.NowCanvasSize.x <= troutPos.x) { troutPos.x = CanvasData.NowCanvasSize.x - 1; }
			if (CanvasData.NowCanvasSize.y <= troutPos.y) { troutPos.y = CanvasData.NowCanvasSize.y - 1; }

			return troutPos;
		}
		private void SelectIDDrawTraslucent()
		{
			var trout = GetTroutPos();
			if (trout == null) { return; }

			if (ParentForm.SelectTipData.IsSame(CanvasData.MapData.GetTipData(trout + CanvasData.LeftTopPos))) { return; }

			Draw(trout, 0);
			DrawTraslucentImg(trout, ParentForm.SelectTipData);
		}
		private void BeforeIDDrawNormal()
		{
			if (TraslucentObj.NowPos != null)
			{
				var tipData = CanvasData.MapData.GetTipData(TraslucentObj.NowPos + CanvasData.LeftTopPos);
				Draw(TraslucentObj.NowPos, 0);
				Draw(TraslucentObj.NowPos, tipData);
			}
			if (TraslucentObj.BeforePos != null)
			{
				var tipData = CanvasData.MapData.GetTipData(TraslucentObj.BeforePos + CanvasData.LeftTopPos);
				Draw(TraslucentObj.BeforePos, 0);
				Draw(TraslucentObj.BeforePos, tipData);
			}
		}

		private void Canvas_Form_Deactivate(object sender, EventArgs e)
		{
			pictureBox1_MouseLeave(sender, e);
		}
		private void 名前を付けて保存ToolStripMenuItem_Click_1(object sender, EventArgs e)
		{
			if (Global.Sfd.ShowDialog() == DialogResult.Cancel) { return; }

			CanvasData.Save(Global.Sfd.FileName);

			SetFormText();
		}
		private void 最前面に表示ToolStripMenuItem_Click(object sender, EventArgs e)
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
		private void 仮描画をするToolStripMenuItem_Click(object sender, EventArgs e)
		{
			TraslucentObj.DrawFlag = !TraslucentObj.DrawFlag;

			if (TraslucentObj.DrawFlag)
			{
				var v = (ToolStripMenuItem)menuStrip1.Items[1];
				v.DropDownItems[1].Text = "仮描画をしない(&K)";
				menuStrip1.Items.RemoveAt(1);
				menuStrip1.Items.Add(v);
			}
			else
			{
				var v = (ToolStripMenuItem)menuStrip1.Items[1];
				v.DropDownItems[1].Text = "仮描画をする(&K)";
				menuStrip1.Items.RemoveAt(1);
				menuStrip1.Items.Add(v);
			}

		}
	}

	class TraslucentStruct
	{
		public Vec2 BeforePos { get; set; } = new Vec2();
		public Vec2 NowPos { get; set; } = new Vec2();
		public bool DrawFlag { get; set; } = true;

		public void Clear()
		{
			BeforePos?.Clear();
			NowPos?.Clear();
		}
	}
}
