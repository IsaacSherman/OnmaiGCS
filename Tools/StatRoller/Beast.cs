using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Schema;

namespace StatRoller
{
	public abstract class Beast : IBeast
	{
		protected static string typeString = "Beast";

		public HashSet<Tag> Tags { get; set; } = new HashSet<Tag>();
		public HashSet<AttackTag> AttackTags = new HashSet<AttackTag>();
		public bool AddTag(Tag tag)
		{
			if (tag is RangedAttackTag rat)
			{
				AttackTags.Add((RangedAttackTag)rat.Clone() );
			}
			else if (tag is AttackTag attackTag)
			{

				AttackTags.Add((AttackTag)attackTag.Clone());
			}

			Tag clone = tag.Clone() as Tag;

			return clone != null && Tags.Add(clone);

		}

		protected int _difficulty = -1;

		public int Difficulty
		{
			get
			{
				if (_difficulty == -1) CalculateDifficulty();
				return _difficulty;
			}
			set => _difficulty = value;
		}

		public string Name { get; set; }
		public int ST => STModifier + BasicST;
		public int IQ => IQModifier + BasicIQ;
		public int DX => DXModifier + BasicDX;
		public int HT => HTModifier + BasicHT;

		public int HP => BasicHP + BonusHP;
		public int Perception => BasicPer + BonusPer;
		public int Will => BasicWill + BonusWill;
		public int FP => BasicFP + BonusFP;
		public int DR => BasicDR + BonusDR;
		public int Move => (int)(MoveMultiplier * (BasicMove + BonusMove));
		public float Speed => BasicSpeed + BonusSpeed;

		public int STModifier { get; set; }
		public int IQModifier { get; set; }
		public int DXModifier { get; set; }
		public int HTModifier { get; set; }
		public int BasicST { get; set; }
		public int BasicIQ { get; set; }
		public int BasicDX { get; set; }
		public int BasicHT { get; set; }
		public int BasicDR { get; set; }
		public int BonusDR { get; set; }
		public int BonusHP { get; set; }
		public int BonusPer { get; set; }
		public int BonusWill { get; set; }
		public int BonusFP { get; set; }
		public int BasicHP { get; set; }
		public int BasicPer { get; set; }
		public int BasicWill { get; set; }
		public int BasicFP { get; set; }
		public float MoveMultiplier { get; set; } = 1;
		public DieString ThrustDamage => GurpsLookup.Instance.ThrustDamage(ST);
		public DieString SwingDamage => GurpsLookup.Instance.SwingDamage(ST);

		public string Description { get; set; }

		public int BasicMove { get; set; }
		public int BonusMove { get; set; }
		public float BasicSpeed { get; set; }
		public float BonusSpeed { get; set; }

		public DamageType DamageType { get; set; }
		public int ArmorDivisor { get; set; }
		public Dictionary<DamageType, float> DamageResistances { get; set; }

		public XmlSchema GetSchema()
		{
			return null;
		}

		public static readonly DamageType[] AllDamageTypes = new DamageType[]
		{
			DamageType.Crush,
			DamageType.Cut,
			DamageType.Impale,
			DamageType.SmallPierce,
			DamageType.Pierce,
			DamageType.LargePierce,
			DamageType.HugePierce,
			DamageType.Corrosive,
			DamageType.Burn,
			DamageType.Toxic,
			DamageType.Electric,
			DamageType.Fatigue
		};

		protected virtual void Init()
		{
			BasicSpeed = (DX + HT) / 4.0f;
			BasicMove = (int)Math.Floor(BasicSpeed);
			ArmorDivisor = 0;
		}

		private void AssignBasicResistances()
		{
			DamageResistances = new Dictionary<DamageType, float>();
			foreach (DamageType type in AllDamageTypes)
			{
				DamageResistances.Add(type, 1f);
			}

			Init();
		}

		protected Beast()
		{
			AssignBasicResistances();
		}

		public void SaveXml(XmlElement sink)
		{
			throw new NotImplementedException();
		}

