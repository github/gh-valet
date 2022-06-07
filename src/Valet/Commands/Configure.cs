using System.CommandLine;
using System.CommandLine.NamingConventionBinder;

namespace Valet.Commands;

public class Configure : BaseCommand
{
    protected override string Name => "configure";
    protected override string Description => "Start an interactive prompt to configure credentials used to authenticate to your CI server(s).";
    
    protected override Command GenerateCommand(App app)
    {
        var command = base.GenerateCommand(app);
        
        command.Handler = CommandHandler.Create(app.ConfigureAsync);

        return command;
    }
}