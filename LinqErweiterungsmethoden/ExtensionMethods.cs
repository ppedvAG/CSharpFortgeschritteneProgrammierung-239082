using System.Xml.Serialization;

namespace LinqErweiterungsmethoden;

public static class ExtensionMethods
{
	public static int Quersumme(this int x)
	{
		//int summe = 0;
		//string zahlAlsString = x.ToString();
		//for (int i = 0; i < zahlAlsString.Length; i++)
		//{
		//	summe += (int) char.GetNumericValue(zahlAlsString[i]);
		//}
		//return summe;

		return x.ToString().Sum(e => (int) char.GetNumericValue(e));
	}

	public static void Serialize(this XmlSerializer xml, string path, object o)
	{
		using StreamWriter sw = new(path);
		xml.Serialize(sw, o);
	}

	public static T Deserialize<T>(this XmlSerializer xml, string path)
	{
		using StreamReader sr = new StreamReader(path);
		return (T) xml.Deserialize(sr);
	}

	public static void ForEach<T>(this IEnumerable<T> x, Action<T> a)
	{
		ArgumentNullException.ThrowIfNull(a);

		foreach (T item in x)
			a(item);
	}

	public static IEnumerable<TResult> ForEach<T, TResult>(this IEnumerable<T> x, Func<T, TResult> a)
	{
		ArgumentNullException.ThrowIfNull(a);

		foreach (T item in x)
			yield return a(item); //yield return: Wirf das Ergebnis zurück, am Ende der Funktion bekommt der User eine Liste mit allen Elementen

		//List<TResult> list = new List<TResult>();
		//foreach (T item in x)
		//	list.Add(a(item));
		//return list;
	}
}