using System;

namespace networkScript.Expressions
{
	internal class MemberExpression : Expression
	{
		private readonly Expression m_expression;
		private readonly Expression m_name;

		public MemberExpression(Expression expression, Expression name)
		{
			m_expression = expression;
			m_name = name;
		}

		public override Value evaluate(Context context)
		{
			Value lhs = m_expression.evaluate(context);
			if (!lhs.isObject()) return Value.Undefined;

			string key = m_name.GetType() == typeof(Identifier) ? ((Identifier) m_name).value() : m_name.evaluate(context).asString();
			Object found = lhs.asObject();

			return !found.has(key) ? Value.Undefined : found.get(key);
		}

		public override void dump(int indent)
		{
			base.dump(indent);
			m_expression.dump(indent + 1);
			m_name.dump(indent + 1);
		}
	}
}