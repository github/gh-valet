using System.Collections;

namespace Valet.Models.Docker;

public class Manifest
{
    public int schemaVersion { get; set; }
    public string? mediaType { get; set; }
    public ManifestConfig? config { get; set; }
    public List<ManifestConfig>? layers { get; set; }

    public string? GetDigest()
    {
        return config?.digest?.Split(':')[1].Trim();
    }
}