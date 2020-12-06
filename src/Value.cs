using System;
using System.Collections.Generic;
using System.Diagnostics;
using networkScript.Parsing;

namespace networkScript {
	public class Value : Expression {
		private object m_value;
		private readonly Type m_type;

		private Value(Type type, object value) {
			m_value = value;
			m_type = type;
		}

		public Value(Type type, TokenMatch match) {
			m_value = match.value();
			m_info = match.info();
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
		/*
		public Value(Func<Value, Value> nativeProperty)
		{
			m_value = nativeProperty;
			m_type = Type.NativeProperty;
		}*/

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
			m_type = Type.Number;
		}

		public Value(double value) {
			m_value = value;
			m_type = Type.Number;
		}

		//FIXME: Full implementation
		public bool isNaN() { return !isNumber(); }
		public bool isNumber() { return m_type == Type.Number; }
		public bool isBoolean() { return m_type == Type.Boolean; }
		public bool isObject() { return m_type == Type.Object; }
		public bool isFunction() { return m_type == Type.Function; }
		public bool isNativeFunction() { return m_type == Type.NativeFunction; }
		public bool isNativeProperty() { return m_type == Type.NativeProperty; }

		public bool asBoolean() { return Convert.ToBoolean(m_value); }
		public double asDouble() { return Convert.ToDouble(m_value); }
		public Object asObject() { return (Object) m_value; }
		public Func<List<Expression>, Context, Value> asNativeFunction() { return (Func<List<Expression>, Context, Value>) m_value; }

		public Func<Value> asNativeProperty() { return (Func<Value>) m_value; }

		public string asString() {
			switch (m_type) {
				case Type.Undefined: return "undefined";
				case Type.Null: return "null";
				case Type.NativeFunction: return "<NativeFunction>";
				case Type.NativeProperty: return "<NativeProperty>";
				default: return Convert.ToString(m_value);
			}
		}

		public bool toBoolean() {
			switch (m_type) {
				case Type.Undefined:
				case Type.Null:
					return false;
				case Type.Boolean: return asBoolean();
				case Type.Number:
					if (isNaN()) return false;
					return asDouble() != 0;
				case Type.String: return asString() != "";
				default:
					Debug.Assert(true);
					return false;
			}
		}

		public Value increaseNumberBy(Value value) {
			if (!value.isNumber() || !isNumber()) return Undefined;

			m_value = asDouble() + value.asDouble();
			return this;
		}

		public static readonly Value Undefined = new Value(Type.Undefined, (object) null);
		public static readonly Value Null = new Value(Type.Null, (object) null);
		public static readonly Value False = new Value(Type.Boolean, false);
		public static readonly Value True = new Value(Type.Boolean, true);

		public Type type() { return m_type; }

		public override Value evaluate(Context context) { return isNativeProperty() ? asNativeProperty().Invoke() : this; }
		public override string ToString() { return asString(); }

		public Value copy() { return new Value(m_type, m_value); }

		public override void dump(int indent) { dumpString("Value(" + (m_type.Equals(Type.String) ? "\"" : "") + ToString() + (m_type.Equals(Type.String) ? "\"" : "") + ")", indent); }

		public enum Type {
			Null,
			Undefined,
			String,
			Number,
			Boolean,
			Object,
			Function,
			NativeFunction,
			NativeProperty
		}
	}
}