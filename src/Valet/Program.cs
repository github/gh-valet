﻿using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Parsing;
using Valet;
using Valet.Commands;
using Valet.Services;

var processService = new ProcessService();

var app = new App(
    new DockerService(processService),
    new ConfigurationService()
);

var command = new RootCommand("Valet is a tool that facilitates migrations to GitHub Actions.")
{
    new Update().Command(app),
    new Valet.Commands.Version(args).Command(app),
    new Configure().Command(app),
    new Audit(args).Command(app),
    new DryRun(args).Command(app),
    new Migrate(args).Command(app),
    new Forecast(args).Command(app)
};

var parser = new CommandLineBuilder(command)
    .UseHelp()
    .UseEnvironmentVariableDirective()
    .RegisterWithDotnetSuggest()
    .UseSuggestDirective()
    .UseTypoCorrections()
    .UseParseErrorReporting()
    .CancelOnProcessTermination()
    .Build();

try
{
    await parser.InvokeAsync(args);
    return 0;
}
catch (Exception e)
{
    Console.Error.Write(e.Message);
    return 1;
}