		public void ExportToFantasyGroundsNPCFormat()
		{
			ApplyTags();
			SaveFileDialog dlg = new SaveFileDialog
			{
				AddExtension = true,
				CheckPathExists = true,
				Filter = "xml files|*.xml",
				Title = "Save as FG NPC",
				DefaultExt = ".xml",
			};
			if (dlg.ShowDialog() ?? false)
			{
				XmlDocument doc = new XmlDocument();
				doc.CreateXmlDeclaration("1.0", "iso-8859-1", null);

				var RootElement = CreateElementAndAdd(doc, "root");
				RootElement.SetAttribute("version", "2.8");
				var NPCElement = CreateElementAndAdd(doc, "npc", RootElement);

				string name = String.IsNullOrEmpty(Name) ? dlg.FileName.Substring(0, dlg.FileName.Length - 4) : Name;
				#region Notes And Points

				var nameEl = CreateElementAndAdd(doc, "name", NPCElement);
				nameEl.InnerText = name;
				nameEl.SetAttribute("type", "string");

				var notesEl = CreateElementAndAdd(doc, "notes", NPCElement);
				//Not used right now, maybe soon!
				// ReSharper disable once StringLiteralTypo
				DoFormattedText(doc, notesEl);

				var ptsEl = CreateElementAndAdd(doc, "pts", NPCElement);
				ptsEl.SetAttribute("type", "number");
				ptsEl.InnerText = "300";
				#endregion
				#region Traits

				var traitsEl = CreateElementAndAdd(doc, "traits", NPCElement);

				var smEl = CreateElementAndAdd(doc, "sizemodifier", traitsEl);
				smEl.SetAttribute("type", "string");
				smEl.InnerText = "+3";

				var rmEl = CreateElementAndAdd(doc, "reactionmodifier", traitsEl);
				rmEl.SetAttribute("type", "string");

				var descEl = CreateElementAndAdd(doc, "description", traitsEl);
				descEl.SetAttribute("type", "string");
				StringBuilder sb = new StringBuilder(Description);
				sb.Replace("\r\n", "(0)\r");
				descEl.InnerText = sb.ToString();


				//Close traits

				#endregion
				#region Attributes
				var attEl = CreateElementAndAdd(doc, "attributes", NPCElement);

				var strEl = CreateElementAndAdd(doc, "strength", attEl);
				strEl.SetAttribute("type", "number");
				strEl.InnerText = $"{ST}";

				var dexEl = CreateElementAndAdd(doc, "dexterity", attEl);
				dexEl.SetAttribute("type", "number");
				dexEl.InnerText = $"{DX}";
				var IQEl = CreateElementAndAdd(doc, "intelligence", attEl);
				IQEl.SetAttribute("type", "number");
				IQEl.InnerText = $"{IQ}";
				var HTEl = CreateElementAndAdd(doc, "health", attEl);
				HTEl.SetAttribute("type", "number");
				HTEl.InnerText = $"{HT}";

				var HPEl = CreateElementAndAdd(doc, "hitpoints", attEl);
				HPEl.SetAttribute("type", "number");
				HPEl.InnerText = $"{HP}";

				var WillEl = CreateElementAndAdd(doc, "will", attEl);
				WillEl.SetAttribute("type", "number");
				WillEl.InnerText = $"{Will}";

				var PerEl = CreateElementAndAdd(doc, "perception", attEl);
				PerEl.SetAttribute("type", "number");
				PerEl.InnerText = $"{Perception}";

				var fpEl = CreateElementAndAdd(doc, "fatiguepoints", attEl);
				fpEl.SetAttribute("type", "number");
				fpEl.InnerText = $"{FP}";

				var blEl = CreateElementAndAdd(doc, "basiclift", attEl);
				blEl.SetAttribute("type", "string");
				blEl.InnerText = $"{ST * ST / 5}";

				var thrEl = CreateElementAndAdd(doc, "thrust", attEl);
				thrEl.SetAttribute("type", "string");
				thrEl.InnerText = $"{ThrustDamage}";

				var swEl = CreateElementAndAdd(doc, "swing", attEl);
				swEl.SetAttribute("type", "string");
				swEl.InnerText = $"{SwingDamage}";

				var bsEl = CreateElementAndAdd(doc, "basicspeed", attEl);
				bsEl.SetAttribute("type", "string");
				bsEl.InnerText = $"{Speed:F2}";

				var bmEl = CreateElementAndAdd(doc, "basicmove", attEl);
				bmEl.SetAttribute("type", "string");
				bmEl.InnerText = $"{Move}";

				var mvEl = CreateElementAndAdd(doc, "move", attEl);
				mvEl.SetAttribute("type", "string");
				mvEl.InnerText = $"{MoveMultiplier * Move}";
				#endregion
				#region Combat

				var combatEl = CreateElementAndAdd(doc, "combat", NPCElement);
				var dodgeEl = CreateElementAndAdd(doc, "dodge", combatEl);
				dodgeEl.SetAttribute("type", "number");
				dodgeEl.InnerText = $"{3 + (int)Math.Floor(BasicSpeed)}";

				var blockEl = CreateElementAndAdd(doc, "block", combatEl);
				blockEl.SetAttribute("type", "number");
				blockEl.InnerText = $"-";
				var parryEl = CreateElementAndAdd(doc, "parry", combatEl);
				parryEl.SetAttribute("type", "number");
				parryEl.InnerText = $"-";

				var drEl = CreateElementAndAdd(doc, "dr", combatEl);
				drEl.SetAttribute("type", "string");
				drEl.InnerText = $"{DR}";

				var meleeCombatList = CreateElementAndAdd(doc, "meleecombatlist", combatEl);
				var rngCmbEl = CreateElementAndAdd(doc, "rangedcombatlist", combatEl);

				int count = 1;
				foreach (AttackTag att in AttackTags)
				{
					if (att is RangedAttackTag rangedAttackTag)
					{
						rngCmbEl.AppendChild(ParseRangedAttackElement(doc, doc.CreateElement($"id-1"),
							rangedAttackTag, this));
					}
					else
					{
						var tempNode = CreateElementAndAdd(doc, $"id-{count++}", meleeCombatList);
						var mmlEl = CreateElementAndAdd(doc, "meleemodelist", tempNode);

						var attNameEl = CreateElementAndAdd(doc, "name", tempNode);
						attNameEl.SetAttribute("type", "string");
						attNameEl.InnerText = att.AttackName;

						var attStEl = CreateElementAndAdd(doc, "st", tempNode);
						attStEl.SetAttribute("type", "string");
						attStEl.InnerText = "";

						var attWtEl = CreateElementAndAdd(doc, "weight", tempNode);
						attWtEl.SetAttribute("type", "string");
						attWtEl.InnerText = "";

						var attTlEl = CreateElementAndAdd(doc, "tl", tempNode);
						attTlEl.SetAttribute("type", "string");
						attTlEl.InnerText = "";

						var attCostEl = CreateElementAndAdd(doc, "cost", tempNode);
						attCostEl.SetAttribute("type", "string");
						attCostEl.InnerText = "";

						var atttextEl = CreateElementAndAdd(doc, "text", tempNode);
						DoFormattedText(doc, atttextEl);

						mmlEl.AppendChild(ParseAttackElement(doc, doc.CreateElement($"id-1"),
							att, this));
					}

				}
				#endregion
				doc.Save(dlg.OpenFile());
			}
		}

