using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Beartrail.Web.Configuration;

internal static class ConfigurationStartup
{
    internal static WebApplicationBuilder AddCustomConfig(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection(JwtSettings.OptionsName));
        builder.Services.Configure<BearConfigurationSettings>(builder.Configuration.GetSection(BearConfigurationSettings.OptionsName));
        return builder;
    }
}
