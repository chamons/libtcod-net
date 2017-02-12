using System;
using libtcod;

namespace libtcod.sample
{
	class Sample
	{
		readonly Size SampleScreenSize = new Size (46, 20);
		readonly Point SampleScreenPosition = new Point (20, 10);

		ConsoleWindow sampleConsole;

		public void Run ()
		{
			CustomFontRequest font = new CustomFontRequest ("fonts/consolas10x10_gs_tc.png", CustomFontRequestFontTypes.Grayscale | CustomFontRequestFontTypes.LayoutTCOD, 0, 0);
			RootConsoleWindow.Setup (80, 50, "C# sample", false, ConsoleRender.SDL, font);

			sampleConsole = RootConsoleWindow.CreateOffscreenConsole (SampleScreenSize);

			do
			{
				// To spin GUI
				Keyboard.CheckForKeypress (KeyPressType.Pressed);
			}
			while (!RootConsoleWindow.Instance.IsWindowClosed);
		}

		static void Main (string[] args)
		{
			(new Sample ()).Run ();
		}
	}
}