		private XmlElement ParseRangedAttackElement(XmlDocument doc, XmlElement parent, RangedAttackTag att, Beast beast)
		{
			var attNameEl = CreateElementAndAdd(doc, "name", parent);
			attNameEl.SetAttribute("type", "string");
			attNameEl.InnerText = att.AttackName;

			var attStEl = CreateElementAndAdd(doc, "st", parent);
			attStEl.SetAttribute("type", "string");
			attStEl.InnerText = "";
			var attWtEl = CreateElementAndAdd(doc, "bulk", parent);
			attWtEl.SetAttribute("type", "number");
			attWtEl.InnerText = "0";


			var attLCEl = CreateElementAndAdd(doc, "LC", parent);
			attLCEl.SetAttribute("type", "string");
			attLCEl.InnerText = "0";

			var atttextEl =
				CreateElementAndAdd(doc, "text", parent);
			DoFormattedText(doc, atttextEl);



			var rngml = CreateElementAndAdd(doc, "rangedmodelist", parent);
			var id1el = CreateElementAndAdd(doc, "id-1", rngml);




			var id1elname =
				CreateElementAndAdd(doc, "name", id1el);
			id1elname.SetAttribute("type", "string");
			id1elname.InnerText = att.AttackName;
			var level =
				CreateElementAndAdd(doc, "level", id1el);
			level.SetAttribute("type", "number");
			level.InnerText = $"{AttackTag.ResolveAbsoluteLevel(att, beast)}";
			var attDmgEl =
				CreateElementAndAdd(doc, "damage", id1el);
			attDmgEl.SetAttribute("type", "string");
			attDmgEl.InnerText = $"{AttackTag.ResolveDamageString(att, beast)}|{att.DamageType}";
			var attCostEl =
				CreateElementAndAdd(doc, "acc", id1el);
			attCostEl.SetAttribute("type", "number");
			attCostEl.InnerText = $"{att.Accuracy}";
			var rangeEl =
				CreateElementAndAdd(doc, "range", id1el);
			rangeEl.SetAttribute("type", "string");
			rangeEl.InnerText = $"{att.HalfRange}/{att.MaxRange}";

			var rof = CreateElementAndAdd(doc, "rof", id1el);
			rof.SetAttribute("type", "string");
			rof.InnerText = $"{att.RateOfFire}";

			var shots =
				CreateElementAndAdd(doc, "shots", id1el);
			shots.SetAttribute("type", "string");
			shots.InnerText = $"{att.Shots}";



			var recoil =
				CreateElementAndAdd(doc, "rcl", id1el);
			recoil.SetAttribute("type", "number");
			recoil.InnerText = $"{att.Recoil}";


			return parent;
		}

