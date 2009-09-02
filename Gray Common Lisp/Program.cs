using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Gray_Common_Lisp.Frontend;
using Gray_Common_Lisp.Backend;

namespace Gray_Common_Lisp
{
	class Program
	{
		static void Main(string[] args)
		{
			var interpreter = new Interpreter();

			while (true)
			{
				Console.Write("gcl> ");
				var obj = interpreter.Eval(Console.ReadLine());
				Console.WriteLine("{0} ; {1}", obj, obj.GetType());
			}
		}
	}
}
