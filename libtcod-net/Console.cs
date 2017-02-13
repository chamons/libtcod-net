using System;
using System.Runtime.InteropServices;

namespace libtcod
{
	public class Background
	{
		internal int Value;

		public BackgroundFlag BackgroundFlag => (BackgroundFlag)(Value & 0xff);
		public byte AlphaValue => (byte)(Value >> 8);

		public static readonly Background None = new Background (BackgroundFlag.None);
		public static readonly Background Set = new Background (BackgroundFlag.Set);

		public Background (BackgroundFlag flag)
		{
			if (flag == BackgroundFlag.AddA || flag == BackgroundFlag.Alph)
				throw new InvalidOperationException ($"Must use Background constructor which takes value for {flag}");
			Value = (int)flag;
		}

		public Background (BackgroundFlag flag, double val)
		{
			if (flag != BackgroundFlag.AddA && flag != BackgroundFlag.Alph)
				throw new InvalidOperationException ($"Must not use Background constructor which takes value for {flag}");
			Value = (int)flag | (((byte)(val * 255)) << 8);
		}
	}

	public class CustomFontRequest
	{
		public String FontFile { get; private set; }
		public int NumberHorizontalChars { get; private set; }
		public int NumberVerticalChars { get; private set; }
		public CustomFontRequestFontTypes FontRequestType { get; private set; }

		public CustomFontRequest (String fontFile, CustomFontRequestFontTypes type, int numberHorizontalChars, int numberVerticalChars)
		{
			FontFile = fontFile;
			NumberHorizontalChars = numberHorizontalChars;
			NumberVerticalChars = numberVerticalChars;
			FontRequestType = type;
		}
	}

	public class ConsoleWindow : IDisposable
	{
		internal ConsoleWindow (IntPtr handle, int width, int height)
		{
			Handle = handle;
			Width = width;
			Height = height;
		}

		[DllImport (Constants.LibraryName)]
		private extern static void TCOD_console_delete (IntPtr handle);

		public virtual void Dispose ()
		{
			if (Handle != IntPtr.Zero)
				TCOD_console_delete (Handle);
		}

		public IntPtr Handle { get; private set; }
		public int Width { get; private set; }
		public int Height { get; private set; }

		[DllImport (Constants.LibraryName)]
		private extern static Color TCOD_console_get_default_foreground (IntPtr handle);

		[DllImport (Constants.LibraryName)]
		private extern static void TCOD_console_set_default_foreground (IntPtr handle, Color color);

		public Color DefaultForegroundColor
		{
			get { return TCOD_console_get_default_foreground (Handle); }
			set { TCOD_console_set_default_foreground (Handle, value); }
		}

		[DllImport (Constants.LibraryName)]
		private extern static Color TCOD_console_get_default_background (IntPtr handle);

		[DllImport (Constants.LibraryName)]
		private extern static void TCOD_console_set_default_background (IntPtr handle, Color color);

		public Color DefaultBackgroundColor
		{
			get { return TCOD_console_get_default_background (Handle); }
			set { TCOD_console_set_default_background (Handle, value); }
		}

		[DllImport (Constants.LibraryName)]
		private extern static void TCOD_console_clear (IntPtr handle);

		public void Clear ()
		{
			TCOD_console_clear (Handle);
		}

		[DllImport (Constants.LibraryName)]
		private extern static void TCOD_console_put_char (IntPtr handle, int x, int y, int c, /*BackgroundFlag*/ int flag);

		public void PutChar (Point p, char c, Background flag)
		{
			TCOD_console_put_char (Handle, p.X, p.Y, (int)c, flag.Value);
		}

		public void PutChar (Point p, byte c, Background flag)
		{
			TCOD_console_put_char (Handle, p.X, p.Y, c, flag.Value);
		}

		public void PutChar (Point p, char c)
		{
			TCOD_console_put_char (Handle, p.X, p.Y, (int)c, (int)BackgroundFlag.Set);
		}

		public void PutChar (Point p, byte c)
		{
			TCOD_console_put_char (Handle, p.X, p.Y, c, (int)BackgroundFlag.Set);
		}

