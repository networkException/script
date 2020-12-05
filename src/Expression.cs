using System;

namespace networkScript
{
	public abstract class Expression : Node
	{
		public abstract Value evaluate(Context context);

		public string asString(Context context) { return GetType() == typeof(Identifier) ? ((Identifier) this).value() : evaluate(context).asString(); }
		
		public void visit(Action<Expression> visitor) { visitor.Invoke(this); }
	}
}