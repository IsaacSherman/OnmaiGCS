using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Security.Cryptography.Pkcs;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Interop;
using System.Windows.Markup;

namespace StatRoller
{
	public class DieString
	{
		public string String;
		private static Regex _isDieString = new Regex("^(\\d*)d(\\d*)(([+-]\\d+)?)$");
		private static Regex _containsDieString = new Regex("\\d*d\\d*([+-]\\d+)?");
		private static Random rng = new Random(DateTime.UtcNow.Millisecond);

		public int Difficulty => (int) (3.5f * NumberOfDice + Modifier);
		public DieString Add(int mod)
		{
			DieString ret = new DieString(String);
			mod += Modifier;
			while (mod > 3)
			{
				ret.NumberOfDice += 1;
				mod -= 3;
			}

			ret.Modifier = mod;
			ret.String = $"{ret.NumberOfDice}d{mod:+0;-#}";
			return ret;
		}

		public int NumberOfDice { get; protected set; }
		public int DieSides { get; protected set; }
		public int Modifier { get; protected set; }
		public override string ToString()
		{
			return $"{NumberOfDice}d{Modifier:+0;-#}";
		}

		public DieString(string src)
		{
			String = src; Match match = _isDieString.Match(src);

			if (match.Success)
			{
				int.TryParse(match.Groups[1].Value, out int numResult);
				if (numResult == 0)
				{
					numResult = 1;
				}
				NumberOfDice = numResult;
				int.TryParse(match.Groups[2].Value, out int sidesResult);
				if (sidesResult == 0) sidesResult = 6;
				DieSides = sidesResult;

				bool hasModifier = int.TryParse(match.Groups[3].Value, out int modResult);
				Modifier = modResult;
			}
			else
			{
				if (src == "") src = "Empty String";
				throw new ArgumentException($"Invalid dice string given: {src}");
			}

		}

		public int Roll()
		{
			int accumulator = 0;
			for (int i = 0; i < NumberOfDice; ++i)
			{
				accumulator += rng.Next(1, DieSides);
			}
			accumulator += Modifier;
			return accumulator;
		}
	}

	public class DieStringTester : ITester
	{
		private static string[] strs = new[]
		{
			"3d6",
			"d3+1",
			"go d12-2",
			"12d3-3",
			"13d3+1",
			"3d",
			"12d+2",
			"12dildoes!",
			"I rolled 2d12 to kill beasts"
		};

		private static List<Tuple<int, int, int>> values = new List<Tuple<int, int, int>>
		{
			new Tuple<int, int, int>(3, 6, 0),
			new Tuple<int, int, int>(1, 3, 1),
			new Tuple<int, int, int>(0, 0, 0),
			new Tuple<int, int, int>(12, 3, -3),
			new Tuple<int, int, int>(13, 3, 1),
			new Tuple<int, int, int>(3, 6, 0),
			new Tuple<int, int, int>(12, 6, 2),
			new Tuple<int, int, int>(0, 0, 0),
			new Tuple<int, int, int>(0, 0, 0),
		};

		private static string PrintStats(DieString die)
		{
			return
				$"Number of dice = {die.NumberOfDice}, sides per die = {die.DieSides}, modifier = {die.Modifier:+0;-#}";
		}
		private static string PrintStats(Tuple<int, int, int> die)
		{
			return
				$"Number of dice = {die.Item1}, sides per die = {die.Item2}, modifier = {die.Item3:+0;-#}";
		}

		public static bool Test()
		{
			Dictionary<string, Tuple<int, int, int>> pairs = new Dictionary<string, Tuple<int, int, int>>();
			for (int i = 0; i < strs.Length; i++)
			{
				pairs.Add(strs[i], values[i]);
			}
			foreach (string str in strs)
			{
				try
				{
					var die = new DieString(str);
					int roll = die.Roll();
					bool success = die.NumberOfDice == pairs[str].Item1
								   && die.DieSides == pairs[str].Item2 &&
								   die.Modifier == pairs[str].Item3 && roll > die.NumberOfDice &&
								   roll <= die.NumberOfDice * die.DieSides + die.Modifier;
					if (!success)
					{
						Debug.WriteLine(
							$"DieString '{str}' failed. Results = {PrintStats(die)}\r\nExpectation = {PrintStats(pairs[str])} ");
						return false;
					}
					else
					{
						Debug.WriteLine(
							$"DieString '{str}' Passed. Results = {PrintStats(die)}\r\nExpectation = {PrintStats(pairs[str])} ");
					}
				}
				catch (ArgumentException)
				{
					Debug.WriteLine(
						$"dieString {str} failed.  This is {(pairs[str].Item1 == 0 ? "" : "not")} a good thing.");
				}
				catch (Exception)
				{
					Debug.WriteLine($"Another error occurred.  DieStr = {str}");
					return false;
				}


			}
			DieString d = new DieString("3d+4");
			DieString d2 = d.Add(4);

			return d2.NumberOfDice == 5 && d2.Modifier == 2;
		}

		bool ITester.Test()
		{
			return Test();
		}
	}

	public interface ITester
	{
		public bool Test();
	}

	public class TestFailedException : Exception
	{
		public Type TestFailed { get; set; } = null;
		public TestFailedException(Type type)
		{
			TestFailed = type;
		}
	}
}