namespace Beartrail.Infrastracture.Services.Env;

public class EnvFileParser : IEnvFileParser
{
    private static readonly string DOT_ENV_FILE_NAME = ".env";
    private readonly ILogger<EnvFileParser> _logger;

    public EnvFileParser(ILogger<EnvFileParser> logger)
    {
        _logger = logger;
    }


    public void ParseDotEnvFile(string location)
    {
        var file = Path.Combine(location, DOT_ENV_FILE_NAME);
        if (!File.Exists(file))
        {
            try
            {
                var topLevelFolder = Directory.GetParent(location).FullName;
                _logger.LogInformation($"could not find .env file at location {location}. checking next folder {topLevelFolder}");
                ParseDotEnvFile(topLevelFolder);
                return;
            }
            catch (Exception e)
            {
                _logger.LogError($"got the error {e} during .env parsing");
            }
        }

        _logger.LogInformation($"parsing .env from {location}");
        var lines = File.ReadAllLines(file);
        foreach (var line in lines)
        {
            if (line.IndexOf("=") < 0)
            {
                continue;
            }
            var entry = line.Split("=", StringSplitOptions.RemoveEmptyEntries);
            Environment.SetEnvironmentVariable(entry[0].Trim(), entry[1].Trim());
        }
    }
}
