using System;
using System.Collections.Generic;
using System.Linq;
using networkScript.Parsing;

namespace networkScript.Expressions {
	public class TemplateExpression : Expression {
		private readonly List<Expression> m_elements;

		public TemplateExpression(List<Expression> elements, LocationInfo info) {
			m_elements = elements;
			m_info = info;
		}

		public override Value evaluate(Context context) { return new Value(string.Join("", m_elements.Select(element => element.evaluate(context).asString()))); }

		public override void visit(Action<Expression> visitor) { m_elements.ForEach(visitor); }

		public override void dump(int indent) {
			base.dump(indent);

			foreach (Expression element in m_elements) element.dump(indent + 1);
		}
		public override string ToString() {
			return string.Join(", ", m_elements);
		}
	}
}