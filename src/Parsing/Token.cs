using System.Text.RegularExpressions;

namespace networkScript.Parsing {
	public class Token {
		private readonly TokenType m_type;
		private readonly string m_pattern;

		public Token(TokenType type, string pattern) {
			m_type = type;
			m_pattern = pattern;
		}

		public MatchCollection match(string source) { return Regex.Matches(source, m_pattern); }

		public TokenType type() { return m_type; }

		public override string ToString() { return m_type.ToString(); }
	}
}