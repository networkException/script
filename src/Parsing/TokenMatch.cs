namespace networkScript.Parsing
{
	public class TokenMatch
	{
		private readonly string m_value;
		private readonly PositionInfo m_info;

		public TokenMatch(string value, PositionInfo info)
		{
			m_value = value;
			m_info = info;
		}

		public Token token() { return m_info.token(); }
		public string value() { return m_value; }

		public PositionInfo info() { return m_info; }

		public override string ToString() { return m_info.token() + "(" + m_value + ") at line " + m_info.position(); }
	}
}