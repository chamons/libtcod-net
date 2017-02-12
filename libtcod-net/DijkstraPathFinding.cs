using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace libtcod
{
	public class DijkstraPathfinding : PathfindingBase
	{
		[DllImport (Constants.LibraryName)]
		private extern static IntPtr TCOD_dijkstra_new_using_function (int map_width, int map_height, PathCostCallbackInternal func, IntPtr nullData, float diagonalCost);

		public DijkstraPathfinding (Size size, double diagonalCost, PathCostDelegate callback)
		{
			SetupCallback (callback);
			Handle = TCOD_dijkstra_new_using_function (size.Width, size.Height, Trampoline, IntPtr.Zero, (float)diagonalCost);
		}

		[DllImport (Constants.LibraryName)]
		private extern static IntPtr TCOD_dijkstra_new (IntPtr map, float diagonalCost);

		public DijkstraPathfinding (FOV fovMap, double diagonalCost)
		{
			Handle = TCOD_dijkstra_new (fovMap.Handle, (float)diagonalCost);
		}

		[DllImport (Constants.LibraryName)]
		[return: MarshalAs (UnmanagedType.I1)]
		private extern static void TCOD_dijkstra_compute (IntPtr path, int origX, int origY);

		public void Compute (Point original)
		{
			TCOD_dijkstra_compute (Handle, original.X, original.Y);
		}

		[DllImport (Constants.LibraryName)]
		private extern static float TCOD_dijkstra_get_distance (IntPtr dijkstra, int x, int y);

		public float GetDistance (Point target)
		{
			return TCOD_dijkstra_get_distance (Handle, target.X, target.Y);
		}

		[DllImport (Constants.LibraryName)]
		[return: MarshalAs (UnmanagedType.I1)]
		private extern static bool TCOD_dijkstra_path_set (IntPtr dijkstra, int x, int y);

		public bool SetPath (Point dest)
		{
			return TCOD_dijkstra_path_set (Handle, dest.X, dest.Y);
		}

		[DllImport (Constants.LibraryName)]
		[return: MarshalAs (UnmanagedType.I1)]
		private extern static bool TCOD_dijkstra_path_walk (IntPtr path, ref int x, ref int y);

		public List<Point> Generate ()
		{
			List<Point> path = new List<Point> ();
			int x = 0;
			int y = 0;
			while (!IsEmpty)
			{
				if (TCOD_dijkstra_path_walk (Handle, ref x, ref y))
					path.Add (new Point (x, y));
				else
					throw new InvalidOperationException ("DijkstraPathfinding got stuck");
			}
			return path;
		}

		[DllImport (Constants.LibraryName)]
		[return: MarshalAs (UnmanagedType.I1)]
		private extern static bool TCOD_dijkstra_is_empty (IntPtr path);

		public bool IsEmpty
		{
			get
			{
				return TCOD_dijkstra_is_empty (Handle);
			}
		}

		[DllImport (Constants.LibraryName)]
		private extern static int TCOD_dijkstra_size (IntPtr path);

		public int Length
		{
			get
			{
				return TCOD_dijkstra_size (Handle);
			}
		}

		[DllImport (Constants.LibraryName)]
		private extern static void TCOD_dijkstra_get (IntPtr path, int index, out int x, out int y);

		public Point this [int key]
		{
			get
			{
				int x;
				int y;
				TCOD_dijkstra_get (Handle, key, out x, out y);
				return new Point (x, y);
			}
		}

		[DllImport (Constants.LibraryName)]
		private extern static void TCOD_dijkstra_delete (IntPtr path);

		public override void Dispose ()
		{
			TCOD_dijkstra_delete (Handle);
		}
	}
}
