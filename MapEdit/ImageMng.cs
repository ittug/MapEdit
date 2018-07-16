using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;

namespace MapEdit
{
	public class ImageMng
	{
		// 外部から読み込む画像用
		private List<Bitmap> ImageList;

		public ImageMng()
		{
			ImageList = new List<Bitmap>();
		}

		public void Init()
		{
			ImageList.Clear();
			CreateWhiteImage();
		}
		//
		public void Load(TipData.TipInfo_Form form)
		{
			LoadImage(form);
		}
		//
		public void InitLoad(TipData.TipInfo_Form form)
		{
			Init();
			Load(form);
		}
		// 
		private void LoadImage(TipData.TipInfo_Form form)
		{
			for (int i = 1; i < form.TipInfoCount; i++)
			{
				//ファイルがないなら次のインデックスへ
				if (!File.Exists(form.GetTipInfoDataPath(i))) { ImageList.Add(ImageList[0]); continue; }
				ImageList.Add((Bitmap)Image.FromFile(form.GetTipInfoDataPath(i)));
			}
		}
		//
		public void Clear()
		{
			for (int i = 0; i < ImageList.Count; i++)
			{
				ImageList[i].Dispose();
			}
			ImageList.Clear();
		}
		// ID = 0 用の白の画像を生成する
		private void CreateWhiteImage()
		{
			var img = new Bitmap(Global.IMG_SIZE, Global.IMG_SIZE);

			var g = Graphics.FromImage(img);

			g.FillRectangle(Brushes.White, g.VisibleClipBounds);
			ImageList.Add(img);
		}

		//
		public Image GetImage(int id)
		{
			if (id < 0) { return ImageList[0]; }
			if (!(id < ImageList.Count)) { return (Image)ImageList[0].Clone(); }

			return (Image)ImageList[id].Clone();
		}
	}
}
