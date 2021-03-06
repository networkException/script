﻿using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace networkScript.Parsing {
	internal class Tokenizer {
		private readonly List<Token> m_definitions;

		private bool m_line_comment;
		private bool m_block_comment;

		private readonly Stack<StringToken> m_string_token_stack;
		private string m_string_literal;

		private int m_line;
		private int m_column;

		private string m_source;
		private readonly List<TokenMatch> m_matches;

		public Tokenizer(string source) {
			m_definitions = new List<Token> {
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
				new Token(TokenType.DoubleLiteral, "^\\d+\\.\\d+"),
				new Token(TokenType.IntLiteral, "^(\\d+)"),
				new Token(TokenType.BooleanLiteral, "^(?:true|false)"),
				new Token(TokenType.TypeDefinition, "^: +([A-Za-z0-9_]+)"),
				new Token(TokenType.Identifier, "^[A-Za-z0-9_]+"),
				new Token(TokenType.Equals, "^="),
				new Token(TokenType.Plus, "^\\+")
			};

			m_line_comment = false;
			m_block_comment = false;

			m_string_token_stack = new Stack<StringToken>();
			m_string_token_stack.Push(StringToken.None);
			m_string_literal = string.Empty;

			m_line = 0;
			m_column = 0;

			m_source = Regex.Replace(source, @"\r\n|\n\r|\n|\r|\t", Environment.NewLine);
			m_matches = new List<TokenMatch>();
		}

		public List<TokenMatch> tokenize() {
			while (!done()) {
				if (match(Environment.NewLine)) {
					if (m_line_comment) m_line_comment = false;

					takeLine();
					continue;
				}

				if (!done() && !inString() && matchAndTake(" ")) continue;

				if (m_block_comment || m_line_comment) {
					take(1);
					continue;
				}

				if (matchAndTake("//")) {
					m_line_comment = true;
					continue;
				}

				if (matchAndTake("/*")) {
					m_block_comment = true;
					continue;
				}

				if (matchAndTake("*/")) {
					m_block_comment = false;
					continue;
				}

				if (!inString() && matchAndTake("'")) {
					pushStringToken(StringToken.SingleQuote);
					continue;
				}

				if (peekStringToken() == StringToken.SingleQuote && matchAndTake("\\'")) {
					m_string_literal += "'";
					continue;
				}

				if (peekStringToken() == StringToken.SingleQuote && matchAndTake("'")) {
					match(m_string_literal, TokenType.StringLiteral);
					popStringToken();
					m_string_literal = string.Empty;
					continue;
				}

				if (!inString() && matchAndTake("\"")) {
					pushStringToken(StringToken.DoubleQuote);
					continue;
				}

				if (peekStringToken() == StringToken.DoubleQuote && matchAndTake("\\\"")) {
					m_string_literal += "\"";
					continue;
				}

				if (peekStringToken() == StringToken.DoubleQuote && matchAndTake("\"")) {
					match(m_string_literal, TokenType.StringLiteral);
					popStringToken();
					m_string_literal = string.Empty;
					continue;
				}

				if (inString() && matchAndTake("\\\\")) {
					m_string_literal += "\\";
					continue;
				}

				if (inString() && match("\\${")) {
					m_string_literal += take("\\${");
					continue;
				}

				if (inString() && matchAndTake("${")) {
					match(m_string_literal, TokenType.StringLiteral);
					m_string_literal = string.Empty;
					pushStringToken(StringToken.Template);
					match("${", TokenType.TemplateOpen);
					continue;
				}

				if (inTemplate() && matchAndTake("}")) {
					popStringToken();
					match("}", TokenType.TemplateClose);
					continue;
				}

				if (inString()) {
					m_string_literal += take(1);
					continue;
				}

				bool matched = false;

				foreach (Token token in m_definitions) {
					MatchCollection tokenMatch = token.match(m_source);

					if (tokenMatch.Count != 1 || !tokenMatch[0].Success) continue;

					matched = true;
					int length = tokenMatch[0].Value.Length;
					string value = tokenMatch[0].Groups.Count == 2 ? tokenMatch[0].Groups[1].Value : tokenMatch[0].Value;

					match(value, length, token.type());
					take(length);
					break;
				}

				if (matched) continue;

				break;
			}

			match("", TokenType.Eof);

			return m_matches;
		}

		private bool match(string characters) { return m_source.StartsWith(characters); }

		private bool matchAndTake(string characters) {
			bool matches = m_source.StartsWith(characters);

			if (matches) take(characters.Length);

			return matches;
		}

		private string take(int characters) {
			string taken = m_source.Substring(0, characters);

			m_source = m_source.Substring(characters);
			m_column += characters;
			return taken;
		}

		private string take(string characters) { return take(characters.Length); }

		private void takeLine() {
			m_source = m_source.Substring(Environment.NewLine.Length);
			m_column = 0;
			m_line++;
		}

		private void match(string value, TokenType token) { match(value, value.Length, token); }

		private void match(string value, int lenght, TokenType token) {
			TokenMatch match = new TokenMatch(value, new LocationInfo(m_line, m_column, m_column + lenght, token));
			Console.WriteLine(match);
			m_matches.Add(match);
		}

		private bool inTemplate() { return m_string_token_stack.Peek() == StringToken.Template; }

		private bool inString() { return m_string_token_stack.Peek() == StringToken.SingleQuote || m_string_token_stack.Peek() == StringToken.DoubleQuote; }

		private void pushStringToken(StringToken token) { m_string_token_stack.Push(token); }
		private void popStringToken() { m_string_token_stack.Pop(); }

		private StringToken peekStringToken() { return m_string_token_stack.Peek(); }

		private bool done() { return m_source.Length < 1; }

		public static string escape(string input) {
			using (StringWriter writer = new StringWriter()) {
				using (CodeDomProvider provider = CodeDomProvider.CreateProvider("CSharp")) {
					provider.GenerateCodeFromExpression(new CodePrimitiveExpression(input), writer, null);
					return writer.ToString();
				}
			}
		}
	}
}