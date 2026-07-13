using System.Xml.Linq;

XDocument doc = XDocument.Load("users.xml");
var users = doc.Root.Elements();

string name = users
    .First()
    .Element("firstName")!
    .Value;

Console.WriteLine(name);
