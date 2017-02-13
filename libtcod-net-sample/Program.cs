using System;
using System.Collections.Generic;

namespace libtcod.sample
{
	interface ISample
	{
		string Name { get; }
		void Setup (ConsoleWindow window);
		void Render (KeyPress key, Mouse mouse);
	}

	public class TrueColorSample : ISample
	{
		public string Name => "  True colors        ";

		public void Setup (ConsoleWindow window)
		{
		}

		public void Render (KeyPress key, Mouse mouse)
		{

		}
	}

	public class OffscreenConsoleSample : ISample
	{
		public string Name => "  Offscreen console  ";

		public void Setup (ConsoleWindow window)
		{
		}

		public void Render (KeyPress key, Mouse mouse)
		{

		}
	}

	public class LineDrawingSample : ISample
	{
		public string Name => "  Line drawing       ";

		public void Setup (ConsoleWindow window)
		{
		}

		public void Render (KeyPress key, Mouse mouse)
		{

		}
	}

	public class NoiseSample : ISample
	{
		public string Name => "  Noise              ";

		public void Setup (ConsoleWindow window)
		{
		}

		public void Render (KeyPress key, Mouse mouse)
		{

		}
	}

	public class FOVSample : ISample
	{
		public string Name => "  Field of view      ";

		public void Setup (ConsoleWindow window)
		{
		}

		public void Render (KeyPress key, Mouse mouse)
		{

		}
	}

	public class PathFindingSample : ISample
	{
		public string Name => "  Path finding       ";

		public void Setup (ConsoleWindow window)
		{
		}

		public void Render (KeyPress key, Mouse mouse)
		{

		}
	}


	public class BSPSample : ISample
	{
		public string Name => "  Bsp toolkit        ";

		int BSPDepth = 8;
		int MinRoomSize = 4;
		bool RandomRoom = false;
		bool RoomWalls = true;

		BSP bsp;
		char [,] map;
		bool Generate = true;
		bool Refresh = false;
		ConsoleWindow window;

		Color DarkWall = Color.FromRGB (0, 0, 100);
		Color DarkGround = Color.FromRGB (50, 50, 150);

		public void Setup (ConsoleWindow window)
		{
			this.window = window;
		}

		bool TraverseNode (BSP node)
		{
			if (node.IsLeaf)
			{
				// calculate the room size
				int minx = node.X + 1;
				int maxx = node.X + node.W - 1;
				int miny = node.Y + 1;
				int maxy = node.Y + node.H - 1;
				int x, y;
				if (!RoomWalls)
				{
					if (minx > 1)
						minx--;
					if (miny > 1)
						miny--;
				}
				if (maxx == Sample.SampleScreenSize.Width - 1)
					maxx--;
				if (maxy == Sample.SampleScreenSize.Height - 1)
					maxy--;
				if (RandomRoom)
				{
					minx = Random.Default.GetInt (minx, maxx - MinRoomSize + 1);
					miny = Random.Default.GetInt (miny, maxy - MinRoomSize + 1);
					maxx = Random.Default.GetInt (minx + MinRoomSize - 1, maxx);
					maxy = Random.Default.GetInt (miny + MinRoomSize - 1, maxy);
				}
				// resize the node to fit the room
				//	printf("node %dx%d %dx%d => room %dx%d %dx%d\n",node.X,node.Y,node.W,node.H,minx,miny,maxx-minx+1,maxy-miny+1);
				node.X = minx;
				node.Y = miny;
				node.W = maxx - minx + 1;
				node.H = maxy - miny + 1;
				// dig the room
				for (x = minx; x <= maxx; x++)
				{
					for (y = miny; y <= maxy; y++)
					{
						map[x, y] = ' ';
					}
				}
			}
			else {
				// resize the node to fit its sons
				BSP left = node.Left;
				BSP right = node.Right;
				node.X = Math.Min (left.X, right.X);
				node.Y = Math.Min (left.Y, right.Y);
				node.W = Math.Max (left.X + left.W, right.X + right.W) - node.X;
				node.H = Math.Max (left.Y + left.H, right.Y + right.H) - node.Y;
				// create a corridor between the two lower nodes
				if (node.Horizontal)
				{
					// vertical corridor
					if (left.X + left.W - 1 < right.X || right.X + right.W - 1 < left.X)
					{
						// no overlapping zone. we need a Z shaped corridor
						int x1 = Random.Default.GetInt (left.X, left.X + left.W - 1);
						int x2 = Random.Default.GetInt (right.X, right.X + right.W - 1);
						int y = Random.Default.GetInt (left.Y + left.H, right.Y);
						vline_up (map, x1, y - 1);
						hline (map, x1, y, x2);
						vline_down (map, x2, y + 1);
					}
					else {
						// straight vertical corridor
						int minx = Math.Max (left.X, right.X);
						int maxx = Math.Min (left.X + left.W - 1, right.X + right.W - 1);
						int x = Random.Default.GetInt (minx, maxx);
						vline_down (map, x, right.Y);
						vline_up (map, x, right.Y - 1);
					}
				}
				else {
					// horizontal corridor
					if (left.Y + left.H - 1 < right.Y || right.Y + right.H - 1 < left.Y)
					{
						// no overlapping zone. we need a Z shaped corridor
						int y1 = Random.Default.GetInt (left.Y, left.Y + left.H - 1);
						int y2 = Random.Default.GetInt (right.Y, right.Y + right.H - 1);
						int x = Random.Default.GetInt (left.X + left.W, right.X);
						hline_left (map, x - 1, y1);
						vline (map, x, y1, y2);
						hline_right (map, x + 1, y2);
					}
					else {
						// straight horizontal corridor
						int miny = Math.Max (left.Y, right.Y);
						int maxy = Math.Min (left.Y + left.H - 1, right.Y + right.H - 1);
						int y = Random.Default.GetInt (miny, maxy);
						hline_left (map, right.X - 1, y);
						hline_right (map, right.X, y);
					}
				}
			}
			return true;
		}

