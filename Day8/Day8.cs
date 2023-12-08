using System.Text.RegularExpressions;

public static class Day8
{
	private class Map : Dictionary<string, (string, string)>
	{
		//AAA = (BBB, CCC)
		private static string LineRegex = @"(\w+) = \((\w+), (\w+)\)";
		public Map(string[] input)
		{
			foreach (var line in input)
			{
				Match match = Regex.Match(line.Trim(), LineRegex);
				if (match.Success)
				{
					this[match.Groups[1].Value] = (match.Groups[2].Value, match.Groups[3].Value);
				}
			}
		}

		public string NextNode(string currentNode, string directions, long step)
		{
			char direction = directions[(int)(step % directions.Length)];

			return direction == 'L' ? this[currentNode].Item1 : this[currentNode].Item2; 
		}
	}
	
	public static int PartOne()
	{
		var lines = File.ReadAllLines("Day8.txt");

		string directions = lines[0].Trim();
		Map map = new Map(lines);

		int steps = 0;
		string currentNode = "AAA";

		while (currentNode != "ZZZ")
		{
			currentNode = map.NextNode(currentNode, directions, steps);
			
			steps++;
		}

		return steps;
	}
	
	public static int PartTwo()
	{
		var lines = File.ReadAllLines("Day8.txt");

		string directions = lines[0].Trim();
		Map map = new Map(lines);

		List<string> startNodes = map.Keys.Where(x => x.EndsWith("A")).ToList();

		List<(List<int>, int)> possibleEndSteps = new();

		foreach (string startPoint in startNodes)
		{
			List<int> nodeEndSteps = new List<int>();
			int steps = 0;
			HashSet<(string, int)> seenNodes = new();
			string currentNode = startPoint;

			while (!seenNodes.Contains((currentNode, steps % directions.Length)))
			{
				if (currentNode.EndsWith('Z'))
				{
					nodeEndSteps.Add(steps);
				}
				
				seenNodes.Add((currentNode, steps % directions.Length));
				currentNode = map.NextNode(currentNode, directions, steps);
				steps++;
			}
			
			possibleEndSteps.Add((nodeEndSteps, steps));
		}
		
		return 0;
	}
}