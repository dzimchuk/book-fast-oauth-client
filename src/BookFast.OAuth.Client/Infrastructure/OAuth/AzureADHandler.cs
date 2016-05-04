using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNet.Authentication.OAuth;
using Newtonsoft.Json.Linq;

namespace BookFast.OAuth.Client.Infrastructure.OAuth
{
    internal class AzureADHandler : OAuthHandler<AzureADOptions>
    {
        public AzureADHandler(HttpClient backchannel) : base(backchannel)
        {
        }

        protected override async Task<OAuthTokenResponse> ExchangeCodeAsync(string code, string redirectUri)
        {
            var tokenRequestParameters = new Dictionary<string, string>()
            {
                { "client_id", Options.ClientId },
                { "redirect_uri", redirectUri },
                { "client_secret", Options.ClientSecret },
                { "code", code },
                { "grant_type", "authorization_code" },
                { "resource", Options.Resource }
            };

            var requestContent = new FormUrlEncodedContent(tokenRequestParameters);

            var requestMessage = new HttpRequestMessage(HttpMethod.Post, Options.TokenEndpoint);
            requestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            requestMessage.Content = requestContent;
            var response = await Backchannel.SendAsync(requestMessage, Context.RequestAborted);
            response.EnsureSuccessStatusCode();
            var payload = JObject.Parse(await response.Content.ReadAsStringAsync());
            return new OAuthTokenResponse(payload);
        }
    }
}