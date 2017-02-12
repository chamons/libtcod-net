using System;
using System.Runtime.InteropServices;

namespace libtcod
{
	public enum NoiseType
	{
		Default = 0,
		Perlin = 1,
		Simplex = 2,
		Wavelet = 4
	}

	public class Noise : IDisposable
	{
		public const float NoiseDefaultHurst = 0.5f;
		public const float NoiseDefaultLacunarity = 2.0f;

		IntPtr Handle;
		int Dimensions;

		[DllImport (Constants.LibraryName)]
		private extern static IntPtr TCOD_noise_new (int dimensions, float hurst, float lacunarity, IntPtr random);

		public Noise (int dimensions, double hurst = NoiseDefaultHurst, double lacunarity = NoiseDefaultLacunarity, Random random = null)
		{
			Dimensions = dimensions;
			Handle = TCOD_noise_new (dimensions, (float)hurst, (float)lacunarity, random != null ? random.Handle : IntPtr.Zero);
		}

		[DllImport (Constants.LibraryName)]
		private extern static void TCOD_noise_set_type (IntPtr noise, int type); // HACK - Is this always int? 

		public void SetType (NoiseType type)
		{
			TCOD_noise_set_type (Handle, (int)type);
		}

		[DllImport (Constants.LibraryName)]
		private extern static float TCOD_noise_get (IntPtr noise, float [] coords);

		public float GetNoise (float[] coords)
		{
			return TCOD_noise_get (Handle, coords);
		}

		[DllImport (Constants.LibraryName)]
		private extern static float TCOD_noise_get_fbm (IntPtr noise, float[] coords, float octaves);

		public float GetFBMNoise (float[] coords, float octaves)
		{
			return TCOD_noise_get_fbm (Handle, coords, octaves);
		}

		[DllImport (Constants.LibraryName)]
		private extern static float TCOD_noise_get_turbulence (IntPtr noise, float[] coords, float octaves);

		public float GetTurbulence (float[] coords, float octaves)
		{
			return TCOD_noise_get_turbulence (Handle, coords, octaves);
		}

		[DllImport (Constants.LibraryName)]
		private extern static void TCOD_noise_delete (IntPtr noise);

		public void Dispose ()
		{
			TCOD_noise_delete (Handle);
		}
	}
}