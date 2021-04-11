using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows.Automation;
using System.Windows.Documents;
using System.Windows.Input;
using System.Xml;

namespace StatRoller
{
	public class Tag : ITag, IXmlStorable, ICloneable
	{
		protected virtual string NodeName()
		{
			return "Tag";
		}

		public static HashSet<AttackTag> AttackTags = new HashSet<AttackTag>();


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
			var el = sink.OwnerDocument.CreateElement(NodeName());
			sink.AppendChild(el);
			TranscribeAttributes(el);
		}

		public void LoadXml(XmlElement source)
		{
			HelperFunctions.ParseAttributes(source, GetType(), this);
		}

		protected virtual void Copy(Tag from, Tag to)
		{
			to.Description = new string(from.Description);
			to.Bonus = from.Bonus;
			to.BonusTarget = new string(from.BonusTarget);
			to.TagType = from.TagType;
			to.Operation = from.Operation;
			to.Difficulty = from.Difficulty;
		}

		public virtual object Clone()
		{
			Tag clone = new Tag();
			Copy(this, clone);
			return clone;
		}
	}

	public class Armor : Tag
	{
		/// <summary>
		/// Armorses are a special case- they have multiple tags associated, so they need to get monitored... differently
		/// </summary>
		public static List<Armor[]> Armorses = new List<Armor[]>
		{
			BeastFactory.BoneArmor,
			BeastFactory.ChitinArmor,
			BeastFactory.HideArmor,
			BeastFactory.ScaleArmor,
			BeastFactory.MucusArmor,
			BeastFactory.FurArmor,
			BeastFactory.FeatherArmor,
		};

		protected override string NodeName()
		{
			return "Armor";
		}
	}

	public class BodyPlan : Tag
	{
		public int Limbs { get; set; }
		public int LimbLength { get; set; }

		protected override void Copy(Tag @from, Tag to)
		{
			if (@from is BodyPlan frm && to is BodyPlan t)
			{
				t.LimbLength = frm.LimbLength;
				t.Limbs = frm.Limbs;
				t.HasRadialSymmetry = frm.HasRadialSymmetry;
			}

			base.Copy(@from, to);
		}

		public override object Clone()
		{
			BodyPlan clone = new BodyPlan();

			this.Copy(this, clone);
			return clone;
		}

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
				BonusTarget = "BonusDR",
				Description = " can't be sneaked up on, flanked, or back-stabbed",
				Difficulty = 10,
				TagType = TagType.DefensiveState,
			},
			new Tag()
			{
				Bonus = 0,
				Description = " doesn't have off-sides, can attack any direction at any time without penalty",
				BonusTarget = "BonusDR",
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
					Limbs = i * 2,
					LimbLength = (int)Math.Floor(App.rng.Next(2, 6) / 2f),
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
					BonusTarget = "BonusHP",
					Difficulty = (int)(Math.Ceiling(i / 4.0f) - 1),
					HasRadialSymmetry = true,
					Limbs = i,
					LimbLength = App.rng.Next(1, 4),
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

			BeastFactory.BodyPlans = bodyPlans.ToArray();

		}

	}

	public class Milieu : Tag
	{

		public bool CanTrample { get; set; } 
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
					BonusTarget = "BonusMove",
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
						m.CanTrample = true;
						break;
					case 1: //Terrestrial
						description =
							" this beast seems to have a good physique... it's probably a fast runner";
						m.Bonus = 2f;
						m.CanTrample = true;

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

			BeastFactory.Milieux = milieux.ToArray();
		}

	}

	public class AttackTag : Tag
	{
		/// <summary>
		/// Modifier to attack type
		/// </summary>
		public int Mod { get; set; }

		/// <summary>
		/// Base damage off thrust, swing, or something else
		/// </summary>
		public AttackBases Base { get; set; }
		/// <summary>
		/// Not used unless Base is Other
		/// </summary>
		public DieString BaseDamage { get; set; }
		public int RelativeLevel { get; set; }
		public BaseStat BaseStat { get; set; }
		public int Reach { get; set; }
		public int Parry { get; set; }
		public string AttackName { get; set; }
		public int AbsoluteLevel { get; set; } = 10;
		public bool CanParry { get; set; }
		public DamageType DamageType { get; set; } = DamageType.Crush;


		protected override void Copy(Tag @from, Tag to)
		{
			if (@from is AttackTag frm && to is AttackTag t)
			{
				if (frm.BaseDamage != null)
				{
					t.BaseDamage = new DieString(frm.BaseDamage.String);
				}

				t.RelativeLevel = frm.RelativeLevel;
				t.BaseStat = frm.BaseStat;
				t.Reach = frm.Reach;
				t.Parry = frm.Parry;
				t.AttackName = frm.AttackName;
				t.AbsoluteLevel = frm.AbsoluteLevel;
				t.CanParry = frm.CanParry;
				t.DamageType = frm.DamageType;
			}

			base.Copy(@from, to);
		}

		public override object Clone()
		{
			AttackTag clone = new AttackTag();

			this.Copy(this, clone);
			return clone;
		}

		public static int ResolveAbsoluteLevel(AttackTag att, Beast beast)
		{
			int basic = att.RelativeLevel;
			switch (att.BaseStat)
			{
				case BaseStat.ST:
					basic += beast.ST;
					break;
				case BaseStat.DX:
					basic += beast.DX;
					break;
				case BaseStat.IQ:
					basic += beast.IQ;
					break;
				case BaseStat.HT:
					basic += beast.HT;
					break;
				case BaseStat.Per:
					basic += beast.Perception;
					break;
				case BaseStat.Will:
					basic += beast.Will;
					break;
				case BaseStat.Other:
					basic = att.AbsoluteLevel;
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}

			if (att.CanParry)
			{
				att.Parry = 3 + basic / 2;
			}
			return basic;
		}

		public static DieString ResolveDamageString(AttackTag att, Beast beast)
		{
			DieString dmg;
			switch (att.Base)
			{
				case AttackBases.Thrust:
					dmg = beast.ThrustDamage.Add(att.Mod);
					break;
				case AttackBases.Swing:
					dmg = beast.SwingDamage.Add(att.Mod);
					break;
				case AttackBases.HalfSTThr:
					dmg = GurpsLookup.Instance.ThrustDamage(beast.ST + att.Mod / 2);
					att.BaseDamage = dmg;
					break;				
				case AttackBases.HalfSTSw:
					dmg = GurpsLookup.Instance.SwingDamage(beast.ST +att.Mod / 2);
					att.BaseDamage = dmg;
					break;
				case AttackBases.Other:
					dmg = att.BaseDamage;
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
			return dmg;
		}

		public static AttackTag BladedStriker = new AttackTag()
		{
			RelativeLevel = App.rng.Next(0, 3),
			DamageType = DamageType.Cut,
			BaseStat = BaseStat.DX,
			Base = AttackBases.Thrust,
			AttackName = "Striker, Bladed",
			CanParry = true,
		};

	}

	public class RangedAttackTag : AttackTag
	{
		public int HalfRange { get; set; }
		public int MaxRange { get; set; }
		public int Accuracy { get; set; }
		public int Ammo { get; set; }
		public int RateOfFire { get; set; }
		public int Shots { get; set; }
		public int ReloadTime { get; set; }
		public int Recoil { get; set; }


		protected override void Copy(Tag @from, Tag to)
		{
			if (@from is RangedAttackTag frm && to is RangedAttackTag t)
			{
				t.HalfRange = frm.HalfRange;
				t.MaxRange = frm.MaxRange;
				t.Accuracy = frm.Accuracy;
				t.Ammo = frm.Ammo;
				t.RateOfFire = frm.RateOfFire;
				t.Shots = frm.Shots;
				t.ReloadTime = frm.ReloadTime;
				t.Recoil = frm.Recoil;
			}
			base.Copy(@from, to);
		}

		public override object Clone()
		{
			RangedAttackTag clone = new RangedAttackTag();
			Copy(this, clone);


			return clone;
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
				BonusTarget = "BonusMove",
				Operation = SupportedOperations.Multiply,
				TagType = TagType.StatEnhancing,
				Description = "Testy stuff",
			};
			t.SaveXmlRoot(doc);
			foreach (Armor armor in BeastFactory.BoneArmor)
			{
				armor.SaveXmlRoot(doc);
			}

			RangedAttackTag tag = new RangedAttackTag
			{
				AbsoluteLevel = 12, Accuracy = 13, Ammo = 1, AttackName = "Pewpewpew", BaseStat = BaseStat.DX,
				Difficulty = 13
			};
			var clonetest = tag.Clone() as RangedAttackTag;

			if (!(tag.AbsoluteLevel == clonetest?.AbsoluteLevel &&
			    tag.Accuracy == clonetest.Accuracy &&
			    tag.Ammo == clonetest.Ammo &&
			    tag.AttackName == clonetest.AttackName &&
			    tag.BaseStat == clonetest.BaseStat &&
			    tag.Difficulty == clonetest.Difficulty))
			{
				return false;
			}
		
				clonetest.AttackName = "KEYKEYKEY";
				if (tag.AttackName == clonetest.AttackName) {return false;}
			


			BodyPlan b = new BodyPlan();
			b = BeastFactory.BodyPlans[12];
			b.SaveXmlRoot(doc);

			Milieu m = new Milieu();
			m = BeastFactory.Milieux[3];
			m.SaveXmlRoot(doc);
			doc.Save("tagTest.xml");
			t.LoadXml(doc.DocumentElement.LastChild as XmlElement);

			return true;
		}
	}
}
