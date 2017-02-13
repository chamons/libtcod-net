using System;
using System.Runtime.InteropServices;

namespace libtcod
{
	public unsafe struct KeyPress
	{
		KeyCode keyCode;
		byte character;

		fixed byte text [32];

		byte pressed;
		byte lalt;
		byte lctrl;
		byte lmeta;
		byte ralt;
		byte rctrl;
		byte rmeta;
		byte shift;

		public KeyCode KeyCode => keyCode;
		public char Character => (char)character;

		public unsafe string Text
		{
			get
			{
				fixed (byte* ptr = text)
				{
					return Marshal.PtrToStringAnsi ((IntPtr)ptr);
				}
			}
		}

		public bool Pressed => pressed == 1;
		public bool LeftAlt => lalt == 1;
		public bool LeftControl => lctrl == 1;
		public bool RightAlt => ralt == 1;
		public bool RightControl => rctrl == 1;
		public bool Shift => shift == 1;
		public bool Alt => LeftAlt || RightAlt;
		public bool Control => LeftControl || RightControl;
	}

	public static class Keyboard
	{
		[DllImport (Constants.LibraryName)]
		private extern static KeyPress TCOD_console_wait_for_keypress (bool flush);

		public static KeyPress WaitForKeyPress (bool flushInputBuffer)
		{
			return TCOD_console_wait_for_keypress (flushInputBuffer);
		}

		[DllImport (Constants.LibraryName)]
		private extern static KeyPress TCOD_console_check_for_keypress (KeyPressType flags);

		public static KeyPress CheckForKeypress (KeyPressType pressFlags)
		{
				return TCOD_console_check_for_keypress (pressFlags);
		}

		[DllImport (Constants.LibraryName)]
		[return: MarshalAs (UnmanagedType.I1)]
		private extern static bool TCOD_console_is_key_pressed (KeyCode key);

		public static bool IsKeyPressed (KeyCode key)
		{
			return TCOD_console_is_key_pressed (key);
		}

		[DllImport (Constants.LibraryName)]
		private extern static void TCOD_console_set_keyboard_repeat (int initial_delay, int interval);

		public static void SetRepeat (int initialDelay, int interval)
		{
			TCOD_console_set_keyboard_repeat (initialDelay, interval);
		}

		[DllImport (Constants.LibraryName)]
		private extern static void TCOD_console_disable_keyboard_repeat ();

		public static void DisableRepeat ()
		{
			TCOD_console_disable_keyboard_repeat ();
		}
	}
}