		private static void DoFormattedText(XmlDocument doc, XmlElement elem)
		{
			elem.SetAttribute("type", "formattedtext");
			var temp = doc.CreateElement("p");
			temp.InnerText = " ";
			elem.AppendChild(temp);
		}

		public static XmlElement ParseAttackElement(XmlDocument doc, XmlElement parent, AttackTag att, Beast beast)
		{
			if (att is RangedAttackTag)
			{
				return null;
			}
			var attName1El =
				CreateElementAndAdd(doc, "name", parent);
			attName1El.SetAttribute("type", "string");
			attName1El.InnerText = att.AttackName;

			var attLevelEl =
				CreateElementAndAdd(doc, "level", parent);
			attLevelEl.SetAttribute("type", "number");
			attLevelEl.InnerText = $"{AttackTag.ResolveAbsoluteLevel(att, beast)}";

			var attDamage1El = CreateElementAndAdd(doc, "damage", parent);
			attDamage1El.SetAttribute("type", "string");
			attDamage1El.InnerText = $"{AttackTag.ResolveDamageString(att, beast)}|{att.DamageType}";

			var attReach1El = CreateElementAndAdd(doc, "reach", parent);
			attReach1El.SetAttribute("type", "string");
			attReach1El.InnerText = $"{ att.Reach}";

			var attParry1El = CreateElementAndAdd(doc, "parry", parent);
			attParry1El.SetAttribute("type", "string");
			attParry1El.InnerText = $"{(att.CanParry ? $"{att.Parry}" : "No")}";
			return parent;
		}


		public static XmlElement CreateElementAndAdd(XmlDocument doc, string name, XmlElement parent = null)
		{
			XmlElement target = doc.CreateElement(name);
			if (parent == null)
			{
				doc.AppendChild(target);
			}
			else
			{
				parent.AppendChild(target);
			}

			return target;
		}

		public void LoadXml(XmlElement source)
		{
			Init();

			HelperFunctions.ParseAttributes(source, typeof(Beast), this);

			foreach (var obj in source.ChildNodes)
			{
				XmlElement node = obj as XmlElement;

				switch (node.Name)
				{
					case "DamageResistances":
						ParseDamageResistances(source);
						break;
					case "Tags":
						ParseTags(source);
						break;
				}
			}
		}

		private void ParseTags(XmlElement source)
		{
			XmlNodeList nodes = source.ChildNodes;
			foreach (XmlElement node in nodes)
			{
				Tag newTag;
				switch (node.Name)
				{
					case "Tag":
					default:
						newTag = new Tag();
						break;
					case "Milieu":
						newTag = new Milieu();
						break;
					case "Armor":
						newTag = new Armor();
						break;
					case "BodyPlan":
						newTag = new BodyPlan();
						break;
				}

				newTag.LoadXml(node);
			}


		}


