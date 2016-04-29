using System;
using System.Globalization;
using System.Net.Http;
using Microsoft.AspNet.Authentication;
using Microsoft.AspNet.Authentication.OAuth;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.DataProtection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.OptionsModel;
using Microsoft.Extensions.WebEncoders;

namespace BookFast.OAuth.Client.Infrastructure.OAuth
{
    public class AzureADMiddleware : AuthenticationMiddleware<AzureADOptions>
    {
        private const string MissingOptionErrorFormat = "{0} must be provided";

        public AzureADMiddleware(
            RequestDelegate next,
            IDataProtectionProvider dataProtectionProvider,
            ILoggerFactory loggerFactory,
            IUrlEncoder encoder,
            IOptions<SharedAuthenticationOptions> sharedOptions,
            AzureADOptions options)
            : base(next, options, loggerFactory, encoder)
        {
            if (next == null)
            {
                throw new ArgumentNullException(nameof(next));
            }

            if (dataProtectionProvider == null)
            {
                throw new ArgumentNullException(nameof(dataProtectionProvider));
            }

            if (loggerFactory == null)
            {
                throw new ArgumentNullException(nameof(loggerFactory));
            }

            if (encoder == null)
            {
                throw new ArgumentNullException(nameof(encoder));
            }

            if (sharedOptions == null)
            {
                throw new ArgumentNullException(nameof(sharedOptions));
            }

            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }
            
            if (string.IsNullOrEmpty(Options.AuthenticationScheme))
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, MissingOptionErrorFormat, nameof(Options.AuthenticationScheme)));
            }

            if (string.IsNullOrEmpty(Options.ClientId))
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, MissingOptionErrorFormat, nameof(Options.ClientId)));
            }

            if (string.IsNullOrEmpty(Options.ClientSecret))
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, MissingOptionErrorFormat, nameof(Options.ClientSecret)));
            }

            if (string.IsNullOrEmpty(Options.AuthorizationEndpoint))
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, MissingOptionErrorFormat, nameof(Options.AuthorizationEndpoint)));
            }

            if (string.IsNullOrEmpty(Options.TokenEndpoint))
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, MissingOptionErrorFormat, nameof(Options.TokenEndpoint)));
            }

            if (!Options.CallbackPath.HasValue)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, MissingOptionErrorFormat, nameof(Options.CallbackPath)));
            }

            if (Options.Events == null)
            {
                Options.Events = new OAuthEvents();
            }

            if (Options.StateDataFormat == null)
            {
                var dataProtector = dataProtectionProvider.CreateProtector(
                    GetType().FullName, Options.AuthenticationScheme, "v1");
                Options.StateDataFormat = new PropertiesDataFormat(dataProtector);
            }

            Backchannel = new HttpClient(Options.BackchannelHttpHandler ?? new HttpClientHandler());
            Backchannel.DefaultRequestHeaders.UserAgent.ParseAdd("Microsoft ASP.NET OAuth middleware");
            Backchannel.Timeout = Options.BackchannelTimeout;
            Backchannel.MaxResponseContentBufferSize = 1024 * 1024 * 10; // 10 MB

            if (string.IsNullOrEmpty(Options.SignInScheme))
            {
                Options.SignInScheme = sharedOptions.Value.SignInScheme;
            }
            if (string.IsNullOrEmpty(Options.SignInScheme))
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, MissingOptionErrorFormat, nameof(Options.SignInScheme)));
            }
        }

        protected HttpClient Backchannel { get; }
        
        protected override AuthenticationHandler<AzureADOptions> CreateHandler()
        {
            return new AzureADHandler(Backchannel);
        }
    }
}