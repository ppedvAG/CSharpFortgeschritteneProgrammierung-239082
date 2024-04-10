using System.Security.Cryptography;

namespace Multitasking;

public class _04_TaskBeenden
{
	static void Main(string[] args)
	{
		//CancellationToken
		CancellationTokenSource cts = new(); //Tokenproduktion, Quelle der Tokens
		CancellationToken token = cts.Token; //Einzelne Tokens erzeugen

		Task t = new Task(Run, token);
		t.Start();

		Thread.Sleep(500);

		cts.Cancel(); //Canceln wird immer auf der Source durchgeführt

		Console.ReadKey();
	}

	public static void Run(object o)
	{
		if (o is CancellationToken ct)
		{
			for (int i = 0; i < 100; i++)
			{
				//if (ct.IsCancellationRequested)
				ct.ThrowIfCancellationRequested(); //Wirft eine Exception, welche den Task zum Absturz bringt
				Console.WriteLine(i);
				Thread.Sleep(20);
			}
		}
	}
}