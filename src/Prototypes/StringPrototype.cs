using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace networkScript.Prototypes {
	public class StringPrototype : Object {
		private readonly string m_value;

		public StringPrototype(string value) {
			m_value = value;

			set("length", length);
			set("substring", substring);
			set("toLowerCase", toLowerCase);
			set("toLowerChar", toLowerChar);
			set("toUpperCase", toUpperCase);
			set("toUpperChar", toUpperChar);
			set("charAt", charAt);
			set("concat", concat);
			set("trim", trim);
			set("startsWith", startsWith);
			set("endsWith", endsWith);
			set("contains", contains);
			set("replace", replace);

			registerNumericIndex(index => new Value(m_value.ToCharArray()[index].ToString()));
		}

		private Value length() { return new Value(m_value.Length); }

		private Value substring(IReadOnlyList<Expression> parameters, Context context) {
			if (parameters.Count != 1 && parameters.Count != 2) context.typeError("Expected 1-2 arguments for string.prototype.substring");

			if (parameters.Count == 1) {
				int first = parameters[0].evaluate(context).getInt();

				return new Value(m_value.Substring(first));
			} else {
				int first = parameters[0].evaluate(context).getInt();
				int second = parameters[1].evaluate(context).getInt();

				return new Value(m_value.Substring(first, second - first));
			}
		}

		private Value sublength(IReadOnlyList<Expression> parameters, Context context) {
			if (parameters.Count != 1 && parameters.Count != 2) context.typeError("Expected 1-2 arguments for string.prototype.sublength");

			if (parameters.Count == 1) {
				int first = parameters[0].evaluate(context).getInt();

				return new Value(m_value.Substring(0, first));
			} else {
				int first = parameters[0].evaluate(context).getInt();
				int second = parameters[1].evaluate(context).getInt();

				return new Value(m_value.Substring(first, second));
			}
		}

		private Value toLowerCase(IReadOnlyList<Expression> parameters, Context context) { return new Value(m_value.ToLower()); }

		private Value toLowerChar(IReadOnlyList<Expression> parameters, Context context) {
			if (parameters.Count != 1) context.typeError("Expected 1 argument for string.prototype.toLowerChar");
			
			StringBuilder output = new StringBuilder(m_value);
			int index = parameters[0].evaluate(context).getInt();

			output[index] = m_value[index].ToString().ToLower()[0];

			return new Value(output.ToString());
		}
		
		private Value toUpperCase(IReadOnlyList<Expression> parameters, Context context) { return new Value(m_value.ToUpper()); }

		private Value toUpperChar(IReadOnlyList<Expression> parameters, Context context) {
			if (parameters.Count != 1) context.typeError("Expected 1 argument for string.prototype.toUpperChar");
			
			StringBuilder output = new StringBuilder(m_value);
			int index = parameters[0].evaluate(context).getInt();

			output[index] = m_value[index].ToString().ToUpper()[0];

			return new Value(output.ToString());
		}
		
		private Value trim(IReadOnlyList<Expression> parameters, Context context) { return new Value(m_value.Trim()); }

		private Value startsWith(IReadOnlyList<Expression> parameters, Context context) {
			if (parameters.Count != 1) context.typeError("Expected 1 argument for string.prototype.startsWith");

			return new Value(m_value.StartsWith(parameters[0].evaluate(context).asString()));
		}

		private Value endsWith(IReadOnlyList<Expression> parameters, Context context) {
			if (parameters.Count != 1 ) context.typeError("Expected 1 argument for string.prototype.endsWith");

			return new Value(m_value.EndsWith(parameters[0].evaluate(context).asString()));
		}

		private Value contains(IReadOnlyList<Expression> parameters, Context context) {
			if (parameters.Count != 1) context.typeError("Expected 1 argument for string.prototype.contains");

			return new Value(m_value.Contains(parameters[0].evaluate(context).asString()));
		}
		
		private Value replace(IReadOnlyList<Expression> parameters, Context context) {
			if (parameters.Count != 2) context.typeError("Expected 2 arguments for string.prototype.replace");

			string replace = parameters[0].evaluate(context).asString();
			string with = parameters[1].evaluate(context).asString();
			
			return new Value(m_value.Replace(replace, with));
		}

		private Value charAt(IReadOnlyList<Expression> parameters, Context context) {
			if (parameters.Count != 1) context.typeError("Expected 1 argument for string.prototype.charAt");

			return new Value(m_value.ToCharArray()[parameters[0].evaluate(context).getInt()].ToString());
		}

		private Value concat(IEnumerable<Expression> parameters, Context context) { return new Value(m_value + string.Join("", parameters.Select(expression => expression.evaluate(context).asString()))); }

		public override string ToString() { return m_value; }
	}
}