using System;
using System.Collections.Generic;
using System.Linq;
using networkScript.Declarations;
using networkScript.Expressions;
using networkScript.Statements;

namespace networkScript.Parsing
{
	internal class Parser // https://github.com/SerenityOS/serenity/blob/f3a9eba987a8346dc2c13cedac3865eee38a10a4/Libraries/LibJS/Parser.cpp
	{
		private List<TokenMatch> m_tokens;
		private List<TokenType> m_operator_precedence;
		private int m_index;

		private bool m_errored;

		public Statement parse(List<TokenMatch> tokens)
		{
			m_tokens = tokens;
			m_operator_precedence = new List<TokenType> {TokenType.Asterisk, TokenType.Slash, TokenType.Plus, TokenType.Minus};
			m_index = 0;

			BlockStatement program = new BlockStatement();

			while (!done())
			{
				if (match(TokenType.Semicolon)) consume(TokenType.Semicolon);
				else if (matchStatement()) program.append(parseStatement());
				else if (matchExpression()) program.append(new ExpressionStatement(parseExpression()));
				else
				{
					expected("Statement");
					consume();
				}
			}

			return program;
		}

		private Statement parseStatement()
		{
			if (matchExpression()) return new ExpressionStatement(parseExpression());

			switch (current().token().type())
			{
				case TokenType.CurlyOpen:
					return parseBlockStatement();
				case TokenType.For:
				{
					consume(TokenType.For);
					consume(TokenType.ParenOpen);
					Expression initializer = parseExpression();
					consume(TokenType.Semicolon);
					Expression predicate = parseExpression();
					consume(TokenType.Semicolon);
					Expression increment = parseExpression();
					consume(TokenType.ParenClose);
					return new ForStatement(initializer, predicate, increment, parseStatement());
				}
				case TokenType.If:
				{
					consume(TokenType.If);
					consume(TokenType.ParenOpen);
					Expression predicate = parseExpression();
					consume(TokenType.ParenClose);
					Statement consequent = parseStatement();

					return matchAndConsume(TokenType.Else) ? new IfStatement(predicate, consequent, parseStatement()) : new IfStatement(predicate, consequent);
				}
				case TokenType.While:
				{
					consume(TokenType.While);
					consume(TokenType.ParenOpen);
					Expression predicate = parseExpression();
					consume(TokenType.ParenClose);
					Statement consequent = parseStatement();
					return new WhileStatement(predicate, consequent);
				}
				default:
					error("Unimplemented statement parsing for " + current());
					return null;
			}
		}

		private Expression parseExpression()
		{
			Expression expression = parsePrimaryExpression();

			while (matchSecondaryExpression())
			{
				expression = parseSecondaryExpression(expression);
			}

			return expression;
		}

		private Expression parsePrimaryExpression()
		{
			switch (current().token().type())
			{
				case TokenType.ParenOpen:
					consume(TokenType.ParenOpen);
					Expression expression = parseExpression();
					consume(TokenType.ParenClose);
					return expression;
				case TokenType.Identifier:
					return parseIdentifier();
				case TokenType.StringLiteral:
					return new Value(Value.Type.String, consume(TokenType.StringLiteral));
				case TokenType.NumericLiteral:
					return new Value(Value.Type.Number, consume(TokenType.NumericLiteral));
				case TokenType.Let:
					consume(TokenType.Let);
					return new VariableDeclaration(parseIdentifiers());
				default:
					error("Unimplemented primary expression parsing for " + current());
					return null;
			}
		}

