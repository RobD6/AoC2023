using System.Text.RegularExpressions;

public static class Day7
{
	private enum HandType
	{
		HighCard,
		Pair,
		TwoPair,
		ThreeOfKind,
		FullHouse,
		FourOfKind,
		FiveOfKind
	}

	private struct HandInfo
	{
		public HandType HandType;
		public string Cards;
		public int Bet;
	}

	private static Dictionary<char, int> CardValueDict = new Dictionary<char, int>()
	{
		{ '2', 2 },
		{ '3', 3 },
		{ '4', 4 },
		{ '5', 5 },
		{ '6', 6 },
		{ '7', 7 },
		{ '8', 8 },
		{ '9', 9 },
		{ 'T', 10 },
		{ 'J', 11 },
		{ 'Q', 12 },
		{ 'K', 13 },
		{ 'A', 14 },
	};

	private static Dictionary<char, int> CardValueDictPt2 = new Dictionary<char, int>()
	{
		{ '2', 2 },
		{ '3', 3 },
		{ '4', 4 },
		{ '5', 5 },
		{ '6', 6 },
		{ '7', 7 },
		{ '8', 8 },
		{ '9', 9 },
		{ 'T', 10 },
		{ 'J', 1 },
		{ 'Q', 12 },
		{ 'K', 13 },
		{ 'A', 14 },
	};

	private static Dictionary<char, int> CurrentCardValueDict;
	
	private static HandInfo GetHandInfo(string inputLine)
	{
		HandInfo info = new();
		//Hand in the form 32T3K 765
		var split = inputLine.Split(' ');
		info.Bet = int.Parse(split[1]);
		info.Cards = split[0];

		var cardsCounts = split[0].GroupBy(x => x).Select(x => x.Count()).ToList();
		cardsCounts.Sort((countA, countB) => countB.CompareTo(countA));	//Reverse order

		HandType hType = HandType.HighCard;

		switch (cardsCounts[0])
		{
			case 5:
				hType = HandType.FiveOfKind;
				break;
			case 4:
				hType = HandType.FourOfKind;
				break;
			case 3:
				hType = cardsCounts[1] == 2 ? HandType.FullHouse : HandType.ThreeOfKind;
				break;
			case 2:
				hType = cardsCounts[1] == 2 ? HandType.TwoPair : HandType.Pair;
				break;
			default:
				break;
		}

		info.HandType = hType;
		
		return info;
	}
	
	private static int CompareHands(HandInfo handA, HandInfo handB)
	{
		if (handA.HandType != handB.HandType)
		{
			return handA.HandType.CompareTo(handB.HandType);
		}

		for (int i = 0; i < handA.Cards.Length; i++)
		{
			if (handA.Cards[i] != handB.Cards[i])
			{
				return CurrentCardValueDict[handA.Cards[i]].CompareTo(CurrentCardValueDict[handB.Cards[i]]);
			}
		}

		return 0;
	}
	
	public static int PartOne()
	{
		CurrentCardValueDict = CardValueDict;
		var lines = File.ReadAllLines("Day7.txt");

		List<HandInfo> hands = new();
		foreach (var line in lines)
		{
			hands.Add(GetHandInfo(line.Trim()));
		}
		
		//Sort hands
		hands.Sort(CompareHands);

		int sum = 0;
		for (int i = 0; i < hands.Count(); i++)
		{
			sum += (i + 1) * hands[i].Bet;
		}
			
		return sum;
	}
	
	private static HandInfo GetHandInfo2(string inputLine)
	{
		HandInfo info = new();
		//Hand in the form 32T3K 765
		var split = inputLine.Split(' ');
		info.Bet = int.Parse(split[1]);
		info.Cards = split[0];
		
		//Count the Jacks and remove them
		string nonJackCards = info.Cards;
		int numJacks = nonJackCards.Count(x => x == 'J');
		nonJackCards = nonJackCards.Replace("J", "");

		List<int> cardCounts = new();
		if (nonJackCards.Length == 0)
		{
			cardCounts.Add(0);
		}
		else
		{
			cardCounts = nonJackCards.GroupBy(x => x).Select(x => x.Count()).ToList();
		}

		cardCounts.Sort((countA, countB) => countB.CompareTo(countA));	//Reverse order
		
		//Add all jacks to the biggest group
		cardCounts[0] += numJacks;

		HandType hType = HandType.HighCard;

		switch (cardCounts[0])
		{
			case 5:
				hType = HandType.FiveOfKind;
				break;
			case 4:
				hType = HandType.FourOfKind;
				break;
			case 3:
				hType = cardCounts[1] == 2 ? HandType.FullHouse : HandType.ThreeOfKind;
				break;
			case 2:
				hType = cardCounts[1] == 2 ? HandType.TwoPair : HandType.Pair;
				break;
			default:
				break;
		}

		info.HandType = hType;
		
		return info;
	}
	
	public static int PartTwo()
	{
		CurrentCardValueDict = CardValueDictPt2;
		
		var lines = File.ReadAllLines("Day7.txt");

		List<HandInfo> hands = new();
		foreach (var line in lines)
		{
			hands.Add(GetHandInfo2(line.Trim()));
		}
		
		//Sort hands
		hands.Sort(CompareHands);

		int sum = 0;
		for (int i = 0; i < hands.Count(); i++)
		{
			sum += (i + 1) * hands[i].Bet;
		}
			
		return sum;
	}
}