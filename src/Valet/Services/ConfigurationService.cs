using System.Text;
using Valet.Interfaces;

namespace Valet.Services;

public class ConfigurationService : IConfigurationService
{
    public async Task<Dictionary<string, string>> ReadCurrentVariablesAsync(string filePath = ".env.local")
    {
        var lines = await File.ReadAllLinesAsync(filePath);

        var variables = new Dictionary<string, string>();
        foreach (var line in lines)
        {
            if (string.IsNullOrWhiteSpace(line)) continue;

            var variable = line.Split('=', StringSplitOptions.TrimEntries);
            if (variable.Length != 2) continue;

            variables[variable[0]] = variable[1];
        }

        return variables;
    }

    public async Task<Dictionary<string, string>> GetUserInputAsync()
    {
        var input = new Dictionary<string, string>();

        foreach (var variable in Constants.UserInputVariables)
        {
            Console.Write($"Enter value for '{variable}' (leave empty to omit): ");
            var value = await Console.In.ReadLineAsync();
            if (string.IsNullOrWhiteSpace(value)) continue;
            
            input[variable] = value;
        }

        return input;
    }

    public async Task WriteVariablesAsync(Dictionary<string, string> variables, string filePath = ".env.local")
    {
        var lines = variables.Select(kvp => $"{kvp.Key}={kvp.Value}").ToList();

        Console.WriteLine(string.Join('\n', lines));
        await Task.FromResult(true);
        // await File.WriteAllLinesAsync(filePath, lines);
    }
}