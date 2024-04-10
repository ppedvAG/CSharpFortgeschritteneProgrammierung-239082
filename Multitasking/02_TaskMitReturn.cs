namespace Multitasking;

public class _02_TaskMitReturn
{
	static void Main(string[] args)
	{
		Task<int> t = new Task<int>(RandomZahl);
		t.Start();

		//Console.WriteLine(t.Result); //Blockiert die Schleife

		//Lösungen: ContinueWith, await

		bool resultPrinted = false;
		for (int i = 0; i < 100; i++)
		{
			Console.WriteLine(i);

			if (t.IsCompletedSuccessfully && !resultPrinted)
			{
				Console.WriteLine(t.Result); //Verbos
				resultPrinted = true;
			}

            Thread.Sleep(20);
		}

		Task<int> t2 = Task.Run(RandomZahl);
        Console.WriteLine(t2.Result);

        Console.WriteLine(t.Result); //Kommt immer nach der Schleife

		///////////////////////////////////////////////

		Task<int> t3 = Task.Run<int>(() => Berechne(1, 2, 3)); //Funktion mit mehreren Parametern in eine Action wrappen um diese im Task benutzen zu können

		Task<int> t4 = Task.Run<int>(() =>
		{
			int erg	= Berechne(1, 2, 3);
            Console.WriteLine($"{1} + {2} + {3} = {erg}");
            return erg;
		}); //Anonymer Task mit Body
	}

	public static int RandomZahl()
	{
		Thread.Sleep(1000);
		return Random.Shared.Next();
	}

	public static int Berechne(int x, int y, int z)
	{
		Thread.Sleep(1000);
		return x + y + z;
	}
}