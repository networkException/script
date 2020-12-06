namespace networkScript.Expressions {
	internal class MemberExpression : Expression {
		private readonly Expression m_lhs;
		private readonly Expression m_rhs;

		public MemberExpression(Expression lhs, Expression rhs) {
			m_lhs = lhs;
			m_rhs = rhs;
		}

		public override Value evaluate(Context context) {
			Object lhs = m_lhs.evaluate(context).toObject();
			string key;

			if (lhs.hasNumericIndex() && !m_rhs.isIdentifier()) {
				Value rhs = m_rhs.evaluate(context);

				if (rhs.isNumber()) return lhs.numericIndex((int) rhs.asDouble());

				key = rhs.asString();
			} else {
				key = m_rhs.asString(context);
			}

			if(!lhs.has(key)) context.memberError("Property " + key + " does not exist on " + lhs);
			
			// The object's property needs to be evaluated again in case the result of this expression is the evaluation result of another
			// (Member)Expression evaluation this expression.
			return !lhs.has(key) ? Value.Undefined : lhs.get(key).evaluate(context);
		}

		public override void dump(int indent) {
			base.dump(indent);
			m_lhs.dump(indent + 1);
			m_rhs.dump(indent + 1);
		}

		public override string ToString() {
			return "MemberExpression(" + m_lhs + ", " + m_rhs + ")";
		}
	}
}