using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Security.Cryptography;
using System.Windows.Input;
using System.Xml;

namespace StatRoller
{
	public class BeastFactory
	{
		private static readonly Lazy<BeastFactory> _instance = new Lazy<BeastFactory>();
		public static BeastFactory Instance => _instance.Value;

		
		#region Armors

		public static Armor[] BoneArmor =
		{
			new Armor()
			{
				//This is the part that enhances DR
				Description = "is covered in bony protrusions, moving noticeably more slowly under the weight.",
				Bonus = 15,
				TagType = TagType.StatEnhancing,
				Operation = SupportedOperations.Add,
				BonusTarget = "BonusDR",
				Difficulty = 15,
			},
			new Armor()
			{
				//This part decreases the movement speed
				Description = "",
				Bonus = .50f,
				TagType = TagType.MovementType,
				BonusTarget = "BonusMove",
				Operation = SupportedOperations.Multiply,
				Difficulty = -5
			}
			//You might also do some editing of DamageResistances with additional tags
		};

		public static Armor[] ChitinArmor =
		{
			new Armor()
			{
				//This is the part that enhances DR
				Description = "is covered in weirdly shiny black material, it reminds you of the stuff that protects beetles",
				Bonus = 8,
				TagType = TagType.StatEnhancing,
				Operation = SupportedOperations.Add,
				BonusTarget = "BonusDR",
				Difficulty = 8
			},
			new Armor()
			{
				//This part decreases the movement speed
				Description = "",
				Bonus = .90f,
				TagType = TagType.MovementType,
				BonusTarget = "BonusMove",
				Operation = SupportedOperations.Multiply,
				Difficulty = -1
			}
			//You might also do some editing of DamageResistances
		};

		public static Armor[] HideArmor =
		{
			new Armor()
			{
				//This is the part that enhances DR
				Description = "is almost swimming in extra hide- except that anywhere it would chafe has been rubbed smooth by the relentless search for flesh",
				Bonus = 6,
				TagType = TagType.StatEnhancing,
				Operation = SupportedOperations.Add,
				BonusTarget = "BonusDR",
				Difficulty = 6
			},
			//You might also do some editing of DamageResistances
		};

		public static Armor[] ScaleArmor =
		{
			new Armor()
			{
				//This is the part that enhances DR
				Description = "is covered greenish plates, they're similar to the scales of Saurians... but much larger",
				Bonus = 8,
				TagType = TagType.StatEnhancing,
				Operation = SupportedOperations.Add,
				BonusTarget = "BonusDR",
				Difficulty = 8
			},
			new Armor()
			{
				//This part decreases the movement speed
				Description = "",
				Bonus = .90f,
				TagType = TagType.MovementType,
				BonusTarget = "BonusMove",
				Operation = SupportedOperations.Multiply,
				Difficulty = -1
			}
			//You might also do some editing of DamageResistances
		};

		public static Armor[] MucusArmor =
		{
			new Armor()
			{
				//This is the part that enhances DR
				Description = "is always shiny, like a water creature it seems to be bringing its moisture with it",
				Bonus = -3,
				TagType = TagType.StatEnhancing,
				Operation = SupportedOperations.Add,
				BonusTarget = "BonusDR",
				Difficulty = -3
			},
			new Armor()
			{
				//This part decreases the movement speed
				Description = "",
				Bonus = 1.25f,
				TagType = TagType.MovementType,
				Operation = SupportedOperations.Multiply,

				BonusTarget = "BonusMove",
				Difficulty = 3
			}
			//You might also do some editing of DamageResistances
		};

		public static Armor[] FurArmor =
		{
			new Armor()
			{
				//This is the part that enhances DR
				Description = "is covered in a thick layer of fur... it's probably really soft!  Pity you won't know for sure until after it's dead...",
				Bonus = 3,
				TagType = TagType.StatEnhancing,
				Operation = SupportedOperations.Add,
				BonusTarget = "BonusDR",
				Difficulty = 3

			},
			new Armor()
			{
				//This part decreases the movement speed
				Description = "",
				Bonus = .90f,
				TagType = TagType.MovementType,
				BonusTarget = "BonusMove",
				Operation = SupportedOperations.Multiply,
				Difficulty = -1
			}
			//You might also do some editing of DamageResistances
		};

		public static Armor[] FeatherArmor =
		{
			new Armor()
			{
				//This is the part that enhances DR
				Description = "is covered in dense plumage, in a dazzling array of colors",
				Bonus = -1,
				TagType = TagType.StatEnhancing,
				BonusTarget = "BonusDR",
				Operation = SupportedOperations.Add,
				Difficulty = -1
			},
			new Armor()
			{
				//This part decreases the movement speed
				Description = "",
				Bonus = 1.50f,
				TagType = TagType.MovementType,
				BonusTarget = "BonusMove",
				Operation = SupportedOperations.Multiply,
				Difficulty = 5
			}
			//You might also do some editing of DamageResistances
		};

