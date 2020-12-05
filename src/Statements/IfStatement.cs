namespace networkScript.Statements
{
	internal class IfStatement : Statement
	{
		private readonly Expression m_predicate;
		private readonly Statement m_consequent;
		private readonly Statement m_alternate;

		public IfStatement(Expression predicate, Statement consequent, Statement alternate = null)
		{
			m_predicate = predicate;
			m_consequent = consequent;
			m_alternate = alternate;
		}

		public override Value execute(Context context)
		{
			if (m_predicate.evaluate(context).asBoolean())
			{
				return m_consequent.execute(context);
			}

			return m_alternate != null ? m_alternate.execute(context) : Value.Undefined;
		}

		public Expression predicate() { return m_predicate; }
		public Statement consequent() { return m_consequent; }
		public Statement alternate() { return m_alternate; }

		public override void dump(int indent)
		{
			base.dump(indent);
			m_predicate.dump(indent + 1);
			m_consequent.dump(indent + 1);
			m_alternate?.dump(indent + 1);
		}
	}
}