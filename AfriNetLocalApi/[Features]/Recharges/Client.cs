using AfriNetLocalApi.Constants;
using AfriNetLocalApi.Recharges.Services;

namespace AfriNetLocalApi._Features_.Recharges
{
    public record RechargeClientRequest(Guid ClientId, Guid BundleId, int Quantity, Guid? RetailerId = null);
    public record RechargeClientResponse(IEnumerable<Guid> TransactionIds, IEnumerable<Guid> ActiveBundleIds);
    public class Client:Endpoint<RechargeClientRequest,RechargeClientResponse>
    {
        private readonly IRechargeService _rechargeService;
        public Client(IRechargeService rechargeService)
        {
            _rechargeService = rechargeService;
        }

        public override void Configure()
        {
            Post("/api/recharges/client");
            Roles(AuthKeys.Roles.Client, AuthKeys.Roles.SuperAdmin);
        }

        public override Task HandleAsync(RechargeClientRequest req, CancellationToken ct)
        => TryAsync(_rechargeService.RechargeClient(new RechargeCommand(req.ClientId, req.BundleId, req.Quantity), ct))
            .Match(result => SendAsync(new RechargeClientResponse(result.Select(x => x.Trasaction.Id), result.Select(x => x.ActiveBundle.Id))),
             error => this.ThrowError(error));
    }
}
