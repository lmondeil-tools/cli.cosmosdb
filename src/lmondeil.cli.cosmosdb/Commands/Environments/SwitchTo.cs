namespace lmondeil.cli.cosmosdb.Commands.Environments
{
    using McMaster.Extensions.CommandLineUtils;

    using System;

    [Command(Name = "switchto", ExtendedHelpText = "Example: switchto test")]
    internal class SwitchTo
    {
        [Argument(0)]
        public string ConfigurationName { get; set; }

        private void OnExecute(IConsole console)
        {
            console.WriteLine($"Switching to {this.ConfigurationName}");

            string configFile = $"{AppDomain.CurrentDomain.BaseDirectory}appSettings.{this.ConfigurationName}.json";
            if (File.Exists(configFile))
            {
                File.Delete("appSettings.json");
                File.Copy(configFile, $"{AppDomain.CurrentDomain.BaseDirectory}appSettings.json", true);
                console.WriteLine($"Switched to {this.ConfigurationName}");
            }
            else
            {
                console.WriteLine(@$"Missing {AppDomain.CurrentDomain.BaseDirectory}{configFile} file");
            }

        }
    }
}
