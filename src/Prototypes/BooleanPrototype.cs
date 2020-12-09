using System.Collections.Generic;

namespace networkScript.Prototypes {
	public class BooleanPrototype : Object {
		private readonly bool m_value;

		public BooleanPrototype(bool value) {
			m_value = value;

			set("invert", invert);
		}

		private Value invert(IReadOnlyList<Expression> parameters, Context context) {
			return new Value(!m_value);
		}

		public override string ToString() { return m_value ? "true" : "false"; }
	}
}