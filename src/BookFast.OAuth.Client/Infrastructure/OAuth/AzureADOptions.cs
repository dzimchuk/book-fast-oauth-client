using Microsoft.AspNet.Authentication.OAuth;

namespace BookFast.OAuth.Client.Infrastructure.OAuth
{
    public class AzureADOptions : OAuthOptions
    {
        public string Resource { get; set; }
    }
}