using System;
namespace libtcod
{
	internal static class Constants
	{
#if MAC
		internal const string LibraryName = "__Internal";
#else
		internal const string LibraryName = "libtcod";
#endif
	}
}
