using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gray_Common_Lisp.Runtime
{
	class ConsCell
	{
		internal object car;
		internal object cdr;

		public ConsCell(object car, object cdr)
		{
			this.car = car;
			this.cdr = cdr;
		}

		public override string ToString()
		{
			return String.Format("({0} . {1})", car, cdr);
		}
	}
}
