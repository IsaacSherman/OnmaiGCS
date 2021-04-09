using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace StatRoller
{
	public static class HelperFunctions
	{
		public static void ParseAttributes(XmlElement source, Type targetType, object target)
		{
			foreach (XmlAttribute attr in source.Attributes)
			{
				PropertyInfo property = targetType.GetProperty(attr.Name);
				if (property == null) throw new InvalidDataException($"Property \"{attr.Name}\" not found");
				object value = null;

				if (property.PropertyType == typeof(int))
				{
					value = int.Parse(attr.Value);
				}
				else if (property.PropertyType == typeof(float))
				{
					value = float.Parse(attr.Value);
				}
				else if (property.PropertyType == typeof(string))
				{
					value = attr.Value;
				}
				if (value != null)
				{
					property.SetValue(target, value);
				}
			}
		}
	}
}
