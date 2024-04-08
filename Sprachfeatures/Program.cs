using System.Numerics;
using Point4D = (int, int, int, int);
using Group = System.Collections.Generic.IEnumerable<System.Linq.IGrouping<int, int>>;

namespace Sprachfeatures;

internal class Program
{
	int referenztyp = 10;

	static void Main(string[] args)
	{
		decimal d = 325_325_823_857.328_521_348_243m;
        Console.WriteLine(d);

        Console.WriteLine(2.3E9); //2.3 * 10^9

		//class und struct

		//class
		//Referenztyp
		//Wenn zwei Variablen von Referenztypen verglichen werden, werden die Speicheradressen (Hash Codes) verglichen
		Program p = new Program(); //Zeiger auf das unterliegende Objekt
		Program p2 = p; //Zeiger auf das Objekt unter p
		p.referenztyp = 100; //Der Wert wird hinter beiden Variablen verändert

		//struct
		//Wertetyp
		//Wenn zwei Variablen von Wertetypen verglichen werden, werden die Inhalte verglichen
		int x = 10; //Wert anlegen und in x speichern
		int y = x; //Den Wert hinter x in y kopieren
		x = 100; //y bleibt unberührt

		ref int z = ref x; //Hier wird ein Zeiger zu x hergestellt
		x = 200; //z wird auch verändert

		void Test(int x) { } //Ohne ref wird hier eine Kopie erzeugt, originaler Wert wird nicht verändert

		void Test2(ref int x) => Console.WriteLine(x);

		//Null-Coalescing Operator (??-Operator): Nimm die linke Seite, wenn diese nicht null ist, sonst die rechte Seite
		string str = null;
		int a;
		if (str == null)
			a = 0;
		else
			a = str.Length;

		a = str == null ? 0 : str.Length;

		a = str?.Length ?? 0;

		//Switch Pattern
		int b = a switch
		{
			0 => 1,
			1 => 5,
			2 => 13
		};

		string Test3() => DateTime.Now.DayOfWeek switch
		{
			DayOfWeek.Sunday => throw new NotImplementedException(),
			DayOfWeek.Monday => throw new NotImplementedException(),
			DayOfWeek.Tuesday => throw new NotImplementedException(),
			DayOfWeek.Wednesday => throw new NotImplementedException(),
			DayOfWeek.Thursday => throw new NotImplementedException(),
			DayOfWeek.Friday => throw new NotImplementedException(),
			DayOfWeek.Saturday => throw new NotImplementedException(),
			_ => throw new NotImplementedException()
		};

		static void Test4()
		{
            //Console.WriteLine(a);
        }

		//String-Interpolation ($-String): Ermöglicht, Code in einen String einzubauen
		string kombination = "Die Zahl ist: " + a + ", die Kommazahl ist: " + d + ", der String ist: " + str;
		string interpolation = $"Die Zahl ist: {{{a}}}, die Kommazahl ist: {d}, der String ist: {str}";

		Console.WriteLine(kombination);
        Console.WriteLine(interpolation);

		//Verbatim-String (@-String): Ignoriert Escape-Sequenzen
		string escape = @"\n\t\\";
		string pfad = @"C:\Program Files\dotnet\shared\Microsoft.NETCore.App\8.0.1\System.Security.AccessControl.dll";

		string tag = DateTime.Now.DayOfWeek switch
		{
			//&&, || -> and, or
			>= DayOfWeek.Monday and <= DayOfWeek.Friday => "Unter der Woche",
			DayOfWeek.Saturday or DayOfWeek.Sunday => "Wochenende"
		};

		if (DateTime.Now.DayOfWeek == DayOfWeek.Monday || DateTime.Now.DayOfWeek == DayOfWeek.Thursday || DateTime.Now.DayOfWeek == DayOfWeek.Sunday)
		{

		}

		if (DateTime.Now.DayOfWeek is DayOfWeek.Monday or DayOfWeek.Thursday or DayOfWeek.Sunday)
		{

		}

		char c = 'a';
		if (c is >= 'a' and <= 'z')
		{

		}

		Person person = new Person(1, "Max");
		//person.ID = 5;

		BigInteger big = 75983275982375983;
		big = big * big;

		List<int> ints = new();
		ints.Add(1);
		ints.Add(2);
		ints.Add(3);

		ints = new() { 1, 2, 3 };

		ints = new() { Capacity = 10 };

		Fahrzeug fzg = new();
		fzg.Name = "Hallo";
		fzg.Version = "1.0";

		// ->

		Fahrzeug fzg2 = new() { Name = "Hallo", Version = "1.0" };

		//void CheckFahrzeug(Fahrzeug f!!)
		//{
		//	if (f is null)
		//	{
		//		throw new ArgumentNullException(nameof(f));
		//	}
		//}

		//https://github.com/dotnet/runtime

		Point4D p4d = (1, 2, 3, 4);
		Group g = ints.GroupBy(e => e);
	}

	public class Fahrzeug(string name, string description, string author, string version)
	{
		public string Name { get; set; } = name;

		public string Description { get; set; } = description;

		public string Author { get; set; } = author;

		public string Version { get; set; } = version;

		public void Test()
		{
            Console.WriteLine(name);
        }
	}

	public record Person(int ID, string Name)
	{
		public void Test() { }
	}
}
