using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Common;
using Common.Streams;

namespace Day17
{
	public class Program
	{
		static void Main()
		{
			var data = File.ReadAllText("input.txt").Split(',').Select(long.Parse).ToArray();
			var image = Part1(data);
			Part2(data, image);
		}
		
		private static List<List<char>> Part1(long[] data)
		{
			var program = new IntCodeProgram2(data, 1000000);
			var image = new List<List<char>>();
			long output = 0;
			image.Add(new List<char>());
			int i = 0;
			while(true)
			{
				output = program.ExecuteTilOutput();
				if(output == 99)
					break;
				var c = (char)output;
				if(c == '\n')
				{
					image.Add(new List<char>());
					i++;
				}
				else
					image[i].Add(c);
			}
			image.RemoveAt(i);
			image.RemoveAt(i-1);
			int sum = 0;
			for(i = 1; i < image.Count-1; i++)
			{
				for(int j = 1; j < image[i].Count-1; j++)
				{
					if(image[i][j] == '#' && image[i-1][j] == '#' && image[i+1][j] == '#' 
						&& image[i][j-1] == '#' && image[i][j+1] == '#')
					{
						sum += i * j; 
					}
				}
			}
			Console.WriteLine(sum);
			return image;
		}
		
		private static void Part2(long[] data, List<List<char>> image)
		{
			data[0] = 2;
			var path = FindPath(image);
			foreach(var (a,b,c,main) in TrySplitIntoABC(path))
			{
				var mainEncoded = EncodeProcedure(main);
				var aEncoded = EncodeProcedure(a);
				var bEncoded = EncodeProcedure(b);
				var cEncoded = EncodeProcedure(c);
				if(mainEncoded.Length <= 21 && aEncoded.Length <= 21 && bEncoded.Length <= 21 && cEncoded.Length <= 21)
				{
					var input = new List<long>();
					input.AddRange(mainEncoded.Select(c => (long)c));
					input.AddRange(aEncoded.Select(c => (long)c));
					input.AddRange(bEncoded.Select(c => (long)c));
					input.AddRange(cEncoded.Select(c => (long)c));
					input.Add((long)'n');
					input.Add((long)'\n');
					var program = new IntCodeProgram(new InputStream(input), new OutputStream());
					program.Run(data, capacity: 1000000000);
					var output = program.GetOutput();
					Console.WriteLine(output.Last());
					return;
				}
			}
		}

		private static string EncodeProcedure(List<string> routine)
		{
			return string.Join(",", routine.Select(r => {
				if(r.Length == 1) return r;
				return $"{r[0]},{r.Substring(1)}";
			})) + '\n';
		}

		private static IEnumerable<(List<string>, List<string>, List<string>, List<string>)> TrySplitIntoABC(string path)
		{
			var items = path.Split(',').ToList();
			for(int aRoutineLength = 1; aRoutineLength <= 10; aRoutineLength++)
			{
				var aRoutine = items.Take(aRoutineLength).ToList();
				var listA = ReplaceRoutine(items, aRoutine, "A");
				for(int bRoutineLenght = 1; bRoutineLenght <= 10; bRoutineLenght++)
				{
					var bRoutine = listA.SkipWhile(i => i == "A").Take(bRoutineLenght).ToList();
					var listB = ReplaceRoutine(listA, bRoutine, "B");
					for(int cRoutineLength = 1; cRoutineLength <= 10; cRoutineLength++)
					{
						var cRoutine = listB.SkipWhile(i => i == "A" || i == "B").Take(cRoutineLength).ToList();
						if(cRoutine.Count == 0 || cRoutine.Contains("A") || cRoutine.Contains("B"))
							break;
						var listC = ReplaceRoutine(listB, cRoutine, "C");
						if(listC.Count <= 10 && listC.All(s => s.Length == 1))
						{
							yield return (aRoutine, bRoutine, cRoutine, listC);
						}
					}
				}
			}
		}

		private static List<string> ReplaceRoutine(List<string> items, List<string> routine, string to)
		{
			var list = new List<string>();
			int s = 0;
			while(s < items.Count)
			{
				var took = items.Skip(s).Take(routine.Count).ToList();
				bool equal = false;
				if(took.Count == routine.Count)
				{
					equal = true;
					for(int i = 0; i < took.Count; i++)
						if(took[i] != routine[i])
						{
							equal = false;
							break;
						}
				}
				if(equal)
				{
					list.Add(to);
					s += routine.Count;
				}
				else
				{
					list.Add(items[s]);
					s++;
				}
			}
			return list;
		}

		private static string FindPath(List<List<char>> image)
		{
			var robotPosition = (0,0);
			for(int i = 0; i < image.Count; i++)
				for(int j = 0; j < image[i].Count; j++)
					if(image[i][j] != '.' && image[i][j] != '#')
					{
						robotPosition = (i,j);
						break;
					}
			char direction = image[robotPosition.Item1][robotPosition.Item2];
			int moves = 0;
			bool follow;
			char turn;
			StringBuilder sb = new StringBuilder();
			while(true)
			{
				(follow, turn, robotPosition) = FollowDirection(image, robotPosition, direction);
				if(follow) 
					moves++;
				else if(turn == 'E')
				{
					sb.Append(moves+1);
					break;
				}
				else
				{
					sb.Append(moves > 0 ? $"{moves+1},{turn}" : $"{turn}");
					moves = 0;
					direction = (turn == 'L' ? TurnLeft(direction) : TurnRight(direction));
				}
				
			}	
			return sb.ToString();
		}

		private static (bool, char, (int,int)) FollowDirection(List<List<char>> image, (int,int) position, char direction, bool turn = false)
		{
			var (i,j) = NextPosition(position, direction);
			if(i >= 0 && i < image.Count && j >= 0 && j < image[i].Count && image[i][j] == '#')
				return (true, direction, (i,j));
			if(!turn)
			{
				var left = FollowDirection(image, position, TurnLeft(direction), true);
				if(left.Item1)
					return (false, 'L', left.Item3);
				var right = FollowDirection(image, position, TurnRight(direction), true);
				if(right.Item1)
					return (false, 'R', right.Item3);
			}
			return (false, 'E', (i,j));
		}

		private static char TurnLeft(char direction)
		{
			return direction switch
			{
				'^' => '<',
				'v' => '>',
				'<' => 'v',
				'>' => '^',
				_ => direction
			};
		}

		private static char TurnRight(char direction)
		{
			return direction switch
			{
				'^' => '>',
				'v' => '<',
				'<' => '^',
				'>' => 'v',
				_ => direction
			};
		}

		private static (int,int) NextPosition((int,int) position, char direction)
		{
			var (i,j) = position;
			return direction switch
			{
				'^' => (i-1, j),
				'v' => (i+1, j),
				'<' => (i, j-1),
				'>' => (i, j+1),
				_ => position
			};
		}
	}
}