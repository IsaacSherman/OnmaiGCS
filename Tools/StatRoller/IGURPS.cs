namespace StatRoller
{
	public interface IGURPS
	{
		public string Name { get; set; }
		public int ST { get; }
		public int IQ { get; }
		public int DX { get; }
		public int HT { get; }
		public int STModifier { get; set; }
		public int IQModifier { get; set; }
		public int DXModifier { get; set; }
		public int HTModifier { get; set; }
		public int BasicST { get; set; }
		public int BasicIQ { get; set; }
		public int BasicDX { get; set; }
		public int BasicHT { get; set; }
		public int BonusHP { get; set; }
		public int BonusPer { get; set; }
		public int BonusWill { get; set; }
		public int BonusFP { get; set; }
		public int BasicMove { get; set; }
		public float MoveMultiplier { get; set; }
		public float Speed { get; }
		public float BasicSpeed { get; set; }
		public float BonusSpeed { get; set; }
		public DieString SwingDamage { get; }
		public DieString ThrustDamage { get; }
		DamageType DamageType { get; set; }
	}


}