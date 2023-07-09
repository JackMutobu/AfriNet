using AfriNetLocalApi.Models.Responses;
using AfriNetRouterLib.Interfaces;
using FluentValidation;

namespace AfriNetLocalApi._Features_.Router
{
    public class BlockDeviceRequest
    {
        public string MacAddress { get; set; } = null!;
    }

    public class BlockDeviceRequestValidator: Validator<BlockDeviceRequest>
    {
        public BlockDeviceRequestValidator()
        {
            RuleFor(x => x.MacAddress)
                .NotEmpty()
                .WithMessage("Mac address was not provided");
        }
    }

    public class BlockDevice:Endpoint<BlockDeviceRequest,SuccessResponse>
    {
        private readonly IRouterService _routerService;
        public BlockDevice(IRouterService routerService)
        {
            _routerService = routerService;
        }

        public override void Configure()
        {
            Post("/api/router/block");
            AllowAnonymous();
        }

        public override Task HandleAsync(BlockDeviceRequest req, CancellationToken ct)
        => _routerService.BlockDevice(req.MacAddress)
            .Match(
                _ => SendAsync(new SuccessResponse($"{req.MacAddress} has been blocked")),
                exception => ThrowError(exception.Message));
    }
}
