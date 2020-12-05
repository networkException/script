using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace networkScript.Parsing
{
	internal class Tokenizer
	{
		private readonly List<Token> m_definitions;
		private readonly Token m_end_of_file;

		private bool m_line_comment;
		private bool m_block_comment;

		private int m_line;
		private int m_column;

		private string m_source;

		public Tokenizer(string source)
		{
			m_definitions = new List<Token>
			{
				new Token(TokenType.If, "^if"),
				new Token(TokenType.Else, "^else"),
				new Token(TokenType.For, "^for"),
				new Token(TokenType.While, "^while"),
				new Token(TokenType.Let, "^let"),
				new Token(TokenType.Const, "^const"),
				new Token(TokenType.Class, "^class"),
				new Token(TokenType.EqualsEquals, "^=="),
				new Token(TokenType.ExclamationEquals, "^!="),
				new Token(TokenType.ColumnEquals, "^:="),
				new Token(TokenType.Semicolon, "^;"),
				new Token(TokenType.Period, "^\\."),
				new Token(TokenType.PlusPlus, "^\\+\\+"),
				new Token(TokenType.Comma, "^,"),
				new Token(TokenType.Greater, "^>"),
				new Token(TokenType.Less, "^<"),
				new Token(TokenType.Minus, "^-"),
				new Token(TokenType.Asterisk, "^\\*"),
				new Token(TokenType.Slash, "^/"),
				new Token(TokenType.PlusPlus, "^\\+\\+"),
				new Token(TokenType.PlusEquals, "^\\+="),
				new Token(TokenType.ParenOpen, "^\\("),
				new Token(TokenType.ParenClose, "^\\)"),
				new Token(TokenType.CurlyOpen, "^\\{"),
				new Token(TokenType.CurlyClose, "^\\}"),
				new Token(TokenType.BracketOpen, "^\\["),
				new Token(TokenType.BracketClose, "^\\]"),
				new Token(TokenType.StringLiteral, "^\"(.*?)\""),
				new Token(TokenType.StringLiteral, "^'(.*?)'"),
				new Token(TokenType.NumericLiteral, "^\\d+\\.\\d+"),
				new Token(TokenType.NumericLiteral, "^(\\d+)"),
				new Token(TokenType.BooleanLiteral, "^(?:true|false)"),
				new Token(TokenType.TypeDefinition, "^: +([A-Za-z0-9_]+)"),
				new Token(TokenType.Identifier, "^[A-Za-z0-9_]+"),
				new Token(TokenType.Equals, "^="),
				new Token(TokenType.Plus, "^\\+")
			};

			m_end_of_file = new Token(TokenType.Eof, "");

			m_line_comment = false;
			m_block_comment = false;

			m_line = 0;
			m_column = 0;

			m_source = source;
		}

		public List<TokenMatch> tokenize()
		{
			m_source = Regex.Replace(m_source, @"\r\n|\n\r|\n|\r|\t", Environment.NewLine);

			List<TokenMatch> matches = new List<TokenMatch>();

			while (!done())
			{
				bool matched = false;

				if (match(Environment.NewLine))
				{
					if (m_line_comment) m_line_comment = false;

					takeLine();
					continue;
				}

				if (matchAndTake("//"))
				{
					m_line_comment = true;
					continue;
				}

				if (matchAndTake("/*"))
				{
					m_block_comment = true;
					continue;
				}

				if (matchAndTake("*/"))
				{
					m_block_comment = false;
					continue;
				}

				if (!done() && matchAndTake(" ")) continue;

				if (m_block_comment || m_line_comment)
				{
					take(1);
					continue;
				}

				foreach (Token token in m_definitions)
				{
					MatchCollection match = token.match(m_source);

					if (match.Count != 1 || !match[0].Success) continue;

					matched = true;
					int length = match[0].Value.Length;
					string value = match[0].Groups.Count == 2 ? match[0].Groups[1].Value : match[0].Value;

					TokenMatch tokenMatch = new TokenMatch(value, new PositionInfo(m_line, m_column, length, token));

					matches.Add(tokenMatch);
					Console.WriteLine(tokenMatch);

					take(length);
					break;
				}

				if (matched) continue;

				List<TokenMatch> empty = new List<TokenMatch> {new TokenMatch("", new PositionInfo(m_line, m_column, 0, m_end_of_file))};

				return empty;
			}

			matches.Add(new TokenMatch("", new PositionInfo(m_line, m_column, 0, m_end_of_file)));

			return matches;
		}

		private bool match(string characters) { return m_source.StartsWith(characters); }

		private bool matchAndTake(string characters)
		{
			bool matches = m_source.StartsWith(characters);

			if (matches) take(characters.Length);

			return matches;
		}

		private void take(int characters)
		{
			m_source = m_source.Substring(characters);
			m_column += characters;
		}

		private void takeLine()
		{
			m_source = m_source.Substring(Environment.NewLine.Length);
			m_column = 0;
			m_line++;
		}

		private bool done() { return m_source.Length < 1; }

		public static string escape(string input)
		{
			using (StringWriter writer = new StringWriter())
			{
				using (CodeDomProvider provider = CodeDomProvider.CreateProvider("CSharp"))
				{
					provider.GenerateCodeFromExpression(new CodePrimitiveExpression(input), writer, null);
					return writer.ToString();
				}
			}
		}
	}
}