namespace StatRoller
{
	public class Fortifier : Beast
	{
		protected static readonly Bounds FortifierBounds = new Bounds
		{
			MinST = 30,
			MaxST = 40,
			MinDX = 11,
			MaxDX = 14,
			MinIQ = 4,
			MaxIQ = 7,
			MinHT = 12,
			MaxHT = 15,
			MinDR = 15,
			MaxDR = 20,
			MinPer = 7,
			MaxPer = 10,
			MinWill = 5,
			MaxWill = 6,
			MinHP = 0,
			MaxHP = 5,
			MinFP = 0,
			MaxFP = 5,
			MinSpeed = 0,
			MaxSpeed = 4,

		};

		private static readonly Tag FortifierTag = new Tag()
		{
			Description =
				" this beast will retreat to its fortifications when it takes any significant damage, if it has one.  Otherwise it will just flee to a suitable position. ",
			Difficulty = 10
		};
		protected override void Init()
		{
			GenerateBeast(FortifierBounds, this);
			Tags.Add(FortifierTag);
		}
	}
}