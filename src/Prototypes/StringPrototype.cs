using System.Collections.Generic;
using System.Linq;

namespace networkScript.Prototypes {
	public class StringPrototype : Object {
		private readonly string m_value;

		public StringPrototype(string value) {
			m_value = value;

			set("length", length);
			set("substring", substring);
			set("toLowerCase", toLowerCase);
			set("toUpperCase", toUpperCase);
			set("charAt", charAt);
			set("concat", concat);
			set("trim", trim);

			registerNumericIndex(index => new Value(m_value.ToCharArray()[index].ToString()));
		}

		private Value length() { return new Value(m_value.Length); }

		private Value substring(IReadOnlyList<Expression> parameters, Context context) {
			if (parameters.Count != 1 && parameters.Count != 2) context.typeError("Expected 1-2 arguments for string.prototype.substring");

			if (parameters.Count == 1) {
				int first = (int) parameters[0].evaluate(context).getDouble();

				return new Value(m_value.Substring(first));
			} else {
				int first = (int) parameters[0].evaluate(context).getDouble();
				int second = (int) parameters[1].evaluate(context).getDouble();

				return new Value(m_value.Substring(first, second - first));
			}
		}

		private Value sublength(IReadOnlyList<Expression> parameters, Context context) {
			if (parameters.Count != 1 && parameters.Count != 2) context.typeError("Expected 1-2 arguments for string.prototype.sublength");

			if (parameters.Count == 1) {
				int first = (int) parameters[0].evaluate(context).getDouble();

				return new Value(m_value.Substring(0, first));
			} else {
				int first = (int) parameters[0].evaluate(context).getDouble();
				int second = (int) parameters[1].evaluate(context).getDouble();

				return new Value(m_value.Substring(first, second));
			}
		}

		private Value toLowerCase(IReadOnlyList<Expression> parameters, Context context) { return new Value(m_value.ToLower()); }

		private Value toUpperCase(IReadOnlyList<Expression> parameters, Context context) { return new Value(m_value.ToUpper()); }

		private Value trim(IReadOnlyList<Expression> parameters, Context context) { return new Value(m_value.Trim()); }

		private Value charAt(IReadOnlyList<Expression> parameters, Context context) {
			if (parameters.Count != 1) context.typeError("Expected 1 argument for string.prototype.charAt");

			return new Value(m_value.ToCharArray()[(int) parameters[0].evaluate(context).getDouble()].ToString());
		}

		private Value concat(IEnumerable<Expression> parameters, Context context) { return new Value(m_value + string.Join("", parameters.Select(expression => expression.evaluate(context).asString()))); }

		public override string ToString() { return m_value; }
	}
}