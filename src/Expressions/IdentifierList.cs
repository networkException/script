using System;
using System.Collections.Generic;
using System.Data;

namespace networkScript.Expressions {
	public class IdentifierList : Expression {
		private readonly List<Identifier> m_identifiers;

		public IdentifierList(List<Identifier> identifiers) { m_identifiers = identifiers; }

		public override Value evaluate(Context context) { throw new EvaluateException("Disallowed method call evaluate on visitable expression"); }

		public new void visit(Action<Expression> visitor) {
			foreach (Identifier identifier in m_identifiers) visitor.Invoke(identifier);
		}

		public List<Identifier> identifiers() { return m_identifiers; }

		public override void dump(int indent) {
			base.dump(indent);

			foreach (Identifier identifier in m_identifiers) identifier.dump(indent + 1);
		}
	}
}