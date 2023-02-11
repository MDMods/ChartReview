using System.IO;
using Tomlet;
using Tomlet.Attributes;

namespace ChartReview
{
    internal static class Save
    {
        internal static Data data = new Data();

        public static void Load()
        {
            if (!File.Exists(Path.Combine("UserData", "ChartReview.cfg")))
            {
                string defaultConfig = TomletMain.TomlStringFrom(data);
                File.WriteAllText(Path.Combine("UserData", "ChartReview.cfg"), defaultConfig);
            }
            string enabled = File.ReadAllText(Path.Combine("UserData", "ChartReview.cfg"));
            data = TomletMain.To<Data>(enabled);
        }
    }

    internal struct Data
    {
        [TomlPrecedingComment("Whether the chart review is enabled")]
        internal bool ChartReviewEnabled = false;

        [TomlPrecedingComment("The character before enable chart review")]
        internal int LastCharacter = 0;

        [TomlPrecedingComment("The elfin before enable chart review")]
        internal int LastElfin = -1;

        [TomlPrecedingComment("The offset before enable chart review")]
        internal int LastOffset = 0;

        public Data()
        { }

        public Data(bool chartReviewEnabled, int lastCharacter, int lastElfin, int lastOffset)
        {
            ChartReviewEnabled = chartReviewEnabled;
            LastCharacter = lastCharacter;
            LastElfin = lastElfin;
            LastOffset = lastOffset;
        }
    }
}