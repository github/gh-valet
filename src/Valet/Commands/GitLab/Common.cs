using System.CommandLine;

namespace Valet.Commands.GitLab;

public static class Common
{
    public static readonly Option<string> InstanceUrl = new("--gitlab-instance-url")
    {
        Description = "The URL of the GitLab instance.",
        IsRequired = false,
    };

    public static readonly Option<string[]> Namespace = new(new[] { "--namespace", "-n" })
    {
        Description = "The GitLab namespace(s).",
        IsRequired = false,
        AllowMultipleArgumentsPerToken = true
    };

    public static readonly Option<string> AccessToken = new("--gitlab-access-token")
    {
        Description = "Access token for the GitLab instance.",
        IsRequired = false,
    };

    public static readonly Option<FileInfo[]> SourceFilePath = new("--source-file-path")
    {
        Description = "The file path(s) to existing jobs data.",
        IsRequired = false,
        AllowMultipleArgumentsPerToken = true,
    };

    public static readonly Option<string> Project = new("--project")
    {
        Description = "The GitLab project name.",
        IsRequired = true,
    };
}