namespace AsyncAwait;

internal class Program
{
	static async Task Main(string[] args)
	{
		//async/await
		//Effektiv wie TPL, aber simpler

		//Drei Teile:
		//Aufgabe starten
		//Zwischenschritte
		//Auf Aufgabe warten

		//Ohne Async/Await
		//Task<string> t = Task.Run(() => File.ReadAllText("Pfad.txt"));
		//string output = t.Result;

		////Mit Async/Await
		//Task<string> t2 = File.ReadAllTextAsync("Pfad.txt");
		//string output2 = await t2; //Bei await kommt das Ergebnis heraus (effektiv t2.Result)

		////->
		//string output3 = await File.ReadAllTextAsync("Pfad.txt");

		////Vorteil: Blockiert nicht die GUI

		////Ohne async/await
		//List<Task> xmlTasks = [];
		//for (int i = 0; i < 10; i++)
		//{
		//	xmlTasks.Add(Task.Run(() => Thread.Sleep((Random.Shared.Next() % 2000) + 1000))); //10 Tasks die jeweils zw. 1s und 3s warten
		//}
		//Task.WaitAll([.. xmlTasks]);

		////Mit async/await
		////WICHTIG: Methode selbst muss async sein
		//List<Task> xmlTasks2 = [];
		//for (int i = 0; i < 10; i++)
		//{
		//	xmlTasks.Add(Task.Delay((Random.Shared.Next() % 2000) + 1000)); //10 Tasks die jeweils zw. 1s und 3s warten
		//	//Task.Delay: Effektiv Thread.Sleep aber awaitable
		//}
		//await Task.WhenAll(xmlTasks2); //Selbiges wie Task.WaitAll, aber blockiert nicht die GUI

		//Wird als Task gestartet
		Test();

		//Asynchroner Methodenaufruf
		Console.WriteLine("Test2 gestartet"); //1s
		await Test2();
		Console.WriteLine("Test2 fertig"); //1s

		Task<string> t = Test3();
		string ergebnis = await t;
        await Console.Out.WriteLineAsync(ergebnis);

        Console.ReadKey();
	}

	//Drei verschiedene von Async-Methoden
	//async void: Diese Methode kann await benutzen, aber selbst nicht awaited werden
	//async Task: Diese Methode kann await benutzen und kann awaited werden
	//async Task<T>: Diese Methode kann await benutzen, kann awaited werden, und hat ein Ergebnis

	public static async void Test()
	{
		Console.WriteLine("Test gestartet");
		await Task.Delay(3000);
		Console.WriteLine("Test fertig");
	}

	public static async Task Test2()
	{
		await Task.Delay(3000);
	}

	public static async Task<string> Test3()
	{
		await Task.Delay(3000);
		return "Hallo";
	}
}
