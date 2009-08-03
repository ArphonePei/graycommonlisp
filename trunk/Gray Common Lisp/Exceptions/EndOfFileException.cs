using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gray_Common_Lisp.Exceptions
{
	[global::System.Serializable]
	public class EndOfFileException : Exception
	{
		public EndOfFileException() { }
		public EndOfFileException(string message) : base(message) { }
		public EndOfFileException(string message, Exception inner) : base(message, inner) { }
		protected EndOfFileException(
		  System.Runtime.Serialization.SerializationInfo info,
		  System.Runtime.Serialization.StreamingContext context)
			: base(info, context) { }
	}
}