		private void ApplyTags()
		{
			StringBuilder description = new StringBuilder();
			foreach (var tag in Tags)
			{
				description.Append(tag.Description + "\r\n");
				if (string.IsNullOrEmpty(tag.BonusTarget))
				{
					continue;
				}

				PropertyInfo property = GetType().GetProperty(tag.BonusTarget);

				object value = null;
				if (property.GetMethod.ReturnType == typeof(int))
				{
					var v = (int)property.GetValue(this);
					if (tag.Operation == SupportedOperations.Add)
					{
						v += (int)tag.Bonus;
					}
					else
					{
						v = (int)(v * tag.Bonus);
					}
					value = v;
				}
				else if (property.GetMethod.ReturnType == typeof(int))
				{
					float f = (int)property.GetValue(property);
					if (tag.Operation == SupportedOperations.Add)
					{
						f += tag.Bonus;
					}
					else
					{
						f *= tag.Bonus;
					}
					value = f;
				}

				if (value != null)
				{
					property.SetValue(this, value);
				}
			}
			Description = description.ToString();
		}


		public void SaveXmlRoot(XmlDocument doc)
		{
			var sink = doc.CreateElement(typeString);
			doc.DocumentElement.AppendChild(sink);
			//The theory is all the tags will be responsible for all the bonuses.  Non-tag bonuses should be added to Basic stats.
			SaveToSink(sink, "Name", $"{Name}");
			SaveToSink(sink, "BasicST", $"{BasicST}");
			SaveToSink(sink, "BasicIQ", $"{BasicIQ}");
			SaveToSink(sink, "BasicDX", $"{BasicDX}");
			SaveToSink(sink, "BasicHT", $"{BasicHT}");
			SaveToSink(sink, "BasicDR", $"{BasicDR}");
			SaveToSink(sink, "BasicPer", $"{ BasicPer}");
			SaveToSink(sink, "BasicWill", $"{BasicWill}");
			SaveToSink(sink, "BasicFP", $"{  BasicFP}");
			SaveToSink(sink, "BasicMove", $"{BasicMove}");
			SaveToSink(sink, "BasicSpeed", $"{BasicSpeed}");
			SaveToSink(sink, "BonusMove", $" {BonusMove}");
			SaveToSink(sink, "BonusSpeed", $"{BonusSpeed}");
			var damageResistances = doc.CreateElement("DamageResistances");
			sink.AppendChild(damageResistances);

			foreach (DamageType type in DamageResistances.Keys)
			{
				var node = doc.CreateElement("DamageResistance");
				node.SetAttribute("Type", type.ToString());
				node.SetAttribute("Amount", $"{DamageResistances[type]:F2}");
				damageResistances.AppendChild(node);
			}

			if (Tags.Count > 0)
			{
				int f = 5;
				var tagElement = doc.CreateElement("Tags");
				sink.AppendChild(tagElement);

				foreach (Tag tag in Tags)
				{
					tag.SaveXml(tagElement);
				}
			}


		}

		protected int CalculateDifficulty()
		{

			ApplyTags();

			int diff = ST + HP + HT + FP;
			diff += IQ * IQ;
			diff += (DX - 10) * 5;
			diff += Tags.Sum(t => t.Difficulty);
			diff += (int)Math.Ceiling(Will / 4.0f);
			diff += (int)Math.Ceiling(Perception / 4.0f);
			diff += BasicMove / 5;
			diff += (int)(BasicSpeed - 5) * 4;
			Difficulty = diff;
			return diff;
		}

		private void SaveToSink(XmlElement sink, string stat, string value)
		{
			sink.Attributes.Append(sink.OwnerDocument.CreateAttribute(stat));
			sink.Attributes[stat].Value = value;
		}


