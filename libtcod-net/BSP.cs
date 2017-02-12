using System;
using System.Runtime.InteropServices;

namespace libtcod
{
	public delegate bool BSPTraversalDelegate (BSP bsp);

	// Binary Space Partition
	public unsafe class BSP : IDisposable
	{
		TCODBSPData * Handle;
		BSPTraversalDelegate Delegate;
		BSPTraversalTrampolineDelegate Trampoline;
		bool IsRoot;
	
		[UnmanagedFunctionPointer (CallingConvention.Cdecl)]
		delegate bool BSPTraversalTrampolineDelegate (IntPtr bsp, IntPtr nullPtr);
	
		[DllImport (Constants.LibraryName)]
		extern static IntPtr TCOD_bsp_new ();

		[DllImport (Constants.LibraryName)]
		extern static IntPtr TCOD_bsp_new_with_size (int x, int y, int w, int h);

		public BSP ()
		{
			Handle = (TCODBSPData*)TCOD_bsp_new ();
			IsRoot = true;
			Delegate = null;
			Trampoline = new BSPTraversalTrampolineDelegate (TraversalTrampoline);
		}

		public BSP (Point p, Size s)
		{
			Handle = (TCODBSPData*)TCOD_bsp_new_with_size (p.X, p.Y, s.Width, s.Height);
			IsRoot = true;
			Delegate = null;
			Trampoline = new BSPTraversalTrampolineDelegate (TraversalTrampoline);
		}

		BSP (TCODBSPData* data)
		{
			Handle = data;
			IsRoot = false;
			Delegate = null;
			Trampoline = new BSPTraversalTrampolineDelegate (TraversalTrampoline);
		}

		bool TraversalTrampoline (IntPtr bsp, IntPtr nullPtr)
		{
			if (Delegate != null)
			{
				TCODBSPData* p = (TCODBSPData*)bsp;
				if (p != null)
					return Delegate (new BSP (p));
				else
					return Delegate (null);
			}
			return false;
		}

		[DllImport (Constants.LibraryName)]
		extern static void TCOD_bsp_delete (IntPtr node);

		public void Dispose ()
		{
			// Only TCOD_bsp_new / TCOD_bsp_new_with_size cleans up. And they will kill their children
			// Warning - If your children lifespan outlive root, you could crash.
			if (IsRoot && Handle != null)
				TCOD_bsp_delete ((IntPtr)Handle);
		}

		[DllImport (Constants.LibraryName)]
		extern static IntPtr TCOD_bsp_left (IntPtr node);

		public BSP Left
		{
			get
			{
				TCODBSPData* p = (TCODBSPData*)TCOD_bsp_left ((IntPtr)Handle);
				return p != null ? new BSP (p) : null;
			}
		}

		[DllImport (Constants.LibraryName)]
		extern static IntPtr TCOD_bsp_right (IntPtr node);

		public BSP Right
		{
			get
			{
				TCODBSPData* p = (TCODBSPData*)TCOD_bsp_right ((IntPtr)Handle);
				return p != null ? new BSP (p) : null;
			}
		}

		[DllImport (Constants.LibraryName)]
		extern static IntPtr TCOD_bsp_find_node (IntPtr node, int x, int y);

		public BSP Find (Point position)
		{
			TCODBSPData* p = (TCODBSPData *)TCOD_bsp_find_node ((IntPtr)Handle, position.X, position.Y);
			return p != null ? new BSP (p) : null;

		}

		[DllImport (Constants.LibraryName)]
		extern static IntPtr TCOD_bsp_father (IntPtr node);

		public BSP Father
		{
			get
			{
				TCODBSPData* p = (TCODBSPData*)TCOD_bsp_father ((IntPtr)Handle);
				return p != null ? new BSP (p) : null;
			}
		}

		[DllImport (Constants.LibraryName)]
		extern static void TCOD_bsp_split_once (IntPtr node, bool horizontal, int position);

		public void SplitOnce (bool horizontal, int position)
		{
			TCOD_bsp_split_once ((IntPtr)Handle, horizontal, position);
		}

		[DllImport (Constants.LibraryName)]
		[return: MarshalAs (UnmanagedType.I1)]
		extern static bool TCOD_bsp_is_leaf (IntPtr node);

		public bool IsLeaf
		{
			get
			{
				return TCOD_bsp_is_leaf ((IntPtr)Handle);
			}
		}

		[DllImport (Constants.LibraryName)]
		extern static void TCOD_bsp_resize (IntPtr node, int x, int y, int w, int h);

		public void Resize (Point position, Size size)
		{
			TCOD_bsp_resize ((IntPtr)Handle, position.X, position.Y, size.Width, size.Height);
		}

