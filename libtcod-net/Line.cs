using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace libtcod
{
	public static class Line
	{
		[DllImport (Constants.LibraryName)]
		private extern static void TCOD_line_init (int xFrom, int yFrom, int xTo, int yTo);

		public static void Setup (Point from, Point to)
		{
			TCOD_line_init (from.X, from.Y, to.X, to.Y);
		}

		[DllImport (Constants.LibraryName)]
		[return: MarshalAs (UnmanagedType.I1)]
		private extern static bool TCOD_line_step (ref int xCur, ref int yCur);

		public static IEnumerable<Point> GetPoints (Point starting)
		{
			int x = starting.X;
			int y = starting.Y;
			while (!TCOD_line_step (ref x, ref y))
			{
				yield return new Point (x, y);
			}
		}
	}
}
