using System.Text.RegularExpressions;

public static class Day9
{
	public static long PartOne()
	{
		var lines = File.ReadAllLines("Day9.txt");

		long sum = 0;
		foreach (var line in lines)
		{
			List<long> sequence = line.Trim().Split(' ').Select(long.Parse).ToList();

			sum += GetNextNumber(sequence);
		}

		return sum;
	}

	private static long GetNextNumber(List<long> sequence)
	{
		List<List<long>> sequences = new() { sequence };

		while (sequences[^1].FirstOrDefault(x => x != 0) != 0)
		{
			List<long> lastSequence = sequences[^1];
			List<long> diffs = new();
			for (int i = 0; i < lastSequence.Count - 1; i++)
			{
				diffs.Add(lastSequence[i+1] - lastSequence[i]);
			}
			
			sequences.Add(diffs);
		}
		
		sequences[^1].Add(0);
		
		for (int i = sequences.Count-2; i >= 0; i--)
		{
			sequences[i].Add(sequences[i][^1] + sequences[i + 1][^1]);
		}

		return sequences[0][^1];
	}

	public static long PartTwo()
	{
		var lines = File.ReadAllLines("Day9.txt");

		long sum = 0;
		foreach (var line in lines)
		{
			List<long> sequence = line.Trim().Split(' ').Select(long.Parse).ToList();
			sequence.Reverse();

			sum += GetNextNumber(sequence);
		}

		return sum;
	}
}