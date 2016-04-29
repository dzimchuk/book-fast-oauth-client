using Microsoft.Extensions.OptionsModel;

namespace BookFast.OAuth.Client.Infrastructure
{
    public class AuthenticationOptions : IOptions<AuthenticationOptions>
    {
        public string AuthorizationEndpoint { get; set; }
        public string TokenEndpoint { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string Resource { get; set; }
        public AuthenticationOptions Value => this;
    }
}