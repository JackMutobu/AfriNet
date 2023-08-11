using AfriNetLocalApi.Constants;
using AfriNetLocalApi.Entities;
using AfriNetLocalApi.Models.Responses;
using AfriNetLocalApi.Services.Bundles;


namespace AfriNetLocalApi._Features_.Bundles
{
    public class BundleDto
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Amount { get; set; }
        public decimal Data { get; set; }
        public int ExpiresIn { get; set; }
        public bool IsUnlimited { get; set; }
    }

    public record BundleRequst(BundleDto Bundle);
    public class Create:Endpoint<BundleRequst, SuccessResponse>
    {
        private readonly IBundleService _bundleService;
        public Create(IBundleService bundleService)
        {
            _bundleService = bundleService;
        }

        public override void Configure()
        {
            Post("/api/bundles");
            Roles(AuthKeys.Roles.SuperAdmin);
        }

        public override Task HandleAsync(BundleRequst req, CancellationToken ct)
        => _bundleService.Create(req.Bundle.Adapt<Bundle>(), ct)
            .Map(_ => SendAsync(new SuccessResponse("Bundle created")));
    }
}
