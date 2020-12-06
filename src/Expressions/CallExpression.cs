using System;
using System.Collections.Generic;
using System.Linq;

namespace networkScript.Expressions
{
	internal class CallExpression : Expression
	{
		private readonly Expression m_expression;
		private readonly List<Expression> m_parameters;

		public CallExpression(Expression expression, List<Expression> parameters)
		{
			m_expression = expression;
			m_parameters = parameters;
		}

		public override Value evaluate(Context context)
		{
			Value function = m_expression.evaluate(context);

			if (function.isNativeFunction()) return function.asNativeFunction().Invoke(m_parameters, context);
			
			context.typeError("Value of type " + function.type() + " is not callable");
			
			return Value.Undefined;
		}

		public override void dump(int indent)
		{
			base.dump(indent);
			m_expression.dump(indent + 1);

			foreach (Expression parameter in m_parameters)
				parameter.dump(indent + 1);
		}
	}
}