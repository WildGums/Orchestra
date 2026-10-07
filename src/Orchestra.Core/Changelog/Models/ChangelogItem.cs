namespace Orchestra.Changelog;

using System.Text;
using System.Text.Json.Serialization;

public class ChangelogItem
{
    public ChangelogItem()
    {
        Group = string.Empty;
        Name = string.Empty;
        Description = string.Empty;

        Type = ChangelogType.Change;
    }

    public string Group { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ChangelogType Type { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    [JsonIgnore]
    public string? Reference { get; set; }

    [JsonIgnore]
    public string? Url { get; set; }

    [JsonIgnore]
    public object? Tag { get; set; }

    public override string ToString()
    {
        var stringBuilder = new StringBuilder($"[{Group}] ");

        if (!string.IsNullOrWhiteSpace(Reference))
        {
            stringBuilder.Append($"[{Reference}] ");
        }

        stringBuilder.Append(Name);

        return stringBuilder.ToString();
    }
}
