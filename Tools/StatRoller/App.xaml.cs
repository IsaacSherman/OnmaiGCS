using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace StatRoller
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		private static List<ITester> Testers = new List<ITester>
		{
			new DieStringTester(),
			new BeastTester(),
			new TagTester(),
			new FactoryTester(),
		};
		public static Random rng = new Random();

		private static bool DoTesting = true;
		static App()
		{
			if (DoTesting)
			{
				DoTests();
			}
		}

		private static void DoTests()
		{
			foreach (ITester tester in Testers)
			{
				if (!tester.Test())
				{
					throw new TestFailedException(tester.GetType());
				}
			}
		}
	}
}
