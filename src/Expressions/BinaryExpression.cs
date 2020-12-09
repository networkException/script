using networkScript.Declarations;

namespace networkScript.Expressions {
	internal enum BinaryOperation {
		Equals,
		NotEquals,
		Assign,
		Reference,
		Greater,
		Less,
		PlusEquals,
		Add,
		Subtract,
		Multiply,
		Divide
	}

	internal class BinaryExpression : Expression {
		private readonly BinaryOperation m_operation;
		private readonly Expression m_lhs;
		private readonly Expression m_rhs;

		public BinaryExpression(BinaryOperation operation, Expression lhs, Expression rhs) {
			m_operation = operation;
			m_lhs = lhs;
			m_rhs = rhs;
		}

		public override Value evaluate(Context context) {
			switch (m_operation) {
				case BinaryOperation.Equals: {
					Value lhs = m_lhs.evaluate(context);
					Value rhs = m_rhs.evaluate(context);

					return new Value(lhs.type() == rhs.type() && lhs.asString() == rhs.asString());
				}
				case BinaryOperation.NotEquals: {
					Value lhs = m_lhs.evaluate(context);
					Value rhs = m_rhs.evaluate(context);

					return new Value(!(lhs.type() == rhs.type() && lhs.asString() == rhs.asString()));
				}
				case BinaryOperation.Assign: {
					Value lhs = m_lhs.evaluate(context);
					Value rhs = m_rhs.evaluate(context);

					if (lhs.type() == rhs.type() || lhs.type() == Value.Type.Null && m_lhs.GetType() == typeof(VariableDeclaration)) {
						return context.assign(m_lhs, rhs);
					}
					
					context.typeError("Cannot assign " + rhs.type() + " to " + lhs.type(), m_lhs);
					return Value.Undefined;
				}
				case BinaryOperation.Reference: {
					Value lhs = m_lhs.evaluate(context);
					Value rhs = m_rhs.evaluate(context);

					if (lhs.type() == rhs.type() || lhs.type() == Value.Type.Null && m_lhs.GetType() == typeof(VariableDeclaration)) {
						context.reference(m_lhs, m_rhs);
						return rhs;
					}
					
					context.typeError("Cannot reference " + rhs.type() + " to " + lhs.type(), m_lhs);
					return Value.Undefined;
				}
				case BinaryOperation.Greater: {
					Value lhs = m_lhs.evaluate(context);
					Value rhs = m_rhs.evaluate(context);

					if (lhs.isNumber() && rhs.isNumber()) return new Value(m_lhs.evaluate(context).getDouble() > m_rhs.evaluate(context).getDouble());

					return Value.False;
				}
				case BinaryOperation.Less: {
					Value lhs = m_lhs.evaluate(context);
					Value rhs = m_rhs.evaluate(context);

					if (lhs.isNumber() && rhs.isNumber()) return new Value(m_lhs.evaluate(context).getDouble() < m_rhs.evaluate(context).getDouble());

					return Value.False;
				}
				case BinaryOperation.Add: {
					Value lhs = m_lhs.evaluate(context);
					Value rhs = m_rhs.evaluate(context);

					if (lhs.type() == Value.Type.Number && rhs.type() == Value.Type.Number) return new Value(lhs.getDouble() + rhs.getDouble());

					if (lhs.type() == Value.Type.String || rhs.type() == Value.Type.String) return new Value(lhs.asString() + rhs.asString());

					return Value.Null;
				}
				case BinaryOperation.Multiply: {
					Value lhs = m_lhs.evaluate(context);
					Value rhs = m_rhs.evaluate(context);

					if (lhs.type() == Value.Type.Number && rhs.type() == Value.Type.Number) return new Value(lhs.getDouble() * rhs.getDouble());

					return Value.Null;
				}
				case BinaryOperation.PlusEquals: {
					Value lhs = m_lhs.evaluate(context);
					Value rhs = m_rhs.evaluate(context);

					if (lhs.isNumber() && rhs.isNumber()) return context.get(m_lhs).evaluate(context).increaseNumberBy(m_rhs.evaluate(context));

					context.typeError("PlusEquals binary expression only applicable to numbers", lhs);
					return Value.Null;
				}
				default: return Value.Undefined;
			}
		}

		public BinaryOperation operation() { return m_operation; }
		public Expression lhs() { return m_lhs; }
		public Expression rhs() { return m_rhs; }

		public override void dump(int indent) {
			base.dump(indent);
			m_lhs.dump(indent + 1);
			dumpString(m_operation.ToString(), indent + 1);
			m_rhs.dump(indent + 1);
		}
	}
}