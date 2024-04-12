using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Serialization;

namespace LinqErweiterungsmethoden;

internal class Program
{
	static async Task Main(string[] args)
	{
		#region IEnumerable
		//Interface, welches die Basis von allen Listentypen in C# darstellt

		//Aufgaben/Effekte
		//Ermöglicht foreach auf einem Objekt zu benutzen
		//IEnumerable ist nur eine Anleitung -> Es speichert selbst keine Daten

		Enumerable.Range(1, 20); //Anleitung zum Erstellen der Werte von 1 bis 20
		Enumerable.Range(1, (int) 1E9); //Anleitung zum Erstellen der Werte von 1 bis 1E9

		//Enumerable.Range(1, (int) 1E9).ToList(); //2s, 4GB RAM
		//Wenn eine Anleitung ausgeführt wird (z.B. mit ToList() oder einer foreach-Schleife), werden die Werte erstellt

		List<int> x = [];
		x.AddRange(Enumerable.Range(1, 20)); //Anleitung wird einmal ausgeführt
		x.AddRange(Enumerable.Range(1, 20).ToList()); //Anleitung wird zweimal ausgeführt (Zwei Schleifen)

		//IEnumerator
		//Grundkomponente von Listen, ermöglicht den sequenziellen Zugriff auf die Elemente
		//3 Teile:
		//object Current: Zeiger auf das derzeitige Objekt zeigt
		//bool MoveNext(): Bewegt den Zeiger um ein Element weiter
		//void Reset(): Bewegt den Zeiger an den Anfang zurück

		IEnumerable<int> range = Enumerable.Range(1, 20);
		foreach (int i in range) //.GetEnumerator()
		{
            Console.WriteLine(i);
        }

		IEnumerator<int> enumerator = range.GetEnumerator();
		start:
        Console.WriteLine(enumerator.Current);
		bool next = enumerator.MoveNext();
		if (next)
			goto start;
		//enumerator.Reset();
		#endregion

		List<Fahrzeug> fahrzeuge = new List<Fahrzeug>
		{
			new Fahrzeug(251, FahrzeugMarke.BMW),
			new Fahrzeug(274, FahrzeugMarke.BMW),
			new Fahrzeug(146, FahrzeugMarke.BMW),
			new Fahrzeug(208, FahrzeugMarke.Audi),
			new Fahrzeug(189, FahrzeugMarke.Audi),
			new Fahrzeug(133, FahrzeugMarke.VW),
			new Fahrzeug(253, FahrzeugMarke.VW),
			new Fahrzeug(304, FahrzeugMarke.BMW),
			new Fahrzeug(151, FahrzeugMarke.VW),
			new Fahrzeug(250, FahrzeugMarke.VW),
			new Fahrzeug(217, FahrzeugMarke.Audi),
			new Fahrzeug(125, FahrzeugMarke.Audi)
		};

		#region Linq
		//Durchschnittsgeschwindigkeit aller Fahrzeuge
		double avg = 0;
		foreach (Fahrzeug f in fahrzeuge)
			avg += f.MaxV;
		Console.WriteLine(avg / fahrzeuge.Count);

		//Mit Linq
        Console.WriteLine(fahrzeuge.Average(e => e.MaxV));
        fahrzeuge.Min(e => e.MaxV);
        fahrzeuge.Max(e => e.MaxV);
        fahrzeuge.Sum(e => e.MaxV);

		fahrzeuge.First();
		fahrzeuge.Last();

		fahrzeuge.First(e => e.MaxV >= 250); //Wenn kein Ergebnis zurückkommt, kommt eine Exception heraus
		fahrzeuge.FirstOrDefault(e => e.MaxV >= 250); //Wenn kein Ergebnis zurückkommt, kommt hier null zurück

		//OrderBy-/Descending
		fahrzeuge.OrderBy(e => e.Marke).ThenBy(e => e.MaxV);
		fahrzeuge.OrderByDescending(e => e.Marke).ThenByDescending(e => e.MaxV);

		fahrzeuge.Where(e => e.MaxV >= 250).OrderByDescending(e => e.MaxV);

		//Predicate, Selector
		//Linq Funktionen mit einem Predicate benötigen eine Bedingung die einen bool zurückgibt (z.B. Where, Count, All/Any, ...)
		//Linq Funktionen mit einem Selector benötigen ein Feld (z.B. OrderBy, Min, Max, Average, Sum, Select, ...)

		//Aufgabe: Wieviele BMWs haben wir?
		fahrzeuge.Where(e => e.Marke == FahrzeugMarke.BMW).Count();
		fahrzeuge.Count(e => e.Marke == FahrzeugMarke.BMW);

		fahrzeuge.All(e => e.MaxV >= 250); //Fahren alle Fahrzeuge mind. 250km/h?
		fahrzeuge.Any(e => e.MaxV >= 250); //Fährt ein Fahrzeug mind. 250km/h?

		fahrzeuge.Min(e => e.MaxV); //Die kleinste Geschwindigkeit
		fahrzeuge.MinBy(e => e.MaxV); //Das Objekt mit der kleinsten Geschwindigkeit

		fahrzeuge.Skip(3).Take(3);

		//Aufgabe: Finde die 3 schnellsten Fahrzeuge
		fahrzeuge
			.OrderByDescending(e => e.MaxV)
			.Take(3);

		//Aufgabe: Finde die 3 schnellsten VWs
		fahrzeuge
			.Where(e => e.Marke == FahrzeugMarke.VW)
			.OrderByDescending(e => e.MaxV)
			.Take(3);

		Console.WriteLine(new int[] { 1, 2, 3 }.SequenceEqual([1, 3, 2])); //false
		Console.WriteLine(new int[] { 1, 2, 3 }.SequenceEqual([1, 2, 3])); //true

		fahrzeuge.Except(fahrzeuge);
		fahrzeuge.Intersect(fahrzeuge);

		fahrzeuge.Chunk(5); //Arrays mit 5, 5, 2 Elementen

		//GroupBy
		fahrzeuge.GroupBy(e => e.Marke); //3 Gruppen: Audi-Gruppe, BMW-Gruppe, VW-Gruppe

		fahrzeuge.GroupBy(e => e.Marke).Single(e => e.Key == FahrzeugMarke.VW); //Einzelne Gruppe angreifen
		fahrzeuge
			.GroupBy(e => e.Marke)
			.Single(e => e.Key == FahrzeugMarke.VW)
			.AsEnumerable(); //IGrouping<FahrzeugMarke, Fahrzeug> -> IEnumerable<Fahrzeug>

		Dictionary<FahrzeugMarke, List<Fahrzeug>> dict = fahrzeuge.GroupBy(e => e.Marke).ToDictionary(e => e.Key, e => e.ToList());

		fahrzeuge.GroupBy(e => e.Marke).ToLookup(e => e.Key);

		//Select
		//Transformiert die Liste in eine andere Form

		//Zwei Hauptanwendungen:
		//- Entnehmen eines einzelnen Felds
		//- Liste umwandeln

		//Aufgabe: Finde alle Fahrzeugmarken
		fahrzeuge
			.Select(e => e.Marke)
			.Distinct();

		fahrzeuge
			.Select(e => e.MaxV)
			.Average();

		fahrzeuge.Average(e => e.MaxV);

		//Liste umwandeln
		//Aus einem Ordner alle Dateien entnehmen ohne Endung und Pfad
		string[] pfade = Directory.GetFiles("C:\\Windows");
		List<string> pfadeOhneEndung = new();
		foreach (string p in pfade)
			pfadeOhneEndung.Add(Path.GetFileNameWithoutExtension(p));

		List<string> p2 = Directory.GetFiles("C:\\Windows").Select(e => Path.GetFileNameWithoutExtension(e)).ToList();
		List<string> p3 = Directory.GetFiles("C:\\Windows").Select(Path.GetFileNameWithoutExtension).ToList();

        Console.WriteLine(pfadeOhneEndung.SequenceEqual(p2));

		//Liste casten
		//Aufgabe: Eine double Liste erstellen von 0 bis 10 mit 0.5 Schritten
		Enumerable.Range(0, 10).Select(e => (double) e); //Ganze Liste von int zu double casten
		Enumerable.Range(0, 21).Select(e => e / 2.0); //Gehe die Liste durch und führe den Code in Select für jedes Element aus

		//Liste von Halbe + Liste von Viertel
		Enumerable.Range(0, 21).Select(e => e / 2.0).Concat(Enumerable.Range(0, 41).Select(e => e / 4.0));

		//Aggregate
		//Gehe über die Liste drüber und führe für jedes Element eine Funktion aus, wobei diese Funktion ein Ergebnis zurückgeben muss
		//Das Ergebnis kann in den Aggregator gespeichert werden

		//Das Produkt von X bis Y
		Enumerable.Range(1, 10).Aggregate(1, (agg, i) => agg * i);

		int produkt = 1;
		foreach (int i in Enumerable.Range(1, 10))
			produkt = produkt * i;
        Console.WriteLine(produkt);

		//Output einer Liste erzeugen
		StringBuilder sb = fahrzeuge.Aggregate(new StringBuilder(), (sb, fzg) =>
			sb.AppendLine($"Das Fahrzeug hat die Marke {fzg.Marke} und kann maximal {fzg.MaxV}km/h fahren."));

		Console.WriteLine(sb.ToString());

		Console.WriteLine(string.Join("\n", fahrzeuge.Select(e => $"Das Fahrzeug hat die Marke {e.Marke} und kann maximal {e.MaxV}km/h fahren.")));
		#endregion

		#region Erweiterungsmethoden
		int zahl = 3825;
		zahl.Quersumme();

		Console.WriteLine(32857.Quersumme());

		Console.WriteLine(ExtensionMethods.Quersumme(38957)); //Compiler generiert diesen Code im Hintergrund

		//Enumerable.Where(fahrzeuge, e => e.MaxV >= 200);

		//XmlSerializer xml = new(fahrzeuge.GetType());
		//xml.Serialize("Test.xml", fahrzeuge);

		//xml.Deserialize<List<Fahrzeug>>("Test.xml");

		fahrzeuge.ToArray().ForEach(Console.WriteLine);

		//await foreach: Warte auf den nächsten Wert, und führe den Inhalt der Schleife aus
		await foreach (int i in GetTest())
		{
			Console.WriteLine(i);
		}
		#endregion
	}

	/// <summary>
	/// IEnumerable: Gib mir das jetztige Element und gehe danach einen Schritt weiter
	/// IAsyncEnumerable: Gib mir das jetztige Element und warte danach auf den nächsten Schritt
	/// 
	/// Beispiel: Livestream
	/// </summary>
	public static async IAsyncEnumerable<int> GetTest()
	{
		//Aufgabe: Gib Random Zahlen nach einer Wartezeit zurück
		while (true)
		{
			await Task.Delay(Random.Shared.Next(500) + 500);
			yield return Random.Shared.Next();
		}
	}
}

[DebuggerDisplay("Marke: {Marke}, MaxV: {MaxV}")]
public class Fahrzeug
{
	public Fahrzeug(int maxV, FahrzeugMarke marke)
	{
		MaxV = maxV;
		Marke = marke;
	}

	public int MaxV { get; set; }

	public FahrzeugMarke Marke { get; set; }
}

public enum FahrzeugMarke { Audi, BMW, VW }