		void vline (char[,] map, int x, int y1, int y2)
		{
			int y = y1;
			int dy = (y1 > y2 ? -1 : 1);
			map[x,y] = ' ';
			if (y1 == y2)
				return;
			do
			{
				y += dy;
				map[x,y] = ' ';
			}
			while (y != y2);
		}

		// draw a vertical line up until we reach an empty space
		void vline_up (char[,] map, int x, int y)
		{
			while (y >= 0 && map[x,y] != ' ')
			{
				map[x,y] = ' ';
				y--;
			}
		}

		// draw a vertical line down until we reach an empty space
		void vline_down (char[,] map, int x, int y)
		{
			while (y < Sample.SampleScreenSize.Height && map[x,y] != ' ')
			{
				map[x,y] = ' ';
				y++;
			}
		}

		// draw a horizontal line
		void hline (char[,] map, int x1, int y, int x2)
		{
			int x = x1;
			int dx = (x1 > x2 ? -1 : 1);
			map[x,y] = ' ';
			if (x1 == x2)
				return;
			do
			{
				x += dx;
				map[x,y] = ' ';
			}
			while (x != x2);
		}

		// draw a horizontal line left until we reach an empty space
		void hline_left (char[,] map, int x, int y)
		{
			while (x >= 0 && map[x,y] != ' ')
			{
				map[x,y] = ' ';
				x--;
			}
		}

		// draw a horizontal line right until we reach an empty space
		void hline_right (char[,] map, int x, int y)
		{
			while (x < Sample.SampleScreenSize.Width && map[x,y] != ' ')
			{
				map[x,y] = ' ';
				x++;
			}
		}

		public void Render (KeyPress key, Mouse mouse)
		{
			if (Generate || Refresh)
			{
				if (bsp == null)
					bsp = new BSP (Point.Empty, Sample.SampleScreenSize);
				else
					bsp.Resize (Point.Empty, Sample.SampleScreenSize);
				map = new char[Sample.SampleScreenSize.Width, Sample.SampleScreenSize.Height];
				for (int i = 0; i < Sample.SampleScreenSize.Width; ++i)
					for (int j = 0; j < Sample.SampleScreenSize.Height; ++j)
						map[i, j] = '#';

				if (Generate)
				{
					bsp.RemoveSons ();
					bsp.SplitRecursive (BSPDepth, MinRoomSize + (RoomWalls ? 1 : 0), MinRoomSize + (RoomWalls ? 1 : 0), 1.5f, 1.5f);
				}

				bsp.TraverseInvertedLevelOrder (TraverseNode);
				Generate = false;
				Refresh = false;
			}

			window.Clear ();
			window.DefaultForegroundColor = ColorPresets.White;
			string randomRoomString = RandomRoom ? "ON" : "OFF";
			window.PrintLine ($"ENTER : rebuild bsp\nSPACE : rebuild dungeon\n+-: bsp depth {BSPDepth}\n*/: room size {MinRoomSize}\n1 : random room size {randomRoomString}", new Point (1, 1));
			if (RandomRoom)
				window.PrintLine ("2 : room walls " + (RoomWalls ? "ON" : "OFF"), new Point (1, 6));
			for (int y = 0; y < Sample.SampleScreenSize.Height; ++y)
				for (int x = 0; x < Sample.SampleScreenSize.Width; ++x)
					window.SetBackground (new Point (x, y), map[x,y] == '#' ? DarkWall : DarkGround);

			if (key.KeyCode == KeyCode.Enter || key.KeyCode == KeyCode.KPEnter)
			{
				Generate = true;
			}
			else if (key.KeyCode == KeyCode.Text)
			{
				if (key.Text == " ")
				{
					Refresh = true;
				}
				else if (key.Text == "+")
				{
					BSPDepth++;
					Generate = true;
				}
				else if (key.Text == "-" && BSPDepth > 1)
				{
					BSPDepth--;
					Generate = true;
				}
				else if (key.Text == "*")
				{
					MinRoomSize++;
					Generate = true;
				}
				else if (key.Text == "/" && MinRoomSize > 2)
				{
					MinRoomSize--;
					Generate = true;
				}
				else if (key.Text == "1")
				{
					RandomRoom = !RandomRoom;
					if (!RandomRoom)
						RoomWalls = true;
					Refresh = true;
				}
				else if (key.Text == "2")
				{
					RoomWalls = !RoomWalls;
					Refresh = true;
				}
			}
		}
	}

