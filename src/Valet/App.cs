using Valet.Interfaces;

namespace Valet;

public class App
{
    const string ValetImage = "valet-customers/valet-cli";
    const string ValetContainerRegistry = "ghcr.io";

    private readonly IDockerService _dockerService;
    private readonly IProcessService _processService;
    
    public App(IDockerService dockerService, IProcessService processService)
    {
        _dockerService = dockerService;
        _processService = processService;
    }

    public async Task<int> UpdateValetAsync(string? username = null, string? password = null, bool passwordStdin = false)
    {
        await _dockerService.VerifyDockerRunningAsync().ConfigureAwait(false);

        username ??= Environment.GetEnvironmentVariable("GHCR_USERNAME");
        password ??= Environment.GetEnvironmentVariable("GHCR_PASSWORD");

        await _dockerService.UpdateImageAsync(
            ValetImage,
            ValetContainerRegistry,
            "latest",
            username,
            password,
            passwordStdin
        );

        return 0;
    }

    public async Task<int> ExecuteValetAsync(string[] args)
    {
        await _dockerService.VerifyDockerRunningAsync().ConfigureAwait(false);
        await _dockerService.VerifyImagePresentAsync(
            ValetImage,
            ValetContainerRegistry,
            "latest"
        ).ConfigureAwait(false);

        await _dockerService.ExecuteCommandAsync(
            ValetImage,
            ValetContainerRegistry,
            "latest",
            args
        );
        return 0;
    }

    public async Task<int> GetVersionAsync()
    {
        var ghVersion = await _processService.RunAndCaptureAsync("gh", "version");
        var ghValetVersion = await _processService.RunAndCaptureAsync("gh", "extension list");
        var valetVersion = await _processService.RunAndCaptureAsync("docker", $"run --rm {ValetContainerRegistry}/{ValetImage}:latest version", throwOnError:false);

        var formattedGhVersion = ghVersion.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).FirstOrDefault();
        var formattedGhValetVersion = ghValetVersion.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .FirstOrDefault(x => x.Contains("github/gh-valet"));
        var formattedValetVersion = valetVersion.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).FirstOrDefault() ?? "unknown";

        Console.WriteLine(formattedGhVersion);
        Console.WriteLine(formattedGhValetVersion);
        Console.WriteLine($"valet-cli\t{formattedValetVersion}");

        return 0;
    }
}