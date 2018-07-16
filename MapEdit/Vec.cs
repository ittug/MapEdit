using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapEdit
{
	public class Vec2 : ICloneable
	{
		public int x { get; set; }
		public int y { get; set; }

		public Vec2()
		{
			x = 0;
			y = 0;
		}
		public Vec2(Vec2 v)
		{
			this.x = v.x;
			this.y = v.y;
		}
		public Vec2(int x, int y)
		{
			this.x = x;
			this.y = y;
		}

		public void Set(int x, int y)
		{
			this.x = x;
			this.y = y;
		}
		public void Clear()
		{
			x = 0;
			y = 0;
		}

		public override bool Equals(object obj)
		{
			if (obj is Vec2 tmp)
			{
				return this.x == tmp.x &&
					   this.y == tmp.y;
			}
			return false;
		}

		public object Clone()
		{
			var tmp = new Vec2(this);
			return tmp;
		}

		public static Vec2 operator +(Vec2 v1, Vec2 v2)
		{
			return new Vec2(v1.x + v2.x, v1.y + v2.y);
		}
		public static Vec2 operator -(Vec2 v1, Vec2 v2)
		{
			return new Vec2(v1.x - v2.x, v1.y - v2.y);
		}
		public static Vec2 operator /(Vec2 v, int num)
		{
			return new Vec2(v.x / num, v.y / num);
		}
		public static Vec2 operator -(Vec2 v, int num)
		{
			return new Vec2(v.x - num, v.y - num);
		}
	}

	public class Vec3 : ICloneable
	{
		public int x { get; set; }
		public int y { get; set; }
		public int z { get; set; }

		public Vec3()
		{
			x = 0;
			y = 0;
			z = 0;
		}
		public Vec3(Vec3 v)
		{
			this.x = v.x;
			this.y = v.y;
			this.z = v.z;
		}
		public Vec3(int x, int y, int z)
		{
			this.x = x;
			this.y = y;
			this.z = z;
		}

		public void Set(int x, int y, int z)
		{
			this.x = x;
			this.y = y;
			this.z = z;
		}
		public void Clear()
		{
			x = 0;
			y = 0;
			z = 0;
		}

		public override bool Equals(object obj)
		{
			if (obj is Vec3)
			{
				Vec3 tmp = (Vec3)obj;

				return this.x == tmp.x &&
					   this.y == tmp.y &&
					   this.z == tmp.z;
			}
			return false;
		}
		public object Clone()
		{
			var tmp = new Vec3(this);
			return tmp;
		}

		public static Vec3 operator +(Vec3 v1, Vec3 v2)
		{
			return new Vec3(v1.x + v2.x, v1.y + v2.y, v1.z + v2.z);
		}
		public static Vec3 operator -(Vec3 v1, Vec3 v2)
		{
			return new Vec3(v1.x - v2.x, v1.y - v2.y, v1.z - v2.z);
		}
		public static Vec3 operator /(Vec3 v, int num)
		{
			return new Vec3(v.x / num, v.y / num, v.z / num);
		}
		public static Vec3 operator -(Vec3 v, int num)
		{
			return new Vec3(v.x - num, v.y - num, v.z - num);
		}
	}
}
