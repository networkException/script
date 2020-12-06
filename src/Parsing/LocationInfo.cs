namespace networkScript.Parsing {
	public class LocationInfo {
		private readonly int m_line;
		private readonly int m_column;
		private int m_length;
		private readonly TokenType m_token;

		public LocationInfo(int line, int column, int lenght, TokenType token) {
			m_line = line;
			m_length = lenght;
			m_column = column;
			m_token = token;
		}

		public TokenType token() { return m_token; }

		public string position() { return m_line + ":" + m_column; }

		public override string ToString() { return m_token + "(" + position() + ")"; }
	}
}