using System;

namespace networkScript.Error
{
	[Serializable]
	public class TypeError : Exception
	{
		public TypeError(string message) : base(message) { }
	}
}