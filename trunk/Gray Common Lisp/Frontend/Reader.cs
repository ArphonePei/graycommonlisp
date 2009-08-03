using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Gray_Common_Lisp.Runtime;

// TODO: The handling of the . in lists is kind of odd.
// 		 lisp: (1 2 .3) => (1 2 0.3)
//       gcl : (1 2 .3) => (1 2 . 3)
// 		 It's the fault of the simplistic way the reader
// 		 thinks.
namespace Gray_Common_Lisp.Frontend
{
	enum PeekType
	{
		TargetChar,
		SkipWhiteSpace,
		Normal
	}

	static class Reader
	{
		private delegate object ReaderMacroDelegate(TextReader stream, char @char);

		private static readonly StringBuilder buffer = new StringBuilder();

		private static readonly Dictionary<char, ReaderMacroDelegate> readtable = new Dictionary<char, ReaderMacroDelegate>();

		internal static readonly SymbolStore symbolStore = new SymbolStore();

		public static void Init()
		{
			symbolStore.Intern(Symbol.Nil);
			symbolStore.Intern(Symbol.T);

			readtable.Add('(', (s, c) => ReadDelimitedList(s, ')', true));
		}

		public static object Read(TextReader stream)
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

			return symbolStore.Intern(token);
		}

		// NOTE: This is an implementation specific function. The library wrapper will implement
		//       the interface required by the standard.
		public static char PeekChar(TextReader stream, PeekType peekType = PeekType.Normal, char targetChar = '\x00')
		{
			if (peekType == PeekType.SkipWhiteSpace)
				SkipWhile(stream, Char.IsWhiteSpace);
			else if (peekType == PeekType.TargetChar)
				SkipWhile(stream, c => c != targetChar);

			return (char) stream.Peek();
		}

		public static object ReadDelimitedList(TextReader stream, char delimiter, bool lispList = false)
		{
			if (PeekChar(stream, PeekType.SkipWhiteSpace) == delimiter)
				return Symbol.Nil;

			if (lispList && PeekChar(stream, PeekType.SkipWhiteSpace) == '.')
			{
				stream.Read();
				var cdr = Read(stream);
				// TODO: Implement assertions class.
				if (PeekChar(stream, PeekType.SkipWhiteSpace) != delimiter)
					throw new ApplicationException("Malformed list: more than one element after `.'");

				return cdr;
			}

			return new ConsCell(Read(stream), ReadDelimitedList(stream, delimiter, lispList));
		}

		private static bool IsTokenConstituent(char c)
		{
			return !Char.IsWhiteSpace(c) && c != ')';
		}

		private static void SkipWhile(TextReader stream, Predicate<char> predicate)
		{
			while (stream.Peek() != -1 && predicate((char) stream.Peek()))
				stream.Read();
		}

		private static string ReadWhile(TextReader stream, Predicate<char> predicate)
		{
			buffer.Length = 0;

			while (stream.Peek() != -1 && predicate((char) stream.Peek()))
				buffer.Append((char) stream.Read());

			return buffer.ToString();
		}
	}
}
