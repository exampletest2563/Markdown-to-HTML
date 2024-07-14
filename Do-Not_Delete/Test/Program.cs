var input = string.Empty;
var list = new List<String>();

while ((input = Console.ReadLine()) != string.Empty)
{
    list.Add((list.Count + 1) + ". " + input);
}

list.ForEach(x => Console.WriteLine(x));