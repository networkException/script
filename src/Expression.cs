using System;
using networkScript.Expressions;

namespace networkScript {
	public abstract class Expression : Node {
		public abstract Value evaluate(Context context);

		public string asString(Context context) { return GetType() == typeof(Identifier) ? ((Identifier) this).value() : evaluate(context).asString(); }

		public virtual void visit(Action<Expression> visitor) { visitor.Invoke(this); }
	}
}