namespace StatRoller
{
	public class Tactician : Beast
	{
		protected static readonly Bounds TacticianBounds = new Bounds
		{
			MinST = 35,
			MaxST = 50,
			MinDX = 11,
			MaxDX = 15,
			MinIQ = 6,
			MaxIQ = 8,
			MinHT = 13,
			MaxHT = 15,
			MinDR = 20,
			MaxDR = 25,
			MinPer = 5,
			MaxPer = 8,
			MinWill = 5,
			MaxWill = 6,
			MinHP = 5,
			MaxHP = 10,
			MinFP = 0,
			MaxFP = 5,
			MinSpeed = 0,
			MaxSpeed = 4,
			MinMove = 1,
			MaxMove = 3
		};

		private static readonly Tag TacticianTag = new Tag()
		{
			Description =
				" this beast will always focus on the weakest target.  It determines it with uncanny skill and clarity, and it is strong enough to be one of the most dangerous of the beast types.",
			Difficulty = 20
		};

		protected override void Init()
		{
			GenerateBeast(TacticianBounds, this);
			AddTag(TacticianTag);
		}
	}
}