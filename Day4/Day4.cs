using System.Text.RegularExpressions;

public static class Day4
{
	public static int PartOne()
	{
		var lines = File.ReadAllLines("Day4.txt");

		int sum = 0;
		foreach (var line in lines)
		{
			var matches = CountMatches(line);
			
			int score = 0;
			if (matches > 0)
			{
				score = 1;
				for (int i = 1; i < matches; i++)
				{
					score *= 2;
				}
			}

			sum += score;
		}

		return sum;
	}

	private static int CountMatches(string card)
	{
		var myNums_WinningNums = card.Split(':')[1].Split('|');

		var myNums = myNums_WinningNums[0].Trim().Split(' ').ToList().Where(x => x.Length > 0).Select(int.Parse)
			.ToHashSet();
		var winningNums = myNums_WinningNums[1].Trim().Split(' ').ToList().Where(x => x.Length > 0)
			.Select(int.Parse).ToHashSet();

		myNums.IntersectWith(winningNums);
		return myNums.Count;
	}


	public static long PartTwo()
	{
		var lines = File.ReadAllLines("Day4.txt");

		Dictionary<int, Int64> cardNumCopies = new();
		for (int i = 0; i < lines.Length; i++)
		{
			cardNumCopies[i] = 1;
		}

		for (int i = 0; i < lines.Length; i++)
		{
			int score = CountMatches(lines[i]);
			long dupes = cardNumCopies[i];

			for (int copy = 0; copy < score; copy++)
			{
				int cardToCopy = i + copy + 1;
				if (cardToCopy < lines.Length)
				{
					cardNumCopies[cardToCopy] += dupes;
				}
			}
		}

		return cardNumCopies.Values.Sum(x => x);
	}
}