		[DllImport (Constants.LibraryName)]
		private extern static void TCOD_console_set_char_background (IntPtr con, int x, int y, Color col, /*BackgroundFlag*/ int flag);

		public void SetBackground (Point p, Color col, Background flag)
		{
			TCOD_console_set_char_background (Handle, p.X, p.Y, col, flag.Value);
		}

		public void SetBackground (Point p, Color col)
		{
			TCOD_console_set_char_background (Handle, p.X, p.Y, col, (int)BackgroundFlag.Set);
		}

		[DllImport (Constants.LibraryName)]
		private extern static void TCOD_console_set_char_foreground (IntPtr con, int x, int y, Color col);

		public void SetForeground (Point p, Color col)
		{
			TCOD_console_set_char_foreground (Handle, p.X, p.Y, col);
		}

		[DllImport (Constants.LibraryName)]
		private extern static Color TCOD_console_get_char_background (IntPtr con, int x, int y);

		public Color GetBackground (Point p)
		{
			return TCOD_console_get_char_background (Handle, p.X, p.Y);
		}

		[DllImport (Constants.LibraryName)]
		private extern static Color TCOD_console_get_char_foreground (IntPtr con, int x, int y);

		public Color GetForeground (Point p)
		{
			return TCOD_console_get_char_foreground (Handle, p.X, p.Y);
		}

		[DllImport (Constants.LibraryName)]
		private extern static int TCOD_console_get_char (IntPtr handle, int x, int y);

		public char GetCharacter (Point p)
		{
			return (char)TCOD_console_get_char (Handle, p.X, p.Y);
		}

		[DllImport (Constants.LibraryName)]
		private extern static void TCOD_console_set_char (IntPtr handle, int x, int y, int c);

		public void SetCharacter (Point p, char c)
		{
			TCOD_console_set_char (Handle, p.X, p.Y, (int)c);
		}

		public void PrintLine (string str, Point p, LineAlignment align = LineAlignment.Left)
		{
			PrintLine (str, p, Background.Set, align);
		}

		[DllImport (Constants.LibraryName)]
		private extern static void TCOD_console_print_ex (IntPtr handle, int x, int y, /*BackgroundFlag*/ int background, /* LineAlignment */ int alignment, [MarshalAs (UnmanagedType.LPStr)]string s);
		
		public void PrintLine (string str, Point p, Background background, LineAlignment align)
		{
			TCOD_console_print_ex (Handle, p.X, p.Y, background.Value, (int)align, str);
		}

		public int PrintLineRect (string str, Rectangle rect, LineAlignment align)
		{
			return PrintLineRect (str, rect, Background.Set, align);
		}

		[DllImport (Constants.LibraryName)]
		private extern static int TCOD_console_print_rect_ex (IntPtr handle, int x, int y, int w, int h, /*BackgroundFlag*/ int background, /* LineAlignment */ int alignment, [MarshalAs (UnmanagedType.LPStr)]string s);

		public int PrintLineRect (string str, Rectangle rect, Background background, LineAlignment align)
		{
			return TCOD_console_print_rect_ex (Handle, rect.X, rect.Y, rect.Width, rect.Height, background.Value, (int)align, str);
		}

		[DllImport (Constants.LibraryName)]
		private extern static int TCOD_console_get_height_rect (IntPtr handle, int x, int y, int w, int h, [MarshalAs (UnmanagedType.LPStr)]string s);

		public int GetPrintHeight (string str, Rectangle rect)
		{
			return TCOD_console_get_height_rect (Handle, rect.X, rect.Y, rect.Width, rect.Height, str);
		}

		[DllImport (Constants.LibraryName)]
		private extern static void TCOD_console_blit (IntPtr handle, int xSrc, int ySrc, int wSrc, int hSrc, IntPtr dst, int xDst, int yDst, float foregroundAlpha, float backgroundAlpha);

		public void Blit (Rectangle source, ConsoleWindow dest, int xDst, int yDst)
		{
			Blit (source, dest, xDst, yDst, 1.0f, 1.0f);
		}

