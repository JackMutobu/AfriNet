using AfriNetLocalApi.Constants;
using AfriNetLocalApi.Recharges.Services;

namespace AfriNetLocalApi._Features_.Recharges
{
    public record RechargeRetailerRequest(Guid RetailerId, Guid BundleId, int Quantity, Guid? DealerId = null);
    public record RechargeRetailerResponse(IEnumerable<Guid> TransactionIds);
    public class Retailer : Endpoint<RechargeRetailerRequest, RechargeRetailerResponse>
    {
        private readonly IRechargeService _rechargeService;
        public Retailer(IRechargeService rechargeService)
        {
            _rechargeService = rechargeService;
        }

        public override void Configure()
        {
            Post("/api/recharges/retailer");
            Roles(AuthKeys.Roles.Retailer, AuthKeys.Roles.SuperAdmin);
        }

        public override Task HandleAsync(RechargeRetailerRequest req, CancellationToken ct)
        => TryAsync(_rechargeService.RechargeRetailer(new RechargeCommand(req.RetailerId, req.BundleId, req.Quantity), ct))
            .Match(result => SendAsync(new RechargeRetailerResponse(result.Select(x => x.Id))),
                   error => this.ThrowError(error));
    }
}
