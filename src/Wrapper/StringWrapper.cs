namespace networkScript.Wrapper {
	public class StringWrapper : Object {
		private readonly Value m_value;

		public StringWrapper(Value value) {
			m_value = value;

			set("length", new Value(() => new Value(m_value.asString().Length)));
		}

		public override string ToString() { return "StringWrapper(" + m_value + ")"; }
	}
}