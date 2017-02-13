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

		public void Setup (ConsoleWindow window)
		{
		}

		public void Render (KeyPress key, Mouse mouse)
		{

		}
	}

	public class MouseSample : ISample
	{
		public string Name => "  Mouse support      ";

		public void Setup (ConsoleWindow window)
		{
		}

		public void Render (KeyPress key, Mouse mouse)
		{

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

				KeyPress key = Keyboard.CheckForKeypress (KeyPressType.Pressed);
				Mouse mouse = Mouse.GetStatus ();

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

