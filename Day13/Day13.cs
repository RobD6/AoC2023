using System.Text.RegularExpressions;

public static class Day13
{
	private class RockField
	{
		public List<int> Rows;
		public List<int> Columns;

		public RockField(List<string> input)
		{
			//Construct rows
			Rows = new();
			for (int i = 0; i < input.Count; i++)
			{
				int sum = 0;
				for (int pow = 0; pow < input[i].Trim().Length; pow++)
				{
					if (input[i][pow] == '#')
					{
						sum += (int)Math.Pow(2, pow);
					}
				}
				Rows.Add(sum);
			}
			
			//Construct Columns
			Columns = new();
			for (int i = 0; i < input[0].Trim().Length; i++)
			{
				int sum = 0;
				for (int pow = 0; pow < input.Count; pow++)
				{
					if (input[pow][i] == '#')
					{
						sum += (int)Math.Pow(2, pow);
					}
				}
				Columns.Add(sum);	
			}
		}

		public int ScorePt1()
		{
			int sum = 0;
			sum += FindReflections(Columns).Sum();
			sum += FindReflections(Rows).Sum() * 100;
			return sum;
		}

		private List<int> FindReflections(List<int> data)
		{
			List<int> results = new();
			for (int i = 0; i < data.Count - 1; i++)
			{
				if (data[i] == data[i + 1])
				{
					//Found two matching rows. Check outwards
					bool isReflection = true;
					for (int j = 1; (j <= i) && (i + j + 1 < data.Count); j++)
					{
						if (data[i - j] != data[i + 1 + j])
						{
							isReflection = false;
							break;
						}
					}

					if (isReflection)
					{
						results.Add(i+1);
					}
				}
			}

			return results;
		}
	}
	
	public static int PartOne()
	{
		var lines = File.ReadAllLines("Day13.txt");

		List<string> accum = new();
		List<RockField> fields = new();
		foreach (var line in lines)
		{
			if (string.IsNullOrEmpty(line.Trim()))
			{
				fields.Add(new RockField(accum));
				accum.Clear();
			}
			else
			{
				accum.Add(line.Trim());
			}
		}
		fields.Add(new RockField(accum));

		return fields.Sum(x => x.ScorePt1());
	}
	
	public static int PartTwo()
	{
		var lines = File.ReadAllLines("Day13.txt");

		return lines.Length;
	}
}