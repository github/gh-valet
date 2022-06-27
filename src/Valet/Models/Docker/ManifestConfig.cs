namespace Valet.Models.Docker;

public class ManifestConfig
{
    public string? mediaType { get; set; }
    public int size { get; set; }
    public string? digest { get; set; }
}