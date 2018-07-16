using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapEdit.Canvas
{
	/// <summary>
	/// c-z,c-y ように記憶するための構造体
	/// </summary>
	public class HistoryObj
	{
		public TipData.TipData beforeID;
		public TipData.TipData afterID;
		public Vec2 Pos = new Vec2(0, 0);
	}


	class CanvasHistory
	{
		//それぞれ値の保存は生の配列を使用し、リング状のような感じで運用する

		private HistoryObj[] Ctrl_zStack;
		private HistoryObj[] Ctrl_yStack;
		private int CtrlZIndex;
		private int CtrlYIndex;

		public CanvasHistory()
		{
			Ctrl_zStack = new HistoryObj[Global.HistoryNum];
			Ctrl_yStack = new HistoryObj[Global.HistoryNum];

			for (int i = 0; i < Global.HistoryNum; i++)
			{
				Ctrl_zStack[i] = null;
				Ctrl_yStack[i] = null;
			}

			CtrlZIndex = 0;
			CtrlYIndex = 0;
		}

		private int PlusCtrlZIndex()
		{
			return CtrlZIndex + 1 < Global.HistoryNum ? CtrlZIndex + 1 : 0;
		}
		private int MinusCtrlZIndex()
		{
			return CtrlZIndex - 1 < 0 ? Global.HistoryNum - 1 : CtrlZIndex - 1;
		}
		private int PlusCtrlYIndex()
		{
			return CtrlYIndex + 1 < Global.HistoryNum ? CtrlYIndex + 1 : 0;
		}
		private int MinusCtrlYIndex()
		{
			return CtrlYIndex - 1 < 0 ? Global.HistoryNum - 1 : CtrlYIndex - 1;
		}

		/// <summary>
		/// クリックする前のオブジェクト
		/// </summary>
		/// <param name="obj"></param>
		public void Push(HistoryObj obj)
		{
			Ctrl_zStack[CtrlZIndex] = obj;

			CtrlZIndex = PlusCtrlZIndex();

			ClearCtrl_yStack();
		}
		public HistoryObj CtrlZ()
		{
			CtrlZIndex = MinusCtrlZIndex();

			//これ以上戻る要素がない
			if (Ctrl_zStack[CtrlZIndex] == null) { return null; }

			//todo コピーするべき？
			Ctrl_yStack[CtrlYIndex] = Ctrl_zStack[CtrlZIndex];
			CtrlYIndex = PlusCtrlYIndex();

			//値を消す
			Ctrl_zStack[CtrlZIndex] = null;

			return Ctrl_yStack[MinusCtrlYIndex()];
		}
		public HistoryObj CtrlY()
		{
			CtrlYIndex = MinusCtrlYIndex();

			if (Ctrl_yStack[CtrlYIndex] == null) { return null; }

			Ctrl_zStack[CtrlZIndex] = Ctrl_yStack[CtrlYIndex];
			CtrlZIndex = PlusCtrlZIndex();

			Ctrl_yStack[CtrlYIndex] = null;

			return Ctrl_zStack[MinusCtrlZIndex()];
		}
		/// <summary>
		/// <para>c-z 操作をしていて c-y stack に値を入れていくのだが、そのタイミングで</para>
		/// <para>Draw が走ると c-y stack にあるデータが意味をなさなくなるので消す</para>
		/// </summary>
		private void ClearCtrl_yStack()
		{
			for (int i = 0; i < Global.HistoryNum; i++)
			{
				Ctrl_yStack[i] = null;
			}
			CtrlYIndex = 0;
		}
	}
}