		public void Blit (Rectangle source, ConsoleWindow dest, int xDst, int yDst, float foregroundAlpha, float backgroundAlpha)
		{
			TCOD_console_blit (Handle, source.X, source.Y, source.Width, source.Height, dest.Handle, xDst, yDst, foregroundAlpha, backgroundAlpha);
		}

		[DllImport (Constants.LibraryName)]
		private extern static void TCOD_console_rect (IntPtr handle, int x, int y, int w, int h, bool clear, /*BackgroundFlag*/ int flag);

		public void DrawRect (Rectangle rect, bool clear)
		{
			DrawRect (rect, clear, Background.Set);
		}

		public void DrawRect (Rectangle rect, bool clear, Background flag)
		{
			TCOD_console_rect (Handle, rect.X, rect.Y, rect.Width, rect.Height, clear, flag.Value);
		}

		[DllImport (Constants.LibraryName)]
		private extern static void TCOD_console_hline (IntPtr handle, int x, int y, int l);

		public void DrawHLine (Point p, int l)
		{
			TCOD_console_hline (Handle, p.X, p.Y, l);
		}

		[DllImport (Constants.LibraryName)]
		private extern static void TCOD_console_vline (IntPtr handle, int x, int y, int l);

		public void DrawVLine (Point p, int l)
		{
			TCOD_console_vline (Handle, p.X, p.Y, l);
		}

		[DllImport (Constants.LibraryName)]
		private extern static void TCOD_console_print_frame (IntPtr handle, int x, int y, int w, int h, bool clear, /*BackgroundFlag*/ int flag, [MarshalAs (UnmanagedType.LPStr)]string str);

		public void DrawFrame (Rectangle rect, bool clear, string str)
		{
			TCOD_console_print_frame (Handle, rect.X, rect.Y, rect.Width, rect.Height, clear, Background.Set.Value, str);
		}

		[DllImport (Constants.LibraryName)]
		private extern static void TCOD_console_print_frame (IntPtr handle, int x, int y, int w, int h, bool clear, /*BackgroundFlag*/ int flag, IntPtr nullStr);

		public void DrawFrame (Rectangle rect, bool clear, Background flag, string str)
		{
			TCOD_console_print_frame (Handle, rect.X, rect.Y, rect.Width, rect.Height, clear, flag.Value, str);
		}

		public void DrawFrame (Rectangle rect, bool clear)
		{
			TCOD_console_print_frame (Handle, rect.X, rect.Y, rect.Width, rect.Height, clear, Background.Set.Value, IntPtr.Zero);
		}

		public void DrawFrame (Rectangle rect, bool clear, Background flag)
		{
			TCOD_console_print_frame (Handle, rect.X, rect.Y, rect.Width, rect.Height, clear, flag.Value, IntPtr.Zero);
		}

		[DllImport (Constants.LibraryName)]
		private static extern void TCOD_console_credits ();

		public void ConsoleCredits ()
		{
			TCOD_console_credits ();
		}

		[DllImport (Constants.LibraryName)]
		private static extern byte TCOD_console_credits_render (int x, int y, bool alpha);

		public bool ConsoleCreditsRender (Point p, bool alpha)
		{
			return TCOD_console_credits_render (p.X, p.Y, alpha) == 1;
		}

		[DllImport (Constants.LibraryName)]
		private extern static void TCOD_console_credits_reset ();

		public void ResetCreditsAnimation ()
		{
			TCOD_console_credits_reset ();
		}
	}

	public enum ConsoleRender
	{
		GLSL,
		OPENGL,
		SDL
	}

	public class RootConsoleWindow : ConsoleWindow
	{
		public override void Dispose ()
		{
			// Don't dispose root console
		}

		[DllImport (Constants.LibraryName)]
		private extern static void TCOD_console_init_root (int w, int h, [MarshalAs (UnmanagedType.LPStr)]string title, bool fullscreen, ConsoleRender renderer);

		private RootConsoleWindow (Size s, String title, bool fullscreen, ConsoleRender renderer)
			: base (IntPtr.Zero, s.Width, s.Height)
		{
			TCOD_console_init_root (s.Width, s.Height, title, fullscreen, renderer);
		}

