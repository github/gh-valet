using System.Collections.Immutable;
using System.CommandLine;

namespace Valet.Commands.GitHub;

public class Forecast : ContainerCommand
{
    public Forecast(string[] args) : base(args)
    {
    }

    protected override string Name => "github";
    protected override string Description => "Forecasts GitHub Actions usage from historical GitHub pipeline utilization.";

    public static readonly Option<string> InstanceUrl = new("--github-instance-url")
    {
        Description = "The URL of the GitHub instance.",
        IsRequired = false,
    };

    public static readonly Option<string> AccessToken = new("--github-access-token")
    {
        Description = "Access token for the GitHub instance.",
        IsRequired = false,
    };

    public static readonly Option<string> Organization = new("--organization")
    {
        Description = "The GitHub organization name.",
        IsRequired = true,
    };

    public static readonly Option<string> Repository = new("--repository")
    {
        Description = "The GitHub repository name.",
        IsRequired = false,
    };

    private static readonly Option<FileInfo[]> SourceFilePath = new("--source-file-path")
    {
        Description = "The file path(s) to existing jobs data.",
        IsRequired = false,
        AllowMultipleArgumentsPerToken = true,
    };

    protected override ImmutableArray<Option> Options => ImmutableArray.Create<Option>(
        InstanceUrl,
        AccessToken,
        Organization,
        Repository,
        SourceFilePath
    );
}
