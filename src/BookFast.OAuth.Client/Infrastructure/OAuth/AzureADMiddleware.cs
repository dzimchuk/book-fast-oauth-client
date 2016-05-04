using Microsoft.AspNet.Authentication;
using Microsoft.AspNet.Authentication.OAuth;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.DataProtection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.OptionsModel;
using Microsoft.Extensions.WebEncoders;

namespace BookFast.OAuth.Client.Infrastructure.OAuth
{
    public class AzureADMiddleware : OAuthMiddleware<AzureADOptions>
    {
        public AzureADMiddleware(
            RequestDelegate next,
            IDataProtectionProvider dataProtectionProvider,
            ILoggerFactory loggerFactory,
            IUrlEncoder encoder,
            IOptions<SharedAuthenticationOptions> sharedOptions,
            AzureADOptions options)
            : base(next, dataProtectionProvider, loggerFactory, encoder, sharedOptions, options)
        {
        }
        
        protected override AuthenticationHandler<AzureADOptions> CreateHandler()
        {
            return new AzureADHandler(Backchannel);
        }
    }
}