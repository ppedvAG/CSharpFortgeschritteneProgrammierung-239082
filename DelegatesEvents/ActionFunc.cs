namespace DelegatesEvents;

public class ActionFunc
{
	static void Main(string[] args)
	{
		//Action, Func
		//Vordefinierte Delegates, die in verschiedenen Stellen der Sprache eingebaut sind
		//z.B.: Linq, Multithreading-/tasking, Reflection, ...
		//Essentiell für die fortgeschrittene Programmierung

		//Action: Methode ohne Rückgabewert und bis zu 16 Parametern
		Action a = new Action(PrintRandomNumber);
		//a += PrintRandomNumber; //+= möglich, wird generell nur bei Events benötigt
		a?.Invoke();
		//a();

		//Action mit Parametern
		Action<int, int> addiere = PrintAddiere; //Kurzform zum Anhängen von Methoden
		addiere?.Invoke(4, 8);

		DoAction(4, 8, PrintAddiere);
		DoAction(3, 1, PrintSubtrahiere);
		DoAction(6, 7, addiere);

		//////////////////////////////////////////////////////////////////////////

		//Func: Methode mit Rückgabewert und bis zu 16 Parametern
		Func<int, int, double> func = Multipliziere; //WICHTIG: Letztes generisches Typargument (Generic) bestimmt den Rückgabewert
		double? x = func?.Invoke(4, 8); //Invoke benötigt einen nullable double, weil die func selbst null sein könnte

		//Alternativen
		double y = 0;
		if (func is not null)
			y = func.Invoke(4, 9);

		double z = func == null ? double.NaN : func.Invoke(3, 6);

		double b = func?.Invoke(6, 7) ?? double.NaN;

		DoFunc(4, 6, Multipliziere);
		DoFunc(4, 6, Dividiere);
		DoFunc(4, 6, func);

		//Praktische Beispiele
		List<int> ints = [];
		ints.Find(Mod2); //Hier wird ein Methodenzeiger benötigt, welcher einen int als Parameter hat und bool zurückgibt

		//Find: Führe eine Schleife aus, und verwende bei jedem Element das Predicate
		//Wenn bei einem Element true zurückkommt, gib dieses Element per return zurück

		bool Mod2(int x) => x % 2 == 0;

		ints.FindAll(Mod2);

		ints.Where(Mod2); //Auch hier kann ein Methodenzeiger übergeben werden

		//Anonyme Methoden: Methoden, für die keine separate Methode angelegt wird
		func += delegate (int x, int y) { return x + y; }; //Anonyme Methode

		func += (int x, int y) => { return x + y; }; //Kürzere Form

		func += (x, y) => { return x - y; };

		func += (x, y) => (double) x / y; //Kürzeste, häufigste Form

		//Anwendungbeispiele
		ints.Where(e => e % 2 == 0);
		DoAction(3, 6, (x, y) => Console.WriteLine(x + y));
		DoFunc(8, 4, (x, y) => (double) x % y);
	}

	#region Action
	static void PrintRandomNumber() => Console.WriteLine(Random.Shared.Next());

	static void PrintAddiere(int x, int y) => Console.WriteLine($"{x} + {y} = {x + y}");

	static void PrintSubtrahiere(int x, int y) => Console.WriteLine($"{x} - {y} = {x - y}");

	/// <summary>
	/// Bei dieser Methode kann der User über den Action Parameter frei entscheiden, was diese Methode tun soll
	/// </summary>
	static void DoAction(int x, int y, Action<int, int> action)
	{
        Console.WriteLine("DoAction Start");
        action?.Invoke(x, y);
        Console.WriteLine("DoAction Ende");
    }
	#endregion

	#region Func
	public static double Multipliziere(int x, int y) => x * y;

	public static double Dividiere(int x, int y) => x / y;

	public static double DoFunc(int x, int y, Func<int, int, double> func)
	{
		Console.WriteLine("DoFunc Start");
		return func?.Invoke(x, y) ?? double.NaN;
	}
	#endregion
}