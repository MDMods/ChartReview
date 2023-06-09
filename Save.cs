using System.IO;
using Tomlet;
using Tomlet.Attributes;

namespace ChartReview;

internal static class Save
{
    internal static Data Data = new();

    public static void Load()
    {
        if (!File.Exists(Path.Combine("UserData", "ChartReview.cfg")))
        {
            var defaultConfig = TomletMain.TomlStringFrom(Data);
            File.WriteAllText(Path.Combine("UserData", "ChartReview.cfg"), defaultConfig);
        }

        var data = File.ReadAllText(Path.Combine("UserData", "ChartReview.cfg"));
        Data = TomletMain.To<Data>(data);
    }
}

public class Data
{
    [TomlPrecedingComment("Whether the chart review is enabled")]
    internal bool ChartReviewEnabled;

    [TomlPrecedingComment("The character before enable chart review")]
    internal int LastCharacter { get; set; }

    [TomlPrecedingComment("The elfin before enable chart review")]
    internal int LastElfin { get; set; } = -1;

    [TomlPrecedingComment("The offset before enable chart review")]
    internal int LastOffset { get; set; }
}