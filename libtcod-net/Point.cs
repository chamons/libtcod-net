//
// System.Drawing.Point.cs
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
using System.Globalization;
using System.Runtime.InteropServices;
using System.ComponentModel;

// Stolen with love and modified from https://github.com/mono/mono/tree/master/mcs/class/System.Drawing/System.Drawing
namespace libtcod
{
	[Serializable]
	[ComVisible (true)]
	public struct Point
	{
		private int x, y;

		public static readonly Point Empty;

		public static Point operator + (Point pt, Size sz)
		{
			return new Point (pt.X + sz.Width, pt.Y + sz.Height);
		}

		public static bool operator == (Point left, Point right)
		{
			return ((left.X == right.X) && (left.Y == right.Y));
		}

		public static bool operator != (Point left, Point right)
		{
			return ((left.X != right.X) || (left.Y != right.Y));
		}

		public static Point operator - (Point pt, Size sz)
		{
			return new Point (pt.X - sz.Width, pt.Y - sz.Height);
		}

		public static explicit operator Size (Point p)
		{
			return new Size (p.X, p.Y);
		}

		public Point (Size sz)
		{
			x = sz.Width;
			y = sz.Height;
		}

		public Point (int x, int y)
		{
			this.x = x;
			this.y = y;
		}

		[Browsable (false)]
		public bool IsEmpty
		{
			get
			{
				return ((x == 0) && (y == 0));
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

		public override bool Equals (object obj)
		{
			if (!(obj is Point))
				return false;

			return (this == (Point)obj);
		}

		public override int GetHashCode ()
		{
			return x ^ y;
		}

		public void Offset (int dx, int dy)
		{
			x += dx;
			y += dy;
		}

		public override string ToString ()
		{
			return string.Format ("{{X={0},Y={1}}}", x.ToString (CultureInfo.InvariantCulture),
				y.ToString (CultureInfo.InvariantCulture));
		}
		public static Point Add (Point pt, Size sz)
		{
			return new Point (pt.X + sz.Width, pt.Y + sz.Height);
		}

		public void Offset (Point p)
		{
			Offset (p.X, p.Y);
		}

		public static Point Subtract (Point pt, Size sz)
		{
			return new Point (pt.X - sz.Width, pt.Y - sz.Height);
		}
	}
}
