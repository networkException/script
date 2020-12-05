using networkScript.Parsing;

namespace networkScript
{
	public class Identifier : Expression
	{
		private readonly string m_value;

		public Identifier(string value) { m_value = value; }

		public Identifier(TokenMatch match)
		{
			m_value = match.value();
			m_info = match.info();
		}

		public override Value evaluate(Context context) { return context.get(this).evaluate(context); }

		public string value() { return m_value; }

		public override void dump(int indent) { dumpString(ToString(), indent); }

		public override string ToString() { return "Identifier(" + m_value + ")"; }
	}
}