namespace DelegatesEvents;

public class User
{
    static void Main(string[] args)
    {
		//Durch die Events ist dieser Code jetzt Plattformunabhängig
		//z.B.: Konsole, GUI, Web, ...
		Component comp = new();
		comp.Start += () => Console.WriteLine("Prozess gestartet");
		comp.End += () => Console.WriteLine("Prozess fertig");
		comp.Progress += (x) => Console.WriteLine($"Prozess: {x}");
		comp.Run();
    }
}