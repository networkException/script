using System;
using System.IO;
using System.Text.RegularExpressions;

namespace networkScript.Parsing
{
	public class Token
	{
		private readonly TokenType m_type;
		private readonly string m_pattern;
		private readonly Func<string, string> m_modifier;
		
		public Token(TokenType type, string pattern)
		{
			m_type = type;
			m_pattern = pattern;
		}
		
		public Token(TokenType type, string pattern, Func<string, string> modifier)
		{
			m_type = type;
			m_pattern = pattern;
			m_modifier = modifier;
		}

		public MatchCollection match(string source) { return Regex.Matches(source, m_pattern); }

		public TokenType type() { return m_type; }

		public string modify(string input) { return m_modifier == null ? input : m_modifier.Invoke(input); }

		public override string ToString() { return m_type.ToString(); }
	}
}