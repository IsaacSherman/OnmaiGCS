using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;

namespace StatRoller
{
	public abstract class Beast : IBeast
	{
		protected static string typeString = "Beast";

		public HashSet<Tag> Tags { get; set; } = new HashSet<Tag>();
		public bool AddTag(Tag tag)
		{
			return tag != null && Tags.Add(tag);
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
		public int Move => BasicMove + BonusMove;
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
		public float MoveMultiplier { get; set; }
		public DieString ThrustDamage => GurpsLookup.Instance.ThrustDamage(ST);
		public DieString SwingDamage => GurpsLookup.Instance.ThrustDamage(ST);

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
			BasicST = BasicDX = BasicIQ = BasicHT = 10;
			BasicDR = 0;
			BasicMove = 5;
			BasicSpeed = (DX + HT) / 4.0f;
			ArmorDivisor = 0;
			DamageResistances = new Dictionary<DamageType, float>();
			foreach (DamageType type in AllDamageTypes)
			{
				DamageResistances.Add(type, 1f);
			}
		}



		public void SaveXml(XmlElement sink)
		{
			throw new NotImplementedException();
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
				description.AppendLine(tag.Description);
				PropertyInfo property = GetType().GetProperty(tag.BonusTarget);

				object value = null;
				if (property.GetMethod.ReturnType == typeof(int))
				{
					int v = (int)property.GetValue(property);
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

		protected static Random rng = new Random();

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
			SaveToSink(sink, "BonusPer", $"{BonusPer}");
			SaveToSink(sink, "BonusWill", $"{BonusWill}");
			SaveToSink(sink, "BonusFP", $"{BonusFP}");
			SaveToSink(sink, "BasicMove", $"{BasicMove}");
			SaveToSink(sink, "Speed", $"{Speed}");
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
			diff += (int)(Math.Ceiling(Will / 4.0f));
			diff += (int)(Math.Ceiling(Perception / 4.0f));
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
			beast.BasicST = rng.Next(bounds.MinST, bounds.MaxST + 1);
			beast.BasicDX = rng.Next(bounds.MinDX, bounds.MaxDX + 1);
			beast.BasicIQ = rng.Next(bounds.MinIQ, bounds.MaxIQ + 1);
			beast.BasicHT = rng.Next(bounds.MinHT, bounds.MaxHT + 1);
			beast.BasicDR = rng.Next(bounds.MinDR, bounds.MaxDR + 1);
			beast.BasicPer = beast.BasicIQ + rng.Next(bounds.MinPer, bounds.MaxPer + 1);
			beast.BasicWill = beast.BasicIQ + rng.Next(bounds.MinWill, bounds.MaxWill + 1);
			beast.BasicSpeed = (beast.BasicDX+beast.BasicHT + rng.Next(bounds.MinSpeed, bounds.MaxSpeed + 1))/4f;
			beast.BasicMove = (int)Math.Floor( beast.BasicSpeed + rng.Next(bounds.MinMove, bounds.MaxMove + 1));
			
			beast.BasicHP = rng.Next(bounds.MinHP, bounds.MaxHP + 1);
			beast.BasicFP = rng.Next(bounds.MinFP, bounds.MaxFP + 1);

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









	public struct Bounds
	{
		public int MinST;
		public int MaxST;
		public int MinDX;
		public int MaxDX;
		public int MinIQ;
		public int MaxIQ;
		public int MinHT;
		public int MaxHT;
		public int MinDR;
		public int MaxDR;
		public int MinPer;
		public int MaxPer;
		public int MinWill;
		public int MaxWill;

		public int MinHP;
		public int MaxHP;
		public int MinFP;
		public int MaxFP;
		public int MinMove;
		public int MaxMove;

		public int MinSpeed;
		public int MaxSpeed;

	}




	public class Fortifier : Beast
	{
		static Bounds fortifierBounds = new Bounds
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
			MinWill = 10,
			MaxWill = 10,
			MinHP = 0,
			MaxHP = 5,
			MinFP = 0,
			MaxFP = 5,
			MinSpeed = 0,
			MaxSpeed = 4,

		};
		protected override void Init()
		{

		}
	}
	public class PackHunter : Beast
	{

	}
	public class Assassin : Beast
	{

	}
	public class Berserker : Beast
	{

	}
	public class Tactician : Beast
	{

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
			foreach (Armor armor in Tag.BoneArmor)
			{
				bob.AddTag(armor);
			}
			ITag temp = new BodyPlan();
			temp = new Armor();
			temp = new Milieu();

			bob.AddTag(Tag.BodyPlans[5]);
			bob.AddTag(Tag.Milieux[3]);

			doc.AppendChild(doc.CreateElement("root"));
			foreach (var b in new[]
				{
					bob, frank, sam
				}
			)

			{ b.SaveXmlRoot(doc); }


			doc.Save("test.xml");
			sam = new BeastTest();


			sam.LoadXml(doc.DocumentElement.FirstChild as XmlElement);
			Debug.Assert(bob.Name == sam.Name);
			return true;
		}
	}
}
