using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Beartrail.Infrastructure.Configuration;

public class JwtSettings
{
    public static string OptionsName = "JwtSettings";

    public string Key { get; set; } = string.Empty;

    public string Audience { get; set; } = string.Empty;

    public int ValidInHours { get; set; }

    public string Issuer { get; set; } = string.Empty;
}
