using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace libtcod
{
	[StructLayout (LayoutKind.Sequential)]
	public struct Color : IEquatable<Color>
	{
		private byte r;
		private byte g;
		private byte b;

		public byte Red
		{
			get { return r; }
		}

		public byte Green
		{
			get { return g; }
		}

		public byte Blue
		{
			get { return b; }
		}

		public float Hue
		{
			get
			{
				float h, s, v;
				TCOD_color_get_HSV (this, out h, out s, out v);
				return h;
			}
		}

		public float Saturation
		{
			get
			{
				float h, s, v;
				TCOD_color_get_HSV (this, out h, out s, out v);
				return s;
			}
		}

		public float Value
		{
			get
			{
				float h, s, v;
				TCOD_color_get_HSV (this, out h, out s, out v);
				return v;
			}
		}

		private Color (byte red, byte green, byte blue)
		{
			r = red;
			g = green;
			b = blue;
		}

		// Sometimes due to performance reasons, you don't want to allocate a new color for every color change. This changes a color to a new one.
		public void ConvertToNewColor (byte red, byte green, byte blue)
		{
			r = red;
			g = green;
			b = blue;
		}

		public static Color FromRGB (byte red, byte green, byte blue)
		{
			return new Color (red, green, blue);
		}

		// 0<= h < 360, 0 <= s <= 1, 0 <= v <= 1 
		[DllImport ("libtcod")]
		private extern static void TCOD_color_set_HSV (ref Color c, float h, float s, float v);

		public static Color FromHSV (float hue, float saturation, float value)
		{
			Color c = new Color (0, 0, 0);
			TCOD_color_set_HSV (ref c, hue, saturation, value);
			return c;
		}

		[DllImport ("libtcod")]
		private extern static void TCOD_color_get_HSV (Color c, out float h, out float s, out float v);

		public void GetHSV (out float h, out float s, out float v)
		{
			TCOD_color_get_HSV (this, out h, out s, out v);
		}

		public override bool Equals (object obj)
		{
			if (obj == null || GetType () != obj.GetType ())
				return false;

			Color rhs = (Color)obj;

			return this == rhs;
		}

		public bool Equals (Color other)
		{
			if ((object)other == null)
				return false;
			return this == other;
		}

		public override int GetHashCode ()
		{
			return r.GetHashCode () ^ b.GetHashCode () ^ g.GetHashCode ();
		}

		public static bool operator == (Color lhs, Color rhs)
		{
			if (((object)lhs == null) && ((object)rhs == null))
				return true;

			// If one is null, but not both, return false.
			if (((object)lhs == null) || ((object)rhs == null))
				return false;

			return lhs.r == rhs.r && lhs.g == rhs.g && lhs.b == rhs.b;
		}

		public static bool operator != (Color lhs, Color rhs)
		{
			return !(lhs == rhs);
		}


		[DllImport ("libtcod")]
		private extern static Color TCOD_color_add (Color c1, Color c2);

		public static Color operator + (Color lhs, Color rhs)
		{
			return TCOD_color_add (lhs, rhs);
		}

		[DllImport ("libtcod")]
		private extern static Color TCOD_color_multiply (Color c1, Color c2);

		public static Color operator * (Color lhs, Color rhs)
		{
			return TCOD_color_multiply (lhs, rhs);
		}

		[DllImport ("libtcod")]
		private extern static Color TCOD_color_multiply_scalar (Color c1, float value);

		public static Color operator * (Color lhs, float rhs)
		{
			return TCOD_color_multiply_scalar (lhs, rhs);
		}

		public static Color operator * (Color lhs, double rhs)
		{
			return TCOD_color_multiply_scalar (lhs, (float)rhs);
		}

		public static Color operator / (Color lhs, int rhs)
		{
			return new Color ((byte)(lhs.r / rhs), (byte)(lhs.g / rhs), (byte)(lhs.b / rhs));
		}

		public static Color operator / (Color lhs, float rhs)
		{
			return new Color ((byte)((float)lhs.r / rhs), (byte)((float)lhs.g / rhs), (byte)((float)lhs.b / rhs));
		}

		public static Color operator / (Color lhs, double rhs)
		{
			return new Color ((byte)((double)lhs.r / rhs), (byte)((double)lhs.g / rhs), (byte)((double)lhs.b / rhs));
		}

		public static Color operator - (Color lhs, Color rhs)
		{
			return new Color (SubFloor (lhs.r, rhs.r), SubFloor (lhs.g, rhs.g), SubFloor (lhs.b, rhs.b));
		}

		private static byte SubFloor (byte lhs, byte rhs)
		{
			if (lhs < rhs)
				return 0;
			else
				return (byte)(lhs - rhs);
		}

		public static Color Interpolate (Color c1, Color c2, float coef)
		{
			Color ret = new Color ();
			ret.r = (byte)(c1.r + (c2.r - c1.r) * coef);
			ret.g = (byte)(c1.g + (c2.g - c1.g) * coef);
			ret.b = (byte)(c1.b + (c2.b - c1.b) * coef);
			return ret;
		}

		public static Color Interpolate (Color c1, Color c2, double coef)
		{
			Color ret = new Color ();
			ret.r = (byte)(c1.r + (c2.r - c1.r) * coef);
			ret.g = (byte)(c1.g + (c2.g - c1.g) * coef);
			ret.b = (byte)(c1.b + (c2.b - c1.b) * coef);
			return ret;
		}
	}

	public static class ColorPresets
	{
		public static readonly Color AliceBlue = Color.FromRGB (240, 248, 255);
		public static readonly Color AntiqueWhite = Color.FromRGB (250, 235, 215);
		public static readonly Color Aqua = Color.FromRGB (0, 255, 255);
		public static readonly Color Aquamarine = Color.FromRGB (127, 255, 212);
		public static readonly Color Azure = Color.FromRGB (240, 255, 255);
		public static readonly Color Beige = Color.FromRGB (245, 245, 220);
		public static readonly Color Bisque = Color.FromRGB (255, 228, 196);
		public static readonly Color Black = Color.FromRGB (0, 0, 0);
		public static readonly Color BlanchedAlmond = Color.FromRGB (255, 235, 205);
		public static readonly Color Blue = Color.FromRGB (0, 0, 255);
		public static readonly Color BlueViolet = Color.FromRGB (138, 43, 226);
		public static readonly Color Brown = Color.FromRGB (165, 42, 42);
		public static readonly Color BurlyWood = Color.FromRGB (222, 184, 135);
		public static readonly Color CadetBlue = Color.FromRGB (95, 158, 160);
		public static readonly Color Chartreuse = Color.FromRGB (127, 255, 0);
		public static readonly Color Chocolate = Color.FromRGB (210, 105, 30);
		public static readonly Color Coral = Color.FromRGB (255, 127, 80);
		public static readonly Color CornflowerBlue = Color.FromRGB (100, 149, 237);
		public static readonly Color Cornsilk = Color.FromRGB (255, 248, 220);
		public static readonly Color Crimson = Color.FromRGB (220, 20, 60);
		public static readonly Color Cyan = Color.FromRGB (0, 255, 255);
		public static readonly Color DarkBlue = Color.FromRGB (0, 0, 139);
		public static readonly Color DarkCyan = Color.FromRGB (0, 139, 139);
		public static readonly Color DarkGoldenrod = Color.FromRGB (184, 134, 11);
		public static readonly Color DarkGray = Color.FromRGB (169, 169, 169);
		public static readonly Color DarkGreen = Color.FromRGB (0, 100, 0);
		public static readonly Color DarkKhaki = Color.FromRGB (189, 183, 107);
		public static readonly Color DarkMagenta = Color.FromRGB (139, 0, 139);
		public static readonly Color DarkOliveGreen = Color.FromRGB (85, 107, 47);
		public static readonly Color DarkOrange = Color.FromRGB (255, 140, 0);
		public static readonly Color DarkOrchid = Color.FromRGB (153, 50, 204);
		public static readonly Color DarkRed = Color.FromRGB (139, 0, 0);
		public static readonly Color DarkSalmon = Color.FromRGB (233, 150, 122);
		public static readonly Color DarkSeaGreen = Color.FromRGB (143, 188, 139);
		public static readonly Color DarkSlateBlue = Color.FromRGB (72, 61, 139);
		public static readonly Color DarkSlateGray = Color.FromRGB (47, 79, 79);
		public static readonly Color DarkTurquoise = Color.FromRGB (0, 206, 209);
		public static readonly Color DarkViolet = Color.FromRGB (148, 0, 211);
		public static readonly Color DeepPink = Color.FromRGB (255, 20, 147);
		public static readonly Color DeepSkyBlue = Color.FromRGB (0, 191, 255);
		public static readonly Color DimGray = Color.FromRGB (105, 105, 105);
		public static readonly Color DodgerBlue = Color.FromRGB (30, 144, 255);
		public static readonly Color Firebrick = Color.FromRGB (178, 34, 34);
		public static readonly Color FloralWhite = Color.FromRGB (255, 250, 240);
		public static readonly Color ForestGreen = Color.FromRGB (34, 139, 34);
		public static readonly Color Fuchsia = Color.FromRGB (255, 0, 255);
		public static readonly Color Gainsboro = Color.FromRGB (220, 220, 220);
		public static readonly Color GhostWhite = Color.FromRGB (248, 248, 255);
		public static readonly Color Gold = Color.FromRGB (255, 215, 0);
		public static readonly Color Goldenrod = Color.FromRGB (218, 165, 32);
		public static readonly Color Gray = Color.FromRGB (128, 128, 128);
		public static readonly Color Green = Color.FromRGB (0, 128, 0);
		public static readonly Color GreenYellow = Color.FromRGB (173, 255, 47);
		public static readonly Color Honeydew = Color.FromRGB (240, 255, 240);
		public static readonly Color HotPink = Color.FromRGB (255, 105, 180);
		public static readonly Color IndianRed = Color.FromRGB (205, 92, 92);
		public static readonly Color Indigo = Color.FromRGB (75, 0, 130);
		public static readonly Color Ivory = Color.FromRGB (255, 255, 240);
		public static readonly Color Khaki = Color.FromRGB (240, 230, 140);
		public static readonly Color Lavender = Color.FromRGB (230, 230, 250);
		public static readonly Color LavenderBlush = Color.FromRGB (255, 240, 245);
		public static readonly Color LawnGreen = Color.FromRGB (124, 252, 0);
		public static readonly Color LemonChiffon = Color.FromRGB (255, 250, 205);
		public static readonly Color LightBlue = Color.FromRGB (173, 216, 230);
		public static readonly Color LightCoral = Color.FromRGB (240, 128, 128);
		public static readonly Color LightCyan = Color.FromRGB (224, 255, 255);
		public static readonly Color LightGoldenrodYellow = Color.FromRGB (250, 250, 210);
		public static readonly Color LightGray = Color.FromRGB (211, 211, 211);
		public static readonly Color LightGreen = Color.FromRGB (144, 238, 144);
		public static readonly Color LightPink = Color.FromRGB (255, 182, 193);
		public static readonly Color LightSalmon = Color.FromRGB (255, 160, 122);
		public static readonly Color LightSeaGreen = Color.FromRGB (32, 178, 170);
		public static readonly Color LightSkyBlue = Color.FromRGB (135, 206, 250);
		public static readonly Color LightSlateGray = Color.FromRGB (119, 136, 153);
		public static readonly Color LightSteelBlue = Color.FromRGB (176, 196, 222);
		public static readonly Color LightYellow = Color.FromRGB (255, 255, 224);
		public static readonly Color Lime = Color.FromRGB (0, 255, 0);
		public static readonly Color LimeGreen = Color.FromRGB (50, 205, 50);
		public static readonly Color Linen = Color.FromRGB (250, 240, 230);
		public static readonly Color Magenta = Color.FromRGB (255, 0, 255);
		public static readonly Color Maroon = Color.FromRGB (128, 0, 0);
		public static readonly Color MediumAquamarine = Color.FromRGB (102, 205, 170);
		public static readonly Color MediumBlue = Color.FromRGB (0, 0, 205);
		public static readonly Color MediumOrchid = Color.FromRGB (186, 85, 211);
		public static readonly Color MediumPurple = Color.FromRGB (147, 112, 219);
		public static readonly Color MediumSeaGreen = Color.FromRGB (60, 179, 113);
		public static readonly Color MediumSlateBlue = Color.FromRGB (123, 104, 238);
		public static readonly Color MediumSpringGreen = Color.FromRGB (0, 250, 154);
		public static readonly Color MediumTurquoise = Color.FromRGB (72, 209, 204);
		public static readonly Color MediumVioletRed = Color.FromRGB (199, 21, 133);
		public static readonly Color MidnightBlue = Color.FromRGB (25, 25, 112);
		public static readonly Color MintCream = Color.FromRGB (245, 255, 250);
		public static readonly Color MistyRose = Color.FromRGB (255, 228, 225);
		public static readonly Color Moccasin = Color.FromRGB (255, 228, 181);
		public static readonly Color NavajoWhite = Color.FromRGB (255, 222, 173);
		public static readonly Color Navy = Color.FromRGB (0, 0, 128);
		public static readonly Color OldLace = Color.FromRGB (253, 245, 230);
		public static readonly Color Olive = Color.FromRGB (128, 128, 0);
		public static readonly Color OliveDrab = Color.FromRGB (107, 142, 35);
		public static readonly Color Orange = Color.FromRGB (255, 165, 0);
		public static readonly Color OrangeRed = Color.FromRGB (255, 69, 0);
		public static readonly Color Orchid = Color.FromRGB (218, 112, 214);
		public static readonly Color PaleGoldenrod = Color.FromRGB (238, 232, 170);
		public static readonly Color PaleGreen = Color.FromRGB (152, 251, 152);
		public static readonly Color PaleTurquoise = Color.FromRGB (175, 238, 238);
		public static readonly Color PaleVioletRed = Color.FromRGB (219, 112, 147);
		public static readonly Color PapayaWhip = Color.FromRGB (255, 239, 213);
		public static readonly Color PeachPuff = Color.FromRGB (255, 218, 185);
		public static readonly Color Peru = Color.FromRGB (205, 133, 63);
		public static readonly Color Pink = Color.FromRGB (255, 192, 203);
		public static readonly Color Plum = Color.FromRGB (221, 160, 221);
		public static readonly Color PowderBlue = Color.FromRGB (176, 224, 230);
		public static readonly Color Purple = Color.FromRGB (128, 0, 128);
		public static readonly Color Red = Color.FromRGB (255, 0, 0);
		public static readonly Color RosyBrown = Color.FromRGB (188, 143, 143);
		public static readonly Color RoyalBlue = Color.FromRGB (65, 105, 225);
		public static readonly Color SaddleBrown = Color.FromRGB (139, 69, 19);
		public static readonly Color Salmon = Color.FromRGB (250, 128, 114);
		public static readonly Color SandyBrown = Color.FromRGB (244, 164, 96);
		public static readonly Color SeaGreen = Color.FromRGB (46, 139, 87);
		public static readonly Color SeaShell = Color.FromRGB (255, 245, 238);
		public static readonly Color Sienna = Color.FromRGB (160, 82, 45);
		public static readonly Color Silver = Color.FromRGB (192, 192, 192);
		public static readonly Color SkyBlue = Color.FromRGB (135, 206, 235);
		public static readonly Color SlateBlue = Color.FromRGB (106, 90, 205);
		public static readonly Color SlateGray = Color.FromRGB (112, 128, 144);
		public static readonly Color Snow = Color.FromRGB (255, 250, 250);
		public static readonly Color SpringGreen = Color.FromRGB (0, 255, 127);
		public static readonly Color SteelBlue = Color.FromRGB (70, 130, 180);
		public static readonly Color Tan = Color.FromRGB (210, 180, 140);
		public static readonly Color Teal = Color.FromRGB (0, 128, 128);
		public static readonly Color Thistle = Color.FromRGB (216, 191, 216);
		public static readonly Color Tomato = Color.FromRGB (255, 99, 71);
		public static readonly Color Turquoise = Color.FromRGB (64, 224, 208);
		public static readonly Color Violet = Color.FromRGB (238, 130, 238);
		public static readonly Color Wheat = Color.FromRGB (245, 222, 179);
		public static readonly Color White = Color.FromRGB (255, 255, 255);
		public static readonly Color WhiteSmoke = Color.FromRGB (245, 245, 245);
		public static readonly Color Yellow = Color.FromRGB (255, 255, 0);
		public static readonly Color YellowGreen = Color.FromRGB (154, 205, 50);

		public static readonly Color TCODBlack = Color.FromRGB (0, 0, 0);
		public static readonly Color TCODDarkGray = Color.FromRGB (96, 96, 96);
		public static readonly Color TCODGray = Color.FromRGB (196, 196, 196);
		public static readonly Color TCODWhite = Color.FromRGB (255, 255, 255);
		public static readonly Color TCODDarkBlue = Color.FromRGB (40, 40, 128);
		public static readonly Color TCODBrightBlue = Color.FromRGB (120, 120, 255);
		public static readonly Color TCODDarkRed = Color.FromRGB (128, 0, 0);
		public static readonly Color TCODRed = Color.FromRGB (255, 0, 0);
		public static readonly Color TCODBrightRed = Color.FromRGB (255, 100, 50);
		public static readonly Color TCODBrown = Color.FromRGB (32, 16, 0);
		public static readonly Color TCODBrightYellow = Color.FromRGB (255, 255, 150);
		public static readonly Color TCODYellow = Color.FromRGB (255, 255, 0);
		public static readonly Color TCODDarkYellow = Color.FromRGB (164, 164, 0);
		public static readonly Color TCODBrightGreen = Color.FromRGB (0, 255, 0);
		public static readonly Color TCODGreen = Color.FromRGB (0, 220, 0);
		public static readonly Color TCODDarkGreen = Color.FromRGB (0, 128, 0);
		public static readonly Color TCODOrange = Color.FromRGB (255, 150, 0);
		public static readonly Color TCODSilver = Color.FromRGB (203, 203, 203);
		public static readonly Color TCODGold = Color.FromRGB (255, 255, 102);
		public static readonly Color TCODPurple = Color.FromRGB (204, 51, 153);
		public static readonly Color TCODDarkPurple = Color.FromRGB (51, 0, 51);
	}
}
