using System;
using System.Collections.Generic;

namespace networkScript.Prototypes {
	public class NativeFunctionPrototype : Object {
		private readonly Func<List<Expression>, Context, Value> m_native_function;

		public NativeFunctionPrototype(Func<List<Expression>, Context, Value> nativeFunction) { m_native_function = nativeFunction; }

		public override string ToString() { return "<NativeFunction>"; }
	}
}