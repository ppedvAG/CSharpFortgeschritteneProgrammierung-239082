internal class Program
{
	private static void Main(string[] args)
	{
		Console.WriteLine("Hello, World!");

		var list = new List<string>();
		list.Add("Max");

		//var storeString = new DataStore<string>();

		var storeObj = new DataStore<Foo>();
		storeObj.DoSomethingSpecial(new { Baz = 33 });
		storeObj.DoSomethingSpecial(new Foo());

	}

	public class Foo : IBar
	{
		public Foo()
		{

		}

		public Foo(int count)
		{
			Count = count;
		}

		public int Count { get; set; }
	}

	public interface IBar
	{
		public int Count { get; set; }
	}

	public class DataStore<T> where T : class, IBar, new()
	{
		public T[] Data { get; set; } = new T[0];

		public DataStore()
		{

		}

		public void Add(int index, T item) //T als Parameter
		{
			Data[index] = item;
		}

		public T Get(int index)
		{
			if (Data[index].Count > 0)
			{
				return Data[index];

			}
			return new T();
		}

		public TSpecial DoSomethingSpecial<TSpecial>(TSpecial special) where TSpecial : class
		{
			Console.WriteLine(default(TSpecial));
			//Console.WriteLine(special);

			// Do awesome stuff with TSpecial

			return null;
		}

	}

	public class SpecialStore<T> : DataStore<T> where T : class, IBar, new()
	{

	}

	public class FooStore : DataStore<Foo>
	{
		public void DoSomethingFooy()
		{
			// do something special
		}
	}
}