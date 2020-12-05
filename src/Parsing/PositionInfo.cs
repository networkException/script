namespace networkScript.Parsing
{
	public class PositionInfo
	{
		private readonly int m_line;
		private readonly int m_column;
		private int m_length;
		private readonly Token m_token;

		public PositionInfo(int line, int column, int lenght, Token token)
		{
			m_line = line;
			m_length = lenght;
			m_column = column;
			m_token = token;
		}

		public Token token() { return m_token; }

		public string position() { return m_line + ":" + m_column; }
		
		public override string ToString() { return m_token + "(" + position() + ")"; }
	}
}