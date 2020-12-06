namespace networkScript.Prototypes {
	public class BooleanPrototype : Object {
		private readonly bool m_value;

		public BooleanPrototype(bool value) { m_value = value; }

		public override string ToString() { return m_value ? "true" : "false"; }
	}
}