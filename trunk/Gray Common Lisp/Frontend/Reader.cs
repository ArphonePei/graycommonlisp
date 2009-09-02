using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Gray_Common_Lisp.Runtime;
using Gray_Common_Lisp.Backend;

using Readtable = System.Collections.Generic.Dictionary<char, Gray_Common_Lisp.Frontend.Reader.ReaderMacroDelegate>;

namespace Gray_Common_Lisp.Frontend
{
	enum PeekType
	{
		TargetChar,
		SkipWhiteSpace,
		Normal
	}

	class Reader
	{
		public delegate object ReaderMacroDelegate(TextReader stream, char @char);

		private static Dictionary<string, Symbol> SpecialSymbols = new Dictionary<string, Symbol>()
		{
			{ "nil", null },
			{ "t", new Symbol("t") },
			{ ".", new Symbol(".") },
		};

		public static Symbol Nil = null;
		public static Symbol T = new Symbol("T");
		public static Symbol Dot = new Symbol(".");

		private readonly StringBuilder buffer = new StringBuilder();
		private readonly Interpreter interpreter;

		private readonly Readtable readtable = new Readtable();

		public Reader(Interpreter interpreter)
		{
			this.interpreter = interpreter;
			readtable.Add('(', (s, c) => ReadDelimitedList(s, ')', true));
		}

		public object Read(TextReader stream)
		{
			ReaderMacroDelegate macro;
			if (readtable.TryGetValue(PeekChar(stream, PeekType.SkipWhiteSpace), out macro))
				return macro(stream, (char) stream.Read());

			var token = ReadWhile(stream, IsTokenConstituent);

			int intValue;
			if (Int32.TryParse(token, System.Globalization.NumberStyles.Integer, System.Globalization.CultureInfo.InvariantCulture, out intValue))
				return intValue;

			double doubleValue;
			if (Double.TryParse(token, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out doubleValue))
				return doubleValue;

			// TODO: Check for dot context error
			return interpreter.package.Intern(token);
		}

		// NOTE: This is an implementation specific function. The library wrapper will implement
		//       the interface required by the standard.
		public char PeekChar(TextReader stream, PeekType peekType = PeekType.Normal, char targetChar = '\x00')
		{
			if (peekType == PeekType.SkipWhiteSpace)
				SkipWhile(stream, Char.IsWhiteSpace);
			else if (peekType == PeekType.TargetChar)
				SkipWhile(stream, c => c != targetChar);

			return (char) stream.Peek();
		}

		private object ReadDelimitedList(TextReader stream, char delimiter, bool lispList = false)
		{
			if (PeekChar(stream, PeekType.SkipWhiteSpace) == delimiter)
				return Nil;
			var car = Read(stream);

			if (lispList && car.Equals(Dot))
			{
				stream.Read();
				var cdr = Read(stream);
				// TODO: Implement assertions class.
				if (PeekChar(stream, PeekType.SkipWhiteSpace) != delimiter)
					throw new ApplicationException("Malformed list: more than one element after `.'");

				return cdr;
			}

			return new ConsCell(car, ReadDelimitedList(stream, delimiter, lispList));
		}

		private bool IsTokenConstituent(char c)
		{
			return !Char.IsWhiteSpace(c) && c != ')';
		}

		private void SkipWhile(TextReader stream, Predicate<char> predicate)
		{
			while (stream.Peek() != -1 && predicate((char) stream.Peek()))
				stream.Read();
		}

		private string ReadWhile(TextReader stream, Predicate<char> predicate)
		{
			buffer.Length = 0;

			while (stream.Peek() != -1 && predicate((char) stream.Peek()))
				buffer.Append((char) stream.Read());

			return buffer.ToString();
		}

		public static bool TryGetSpecialSymbol(string name, out Symbol symbol)
		{
			return SpecialSymbols.TryGetValue(name, out symbol);
		}
	}
}