	public class MouseSample : ISample
	{
		public string Name => "  Mouse support      ";

		ConsoleWindow window;
		bool LButton = false;
		bool RButton = false;
		bool MButton = false;
		public void Setup (ConsoleWindow window)
		{
			this.window = window;
			window.DefaultBackgroundColor = ColorPresets.Gray;
			window.DefaultForegroundColor = ColorPresets.LightYellow;
			Mouse.MoveMouse (320, 200);
			Mouse.ShowCursor (true);
			SystemAPI.FPS = 30;
		}

		public void Render (KeyPress key, Mouse mouse)
		{
			window.Clear ();
			if (mouse.LeftButtonPressed)
				LButton = !LButton;
			if (mouse.MiddleButtonPressed)
				MButton = !MButton;
			if (mouse.RightButtonPressed)
				RButton = !RButton;

			string mouseDetails = string.Format ("{0}\nMouse position : {1}x{2} {3}\nMouse cell     : {4}x{5}\nMouse movement : {6}x{7}\nLeft button    : {8} (toggle {9}\nRight button   : {10} (toggle {11})\nMiddle button  : {12} (toggle {13})\nWheel          : {14}\n",
				RootConsoleWindow.Instance.IsActive ? "" : "APPLICATION INACTIVE",
				mouse.PixelLocation.X, mouse.PixelLocation.Y, RootConsoleWindow.Instance.HasMouseFocus ? "" : "OUT OF FOCUS",
				mouse.CellLocation.X, mouse.CellLocation.Y, mouse.PixelVelocity.X, mouse.PixelVelocity.Y,
				mouse.LeftButton ? " ON" : "OFF", LButton ? " ON" : "OFF",
				mouse.RightButton ? " ON" : "OFF", RButton ? " ON" : "OFF",
				mouse.MiddleButton ? " ON" : "OFF", MButton ? " ON" : "OFF",
				mouse.WheelUp ? "UP" : (mouse.WheelDown ? "DOWN" : ""));
			window.PrintLine (mouseDetails, new Point (1, 1));
			window.PrintLine ("1 : Hide cursor\n2 : Show cursor", new Point (1, 10));
			if (key.KeyCode == KeyCode.Text)
			{
				if (key.Text == "1")
					Mouse.ShowCursor (false);
				if (key.Text == "2")
					Mouse.ShowCursor (true);
			}

		}
	}

	public class NameGeneratorSample : ISample
	{
		public string Name => "  Name generator     ";

		ConsoleWindow window;
		int CurrentSetIndex = 0;
		List<string> Sets;
		List<string> GeneratedNames = new List<string> ();
		float delay = 0;

		public NameGeneratorSample ()
		{
			var namegenFiles = System.IO.Directory.GetFiles ("namegen", "*.cfg");
			foreach (var file in namegenFiles)
			{
				NameGenerator.LoadSyllableFile (file);				
			}
			Sets = NameGenerator.GetSet ();
		}

		public void Setup (ConsoleWindow window)
		{
			this.window = window;
			window.DefaultBackgroundColor = ColorPresets.Blue;
			window.DefaultForegroundColor = ColorPresets.White;
			SystemAPI.FPS = 30;
			delay = 0;
		}

		public void Render (KeyPress key, Mouse mouse)
		{
			if (GeneratedNames.Count >= 15)
				GeneratedNames.RemoveRange (0, GeneratedNames.Count - 14);

			window.Clear ();
			if (Sets.Count > 0)
			{
				window.PrintLine ($"{Sets[CurrentSetIndex]}\n\n+ : next generator\n- : prev generator", new Point (1, 1), LineAlignment.Left);
				for (int i = 0; i < GeneratedNames.Count; ++i)
					window.PrintLine (GeneratedNames[i], new Point (Sample.SampleScreenSize.Width - 2, 2 + i), LineAlignment.Right);

			}
			delay += SystemAPI.LastFrameLength;
			if (delay > .5f)
			{
				delay -= .5f;
				GeneratedNames.Add (NameGenerator.Generate (Sets[CurrentSetIndex]));
			}
			if (key.KeyCode == KeyCode.Text)
			{
				if (key.Text == "+")
				{
					CurrentSetIndex++;
					if (CurrentSetIndex == Sets.Count)
						CurrentSetIndex = 0;
					GeneratedNames.Add ("======");
				}
				if (key.Text == "-")
				{
					CurrentSetIndex--;
					if (CurrentSetIndex < 0)
						CurrentSetIndex = Sets.Count - 1;
					GeneratedNames.Add ("======");
				}
			}
		}
	}

