using System;
using networkScript.Expressions;

namespace networkScript {
	public abstract class Expression : Node {
		public abstract Value evaluate(Context context);

		public string asString(Context context) { return isIdentifier() ? ((Identifier) this).value() : evaluate(context).asString(); }

		public bool isIdentifier() { return GetType() == typeof(Identifier); }
		
		public virtual void visit(Action<Expression> visitor) { visitor.Invoke(this); }
	}
}