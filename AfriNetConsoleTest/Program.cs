using AfriNetConsoleTest;
var homeUrl = new Uri("http://192.168.1.78/");

  await homeUrl.InitializePlaywrightAsync(page => 
  page.Login(("#pcPassword", "#loginBtn"), "admin@123")
      .Bind(page => page.GetWirelessLiveDevices(("#frame1", "#menu_wl5g", "#menu_wlstat5g", "#frame2", "#refresh", "#tlbHeadMssid"))))
  .Match(
      title => Console.WriteLine(title), 
      ex =>  Console.WriteLine(ex.Message));