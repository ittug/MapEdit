using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;

namespace MapEdit.Canvas
{
	public class CanvasData
	{
		public MapData MapData;
		public Vec2 CanvasSize { get; set; } = new Vec2();
		// スクロールによって移動したときの左上の番号
		[XmlIgnore]
		public Vec2 LeftTopPos { get; set; } = new Vec2();
		[XmlIgnore]
		public Vec2 NowCanvasSize { get; set; } = new Vec2(Global.CANVAS_INIT_SIZE);
		[XmlIgnore]
		public string SavePath { get; private set; }
		// true なら保存されている
		[XmlIgnore]
		public bool SavedFileFlag { get; private set; } = true;

		public CanvasData(Vec2 v)
		{
			CanvasSize = new Vec2(v);
			MapData = new MapData();
			InitMapData();
		}
		public CanvasData()
		{
			CanvasSize = new Vec2(0, 0);
			MapData = new MapData();
			InitMapData();
		}
		void InitMapData()
		{
			this.MapData.Clear();

			for (int i = 0; i < CanvasSize.y; i++)
			{
				var v = new List<TipData.TipData>();
				for (int j = 0; j < CanvasSize.x; j++)
				{
					v.Add(new TipData.TipData());
				}
				MapData.Data.Add(v);
			}
		}
		public void Load(string path)
		{
			if (Path.GetExtension(path) == ".xml")
			{
				LoadXml(path);
			}
			else
			{
				LoadEditXml(path);
			}

			SavePath = path;

			CanvasSize.x = MapData.Data[0].Count;
			CanvasSize.y = MapData.Data.Count;
		}
		public void LoadXml(string path)
		{
			CanvasWindowData tmp;
			using (StreamReader sr = new StreamReader(path, new UTF8Encoding(false)))
			{
				var xml = new XmlSerializer(typeof(CanvasWindowData));

				tmp = (CanvasWindowData)xml.Deserialize(sr);
			}

			var map = new MapData();
			foreach (var v in tmp.ListData)
			{
				var list = new List<TipData.TipData>();
				foreach (var v2 in v)
				{
					TipData.TipData tip;

					switch (v2.Type)
					{
						case TipData.TipInfoType.slope:
							var slope = new TipData.TipSlopeData();

							slope.Rot = new Vec3(int.Parse(v2.Data[0][1]), int.Parse(v2.Data[0][2]), int.Parse(v2.Data[0][3]));

							tip = slope;
							break;
						case TipData.TipInfoType.through:
							var through = new TipData.TipThroughData();

							through.ThroughBlockColor = Helper.GetIntToEnum<TipData.TipThroughData.ThroughColor>(int.Parse(v2.Data[0][1]));

							tip = through;
							break;
						case TipData.TipInfoType.light:
							var light = new TipData.TipLightData();

							light.Rot = new Vec3(int.Parse(v2.Data[0][1]), int.Parse(v2.Data[0][2]), int.Parse(v2.Data[0][3]));
							light.DefaultBlockColor = Helper.GetIntToEnum<TipData.TipLightData.DefaultColor>(int.Parse(v2.Data[1][1]));
							light.LightHeadAngle = Helper.GetIntToEnum<TipData.TipLightData.HeadAngle>(int.Parse(v2.Data[2][1]));
							light.DefaultSwitch = bool.Parse(v2.Data[3][1]);
							light.CanMove = bool.Parse(v2.Data[4][1]);
							int num = 0;
							if (int.Parse(v2.Data[5][1]).To0or1() != 0) { num |= (int)TipData.TipLightData.PossiColor.r; }
							if (int.Parse(v2.Data[5][2]).To0or1() != 0) { num |= (int)TipData.TipLightData.PossiColor.g; }
							if (int.Parse(v2.Data[5][3]).To0or1() != 0) { num |= (int)TipData.TipLightData.PossiColor.b; }
							light.PossibleCol = (TipData.TipLightData.PossibleColor)num;

							tip = light;
							break;
						default:
							tip = new TipData.TipData();
							break;
					}

					tip.ID = v2.ID;
					tip.Type = v2.Type;

					list.Add(tip);
				}
				map.Data.Add(list);
			}
			MapData = map;
		}
		public void LoadEditXml(string path)
		{
			using (StreamReader sr = new StreamReader(path, new UTF8Encoding(false)))
			{
				var xml = new XmlSerializer(typeof(MapData));

				MapData = (MapData)xml.Deserialize(sr);
			}
		}
		public void Save()
		{
			Save(SavePath);
		}
		public void Save(string path)
		{
			if (Path.GetExtension(path) == ".xml")
			{
				SaveXml(path);
			}
			else
			{
				SaveEditXml(path);
			}

			SavedFileFlag = true;
			SavePath = path;
		}
		public void SaveXml(string path)
		{
			var save = new CanvasWindowData(MapData);

			using (StreamWriter sw = new StreamWriter(path, false, new UTF8Encoding(false)))
			{
				var xml = new XmlSerializer(typeof(CanvasWindowData));
				xml.Serialize(sw, save);
			}
		}
		public void SaveEditXml(string path)
		{
			using (StreamWriter sw = new StreamWriter(path, false, new UTF8Encoding(false)))
			{
				var xml = new XmlSerializer(typeof(MapData));
				xml.Serialize(sw, MapData);
			}
		}

