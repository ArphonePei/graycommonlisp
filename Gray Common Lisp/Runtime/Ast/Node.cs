using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gray_Common_Lisp.Runtime.Ast
{
	abstract class Node
	{
	}

	sealed class ConstantNode : Node
	{
		public readonly object Value;

		public ConstantNode(object value)
		{
			Value = value;
		}
	}

	sealed class VariableBindingNode : Node
	{
		public readonly string Name;
	}
}
