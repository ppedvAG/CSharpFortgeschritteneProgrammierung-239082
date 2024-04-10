namespace Multitasking;

public class _05_Exceptions
{
	static void Main(string[] args)
	{
		try
		{
			Task t = Task.Run(Run); //Tasks "verschlucken" ihre Exceptions -> AggregateException
			t.Wait();
		}
		catch (AggregateException ex)
		{
			Console.WriteLine(ex.InnerException?.Message);
			Console.WriteLine(ex.InnerException?.StackTrace);
		}


		try
		{
			List<Task> xmlTasks = [];
			for (int i = 0; i < 10; i++)
			{
				xmlTasks.Add(Task.Run(Run)); //10 Tasks die jeweils zw. 1s und 3s warten
			}
			Task.WaitAll([.. xmlTasks]); //Problem: Die Fehler werden erst am Ende alle gemeinsam ausgegeben, und nicht während die Tasks abstürzen -> ContinueWith
		}
		catch (AggregateException ex)
		{
			foreach (Exception e in ex.InnerExceptions)
			{
                Console.WriteLine(e.Message);
				Console.WriteLine(e.StackTrace);
                Console.WriteLine("----------------------------------");
            }
		}


		Console.ReadKey();
	}

	public static void Run()
	{
		Thread.Sleep(500);
		throw new InvalidDataException();
	}
}