namespace Valet.Models;

public readonly struct Variable : IEquatable<Variable>
{
    public Variable(string key, Provider provider, string message, string? defaultValue = null)
    {
        Key = key;
        Provider = provider;
        Message = message;
        DefaultValue = defaultValue;
    }

    public string Key { get; }
    public string ProviderName => Provider switch
    {
        Provider.GitHub => "GitHub",
        Provider.AzureDevOps => "Azure DevOps",
        Provider.CircleCI => "CircleCI",
        Provider.GitLabCI => "GitLab CI",
        Provider.Jenkins => "Jenkins",
        Provider.TravisCI => "Travis CI",
        _ => throw new ArgumentOutOfRangeException()
    };

    public bool IsPassword => Key.EndsWith("ACCESS_TOKEN");
    public string Message { get; }
    public string? DefaultValue { get; }

    public string? Placeholder => DefaultValue is not null && IsPassword ? $"({DefaultValue})" : null;

    private Provider Provider { get; }

    public override bool Equals(object? obj)
    {
        return obj is Variable v && Equals(v);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Key, Provider, Message, DefaultValue);
    }

    public static bool operator ==(Variable left, Variable right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Variable left, Variable right)
    {
        return !(left == right);
    }

    public bool Equals(Variable other)
    {
        return Key == other.Key &&
            Provider == other.Provider &&
            Message == other.Message &&
            DefaultValue == other.DefaultValue;
    }
}