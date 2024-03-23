using Microsoft.AspNetCore.Components.WebAssembly.Http;


namespace CatTime.Frontend.Infrastructure.Handler
{
    public class CookieDelegatingHandler : DelegatingHandler
    {
        private readonly ILogger<CookieDelegatingHandler> _logger;

        public CookieDelegatingHandler(ILogger<CookieDelegatingHandler> logger)
        {
            _logger = logger;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            //_logger.TraceMethodEntry();

            request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