		[DllImport (Constants.LibraryName)]
		private extern static void TCOD_console_set_custom_font ([MarshalAs (UnmanagedType.LPStr)] string fontFile, int flags, int numberCharsHoriz, int numberCharsVert);

		private RootConsoleWindow (Size s, String title, bool fullscreen, CustomFontRequest font, ConsoleRender renderer)
			: base (IntPtr.Zero, s.Width, s.Height)
		{
			TCOD_console_set_custom_font (font.FontFile, (int)font.FontRequestType, font.NumberHorizontalChars, font.NumberVerticalChars);
			TCOD_console_init_root (s.Width, s.Height, title, fullscreen, renderer);
		}

		[DllImport (Constants.LibraryName)]
		[return: MarshalAs (UnmanagedType.I1)]
		private extern static bool TCOD_console_is_window_closed ();

		public bool IsWindowClosed
		{
			get
			{
				return TCOD_console_is_window_closed ();
			}			
		}

		[DllImport (Constants.LibraryName)]
		[return: MarshalAs (UnmanagedType.I1)]
		private extern static bool TCOD_console_is_active ();

		public bool IsActive
		{
			get
			{
				return TCOD_console_is_active ();
			}
		}

		[DllImport (Constants.LibraryName)]
		[return: MarshalAs (UnmanagedType.I1)]
		private extern static bool TCOD_console_has_mouse_focus ();

		public bool HasMouseFocus
		{
			get
			{
				return TCOD_console_has_mouse_focus ();
			}
		}

		[DllImport (Constants.LibraryName)]
		private extern static void TCOD_console_flush ();

		public void Flush ()
		{
			TCOD_console_flush ();
		}

		[DllImport (Constants.LibraryName)]
		private extern static void TCOD_console_set_fade (byte fade, Color fadingColor);

		public void SetFade (byte fade, Color fadingColor)
		{
			TCOD_console_set_fade (fade, fadingColor);
		}

		[DllImport (Constants.LibraryName)]
		private extern static byte TCOD_console_get_fade ();

		public byte GetFadeLevel ()
		{
			return TCOD_console_get_fade ();
		}

		[DllImport (Constants.LibraryName)]
		private extern static Color TCOD_console_get_fading_color ();

		public Color GetFadeColor ()
		{
			return TCOD_console_get_fading_color ();
		}

		[DllImport (Constants.LibraryName)]
		private extern static void TCOD_console_set_fullscreen (bool fullscreen);

		[DllImport (Constants.LibraryName)]
		[return: MarshalAs (UnmanagedType.I1)]
		private extern static bool TCOD_console_is_fullscreen ();

		public bool IsFullscreen
		{
			get
			{
				return TCOD_console_is_fullscreen ();
			}
			set
			{
				TCOD_console_set_fullscreen (value);
			}
		}

		[DllImport (Constants.LibraryName)]
		private extern static void TCOD_console_set_window_title ([MarshalAs (UnmanagedType.LPStr)]string title);

		public void SetWindowTitle (string title)
		{
			TCOD_console_set_window_title (title);
		}

		[DllImport (Constants.LibraryName)]
		private extern static IntPtr TCOD_console_new (int w, int h);

		public static ConsoleWindow CreateOffscreenConsole (Size s)
		{
			return new ConsoleWindow (TCOD_console_new (s.Width, s.Height), s.Width, s.Height);
		}

		public static RootConsoleWindow Instance { get; private set; }

		public static RootConsoleWindow Setup (int width, int height, string windowTitle, bool isFullscreen, ConsoleRender render, CustomFontRequest font = null)
		{
			if (Instance != null)
				throw new InvalidOperationException ("Can not setup RootConsole twice");

			if (font == null)
				Instance = new RootConsoleWindow (new Size (width, height), windowTitle, isFullscreen, render);
			else
				Instance = new RootConsoleWindow (new Size (width, height), windowTitle, isFullscreen, font, render);

			return Instance;
		}
	}
}