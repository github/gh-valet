using System.CommandLine;

namespace Valet.Commands.Travis;

public static class Common
{
    public static readonly Option<string> InstanceUrl = new(new[] { "-u", "--travis-ci-instance-url" })
    {
        Description = "The URL of the Travis CI instance.",
        IsRequired = false,
    };

    public static readonly Option<string> AccessToken = new(new[] { "-t", "--travis-ci-access-token" })
    {
        Description = "Access token for the Travis CI instance.",
        IsRequired = false,
    };

    public static readonly Option<string> Organization = new(new[] { "-g", "--travis-ci-organization" })
    {
        Description = "The Travis CI organization name.",
        IsRequired = false,
    };

    public static readonly Option<string> SourceGitHubAccessToken = new(new[] { "-s", "--travis-ci-source-github-access-token" })
    {
        Description = "Access token for the source GitHub instance.",
        IsRequired = false,
    };

    public static readonly Option<string> SourceGitHubInstanceUrl = new(new[] { "-i", "--travis-ci-source-github-instance-url" })
    {
        Description = "The URL of the source GitHub instance.",
        IsRequired = false,
    };

    public static readonly Option<FileInfo[]> SourceFilePath = new("--source-file-path")
    {
        Description = "The file path(s) to existing jobs data.",
        IsRequired = false,
        AllowMultipleArgumentsPerToken = true,
    };

    public static readonly Option<string> Repository = new(new[] { "--travis-ci-repository", "-r" })
    {
        Description = "The Travis CI repository name.",
        IsRequired = true,
    };
}