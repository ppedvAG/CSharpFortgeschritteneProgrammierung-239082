using System.Collections.Concurrent;

namespace Multitasking;

public class _09_ConcurrentCollections
{
    static void Main(string[] args)
    {
		//NuGet: System.ServiceModel.Primitives
		SynchronizedCollection<int> x = new SynchronizedCollection<int>(); //Wie List, aber mit einem Lock-Block um jede Methode herum
		x.Add(1);
		Console.WriteLine(x[0]);

		ConcurrentBag<int> y = new(); //"Haufen von Objekten", muss mit Linq verarbeitet werden
		y.Add(2);

		ConcurrentDictionary<string, int> dict = new();
		dict.AddOrUpdate("Eins", 1, (str, i) => i + dict[str]); //Fügt einfach hinzu wenn der Key nicht existiert, führt die Func aus, wenn der Key existiert

		dict.TryAdd("Zwei", 2); //Statt Add beim normalen Dictionary

        Console.WriteLine(dict["Eins"]);
	}
}