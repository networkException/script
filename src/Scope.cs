using System.Collections.Generic;

namespace networkScript
{
	internal class Scope : Node
	{
		private readonly Dictionary<string, Value> m_identifiers;

		public Scope() { m_identifiers = new Dictionary<string, Value>(); }

		public bool has(string identifier) { return m_identifiers.ContainsKey(identifier); }

		public Value get(string identifier) { return has(identifier) ? m_identifiers[identifier] : Value.Undefined; }

		public Value declare(string identifier) { return m_identifiers[identifier] = Value.Null; }

		public Value assign(string identifier, Value value) { return m_identifiers[identifier] = value.copy(); }

		public Value reference(string identifier, ref Value value) { return m_identifiers[identifier] = value; }

		public override void dump(int indent)
		{
			base.dump(indent);

			foreach (KeyValuePair<string, Value> entry in m_identifiers)
			{
				dumpString(entry.Key + ": " + entry.Value.ToString(), indent + 1);
			}
		}

		public override string ToString() { return "Scope(" + string.Join(", ", m_identifiers) + ")"; }
	}
}