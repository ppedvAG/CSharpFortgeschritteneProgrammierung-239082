namespace Multitasking;

internal class _01_TaskStarten
{
	static void Main(string[] args)
	{
		Task t = new Task(RandomZahl);
		t.Start(); //Diese Methode wird im Hintergrund ausgeführt

		Task.Factory.StartNew(RandomZahl); //Führt Start automatisch aus (ab .NET Framework 4.0)

		Task.Run(RandomZahl); //Führt Start automatisch aus (ab .NET Framework 4.5)

		//Task mit Parameter
		Task t2 = new Task(Schleife, 200);
		t2.Start();

		for (int i = 0; i < 100; i++)
		{
            Console.WriteLine(i);
			Thread.Sleep(20);
        }

		//t2 wird mittendrin abgebrochen
		//Tasks sind Hintergrundthreads, Main Thread ist ein Vordergrundthread

		Console.ReadKey(); //Beenden des Main Threads aufhalten
	}

	public static void RandomZahl()
	{
		Thread.Sleep(1000);
        Console.WriteLine(Random.Shared.Next());
    }

	public static void Schleife(object maximum)
	{
		if (maximum is int max)
		{
			for (int i = 0; i < max; i++)
			{
				Console.WriteLine($"Task: {i}");
				Thread.Sleep(20);
			}
		}
	}
}
