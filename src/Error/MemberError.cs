using System;

namespace networkScript.Error {
	[Serializable]
	public class MemberError : Exception {
		public MemberError(string message) : base(message) { }
	}
}