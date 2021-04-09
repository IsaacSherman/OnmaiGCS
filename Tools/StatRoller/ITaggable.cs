using System;
using System.Collections.Generic;

namespace StatRoller
{
	public interface ITaggable
	{
		HashSet<Tag> Tags { get; set; }
		bool AddTag(Tag tag);
	}
}