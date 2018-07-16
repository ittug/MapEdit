using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MapEdit
{
	class Global
	{
		public static readonly int IMG_SIZE = 32;
		public static readonly Vec2 CANVAS_INIT_SIZE = new Vec2(10,	10);
		public static readonly string IMG_PATH = "Data/Img.xml";

		//c-z,c-y で戻る・進むことができる数
		public static readonly int HistoryNum = 250;

		public static readonly int ONE_COLOR_SIZE = 12;

		public static OpenFileDialog Ofd { get; set; }
		public static OpenFileDialog OfdPic { get; set; }
		public static SaveFileDialog Sfd { get; set; }
	}
}

