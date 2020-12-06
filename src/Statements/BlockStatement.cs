using System.Collections.Generic;
using System.Linq;

namespace networkScript.Statements {
	internal class BlockStatement : Statement {
		private readonly List<Statement> m_children;

		public BlockStatement() { m_children = new List<Statement>(); }
		public BlockStatement(params Statement[] children) { m_children = children.ToList(); }

		public override Value execute(Context context) {
			context.enter();

			Value result = Value.Undefined;

			foreach (Statement child in m_children) result = child.execute(context);

			context.leave();

			return result;
		}

		public void append(Statement child) { m_children.Add(child); }
		public List<Statement> children() { return m_children; }

		public override void dump(int indent) {
			base.dump(indent);

			foreach (Statement child in m_children) child.dump(indent + 1);
		}
	}
}