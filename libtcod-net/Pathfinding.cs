using System;
using System.Runtime.InteropServices;

namespace libtcod
{
	public class Pathfinding : PathfindingBase
	{
		[DllImport (Constants.LibraryName)]
		extern static IntPtr TCOD_path_new_using_function (int map_width, int map_height, PathCostCallbackInternal func, IntPtr nullData, float diagonalCost);

		public Pathfinding (Size size, double diagonalCost, PathCostDelegate callback)
		{
			SetupCallback (callback);
			Handle = TCOD_path_new_using_function (size.Width, size.Height, Trampoline, IntPtr.Zero, (float)diagonalCost);
		}

		[DllImport (Constants.LibraryName)]
		extern static IntPtr TCOD_path_new_using_map (IntPtr map, float diagonalCost);

		public Pathfinding (Map map, double diagonalCost)
		{
			Handle = TCOD_path_new_using_map (map.Handle, (float)diagonalCost);
		}

		[DllImport (Constants.LibraryName)]
		[return: MarshalAs (UnmanagedType.I1)]
		extern static bool TCOD_path_compute (IntPtr path, int origX, int origY, int destX, int destY);

		public bool Compute (Point original, Point destination)
		{
			return TCOD_path_compute (Handle, original.X, original.Y, destination.X, destination.Y);
		}

		[DllImport (Constants.LibraryName)]
		[return: MarshalAs (UnmanagedType.I1)]
		private extern static bool TCOD_path_walk (IntPtr path, ref int x, ref int y, bool recalculate_when_needed);

		public Point? WalkPath (Point currentPosition, bool recalculateWhenNeeded)
		{
			int x = currentPosition.X;
			int y = currentPosition.Y;
			bool notStuck = TCOD_path_walk (Handle, ref x, ref y, recalculateWhenNeeded);
			return notStuck ? (Point?)new Point (x, y) : null;
		}

		[DllImport (Constants.LibraryName)]
		extern static void TCOD_path_get (IntPtr path, int index, out int x, out int y);

		public Point this[int index]
		{
			get
			{
				int x, y;
				TCOD_path_get (Handle, index, out x, out y);
				return new Point (x, y);
			}
		}

		[DllImport (Constants.LibraryName)]
		[return: MarshalAs (UnmanagedType.I1)]
		extern static bool TCOD_path_is_empty (IntPtr path);

		public bool IsEmpty
		{
			get
			{
				return TCOD_path_is_empty (Handle);
			}
		}

		[DllImport (Constants.LibraryName)]
		extern static int TCOD_path_size (IntPtr path);

		public int Size
		{
			get
			{
				return TCOD_path_size (Handle);
			}
		}

		[DllImport (Constants.LibraryName)]
		extern static void TCOD_path_get_origin (IntPtr path, out int x, out int y);

		public Point Origin
		{
			get
			{
				int x, y;
				TCOD_path_get_origin (Handle, out x, out y);
				return new Point (x, y);
			}
		}

		[DllImport (Constants.LibraryName)]
		extern static void TCOD_path_get_destination (IntPtr path, out int x, out int y);

		public Point Destination
		{
			get
			{
				int x, y;
				TCOD_path_get_destination (Handle, out x, out y);
				return new Point (x, y);
			}
		}

		[DllImport (Constants.LibraryName)]
		extern static void TCOD_path_delete (IntPtr path);

		public override void Dispose ()
		{
			TCOD_path_delete (Handle);
		}
	}
}
