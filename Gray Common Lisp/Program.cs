using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Gray_Common_Lisp.Frontend;

namespace Gray_Common_Lisp
{
	class Program
	{
		static void Main(string[] args)
		{
			Reader.Init();

			while (true)
			{
				Console.Write("gcl> ");
				var obj = Reader.Read(new StringReader(Console.ReadLine()));
				Console.WriteLine("{0} ; {1}", obj, obj.GetType());
			}
		}
	}
}
