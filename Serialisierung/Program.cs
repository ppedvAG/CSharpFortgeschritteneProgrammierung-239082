using System.Diagnostics;
//using System.Text.Json;
//using System.Text.Json.Serialization;
using System.Xml;
using System.Xml.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Serialisierung;

internal class Program
{
	static List<Fahrzeug> fahrzeuge = new()
	{
		new Fahrzeug(0, 251, FahrzeugMarke.BMW),
		new Fahrzeug(1, 274, FahrzeugMarke.BMW),
		new Fahrzeug(2, 146, FahrzeugMarke.BMW),
		new Fahrzeug(3, 208, FahrzeugMarke.Audi),
		new Fahrzeug(4, 189, FahrzeugMarke.Audi),
		new Fahrzeug(5, 133, FahrzeugMarke.VW),
		new Fahrzeug(6, 253, FahrzeugMarke.VW),
		new Fahrzeug(7, 304, FahrzeugMarke.BMW),
		new Fahrzeug(8, 151, FahrzeugMarke.VW),
		new Fahrzeug(9, 250, FahrzeugMarke.VW),
		new Fahrzeug(10, 217, FahrzeugMarke.Audi),
		new PKW(11, 125, FahrzeugMarke.Audi)
	};

	static void Main(string[] args)
	{
		XML();

		SystemJson();

		NewtonsoftJson();
	}

	public static void XML()
	{
		//XML
		XmlSerializer xml = new(fahrzeuge.GetType());
		using (StreamWriter sw = new("Test.xml"))
		{
			xml.Serialize(sw.BaseStream, fahrzeuge);
		}

		using StreamReader sr = new("Test.xml");
		List<Fahrzeug> readFzg = xml.Deserialize(sr) as List<Fahrzeug>;

		//Attribute
		//XmlIgnore: Ignoriert das Feld
		//XmlAttribute: Schreibt den Wert in die Klammer selbst als Attribut
		//XmlInclude: Vererbung

		//XML per Hand durchgehen (XmlDocument)
		XmlDocument doc = new();
		sr.BaseStream.Position = 0;
		doc.Load(sr.BaseStream);
		foreach (XmlElement element in doc.DocumentElement)
		{
			//Console.WriteLine(element["Marke"]); //Ohne Attribute
			Console.WriteLine(element.GetAttribute("Marke"));
		}
	}

	public static void SystemJson()
	{
		//System.Text.Json

		//Einstellungen vornehmen
		//JsonSerializerOptions options = new();
		//options.WriteIndented = true;
		//options.IncludeFields = true;

		////De-/Serialisieren
		//string json = JsonSerializer.Serialize(fahrzeuge, options); //WICHTIG: Options hier übergeben
		//File.WriteAllText("Test.json", json);

		//string readJson = File.ReadAllText("Test.json");
		//Fahrzeug[] fzg = JsonSerializer.Deserialize<Fahrzeug[]>(readJson, options); //WICHTIG: Options hier übergeben

		////Attribute
		////JsonIgnore
		////JsonPropertyName, JsonPropertyOrder
		////JsonDerivedType
		////JsonExtensionData: Schreibt Felder ohne dediziertes Property in ein Dictionary

		////Json per Hand durchgehen
		//JsonDocument doc = JsonDocument.Parse(json);
		//foreach (JsonElement element in doc.RootElement.EnumerateArray())
		//{
		//	Console.WriteLine(element.GetProperty("MaxV").GetInt32());
		//}
	}

	public static void NewtonsoftJson()
	{
		//Newtonsoft.Json

		//Einstellungen vornehmen
		JsonSerializerSettings settings = new();
		settings.Formatting = Newtonsoft.Json.Formatting.Indented;
		settings.TypeNameHandling = TypeNameHandling.Objects; //Vererbung ermöglichen

		//De-/Serialisieren
		string json = JsonConvert.SerializeObject(fahrzeuge, settings); //WICHTIG: Options hier übergeben
		File.WriteAllText("Test.json", json);

		string readJson = File.ReadAllText("Test.json");
		Fahrzeug[] fzg = JsonConvert.DeserializeObject<Fahrzeug[]>(readJson, settings); //WICHTIG: Options hier übergeben

		//Attribute
		//JsonIgnore
		//JsonProperty: Ermöglicht, Einstellungen zu dem Feld vorzunehmen (enthält JsonPropertName + JsonPropertyOrder von System.Text.Json)
		//JsonExtensionData

		//Json per Hand durchgehen
		JToken doc = JToken.Parse(json);
		foreach (JToken element in doc)
		{
			Console.WriteLine(element["MaxV"].Value<int>());
		}
	}
}

//[XmlInclude(typeof(Fahrzeug))]
//[XmlInclude(typeof(PKW))]

//[JsonDerivedType(typeof(Fahrzeug), "F")]
//[JsonDerivedType(typeof(PKW), "P")]

[DebuggerDisplay("Marke: {Marke}, MaxV: {MaxV}")]
public class Fahrzeug
{
	//System.Text.Json Attribute
	//[JsonIgnore]
	//[JsonPropertyName("Identifier")]
	//[JsonPropertyOrder(10)]
	public int ID { get; set; }

	//Newtonsoft.Json Attribute
	//[JsonIgnore]
	[JsonProperty(PropertyName = "Maximalgeschwindigkeit", Order = 10)]
	public int MaxV { get; set; }

	//[XmlAttribute]
	public FahrzeugMarke Marke { get; set; }

	[JsonExtensionData]
	public Dictionary<string, JToken> RemainingData { get; set; }

	public Fahrzeug(int id = 0, int maxV = 0, FahrzeugMarke marke = FahrzeugMarke.Audi)
	{
		ID = id;
		MaxV = maxV;
		Marke = marke;
	}

    public Fahrzeug() { }
}

public class PKW : Fahrzeug
{
	public PKW(int id, int maxV, FahrzeugMarke marke) : base(id, maxV, marke)
	{
	}

    public PKW()
    {
        
    }
}

public enum FahrzeugMarke { Audi, BMW, VW }