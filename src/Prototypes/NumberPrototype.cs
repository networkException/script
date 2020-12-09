using System.Collections.Generic;
using System.Globalization;

namespace networkScript.Prototypes {
	public class NumberPrototype : Object {
		private readonly double m_value;

		public NumberPrototype(double value) {
			m_value = value;
			set("toString", toString);
		}

		private Value toString(List<Expression> parameters, Context context) { return new Value(m_value.ToString(CultureInfo.InvariantCulture)); }
	}
}