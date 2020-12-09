using System;
using System.Collections.Generic;
using System.Diagnostics;
using networkScript.Parsing;
using networkScript.Prototypes;

namespace networkScript {
	public class Value : Expression {
		private object m_value;
		private Type m_type;

		private Value(Type type, object value) {
			m_value = value;
			m_type = type;
		}

		public Value(Type type, TokenMatch match) {
			m_value = match.value();
			m_info = match.info();
			m_type = type;
		}

		public Value(Type type, object value, LocationInfo info) {
			m_value = value;
			m_info = info;
			m_type = type;
		}

		public Value(Object value) {
			m_value = value;
			m_type = Type.Object;
		}

		public Value(Function value) {
			m_value = value;
			m_type = Type.Function;
		}

		public Value(Action<List<Expression>, Context> nativeFunction) : this((parameters, context) => {
			nativeFunction.Invoke(parameters, context);
			return Undefined;
		}) { }

		public Value(Func<List<Expression>, Context, Value> nativeFunction) {
			m_value = nativeFunction;
			m_type = Type.NativeFunction;
		}

		// TODO: Setter implementation
		public Value(Func<Value, Value> nativeProperty) { throw new NotImplementedException("NativeProperty with Setter not implemented"); }

		public Value(Func<Value> nativeProperty) {
			m_value = nativeProperty;
			m_type = Type.NativeProperty;
		}

		public Value(bool value) {
			m_value = value;
			m_type = Type.Boolean;
		}

		public Value(string value) {
			m_value = value;
			m_type = Type.String;
		}

		public Value(int value) {
			m_value = value;
			m_type = Type.Int;
		}

		public Value(double value) {
			m_value = value;
			m_type = Type.Double;
		}

		//FIXME: Full implementation
		public bool isNumber() { return isDouble() || isInt(); }
		public bool isDouble() { return m_type == Type.Double; }
		public bool isInt() { return m_type == Type.Int; }
		public bool isString() { return m_type == Type.String; }
		public bool isBoolean() { return m_type == Type.Boolean; }
		public bool isObject() { return m_type == Type.Object; }
		public bool isFunction() { return m_type == Type.Function; }
		public bool isNativeFunction() { return m_type == Type.NativeFunction; }
		public bool isNativeProperty() { return m_type == Type.NativeProperty; }

		public bool getBoolean() { return (bool) m_value; }
		public double getDouble() { return (double) m_value; }
		public int getInt() { return (int) m_value; }
		public string getString() { return (string) m_value; }
		public Object getObject() { return (Object) m_value; }
		public Func<List<Expression>, Context, Value> getNativeFunction() { return (Func<List<Expression>, Context, Value>) m_value; }
		public Func<Value> getNativeProperty() { return (Func<Value>) m_value; }

		public string asString() {
			switch (m_type) {
				case Type.String: return getString();
				case Type.Boolean: return getBoolean() ? "true" : "false";
				case Type.Undefined: return "undefined";
				case Type.Null: return "null";
				case Type.NativeFunction: return "<NativeFunction>";
				case Type.NativeProperty: return "<NativeProperty>";
				default: return m_value.ToString();
			}
		}

		public bool asBoolean() {
			switch (m_type) {
				case Type.Undefined:
				case Type.Null:
					return false;
				case Type.Boolean: return getBoolean();
				case Type.Double: return getDouble() != 0;
				case Type.Int: return getDouble() != 0;
				case Type.String: return asString() != "";
				default:
					Debug.Assert(true);
					return false;
			}
		}
		
		public double asDouble() {
			switch (m_type) {
				case Type.Int: return Convert.ToDouble(getInt());
				case Type.Double: return getDouble();
				default:
					Debug.Assert(true);
					return 0;
			}
		}
		
		public Object asObject() {
			switch (m_type) {
				case Type.Object: return getObject();
				case Type.String: return new StringPrototype(asString());
				case Type.Boolean: return new BooleanPrototype(getBoolean());
				case Type.NativeFunction: return new NativeFunctionPrototype(getNativeFunction());
				default: return new Object();
			}
		}

		public Value increaseNumberBy(Value value) {
			if (!value.isNumber() || !isNumber()) return Undefined;
			if (value.isInt() && isInt()) {
				m_value = getInt() + value.getInt();
				return this;
			}

			m_type = Type.Double;
			m_value = m_value = asDouble() + value.asDouble();

			return this;
		}

		public static readonly Value Undefined = new Value(Type.Undefined, (object) null);
		public static readonly Value Null = new Value(Type.Null, (object) null);
		public static readonly Value False = new Value(Type.Boolean, false);
		public static readonly Value True = new Value(Type.Boolean, true);

		public Type type() { return m_type; }

		public override Value evaluate(Context context) { return isNativeProperty() ? getNativeProperty().Invoke() : this; }

		public override string ToString() {
			if (isString()) return "string: '" + asString() + "'";
			if (isBoolean()) return "boolean" + (getBoolean() ? "true" : "false");
			return m_type.ToString().ToLower() + ": "+ asString();
		}

		public Value copy() { return new Value(m_type, m_value); }

		public override void dump(int indent) { dumpString("Value(" + ToString() + ")", indent); }

		public enum Type {
			Null,
			Undefined,
			String,
			Double,
			Int,
			Boolean,
			Object,
			Function,
			NativeFunction,
			NativeProperty
		}
	}
}