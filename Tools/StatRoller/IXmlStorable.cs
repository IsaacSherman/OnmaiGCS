using System.Xml;

namespace StatRoller
{
	public interface IXmlStorable
	{
		void SaveXmlRoot(XmlDocument sink); 
		void SaveXml(XmlElement sink); 
		void LoadXml(XmlElement source);
	}
}