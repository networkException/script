using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace networkScript
{
	internal abstract class Statement : Node
	{
		public abstract Value execute(Context context);
	}
}