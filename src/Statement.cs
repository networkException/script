namespace networkScript {
	internal abstract class Statement : Node {
		public abstract void execute(Context context);
	}
}