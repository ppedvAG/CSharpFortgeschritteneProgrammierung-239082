using System.Reflection;
using System.Text;

namespace Reflection;

internal class Program
{
	static void Main(string[] args)
	{
		//Reflection: Dynamische Analyse von Objekten zur Laufzeit
		//Geht immer von Type aus
		//Zwei Möglichkeiten: GetType(), typeof(<Typ>)

		Type pt = typeof(Program);
		Program p = new();
		Type type = p.GetType();

		//Über das Objekt gibt es sehr viele Eigenschaften
		//z.B.: Welche Methoden/Properties/Events/... hat das Objekt?
		//Ist es Public, ist es Static, ...

		//Über die entsprechenden Get-Methoden können weitere Informationen über den entsprechenden Member erlangt werden

		//Methode, die alle Properties von einem Objekt ausgibt + Typen
		Person person = new(0, "Max", "Mustermann", 33, true);
		PrintProperties(person);

		pt.GetMethod("Test", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, null); //private Methode ausführen

		//Activator
		//Gibt die Möglichkeit über ein Type Objekt ein Objekt zu erstellen
		object o = Activator.CreateInstance(type);

		//Assembly
		//Die derzeitige DLL/das derzeitige Projekt
		Assembly a = Assembly.GetExecutingAssembly(); //Das derzeitige Projekt
		Assembly loaded = Assembly.LoadFrom(@"C:\Users\lk3\source\repos\CSharp_Fortgeschritten_2024_01_08\DelegateEvents\bin\Debug\net7.0\DelegateEvents.dll");

		//Component erstellen, Events anhängen, Component ausführen
		Type compType = loaded.GetType("DelegateEvents.Component");
		object comp = Activator.CreateInstance(compType);
		compType.GetEvent("ProcessStarted").AddEventHandler(comp, () => Console.WriteLine("Prozess gestartet"));
		compType.GetEvent("ProcessEnded").AddEventHandler(comp, () => Console.WriteLine("Prozess beendet"));
		compType.GetEvent("Progress").AddEventHandler(comp, (int x) => Console.WriteLine($"Fortschritt: {x}"));
		compType.GetMethod("DoWork").Invoke(comp, null);
	}

	static void PrintProperties(object o)
	{
		Console.WriteLine
		(
			o.GetType()
				.GetProperties()
				.Aggregate(new StringBuilder(), (sb, prop) => sb.AppendLine($"{prop.PropertyType.Name} {prop.Name} {prop.GetValue(o)}"))
				.ToString()
		);
	}

	private static void Test()
	{
		Console.WriteLine("Kann nicht mit Reflection ausgeführt werden");
	}
}

public record Person(int ID, string Vorname, string Nachname, int Alter, bool IstVerheiratet);