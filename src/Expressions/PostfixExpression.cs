namespace networkScript.Expressions
{
	internal enum PostfixOperation
	{
		Increment
	}

	internal class PostfixExpression : Expression
	{
		private readonly PostfixOperation m_operation;
		private readonly Expression m_value;

		public PostfixExpression(PostfixOperation operation, Expression value)
		{
			m_operation = operation;
			m_value = value;
		}

		public override Value evaluate(Context context)
		{
			switch (m_operation)
			{
				case PostfixOperation.Increment:
				{
					Value lhs = m_value.evaluate(context);

					if (lhs.isNumber()) return lhs.increaseNumberBy(new Value(1));

					context.typeError("Postfix expression only applicable to numbers", m_value);
					return Value.Undefined;
				}
				default:
					return Value.Undefined;
			}
		}

		public override void dump(int indent)
		{
			base.dump(indent);
			dumpString(m_operation.ToString(), indent + 1);
			m_value.dump(indent + 1);
		}
	}
}