		private Expression parseSecondaryExpression(Expression primary)
		{
			switch (current().token().type())
			{
				case TokenType.Asterisk:
					consume(TokenType.Asterisk);
					return new BinaryExpression(BinaryOperation.Multiply, primary, parseExpression());
				case TokenType.Slash:
					consume(TokenType.Slash);
					return new BinaryExpression(BinaryOperation.Divide, primary, parseExpression());
				case TokenType.Plus:
					consume(TokenType.Plus);
					return new BinaryExpression(BinaryOperation.Add, primary, parseExpression());
				case TokenType.Minus:
					consume(TokenType.Minus);
					return new BinaryExpression(BinaryOperation.Subtract, primary, parseExpression());
				case TokenType.Greater:
					consume(TokenType.Greater);
					return new BinaryExpression(BinaryOperation.Greater, primary, parseExpression());
				case TokenType.Less:
					consume(TokenType.Less);
					return new BinaryExpression(BinaryOperation.Less, primary, parseExpression());
				case TokenType.Equals:
					consume(TokenType.Equals);
					return new BinaryExpression(BinaryOperation.Reference, primary, parseExpression());
				case TokenType.EqualsEquals:
					consume(TokenType.EqualsEquals);
					return new BinaryExpression(BinaryOperation.Equals, primary, parseExpression());
				case TokenType.ExclamationEquals:
					consume(TokenType.ExclamationEquals);
					return new BinaryExpression(BinaryOperation.NotEquals, primary, parseExpression());
				case TokenType.ColumnEquals:
					consume(TokenType.ColumnEquals);
					return new BinaryExpression(BinaryOperation.Assign, primary, parseExpression());
				case TokenType.PlusPlus:
					consume(TokenType.PlusPlus);
					return new PostfixExpression(PostfixOperation.Increment, primary);
				case TokenType.PlusEquals:
					consume(TokenType.PlusEquals);
					return new BinaryExpression(BinaryOperation.PlusEquals, primary, parseExpression());
				case TokenType.Period:
					consume(TokenType.Period);
					return new MemberExpression(primary, parseIdentifier());
				case TokenType.BracketOpen:
					consume(TokenType.BracketOpen);
					Expression computed = parseExpression();
					consume(TokenType.BracketClose);
					return new MemberExpression(primary, computed);
				case TokenType.ParenOpen:
					consume(TokenType.ParenOpen);
					if (matchAndConsume(TokenType.ParenClose)) return new CallExpression(primary, new List<Expression>());

					List<Expression> parameters = new List<Expression>();
					parameters.Add(parseExpression());

					while (match(TokenType.Comma))
					{
						consume(TokenType.Comma);
						parameters.Add(parseExpression());
					}

					consume(TokenType.ParenClose);

					return new CallExpression(primary, parameters);
				default:
					error("Unimplemented secondary expression parsing for " + current());
					return null;
			}
		}

		private Identifier parseIdentifier() { return new Identifier(consume(TokenType.Identifier)); }
		
		private IdentifierList parseIdentifiers()
		{
			List<Identifier> identifiers = new List<Identifier>();
			do identifiers.Add(parseIdentifier());
			while (matchAndConsume(TokenType.Comma));
			return new IdentifierList(identifiers);
		}

		private BlockStatement parseBlockStatement()
		{
			BlockStatement block = new BlockStatement();
			consume(TokenType.CurlyOpen);

			while (!done() && !match(TokenType.CurlyClose))
			{
				if (match(TokenType.Semicolon)) consume(TokenType.Semicolon);
				else if (matchStatement()) block.append(parseStatement());
				else if (matchExpression()) block.append(new ExpressionStatement(parseExpression()));
				else
				{
					expected("Statement");
					consume();
				}
			}

			consume(TokenType.CurlyClose);
			return block;
		}

		private bool matchStatement()
		{
			switch (current().token().type())
			{
				case TokenType.For:
				case TokenType.If:
				case TokenType.While:
				case TokenType.CurlyOpen:
					return true;
				default:
					return false;
			}
		}

		private bool matchExpression()
		{
			switch (current().token().type())
			{
				case TokenType.NumericLiteral:
				case TokenType.StringLiteral:
				case TokenType.ParenOpen:
				case TokenType.Identifier:
				case TokenType.Let:
					return true;
				default:
					return false;
			}
		}

		private bool matchSecondaryExpression()
		{
			switch (current().token().type())
			{
				case TokenType.Plus:
				case TokenType.Minus:
				case TokenType.Asterisk:
				case TokenType.Slash:
				case TokenType.Period:
				case TokenType.ParenOpen:
				case TokenType.BracketOpen:
				case TokenType.Greater:
				case TokenType.Less:
				case TokenType.Equals:
				case TokenType.PlusPlus:
				case TokenType.PlusEquals:
				case TokenType.EqualsEquals:
				case TokenType.ExclamationEquals:
				case TokenType.ColumnEquals:
					return true;
				default:
					return false;
			}
		}

		private bool done() { return match(TokenType.Eof); }

		private bool match(TokenType type) { return current().token().type() == type; }

		private bool match(params TokenType[] types) { return types.All(t => m_tokens[m_index + 0].token().type() == t); }

		private bool matchAndConsume(TokenType type)
		{
			if (!match(type)) return false;
			consume(type);
			return true;
		}

		private TokenMatch consume()
		{
			TokenMatch old = current();
			m_index++;
			return old;
		}

		private TokenMatch consume(TokenType type)
		{
			if (current().token().type() != type) expected(type);
			return consume();
		}

		private bool check(Type type, Node node)
		{
			if (type == node.GetType()) return true;
			
			error("Unexpected node " + node + ", expected " + type);
			return false;
		}

		private bool check(Node node, params Type[] types)
		{
			if (types.Contains(node.GetType())) return true;
			
			error("Unexpected node " + node + ", expected one of " + types.ToArray());
			return false;
		}

		private TokenMatch current() { return m_tokens[m_index]; }

		private void expected(TokenType type) { error("Unexpected token " + current() + ", expected " + type); }

		private void expected(string type) { error("Unexpected token " + current() + ", expected " + type); }

		private void error(string message)
		{
			if (m_errored) return;

			Console.WriteLine("ParseError: " + message);
			Console.WriteLine(Environment.StackTrace);

			m_errored = true;
		}
	}
}