		[DllImport (Constants.LibraryName)]
		extern static void TCOD_bsp_split_recursive (IntPtr node, IntPtr randomizer, int nb, int minHSize, int minVSize, float maxHRatio, float maxVRatio);

		public void SplitRecursive (Random randomizer, int nb, int minHSize, int minVSize, float maxHRatio, float maxVRatio)
		{
				TCOD_bsp_split_recursive ((IntPtr)Handle, randomizer == null ? IntPtr.Zero : randomizer.Handle, nb, minHSize, minVSize, maxHRatio, maxVRatio);
		}

		[DllImport (Constants.LibraryName)]
		[return: MarshalAs (UnmanagedType.I1)]
		extern static bool TCOD_bsp_contains (IntPtr node, int x, int y);

		public bool Contains (Point position)
		{
			return TCOD_bsp_contains ((IntPtr)Handle, position.X, position.Y);
		}

		[DllImport (Constants.LibraryName)]
		[return: MarshalAs (UnmanagedType.I1)]
		extern static bool TCOD_bsp_traverse_pre_order (IntPtr node, BSPTraversalTrampolineDelegate listener, IntPtr userData);

		public bool TraversePreOrder (BSPTraversalDelegate listner)
		{
			Delegate = listner;
			return TCOD_bsp_traverse_pre_order ((IntPtr)Handle, Trampoline, IntPtr.Zero);
		}

		[DllImport (Constants.LibraryName)]
		[return: MarshalAs (UnmanagedType.I1)]
		extern static bool TCOD_bsp_traverse_in_order (IntPtr node, BSPTraversalTrampolineDelegate listener, IntPtr userData);

		public bool TraverseInOrder (BSPTraversalDelegate listner)
		{
			Delegate = listner;
			return TCOD_bsp_traverse_in_order ((IntPtr)Handle, Trampoline, IntPtr.Zero);
		}

		[DllImport (Constants.LibraryName)]
		[return: MarshalAs (UnmanagedType.I1)]
		extern static bool TCOD_bsp_traverse_post_order (IntPtr node, BSPTraversalTrampolineDelegate listener, IntPtr userData);

		public bool TraversePostOrder (BSPTraversalDelegate listner)
		{
			Delegate = listner;
			return TCOD_bsp_traverse_in_order ((IntPtr)Handle, Trampoline, IntPtr.Zero);
		}

		[DllImport (Constants.LibraryName)]
		[return: MarshalAs (UnmanagedType.I1)]
		extern static bool TCOD_bsp_traverse_level_order (IntPtr node, BSPTraversalTrampolineDelegate listener, IntPtr userData);

		public bool TraverseLevelOrder (BSPTraversalDelegate listner)
		{
			Delegate = listner;
			return TCOD_bsp_traverse_level_order ((IntPtr)Handle, Trampoline, IntPtr.Zero);
		}

		[DllImport (Constants.LibraryName)]
		[return: MarshalAs (UnmanagedType.I1)]
		extern static bool TCOD_bsp_traverse_inverted_level_order (IntPtr node, BSPTraversalTrampolineDelegate listener, IntPtr userData);

		public bool TraverseInvertedOrder (BSPTraversalDelegate listner)
		{
			Delegate = listner;
			return TCOD_bsp_traverse_inverted_level_order ((IntPtr)Handle, Trampoline, IntPtr.Zero);
		}

		public int X
		{
			get { return Handle->X; }
			set { Handle->X = value; }
		}

		public int Y
		{
			get { return Handle->Y; }
			set { Handle->Y = value; }
		}

		public Point Position
		{
			get { return new Point (Handle->X, Handle->Y); }
			set { Handle->X = value.X; Handle->Y = value.Y; }
		}

		public int W
		{
			get { return Handle->W; }
			set { Handle->W = value; }
		}

		public int H
		{
			get { return Handle->H; }
			set { Handle->H = value; }
		}

		public Size Size
		{
			get { return new Size (Handle->W, Handle->H); }
			set { Handle->W = value.Width; Handle->H = value.Height; }
		}

		public int SplittingPosition => Handle->Position;
		public bool Horizontal => Handle->Horizontal;
		public byte Level => Handle->Level;
		
		[StructLayout (LayoutKind.Sequential)]
		struct TCODBSPData
		{
			public IntPtr Pointer1;
			public IntPtr Pointer2;
			public IntPtr Pointer3;
			public int X;
			public int Y;
			public int W;
			public int H;
			public int Position;
			public byte Level;
			public bool Horizontal;
		}
	}
}
