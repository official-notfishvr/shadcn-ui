namespace shadcnui
{
    public class PluginInfo
    {
        public const string GUID = "org.a.a.a";
        public const string Name = "";
        public const string Description = "";
        public const string BuildTimestamp = "2025-09-09T00:40:59Z";
        public const string Version = "0.0.0";

#if DEBUG
        public static bool BetaBuild = true;
#else
        public static bool BetaBuild = false;
#endif
    }
}
