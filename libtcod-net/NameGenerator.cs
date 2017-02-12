using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace libtcod
{
	public class NameGenerator
	{
		[DllImport (Constants.LibraryName)]
		private extern static void TCOD_namegen_parse ([MarshalAs (UnmanagedType.LPStr)]string filename, IntPtr random);

		public static void LoadSyllableFile (string filename)
		{
			TCOD_namegen_parse (filename, IntPtr.Zero);
		}

		[DllImport (Constants.LibraryName)]
		private extern static void TCOD_namegen_destroy ();

		public static void Reset ()
		{
			TCOD_namegen_destroy ();
		}

		[DllImport (Constants.LibraryName)]
		private extern static IntPtr TCOD_namegen_generate ([MarshalAs (UnmanagedType.LPStr)]string name, bool allocate);

		public static string Generate (string type)
		{
			return Marshal.PtrToStringAnsi (TCOD_namegen_generate (type, false));
		}

		[DllImport (Constants.LibraryName)]
		private extern static IntPtr TCOD_namegen_generate_custom (string name, string rule, bool allocate);

		public static string GenerateCustom (string type, string rule)
		{
			return Marshal.PtrToStringAnsi (TCOD_namegen_generate_custom (type, rule, false));
		}

		[DllImport (Constants.LibraryName)]
		private extern static IntPtr TCOD_namegen_get_sets ();

		[DllImport (Constants.LibraryName)]
		private extern static IntPtr TCOD_list_get (IntPtr l, int idx);

		[DllImport (Constants.LibraryName)]
		private extern static int TCOD_list_size (IntPtr l);

		[DllImport (Constants.LibraryName)]
		private extern static void TCOD_list_delete (IntPtr l);

		public static List<string> GetSet ()
		{
			List<string> returnList = new List<string> ();
			IntPtr tcodStringList = TCOD_namegen_get_sets ();

			int listSize = TCOD_list_size (tcodStringList);
			for (int i = 0; i < listSize; ++i)
			{
				string str = Marshal.PtrToStringAnsi (TCOD_list_get (tcodStringList, i));
				returnList.Add (str);
			}

			TCOD_list_delete (tcodStringList);
			return returnList;
		}
	}
}