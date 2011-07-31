//StormCom Object Generator
//
//Full use to all StormCom Server System codes (in regards to minecraft classic) have been granted to McForge without restriction.
//
// ~Merlin33069

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace MCForge.Util
{
	class SCOGenerator
	{
		static Random random = new Random();
		public static double pi = 3.141592653;

		public static void AddTree(Player p, ushort x, ushort y, ushort z, byte type)
		{
			byte num = (byte)random.Next(5, 8);
			for (ushort i = 0; i < num; i = (ushort)(i + 1))
			{
				p.level.Blockchange(p, x, (ushort)(y + i), z, 0x11);
			}
			short num3 = (short)(num - random.Next(2, 4));
			for (short j = Convert.ToInt16(-num3); j <= num3; j = (short)(j + 1))
			{
				if ((x + j) < 0 || (x + j) > p.level.width) continue;
				for (short k = Convert.ToInt16(-num3); k <= num3; k = (short)(k + 1))
				{
					if ((y + k) < 0 || (y + k) > p.level.depth) continue;
					for (short m = Convert.ToInt16(-num3); m <= num3; m = (short)(m + 1))
					{
						if ((z + m) < 0 || (z + m) > p.level.length) continue;
						short maxValue = (short)Math.Sqrt((double)(((j * j) + (k * k)) + (m * m)));
						if ((maxValue < (num3 + 1)) && (random.Next(maxValue) < 2))
						{
							try
							{
								byte that = p.level.GetTile((ushort)(x + j), (ushort)((y + k) + num), (ushort)(z + m));
								if (that == 0)
								{
									p.level.Blockchange(p, (ushort)(x + j), (ushort)((y + k) + num), (ushort)(z + m), 0x12);
								}
							}
							catch
							{
							}
						}
					}
				}
			}
		}
		public static void AddCactus(Level l, ushort x, ushort y, ushort z, byte type)
		{
			ushort num2;
			byte num = (byte)random.Next(3, 6);
			for (num2 = 0; num2 <= num; num2 = (ushort)(num2 + 1))
			{
				l.Blockchange(x, (ushort)(y + num2), z, 0x19);
			}
			int num3 = 0;
			int num4 = 0;
			switch (random.Next(1, 3))
			{
				case 1:
					num3 = -1;
					break;

				default:
					num4 = -1;
					break;
			}
			num2 = num;
			while (num2 <= random.Next(num + 2, num + 5))
			{
				ushort x2 = (ushort)(x + num3); //width
				ushort y2 = (ushort)(y + num2); //depth
				ushort z2 = (ushort)(z + num4); //height
				if (l.GetTile(x2, y2, z2) == 0)
				{
					l.Blockchange((ushort)(x + num3), (ushort)(y + num2), (ushort)(z + num4), 0x19);
				}
				num2 = (ushort)(num2 + 1);
			}
			for (num2 = num; num2 <= random.Next(num + 2, num + 5); num2 = (ushort)(num2 + 1))
			{
				ushort x2 = (ushort)(x - num3); //width
				ushort y2 = (ushort)(y + num2); //depth
				ushort z2 = (ushort)(z - num4); //height
				if (l.GetTile(x2, y2, z2) == 0)
				{
					l.Blockchange((ushort)(x - num3), (ushort)(y + num2), (ushort)(z - num4), 0x19);
				}
			}
		}

		public static void Nuke(Level l, ushort x, ushort y, ushort z)
		{
			foreach (Player p in Player.players) if (p.level == l) p.SendBlockchange(x, y, z, 0);
			List<string> toair = new List<string>();

			short num3 = (short)(random.Next(15, 20));

			for (short j = Convert.ToInt16(-num3); j <= num3; j = (short)(j + 1))
			{
				if ((x + j) < 0 || (x + j) > l.width) continue;
				for (short k = Convert.ToInt16(-num3); k <= num3; k = (short)(k + 1))
				{
					if ((y + k) < 0 || (y + k) > l.depth) continue;
					for (short m = Convert.ToInt16(-num3); m <= num3; m = (short)(m + 1))
					{
						if ((z + m) < 0 || (z + m) > l.length) continue;
						short maxValue = (short)Math.Sqrt((double)(((j * j) + (k * k)) + (m * m)));
						if ((maxValue < (num3 + 1)) && (random.Next(maxValue) < 15))
						{
							try
							{
								ushort x2 = (ushort)(x + j); //width
								ushort y2 = (ushort)(y + k); //depth
								ushort z2 = (ushort)(z + m); //height

								if (x2 <= l.width && y2 <= l.depth && z2 <= l.length)
								{
									byte that = l.GetTile((ushort)(x + j), (ushort)((y + k)), (ushort)(z + m));

									if (that != 7)
									{
										l.Blockchange((ushort)(x + j), (ushort)((y + k)), (ushort)(z + m), 0);
									}
								}
							}
							catch (Exception e)
							{
								Server.s.Log(e.Message);
							}
						}
					}
				}
			}
		}
		public static void NukeS(Level l, ushort x, ushort y, ushort z, int size)
		{
			foreach (Player p in Player.players) if (p.level == l) p.SendBlockchange(x, y, z, 0);
			List<string> toair = new List<string>();

			int num3 = size;

			for (short j = Convert.ToInt16(-num3); j <= num3; j = (short)(j + 1))
			{
				if ((x + j) < 0 || (x + j) > l.width) continue;
				for (short k = Convert.ToInt16(-num3); k <= num3; k = (short)(k + 1))
				{
					if ((y + k) < 0 || (y + k) > l.depth) continue;
					for (short m = Convert.ToInt16(-num3); m <= num3; m = (short)(m + 1))
					{
						if ((z + m) < 0 || (z + m) > l.length) continue;

						short maxValue = (short)Math.Sqrt((double)(((j * j) + (k * k)) + (m * m))); //W00t FOUND THE SECRET!
						if ((maxValue < (num3 + 1)) && (random.Next(maxValue) < size))
						{
							try
							{
								ushort x2 = (ushort)(x + j); //width
								ushort y2 = (ushort)(y + k); //depth
								ushort z2 = (ushort)(z + m); //height

								if (x2 <= l.width && y2 <= l.depth && z2 <= l.length)
								{
									byte that = l.GetTile((ushort)(x + j), (ushort)((y + k)), (ushort)(z + m));

									if (that != 7)
									{
										l.Blockchange((ushort)(x + j), (ushort)((y + k)), (ushort)(z + m), 0);
									}
								}
							}
							catch (Exception e)
							{
								Server.s.Log(e.Message);
							}
						}
					}
				}
			}
		}

		public static void Cone(Level l, ushort x, ushort y, ushort z, int height, int radius, byte block)
		{
			foreach (Player p in Player.players) if (p.level == l) p.SendBlockchange(x, y, z, 0);

			for (short k = 0; k <= height; k = (short)(k + 1))
			{
				if ((y + k) < 0 || (y + k) > l.depth) continue;
				for (short j = Convert.ToInt16(-radius); j <= radius; j = (short)(j + 1))
				{
					if ((x + j) < 0 || (x + j) > l.width) continue;
					for (short m = Convert.ToInt16(-radius); m <= radius; m = (short)(m + 1))
					{
						if ((z + m) < 0 || (z + m) > l.length) continue;

						int ox = x;
						int oy = y;
						int oz = z;

						int cx = (x + j);
						int cy = (y + k);
						int cz = (z + m);

						double currentheight = height - k;

						double currentradius;
						if (currentheight == 0)
						{ }
						else
						{
							currentradius = (double)((double)radius * (double)((double)currentheight / (double)height));
							int absx = Math.Abs(j);
							int absz = Math.Abs(m);

							double pointradius = sqrt((absx * absx) + (absz * absz));

							if (pointradius <= currentradius)
							{
								byte ctile = l.GetTile((ushort)cx, (ushort)cy, (ushort)cz);
								if (ctile == 0)
								{
									l.Blockchange((ushort)cx, (ushort)cy, (ushort)cz, block);
								}
							}
						}

					}
				}
			}
			if ((y + height) <= l.depth)
				l.Blockchange(x, (ushort)(y + height), z, block);
		}
		public static void HCone(Level l, ushort x, ushort y, ushort z, int height, int radius, byte block)
		{
			foreach (Player p in Player.players) if (p.level == l) p.SendBlockchange(x, y, z, 0);

			double origionalhypotenuse = sqrt((height * height) + (radius * radius));

			for (short k = 0; k <= height; k = (short)(k + 1))
			{
				if ((y + k) < 0 || (y + k) > l.depth) continue;
				for (short j = Convert.ToInt16(-radius); j <= radius; j = (short)(j + 1))
				{
					if ((x + j) < 0 || (x + j) > l.width) continue;
					for (short m = Convert.ToInt16(-radius); m <= radius; m = (short)(m + 1))
					{
						if ((z + m) < 0 || (z + m) > l.length) continue;

						int ox = x;
						int oy = y;
						int oz = z;

						int cx = (x + j);
						int cy = (y + k);
						int cz = (z + m);

						double currentheight = height - k;

						double currentradius;
						if (currentheight == 0)
						{ }
						else
						{
							currentradius = (double)((double)radius * (double)((double)currentheight / (double)height));
							int absx = Math.Abs(j);
							int absz = Math.Abs(m);

							double pointradius = sqrt((absx * absx) + (absz * absz));

							if (pointradius <= currentradius && pointradius >= (currentradius - 1))
							{
								byte ctile = l.GetTile((ushort)cx, (ushort)cy, (ushort)cz);
								if (ctile == 0)
								{
									l.Blockchange((ushort)cx, (ushort)cy, (ushort)cz, block);
								}
							}
						}

					}
				}
			}
			if ((y + height) <= l.depth)
				l.Blockchange(x, (ushort)(y + height), z, block);
		}
		public static void ICone(Level l, ushort x, ushort y, ushort z, int height, int radius, byte block)
		{
			foreach (Player p in Player.players) if (p.level == l) p.SendBlockchange(x, y, z, 0);

			double origionalhypotenuse = sqrt((height * height) + (radius * radius));

			for (short k = 0; k <= height; k = (short)(k + 1))
			{
				if ((y + k) < 0 || (y + k) > l.depth) continue;
				for (short j = Convert.ToInt16(-radius); j <= radius; j = (short)(j + 1))
				{
					if ((x + j) < 0 || (x + j) > l.width) continue;
					for (short m = Convert.ToInt16(-radius); m <= radius; m = (short)(m + 1))
					{
						if ((z + m) < 0 || (z + m) > l.length) continue;

						int ox = x;
						int oy = y;
						int oz = z;

						int cx = (x + j);
						int cy = (y + k);
						int cz = (z + m);

						double currentheight = k;

						double currentradius;
						if (currentheight == 0)
						{ }
						else
						{
							currentradius = (double)((double)radius * (double)((double)currentheight / (double)height));
							int absx = Math.Abs(j);
							int absz = Math.Abs(m);

							double pointradius = sqrt((absx * absx) + (absz * absz));

							if (pointradius <= currentradius)
							{
								byte ctile = l.GetTile((ushort)cx, (ushort)cy, (ushort)cz);
								if (ctile == 0)
								{
									l.Blockchange((ushort)cx, (ushort)cy, (ushort)cz, block);
								}
							}
						}

					}
				}
			}
			l.Blockchange(x, y, z, block);
		}
		public static void HICone(Level l, ushort x, ushort y, ushort z, int height, int radius, byte block)
		{
			foreach (Player p in Player.players) if (p.level == l) p.SendBlockchange(x, y, z, 0);

			double origionalhypotenuse = sqrt((height * height) + (radius * radius));

			for (short k = 0; k <= height; k = (short)(k + 1))
			{
				if ((y + k) < 0 || (y + k) > l.depth) continue;
				for (short j = Convert.ToInt16(-radius); j <= radius; j = (short)(j + 1))
				{
					if ((x + j) < 0 || (x + j) > l.width) continue;
					for (short m = Convert.ToInt16(-radius); m <= radius; m = (short)(m + 1))
					{
						if ((z + m) < 0 || (z + m) > l.length) continue;

						int ox = x;
						int oy = y;
						int oz = z;

						int cx = (x + j);
						int cy = (y + k);
						int cz = (z + m);

						double currentheight = k;

						double currentradius;
						if (currentheight == 0)
						{ }
						else
						{
							currentradius = (double)((double)radius * (double)((double)currentheight / (double)height));
							int absx = Math.Abs(j);
							int absz = Math.Abs(m);

							double pointradius = sqrt((absx * absx) + (absz * absz));

							if (pointradius <= currentradius && pointradius >= (currentradius - 1))
							{
								byte ctile = l.GetTile((ushort)cx, (ushort)cy, (ushort)cz);
								if (ctile == 0)
								{
									l.Blockchange((ushort)cx, (ushort)cy, (ushort)cz, block);
								}
							}
						}

					}
				}
			}
			l.Blockchange(x, y, z, block);
		}

		//For the pyramid commands, Radius still refers to the distance from the center point, but is axis independant, rather then a referance to both axes
		public static void Pyramid(Level l, ushort x, ushort y, ushort z, int height, int radius, byte block)
		{
			foreach (Player p in Player.players) if (p.level == l) p.SendBlockchange(x, y, z, 0);

			for (short k = 0; k <= height; k = (short)(k + 1))
			{
				if ((y + k) < 0 || (y + k) > l.depth) continue;
				for (short j = Convert.ToInt16(-radius); j <= radius; j = (short)(j + 1))
				{
					if ((x + j) < 0 || (x + j) > l.width) continue;
					for (short m = Convert.ToInt16(-radius); m <= radius; m = (short)(m + 1))
					{
						if ((z + m) < 0 || (z + m) > l.length) continue;

						int ox = x;
						int oy = y;
						int oz = z;

						int cx = (x + j);
						int cy = (y + k);
						int cz = (z + m);

						double currentheight = height - k;

						double currentradius;
						if (currentheight == 0)
						{ }
						else
						{
							currentradius = (double)((double)radius * (double)((double)currentheight / (double)height));
							int absx = Math.Abs(j);
							int absz = Math.Abs(m);

							if (absx > currentradius) continue;
							if (absz > currentradius) continue;

							byte ctile = l.GetTile((ushort)cx, (ushort)cy, (ushort)cz);
							if (ctile == 0)
							{
								l.Blockchange((ushort)cx, (ushort)cy, (ushort)cz, block);
							}
						}

					}
				}
			}
			if ((y + height) <= l.depth)
				l.Blockchange(x, (ushort)(y + height), z, block);
		}
		public static void HPyramid(Level l, ushort x, ushort y, ushort z, int height, int radius, byte block)
		{
			foreach (Player p in Player.players) if (p.level == l) p.SendBlockchange(x, y, z, 0);

			for (short k = 0; k <= height; k = (short)(k + 1))
			{
				if ((y + k) < 0 || (y + k) > l.depth) continue;
				for (short j = Convert.ToInt16(-radius); j <= radius; j = (short)(j + 1))
				{
					if ((x + j) < 0 || (x + j) > l.width) continue;
					for (short m = Convert.ToInt16(-radius); m <= radius; m = (short)(m + 1))
					{
						if ((z + m) < 0 || (z + m) > l.length) continue;

						int ox = x;
						int oy = y;
						int oz = z;

						int cx = (x + j);
						int cy = (y + k);
						int cz = (z + m);

						double currentheight = height - k;

						double currentradius;
						if (currentheight == 0)
						{ }
						else
						{
							currentradius = (double)((double)radius * (double)((double)currentheight / (double)height));
							int absx = Math.Abs(j);
							int absz = Math.Abs(m);

							if (absx > currentradius || absz > currentradius) continue;
							if (absx < (currentradius - 1) && absz < (currentradius - 1)) continue;

							byte ctile = l.GetTile((ushort)cx, (ushort)cy, (ushort)cz);
							if (ctile == 0)
							{
								l.Blockchange((ushort)cx, (ushort)cy, (ushort)cz, block);
							}
						}

					}
				}
			}
			if ((y + height) <= l.depth)
				l.Blockchange(x, (ushort)(y), z, block);
		}
		public static void IPyramid(Level l, ushort x, ushort y, ushort z, int height, int radius, byte block)
		{
			foreach (Player p in Player.players) if (p.level == l) p.SendBlockchange(x, y, z, 0);

			for (short k = 0; k <= height; k = (short)(k + 1))
			{
				if ((y + k) < 0 || (y + k) > l.depth) continue;
				for (short j = Convert.ToInt16(-radius); j <= radius; j = (short)(j + 1))
				{
					if ((x + j) < 0 || (x + j) > l.width) continue;
					for (short m = Convert.ToInt16(-radius); m <= radius; m = (short)(m + 1))
					{
						if ((z + m) < 0 || (z + m) > l.length) continue;

						int ox = x;
						int oy = y;
						int oz = z;

						int cx = (x + j);
						int cy = (y + k);
						int cz = (z + m);

						double currentheight = k;

						double currentradius;
						if (currentheight == 0)
						{ }
						else
						{
							currentradius = (double)((double)radius * (double)((double)currentheight / (double)height));
							int absx = Math.Abs(j);
							int absz = Math.Abs(m);

							if (absx > currentradius) continue;
							if (absz > currentradius) continue;

							byte ctile = l.GetTile((ushort)cx, (ushort)cy, (ushort)cz);
							if (ctile == 0)
							{
								l.Blockchange((ushort)cx, (ushort)cy, (ushort)cz, block);
							}
						}

					}
				}
			}
			if ((y + height) <= l.depth)
				l.Blockchange(x, (ushort)(y + height), z, block);
		}
		public static void HIPyramid(Level l, ushort x, ushort y, ushort z, int height, int radius, byte block)
		{
			foreach (Player p in Player.players) if (p.level == l) p.SendBlockchange(x, y, z, 0);

			for (short k = 0; k <= height; k = (short)(k + 1))
			{
				if ((y + k) < 0 || (y + k) > l.depth) continue;
				for (short j = Convert.ToInt16(-radius); j <= radius; j = (short)(j + 1))
				{
					if ((x + j) < 0 || (x + j) > l.width) continue;
					for (short m = Convert.ToInt16(-radius); m <= radius; m = (short)(m + 1))
					{
						if ((z + m) < 0 || (z + m) > l.length) continue;

						int ox = x;
						int oy = y;
						int oz = z;

						int cx = (x + j);
						int cy = (y + k);
						int cz = (z + m);

						double currentheight = k;

						double currentradius;
						if (currentheight == 0)
						{ }
						else
						{
							currentradius = (double)((double)radius * (double)((double)currentheight / (double)height));
							int absx = Math.Abs(j);
							int absz = Math.Abs(m);

							if (absx > currentradius || absz > currentradius) continue;
							if (absx < (currentradius - 1) && absz < (currentradius - 1)) continue;

							byte ctile = l.GetTile((ushort)cx, (ushort)cy, (ushort)cz);
							if (ctile == 0)
							{
								l.Blockchange((ushort)cx, (ushort)cy, (ushort)cz, block);
							}
						}

					}
				}
			}
			if ((y + height) <= l.depth)
				l.Blockchange(x, (ushort)(y), z, block);
		}

		public static void Sphere(Level l, ushort x, ushort y, ushort z, int radius, byte type)
		{
			for (short j = Convert.ToInt16(-radius); j <= radius; j = (short)(j + 1))
			{
				if ((x + j) < 0 || (x + j) > l.width) continue;
				for (short k = Convert.ToInt16(-radius); k <= radius; k = (short)(k + 1))
				{
					if ((y + k) < 0 || (y + k) > l.depth) continue;
					for (short m = Convert.ToInt16(-radius); m <= radius; m = (short)(m + 1))
					{
						if ((z + m) < 0 || (z + m) > l.length) continue;
						short maxValue = (short)Math.Sqrt((double)(((j * j) + (k * k)) + (m * m)));
						if ((maxValue < (radius + 1)))
						{
							try
							{
								ushort x2 = (ushort)(x + j);
								ushort y2 = (ushort)(y + k);
								ushort z2 = (ushort)(z + m);
								if (x2 <= l.width && y2 <= l.depth && z2 <= l.length)
								{
									byte that = l.GetTile((ushort)(x + j), (ushort)((y + k)), (ushort)(z + m));
									if (that != 7)
									{
										l.Blockchange((ushort)(x + j), (ushort)((y + k)), (ushort)(z + m), type);
									}
								}
							}
							catch { }
						}
					}
				}
			}
		}
		public static void HSphere(Level l, ushort x, ushort y, ushort z, int radius, byte type)
		{
			for (short j = Convert.ToInt16(-radius); j <= radius; j = (short)(j + 1))
			{
				if ((x + j) < 0 || (x + j) > l.width) continue;
				for (short k = Convert.ToInt16(-radius); k <= radius; k = (short)(k + 1))
				{
					if ((y + k) < 0 || (y + k) > l.depth) continue;
					for (short m = Convert.ToInt16(-radius); m <= radius; m = (short)(m + 1))
					{
						if ((z + m) < 0 || (z + m) > l.length) continue;
						short maxValue = (short)Math.Sqrt((double)(((j * j) + (k * k)) + (m * m)));
						if (maxValue < (radius + 1) && maxValue >= (radius - 1))
						{
							try
							{
								ushort x2 = (ushort)(x + j);
								ushort y2 = (ushort)(y + k);
								ushort z2 = (ushort)(z + m);
								if (x2 <= l.width && y2 <= l.depth && z2 <= l.length)
								{
									byte that = l.GetTile((ushort)(x + j), (ushort)((y + k)), (ushort)(z + m));
									if (that != 7)
									{
										l.Blockchange((ushort)(x + j), (ushort)((y + k)), (ushort)(z + m), type);
									}
								}
							}
							catch { }
						}
					}
				}
			}
		}

		public static void Valcano(Level l, ushort x, ushort y, ushort z, int height, int radius)
		{
			foreach (Player p in Player.players) if (p.level == l) p.SendBlockchange(x, y, z, 0);

			double origionalhypotenuse = sqrt((height * height) + (radius * radius));

			for (short k = 0; k <= height; k = (short)(k + 1))
			{
				if ((y + k) < 0 || (y + k) > l.depth) continue;
				for (short j = Convert.ToInt16(-radius); j <= radius; j = (short)(j + 1))
				{
					if ((x + j) < 0 || (x + j) > l.width) continue;
					for (short m = Convert.ToInt16(-radius); m <= radius; m = (short)(m + 1))
					{
						if ((z + m) < 0 || (z + m) > l.length) continue;

						int ox = x;
						int oy = y;
						int oz = z;

						int cx = (x + j);
						int cy = (y + k);
						int cz = (z + m);

						double currentheight = height - k;

						double currentradius;
						if (currentheight == 0)
						{ }
						else
						{
							currentradius = (double)((double)radius * (double)((double)currentheight / (double)height));
							int absx = Math.Abs(j);
							int absz = Math.Abs(m);

							double pointradius = sqrt((absx * absx) + (absz * absz));

							if (pointradius <= currentradius && pointradius >= (currentradius - 1))
							{
								byte ctile = l.GetTile((ushort)cx, (ushort)cy, (ushort)cz);
								if (ctile == 0)
								{
									l.Blockchange((ushort)cx, (ushort)cy, (ushort)cz, Block.grass);
								}
							}
							else if (pointradius <= currentradius)
							{
								byte ctile = l.GetTile((ushort)cx, (ushort)cy, (ushort)cz);
								if (ctile == 0)
								{
									l.Blockchange((ushort)cx, (ushort)cy, (ushort)cz, Block.lava);
								}
							}
						}

					}
				}
			}
		}

		#region Stuff that's not used in McForge (at least not yet)
		//public void ToAirMethod(List<string> toair)
		//{
		//    if (toair.Count >= 1)
		//    {
		//        foreach (string s in toair)
		//        {
		//            try
		//            {
		//                string[] k = s.Split('^');
		//                Level l = Level.Find(k[0]);
		//                ushort x = Convert.ToUInt16(k[1]);
		//                ushort y = Convert.ToUInt16(k[2]);
		//                ushort z = Convert.ToUInt16(k[3]);
		//                l.Blockchange(x, y, z, 0);
		//            }
		//            catch { }
		//        }
		//    }
		//}

		//public void TNTthreader() //We may leave this because i cant think of an efficient way to do this.
		//{
		//    while (true)
		//    {
		//        try
		//        {
		//            foreach (string t in TNTCHAIN.ToArray())
		//            {
		//                TNTCHAIN.Remove(t); //do first so if it fails its not there anymore :D

		//                string[] t2 = t.Split('^');
		//                Level l = Level.Find(t2[0]);
		//                ushort x = Convert.ToUInt16(t2[1]);
		//                ushort y = Convert.ToUInt16(t2[2]);
		//                ushort z = Convert.ToUInt16(t2[3]);
		//                TNT(l, x, y, z);
		//            }
		//        }
		//        catch { }
		//        Thread.Sleep(100);
		//    }
		//}
		#endregion

		#region utilities
		static private double sqrt(double x)
		{
			return Math.Sqrt(x);
		}
		#endregion
	}
}