	class Sample
	{
		public static readonly Size SampleScreenSize = new Size (46, 20);
		public static readonly Point SampleScreenPosition = new Point (20, 10);

		ConsoleWindow sampleConsole;

		List<ISample> Samples = new List<ISample> ()
		{
			new TrueColorSample (), new OffscreenConsoleSample (), new LineDrawingSample (), new NoiseSample (),
			new FOVSample (), new PathFindingSample (), new BSPSample (), new MouseSample (), new NameGeneratorSample ()
		};

		public void Run ()
		{
			CustomFontRequest font = new CustomFontRequest ("fonts/consolas10x10_gs_tc.png", CustomFontRequestFontTypes.Grayscale | CustomFontRequestFontTypes.LayoutTCOD, 0, 0);
			RootConsoleWindow.Setup (80, 50, "C# sample", false, ConsoleRender.SDL, font);

			var console = RootConsoleWindow.Instance;
			sampleConsole = RootConsoleWindow.CreateOffscreenConsole (SampleScreenSize);

			bool creditsHaveEnded = false;
			bool first = true;
			int currentSampleIndex = 0;
			do
			{
				console.Clear ();
				if (!creditsHaveEnded)
					creditsHaveEnded = console.ConsoleCreditsRender (new Point (60, 43), false);

				for (int i = 0; i < Samples.Count; ++i)
				{
					if (i == currentSampleIndex)
					{
						console.DefaultForegroundColor = ColorPresets.White;
						console.DefaultBackgroundColor = ColorPresets.LightBlue;
					}
					else
					{
						console.DefaultForegroundColor = ColorPresets.Gray;
						console.DefaultBackgroundColor = ColorPresets.Black;
					}
					console.PrintLine (Samples[i].Name, new Point (2, 46 - Samples.Count + i), Background.Set, LineAlignment.Left);
				}
				console.DefaultForegroundColor = ColorPresets.Gray;
				console.DefaultBackgroundColor = ColorPresets.Black;

				console.PrintLine ($"last frame : {(int)(SystemAPI.LastFrameLength * 1000)} ms ({SystemAPI.FPS} fps)", new Point (79, 46), LineAlignment.Right);
				string msString = string.Format ("{0:########}", SystemAPI.ElapsedMilliseconds);
				string sString = string.Format ("{0:###0.00}", SystemAPI.ElapsedSeconds);
				console.PrintLine ($"elapsed : {msString}ms {sString}s", new Point (79, 47), LineAlignment.Right);

				console.PrintLine ($"{(char)SpecialCharacter.Arrow_N}{(char)SpecialCharacter.Arrow_S} : select a sample", new Point (2, 47), LineAlignment.Left);

				string consoleFullScreenTextPrompt = console.IsFullscreen ? "windowed mode  " : "fullscreen mode";
				console.PrintLine ($"ALT-ENTER : switch to {consoleFullScreenTextPrompt}", new Point (2, 48), LineAlignment.Left);

				KeyPress key = new KeyPress ();
				Mouse mouse = new Mouse ();
				SystemAPI.CheckForEvent (EventType.KeyPress | EventType.Mouse, ref key, ref mouse);

				if (first)
				{
					first = false;
					Samples[currentSampleIndex].Setup (sampleConsole);
				}
				Samples[currentSampleIndex].Render (key, mouse);

				sampleConsole.Blit (new Rectangle (new Point (0, 0), SampleScreenSize), console, SampleScreenPosition.X, SampleScreenPosition.Y);

				console.Flush ();
				if (key.KeyCode == KeyCode.Down)
				{
					currentSampleIndex = (currentSampleIndex + 1) % Samples.Count;
					first = true;
				}
				else if (key.KeyCode == KeyCode.Up)
				{
					currentSampleIndex -= 1;
					if (currentSampleIndex < 0)
						currentSampleIndex = Samples.Count - 1;
					first = true;
				}
				else if (key.KeyCode == KeyCode.Enter && key.Alt)
				{
					console.IsFullscreen = !console.IsFullscreen;
				}

			}
			while (!RootConsoleWindow.Instance.IsWindowClosed);
		}

		static void Main (string[] args)
		{
			(new Sample ()).Run ();
		}
	}
}

