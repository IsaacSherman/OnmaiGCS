using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading;

namespace StatRoller
{
	class Program
	{
			static Random rng = new Random();
			static Dictionary<int, int> dict = new Dictionary<int, int>();
			private static object Lock = new object();
		static void Main(string[] args)
		{

			long num = 1000000000000;
			int sum=0;
			//DieString d = new DieString("5d20");
			int dice = 25, size = 4;
			for ( ; num > 0; --num)
			{
				sum = 0;
				object[] ThreadArgs = {dice, size};
				Thread newThread = new Thread(RollDice,0);
				newThread.Start(ThreadArgs);

				
			}

			List<int> list = new List<int>(dict.Keys.ToArray());
			list.Sort();
			foreach (int key in list)
			{
				Console.WriteLine($"{key}: {dict[key]}");
			}
		}

		private static void RollDice(object boxed)
		{
			Object[] box = boxed as Object[];
			RollDice((int)box[0], (int)box[1]);
		}

		private static void RollDice(int dice, int size)
		{
			int sum = 0;
			for (int f = 0; f < dice; f++)
			{
				sum += rng.Next(1, size + 1);
			}

			lock(Lock){
				if (dict.ContainsKey(sum))
				{
					dict[sum] += 1;
				}
				else
				{
					dict.Add(sum, 1);
				}
			}
		}
	}
}
