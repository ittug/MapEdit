using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Serialization;
using System.Windows.Forms;

namespace MapEdit.TipData
{
	public class TipInfoData
	{
		public List<TipInfo> TipInfoList { get; private set; }


		/// <summary>
		/// ファイルのパスを取得
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		public string GetFilePath(int index)
		{
			return TipInfoList[index].FilePath;
		}
		/// <summary>
		/// ディレクトリ・拡張子を除いたファイルの名前
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		public string GetFileName(int index)
		{
			return TipInfoList[index].FileName;
		}


		public TipInfoData()
		{
			TipInfoList = new List<TipInfo>();
		}
		public void Load()
		{
			try
			{
				using (StreamReader sr = new StreamReader(Global.IMG_PATH, new UTF8Encoding(false)))
				{
					var xml = new XmlSerializer(typeof(List<TipInfo>));
					TipInfoList = (List<TipInfo>)xml.Deserialize(sr);
				}
			}
			catch (Exception e)
			{
				MessageBox.Show(e.Message);
			}

			TipDefaultInfo();
		}
		public void Save()
		{
			try
			{
				using (StreamWriter sw = new StreamWriter(Global.IMG_PATH, false, new UTF8Encoding(false)))
				{
					var xml = new XmlSerializer(typeof(List<TipInfo>));
					xml.Serialize(sw, TipInfoList);
				}
			}
			catch (Exception e)
			{
				MessageBox.Show(e.Message);
			}
		}

		public void Add(TipInfo tipInfo)
		{
			TipInfoList.Add(tipInfo);
		}
		public void Insert(int index, TipInfo tipInfo)
		{
			TipInfoList.Insert(index, tipInfo);

			//挿入することでそれ以降のIDがずれてしまうので修正
			for (int i = index + 1; i < TipInfoList.Count; i++)
			{
				TipInfoList[i].ChangeID(i);
			}
		}
		public void Change(int index, TipInfo tipInfo)
		{
			TipInfoList[index] = tipInfo;
		}
		public void RemoveAt(int index)
		{
			TipInfoList.RemoveAt(index);

			//削除することでそれ以降のIDがずれてしまうので修正
			for (int i = index; i < TipInfoList.Count; i++)
			{
				TipInfoList[i].ChangeID(i);
			}
		}
		public void Swap(int index1, int index2)
		{
			var tmp = TipInfoList[index1];
			TipInfoList[index1] = TipInfoList[index2];
			TipInfoList[index2] = tmp;
		}
		public void Clear()
		{
			TipInfoList.Clear();
		}


		/// <summary>
		/// ID=0の何もない白色画像生成用
		/// </summary>
		public void TipDefaultInfo()
		{
			if (TipInfoList.Count != 0) { return; }

			var tmpData = new TempTipData(TipInfoType.none);
			tmpData.Name = "デフォルト";
			var tmp = new TipInfo(0);
			tmp.TempDataList.DataList.Add(tmpData);

			TipInfoList.Add(tmp);
		}
	}


	public class TipInfo
	{
		//画像へのパス
		public string FilePath { get; set; }
		//追加情報をあらかじめ決定しておく
		public TempTipDataList TempDataList { get; set; }
		//追加の情報
		public TipInfoType TipType;


		public string FileName => Path.GetFileNameWithoutExtension(FilePath);

		public TipInfo()
		{
			FilePath = "";
			TipType = TipInfoType.none;
			TempDataList = new TempTipDataList();
		}
		public TipInfo(int ID)
		{
			FilePath = "";
			TipType = TipInfoType.none;
			TempDataList = new TempTipDataList(ID);
		}
		public TipInfo(int ID, string path, TipInfoType type)
		{
			FilePath = path;
			TipType = type;
			TempDataList = new TempTipDataList(ID);
		}

		/// <summary>
		/// IDを変更する
		/// </summary>
		/// <param name="ID"></param>
		public void ChangeID(int ID)
		{
			TempDataList.ChangeID(ID);
		}
	}

	public class TempTipDataList
	{
		public List<TempTipData> DataList;

		public int ID;
		public int Count => DataList.Count;


		public TempTipDataList()
		{
			DataList = new List<TempTipData>();
			ChangeID(-1);
		}
		public TempTipDataList(int ID)
		{
			DataList = new List<TempTipData>();
			ChangeID(ID);
		}

		public void ChangeID(int ID)
		{
			this.ID = ID;
			foreach (var v in DataList)
			{
				v.Data.ID = ID;
			}
		}
		public void Clear()
		{
			DataList.Clear();
		}
	}
	/// <summary>
	/// ID に対しての追加情報の型をもとに、データをあらかじめ登録しておく
	/// </summary>
	public class TempTipData
	{
		public string Name { get; set; }
		public TipData Data { get; set; }
		public bool FripUpDownSide
		{
			get
			{
				return Data.FripUpDownSide;
			}
			set
			{
				Data.FripUpDownSide = value;
			}
		}
		public bool FripRightLeftSide
		{
			get
			{
				return Data.FripRightLeftSide;
			}
			set
			{
				Data.FripRightLeftSide = value;
			}
		}

		public static Func<TipData>[] InstanceTipData = new Func<TipData>[(int)TipInfoType._End]
			{
				() => new TipData(),
				() => new TipSlopeData(),
				() => new TipThroughData(),
				() => new TipLightData(),
			};


		public TempTipData()
		{
			Data = InstanceTipData[(int)TipInfoType.none]();
		}
		public TempTipData(TipInfoType type)
		{
			Data = InstanceTipData[(int)type]();
		}

	}
}
