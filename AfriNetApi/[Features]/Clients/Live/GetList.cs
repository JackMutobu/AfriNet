namespace AfriNetApi._Features_.Clients.Connected
{
    public class GetListResponse
    {
        public string Content { get; set; } = string.Empty;
    }

    public class GetListEndpoint : EndpointWithoutRequest<GetListResponse>
    {
        private readonly IHttpClientFactory _clientFactory;
        public GetListEndpoint(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public override void Configure()
        {
            Get("/api/clients/live");
            AllowAnonymous();
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            using var client = _clientFactory.CreateClient("tplink");
            var response = new GetListResponse()
            {
                Content = "Testing"
            };

            await SendAsync(response);
        }
    }

}
