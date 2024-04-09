namespace DelegatesEvents;

internal class Program
{
	public delegate void Vorstellung(string name); //Definition eines Delegates (eigener Typ wie Enum, Interface, ...)

	static void Main(string[] args)
	{
		Vorstellung v = new Vorstellung(VorstellungDE); //Erstellung eines Delegates mit Initialmethode

		v("Max"); //Ausführung eines Delegates

		v += VorstellungDE; //Weitere Methode anhängen mit +=
		v("Tim");

		v += VorstellungEN;
		v += VorstellungEN;
		v += VorstellungEN;
		v += VorstellungEN;
		v("Udo");

		v -= VorstellungDE; //Methode abnehmen
		v -= VorstellungDE;
		v("Max");

		v -= VorstellungEN;
		v -= VorstellungEN;
		v -= VorstellungEN;
		v -= VorstellungEN;
		//v("Tim"); //v ist null

		//Immer einen Null-Check vor Ausführung eines Delegates durchführen
		if (v is not null)
			v("Max");

		v?.Invoke("Udo"); //Null-Propagation: Führe den Code nach dem Fragezeichen nur aus, wenn das Objekt nicht null ist

		List<int> ints = null;
		ints?.Add(1);

		//Delegate durchgehen
		foreach (Delegate dg in v.GetInvocationList())
		{
            Console.WriteLine(dg.Method.Name);
        }
	}

	public static void VorstellungDE(string name)
	{
        Console.WriteLine($"Hallo mein Name ist {name}");
    }

	public static void VorstellungEN(string name)
	{
		Console.WriteLine($"Hello my name is {name}");
	}
}
