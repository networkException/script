using System.Collections.Generic;

namespace networkScript.Prototypes {
	public class StringPrototype : Object {
		private readonly string m_value;

		public StringPrototype(Value value) {
			m_value = value.asString();

			set("length", length);
			set("substring", substring);
			set("sublength", sublength);
		}

		private Value length() { return new Value(m_value.Length); }

		private Value substring(IReadOnlyList<Expression> parameters, Context context) {
			if (parameters.Count != 1 && parameters.Count != 2) context.typeError("Expected 1-2 arguments for string.prototype.substring");

			if (parameters.Count == 1) {
				int first = (int) parameters[0].evaluate(context).asDouble();
				
				return new Value(m_value.Substring(first));
			} else {
				int first = (int) parameters[0].evaluate(context).asDouble();
				int second = (int) parameters[0].evaluate(context).asDouble();
				
				return new Value(m_value.Substring(first, m_value.Length - second));
			}
		}

		private Value sublength(IReadOnlyList<Expression> parameters, Context context) {
			if (parameters.Count != 1 && parameters.Count != 2) context.typeError("Expected 1-2 arguments for string.prototype.sublength");

			if (parameters.Count == 1) {
				int first = (int) parameters[0].evaluate(context).asDouble();
				
				return new Value(m_value.Substring(0, first));
			} else {
				int first = (int) parameters[0].evaluate(context).asDouble();
				int second = (int) parameters[0].evaluate(context).asDouble();
				
				return new Value(m_value.Substring(first, second));
			}
		}

		public override string ToString() { return "string.prototype(" + m_value + ")"; }
	}
}