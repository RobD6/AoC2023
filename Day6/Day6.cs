using System.Text.RegularExpressions;

public static class Day6
{
	public static int PartOne()
	{
		var lines = File.ReadAllLines("Day6.txt");
		
		var times = Regex.Matches(lines[0].Split(':')[1], @"\d+").Select(x => int.Parse(x.Value)).ToArray();
		var dists = Regex.Matches(lines[1].Split(':')[1], @"\d+").Select(x => int.Parse(x.Value)).ToArray();

		int mult = 1;
		for (int i = 0; i < times.Length; i++)
		{
			int time = times[i];
			int record = dists[i];

			int numWinning = 0;

			for (int t = 0; t < time; t++)
			{
				int dist = t * (time - t);
				if (dist > record)
				{
					numWinning++;
				}
			}

			mult *= numWinning;
		}
		
		return mult;
	}
	
	//Did this as a copy and paste of part one assuming it'd need optimising.
	//Turns out it finishes instantly.
	public static long PartTwo()
	{
		var lines = File.ReadAllLines("Day6.txt");

		var times = Regex.Matches(lines[0].Replace(" ", "").Split(':')[1], @"\d+").Select(x => long.Parse(x.Value)).ToArray();
		var dists = Regex.Matches(lines[1].Replace(" ", "").Split(':')[1], @"\d+").Select(x => long.Parse(x.Value)).ToArray();

		long mult = 1;
		for (long i = 0; i < times.Length; i++)
		{
			long time = times[i];
			long record = dists[i];

			long numWinning = 0;

			for (long t = 0; t < time; t++)
			{
				long dist = t * (time - t);
				if (dist > record)
				{
					numWinning++;
				}
			}

			mult *= numWinning;
		}
		
		return mult;
	}
}