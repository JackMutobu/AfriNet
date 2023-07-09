using AfriNetRouterLib.Interfaces;
using AfriNetRouterLib.Models;

namespace AfriNetLocalApi._Features_.Router
{
    public record ConnectedDevicesResponse(IEnumerable<WireLessLiveDevice> connectedDevices);
    public class GetConnectedDevices: EndpointWithoutRequest<ConnectedDevicesResponse>
    {
        private readonly IRouterService _routerService;
        public GetConnectedDevices(IRouterService routerService)
        {
            _routerService = routerService;
        }
        public override void Configure()
        {
            Get("/api/router/connected");
            AllowAnonymous();
        }

        public override Task HandleAsync(CancellationToken ct)
        => _routerService.GetConnectedDevices()
            .Match(
                devices => SendAsync(new ConnectedDevicesResponse(devices)),
                exception => ThrowError(exception.Message));
    }
}
