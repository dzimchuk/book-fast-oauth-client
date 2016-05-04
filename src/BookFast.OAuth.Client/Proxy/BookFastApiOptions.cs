using Microsoft.Extensions.OptionsModel;

namespace BookFast.OAuth.Client.Proxy
{
    public class BookFastApiOptions : IOptions<BookFastApiOptions>
    {
        public string BaseUrl { get; set; }
        public BookFastApiOptions Value => this;
    }
}