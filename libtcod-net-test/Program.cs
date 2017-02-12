﻿using System;
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
			System.Console.WriteLine ();
			TestBSP ();
			System.Console.WriteLine ();
			TestPathfinding ();
			System.Console.WriteLine ();
			TestConsole ();
			System.Console.ReadLine ();
		}

		static void TestConsole ()
		{
			// Not sure how to do this safely		
		}

		static void TestColor ()
		{
			Color red = ColorPresets.Red;
			float h, s, v;
			red.GetHSV (out h, out s, out v);
			System.Console.WriteLine ($"Red in HSV - {h} {s} {v}");

			Color superBlue = ColorPresets.LightBlue * 2;
			System.Console.WriteLine ($"Blue *2 - {superBlue.Red} {superBlue.Green} {superBlue.Blue}");

			Color purple = ColorPresets.Red + ColorPresets.Blue;
			System.Console.WriteLine ($"purple - {purple.Red} {purple.Green} {purple.Blue}");
		}

		static void TestBSP ()
		{
			BSP root = new BSP (Point.Empty, new Size (100, 100));
			root.SplitOnce (true, 50);
			BSP topLeft = root.Left;
			topLeft.SplitOnce (false, 30);

			root.Resize (Point.Empty, new Size (200, 200));
			System.Console.WriteLine ($"Root is leaf - {root.IsLeaf}");
			System.Console.WriteLine ($"TopLeft is leaf - {topLeft.IsLeaf}");
			System.Console.WriteLine ($"TopLeft.Left is leaf - {topLeft.Left.IsLeaf}");
			System.Console.WriteLine ($"Root contains (10,10) - {root.Contains (new Point (10, 10))}");
			System.Console.WriteLine ($"TopLeft contains (10,10) - {topLeft.Contains (new Point (10, 10))}");
			System.Console.WriteLine ($"TopLeft.Right contains (10,10) - {topLeft.Right.Contains (new Point (10, 10))}");

			System.Console.WriteLine ($"TopLeft Father - {topLeft.Father}");

			BSP container = root.Find (new Point (10, 10));
			System.Console.WriteLine ($"Smallest contains (10, 10) - {container.Position} - {container.Size}");

			root.TraverseInOrder (b =>
			{
				System.Console.WriteLine ($"TraverseInOrder - {b.Position} - {b.Size}");
				return true;
			});
			root.TraverseInvertedOrder (b =>
			{
				System.Console.WriteLine ($"TraverseInvertedOrder - {b.Position} - {b.Size}");
				return true;
			});
			root.TraverseLevelOrder (b =>
			{
				System.Console.WriteLine ($"TraverseLevelOrder - {b.Position} - {b.Size}");
				return true;
			});
			root.TraversePostOrder (b =>
			{
				System.Console.WriteLine ($"TraversePostOrder - {b.Position} - {b.Size}");
				return true;
			});
			root.TraversePreOrder (b =>
			{
				System.Console.WriteLine ($"TraversePreOrder - {b.Position} - {b.Size}");
				return true;
			});
		}

		static void TestPathfinding ()
		{
			using (Pathfinding path = new Pathfinding (new Size (50, 50), 1, (f, t) => 1))
			{
				path.Compute (new Point (1, 1), new Point (5, 10));
				System.Console.WriteLine ($"Origin: {path.Origin}");
				System.Console.WriteLine ($"Dest: {path.Destination}");

				Point? current = path.Origin;
				while (current.HasValue)
				{
					System.Console.WriteLine ($"Walk: {current.Value}");
					current = path.WalkPath (current.Value, true);
				}

				path.Compute (new Point (1, 1), new Point (5, 10));

				System.Console.WriteLine ($"Path[3]: {path[3]}");
				System.Console.WriteLine ($"Empty: {path.IsEmpty}");
				System.Console.WriteLine ($"Size: {path.Size}");
			}
		}
	}
}
