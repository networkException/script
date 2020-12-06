using System.Collections.Generic;

namespace networkScript
{
	internal class Scope : Node
	{
		private readonly Dictionary<string, Expression> m_identifiers;

		public Scope() { m_identifiers = new Dictionary<string, Expression>(); }

		public bool has(string identifier) { return m_identifiers.ContainsKey(identifier); }

		public Expression get(string identifier) { return has(identifier) ? m_identifiers[identifier] : Value.Undefined; }

		public Expression declare(string identifier) { return m_identifiers[identifier] = Value.Null; }

		public Expression assign(string identifier, Value value) { return m_identifiers[identifier] = value.copy(); }

		public Expression reference(string identifier, Expression value) { return m_identifiers[identifier] = value; }

		public override void dump(int indent)
		{
			base.dump(indent);

			foreach (KeyValuePair<string, Expression> entry in m_identifiers)
				dumpString(entry.Key + ": " + entry.Value, indent + 1);
		}

		public override string ToString() { return "Scope(" + string.Join(", ", m_identifiers) + ")"; }
	}
}