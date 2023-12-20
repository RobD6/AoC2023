using System.Text.RegularExpressions;

public static class Day20
{
	private enum Pulse
	{
		Low,
		High
	}

	private struct Signal
	{
		public Pulse Pulse;
		public string From;
		public string To;

		public Signal(Pulse pulse, string from, string to)
		{
			Pulse = pulse;
			From = from;
			To = to;
		}
	}
	
	private class Module
	{
		private enum ModuleType
		{
			Flip,
			Conjunct,
			Broadcast
		}

		private ModuleType Type;
		private Dictionary<string, Pulse> ConjInputStates;
		private bool FlipState;
		private List<string> Targets;
		public readonly string Name;
		
		public Module(string input)
		{
			//example input
			//%a -> inv, con
			string[] parts = input.Split(" -> ");
			
			//Parse name
			switch (parts[0][0])
			{
				case '%':
					Type = ModuleType.Flip;
					Name = parts[0][1..];
					FlipState = false;
					break;
				case '&':
					Type = ModuleType.Conjunct;
					Name = parts[0][1..];
					ConjInputStates = new();
					break;
				default:
					Type = ModuleType.Broadcast;
					Name = parts[0];
					break;
			}
			
			//Parse destinations
			if (!string.IsNullOrEmpty(parts[1]))
			{
				Targets = parts[1].Split(',').Select(x => x.Trim()).ToList();
			}
			else
			{
				Targets = new();
			}
		}

		public void SendPulse(Pulse pulse, string from, Queue<Signal> pulseQueue)
		{
			Pulse resultPulse;
			switch (Type)
			{
				case ModuleType.Flip:
					if (pulse == Pulse.High)
					{
						return;
					}
					FlipState = !FlipState;
					resultPulse = FlipState ? Pulse.High : Pulse.Low;
					break;
				case ModuleType.Conjunct:
					ConjInputStates[from] = pulse;
					resultPulse = ConjInputStates.Values.Any(x => x == Pulse.Low) ? Pulse.High : Pulse.Low;
					break;
				case ModuleType.Broadcast:
					resultPulse = pulse;
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}

			foreach (var target in Targets)
			{
				pulseQueue.Enqueue(new Signal(resultPulse, Name, target));
			}
		}

		public void RegisterInputs(Dictionary<string, Module> moduleDict)
		{
			foreach (var target in Targets)
			{
				if (!moduleDict.ContainsKey(target))
				{
					continue;
				}
				Module targetMod = moduleDict[target];
				if (targetMod.Type == ModuleType.Conjunct)
				{
					targetMod.ConjInputStates[Name] = Pulse.Low;
				}
			}
		}
	}
	
	public static int PartOne()
	{
		var lines = File.ReadAllLines("Day20.txt");

		Dictionary<string, Module> ModuleDict = new();

		foreach (var line in lines)
		{
			Module newMod = new Module(line.Trim());
			ModuleDict[newMod.Name] = newMod;
		}

		foreach (Module mod in ModuleDict.Values)
		{
			mod.RegisterInputs(ModuleDict);
		}

		Queue<Signal> pulseQueue = new();
		int numHigh = 0, numLow = 0;
		
		for (int i = 0; i < 1000; i++)
		{
			pulseQueue.Enqueue(new Signal(Pulse.Low, "", "broadcaster"));

			while (pulseQueue.Count > 0)
			{
				Signal sig = pulseQueue.Dequeue();
				if (sig.Pulse == Pulse.High)
				{
					numHigh++;
				}
				else
				{
					numLow++;
				}

				if (ModuleDict.ContainsKey(sig.To))
				{
					ModuleDict[sig.To].SendPulse(sig.Pulse, sig.From, pulseQueue);
				}
			}
		}
		
		return numHigh * numLow;
	}
	
	public static int PartTwo()
	{
		var lines = File.ReadAllLines("Day20.txt");

		Dictionary<string, Module> ModuleDict = new();

		foreach (var line in lines)
		{
			Module newMod = new Module(line.Trim());
			ModuleDict[newMod.Name] = newMod;
		}

		foreach (Module mod in ModuleDict.Values)
		{
			mod.RegisterInputs(ModuleDict);
		}

		Queue<Signal> pulseQueue = new();
		int pushes = 0;
		bool hasLitOutput = false;
		while (!hasLitOutput)
		{
			pushes++;
			pulseQueue.Enqueue(new Signal(Pulse.Low, "", "broadcaster"));

			while (pulseQueue.Count > 0)
			{
				Signal sig = pulseQueue.Dequeue();

				if (sig.To == "rx" && sig.Pulse == Pulse.Low)
				{
					hasLitOutput = true;
					break;
				}

				if (ModuleDict.ContainsKey(sig.To))
				{
					ModuleDict[sig.To].SendPulse(sig.Pulse, sig.From, pulseQueue);
				}
			}
		}
		
		return pushes;
	}
}