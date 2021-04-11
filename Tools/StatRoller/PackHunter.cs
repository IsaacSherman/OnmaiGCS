namespace StatRoller
{
	public class PackHunter : Beast
	{
		static Bounds PackHunterBounds = new Bounds
		{		      
			MinST     = 25,
			MaxST     = 35,
			MinDX     = 11,
			MaxDX     = 14,
			MinIQ     = 5,
			MaxIQ     = 7,
			MinHT     = 11,
			MaxHT     = 13,
			MinDR     = 10,
			MaxDR     = 15,
			MinPer    = 6,
			MaxPer    = 8,
			MinWill   = 5,
			MaxWill   = 6,
			MinHP     = 0,
			MaxHP     = 5,
			MinFP     = 0,
			MaxFP     = 5,
			MinSpeed  = 0,
			MaxSpeed  = 4,

		};

		private static Tag PackHunterTag = new Tag()
		{
			Description =
				" this beast will link mentally with similar monsters and fight tactically as 1 multi-bodied monster. Should make at least 2 or 3 for full effect.",
		};

		protected override void Init()
		{
			GenerateBeast(PackHunterBounds, this);
			Tags.Add(PackHunterTag);
		}
	}
}