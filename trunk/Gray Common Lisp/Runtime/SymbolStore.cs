using System.Collections.Generic;

namespace Gray_Common_Lisp.Runtime
{
	class SymbolStore
	{
		private readonly Dictionary<string, Symbol> symbols = new Dictionary<string, Symbol>();

		public Symbol Intern(string name)
		{
			Symbol symbol;
			if (symbols.TryGetValue(name, out symbol))
				return symbol;
			symbol = new Symbol(name);
			symbols.Add(name, symbol);

			return symbol;
		}

		public Symbol Intern(Symbol symbol)
		{
			if (!symbols.ContainsKey(symbol.Name))
				symbols.Add(symbol.Name, symbol);

			return symbol;
		}
	}
}
