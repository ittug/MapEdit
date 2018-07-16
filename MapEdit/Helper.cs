using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace MapEdit
{
	static class Helper
	{
		/// <summary>
		/// ファイルの読み込んだ１行ずつに対して処理を書きたいときに使う
		/// </summary>
		/// <param name="path"></param>
		/// <param name="func">１行分のデータが来るのでそれをどうするか</param>
		public static void LoadFileReadLine(string path, Action<string> func)
		{
			if (!File.Exists(path))
			{
				using (FileStream s = File.Create(path)) { }
			}

			using (StreamReader sr = new StreamReader(path))
			{
				while (!sr.EndOfStream)
				{
					func(sr.ReadLine());
				}
			}
		}
		/// <summary>
		/// １行ずつ読み込み配列で全てを合わせて返す
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		public static IEnumerable<string> LoadFileReadLine(string path)
		{
			if (!File.Exists(path))
			{
				using (FileStream s = File.Create(path)) { }
			}

			List<string> buffer = new List<string>();
			using (StreamReader sr = new StreamReader(path))
			{
				while (!sr.EndOfStream)
				{
					buffer.Add(sr.ReadLine());
				}
			}

			return buffer;
		}
		/// <summary>
		/// .csvの読み込み
		/// 
		/// ※~~,~~,~~, のように最後に , で終わる .csv の場合は
		/// ※配列の最後に "" のデータが入るので注意
		/// </summary>
		/// <param name="path"></param>
		/// <param name="func"></param>
		public static void LoadCSV(string path, Action<string[]> func)
		{
			LoadFileReadLine(path, s =>
			{
				string[] buffer = s.Split(',');

				for (int i = 0; i < buffer.Length; i++)
				{
					buffer[i] = buffer[i].Trim('"');
				}

				func(buffer);
			});
		}
		/// <summary>
		/// 絶対パスを相対パスへ返還する
		/// </summary>
		/// <param name="absolutePath"></param>
		/// <param name="standartdPath">基準となるパス</param>
		/// <returns></returns>
		public static string AbsoluteToRelativePath(string absolutePath, string standartdPath)
		{
			Uri u1 = new Uri(standartdPath);
			Uri u2 = new Uri(absolutePath);

			//絶対Uriから相対Uriを取得する
			Uri relativeUri = u1.MakeRelativeUri(u2);
			//文字列に変換する
			return relativeUri.ToString();
		}
		/// <summary>
		/// listbox のアイテムの入れ替え
		/// </summary>
		/// <param name="list"></param>
		/// <param name="index1"></param>
		/// <param name="index2"></param>
		public static void SwapListBoxData(ListBox list, int index1, int index2)
		{
			var tmp = list.Items[index1];
			list.Items[index1] = list.Items[index2];
			list.Items[index2] = tmp;
		}
		/// <summary>
		/// Enumの名前を取得する
		/// </summary>
		/// <typeparam name="Type"></typeparam>
		/// <param name="t"></param>
		/// <returns></returns>
		public static string GetEnumName<Type>(Type t)
		{
			return Enum.GetName(typeof(Type), t);
		}
		/// <summary>
		/// int から対象の enum 型に変換
		/// </summary>
		/// <typeparam name="Type"></typeparam>
		/// <param name="num"></param>
		/// <returns></returns>
		public static Type GetIntToEnum<Type>(int num)
		{
			return (Type)Enum.ToObject(typeof(Type), num);
		}
		/// <summary>
		/// Enumの項目数を取得する
		/// </summary>
		/// <typeparam name="Type"></typeparam>
		/// <returns></returns>
		public static int GetEnumCount<Type>()
		{
			return Enum.GetNames(typeof(Type)).Length;
		}

		/// <summary>
		/// target & cond した結果をbool値として、=0なら0,=!0ならば1
		/// </summary>
		/// <param name="target"></param>
		/// <param name="cond"></param>
		/// <returns></returns>
		public static int BitAnd(int target, int cond)
		{
			return (target & cond).To0or1();
		}
		/// <summary>
		/// bool から int 辺変換
		/// </summary>
		/// <param name="b"></param>
		/// <returns>b が true なら 1,false なら 0</returns>
		public static int ToInt(this bool b)
		{
			return b ? 1 : 0;
		}
		/// <summary>
		/// 引数を 0 or 1 に変換する
		/// </summary>
		/// <param name="i"></param>
		/// <returns>i が 0 なら 0,それ以外なら 1 を返す</returns>
		public static int To0or1(this int i)
		{
			return i == 0 ? 0 : 1;
		}
		/// <summary>
		/// int 型を bool に変換
		/// </summary>
		/// <param name="i"></param>
		/// <returns></returns>
		public static bool ToBool(this int i)
		{
			return i == 0 ? false : true;
		}
	}
}
