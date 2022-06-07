namespace Valet.Interfaces;

public interface IConfigurationService
{
    Task<Dictionary<string, string>> ReadCurrentVariablesAsync(string filePath = ".env.local");
    Task<Dictionary<string, string>> GetUserInputAsync();
    Task WriteVariablesAsync(Dictionary<string, string> variables, string filePath = ".env.local");
    
    Dictionary<string, string> MergeVariables(Dictionary<string, string> currentVariables, Dictionary<string, string> newVariables)
    {
        foreach (var variable in newVariables)
        {
            currentVariables[variable.Key] = variable.Value;
        }
        
        return currentVariables;
    }
}