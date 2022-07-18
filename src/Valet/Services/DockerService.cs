using System.Diagnostics;
using System.Text.Json;
using Valet.Interfaces;
using Valet.Models.Docker;

namespace Valet.Services;

public class DockerService : IDockerService
{
    private readonly IProcessService _processService;

    public DockerService(IProcessService processService)
    {
        _processService = processService;
    }

    public async Task UpdateImageAsync(string image, string server, string version, string? username, string? password, bool passwordStdin = false)
    {
        if (passwordStdin && Console.IsInputRedirected)
        {
            password = await Console.In.ReadToEndAsync();
        }

        if (!string.IsNullOrWhiteSpace(username) && !string.IsNullOrWhiteSpace(password))
        {
            await _processService.RunAsync(
                "docker",
                $"login {server} --username {username} --password-stdin",
                inputForStdIn: password
            ).ConfigureAwait(false);
        }
        else
        {
            Console.WriteLine("INFO: using cached credentials because no GHCR credentials were provided.");
        }

        await _processService.RunAsync(
            "docker",
            $"pull {server}/{image}:{version}"
        );
    }

    public Task ExecuteCommandAsync(string image, string server, string version, params string[] arguments)
    {
        var valetArguments = new List<string>
        {
            "run --rm -t"
        };
        valetArguments.AddRange(GetEnvironmentVariableArguments());

        var dockerArgs = Environment.GetEnvironmentVariable("DOCKER_ARGS");
        if (dockerArgs is not null)
        {
            valetArguments.Add(dockerArgs);
        }

        valetArguments.Add($"-v \"{Directory.GetCurrentDirectory()}\":/data");
        valetArguments.Add($"{server}/{image}:{version}");
        valetArguments.AddRange(arguments);

        return _processService.RunAsync(
            "docker",
            string.Join(' ', valetArguments),
            Directory.GetCurrentDirectory(),
            new[] { ("MSYS_NO_PATHCONV", "1") }
        );
    }

    public async Task VerifyDockerRunningAsync()
    {
        try
        {
            await _processService.RunAsync(
                "docker",
                "info",
                output: false
            );
        }
        catch (Exception)
        {
            throw new Exception("Please ensure docker is installed and the docker daemon is running");
        }
    }

    public async Task VerifyImagePresentAsync(string image, string server, string version)
    {
        try
        {
            await _processService.RunAsync(
                "docker",
                $"image inspect {server}/{image}:{version}",
                output: false
            );
        }
        catch (Exception)
        {
            throw new Exception("Unable to locate Valet image locally. Please run `gh valet update` to fetch the latest image prior to running this command.");
        }
    }

    public async Task<string?> GetLatestImageDigestAsync(string image, string server)
    {
        var manifestOutput = await _processService.RunAndCaptureAsync("docker", $"manifest inspect {server}/{image}:latest");
        Manifest? manifest = JsonSerializer.Deserialize<Manifest>(manifestOutput);

        return manifest?.GetDigest();
    }

    public async Task<string?> GetCurrentImageDigestAsync(string image, string server)
    {
        var digestOutput = await _processService.RunAndCaptureAsync("docker", $"image inspect --format={{{{.Id}}}} {server}/{image}:latest");

        return digestOutput.Split(":").ElementAtOrDefault(1)?.Trim();
    }

    private IEnumerable<string> GetEnvironmentVariableArguments()
    {
        if (File.Exists(".env.local"))
        {
            yield return "--env-file .env.local";
        }

        foreach (var env in Constants.EnvironmentVariables)
        {
            var value = Environment.GetEnvironmentVariable(env);

            if (string.IsNullOrWhiteSpace(value)) continue;

            var key = env;
            if (key.StartsWith("GH_"))
                key = key.Replace("GH_", "GITHUB_");

            yield return $"--env {key}={value}";
        }
    }
}