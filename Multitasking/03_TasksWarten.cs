using System.Diagnostics;

namespace Multitasking;

public class _03_TasksWarten
{
	static void Main(string[] args)
	{
		Task t = new Task(() => Thread.Sleep(2000));
		t.Start();

		t.Wait(); //Warte hier auf den Task, blockiere den Main Thread

		Task<int> t2 = new Task<int>(() =>
		{
			Thread.Sleep(2000);
			return 123;
		});
		t2.Start();
        Console.WriteLine(t2.Result); //Result ist auch ein Wait()

		Task.WaitAll(t, t2); //Auf alle Tasks einer Liste warten
		Task.WaitAny(t, t2); //Auf den schnellsten der Tasks warten, Rückgabewert bestimmt den Index des schnellsten Tasks

		////////////////////////////////////////////////////////////////

		Stopwatch sw = Stopwatch.StartNew();
		Console.WriteLine("Tasks gestartet");

		List<Task> xmlTasks = [];
		for (int i = 0; i < 10; i++)
		{
			xmlTasks.Add(Task.Run(() => Thread.Sleep((Random.Shared.Next() % 2000) + 1000))); //10 Tasks die jeweils zw. 1s und 3s warten
		}
		Task.WaitAll([.. xmlTasks]); //[.. xmlTasks] == xmlTasks.ToArray()

		Console.WriteLine(sw.ElapsedMilliseconds);
    }
}