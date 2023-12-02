using System.Text.RegularExpressions;

public static class Day2
{
	//Define regex pattern to recognise "3 blue" or "5 red" etc.
	private static string numColRegex = @"(\d+) (\w+)";
	
	public static int PartOne()
	{
		Dictionary<string, int> maxCols = new Dictionary<string, int>()
		{
			{ "red", 12 },
			{ "green", 13 },
			{ "blue", 14 }
		};
		
		var lines = File.ReadAllLines("Day2.txt");

		int matchId = 1;
		int sumIds = 0;
		foreach (var line in lines)
		{
			var gameId_Rounds = line.Split(':');
			var rounds = gameId_Rounds[1].Split(';');

			bool isValid = true;
			foreach (var round in rounds)
			{
				foreach (Match match in Regex.Matches(round, numColRegex))
				{
					int numBalls = int.Parse(match.Groups[1].Value);
					string col = match.Groups[2].Value;

					if (numBalls > maxCols[col])
					{
						isValid = false;
						break;
					}
				}

				if (!isValid)
				{
					break;
				}
			}

			if (isValid)
			{
				sumIds += matchId;
			}

			matchId++;
		}
		return sumIds;
	}
	
	public static int PartTwo()
	{
		var lines = File.ReadAllLines("Day2.txt");

		int sum = 0;
		foreach (var line in lines)
		{
			var gameId_Rounds = line.Split(':');
			var rounds = gameId_Rounds[1].Split(';');
			
			Dictionary<string, int> maxCols = new()
			{
				{ "red", 0 },
				{ "green", 0 },
				{ "blue", 0 }
			};

			bool isValid = true;
			foreach (var round in rounds)
			{
				foreach (Match match in Regex.Matches(round, numColRegex))
				{
					int numBalls = int.Parse(match.Groups[1].Value);
					string col = match.Groups[2].Value;

					if (numBalls > maxCols[col])
					{
						maxCols[col] = numBalls;
					}
				}
			}
			
			sum += maxCols.Values.Aggregate(1, (x,y) => x * y);
		}

		return sum;
	}
}