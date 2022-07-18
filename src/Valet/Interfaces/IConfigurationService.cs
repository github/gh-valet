using System.Collections.Immutable;

namespace Valet.Interfaces;

public interface IConfigurationService
{
    Task<ImmutableDictionary<string, string>> ReadCurrentVariablesAsync(string filePath = ".env.local");
    ImmutableDictionary<string, string> GetUserInput();
    Task WriteVariablesAsync(ImmutableDictionary<string, string> variables, string filePath = ".env.local");

    ImmutableDictionary<string, string> MergeVariables(ImmutableDictionary<string, string> currentVariables, ImmutableDictionary<string, string> newVariables)
    {
        return currentVariables.SetItems(newVariables);
    }
}