namespace Multitasking;

public class _06_ContinueWith
{
    static void Main(string[] args)
    {
		//ContinueWith
		//Task verketten, wenn ein Task fertig wird, wird direkt danach der Folgetask gestartet
		Task<int> data = new Task<int>(GetWebschnittstelle);
		//Wenn der Request erfolgreich war, zeige den Inhalt an, sonst zeige eine Fehlermeldung
		data.ContinueWith(vorherigerTask => Console.WriteLine(vorherigerTask.Result), TaskContinuationOptions.OnlyOnRanToCompletion); //Erfolgstask (aber nur wenn ein Erfolg besteht)
		data.ContinueWith(vorherigerTask => Console.WriteLine(vorherigerTask.Exception!.InnerException!.Message), TaskContinuationOptions.OnlyOnFaulted); //Fehlertask (aber nur wenn ein Fehler aufgetreten ist)
		data.Start();

		////////////////////////////////////////////////////////////

		//Lade 10 XML-Files, alle 10 Files müssen erfolgreich geladen werden, sonst sollen die anderen abgebrochen werden
		CancellationTokenSource cts = new();

		List<Task> xmlTasks = [];
		for (int i = 0; i < 10; i++)
		{
			Task xml = new Task(LoadXML, new XmlTaskData(i, cts.Token));
			xml.ContinueWith(t => cts.Cancel(), TaskContinuationOptions.OnlyOnFaulted);
			xml.Start();
			xmlTasks.Add(xml);
		}

		try
		{
			Task.WaitAll([.. xmlTasks]);
		}
		catch (AggregateException e)
		{
			if (e.InnerException is FileLoadException ex)
				Console.WriteLine(ex.Message);
        }

		//////////////////////////////////////////////////////////////

		//CancellationToken mit File lesen

		//File.ReadAllText(); //Kein Token möglich

		//StreamReader sr = new StreamReader("");
		//while (!sr.EndOfStream)
		//{
		//	sr.ReadLine();
		//	//if (...)
		//}

		Console.ReadKey();
    }

	public static int GetWebschnittstelle()
	{
		Thread.Sleep((Random.Shared.Next() % 1000) + 1000);

		if (Random.Shared.Next() % 2 == 0)
			throw new HttpRequestException("Der Request war nicht erfolgreich: 404");

		return Random.Shared.Next();
	}

	public static void LoadXML(object x)
	{
		XmlTaskData t = (XmlTaskData) x;

        Console.WriteLine($"File laden gestartet: {t.Item1}");
        for (int i = 0; i < 100; i++)
		{
			Thread.Sleep(Random.Shared.Next() % 25 + 10);
			if (t.Item2.IsCancellationRequested)
			{
                Console.WriteLine($"File abgebrochen: {t.Item1}");
				t.Item2.ThrowIfCancellationRequested();
            }
		}

		if (Random.Shared.Next() % 10 == 0)
			throw new FileLoadException($"File konnte nicht geladen werden: {t.Item1}");

		Console.WriteLine($"Erfolgreich geladen: {t.Item1}");
    }
}

public record XmlTaskData(int Item1, CancellationToken Item2);