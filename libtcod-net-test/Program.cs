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
			TestColor ();
			Console.WriteLine ();
			TestBSP ();
			Console.WriteLine ();
			TestPathfinding ();
			Console.ReadLine ();
		}

		static void TestColor ()
		{
			Color red = ColorPresets.Red;
			float h, s, v;
			red.GetHSV (out h, out s, out v);
			Console.WriteLine ($"Red in HSV - {h} {s} {v}");

			Color superBlue = ColorPresets.LightBlue * 2;
			Console.WriteLine ($"Blue *2 - {superBlue.Red} {superBlue.Green} {superBlue.Blue}");

			Color purple = ColorPresets.Red + ColorPresets.Blue;
			Console.WriteLine ($"purple - {purple.Red} {purple.Green} {purple.Blue}");
		}

		static void TestBSP ()
		{
			BSP root = new BSP (Point.Empty, new Size (100, 100));
			root.SplitOnce (true, 50);
			BSP topLeft = root.Left;
			topLeft.SplitOnce (false, 30);

			root.Resize (Point.Empty, new Size (200, 200));
			Console.WriteLine ($"Root is leaf - {root.IsLeaf}");
			Console.WriteLine ($"TopLeft is leaf - {topLeft.IsLeaf}");
			Console.WriteLine ($"TopLeft.Left is leaf - {topLeft.Left.IsLeaf}");
			Console.WriteLine ($"Root contains (10,10) - {root.Contains (new Point (10, 10))}");
			Console.WriteLine ($"TopLeft contains (10,10) - {topLeft.Contains (new Point (10, 10))}");
			Console.WriteLine ($"TopLeft.Right contains (10,10) - {topLeft.Right.Contains (new Point (10, 10))}");

			Console.WriteLine ($"TopLeft Father - {topLeft.Father}");

			BSP container = root.Find (new Point (10, 10));
			Console.WriteLine ($"Smallest contains (10, 10) - {container.Position} - {container.Size}");

			root.TraverseInOrder (b =>
			{
				Console.WriteLine ($"TraverseInOrder - {b.Position} - {b.Size}");
				return true;
			});
			root.TraverseInvertedOrder (b =>
			{
				Console.WriteLine ($"TraverseInvertedOrder - {b.Position} - {b.Size}");
				return true;
			});
			root.TraverseLevelOrder (b =>
			{
				Console.WriteLine ($"TraverseLevelOrder - {b.Position} - {b.Size}");
				return true;
			});
			root.TraversePostOrder (b =>
			{
				Console.WriteLine ($"TraversePostOrder - {b.Position} - {b.Size}");
				return true;
			});
			root.TraversePreOrder (b =>
			{
				Console.WriteLine ($"TraversePreOrder - {b.Position} - {b.Size}");
				return true;
			});
		}

		static void TestPathfinding ()
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
				Console.WriteLine ($"Empty: {path.IsEmpty}");
				Console.WriteLine ($"Size: {path.Size}");
			}
		}
	}
}
