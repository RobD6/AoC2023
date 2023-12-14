using System.Text.RegularExpressions;

public static class Day14
{
	public static int PartOne()
	{
		var lines = File.ReadAllLines("Day14.txt");

		List<(int, int)> roundRocks = new();
		HashSet<(int, int)> blockers = new();

		for (int y = 0; y < lines.Length; y++)
		{
			var trimmedLine = lines[y].Trim();
			for (int x = 0; x < trimmedLine.Length; x++)
			{
				char c = trimmedLine[x];
				switch (c)
				{
					case '#':
						blockers.Add((x, y));
						break;
					case 'O':
						roundRocks.Add((x, y));
						break;
				}
			}
		}
		
		// Process the round rocks from bottom to top. No need to sort first, as we added from the top down.
		// Move the round rocks to the blockers list once they're settled
		int load = 0;
		int numRows = lines.Length;
		while (roundRocks.Count > 0)
		{
			int x = roundRocks[0].Item1;
			int y = roundRocks[0].Item2;

			while (y >= 1 && !blockers.Contains((x, y - 1)))
			{
				y--;
			}

			blockers.Add((x, y));

			load += numRows - y;
			
			roundRocks.RemoveAt(0);
		}

		return load;
	}
	
	public static int PartTwo()
	{
		var lines = File.ReadAllLines("Day14.txt");

		return lines.Length;
	}
}