		protected static void GenerateBeast(Bounds bounds, Beast beast)
		{
			beast.BasicST = App.rng.Next(bounds.MinST, bounds.MaxST + 1);
			beast.BasicDX = App.rng.Next(bounds.MinDX, bounds.MaxDX + 1);
			beast.BasicIQ = App.rng.Next(bounds.MinIQ, bounds.MaxIQ + 1);
			beast.BasicHT = App.rng.Next(bounds.MinHT, bounds.MaxHT + 1);
			beast.BasicDR = App.rng.Next(bounds.MinDR, bounds.MaxDR + 1);
			beast.BasicPer = beast.BasicIQ + App.rng.Next(bounds.MinPer, bounds.MaxPer + 1);
			beast.BasicWill = beast.BasicIQ + App.rng.Next(bounds.MinWill, bounds.MaxWill + 1);
			beast.BasicSpeed = (beast.BasicDX + beast.BasicHT + App.rng.Next(bounds.MinSpeed, bounds.MaxSpeed + 1)) / 4f;
			beast.BasicMove = (int)Math.Floor(beast.BasicSpeed + App.rng.Next(bounds.MinMove, bounds.MaxMove + 1));

			beast.BasicHP = App.rng.Next(bounds.MinHP, bounds.MaxHP + 1);
			beast.BasicFP = App.rng.Next(bounds.MinFP, bounds.MaxFP + 1);

		}

		private void ParseDamageResistances(XmlElement source)
		{
			var list = source.GetElementsByTagName("DamageResistance");
			foreach (object o in list)
			{
				XmlElement node = o as XmlElement;
				XmlAttribute damageTypeAttribute = node.GetAttributeNode("Type");
				XmlAttribute amountAttribute = node.GetAttributeNode("Amount");
				if (Enum.TryParse(damageTypeAttribute.Value, out DamageType t))
				{
					float amount = float.Parse(amountAttribute.Value);
				}

			}
		}
	}


	public class BeastTester : ITester
	{
		public class BeastTest : Beast
		{
			public BeastTest()
			{
				Init();
			}
		}

		private BeastTest bob = new BeastTest()
		{
			Name = "Bob",
			BasicST = 12,
			BasicIQ = 12,
			BasicDX = 13,
			BasicHT = 14,
			BasicDR = 10,
			BasicMove = 7,
			BasicSpeed = 12
		};
		private BeastTest frank = new BeastTest()
		{
			Name = "Frank",
			BasicST = 12,
			BasicIQ = 12,
			BasicDX = 13,
			BasicHT = 14,
			BasicDR = 10,
			BasicMove = 7,
			BasicSpeed = 12
		};
		private BeastTest sam = new BeastTest()
		{
			Name = "Same",
			BasicST = 11,
			BasicIQ = 12,
			BasicDX = 13,
			BasicHT = 14,
			BasicDR = 10,
			BasicMove = 7,
			BasicSpeed = 12
		};

		public bool Test()
		{


			XmlDocument doc = new XmlDocument();
			foreach (Armor armor in BeastFactory.BoneArmor)
			{
				bob.AddTag(armor);
			}
			ITag temp = new BodyPlan();
			temp = new Armor();
			temp = new Milieu();
			Tactician blatt = new Tactician()
			{

			};
			blatt.AddTag(BeastFactory.BodyPlans[5]);
			blatt.AddTag(BeastFactory.Milieux[3]);
			blatt.Name = "Franklefort";
			doc.AppendChild(doc.CreateElement("root"));
			foreach (var b in new[]
				{
					blatt
				}
			)

			{ b.SaveXmlRoot(doc); }


			doc.Save("test.xml");
			sam = new BeastTest();
			AttackTag t = new AttackTag
			{
				AbsoluteLevel = 12,
				AttackName = "chomk",
				Base = AttackBases.Swing,
				Mod = 6,
				BaseStat = BaseStat.DX,
				Reach = 3,
				RelativeLevel = 2,
				CanParry = true,
				Parry = 9
			};
			RangedAttackTag rt = new RangedAttackTag
			{
				AbsoluteLevel = 15,
				Ammo = 12,
				Accuracy = 3,
				AttackName = "snikk",
				Base = AttackBases.Thrust,
				CanParry = false,
				HalfRange = 120,
				MaxRange = 300,
				RateOfFire = 3,
				Mod = 4,
				BaseStat = BaseStat.DX,
				Description = "Bubble bomb",
				Recoil = 2,
			};

			blatt.AddTag(t);
			blatt.AddTag(rt);
			sam.LoadXml(doc.DocumentElement.FirstChild as XmlElement);
			blatt.ExportToFantasyGroundsNPCFormat();
			return true;
		}
	}
}
