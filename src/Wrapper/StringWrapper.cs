namespace networkScript.Wrapper {
	public class StringWrapper : Object {
		private readonly string m_value;

		public StringWrapper(Value value) {
			m_value = value.asString();

			set("length", length);
		}

		private Value length() {
			return new Value(m_value.Length);
		}

		public override string ToString() { return "StringWrapper(" + m_value + ")"; }
	}
}