using System.Collections.Immutable;
using System.CommandLine;

namespace Valet.Commands.AzureDevOps.ReleasePipeline;

public class DryRun : ContainerCommand
{
    public DryRun(string[] args) : base(args)
    {
    }

    protected override string Name => "release";
    protected override string Description => "Target a release pipeline";

    public static readonly Option<int> PipelineId = new(new[] { "--pipeline-id", "-i" })
    {
        Description = "The Azure DevOps pipeline id.",
        IsRequired = true,
    };

    protected override ImmutableArray<Option> Options => ImmutableArray<Option>.Empty;
}
