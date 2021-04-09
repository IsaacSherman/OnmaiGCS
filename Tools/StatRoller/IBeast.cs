using System.Xml.Serialization;

namespace StatRoller
{
	public interface IBeast : ITaggable, IGURPS,  IDamageTaker, IXmlStorable, IArmored
	{
		int Difficulty { get; }
	}
}