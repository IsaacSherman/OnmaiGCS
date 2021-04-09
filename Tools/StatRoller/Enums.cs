namespace StatRoller
{
	public enum DamageType
	{
		Crush, Cut, Impale, SmallPierce,Pierce, LargePierce, HugePierce, Corrosive, Burn, Toxic, Electric, Fatigue
	}

	public enum TagType
	{
		StatEnhancing, OffensiveDamageDealing, OffensiveDamageTypeChange, OffensiveState, DefensiveState, DefensiveResistanceAdding, MovementType, Other
	}

	public enum SupportedOperations
	{
		Add, Multiply
	}
}