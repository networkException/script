using System;
using System.Collections.Generic;
using System.Data;

namespace networkScript.Expressions {
	public class SymbolList : Expression {
		private readonly List<Symbol> m_symbols;

		public SymbolList(List<Symbol> symbols) { m_symbols = symbols; }

		public override Value evaluate(Context context) { throw new EvaluateException("Disallowed method call evaluate on visitable expression"); }

		public new void visit(Action<Expression> visitor) {
			foreach (Symbol identifier in m_symbols) visitor.Invoke(identifier);
		}

		public List<Symbol> symbols() { return m_symbols; }

		public override void dump(int indent) {
			base.dump(indent);

			foreach (Symbol identifier in m_symbols) identifier.dump(indent + 1);
		}
	}
}