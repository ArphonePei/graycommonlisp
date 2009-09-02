using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gray_Common_Lisp.Frontend;
using Gray_Common_Lisp.Runtime;
using System.IO;

namespace Gray_Common_Lisp.Backend
{
	class Interpreter
	{
		private Reader reader;
		internal Package package;

		public Interpreter()
		{
			reader = new Reader(this);
			package = new Package("COMMON-LISP-USER");
			package.InternSpecial("nil", null);
			package.Intern("t");
		}

		public object Eval(string source)
		{
			return reader.Read(new StringReader(source));
		}
	}
}
