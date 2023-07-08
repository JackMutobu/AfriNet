using AfriNetConsoleTest;
using AfriNetRouterLib;
using System.Diagnostics;
using System.Text.Json;

var settings = new Settings();
var homeUrl = $"http://{settings.RouterIP}";

var c50Roter = new TpLinkArcherC50Service(homeUrl, settings.RouterPassword);

var timer = new Stopwatch();
timer.Start();

Console.WriteLine("Get Connected Devices");
await c50Roter.GetConnectedDevices()
    .Match(devices => Console.WriteLine(JsonSerializer.Serialize(devices)), 
           exception => Console.WriteLine(exception.Message));

Console.WriteLine("\nGet Blocked Devices");
await c50Roter.GetBlockedDevices()
    .Match(devices => Console.WriteLine(JsonSerializer.Serialize(devices)),
           exception => Console.WriteLine(exception.Message));

Console.WriteLine("\nBlock Device DA:06:D7:9E:94:06");
await c50Roter.BlockDevice("DA:06:D7:9E:94:06")
    .Match(_ => Console.WriteLine("Device DA:06:D7:9E:94:06 blocked"),
           exception => Console.WriteLine(exception.Message));

Console.WriteLine("\nGet Blocked Devices");
await c50Roter.GetBlockedDevices()
    .Match(devices => Console.WriteLine(JsonSerializer.Serialize(devices)),
           exception => Console.WriteLine(exception.Message));

Console.WriteLine("\nUnblock Device");
await c50Roter.UnBlockDevice("DA:06:D7:9E:94:06")
    .Match(_ => Console.WriteLine("Device DA:06:D7:9E:94:06 unblocked"),
           exception => Console.WriteLine(exception.Message));

Console.WriteLine("\nGet Blocked Devices");
await c50Roter.GetBlockedDevices()
    .Match(devices => Console.WriteLine(JsonSerializer.Serialize(devices)),
           exception => Console.WriteLine(exception.Message));

timer.Stop();
TimeSpan timeTaken = timer.Elapsed;
Console.WriteLine($"\nTime taken: {timeTaken.ToString(@"m\:ss\.fff")}");


Console.ReadKey();