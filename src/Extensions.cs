using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace Alluseri.EvertalePonoserV2;

public static class Extensions {
	public static float GetSeconds(this Stopwatch Stopwatch) => Stopwatch.ElapsedMilliseconds / 1000F;

	public static byte[] ReadSegment(this Stream Self, int Length) {
		byte[] Bytes = new byte[Length];
		byte[] O = new byte[Self.Read(Bytes)];
		Array.Copy(Bytes, O, O.Length);
		return O;
	}

	public static string Gunzip(this byte[] Data) {
		if (Data[0] == 0x1F && Data[1] == 0x8B) {
			using MemoryStream From = new(Data);
			using MemoryStream To = new();
			GZipStream Stream = new(From, CompressionMode.Decompress);
			Stream.CopyTo(To);
			return Encoding.UTF8.GetString(To.ToArray());
		} else
			return Encoding.UTF8.GetString(Data);
	}

	public static T Random<T>(this T[] Self) => Self[System.Random.Shared.Next(Self.Length)];

	public static T By<T>(this T[] Self, int Ord) => Self[Ord % Self.Length];

	public static long StopTime(this Stopwatch Self) {
		Self.Stop();
		return Self.ElapsedMilliseconds;
	}

	/**
		Performs best with small arrays, worst with big arrays. Does not mutate the input array.
	*/
	public static byte[] Merge(this byte[] First, byte[] Second) {
		using (MemoryStream Mem = new(First.Length + Second.Length)) {
			Mem.Write(First, 0, First.Length);
			Mem.Write(Second, 0, Second.Length);
			return Mem.ToArray();
		}
	}
	/**
		Performs best with small arrays, worst with big arrays. Does not mutate the input array.
	*/
	public static byte[] Merge(this byte[] First, byte[] Second, int SecondLength) {
		using (MemoryStream Mem = new(First.Length + SecondLength)) {
			Mem.Write(First, 0, First.Length);
			Mem.Write(Second, 0, SecondLength);
			return Mem.ToArray();
		}
	}
}
