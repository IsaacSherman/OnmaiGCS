namespace StatRoller
{
	/// <summary>
	/// Damage type is handled by GURPS
	/// </summary>
	public interface IDamageDealer
	{
		DamageType DamageType { get; set; }
		int ArmorDivisor { get; set; }
		
	}
}