using System.Text.RegularExpressions;

public static class Day19
{
	private class Workflow
	{
		private class Rule
		{
			private readonly string Result;
			private enum RuleType
			{
				Const,
				Greater,
				Less
			}
			private readonly RuleType Type;
			private readonly int Value;
			private readonly char Component;
			
			public Rule(string ruleDesc)
			{
				const string comparison = @"(\w)([><])(\d+):(\w+)";
				var match = Regex.Match(ruleDesc, comparison);
				if (match.Success)
				{
					Component = match.Groups[1].Value[0];
					Type = match.Groups[2].Value[0] == '>' ? RuleType.Greater : RuleType.Less;
					Value = int.Parse(match.Groups[3].Value);
					Result = match.Groups[4].Value;
				}
				else
				{
					Type = RuleType.Const;
					Result = ruleDesc;
				}
			}

			public bool ApplyRule(PartInfo part, out string result)
			{
				switch (Type)
				{
					case RuleType.Const:
						result = Result;
						return true;
					case RuleType.Greater:
						if (part[Component] > Value)
						{
							result = Result;
							return true;
						}

						result = "";
						return false;
					case RuleType.Less:
						if (part[Component] < Value)
						{
							result = Result;
							return true;
						}

						result = "";
						return false;
					default:
						throw new ArgumentOutOfRangeException();
				}
			}
		}

		private List<Rule> Rules;
		public readonly string Name;
		
		public Workflow(string line)
		{
			string workflowRegex = @"(\w+){([\w><,:]+)}";
			var match = Regex.Match(line, workflowRegex);
			Name = match.Groups[1].Value;
			string rulesString = match.Groups[2].Value;
			string[] rules = rulesString.Split(',');
			Rules = new List<Rule>();
			foreach (var rule in rules)
			{
				Rules.Add(new Rule(rule));
			}
		}

		public string ApplyToPart(PartInfo part)
		{
			foreach (Rule rule in Rules)
			{
				string result;
				if (rule.ApplyRule(part, out result))
				{
					return result;
				}
			}

			throw new Exception("No rule applied!");
		}
	}

	private class PartInfo : Dictionary<char, int>
	{
		public PartInfo(string partDesc)
		{
			string partRegex = @"{x=(\d+),m=(\d+),a=(\d+),s=(\d+)}";
			var match = Regex.Match(partDesc, partRegex);
			Add('x', int.Parse(match.Groups[1].Value));
			Add('m', int.Parse(match.Groups[2].Value));
			Add('a', int.Parse(match.Groups[3].Value));
			Add('s', int.Parse(match.Groups[4].Value));
		}

		public int Sum()
		{
			return this['x'] + this['m'] + this['a'] + this['s'];
		}
	}
	
	public static int PartOne()
	{
		var lines = File.ReadAllLines("Day19.txt");

		Dictionary<string, Workflow> flows = new();
		
		int lineIndex = 0;
		while (lines[lineIndex].Trim().Length != 0)
		{
			Workflow flow = new Workflow(lines[lineIndex].Trim());
			flows[flow.Name] = flow;
			lineIndex++;
		}

		lineIndex++;

		int sum = 0;
		while (lineIndex < lines.Length)
		{
			var part = new PartInfo(lines[lineIndex].Trim());

			string currentFlow = "in";

			while (currentFlow != "A" && currentFlow != "R")
			{
				currentFlow = flows[currentFlow].ApplyToPart(part);
			}

			if (currentFlow == "A")
			{
				sum += part.Sum();
			}
			
			lineIndex++;
		}

		return sum;
	}
	
	public static int PartTwo()
	{
		var lines = File.ReadAllLines("Day19.txt");

		return lines.Length;
	}
}