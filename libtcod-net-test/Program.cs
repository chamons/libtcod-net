using System;

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
			Console.WriteLine ();
			TestConsole ();
			Console.WriteLine ();
			TestDijkstra ();
			Console.WriteLine ();
			TestFOV ();
			Console.WriteLine ();
			TestLine ();
			Console.WriteLine ();
			TestNamegen ();
			Console.WriteLine ();
			TestNoise ();
			Console.WriteLine ();
			TestRandom ();
			Console.WriteLine ();
			TestSystem ();
			Console.ReadLine ();
		}

		static void TestSystem ()
		{
			SystemAPI.Sleep (100);
			// These return zero since message pump from GUI is not setup here
			Console.WriteLine (SystemAPI.ElapsedMilliseconds);
			Console.WriteLine (SystemAPI.ElapsedSeconds);

			// Can not test screenshot since we don't have GUI
			//SystemAPI.SaveScreenshot (@"C:\Users\chris\Desktop\foo.png");
		}

		static void TestRandom ()
		{
			using (var defaultRandom = Random.Default)
			{
				Console.WriteLine (defaultRandom.GetInt (1, 4));
			}

			using (var random = new Random (RandomTypes.MersenneTwister, 42))
			{
				Console.WriteLine (random.GetInt (1, 4));
				Console.WriteLine (random.GetFloat (1, 4));
				Console.WriteLine (random.GetDouble (1, 4));

				random.SetRandomDistribution (RandomDistribution.Gaussian);
				Console.WriteLine (random.GetInt (1, 4, 3));
				Console.WriteLine (random.GetFloat (1, 4, 3));
				Console.WriteLine (random.GetDouble (1, 4, 3));
			}
		}

		static void TestNoise ()
		{
			using (Noise s = new Noise (1))
			{
				float[] f = new float[] { .3f };
				Console.WriteLine (s.GetNoise (f));
				Console.WriteLine (s.GetFBMNoise (f, 4));
				Console.WriteLine (s.GetTurbulence (f, 4));
			}
		}

		static void TestNamegen ()
		{
			NameGenerator.LoadSyllableFile ("namegen/mingos_town.cfg");
			Console.WriteLine (NameGenerator.Generate ("Mingos town"));
			foreach (var item in NameGenerator.GetSet ())
				Console.WriteLine (item);
		}

		static void TestLine ()
		{
			Line.Setup (new Point (1, 1), new Point (5, 3));
			foreach (Point p in Line.GetPoints (new Point (1, 1)))
				Console.WriteLine (p);
		}

		static void TestFOV ()
		{
			using (FOV f = new FOV (new Size (10, 10)))
			{
				// . # .
				// # . .
				// . . .
				f.SetCell (new Point (0, 0), true, true);
				f.SetCell (new Point (1, 0), false, false);
				f.SetCell (new Point (2, 0), true, true);
				f.SetCell (new Point (0, 1), false, false);
				f.SetCell (new Point (1, 1), true, true);
				f.SetCell (new Point (2, 1), true, true);
				f.SetCell (new Point (0, 2), true, true);
				f.SetCell (new Point (1, 2), true, true);
				f.SetCell (new Point (2, 2), true, true);

				Console.WriteLine (f.GetCellTransparent (new Point (2, 2)));
				Console.WriteLine (f.GetCellWalkable (new Point (2, 2)));

				f.Calculate (new Point (0, 0), 5, true, FovAlgorithm.Shadow);
				Console.WriteLine (f.IsInView (new Point (2, 0)));
				Console.WriteLine (f.IsInView (new Point (0, 2)));
				Console.WriteLine (f.IsInView (new Point (2, 2)));
			}
		}

		static void TestDijkstra ()
		{
			using (DijkstraPathfinding path = new DijkstraPathfinding (new Size (10, 10), 1, (from, to) =>
					Math.Abs (from.X - to.X) + Math.Abs (from.Y - to.Y)))
			{
				path.Compute (new Point (5, 5));
				path.SetPath (new Point (1, 1));
				Console.WriteLine (path.Length);
				Console.WriteLine (path.IsEmpty);
				Console.WriteLine (path[2]);
				foreach (var point in path.Generate ())
					Console.WriteLine ("\t" + point);
			}
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
