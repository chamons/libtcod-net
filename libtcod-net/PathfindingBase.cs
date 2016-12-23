using System;
using System.Runtime.InteropServices;

namespace libtcod
{
	public delegate float PathCostDelegate (Point fromPosition, Point toPosition);

	public abstract class PathfindingBase : IDisposable
	{
		[UnmanagedFunctionPointer (CallingConvention.Cdecl)]
		protected delegate float PathCostCallbackInternal (int xFrom, int yFrom, int xTo, int yTo, IntPtr nullPtr);

		public IntPtr Handle;
		protected PathCostCallbackInternal Trampoline;
		private PathCostDelegate Callback;

		protected void SetupCallback (PathCostDelegate callback)
		{
			Callback = callback;
			Trampoline = new PathCostCallbackInternal (PathCallbackTrampoline);
		}

		protected float PathCallbackTrampoline (int xFrom, int yFrom, int xTo, int yTo, IntPtr nullPtr)
		{
			return Callback (new Point (xFrom, yFrom), new Point (xTo, yTo));
		}
			
		public abstract void Dispose ();
	}
}
