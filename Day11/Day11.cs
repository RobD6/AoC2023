public static class Day11
{
	private static long Solve(string[] input, long expansion = 1)
	{
		List<(int, int)> positions = new();
		HashSet<int> emptyRows = new HashSet<int>();
		HashSet<int> emptyCols = new HashSet<int>();

		for (int x = 0; x < input[0].Trim().Length; x++)
		{
			emptyCols.Add(x);
		}

		for (int y = 0; y < input.Length; y++)
		{
			string line = input[y].Trim();
			bool foundGalaxy = false;
			for (int x = 0; x < line.Length; x++)
			{
				if (line[x] == '#')
				{
					positions.Add((x, y));
					foundGalaxy = true;
					if (emptyCols.Contains(x))
					{
						emptyCols.Remove(x);
					}
				}
			}

			if (!foundGalaxy)
			{
				emptyRows.Add(y);
			}
		}

		long sum = 0;
		for (int i = 0; i < positions.Count - 1; i++)
		{
			for (int j = i + 1; j < positions.Count; j++)
			{
				long manDist = Math.Abs(positions[i].Item1 - positions[j].Item1) +
				              Math.Abs(positions[i].Item2 - positions[j].Item2);

				manDist += CountEmpty(positions[i].Item1, positions[j].Item1, emptyCols) * expansion;
				manDist += CountEmpty(positions[i].Item2, positions[j].Item2, emptyRows) * expansion;
				sum += manDist;
			}
		}

		return sum;
	}
	
	private static int CountEmpty(int a, int b, HashSet<int> empties)
	{
		int from = Math.Min(a, b);
		int to = Math.Max(a, b);

		int count = 0;
		for (int i = from + 1; i < to; i++)
		{
			if (empties.Contains(i))
			{
				count++;
			}
		}

		return count;
	}
	
	public static long PartOne()
	{
		var lines = File.ReadAllLines("Day11.txt");

		return Solve(lines, 1);
	}
	
	public static long PartTwo()
	{
		var lines = File.ReadAllLines("Day11.txt");

		return Solve(lines, 1000000-1);
	}
}