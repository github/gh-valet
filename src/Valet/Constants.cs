namespace Valet;

public static class Constants
{
    public static readonly List<string> UserInputVariables = new()
    {
        "GITHUB_ACCESS_TOKEN",
        "GITHUB_INSTANCE_URL",
        "AZURE_DEVOPS_ACCESS_TOKEN",
        "AZURE_DEVOPS_PROJECT",
        "AZURE_DEVOPS_ORGANIZATION",
        "AZURE_DEVOPS_INSTANCE_URL",
        "CIRCLE_CI_ACCESS_TOKEN",
        "CIRCLE_CI_INSTANCE_URL",
        "CIRCLE_CI_ORGANIZATION",
        "CIRCLE_CI_PROVIDER",
        "GITLAB_INSTANCE_URL",
        "GITLAB_ACCESS_TOKEN",
        "JENKINSFILE_ACCESS_TOKEN",
        "JENKINS_USERNAME",
        "JENKINS_ACCESS_TOKEN",
        "JENKINS_INSTANCE_URL",
        "TRAVIS_CI_ACCESS_TOKEN",
        "TRAVIS_CI_INSTANCE_URL",
        "TRAVIS_CI_SOURCE_GITHUB_ACCESS_TOKEN",
        "TRAVIS_CI_SOURCE_GITHUB_INSTANCE_URL",
        "TRAVIS_CI_ORGANIZATION"

    };

    public static Lazy<List<string>> EnvironmentVariables => new Lazy<List<string>>(() =>
    {
        var environmentVariables = new List<string>
         {
             "GH_ACCESS_TOKEN", "GH_INSTANCE_URL",
             "YAML_VERBOSITY", "HTTP_PROXY", "HTTPS_PROXY", "NO_PROXY", "OCTOKIT_PROXY", "OCTOKIT_SSL_VERIFY_MODE"
         };
        environmentVariables.AddRange(UserInputVariables);

        return environmentVariables;
    });
}