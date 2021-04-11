namespace StatRoller
{
	public class Assassin : Beast
	{
		protected static readonly Bounds AssassinBounds = new Bounds
		{
			MinST = 25,
			MaxST = 35,
			MinDX = 13,
			MaxDX = 16,
			MinIQ = 4,
			MaxIQ = 6,
			MinHT = 11,
			MaxHT = 13,
			MinDR = 10,
			MaxDR = 15,
			MinPer = 6,
			MaxPer = 8,
			MinWill = 5,
			MaxWill = 6,
			MinHP = 0,
			MaxHP = 5,
			MinFP = 0,
			MaxFP = 5,
			MinSpeed = 0,
			MaxSpeed = 4,

		};

		private static readonly Tag AssassinTag = new Tag()
		{
			Description =
				" this beast will move with extreme stealth to launch its first attack completely undefended. It will retreat and attempt to do it again- going invisible costs 3 FP and takes 1 second of concentration.  Invisibility can be disrupted by damage. ",
			Difficulty = 15
		};

		protected override void Init()
		{
			GenerateBeast(AssassinBounds, this);
			Tags.Add(AssassinTag);

		}
	}
}