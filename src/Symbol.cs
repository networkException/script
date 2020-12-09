using networkScript.Expressions;
using networkScript.Parsing;

namespace networkScript {
	public class Symbol : Identifier {
		public Symbol(string value) : base(value) { }

		public Symbol(TokenMatch match) : base(match) { }

		public override Value evaluate(Context context) { return new Value(m_value); }

		public override void dump(int indent) { dumpString(ToString(), indent); }

		public override string ToString() { return "Symbol(" + m_value + ")"; }
	}
}