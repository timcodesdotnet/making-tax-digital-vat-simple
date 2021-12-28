using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TimCodes.Mtd.Vat.Core.Authorisation;
using TimCodes.Mtd.Vat.Core.Configuration;
using TimCodes.Mtd.Vat.Core.Services;

namespace TimCodes.Mtd.Vat.Core.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddVatServices(this IServiceCollection services, IConfiguration config)
        {
            services.Configure<MtdOptions>(config.GetSection(nameof(MtdOptions)));

            services.AddHttpClient();
            services.AddMemoryCache();
            services.AddScoped<ITokenCache, FileTokenCache>();
            services.AddScoped<ITokenReceiver, OAuthTokenReveiver>();
            services.AddScoped<AuthorisationProvider>();
            services.AddScoped<IVatService, VatService>();
        }
    }
}
