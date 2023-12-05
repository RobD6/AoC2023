using System.Text.RegularExpressions;

public static class Day5
{
	struct MapRange
	{
		public long SourceStart;
		public long DestStart;
		public long Length;
	}

	class MapData
	{
		public string FromType;
		public string ToType;
		public List<MapRange> Ranges = new();
	}

	public static long PartOne()
	{
		var lines = File.ReadAllLines("Day5.txt");
		
		//Read seed numbers
		List<long> seeds = lines[0].Split(':')[1].Trim().Split(' ').Select(long.Parse).ToList();

		var mappings = ParseMappings(lines);

		long minDist = long.MaxValue;
		foreach (long seed in seeds)
		{
			long currentVal = seed;
			
			foreach (MapData map in mappings)
			{
				foreach (var range in map.Ranges.Where(range => currentVal >= range.SourceStart && currentVal < range.SourceStart + range.Length))
				{
					currentVal = range.DestStart + (currentVal - range.SourceStart);
					break;
				}
			}
			if (currentVal < minDist)
			{
				minDist = currentVal;
			}
		}
		
		return minDist;
	}

	private static List<MapData> ParseMappings(string[] lines)
	{
		const string rangeStartRegex = @"(\w+)-to-(\w+) map:";

		List<MapData> mappings = new();
		MapData currentMap = null;

		for (int i = 1; i < lines.Length; i++)
		{
			string line = lines[i].Trim();

			if (line.Length == 0)
			{
				continue;
			}

			var match = Regex.Match(line, rangeStartRegex);
			if (match.Success)
			{
				currentMap = new MapData()
				{
					FromType = match.Groups[1].Value,
					ToType = match.Groups[2].Value
				};
				mappings.Add(currentMap);
			}
			else
			{
				//Must be a range line
				List<long> rangeData = line.Split(' ').Select(long.Parse).ToList();
				currentMap.Ranges.Add(new MapRange()
					{ DestStart = rangeData[0], SourceStart = rangeData[1], Length = rangeData[2] });
			}
		}

		return mappings;
	}


	public static long PartTwo()
	{
		var lines = File.ReadAllLines("Day5.txt");

		return lines.Length;
	}
}