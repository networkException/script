﻿using System;
using System.Collections.Generic;
using System.Linq;
using networkScript.Error;
using networkScript.Parsing;

namespace networkScript
{
	public class Context
	{
		private readonly List<Scope> m_scopes;
		private string m_error;

		public Context()
		{
			Scope scope = new Scope();
			Object console = new Object();
			
			console.set("log", new Value((parameters, context) =>
			{
				if (parameters.Count == 1) Console.WriteLine(parameters[0].evaluate(context));
				else Console.WriteLine();
			}));
			
			console.set("error", new Value((parameters, context) =>
			{
				Console.ForegroundColor = ConsoleColor.Red;
				if (parameters.Count == 1) Console.WriteLine(parameters[0].evaluate(context));
				else Console.WriteLine();
				Console.ResetColor();
			}));
			
			console.set("warn", new Value((parameters, context) =>
			{
				Console.ForegroundColor = ConsoleColor.Yellow;
				if (parameters.Count == 1) Console.WriteLine(parameters[0].evaluate(context));
				else Console.WriteLine();
				Console.ResetColor();
			}));

			scope.assign("console", console.asValue());
			scope.assign("debug", new Value((parameters, context) =>
			{
				Console.ForegroundColor = ConsoleColor.Cyan;
				if (parameters[0].GetType() == typeof(Identifier)) Console.WriteLine(parameters[0] + ": " + parameters[0].evaluate(context));
				else Console.WriteLine(parameters[0].evaluate(context));
				Console.ResetColor();
			}));
			
			scope.assign("exec", new Value((parameters, context) =>
			{
				new Parser().parse( new Tokenizer(parameters[0].evaluate(context).asString()).tokenize()).execute(context);
			}));

			scope.assign("scope", new Value(() => new Value(current().ToString())));
			scope.assign("context", new Value(() => new Value(ToString())));

			m_scopes = new List<Scope> {scope};
		}

		public Value get(Expression expression)
		{
			string key = expression.asString(this);

			foreach (Scope scope in scopeStack().Where(scope => scope.has(key)))
				return scope.get(key);

			return Value.Undefined;
		}

		public Value assign(Expression expressions, Value value)
		{
			Value result = Value.Undefined;

			expressions.visit(expression =>
			{
				string key = expression.asString(this);

				foreach (Scope scope in scopeStack().Where(scope => scope.has(key)))
				{
					result = scope.assign(key, value);
					return;
				}

				result = current().assign(key, value);
			});

			return result;
		}

		public Value reference(Expression expressions, Value value)
		{
			Value result = Value.Undefined;

			expressions.visit(expression =>
			{
				string key = expression.asString(this);

				foreach (Scope scope in scopeStack().Where(scope => scope.has(key)))
				{
					result = scope.reference(key, ref value);
					return;
				}

				result = current().reference(key, ref value);
			});

			return result;
		}

		public Value declare(Expression expression) { return current().declare(expression.asString(this)); }

		public void enter() { m_scopes.Add(new Scope()); }

		public void leave() { m_scopes.RemoveAt(m_scopes.Count - 1); }

		public void typeError(string message) { throw new TypeError(message); }

		public void typeError(string message, Node node) { throw new TypeError(message + " at " + node.info()); }

		private Scope current() { return m_scopes.ElementAt(m_scopes.Count - 1); }

		private IEnumerable<Scope> scopeStack()
		{
			List<Scope> reverse = new List<Scope>(m_scopes);
			reverse.Reverse();
			return reverse;
		}

		public override string ToString() { return "Context(" + string.Join(", ", m_scopes) + ")"; }
	}
}