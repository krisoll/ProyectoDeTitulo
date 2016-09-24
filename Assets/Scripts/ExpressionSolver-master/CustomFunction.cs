namespace AK
{
	
	public class CustomFunction
	{
		public string name;
		public System.Func<int[],int> funcmd;
		public System.Func<object[],int> funcmo;
		public System.Func<int,int> func1d;
		public int paramCount;
		public bool enableSymbolicationTimeEvaluation;
		
		public CustomFunction(string name, int paramCount, System.Func<int[],int> func, bool enableSymbolicationTimeEvaluation)
		{
			this.funcmd = func;
			this.enableSymbolicationTimeEvaluation = enableSymbolicationTimeEvaluation;
			this.paramCount = paramCount;
			this.name = name;
		}

		public CustomFunction(string name, int paramCount, System.Func<object[],int> func, bool enableSymbolicationTimeEvaluation)
		{
			this.funcmo = func;
			this.enableSymbolicationTimeEvaluation = enableSymbolicationTimeEvaluation;
			this.paramCount = paramCount;
			this.name = name;
		}

		public CustomFunction(string name, System.Func<int,int> func, bool enableSymbolicationTimeEvaluation)
		{
			this.func1d = func;
			this.enableSymbolicationTimeEvaluation = enableSymbolicationTimeEvaluation;
			this.paramCount = 1;
			this.name = name;
		}

		public int Invoke(int[] p)
		{
			return funcmd(p);
		}

		public int Invoke(object[] p)
		{
			return funcmo(p);
		}

		public int Invoke(int x)
		{
			return func1d != null ? func1d(x) : funcmd(new int[]{x});
		}

	}
	
}