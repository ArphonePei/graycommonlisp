using System.Collections.Generic;
using Gray_Common_Lisp.Frontend;

namespace Gray_Common_Lisp.Runtime
{
	class Package
	{
		private readonly Dictionary<string, Symbol> symbols = new Dictionary<string, Symbol>();

		public readonly string Name;

		public Package(string name)
		{
			Name = name;
		}

		public Symbol Intern(string name)
		{
			// TODO This is not quite elegant. Maybe a hash table? Any better ideas?
			Symbol symbol;
			if (Reader.TryGetSpecialSymbol(name, out symbol) || symbols.TryGetValue(name, out symbol))
				return symbol;
			symbol = new Symbol(name);
			symbols.Add(name, symbol);

			return symbol;
		}

		// NOTE: This is one ugly piece of internal hackery.
		// 		 Needed for nil and t for now.
		public void InternSpecial(string name, Symbol symbol)
		{
			if (symbols.ContainsKey(name))
				symbols[name] = symbol;
			else
				symbols.Add(name, symbol);
		}

		public override string ToString()
		{
			return Name;
		}
	}
}
