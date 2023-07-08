using AfriNetConsoleTest;
using System.Text.Json;

var homeUrl = "http://192.168.1.78/";
var password = "admin@123";

var c50Roter = new RouterTpLinkC50Service(homeUrl, password);

await c50Roter.BlockDevice("DA:06:D7:9E:94:05")
    .Bind(_ => c50Roter.Get5GMacFilteringTable())
    .Match(
    devices => 
    Console.WriteLine(JsonSerializer.Serialize(devices)),
    exception => 
    Console.WriteLine(exception.Message)
    );
Console.ReadKey();