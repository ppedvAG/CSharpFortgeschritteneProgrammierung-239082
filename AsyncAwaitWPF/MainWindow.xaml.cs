using System.Net.Http;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AsyncAwaitWPF;

public partial class MainWindow : Window
{
	public MainWindow()
	{
		InitializeComponent();
	}

	private void Button_Click(object sender, RoutedEventArgs e)
	{
		List<Task> xmlTasks = [];
		for (int i = 0; i < 10; i++)
		{
			xmlTasks.Add(Task.Run(() => Thread.Sleep((Random.Shared.Next() % 2000) + 1000))); //10 Tasks die jeweils zw. 1s und 3s warten
		}
		Task.WaitAll([.. xmlTasks]); //Blockierende Operation
		Info.Text += "Tasks fertig";
	}

	private void Button_Click_ContinueWith(object sender, RoutedEventArgs e)
	{
		for (int i = 0; i < 10; i++)
		{
			Task t = new Task(() => Thread.Sleep((Random.Shared.Next() % 2000) + 1000));
			//t.ContinueWith(x => Info.Text += "Task fertig\n"); //Nicht möglich, da Zugriffe von Hintergrundsthreads auf den Hauptthread nicht erlaubt sind
			t.ContinueWith(x => Dispatcher.Invoke(() => Info.Text += "Task fertig\n")); //Dispatcher.Invoke: Führt Code auf dem Main Thread aus
			t.Start();
		}
	}

	private async void Button_Click_Async(object sender, RoutedEventArgs e)
	{
		List<Task> xmlTasks = [];
		for (int i = 0; i < 10; i++)
		{
			xmlTasks.Add(Task.Delay((Random.Shared.Next() % 2000) + 1000)); //10 Tasks die jeweils zw. 1s und 3s warten
		}
		await Task.WhenAll([.. xmlTasks]); //Blockiert nicht
		Info.Text += "Tasks fertig";
	}

	private void Button_Click_ContinueWith_Async(object sender, RoutedEventArgs e)
	{
		for (int i = 0; i < 10; i++)
		{
			Task.Delay((Random.Shared.Next() % 2000) + 1000).ContinueWith(x => Dispatcher.Invoke(() => Info.Text += "Task fertig\n"));
		}
	}

	private async void Request(object sender, RoutedEventArgs e)
	{
		//Ohne async/await
		using HttpClient client = new();
		//Task t = Task.Run(() => client.GetAsync("http://www.gutenberg.org/files/54700/54700-0.txt"))
		//			 .ContinueWith(x => Dispatcher.Invoke(() =>
		//			 {
		//				 Task<string> c = Task.Run(() => x.Result.Content.ReadAsStringAsync());
		//				 Info.Text = "Text wird ausgelesen...";
		//				 Info.Text = c.Result;
		//			 }));
		//Info.Text = "Text wird geladen...";

		//Mit async/await
		try
		{
			Task<HttpResponseMessage> t = client.GetAsync("http://www.gutenberg.org/files/54700/54700-0.txt"); //Aufgabe starten
			Info.Text = "Text wird geladen..."; //Zwischenschritte
			HttpResponseMessage message = await t; //Auf Aufgabe warten

			Task<string> content = message.Content.ReadAsStringAsync();
			Info.Text = "Text wird ausgelesen...";
			string text = await content;

			Info.Text = text;
		}
		catch (InvalidOperationException ex)
		{
			Info.Text = ex.Message;
        }
	}
}