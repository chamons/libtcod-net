using System.Runtime.InteropServices;

namespace libtcod
{
	[StructLayout (LayoutKind.Sequential)]
	public struct Mouse
	{
		int x;
		int y;
		int dx;
		int dy;
		int cx;
		int cy;
		int dcx;
		int dcy;
		byte lbutton;
		byte rbutton;
		byte mbutton;
		byte lbutton_pressed;
		byte rbutton_pressed;
		byte mbutton_pressed;
		byte wheel_up;
		byte wheel_down;

		public Point PixelLocation => new Point (x, y);
		public Point PixelVelocity => new Point (dx, dy);
		public Point CellLocation => new Point (cx, cy);
		public Point CellVelocity => new Point (dcx, dcy);

		public bool LeftButton => lbutton == 1;
		public bool RightButton => rbutton == 1;
		public bool MiddleButton => mbutton == 1;
		public bool LeftButtonPressed => lbutton_pressed == 1;
		public bool RightButtonPressed => rbutton_pressed == 1;
		public bool MiddleButtonPressed => mbutton_pressed == 1;
		public bool WheelUp => wheel_up == 1;
		public bool WheelDown => wheel_down == 1;

		[DllImport (Constants.LibraryName)]
		private extern static void TCOD_mouse_show_cursor (bool visible);

		public static void ShowCursor (bool visible)
		{
			TCOD_mouse_show_cursor (visible);
		}

		[DllImport (Constants.LibraryName)]
		[return: MarshalAs (UnmanagedType.I1)]
		private extern static bool TCOD_mouse_is_cursor_visible ();

		public static bool IsCursorVisible
		{
			get { return TCOD_mouse_is_cursor_visible (); }
		}

		[DllImport (Constants.LibraryName)]
		private extern static void TCOD_mouse_move (int x, int y);
		public static void MoveMouse (int x, int y)
		{
			TCOD_mouse_move (x, y);
		}

		[DllImport (Constants.LibraryName)]
		private extern static Mouse TCOD_mouse_get_status ();

		public static Mouse GetStatus ()
		{
			return TCOD_mouse_get_status ();
		}
	}
}
