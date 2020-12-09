namespace networkScript.Statements {
	internal class ForStatement : Statement {
		private readonly Expression m_initializer;
		private readonly Expression m_predicate;
		private readonly Expression m_increment;
		private readonly Statement m_consequent;

		public ForStatement(Expression initializer, Expression predicate, Expression increment, Statement consequent) {
			m_initializer = initializer;
			m_predicate = predicate;
			m_increment = increment;
			m_consequent = consequent;
		}

		public override void execute(Context context) {
			context.enter();

			m_initializer.evaluate(context);

			while (m_predicate.evaluate(context).getBoolean()) {
				m_consequent.execute(context);
				m_increment.evaluate(context);
			}

			context.leave();
		}

		public override void dump(int indent) {
			base.dump(indent);
			m_initializer.dump(indent + 1);
			m_predicate.dump(indent + 1);
			m_increment.dump(indent + 1);
			m_consequent.dump(indent + 1);
		}
	}
}