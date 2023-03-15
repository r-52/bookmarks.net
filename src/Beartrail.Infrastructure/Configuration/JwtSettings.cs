using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Beartrail.Infrastructure.Configuration;

public class JwtSettings
{
    public static string OptionsName = "JwtSettings";

    public string Key { get; set; } = string.Empty;

    public IEnumerable<string> Audiences { get; set; }

    public int ValidInHours { get; set; }

    public IEnumerable<string> Issuers { get; set; }
}
