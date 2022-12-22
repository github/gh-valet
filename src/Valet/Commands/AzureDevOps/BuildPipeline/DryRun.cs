﻿using System.Collections.Immutable;
using System.CommandLine;

namespace Valet.Commands.AzureDevOps.BuildPipeline;

public class DryRun : ContainerCommand
{
    public DryRun(string[] args) : base(args)
    {
    }

    protected override string Name => "pipeline";
    protected override string Description => "Target a designer or YAML pipeline";

    public static readonly Option<int> PipelineId = new(new[] { "--pipeline-id", "-i" })
    {
        Description = "The Azure DevOps pipeline id.",
        IsRequired = false,
    };

    protected override ImmutableArray<Option> Options => ImmutableArray.Create<Option>(
        PipelineId,
        Common.SourceFilePath
    );
}
