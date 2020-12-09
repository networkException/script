using networkScript.Parsing;

namespace networkScript.Expressions {
	public class Identifier : Expression {
		protected readonly string m_value;

		public Identifier(string value) { m_value = value; }

		public Identifier(TokenMatch match) {
			m_value = match.value();
			m_info = match.info();
		}

		public override Value evaluate(Context context) { return context.get(m_value).evaluate(context); }

		public override void dump(int indent) { dumpString(ToString(), indent); }

		public override string ToString() { return "Identifier(" + m_value + ")"; }
	}
}