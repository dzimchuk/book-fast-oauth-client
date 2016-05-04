using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using BookFast.OAuth.Client.Models;
using Microsoft.Extensions.OptionsModel;
using Newtonsoft.Json;

namespace BookFast.OAuth.Client.Proxy
{
    public class BookFastApiProxy
    {
        private readonly BookFastApiOptions options;

        public BookFastApiProxy(IOptions<BookFastApiOptions> options)
        {
            this.options = options.Value;

#if DNX451
            System.Net.ServicePointManager.ServerCertificateValidationCallback +=
                (o, certificate, chain, errors) => true;
#endif
        }

        public async Task<IEnumerable<Booking>> LoadBookingsAsync(string accessToken)
        {
            var client = new HttpClient { BaseAddress = new Uri(options.BaseUrl, UriKind.Absolute) };
            client.DefaultRequestHeaders.Authorization = 
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

            var response = await client.GetAsync("/api/bookings");
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<Booking>>(result,
                new JsonSerializerSettings { DateParseHandling = DateParseHandling.DateTimeOffset });
        }    
    }
}