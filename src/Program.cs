using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using networkScript.Parsing;

namespace networkScript
{
	internal static class Program
	{
		// ReSharper disable once InconsistentNaming
		private static void Main(string[] args)
		{
			Stopwatch tokenizerWatch = new Stopwatch();
			tokenizerWatch.Start();

			Tokenizer tokenizer = new Tokenizer(File.ReadAllText("./src/index.nscript"));

			List<TokenMatch> tokens = tokenizer.tokenize();

			tokenizerWatch.Stop();

			Console.WriteLine();
			Console.WriteLine("Tokenized source in " + tokenizerWatch.ElapsedMilliseconds + " ms (" + tokenizerWatch.ElapsedTicks + " ticks)");
			Console.WriteLine();

			Stopwatch parserWatch = new Stopwatch();
			parserWatch.Start();

			Parser parser = new Parser();
			Statement program = parser.parse(tokens);

			parserWatch.Stop();

			program.dump(0);

			Console.WriteLine();
			Console.WriteLine("Parsed tokens in " + parserWatch.ElapsedMilliseconds + " ms (" + parserWatch.ElapsedTicks + " ticks)");
			Console.WriteLine();

			Stopwatch interpreterWatch = new Stopwatch();
			interpreterWatch.Start();

			Context context = new Context();
			program.execute(context);

			interpreterWatch.Stop();

			Console.WriteLine();
			Console.WriteLine("Interpreted tree in " + interpreterWatch.ElapsedMilliseconds + " ms (" + interpreterWatch.ElapsedTicks + " ticks)");
		}
	}
}