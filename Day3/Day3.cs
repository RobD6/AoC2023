using System.Text.RegularExpressions;

public static class Day3
{
	private struct Pos
	{
		public Pos(int _x, int _y)
		{
			x = _x;
			y = _y;
		}
		
		public int x, y;
	}
	
	private class MapData
	{
		public Dictionary<Pos, char> Symbols = new();
		public List<(Pos, int)> Values = new();
	}
	public static int PartOne()
	{
		var lines = File.ReadAllLines("Day3.txt");

		var mapData = ParseData(lines);
		int sum = 0;
		
		//Check for adjacent symbols to each value
		foreach (var value in mapData.Values)
		{
			if (GetAdjacentSymbols(value, mapData).Count != 0)
			{
				sum += value.Item2;
			}
		}

		return sum;
	}
	
	public static int PartTwo()
	{
		var lines = File.ReadAllLines("Day3.txt");
		
		var mapData = ParseData(lines);
		
		//Build a dictionary of values adjacent to each * symbol
		Dictionary<Pos, List<int>> gears = new();
		foreach (var value in mapData.Values)
		{
			foreach (var symbolPos in GetAdjacentSymbols(value, mapData))
			{
				if (mapData.Symbols[symbolPos] == '*')
				{
					if (!gears.ContainsKey(symbolPos))
					{
						gears[symbolPos] = new List<int>();
					}
					gears[symbolPos].Add(value.Item2);
				}
			}
		}

		int sum = 0;
		foreach (Pos pos in gears.Keys)
		{
			var valList = gears[pos];
			if (valList.Count == 2)
			{
				sum += (valList[0] * valList[1]);
			}
		}

		return sum;
	}

	private static List<Pos> GetAdjacentSymbols((Pos, int) valueTuple, MapData mapData)
	{
		Pos valPos = valueTuple.Item1;
		int length = valueTuple.Item2.ToString().Length;

		List<Pos> adjacent = new();
		for (int i = valPos.x - 1; i <= valPos.x + length; i++)
		{
			adjacent.Add(new Pos(i, valPos.y-1));
			adjacent.Add(new Pos(i, valPos.y+1));
		}
		adjacent.Add(new Pos(valPos.x-1, valPos.y));
		adjacent.Add(new Pos(valPos.x+length, valPos.y));

		List<Pos> results = new();
		foreach (var pos in adjacent)
		{
			if (mapData.Symbols.ContainsKey(pos))
			{
				results.Add(pos);
			}
		}

		return results;
	}

	private static MapData ParseData(string[] input)
	{
		MapData data = new();

		//Collect numbers
		string numRegex = @"(\d+)";

		int lineIndex = 0;
		foreach (var line in input)
		{
			foreach (Match match in Regex.Matches(line, numRegex))
			{
				var numEntry = (new Pos(match.Index, lineIndex), int.Parse(match.Value));
				data.Values.Add(numEntry);
			}

			lineIndex++;
		}
		
		//Create symbols
		lineIndex = 0;
		foreach (var line in input)
		{
			for (int index = 0; index < line.Length; index++)
			{
				char c = line[index];
				
				//Find anything that isn't an integer, period or newline
				if (!(char.IsDigit(c) || c == '.' || c == '\n'))
				{
					data.Symbols.Add(new Pos(index, lineIndex), c);
				}
			}

			lineIndex++;
		}

		return data;
	}
}