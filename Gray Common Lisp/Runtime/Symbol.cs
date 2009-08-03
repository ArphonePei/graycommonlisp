namespace Gray_Common_Lisp.Runtime
{
	class Symbol
	{
		public static readonly Symbol Nil = new Symbol("nil");
		public static readonly Symbol T = new Symbol("t");

		public readonly string Name;

		public Symbol(string name)
		{
			Name = name;
		}

		public override string ToString()
		{
			return Name;
		}
	}
}
