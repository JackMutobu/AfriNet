using AfriNetLocalApi.Models.Responses;
using AfriNetRouterLib.Interfaces;
using FluentValidation;

namespace AfriNetLocalApi._Features_.Router
{
    public class UnBlockDeviceRequest
    {
        public string MacAddress { get; set; } = null!;
    }

    public class UnBlockDeviceRequestValidator : Validator<UnBlockDeviceRequest>
    {
        public UnBlockDeviceRequestValidator()
        {
            RuleFor(x => x.MacAddress)
                .NotEmpty()
                .WithMessage("Mac address was not provided");
        }
    }

    public class UnBlockDevice : Endpoint<UnBlockDeviceRequest, SuccessResponse>
    {
        private readonly IRouterService _routerService;
        public UnBlockDevice(IRouterService routerService)
        {
            _routerService = routerService;
        }

        public override void Configure()
        {
            Post("/api/router/unblock");
            AllowAnonymous();
        }

        public override Task HandleAsync(UnBlockDeviceRequest req, CancellationToken ct)
        => _routerService.UnBlockDevice(req.MacAddress)
            .Match(
                _ => SendAsync(new SuccessResponse($"{req.MacAddress} has been UnBlocked")),
                exception => ThrowError(exception.Message));
    }
}