		#endregion

		#region Body Plans

		public static BodyPlan[] BodyPlans;

		#endregion

		#region Milieux

		public static Milieu[] Milieux;

		#endregion




		public IBeast MakeBeast()
		{
			switch (App.rng.Next(0, 5))
			{
				case 0: return MakeFortifier();
				case 1: return MakeAssassin();
				case 2: return MakePackHunter();
				case 3: return MakeBerserker();
				case 4: return MakeTactician();
			}

			return null;
		}

		public IBeast MakeFortifier()
		{
			IBeast beast = new Fortifier();
			return DoTheBasics(beast);
		}
		public IBeast MakeTactician()
		{
			IBeast beast = new Tactician();
			return DoTheBasics(beast);
		}
		public IBeast MakePackHunter()
		{
			IBeast beast = new PackHunter();
			return DoTheBasics(beast);
		}
		public IBeast MakeBerserker()
		{
			IBeast beast = new Berserker();
			return DoTheBasics(beast);
		}
		public IBeast MakeAssassin()
		{
			IBeast beast = new Assassin();
			return DoTheBasics(beast);
		}

		public IBeast DoTheBasics(IBeast beast)
		{
			AddArmor(beast);
			AddMilieu(beast);
			AddBodyPlan(beast);
			AddBite(beast);
			AddNaturalAttacks(beast);
		


			return beast;
		}

		private void AddNaturalAttacks(IBeast beast)
		{
			//First get the bodyplans
			var plan = beast.Tags.FirstOrDefault(x => x is BodyPlan) as BodyPlan;
			AttackTag att = new AttackTag()
			{
				RelativeLevel = App.rng.Next(0, 3),
				AttackName = "Natural Attack",
				Base = AttackBases.Thrust,
				DamageType = DamageType.Crush,
				BaseStat = BaseStat.DX,
				CanParry = false,

			};

			if (plan == null || plan.Limbs <= 0) { return; }
			att.Reach = plan.LimbLength;
			beast.AddTag(att);


		}
		
		private static void AddBite(IBeast beast)
		{
			beast.Name = $"Beast {beast.Difficulty}";
			List<DamageType> biteTypes = new List<DamageType>
			{
				DamageType.Crush,
				DamageType.Cut,
				DamageType.Impale
			};

			AttackTag biteAttackTag = new AttackTag
			{
				RelativeLevel = App.rng.Next(0, 3),
				AttackName = "Bite",
				Base = AttackBases.Thrust,
				DamageType = biteTypes[App.rng.Next(biteTypes.Count)],
				BaseStat = BaseStat.DX,
				CanParry = false,
			};
			beast.AddTag(biteAttackTag);
		}

		private void AddArmor(IBeast beast)
		{
			int index = App.rng.Next(Armor.Armorses.Count);
			Armor[] pick = Armor.Armorses[index];
			foreach (Armor armor in pick)
			{
				beast.AddTag(armor);
			}
		}
		private void AddMilieu(IBeast beast)
		{
			int index = App.rng.Next(Milieux.Length);
			beast.AddTag(Milieux[index]);
			if (Milieux[index].CanTrample)
			{
				beast.AddTag(Trample);
			}
		}
		private void AddBodyPlan(IBeast beast)
		{
			int index = App.rng.Next(BodyPlans.Length);
			beast.AddTag(BodyPlans[index]);
			if (BodyPlans[index].HasRadialSymmetry)
			{
				foreach (Tag tag in BodyPlan.RadialSymmetry)
				{
					beast.AddTag(tag);
				}
			}
		}

		private static AttackTag Trample = new AttackTag
		{
			AttackName = "Trample",
			RelativeLevel = 0,
			Reach = 0,
			BaseStat = BaseStat.DX,
			Base = AttackBases.Thrust,
			CanParry = false,

		};
	}

	public class FactoryTester : ITester
	{
		public bool Test()
		{
			var beast = BeastFactory.Instance.MakePackHunter();
			XmlDocument doc = new XmlDocument();
			doc.AppendChild(doc.CreateElement("Root"));
			beast.SaveXmlRoot(doc);

			doc.Save("factoryTest.xml");
			if (beast is Beast realBeast)
			{
				realBeast.ExportToFantasyGroundsNPCFormat();
			}
			return true;
		}
	}
}