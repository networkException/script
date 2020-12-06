using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace networkScript.Statements
{
	internal class ExpressionStatement : Statement
	{
		private readonly Expression m_expression;

		public ExpressionStatement(Expression expression) { m_expression = expression; }

		public override Value execute(Context context) { return m_expression.evaluate(context); }

		public override void dump(int indent)
		{
			base.dump(indent);
			m_expression.dump(indent + 1);
		}
	}
}