namespace Multitasking;

public class _08_Lock
{
	static int Counter = 0;
	static object LockObject = new object();

	static void Main(string[] args)
	{
		for (int i = 0; i < 50; i++)
		{
			Task.Run(CounterIncrement);
		}
		Console.ReadLine();
	}

	static void CounterIncrement()
	{
		for (int i = 0; i < 100; i++)
		{
			//Tasks bleiben hier stehen, bis das Lock wieder freigegeben wird
			lock (LockObject)
			{
				Counter++;
				Console.WriteLine(Counter);
			}

			Monitor.Enter(LockObject);
			Counter++;
			Console.WriteLine(Counter);
			Monitor.Exit(LockObject);

			Interlocked.Add(ref Counter, 1);
		}
	}
}