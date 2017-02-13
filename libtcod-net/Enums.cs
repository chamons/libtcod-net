namespace libtcod
{
	public enum BackgroundFlag
	{
		None,
		Set,
		Multiply,
		Lighten,
		Darken,
		Screen,
		ColorDodge,
		ColorBurn,
		Add,
		AddA,
		Burn,
		Overlay,
		Alph,
		Default
	};

	public enum KeyCode
	{
		None,
		Escape,
		Backspace,
		Tab,
		Enter,
		Shift,
		Control,
		Alt,
		Pause,
		Capslock,
		Pageup,
		Pagedown,
		End,
		Home,
		Up,
		Left,
		Right,
		Down,
		Printscreen,
		Insert,
		Delete,
		Lwin,
		Rwin,
		Apps,
		Zero,
		One,
		Two,
		Three,
		Four,
		Five,
		Six,
		Seven,
		Eight,
		Nine,
		Kp0,
		Kp1,
		Kp2,
		Kp3,
		Kp4,
		Kp5,
		Kp6,
		Kp7,
		Kp8,
		Kp9,
		Kpadd,
		Kpsub,
		Kpdiv,
		Kpmul,
		Kpdec,
		Kpenter,
		F1,
		F2,
		F3,
		F4,
		F5,
		F6,
		F7,
		F8,
		F9,
		F10,
		F11,
		F12,
		Numlock,
		Scrolllock,
		Space,
		Char,
		Text
	}

	public enum KeyPressType
	{
		Pressed = 1,
		Released = 2
	};

	public enum LineAlignment
	{
		Left,
		Right,
		Center
	}

	public static class SpecialCharacter
	{
		// single walls
		public const byte HLine = 196;
		public const byte VLine = 179;
		public const byte NE = 191;
		public const byte NW = 218;
		public const byte SE = 217;
		public const byte SW = 192;
		public const byte TEEW = 180;
		public const byte TEEE = 195;
		public const byte TEEN = 193;
		public const byte TEES = 194;
		public const byte Cross = 197;
		// double walls 
		public const byte DHLine = 205;
		public const byte DVLine = 186;
		public const byte DNE = 187;
		public const byte DNW = 201;
		public const byte DSE = 188;
		public const byte DSW = 200;
		public const byte DTEEW = 181;
		public const byte DTEEE = 198;
		public const byte DTEEN = 208;
		public const byte DTEES = 210;
		public const byte DCross = 213;
		// blocks       
		public const byte Block1 = 176;
		public const byte Block2 = 177;
		public const byte Block3 = 178;
		// arrows 
		public const byte Arrow_N = 24;
		public const byte Arrow_S = 25;
		public const byte Arrow_E = 26;
		public const byte Arrow_W = 27;
		// arrows without tail
		public const byte Arrow2_N = 30;
		public const byte Arrow2_S = 31;
		public const byte Arrow2_E = 16;
		public const byte Arrow2_W = 17;
		// double arrows
		public const byte DArrow_H = 29;
		public const byte DArrow_V = 18;
		// GUI stuff
		public const byte CheckboxUnset = 224;
		public const byte CheckboxSet = 225;
		public const byte RadioUnset = 9;
		public const byte RadioSet = 10;
		// sub-pixel resolution kit
		public const byte SubP_NW = 226;
		public const byte SubP_NE = 227;
		public const byte SubP_N = 228;
		public const byte SubP_SE = 229;
		public const byte SubP_Diag = 230;
		public const byte SubP_E = 231;
		public const byte SubP_SW = 232;
		/* miscellaneous */
		public const byte Smilie = 1;
		public const byte SmilieInv = 2;
		public const byte Heart = 3;
		public const byte Diamond = 4;
		public const byte Club = 5;
		public const byte Spade = 6;
		public const byte Bullet = 7;
		public const byte BulletInv = 8;
		public const byte Male = 11;
		public const byte Female = 12;
		public const byte Note = 13;
		public const byte NoteDouble = 14;
		public const byte Light = 15;
		public const byte ExclamDouble = 19;
		public const byte Pilcrow = 20;
		public const byte Section = 21;
		public const byte Pound = 156;
		public const byte Multiplication = 158;
		public const byte Function = 159;
		public const byte Reserved = 169;
		public const byte Half = 171;
		public const byte OneQuarter = 172;
		public const byte Copyright = 184;
		public const byte Cent = 189;
		public const byte Yen = 190;
		public const byte Currency = 207;
		public const byte ThreeQuarters = 243;
		public const byte Division = 246;
		public const byte Grade = 248;
		public const byte Umlaut = 249;
		public const byte Pow1 = 251;
		public const byte Pow3 = 252;
		public const byte Pow2 = 253;
		public const byte BulletSquare = 254;
	};

	public enum CustomFontRequestFontTypes : int
	{
		LayoutAsciiInColumn = 1,
		LayoutAsciiInRow = 2,
		Greyscale = 4,
		Grayscale = 4,
		LayoutTCOD = 8,
	}

	public enum FovAlgorithm
	{
		Basic = 0,
		Diamond,
		Shadow,
		Permissive0,
		Permissive1,
		Permissive2,
		Permissive3,
		Permissive4,
		Permissive5,
		Permissive6,
		Permissive7,
		Permissive8,
		Restrictive,
	};
}
