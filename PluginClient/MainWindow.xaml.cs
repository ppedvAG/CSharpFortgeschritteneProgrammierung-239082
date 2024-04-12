using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using PluginBase;

namespace PluginClient;

public partial class MainWindow : Window
{
	public MainWindow() => InitializeComponent();

	private void LoadPlugin(object sender, RoutedEventArgs e)
	{
		OpenFileDialog dialog = new();
		dialog.Multiselect = false;
		bool? result = dialog.ShowDialog();
		if (result == true)
		{
			Assembly a = Assembly.LoadFrom(dialog.FileName);
			Type t = a.GetTypes().First(e => e.GetInterface("IPlugin") != null);
			IPlugin plugin = (IPlugin) Activator.CreateInstance(t);
			TB.Text += t.GetProperties().Aggregate(new StringBuilder(), (sb, prop) => sb.AppendLine($"{prop.Name}: {prop.GetValue(plugin)}")).ToString();
			TB.Text += t.GetMethods()
				.Where(e => e.GetCustomAttribute<ReflectionVisible>() != null)
				.Aggregate(new StringBuilder(), (sb, m) => sb.AppendLine($"{m.ReturnType} {m.Name}: ({m.GetParameters().Length})"))
				.ToString();
		}
	}
}
