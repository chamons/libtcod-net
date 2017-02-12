using System;
using System.Runtime.InteropServices;

namespace libtcod
{
	public enum RandomTypes
	{
		MersenneTwister = 0,
		ComplementaryMultiplyWithCarry = 1
	}

	public enum RandomDistribution
	{
		Linear,
		Gaussian,
		GaussianRange,
		GaussianInverse,
		GaussianRangeInverse
	}

	public class Random : IDisposable
	{
		static IntPtr DefaultHandle;
		public static Random Default;

		[DllImport (Constants.LibraryName)]
		private extern static IntPtr TCOD_random_get_instance ();

		static Random ()
		{
			DefaultHandle = TCOD_random_get_instance ();
			Default = new Random (DefaultHandle);
		}

		public IntPtr Handle { get; private set; }

		private Random (IntPtr handle)
		{
			Handle = handle;
		}

		[DllImport (Constants.LibraryName)]
		private extern static IntPtr TCOD_random_new (RandomTypes type);

		[DllImport (Constants.LibraryName)]
		private extern static IntPtr TCOD_random_new_from_seed (RandomTypes type, uint seed);

		public Random (RandomTypes type)
		{
			Handle = TCOD_random_new (type);
		}

		public Random (RandomTypes type, uint seed)
		{
			Handle = TCOD_random_new_from_seed (type, seed);
		}

		[DllImport (Constants.LibraryName)]
		private extern static void TCOD_random_delete (IntPtr mersenne);

		public void Dispose ()
		{
			if (Handle != DefaultHandle)
				TCOD_random_delete (Handle);
		}

		[DllImport (Constants.LibraryName)]
		private extern static void TCOD_random_set_distribution (IntPtr mersenne, RandomDistribution distribution);

		public void SetRandomDistribution (RandomDistribution distribution)
		{
			TCOD_random_set_distribution (Handle, distribution);
		}

		[DllImport (Constants.LibraryName)]
		private extern static int TCOD_random_get_int (IntPtr mersenne, int min, int max);

		public int GetInt (int min, int max)
		{
			return TCOD_random_get_int (Handle, min, max);
		}

		[DllImport (Constants.LibraryName)]
		private extern static int TCOD_random_get_int_mean (IntPtr mersenne, int min, int max, int mean);

		public int GetInt (int min, int max, int mean)
		{
			return TCOD_random_get_int_mean (Handle, min, max, mean);
		}

		[DllImport (Constants.LibraryName)]
		private extern static float TCOD_random_get_float (IntPtr mersenne, float min, float max);

		public float GetFloat (float min, float max)
		{
			return TCOD_random_get_float (Handle, min, max);
		}

		[DllImport (Constants.LibraryName)]
		private extern static float TCOD_random_get_float_mean (IntPtr mersenne, float min, float max, float mean);

		public float GetFloat (float min, float max, float mean)
		{
			return TCOD_random_get_float_mean (Handle, min, max, mean);
		}

		[DllImport (Constants.LibraryName)]
		private extern static double TCOD_random_get_double (IntPtr mersenne, double min, double max);

		public double GetDouble (double min, double max)
		{
			return TCOD_random_get_double (Handle, min, max);
		}

		[DllImport (Constants.LibraryName)]
		private extern static double TCOD_random_get_double_mean (IntPtr mersenne, double min, double max, double mean);

		public double GetDouble (double min, double max, double mean)
		{
			return TCOD_random_get_double_mean (Handle, min, max, mean);
		}
	}
}