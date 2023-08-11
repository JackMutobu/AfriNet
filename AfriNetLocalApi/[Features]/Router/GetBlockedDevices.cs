using AfriNetRouterLib.Interfaces;
using AfriNetRouterLib.Models;

namespace AfriNetLocalApi._Features_.Router
{
    public record BlockedDevicesResponse(IEnumerable<MacFilteredWirelessDevice> BlockedDevices);
    public class GetBlockedDevices: EndpointWithoutRequest<BlockedDevicesResponse>
    {
        private readonly IRouterService _routerService;
        public GetBlockedDevices(IRouterService routerService)
        {
            _routerService = routerService;
        }

        public override void Configure()
        {
            Get("/api/router/block");
        }

        public override Task HandleAsync(CancellationToken ct)
        => _routerService.GetBlockedDevices()
            .Match(
                devices => SendAsync(new BlockedDevicesResponse(devices)),
                exception => ThrowError(exception.Message));
    }
}
