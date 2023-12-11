using System.Text.RegularExpressions;
public static class Day10
{
	private enum Direction
	{
		North,
		South,
		East,
		West
	}

	public struct Position
	{
		public int X;
		public int Y;

		public Position(int x, int y)
		{
			X = x;
			Y = y;
		}
		public override int GetHashCode()
		{
			return X ^ Y;
		}

		public static bool operator ==(Position a, Position b)
		{
			return a.X == b.X && a.Y == b.Y;
		}

		public static bool operator !=(Position a, Position b)
		{
			return !(a == b);
		}
	}
	
	class PipeMap : Dictionary<Position, (Position, Position)>
	{
		public Position StartPoint;

		public PipeMap(string[] input)
		{
			for (int y = 0; y < input.Length; y++)
			{
				string line = input[y].Trim();
				for (int x = 0; x < line.Length; x++)
				{
					Position coord = new(x, y);

					switch (line[x])
					{
						case 'S':
							StartPoint = coord;
							break;
						case '|':
							Add(coord, (new Position(x, y-1), new Position(x, y+1)));
							break;
						case '-':
							Add(coord, (new Position(x-1, y), new Position(x+1, y)));
							break;
						case 'L':
							Add(coord, (new Position(x+1, y), new Position(x, y-1)));
							break;
						case 'J':
							Add(coord, (new Position(x-1, y), new Position(x, y-1)));
							break;
						case '7':
							Add(coord, (new Position(x-1, y), new Position(x, y+1)));
							break;
						case 'F':
							Add(coord, (new Position(x+1, y), new Position(x, y+1)));
							break;
						case '.':
							break;
						default:
							throw new Exception($"Unknown symbol {line[x]}");
					}
				}
			}
		}

		public bool TryGetNextPosition(Position pos, Position prev, out Position next)
		{
			if (ContainsKey(pos))
			{
				(Position, Position) links = this[pos];

				if (links.Item1 == prev)
				{
					next = links.Item2;
					return true;
				}
				else if (links.Item2 == prev)
				{
					next = links.Item1;
					return true;
				}
			}

			next = new Position(-1, -1);
			return false;
		}
	};

	

	public static int PartOne()
	{
		var lines = File.ReadAllLines("Day10.txt");

		PipeMap map = new(lines);
		var startPos = map.StartPoint;

		Position[] possibleNexts = new[]
		{
			new Position(startPos.X, startPos.Y - 1),
			new Position(startPos.X, startPos.Y + 1),
			new Position(startPos.X - 1, startPos.Y),
			new Position(startPos.X + 1, startPos.Y)
		};

		var current = startPos;
		foreach (var firstMove in possibleNexts)
		{
			int steps = 0;
			bool foundLoop = false;
			Position next = firstMove;
			while (map.TryGetNextPosition(next, current, out Position newNext))
			{
				steps++;
				if (newNext == startPos)
				{
					return (steps + 1) / 2;
				}

				current = next;
				next = newNext;
			}
		}

		return -1;
	}
	
	public static int PartTwo()
	{
		var lines = File.ReadAllLines("Day10.txt");

		return lines.Length;
	}
}