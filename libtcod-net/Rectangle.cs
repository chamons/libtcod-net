//
// System.Drawing.Rectangle.cs
//
// Author:
//   Mike Kestner (mkestner@speakeasy.net)
//
// Copyright (C) 2001 Mike Kestner
// Copyright (C) 2004 Novell, Inc.  http://www.novell.com 
//

//
// Copyright (C) 2004 Novell, Inc (http://www.novell.com)
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

using System;
using System.Runtime.InteropServices;
using System.ComponentModel;

// Stolen with love and modified from https://github.com/mono/mono/tree/master/mcs/class/System.Drawing/System.Drawing
namespace libtcod
{
	[Serializable]
	[ComVisible (true)]
	public struct Rectangle
	{
		private int x, y, width, height;
		public static readonly Rectangle Empty;

		public static Rectangle FromLTRB (int left, int top,
						  int right, int bottom)
		{
			return new Rectangle (left, top, right - left,
						  bottom - top);
		}

		public static Rectangle Inflate (Rectangle rect, int x, int y)
		{
			Rectangle r = new Rectangle (rect.Location, rect.Size);
			r.Inflate (x, y);
			return r;
		}

		public void Inflate (int width, int height)
		{
			Inflate (new Size (width, height));
		}

		public void Inflate (Size size)
		{
			x -= size.Width;
			y -= size.Height;
			Width += size.Width * 2;
			Height += size.Height * 2;
		}

		public static Rectangle Intersect (Rectangle a, Rectangle b)
		{
			// MS.NET returns a non-empty rectangle if the two rectangles
			// touch each other
			if (!a.IntersectsWithInclusive (b))
				return Empty;

			return Rectangle.FromLTRB (
				Math.Max (a.Left, b.Left),
				Math.Max (a.Top, b.Top),
				Math.Min (a.Right, b.Right),
				Math.Min (a.Bottom, b.Bottom));
		}

		public void Intersect (Rectangle rect)
		{
			this = Rectangle.Intersect (this, rect);
		}

		public static Rectangle Union (Rectangle a, Rectangle b)
		{
			return FromLTRB (Math.Min (a.Left, b.Left),
					 Math.Min (a.Top, b.Top),
					 Math.Max (a.Right, b.Right),
					 Math.Max (a.Bottom, b.Bottom));
		}


		public static bool operator == (Rectangle left, Rectangle right)
		{
			return ((left.Location == right.Location) &&
				(left.Size == right.Size));
		}

		public static bool operator != (Rectangle left, Rectangle right)
		{
			return ((left.Location != right.Location) ||
				(left.Size != right.Size));
		}

		public Rectangle (Point location, Size size)
		{
			x = location.X;
			y = location.Y;
			width = size.Width;
			height = size.Height;
		}

		public Rectangle (int x, int y, int width, int height)
		{
			this.x = x;
			this.y = y;
			this.width = width;
			this.height = height;
		}

		[Browsable (false)]
		public int Bottom
		{
			get
			{
				return y + height;
			}
		}

		public int Height
		{
			get
			{
				return height;
			}
			set
			{
				height = value;
			}
		}

		[Browsable (false)]
		public bool IsEmpty
		{
			get
			{
				return ((x == 0) && (y == 0) && (width == 0) && (height == 0));
			}
		}

		[Browsable (false)]
		public int Left
		{
			get
			{
				return X;
			}
		}

		[Browsable (false)]
		public Point Location
		{
			get
			{
				return new Point (x, y);
			}
			set
			{
				x = value.X;
				y = value.Y;
			}
		}

		[Browsable (false)]
		public int Right
		{
			get
			{
				return X + Width;
			}
		}

		[Browsable (false)]
		public Size Size
		{
			get
			{
				return new Size (Width, Height);
			}
			set
			{
				Width = value.Width;
				Height = value.Height;
			}
		}


		[Browsable (false)]
		public int Top
		{
			get
			{
				return y;
			}
		}

		public int Width
		{
			get
			{
				return width;
			}
			set
			{
				width = value;
			}
		}

		public int X
		{
			get
			{
				return x;
			}
			set
			{
				x = value;
			}
		}

		public int Y
		{
			get
			{
				return y;
			}
			set
			{
				y = value;
			}
		}

		public bool Contains (int x, int y)
		{
			return ((x >= Left) && (x < Right) &&
				(y >= Top) && (y < Bottom));
		}

		public bool Contains (Point pt)
		{
			return Contains (pt.X, pt.Y);
		}

		public bool Contains (Rectangle rect)
		{
			return (rect == Intersect (this, rect));
		}

		public override bool Equals (object obj)
		{
			if (!(obj is Rectangle))
				return false;

			return (this == (Rectangle)obj);
		}

		public override int GetHashCode ()
		{
			return (height + width) ^ x + y;
		}

		public bool IntersectsWith (Rectangle rect)
		{
			return !((Left >= rect.Right) || (Right <= rect.Left) ||
				(Top >= rect.Bottom) || (Bottom <= rect.Top));
		}

		private bool IntersectsWithInclusive (Rectangle r)
		{
			return !((Left > r.Right) || (Right < r.Left) ||
				(Top > r.Bottom) || (Bottom < r.Top));
		}

		public void Offset (int x, int y)
		{
			this.x += x;
			this.y += y;
		}

		public void Offset (Point pos)
		{
			x += pos.X;
			y += pos.Y;
		}

		public override string ToString ()
		{
			return String.Format ("{{X={0}, Y={1}, Width={2}, Height={3}}}",
						 x, y, width, height);
		}

	}
}
