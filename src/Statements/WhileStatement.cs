using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace networkScript.Statements {
	internal class WhileStatement : Statement {
		private readonly Expression m_predicate;
		private readonly Statement m_consequent;

		public WhileStatement(Expression predicate, Statement consequent) {
			m_predicate = predicate;
			m_consequent = consequent;
		}

		public override Value execute(Context context) {
			Value last = Value.Undefined;

			while (m_predicate.evaluate(context).getBoolean()) last = m_consequent.execute(context);

			return last;
		}

		public Expression predicate() { return m_predicate; }
		public Statement consequent() { return m_consequent; }

		public override void dump(int indent) {
			base.dump(indent);
			m_predicate.dump(indent + 1);
			m_consequent.dump(indent + 1);
		}
	}
}