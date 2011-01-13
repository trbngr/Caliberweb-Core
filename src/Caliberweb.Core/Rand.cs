#region (c)2009 Lokad - New BSD license

// Copyright (c) Lokad 2009 
// Company: http://www.lokad.com
// This code is released under the terms of the new BSD licence

#endregion

using System;
using System.Linq;

namespace Caliberweb.Core
{
	/// <summary>
	/// Helper class that allows to implement non-deterministic 
	/// reproducible testing.
	/// </summary>
	/// <remarks>
	/// Keep in mind, that this implementation is not thread-safe.
	/// </remarks>
	public static partial class Rand
	{
		static Func<int, int> nextInt;
		static Func<Func<int, int>> currentActivator;

		/// <summary>
		/// Resets everything to the default, using <see cref="Random"/> generator and random seed. 
		/// </summary>
		public static void ResetToDefault()
		{
			ResetToDefault(new Random().Next());
		}

		/// <summary>
		/// Resets everything to the default, using <see cref="Random"/> generator and the specified
		/// rand seed.
		/// </summary>
		/// <param name="randSeed">The rand seed.</param>
		public static void ResetToDefault(int randSeed)
		{
			currentActivator = () =>
			{
				var r = new Random(randSeed);
				return i => r.Next(i);
			};
			nextInt = currentActivator();
		}

		static Rand()
		{
			ResetToDefault();
		}

		/// <summary>
		/// Resets the random generator, using the provided activator
		/// </summary>
		public static void Reset()
		{
			nextInt = currentActivator();
		}

		/// <summary>
		/// Overrides with the current activator
		/// </summary>
		/// <param name="activator">The activator.</param>
		public static void Reset(Func<Func<int, int>> activator)
		{
			currentActivator = activator;
			nextInt = currentActivator();
		}

		/// <summary>
		/// Generates random value between 0 and <see cref="int.MaxValue"/> (exclusive)
		/// </summary>
		/// <returns>random integer</returns>
		public static int Next()
		{
			return nextInt(int.MaxValue);
		}

		/// <summary>
		/// Generates random value between 0 and <paramref name="upperBound"/> (exclusive)
		/// </summary>
		/// <param name="upperBound">The upper bound.</param>
		/// <returns>random integer</returns>
		public static int Next(int upperBound)
		{
			return nextInt(upperBound);
		}

		/// <summary>
		/// Generates random value between <paramref name="lowerBound"/>
		/// and <paramref name="upperBound"/> (exclusive)
		/// </summary>
		/// <param name="lowerBound">The lower bound.</param>
		/// <param name="upperBound">The upper bound.</param>
		/// <returns>random integer</returns>
		public static int Next(int lowerBound, int upperBound)
		{
			var range = upperBound - lowerBound;
			return nextInt(range) + lowerBound;
		}

		/// <summary> Picks random item from the provided array </summary>
		/// <typeparam name="TItem">The type of the item.</typeparam>
		/// <param name="items">The items.</param>
		/// <returns>random item from the array</returns>
		public static TItem NextItem<TItem>(TItem[] items)
		{
			var index = nextInt(items.Length);
			return items[index];
		}

		/// <summary>
		/// Returns <em>true</em> with the specified probability.
		/// </summary>
		/// <param name="probability">The probability (between 0 and 1).</param>
		/// <returns><em>true</em> with the specified probability</returns>
		public static bool NextBool(double probability)
		{
			return NextDouble() < probability;
		}

		/// <summary>
		/// Returns either <em>true</em> or <em>false</em>
		/// </summary>
		/// <returns>either <em>true</em> or <em>false</em></returns>
		public static bool NextBool()
		{
			return NextBool(0.5D);
		}

		/// <summary> Picks random <see cref="Guid"/>  </summary>
		/// <returns>random value</returns>
		public static Guid NextGuid()
		{
			return new Guid(Range.Array(16, i => (byte) Next(byte.MaxValue + 1)));
		}

		/// <summary>
		/// Creates an array of random <see cref="Guid"/>
		/// </summary>
		/// <param name="count">Number of items in the array.</param>
		/// <returns>random value</returns>
		public static Guid[] NextGuids(int count)
		{
			return Range.Array(count, x => NextGuid());
		}

		/// <summary>
		/// Creates an array of random <see cref="Guid"/> and random length.
		/// </summary>
		/// <param name="lowerBound">The lower bound.</param>
		/// <param name="upperBound">The upper bound.</param>
		/// <returns></returns>
		public static Guid[] NextGuids(int lowerBound, int upperBound)
		{
			int count = Next(lowerBound, upperBound);
			return Range.Array(count, x => NextGuid());
		}

		/// <summary> Returns random double value with lowered precision </summary>
		/// <returns>random double value</returns>
		public static double NextDouble()
		{
			return (double) nextInt(int.MaxValue)/int.MaxValue;
		}

		static readonly DateTime minDate = new DateTime(1700, 1, 1);

		/// <summary>
		/// Returns a random date between 1700-01-01 and 2100-01-01
		/// </summary>
		/// <returns>random value</returns>
		public static DateTime NextDate()
		{
			return minDate
				.AddYears(Next(500))
				.AddDays(NextDouble()*24D*365.25D);
		}

		/// <summary>
		/// Returns a random date between the specified range.
		/// </summary>
		/// <param name="minYear">The min year.</param>
		/// <param name="maxYear">The max year.</param>
		/// <returns>new random date</returns>
		public static DateTime NextDate(int minYear, int maxYear)
		{
			var days = NextDouble()*356*(maxYear - minYear);
			return new DateTime(minYear, 1, 1).AddDays(days);
		}

		static readonly char[] symbols = "!\"#%&'()*,-./:;?@[\\]_{} ".ToCharArray();


		/// <summary>
		/// Generates random string with the length between 
		/// <paramref name="lowerBound"/> and <paramref name="upperBound"/> (exclusive)
		/// </summary>
		/// <param name="lowerBound">The lower bound for the string length.</param>
		/// <param name="upperBound">The upper bound for the string length.</param>
		/// <returns>new random string</returns>
		public static string NextString(int lowerBound, int upperBound)
		{
			//const int surrogateStartsAt = 55296;
			int count = Next(lowerBound, upperBound);
			var array = Range.Array(count, i => Next(5) == 1 ? NextItem(symbols) : (char) Next(48, 122));
			return new string(array);
		}

		/// <summary>
		/// Gets a random subset from the array
		/// </summary>
		/// <typeparam name="TValue">The type of the value.</typeparam>
		/// <param name="items">The items.</param>
		/// <param name="count">The count.</param>
		/// <returns>array that contains <paramref name="count"/> items from the original array</returns>
		/// <exception cref="ArgumentOutOfRangeException">when <paramref name="count"/> 
		/// is bigger than the length of <paramref name="items"/></exception>
		public static TValue[] NextItems<TValue>(TValue[] items, int count)
		{
			if (items == null) throw new ArgumentNullException("items");
			if (count > items.Length)
				throw new ArgumentOutOfRangeException();

			if (count == 0)
				return new TValue[0];

			var indexes = Range.Array(items.Length).ToList();

			var result = new TValue[count];

			for (int i = 0; i < count; i++)
			{
				var next = Next(indexes.Count);
				var index = indexes[next];
				indexes.RemoveAt(next);
				result[i] = items[index];
			}

			return result;
		}
	}
}