using AfriNetLocalApi.Entities;
using AfriNetLocalApi.Services.Bundles;

namespace AfriNetLocalApi._Features_.Bundles
{
    public record GetRequest(int Skip, int Take, string For);
    public class BundleResponse:Bundle
    {
        public string Type { get; set; } = string.Empty;
    }
    public class GetList: Endpoint<GetRequest, IEnumerable<BundleResponse>>
    {
        private readonly IBundleService _bundleService;
        public GetList(IBundleService bundleService)
        {
            _bundleService = bundleService;
        }

        public override void Configure()
        {
            Get("/api/bundles");
        }

        public override Task HandleAsync(GetRequest req, CancellationToken ct)
        => _bundleService.GetList(req.Skip, req.Take, ct)
            .Map(bundles => SendAsync(bundles.Where(x => string.IsNullOrEmpty(req.For) || x.For == req.For).Select(x => GetBundleResponse(x))));

        static BundleResponse GetBundleResponse(Bundle bundle)
        {
            var response = bundle.Adapt<BundleResponse>();
            response.Type = (response.IsUnlimited, response.ExpiresIn) switch
            {
                (true, _) => BundleType.Unlimited,
                (false, <= 72) => BundleType.Daily,
                (false, > 72 and <= 168) => BundleType.Weekly,
                _ => BundleType.Monthly,
            };

            return response;
        }
    }
}
