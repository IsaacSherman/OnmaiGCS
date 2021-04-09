using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows.Documents;
using System.Windows.Input;
using System.Xml;

namespace StatRoller
{
	public class Tag : ITag, IXmlStorable
	{
		protected virtual string NodeName()
		{
			return "Tag";
		}

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
				BonusTarget = "DR",
				Difficulty = 15,
			},
			new Armor()
			{
				//This part decreases the movement speed
				Description = "",
				Bonus = .50f,
				TagType = TagType.MovementType,
				BonusTarget = "BasicMove",
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
				BonusTarget = "DR",
				Difficulty = 8
			},
			new Armor()
			{
				//This part decreases the movement speed
				Description = "",
				Bonus = .90f,
				TagType = TagType.MovementType,
				BonusTarget = "BasicMove",
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
				BonusTarget = "DR",
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
				BonusTarget = "DR",
				Difficulty = 8
			},
			new Armor()
			{
				//This part decreases the movement speed
				Description = "",
				Bonus = .90f,
				TagType = TagType.MovementType,
				BonusTarget = "BasicMove",
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
				BonusTarget = "DR",
				Difficulty = -3
			},
			new Armor()
			{
				//This part decreases the movement speed
				Description = "",
				Bonus = 1.25f,
				TagType = TagType.MovementType,
				Operation = SupportedOperations.Multiply,

				BonusTarget = "BasicMove",
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
				BonusTarget = "DR",
				Difficulty = 3

			},
			new Armor()
			{
				//This part decreases the movement speed
				Description = "",
				Bonus = .90f,
				TagType = TagType.MovementType,
				BonusTarget = "BasicMove",
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
				BonusTarget = "DR",
				Operation = SupportedOperations.Add,
				Difficulty = -1
			},
			new Armor()
			{
				//This part decreases the movement speed
				Description = "",
				Bonus = 1.50f,
				TagType = TagType.MovementType,
				BonusTarget = "BasicMove",
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


		public string Description { get; set; }
		public float Bonus { get; set; }
		public string BonusTarget { get; set; }
		public TagType TagType { get; set; } = TagType.StatEnhancing;
		public SupportedOperations Operation { get; set; } = SupportedOperations.Add;
		public int Difficulty { get; set; }
		public void SaveXmlRoot(XmlDocument sink)
		{
			var el = sink.CreateElement(NodeName());
			sink.DocumentElement.AppendChild(el);
			TranscribeAttributes(el);
		}

		private void TranscribeAttributes(XmlElement el)
		{
			el.SetAttribute("Description", Description);
			el.SetAttribute("Bonus", $"{Bonus:F3}");
			el.SetAttribute("BonusTarget", BonusTarget);
			el.SetAttribute("TagType", TagType.ToString());
			el.SetAttribute("Operation", Operation.ToString());
		}

		public void SaveXml(XmlElement sink)
		{
			var el =sink.OwnerDocument.CreateElement(NodeName());
			sink.AppendChild(el);
			TranscribeAttributes(el);
		}

		public void LoadXml(XmlElement source)
		{
			HelperFunctions.ParseAttributes(source, GetType(), this);
		}
	}

	public class Armor : Tag
	{
		protected override string NodeName()
		{
			return "Armor";
		}
	}

	public class BodyPlan : Tag
	{
		protected override string NodeName()
		{
			return "BodyPlan";
		}


		public bool HasRadialSymmetry { get; set; } = false;

		public static Tag[] RadialSymmetry =
		{
			new Tag()
			{
				Bonus = 0,
				BonusTarget = "DR",
				Description = " can't be sneaked up on, flanked, or back-stabbed",
				Difficulty = 10,
				TagType = TagType.DefensiveState,
			},
			new Tag()
			{
				Bonus = 0,
				Description = " doesn't have off-sides, can attack any direction at any time without penalty",
				BonusTarget = "DR",
				Difficulty = 5,
				TagType = TagType.OffensiveState,
			},
		};

