namespace DelegatesEvents;

public class Events
{
	//Event: statischer Punkt, an den eine Methode angehängt werden kann
	//Events haben immer ein Delegate dahinter (EventHandler, Action, Func, ...)
	//Events können nicht instanziert werden

	public event EventHandler TestEvent;

	//Zweiseitige Entwicklung:
	//- Entwicklerseite: Legt das Event an und führt es aus
	//- Anwenderseite: Entscheidet, was das Event tun soll, per +=

	public event EventHandler<FileSystemEventArgs> ArgsEvent;

	public event Action<int> ActionEvent;

	public event Action<int> HandlerEvent
	{
		//add, remove: Code vor +=, -= ausführen
		//Funktioniert nur mit dem event Keyword
		add { }
		remove { }
	}

	static void Main(string[] args) => new Events().Start();

	public void Start()
	{
		TestEvent += Events_TestEvent; //Anwenderseite

		//Bei Eventaufrufen IMMER ?.Invoke verwenden!
		TestEvent?.Invoke(this, EventArgs.Empty); //Entwicklerseite

		//EventHandler mit eigenem EventArgs
		ArgsEvent += Events_ArgsEvent;
		ArgsEvent?.Invoke(this, new FileSystemEventArgs(WatcherChangeTypes.Created, "Ein Ordner", "Ein File")); //Hier können Daten mitgegeben werden, die im Event verwendet werden können

		//Event mit Action als Delegate
		ActionEvent += Events_ActionEvent;
		ActionEvent?.Invoke(10);

		//Eigene EventArgs benutzen
		TestEvent?.Invoke(this, new CustomEventArgs() { Status = "Erfolg" });
	}

	private void Events_ActionEvent(int obj) => Console.WriteLine(obj);

	private void Events_ArgsEvent(object sender, FileSystemEventArgs e) => Console.WriteLine($"{e.FullPath} wurde {e.ChangeType}");

	private void Events_TestEvent(object sender, EventArgs e) => Console.WriteLine("TestEvent wurde ausgeführt");
}

public class CustomEventArgs : EventArgs
{
	public string Status { get; set; }
}