using System;
using System.Collections.Generic;
using System.Linq;

namespace networkScript {
	public class Object {
		private readonly Dictionary<string, Value> m_properties;

		public Object() { m_properties = new Dictionary<string, Value>(); }

		public Value get(string key) { return m_properties[key]; }
		public bool has(string key) { return m_properties.ContainsKey(key); }
		public void set(string key, Value value) { m_properties[key] = value; }

		public Value asValue() { return new Value(this); }

		public override string ToString() { return "Object(" + string.Join(", ", m_properties) + ")"; }
	}
}