		static BodyPlan()
		{

			List<BodyPlan> bodyPlans = new List<BodyPlan>();
			for (int i = 0; i < 7; ++i)
			{
				BodyPlan plan = new BodyPlan()
				{
					Bonus = 1,
					TagType = TagType.Other,
					BonusTarget = "MoveMultiplier",
					Difficulty = 0,
				};
				string description = "";
				switch (i)
				{
					case 0:
						description = "with no legs, this creature seems to be one long mass of muscle... or stomach.";
						break;
					case 1:
						description =
							"something is maddeningly familiar about this one... or perhaps it is simply that with two legs, it moves in a twisted mockery of your own motion.";
						break;
					case 2:
						description =
							"this beast feels right, somehow.  Perhaps it is your imagination, but something seems to resonate, as though 4 is the proper number of limbs a creature should have.";
						plan.Bonus = 2;

						break;
					case 3:
						description =
							"six limbs leave you unsettled, or would have, once, had you not seen so many like it.  You can almost remember what it was like to be horrified by the very existence of a beast like this...";
						plan.Bonus = 2;
						break;
					case 4:
						description =
							"eight limbs call to mind things that scuttle and scurry on the periphery of your vision.  They strike in the night, from concealment.  At least this beast will not do you that discourtesy.";
						plan.Bonus = 2;
						break;
					case 5:
						description =
							"10 limbs is... too many.  Even motionless, they evoke images of squeezing crowding squirming wriggling dripping in the dark...  Somehow they are always moving closer, even when you think you've finished them off...";
						plan.Bonus = 2;
						break;

					case 6:
						description =
							"cut off a limb and 2 grow to replace it- an endless forest of limbs, seeking twisting ripping... they won't stop.  They'll never stop. They'll rip you open and drink deeply of you, and immediately forget you, nothing more than an hors d'oeuvre.  Even if you kill them those filthy legs will be scratching around in your mind for years to come.";
						plan.Bonus = 3;
						break;
				}

				plan.Description = description;
				bodyPlans.Add(plan);
			}

			for (int i = 3; i < 14; ++i)
			{
				BodyPlan plan = new BodyPlan()
				{
					Bonus = (int)(Math.Ceiling(i / 4.0f) - 1),
					TagType = TagType.Other,
					BonusTarget = "HP",
					Difficulty = (int)(Math.Ceiling(i / 4.0f) - 1),
					HasRadialSymmetry = true,
				};
				string description = "";
				switch (i)
				{
					case 3:
					case 4:
					case 5:
					case 6:
						description =
							$"with {i} limbs arrayed evenly around its body, everything about this creature's movement seems preternatural.  The limbs move chaotically, with no pattern you can deduce save for one: it knows where you are, and it's coming.";
						break;
					case 7:
					case 8:
					case 9:
					case 10:
						description =
							$" {i} limbs around this creature evenly spaced makes it seem at first like a monstrous spider, but would that such a comforting lie could be true.  This beast is always watching you, even in retreat, and quick to punish those who seek to take advantage of an opening.";
						break;
					case 11:
					case 12:
					case 13:
					case 14:
						description =
							$"Flexible limbs seem to jut out everywhere... {i - 2}... {i}.  The writhing mass of them makes your skin squirm, as though they are already caressing your neck, tearing your flesh away in implausibly neat strips.";
						break;
				}

				plan.Description = description;
				bodyPlans.Add(plan);
			}
			BodyPlans = bodyPlans.ToArray();

		}
	}

	public class Milieu : Tag
	{
		protected override string NodeName()
		{
			return "Milieu";
		}

		static Milieu()
		{
			List<Milieu> milieux = new List<Milieu>(5);
			for (int i = 0; i < 5; ++i)
			{
				Milieu m = new Milieu()
				{
					BonusTarget = "BasicMove",
					TagType = TagType.MovementType,
					Operation = SupportedOperations.Multiply,
					Bonus = 1f,

				};
				string description = "";
				switch (i)
				{
					case 0: //Underground
						description =
							" the creature is covered in debris, it looks like it can burrow as easily as it can walk";
						break;
					case 1: //Terrestrial
						description =
							" this beast seems to have a good physique... it's probably a fast runner";
						m.Bonus = 2f;
						break;
					case 2: //Liquid
						description =
							" this monster is probably in its element in water.";
						m.Bonus = 2f;

						break;
					case 3: //Flier- Hover
						description =
							" this creature is hovering in place high above";
						m.Bonus = 2f;
						m.Difficulty = 10;
						break;
					case 4: //Flier-Fighter Jet
						description =
							" this beast arcs and wheels, never staying in place for a moment";
						m.Difficulty = 15;
						m.Bonus = 3f;
						break;

				}

				m.Description = description;
				milieux.Add(m);
			}

			Milieux = milieux.ToArray();
		}

	}
	/// <summary>
	/// A Tag is a modifier which increases a "stat", adds an attack, adds a defense, alters damage resistance, or does something else (adds burrowing, for instance)
	/// </summary>
	public interface ITag
	{
		string Description { get; set; }
		/// <summary>
		/// This is the magnitude of the bonus
		/// </summary>
		float Bonus { get; set; }
		TagType TagType { get; set; }
		String BonusTarget { get; set; }
		int Difficulty { get; set; }

	}

	public class TagTester : ITester
	{

		public bool Test()
		{
			XmlDocument doc = new XmlDocument();
			doc.AppendChild(doc.CreateElement("root"));
			Tag t = new Tag()
			{
				Bonus = 1.4f,
				BonusTarget = "BasicMove",
				Operation = SupportedOperations.Multiply,
				TagType = TagType.StatEnhancing,
				Description = "Testy stuff",
			};
			t.SaveXmlRoot(doc);
			foreach (Armor armor in Tag.BoneArmor)
			{
				armor.SaveXmlRoot(doc);
			}

			BodyPlan b = new BodyPlan();
			b = Tag.BodyPlans[12];
			b.SaveXmlRoot(doc);

			Milieu m = new Milieu();
			m = Tag.Milieux[3];
			m.SaveXmlRoot(doc);
			doc.Save("tagTest.xml");
			t.LoadXml(doc.DocumentElement.LastChild as XmlElement);

			return true;
		}
	}
}
