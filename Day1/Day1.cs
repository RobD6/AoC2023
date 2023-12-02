using System;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AoC2023.Day1
{
	public static class Day1
	{
		public static int PartOne()
		{
			var lines = File.ReadAllLines("Day1.txt");

			int sum = 0;
			foreach (var line in lines)
			{
				var nums = line.Where(x => Char.IsNumber(x)).ToArray();
				sum += (nums[0] - '0') * 10 + (nums[^1] - '0');
			}

			return sum;
		}

		public static int PartTwo()
		{
			var lines = File.ReadAllLines("Day1.txt");

			List<(string, int)> tokenVals = new();

			for (int i = 1; i <= 9; i++)
			{
				tokenVals.Add((i.ToString(), i));
			}
			tokenVals.Add(("one", 1));
			tokenVals.Add(("two", 2));
			tokenVals.Add(("three",3));
			tokenVals.Add(("four", 4));
			tokenVals.Add(("five", 5));
			tokenVals.Add(("six", 6));
			tokenVals.Add(("seven", 7));
			tokenVals.Add(("eight", 8));
			tokenVals.Add(("nine", 9));

			int sum = 0;
			
			foreach (var line in lines)
			{
				List<(int, int)> valPosition = new();
				foreach (var tokenVal in tokenVals)
				{
					valPosition.AddRange(Regex.Matches(line, tokenVal.Item1).ToArray().
						Select(match => (tokenVal.Item2, match.Index)));
				}
				
				valPosition.Sort(((vp1, vp2) => vp1.Item2.CompareTo(vp2.Item2)));
				sum += valPosition[0].Item1 * 10 + valPosition[^1].Item1;
			}

			return sum;
		}
	}
}