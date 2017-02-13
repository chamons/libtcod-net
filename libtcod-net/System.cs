using System;
using System.Runtime.InteropServices;

namespace libtcod
{
	[Flags]
	public enum EventType
	{
		None = 0,
		KeyPress = 1,
		KeyRelease = 2,
		Key = KeyPress | KeyRelease,
		MouseMove = 4,
		MousePress = 8,
		MouseRelease = 16,
		Mouse = MouseMove | MousePress | MouseRelease,
		Any = Mouse | Key
	}

	public static class SystemAPI
	{
		[DllImport (Constants.LibraryName)]
		private extern static uint TCOD_sys_elapsed_milli ();

		public static uint ElapsedMilliseconds
		{
			get { return TCOD_sys_elapsed_milli (); }
		}

		[DllImport (Constants.LibraryName)]
		private extern static float TCOD_sys_elapsed_seconds ();

		public static float ElapsedSeconds
		{
			get { return TCOD_sys_elapsed_seconds (); }
		}

		[DllImport (Constants.LibraryName)]
		private extern static void TCOD_sys_sleep_milli (uint val);

		public static void Sleep (uint milliseconds)
		{
			TCOD_sys_sleep_milli (milliseconds);
		}

		[DllImport (Constants.LibraryName)]
		private extern static void TCOD_sys_save_screenshot ([MarshalAs (UnmanagedType.LPStr)] string filename);

		public static void SaveScreenshot (string fileName)
		{
			TCOD_sys_save_screenshot (fileName);
		}

		[DllImport (Constants.LibraryName)]
		private extern static void TCOD_sys_force_fullscreen_resolution (int width, int height);

		public static void ForceFullscrenResolution (int width, int height)
		{
			TCOD_sys_force_fullscreen_resolution (width, height);
		}

		[DllImport (Constants.LibraryName)]
		private extern static void TCOD_sys_get_current_resolution (out int w, out int h);

		public static Size CurrentResolution
		{
			get
			{
				int w;
				int h;
				TCOD_sys_get_current_resolution (out w, out h);
				return new Size (w, h);
			}
		}

		[DllImport (Constants.LibraryName)]
		private extern static float TCOD_sys_get_last_frame_length ();

		public static float LastFrameLength
		{
			get { return TCOD_sys_get_last_frame_length (); }
		}

		[DllImport (Constants.LibraryName)]
		private extern static void TCOD_sys_get_char_size (out int w, out int h);

		public static Size CurrentFontSize
		{
			get
			{
				int w;
				int h;
				TCOD_sys_get_char_size (out w, out h);
				return new Size (w, h);
			}
		}

		[DllImport (Constants.LibraryName)]
		private extern static void TCOD_sys_set_fps (int val);

		[DllImport (Constants.LibraryName)]
		private extern static int TCOD_sys_get_fps ();

		public static int FPS
		{
			get { return TCOD_sys_get_fps (); }
			set { TCOD_sys_set_fps (value); }
		}

	


		[DllImport (Constants.LibraryName)]
		private extern static EventType TCOD_sys_wait_for_event (int mask, ref KeyPress key, ref Mouse mouse, bool flush);

		[DllImport (Constants.LibraryName)]
		private extern static EventType TCOD_sys_check_for_event (int mask, ref KeyPress key, ref Mouse mouse);

		public static EventType WaitForEvent (EventType eventMask, ref KeyPress key, ref Mouse mouse, bool flush)
		{
			return TCOD_sys_wait_for_event ((int)eventMask, ref key, ref mouse, flush);
		}

		public static EventType CheckForEvent (EventType eventMask, ref KeyPress key, ref Mouse mouse)
		{
			return TCOD_sys_check_for_event ((int)eventMask, ref key, ref mouse);
		}
	}
}