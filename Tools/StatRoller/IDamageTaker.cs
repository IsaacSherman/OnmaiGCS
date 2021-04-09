using System.Collections.Generic;

namespace StatRoller
{
	public interface IDamageTaker
	{
		/// <summary>
		/// This represents a multiplier to each type of damage 
		/// </summary>
		Dictionary<DamageType, float> DamageResistances { get; set; } 
		
	}
}