using System.IO;
using Tomlet;
using Tomlet.Attributes;

namespace ChartReview
{
    internal static class Save
    {
        internal static Data data;

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
        internal bool ChartReviewEnabled;

        internal Data(bool chartReviewEnabled) => ChartReviewEnabled = chartReviewEnabled;
    }
}