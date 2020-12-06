using System;
using System.Collections.Generic;

namespace networkScript {
	public class Object {
		private readonly Dictionary<string, Value> m_properties;
		private Func<int, Value> m_numeric_index;

		public Object() { m_properties = new Dictionary<string, Value>(); }

		public Value get(string key) { return m_properties[key]; }
		public bool has(string key) { return m_properties.ContainsKey(key); }
		public void set(string key, Value value) { m_properties[key] = value; }

		public void set(string key, Action<List<Expression>, Context> nativeFunction) { set(key, new Value(nativeFunction)); }

		public void set(string key, Func<List<Expression>, Context, Value> nativeFunction) { set(key, new Value(nativeFunction)); }

		public void set(string key, Func<Value, Value> nativeProperty) { set(key, new Value(nativeProperty)); }

		public void set(string key, Func<Value> nativeProperty) { set(key, new Value(nativeProperty)); }

		protected void registerNumericIndex(Func<int, Value> numericIndex) { m_numeric_index = numericIndex; }

		public bool hasNumericIndex() { return m_numeric_index != null; }

		public Value numericIndex(int index) { return m_numeric_index.Invoke(index); }
		
		public Value asValue() { return new Value(this); }

		public override string ToString() { return "Object(" + string.Join(", ", m_properties) + ")"; }
	}
}