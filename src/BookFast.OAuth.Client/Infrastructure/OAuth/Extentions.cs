using System;
using Microsoft.AspNet.Builder;

namespace BookFast.OAuth.Client.Infrastructure.OAuth
{
    public static class Extentions
    {
        public static void UseAzureAD(this IApplicationBuilder builder, Action<AzureADOptions> configAction)
        {
            var options = new AzureADOptions();
            configAction(options);

            builder.UseMiddleware<AzureADMiddleware>(options);
        }
    }
}