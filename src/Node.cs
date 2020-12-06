using System;
using networkScript.Parsing;

namespace networkScript
{
	public abstract class Node
	{
		protected PositionInfo m_info;
		
		public virtual void dump(int indent) { dumpString(GetType().Name, indent); }

		public Node info(PositionInfo info)
		{
			m_info = info;
			return this;
		}
		public PositionInfo info() { return m_info; }

		protected static void dumpString(string value, int indent) { Console.WriteLine(new string(' ', indent * 2) + value); }
	}
}