using System;
using networkScript.Expressions;

namespace networkScript.Declarations {
	public class VariableDeclaration : Expression {
		private readonly IdentifierList m_identifiers;

		public VariableDeclaration(IdentifierList identifiers) { m_identifiers = identifiers; }

		public override Value evaluate(Context context) {
			foreach (Identifier identifier in m_identifiers.identifiers()) context.declare(identifier);

			return Value.Null;
		}

		public override void visit(Action<Expression> visitor) { m_identifiers.visit(visitor); }

		public override void dump(int indent) {
			base.dump(indent);
			m_identifiers.dump(indent + 1);
		}
	}
}