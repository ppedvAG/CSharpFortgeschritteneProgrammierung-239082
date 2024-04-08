using System.Collections;

namespace Sonstiges;

internal class Program
{
	static void Main(string[] args)
	{
		Wagon w1 = new() { AnzSitze = 30, Farbe = "Rot" };
		Wagon w2 = new() { AnzSitze = 30, Farbe = "Rot" };
        Console.WriteLine(w1 == w2);

		Zug z = new();
		z += w1;
		z = z + w2;

		Zug z2 = new();
		z2 += new Wagon();

		z += z2;

		Zug z3 = w1;

		foreach (Wagon w in z) //statt z.Wagons
		{

		}

		Console.WriteLine(z[2]);
        Console.WriteLine(z[30, "Rot"]);
    }
}

public class Zug : IEnumerable<Wagon>
{
	private List<Wagon> Wagons = [];

	public IEnumerator<Wagon> GetEnumerator()
	{
		foreach (Wagon w in Wagons)
			yield return w;
	}

	IEnumerator IEnumerable.GetEnumerator() => throw new NotImplementedException();

	public Wagon this[int index]
	{
		get => Wagons[index];
		set
		{
			if (index > Wagons.Count)
				Wagons.Add(value);
			else
				Wagons[index] = value;
		}
	}

	public Wagon this[int anzSitze, string farbe]
	{
		get => Wagons.FirstOrDefault(e => e.AnzSitze == anzSitze && e.Farbe == farbe);
	}

	public static Zug operator +(Zug z, Wagon w)
	{
		z.Wagons.Add(w);
		return z;
	}

	public static Zug operator +(Zug z1, Zug z2)
	{
		z1.Wagons.AddRange(z2.Wagons);
		return z1;
	}

	/// <summary>
	/// Wagon zu Zug konvertieren
	/// </summary>
	public static implicit operator Zug(Wagon w)
	{
		return new Zug(); //w einbauen
	}
}

public class Wagon
{
	public int AnzSitze { get; set; }

	public string Farbe { get; set; }

	public static bool operator ==(Wagon a, Wagon b) => a.AnzSitze == b.AnzSitze && a.Farbe == b.Farbe;

	public static bool operator !=(Wagon a, Wagon b) => !(a == b);
}