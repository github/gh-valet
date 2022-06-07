using System.Text;
using Sharprompt;
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
            if (variable.Length != 2 || string.IsNullOrWhiteSpace(variable[1])) continue;

            variables[variable[0]] = variable[1];
        }

        return variables;
    }

    public Dictionary<string, string> GetUserInput()
    {
        var input = new Dictionary<string, string>();

        foreach (var variable in Constants.UserInputVariables)
        {
            var value = variable.EndsWith("TOKEN")
                ? Prompt.Password($"Enter value for '{variable}' (leave empty to skip)")
                : Prompt.Input<string>($"Enter value for '{variable}' (leave empty to skip)");

            if (string.IsNullOrWhiteSpace(value)) continue;

            input[variable] = value;
        }

        return input;
    }

    public async Task WriteVariablesAsync(Dictionary<string, string> variables, string filePath = ".env.local")
    {
        var lines = variables.Select(kvp => $"{kvp.Key}={kvp.Value}").ToList();
        await File.WriteAllLinesAsync(filePath, lines);
    }
}