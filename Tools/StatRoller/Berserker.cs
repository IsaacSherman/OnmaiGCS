namespace StatRoller
{
	public class Berserker : Beast
	{
		protected static readonly Bounds BerserkerBounds = new Bounds
		{
			MinST = 40,
			MaxST = 50,
			MinDX = 10,
			MaxDX = 13,
			MinIQ = 4,
			MaxIQ = 6,
			MinHT = 13,
			MaxHT = 16,
			MinDR = 20,
			MaxDR = 25,
			MinPer = 5,
			MaxPer = 6,
			MinWill = 5,
			MaxWill = 6,
			MinHP = 10,
			MaxHP = 15,
			MinFP = 0,
			MaxFP = 5,
			MinSpeed = 0,
			MaxSpeed = 4,
			MinMove = 3,
			MaxMove = 6
		};

		private static readonly Tag BerserkerTag = new Tag()
		{
			Description =
				" this beast will focus on a single enemy to the exclusion of all else once it enters combat.  Roll randomly for any targets in its line of sight.  If its line of sight is broken, make a Per roll- a success means it stays fixated, a failure means it checks again.  One exception- the first target to run away will become the new target of fixation, and it will try to trample.  Otherwise it will use all-out attacks on its fixated target until said target stops moving.  You don't want to know what happens next.",
			Difficulty = 20
		};

		protected override void Init()
		{
			GenerateBeast(BerserkerBounds, this);
			Tags.Add(BerserkerTag);
		}
	}
}