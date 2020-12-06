namespace networkScript {
	internal abstract class Statement : Node {
		public abstract Value execute(Context context);
	}
}