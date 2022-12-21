﻿using System.Collections.Immutable;
using System.CommandLine;

namespace Valet.Commands.AzureDevOps;

public class Audit : ContainerCommand
{
    public Audit(string[] args)
        : base(args)
    {
    }

    protected override string Name => "azure-devops";
    protected override string Description => "An audit will output a list of data used in an Azure DevOps instance.";

    private static readonly Option<FileInfo> ConfigFilePath = new("--config-file-path")
    {
        Description = "The file path corresponding to the Azure DevOps configuration file.",
        IsRequired = false,
    };

    protected override ImmutableArray<Option> Options => ImmutableArray.Create<Option>(
        Common.Organization,
        Common.Project,
        Common.InstanceUrl,
        Common.AccessToken
    );
}
