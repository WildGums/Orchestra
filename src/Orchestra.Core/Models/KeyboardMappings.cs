namespace Orchestra;

using System.Collections.Generic;
using Catel.Data;

public class KeyboardMappings : ObservableObject
{
    public KeyboardMappings()
    {
        GroupName = string.Empty;
        Mappings = new List<KeyboardMapping>();
    }

    public string GroupName { get; set; }

    public List<KeyboardMapping> Mappings { get; init; }
}
