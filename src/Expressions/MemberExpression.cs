namespace networkScript.Expressions
{
	internal class MemberExpression : Expression
	{
		private readonly Expression m_lhs;
		private readonly Expression m_rhs;

		public MemberExpression(Expression lhs, Expression rhs)
		{
			m_lhs = lhs;
			m_rhs = rhs;
		}

		public override Value evaluate(Context context)
		{
			Value lhs = m_lhs.evaluate(context);
			if (!lhs.isObject()) return Value.Undefined; // TODO: Wrapper type checks

			string key = m_rhs.asString(context);
			Object lhsAsObject = lhs.asObject();

			// The object's property needs to be evaluated again in case the result of this expression is the evaluation result of another
			// (Member)Expression evaluation this expression.
			return !lhsAsObject.has(key) ? Value.Undefined : lhsAsObject.get(key).evaluate(context);
		}

		public override void dump(int indent)
		{
			base.dump(indent);
			m_lhs.dump(indent + 1);
			m_rhs.dump(indent + 1);
		}
	}
}