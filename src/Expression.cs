using System;

namespace networkScript {
	public abstract class Expression : Node {
		public abstract Value evaluate(Context context);

		public virtual void visit(Action<Expression> visitor) { visitor.Invoke(this); }
	}
}