		public TipData.TipData GetTipData(Vec2 pos)
		{
			return MapData.GetTipData(pos);
		}
		public void SetData(Vec2 pos, TipData.TipData data)
		{
			MapData.SetData(pos, data);
			SavedFileFlag = false;
		}

	}

	// 実際にキャンバスのデータを保持するためのクラス
	public class MapData
	{
		public List<List<TipData.TipData>> Data;

		public MapData()
		{
			Data = new List<List<TipData.TipData>>();
		}
		public void Clear()
		{
			foreach (var v in Data)
			{
				v.Clear();
			}
			Data.Clear();
		}
		public TipData.TipData GetTipData(Vec2 pos)
		{
			return Data[pos.y][pos.x];
		}
		public void SetData(Vec2 pos, TipData.TipData data)
		{
			Data[pos.y][pos.x] = data;
		}
	}

	// キャンバスでの TipData についてのデータを保持する
	// ゲーム側のソースとの互換性をとるためにこのクラスに加工してからシリアライズする
	public class CanvasWindowData
	{
		public List<List<TipsData>> ListData;

		public CanvasWindowData(MapData map)
		{
			ListData = new List<List<TipsData>>();

			foreach (var v in map.Data)
			{
				List<TipsData> buffer = new List<TipsData>();
				foreach (var v2 in v)
				{
					TipsData tips = new TipsData(v2);
					buffer.Add(tips);
				}
				ListData.Add(buffer);
			}
		}
		public CanvasWindowData()
		{
		}
	}

	// TipData 一つのデータを保持するためのクラス
	public class TipsData
	{
		public int ID;
		public TipData.TipInfoType Type;
		public List<List<string>> Data;

		// ゲーム側で使用する際にどのような情報なのかで ID を割り振る
		public TipsData(TipData.TipData data)
		{
			ID = data.ID;
			Type = data.Type;

			Data = new List<List<string>>();

			List<string> buffer = new List<string>();

			if (data is TipData.TipSlopeData)
			{
				var slope = data as TipData.TipSlopeData;

				buffer.Add("0");
				buffer.Add(slope.Rot.x.ToString());
				buffer.Add(slope.Rot.y.ToString());
				buffer.Add(slope.Rot.z.ToString());
				Data.Add(buffer);
			}
			else if (data is TipData.TipThroughData)
			{
				var through = data as TipData.TipThroughData;

				buffer.Add("1");
				buffer.Add(((int)through.ThroughBlockColor).ToString());
				Data.Add(buffer);
			}
			else if (data is TipData.TipLightData)
			{
				var light = data as TipData.TipLightData;

				buffer.Add("0");
				buffer.Add(light.Rot.x.ToString());
				buffer.Add(light.Rot.y.ToString());
				buffer.Add(light.Rot.z.ToString());
				Data.Add(buffer);

				buffer = new List<string>();
				buffer.Add("2");
				buffer.Add(((int)light.DefaultBlockColor).ToString());
				Data.Add(buffer);

				buffer = new List<string>();
				buffer.Add("3");
				buffer.Add(((int)light.LightHeadAngle).ToString());
				Data.Add(buffer);

				buffer = new List<string>();
				buffer.Add("4");
				buffer.Add(light.DefaultSwitch.ToString());
				Data.Add(buffer);

				buffer = new List<string>();
				buffer.Add("5");
				buffer.Add(light.CanMove.ToString());
				Data.Add(buffer);

				buffer = new List<string>();
				buffer.Add("6");
				buffer.Add(Helper.BitAnd((int)light.PossibleCol, (int)TipData.TipLightData.PossiColor.r).ToString());
				buffer.Add(Helper.BitAnd((int)light.PossibleCol, (int)TipData.TipLightData.PossiColor.g).ToString());
				buffer.Add(Helper.BitAnd((int)light.PossibleCol, (int)TipData.TipLightData.PossiColor.b).ToString());
				Data.Add(buffer);
			}

		}
		public TipsData()
		{

		}
	}
}
