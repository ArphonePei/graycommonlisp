namespace Gray_Common_Lisp.Runtime
{
	class Symbol
	{
		public readonly string Name;
		public Package Package { get; set; }

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
