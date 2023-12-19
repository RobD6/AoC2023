using System.Text.RegularExpressions;
using System;

public static class Day12
{
	public static int PartOne()
	{
		var lines = File.ReadAllLines("Day12.txt");
		
		//Example line
		//???.### 1,1,3
		int sum = 0;
		foreach (var line in lines)
		{
			string[] sections = line.Trim().Split(' ');
			List<int> groups = sections[1].Split(',').Select(int.Parse).ToList();
			int solutions = GetPossibleSolutionsWrapper(sections[0], groups);
			
			// Console.WriteLine($"{line} has {solutions} solutions");

			sum += solutions;
		}
		
		return sum;
	}

	private static Dictionary<(string, string), int> Cache = new();
	private static int GetPossibleSolutionsWrapper(string pattern, List<int> groups)
	{
		string groupStrings = "";
		foreach (int group in groups)
		{
			groupStrings += group + ", ";
		}

		bool wasCached = Cache.TryGetValue((pattern, groupStrings), out int result);

		if (wasCached)
		{
			return result;
		}
		
		int calculated = GetPossibleSolutions(pattern, groups);
		Cache[(pattern, groupStrings)] = calculated;
		return calculated;
	}

	private static int GetPossibleSolutions(string pattern, List<int> groups)
	{
		if (pattern.Length == 0)
		{
			return groups.Count == 0 ? 1 : 0;
		}
		
		if (pattern[0] == '.')
		{
			return GetPossibleSolutionsWrapper(pattern[1..], groups);
		}

		if (pattern[0] == '?')
		{
			return GetPossibleSolutionsWrapper(string.Concat("#", pattern.AsSpan(1)), new List<int>(groups)) +
			       GetPossibleSolutionsWrapper(string.Concat(".", pattern.AsSpan(1)), new List<int>(groups));
		}

		if (pattern[0] == '#')
		{
			if (groups.Count == 0)
			{
				return 0;
			}
			
			int groupLength = groups[0];
			if (pattern.Length < groupLength)
			{
				return 0;
			}
			
			for (int i = 0; i < groupLength; i++)
			{
				if (pattern[i] == '.')
				{
					return 0;
				}
			}

			if (pattern.Length > groupLength)
			{
				if (pattern[groupLength] == '#')
				{
					return 0;
				}
				else
				{
					List<int> newGroups = new List<int>(groups);
					newGroups.RemoveAt(0);
					return GetPossibleSolutionsWrapper(pattern[(groupLength+1)..], newGroups);					
				}
			}
			else
			{
				return groups.Count == 1 ? 1 : 0;
			}
		}

		return 0;
	}

	public static int PartTwo()
	{
		var lines = File.ReadAllLines("Day12.txt");
		
		//Example line
		//???.### 1,1,3
		int sum = 0;
		foreach (var line in lines)
		{
			string[] sections = line.Trim().Split(' ');
			string pattern = sections[0];
			string unfoldedPattern = $"{pattern}?{pattern}?{pattern}?{pattern}?{pattern}";
			List<int> groups = sections[1].Split(',').Select(int.Parse).ToList();
			List<int> unfoldedGroups = new List<int>();
			for (int i = 0; i < 5; i++)
			{
				unfoldedGroups.AddRange(groups);
			}
			int solutions = GetPossibleSolutionsWrapper(unfoldedPattern, unfoldedGroups);
			
			// Console.WriteLine($"{line} has {solutions} solutions");

			sum += solutions;
		}
		
		return sum;
	}
}