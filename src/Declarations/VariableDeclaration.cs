using System;
using networkScript.Expressions;

namespace networkScript.Declarations {
	public class VariableDeclaration : Expression {
		private readonly SymbolList m_symbols;

		public VariableDeclaration(SymbolList symbols) { m_symbols = symbols; }

		public override Value evaluate(Context context) {
			foreach (Symbol symbol in m_symbols.symbols()) context.declare(symbol);

			return Value.Null;
		}

		public override void visit(Action<Expression> visitor) { m_symbols.visit(visitor); }

		public override void dump(int indent) {
			base.dump(indent);
			m_symbols.dump(indent + 1);
		}
	}
}