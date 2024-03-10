using InfluxDbDemo;

const string host = "http://localhost:8086";
const string token = "FvmX6K-EsePzBlay8zPqgzTHmGOuDchSEK_SHIIWZKTD_-8cd0X7C8nc2aKvm9rm92xVixXrI0THqEtWaN7_-Q==";
const string database = "DefaultBucket";
const string organization = "DemoOrg";
string[] commandList = {"P", "L", "Q", "X"};
string? command;


while(true)
{
    Console.Clear();
    Console.WriteLine("InfluxDB Demo!");
    Console.WriteLine("\n\nCommand: ");
    Console.WriteLine("\tP=Insert By Pointdata");
    Console.WriteLine("\tL=Insert By LineProtocol");
    Console.WriteLine("\tQ=Query by Flux");
    Console.WriteLine("\tX=Exit");
    Console.Write(" > ");
    command = Console.ReadKey().KeyChar.ToString().ToUpper();

    if (commandList.Contains(command))
        break;
}
Console.Write(" < \n\n");

if (command != "X")
{
    var db = new DbClient(host, token, database, organization);
    switch (command)
    {
        case "P":
            Console.WriteLine("Insert 10 Points for Tehran");
            for (int i = 0; i < 10; i++)
            {
                db.InsertByPointData();
                await Task.Delay(1000);
            }
            break;
        case "L":
            Console.WriteLine("Insert 10 Points for London");
            for (int i = 0; i < 10; i++)
            {
                db.InsertByLineProtocol();
                await Task.Delay(1000);
            }
            break;
        case "Q":
            await db.QueryByFlux();
            break;
    }
}
