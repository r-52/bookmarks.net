using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Beartrail.Infrastructure.Configuration;

public class BearConfigurationSettings
{
    public static string OptionsName = "BearConfiguration";
    public JwtSettings JwtSettings { get; set; }
}
