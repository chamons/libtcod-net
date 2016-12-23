using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace libtcod.tests
{
	class Program
	{
		static void Main (string[] args)
		{
			TestPathfinding ();
			Console.ReadLine ();
		}

		private static void TestPathfinding ()
		{
			using (Pathfinding path = new Pathfinding (new Size (50, 50), 1, (f, t) => 1))
			{
				path.Compute (new Point (1, 1), new Point (5, 10));
				Console.WriteLine ($"Origin: {path.Origin}");
				Console.WriteLine ($"Dest: {path.Destination}");

				Point? current = path.Origin;
				while (current.HasValue)
				{
					Console.WriteLine ($"Walk: {current.Value}");
					current = path.WalkPath (current.Value, true);
				}

				path.Compute (new Point (1, 1), new Point (5, 10));

				Console.WriteLine ($"Path[3]: {path[3]}");
				Console.WriteLine ($"Empty: {path.Empty}");
				Console.WriteLine ($"Size: {path.Size}");
			}
		}
	}
}
