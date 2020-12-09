using System;
using System.Collections.Generic;
using System.Linq;
using networkScript.Error;
using networkScript.Expressions;
using networkScript.Parsing;

namespace networkScript {
	public class Context {
		private readonly List<Scope> m_scopes;

		public Context() {
			Scope scope = new Scope();
			Object console = new Object();
			Object test = new Object();

			test.set("me", new Value((parameters, context) => Console.WriteLine("called console.test.me")));

			console.set("log", new Value((parameters, context) => {
				if (parameters.Count == 1)
					Console.WriteLine(parameters[0].evaluate(context).asString());
				else
					Console.WriteLine();
			}));

			console.set("error", new Value((parameters, context) => {
				Console.ForegroundColor = ConsoleColor.Red;
				if (parameters.Count == 1)
					Console.WriteLine(parameters[0].evaluate(context).asString());
				else
					Console.WriteLine();
				Console.ResetColor();
			}));

			console.set("test", new Value(() => new Value(test)));

			console.set("warn", new Value((parameters, context) => {
				Console.ForegroundColor = ConsoleColor.Yellow;
				if (parameters.Count == 1)
					Console.WriteLine(parameters[0].evaluate(context).asString());
				else
					Console.WriteLine();
				Console.ResetColor();
			}));

			scope.assign("console", console.asValue());
			scope.assign("debug", new Value((parameters, context) => {
				Console.ForegroundColor = ConsoleColor.Cyan;
				if (parameters[0].GetType() == typeof(Identifier))
					Console.WriteLine(parameters[0] + ": " + parameters[0].evaluate(context).asString());
				else
					Console.WriteLine(parameters[0].evaluate(context).asString());
				Console.ResetColor();
			}));

			scope.assign("exec", new Value((parameters, context) => { new Parser().parse(new Tokenizer(parameters[0].evaluate(context).asString()).tokenize()).execute(context); }));

			scope.assign("scope", new Value(() => new Value(current().ToString())));
			scope.assign("context", new Value(() => new Value(ToString())));

			m_scopes = new List<Scope> {scope};
		}

		public Expression get(Expression expression) {
			string key = expression.evaluate(this).asString();

			foreach (Scope scope in scopeStack().Where(scope => scope.has(key))) return scope.get(key);

			return Value.Undefined;
		}
		
		public Expression get(string key) {
			foreach (Scope scope in scopeStack().Where(scope => scope.has(key))) return scope.get(key);

			return Value.Undefined;
		}

		public Value assign(Expression expressions, Value value) {
			expressions.visit(expression => {
				string key = expression.evaluate(this).asString();

				foreach (Scope scope in scopeStack().Where(scope => scope.has(key))) {
					scope.assign(key, value);
					return;
				}
			});

			return value;
		}

		public Expression reference(Expression expressions, Expression value) {
			expressions.visit(expression => {
				string key = expression.evaluate(this).asString();

				foreach (Scope scope in scopeStack().Where(scope => scope.has(key))) {
					scope.reference(key, value);
					return;
				}
			});

			return value;
		}

		public Expression declare(Expression expression) { return current().declare(expression.evaluate(this).asString()); }

		public void enter() { m_scopes.Add(new Scope()); }

		public void leave() { m_scopes.RemoveAt(m_scopes.Count - 1); }

		public void typeError(string message) { throw new TypeError(message); }

		public void typeError(string message, Node node) { throw new TypeError(message + " at " + node.info()); }

		public void memberError(string message) { throw new MemberError(message); }
		
		public void memberError(string message, Node node) { throw new MemberError(message + " at " + node.info()); }

		private Scope current() { return m_scopes.ElementAt(m_scopes.Count - 1); }

		private IEnumerable<Scope> scopeStack() {
			List<Scope> reverse = new List<Scope>(m_scopes);
			reverse.Reverse();
			return reverse;
		}

		public override string ToString() { return "Context(" + string.Join(", ", m_scopes) + ")"; }
	}
}