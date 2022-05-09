using System.CommandLine;

namespace Valet.Commands.Jenkins;

public class Forecast : ContainerCommand
{
    public Forecast(string[] args) : base(args)
    {
    }

    protected override string Name => "jenkins";
    protected override string Description => "Forecasts GitHub Actions usage from historical Jenkins pipeline utilization.";

    private static readonly Option<FileInfo> ConfigFilePath = new("--config-file-path")
    {
        Description = "The file path corresponding to the Jenkins configuration file.",
        IsRequired = false,
    };

    protected override List<Option> Options => new()
    {
        Common.InstanceUrl,
        Common.Username,
        Common.AccessToken
    };
}