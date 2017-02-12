using System;
using System.Runtime.InteropServices;

namespace libtcod
{
	public class FOV : IDisposable
	{
		public IntPtr Handle { get; private set; }

		[DllImport (Constants.LibraryName)]
		private extern static IntPtr TCOD_map_new (int width, int height);

		[DllImport (Constants.LibraryName)]
		private extern static void TCOD_map_clear (IntPtr map);

		public FOV (Size size)
		{
			Handle = TCOD_map_new (size.Width, size.Height);
			TCOD_map_clear (Handle);
		}

		public void Clear ()
		{
			TCOD_map_clear (Handle);
		}

		[DllImport (Constants.LibraryName)]
		private extern static void TCOD_map_delete (IntPtr map);

		public void Dispose ()
		{
			TCOD_map_delete (Handle);
		}

		[DllImport (Constants.LibraryName)]
		private extern static void TCOD_map_set_properties (IntPtr map, int x, int y, bool is_transparent, bool is_walkable);

		public void SetCell (Point p, bool transparent, bool walkable)
		{
			TCOD_map_set_properties (Handle, p.X, p.Y, transparent, walkable);
		}

		[DllImport (Constants.LibraryName)]
		[return: MarshalAs (UnmanagedType.I1)]
		private extern static bool TCOD_map_is_transparent (IntPtr map, int x, int y);

		[DllImport (Constants.LibraryName)]
		[return: MarshalAs (UnmanagedType.I1)]
		private extern static bool TCOD_map_is_walkable (IntPtr map, int x, int y);

		public bool GetCellTransparent (Point p)
		{
			return TCOD_map_is_transparent (Handle, p.X, p.Y);			
		}

		public bool GetCellWalkable (Point p)
		{
			return TCOD_map_is_walkable (Handle, p.X, p.Y);
		}

		[DllImport (Constants.LibraryName)]
		private extern static void TCOD_map_compute_fov (IntPtr map, int player_x, int player_y, int max_radius, bool light_walls, FovAlgorithm algo);

		public void Calculate (Point center, int radius, bool lightwalls, FovAlgorithm algo)
		{
			TCOD_map_compute_fov (Handle, center.X, center.Y, radius, lightwalls, algo);
		}

		[DllImport (Constants.LibraryName)]
		[return: MarshalAs (UnmanagedType.I1)]
		private extern static bool TCOD_map_is_in_fov (IntPtr map, int x, int y);

		public bool IsInView (Point p)
		{
			return TCOD_map_is_in_fov (Handle, p.X, p.Y);
		}
	}
}
