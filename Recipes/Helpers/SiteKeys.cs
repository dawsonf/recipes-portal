using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Recipes.Helpers
{
    public class SiteKeys
    {
        private static IConfigurationSection _configuration;
        public static void Configure(IConfigurationSection configuration)
        {
            _configuration = configuration;
        }

        public static string WebSiteDomain => _configuration["WebSiteDomain"];
        public static string Token => _configuration["Secret"];

    }
}
