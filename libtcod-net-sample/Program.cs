using System;
using System.Collections.Generic;
using libtcod;
using SampleEntry = System.Collections.Generic.KeyValuePair<string, System.Action<libtcod.KeyPress, libtcod.Mouse>>;


namespace libtcod.sample
{
	class Sample
	{
		readonly Size SampleScreenSize = new Size (46, 20);
		readonly Point SampleScreenPosition = new Point (20, 10);

		ConsoleWindow sampleConsole;

		List<SampleEntry> Samples = new List<SampleEntry> ()
		{
			new SampleEntry ("  True colors        ", null),
			new SampleEntry ("  Offscreen console  ", null),
			new SampleEntry ("  Line drawing       ", null),
			new SampleEntry ("  Noise              ", null),
			new SampleEntry ("  Field of view      ", null),
			new SampleEntry ("  Path finding       ", null),
			new SampleEntry ("  Bsp toolkit        ", null),
			new SampleEntry ("  Mouse support      ", null),
			new SampleEntry ("  Name generator     ", null)
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
					console.PrintLine (Samples[i].Key, new Point (2, 46 - Samples.Count + i), Background.Set, LineAlignment.Left);
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

				//Samples[currentSampleIndex].Value (key, mouse);
				first = false;

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
				else if (key.KeyCode == KeyCode.Enter)
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

