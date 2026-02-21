using UnityEngine;

namespace ShadcnUI.Core.Theming;

/// <summary>
/// Immutable theme definition using C# records.
/// Represents a complete color scheme and design tokens for the UI.
/// </summary>
public sealed record Theme
{
    public string Name { get; init; }
    
    // Base colors
    public Color Background { get; init; }
    public Color Foreground { get; init; }
    public Color Card { get; init; }
    public Color CardForeground { get; init; }
    public Color Popover { get; init; }
    public Color PopoverForeground { get; init; }
    
    // Brand colors
    public Color Primary { get; init; }
    public Color PrimaryForeground { get; init; }
    public Color Secondary { get; init; }
    public Color SecondaryForeground { get; init; }
    
    // Utility colors
    public Color Muted { get; init; }
    public Color MutedForeground { get; init; }
    public Color Accent { get; init; }
    public Color AccentForeground { get; init; }
    public Color Destructive { get; init; }
    public Color DestructiveForeground { get; init; }
    
    // Border and input colors
    public Color Border { get; init; }
    public Color Input { get; init; }
    public Color Ring { get; init; }
    
    // Additional colors
    public Color Success { get; init; }
    public Color SuccessForeground { get; init; }
    public Color Warning { get; init; }
    public Color WarningForeground { get; init; }
    public Color Info { get; init; }
    public Color InfoForeground { get; init; }
    
    // Overlay and effects
    public Color Overlay { get; init; }
    public Color Shadow { get; init; }
    
    // Design tokens
    public float Radius { get; init; } = 0.5f;

    /// <summary>
    /// Creates a color from a hex string (e.g., "#FF5733" or "FF5733").
    /// </summary>
    public static Color FromHex(string hex)
    {
        if (ColorUtility.TryParseHtmlString(hex, out Color color))
            return color;
        return Color.white;
    }

    #region Built-in Themes

    public static Theme Dark => new()
    {
        Name = "dark",
        Background = FromHex("#0c0c0e"),
        Foreground = FromHex("#f8f8f8"),
        Card = FromHex("#16161a"),
        CardForeground = FromHex("#f8f8f8"),
        Popover = FromHex("#16161a"),
        PopoverForeground = FromHex("#f8f8f8"),
        Primary = FromHex("#252832"),
        PrimaryForeground = FromHex("#f8f8f8"),
        Secondary = FromHex("#1f222b"),
        SecondaryForeground = FromHex("#f8f8f8"),
        Muted = FromHex("#111114"),
        MutedForeground = FromHex("#808496"),
        Accent = FromHex("#5b8cff"),
        AccentForeground = FromHex("#f8f8f8"),
        Destructive = FromHex("#ef4444"),
        DestructiveForeground = FromHex("#ffffff"),
        Success = FromHex("#3cc171"),
        SuccessForeground = FromHex("#ffffff"),
        Warning = FromHex("#fbba42"),
        WarningForeground = FromHex("#0c0c0e"),
        Info = FromHex("#5b8cff"),
        InfoForeground = FromHex("#f8f8f8"),
        Border = FromHex("#252832"),
        Input = FromHex("#252832"),
        Ring = FromHex("#5b8cff"),
        Overlay = new Color(0, 0, 0, 0.6f),
        Shadow = new Color(0, 0, 0, 0.5f),
        Radius = 0.5f
    };

    public static Theme Light => new()
    {
        Name = "light",
        Background = FromHex("#ffffff"),
        Foreground = FromHex("#101217"),
        Card = FromHex("#ffffff"),
        CardForeground = FromHex("#101217"),
        Popover = FromHex("#ffffff"),
        PopoverForeground = FromHex("#101217"),
        Primary = FromHex("#101217"),
        PrimaryForeground = FromHex("#fafafa"),
        Secondary = FromHex("#efeff2"),
        SecondaryForeground = FromHex("#101217"),
        Muted = FromHex("#f4f5f7"),
        MutedForeground = FromHex("#656d7d"),
        Accent = FromHex("#2268de"),
        AccentForeground = FromHex("#ffffff"),
        Destructive = FromHex("#dc3232"),
        DestructiveForeground = FromHex("#ffffff"),
        Success = FromHex("#1a8e4f"),
        SuccessForeground = FromHex("#ffffff"),
        Warning = FromHex("#de8c18"),
        WarningForeground = FromHex("#ffffff"),
        Info = FromHex("#2268de"),
        InfoForeground = FromHex("#ffffff"),
        Border = FromHex("#e1e3e7"),
        Input = FromHex("#e1e3e7"),
        Ring = FromHex("#2268de"),
        Overlay = new Color(0, 0, 0, 0.3f),
        Shadow = new Color(0, 0, 0, 0.08f),
        Radius = 0.5f
    };

    public static Theme Zinc => new()
    {
        Name = "zinc",
        Background = FromHex("#030406"),
        Foreground = FromHex("#f0f3f7"),
        Card = FromHex("#101318"),
        CardForeground = FromHex("#f0f3f7"),
        Popover = FromHex("#101318"),
        PopoverForeground = FromHex("#f0f3f7"),
        Primary = FromHex("#5c89ba"),
        PrimaryForeground = FromHex("#fcfdfe"),
        Secondary = FromHex("#101318"),
        SecondaryForeground = FromHex("#f0f3f7"),
        Muted = FromHex("#090c10"),
        MutedForeground = FromHex("#7b848f"),
        Accent = FromHex("#5c89ba"),
        AccentForeground = FromHex("#fcfdfe"),
        Destructive = FromHex("#dc3f4a"),
        DestructiveForeground = FromHex("#f0f3f7"),
        Success = FromHex("#1a8e4f"),
        SuccessForeground = FromHex("#ffffff"),
        Warning = FromHex("#fbba42"),
        WarningForeground = FromHex("#030406"),
        Info = FromHex("#3b82f6"),
        InfoForeground = FromHex("#ffffff"),
        Border = FromHex("#191f27"),
        Input = FromHex("#191f27"),
        Ring = FromHex("#5c89ba"),
        Overlay = new Color(0, 0, 0, 0.8f),
        Shadow = new Color(0, 0, 0, 0.5f),
        Radius = 0.5f
    };

    public static Theme Slate => new()
    {
        Name = "slate",
        Background = FromHex("#020617"),
        Foreground = FromHex("#f1f5f9"),
        Card = FromHex("#0f172a"),
        CardForeground = FromHex("#f1f5f9"),
        Popover = FromHex("#0f172a"),
        PopoverForeground = FromHex("#f1f5f9"),
        Primary = FromHex("#6d90ad"),
        PrimaryForeground = FromHex("#f8fafc"),
        Secondary = FromHex("#1e293b"),
        SecondaryForeground = FromHex("#f1f5f9"),
        Muted = FromHex("#0b1120"),
        MutedForeground = FromHex("#64748b"),
        Accent = FromHex("#6d90ad"),
        AccentForeground = FromHex("#f8fafc"),
        Destructive = FromHex("#ef4444"),
        DestructiveForeground = FromHex("#ffffff"),
        Success = FromHex("#10b981"),
        SuccessForeground = FromHex("#ffffff"),
        Warning = FromHex("#f59e0b"),
        WarningForeground = FromHex("#0c0c0e"),
        Info = FromHex("#3b82f6"),
        InfoForeground = FromHex("#ffffff"),
        Border = FromHex("#1e293b"),
        Input = FromHex("#1e293b"),
        Ring = FromHex("#6d90ad"),
        Overlay = new Color(0, 0, 0, 0.5f),
        Shadow = new Color(0, 0, 0, 0.3f),
        Radius = 0.5f
    };

    public static Theme Gray => new()
    {
        Name = "gray",
        Background = FromHex("#030303"),
        Foreground = FromHex("#f2f2f2"),
        Card = FromHex("#111111"),
        CardForeground = FromHex("#f2f2f2"),
        Popover = FromHex("#111111"),
        PopoverForeground = FromHex("#f2f2f2"),
        Primary = FromHex("#27272a"),
        PrimaryForeground = FromHex("#f2f2f2"),
        Secondary = FromHex("#27272a"),
        SecondaryForeground = FromHex("#f2f2f2"),
        Muted = FromHex("#0a0a0a"),
        MutedForeground = FromHex("#71717a"),
        Accent = FromHex("#3b82f6"),
        AccentForeground = FromHex("#ffffff"),
        Destructive = FromHex("#ef4444"),
        DestructiveForeground = FromHex("#ffffff"),
        Success = FromHex("#22c55e"),
        SuccessForeground = FromHex("#ffffff"),
        Warning = FromHex("#f59e0b"),
        WarningForeground = FromHex("#030303"),
        Info = FromHex("#3b82f6"),
        InfoForeground = FromHex("#ffffff"),
        Border = FromHex("#27272a"),
        Input = FromHex("#27272a"),
        Ring = FromHex("#3b82f6"),
        Overlay = new Color(0, 0, 0, 0.5f),
        Shadow = new Color(0, 0, 0, 0.3f),
        Radius = 0.5f
    };

    public static Theme Stone => new()
    {
        Name = "stone",
        Background = FromHex("#0c0a09"),
        Foreground = FromHex("#fafaf9"),
        Card = FromHex("#1c1917"),
        CardForeground = FromHex("#fafaf9"),
        Popover = FromHex("#1c1917"),
        PopoverForeground = FromHex("#fafaf9"),
        Primary = FromHex("#292524"),
        PrimaryForeground = FromHex("#fafaf9"),
        Secondary = FromHex("#292524"),
        SecondaryForeground = FromHex("#fafaf9"),
        Muted = FromHex("#141312"),
        MutedForeground = FromHex("#78716c"),
        Accent = FromHex("#b45309"),
        AccentForeground = FromHex("#fafaf9"),
        Destructive = FromHex("#ef4444"),
        DestructiveForeground = FromHex("#ffffff"),
        Success = FromHex("#22c55e"),
        SuccessForeground = FromHex("#ffffff"),
        Warning = FromHex("#f59e0b"),
        WarningForeground = FromHex("#0c0a09"),
        Info = FromHex("#3b82f6"),
        InfoForeground = FromHex("#ffffff"),
        Border = FromHex("#292524"),
        Input = FromHex("#292524"),
        Ring = FromHex("#b45309"),
        Overlay = new Color(0, 0, 0, 0.5f),
        Shadow = new Color(0, 0, 0, 0.3f),
        Radius = 0.5f
    };

    public static Theme Olive => new()
    {
        Name = "olive",
        Background = FromHex("#040604"),
        Foreground = FromHex("#f5f7f5"),
        Card = FromHex("#0e130e"),
        CardForeground = FromHex("#f5f7f5"),
        Popover = FromHex("#0e130e"),
        PopoverForeground = FromHex("#f5f7f5"),
        Primary = FromHex("#192119"),
        PrimaryForeground = FromHex("#f5f7f5"),
        Secondary = FromHex("#192119"),
        SecondaryForeground = FromHex("#f5f7f5"),
        Muted = FromHex("#090c09"),
        MutedForeground = FromHex("#848f84"),
        Accent = FromHex("#6dba6d"),
        AccentForeground = FromHex("#f5f7f5"),
        Destructive = FromHex("#ef4444"),
        DestructiveForeground = FromHex("#ffffff"),
        Success = FromHex("#22c55e"),
        SuccessForeground = FromHex("#ffffff"),
        Warning = FromHex("#f59e0b"),
        WarningForeground = FromHex("#040604"),
        Info = FromHex("#3b82f6"),
        InfoForeground = FromHex("#ffffff"),
        Border = FromHex("#192119"),
        Input = FromHex("#192119"),
        Ring = FromHex("#6dba6d"),
        Overlay = new Color(0, 0, 0, 0.5f),
        Shadow = new Color(0, 0, 0, 0.3f),
        Radius = 0.5f
    };

    public static Theme Cyan => new()
    {
        Name = "cyan",
        Background = FromHex("#020507"),
        Foreground = FromHex("#f3f7f9"),
        Card = FromHex("#0b1317"),
        CardForeground = FromHex("#f3f7f9"),
        Popover = FromHex("#0b1317"),
        PopoverForeground = FromHex("#f3f7f9"),
        Primary = FromHex("#162329"),
        PrimaryForeground = FromHex("#f3f7f9"),
        Secondary = FromHex("#162329"),
        SecondaryForeground = FromHex("#f3f7f9"),
        Muted = FromHex("#060c0f"),
        MutedForeground = FromHex("#7f97a3"),
        Accent = FromHex("#39baba"),
        AccentForeground = FromHex("#020507"),
        Destructive = FromHex("#ef4444"),
        DestructiveForeground = FromHex("#ffffff"),
        Success = FromHex("#22c55e"),
        SuccessForeground = FromHex("#ffffff"),
        Warning = FromHex("#f59e0b"),
        WarningForeground = FromHex("#020507"),
        Info = FromHex("#3b82f6"),
        InfoForeground = FromHex("#ffffff"),
        Border = FromHex("#162329"),
        Input = FromHex("#162329"),
        Ring = FromHex("#39baba"),
        Overlay = new Color(0, 0, 0, 0.5f),
        Shadow = new Color(0, 0, 0, 0.3f),
        Radius = 0.5f
    };

    public static Theme BlueDark => new()
    {
        Name = "blue-dark",
        Background = FromHex("#050912"),
        Foreground = FromHex("#f5f8ff"),
        Card = FromHex("#101726"),
        CardForeground = FromHex("#f5f8ff"),
        Popover = FromHex("#101726"),
        PopoverForeground = FromHex("#f5f8ff"),
        Primary = FromHex("#192335"),
        PrimaryForeground = FromHex("#f5f8ff"),
        Secondary = FromHex("#192335"),
        SecondaryForeground = FromHex("#f5f8ff"),
        Muted = FromHex("#0b101d"),
        MutedForeground = FromHex("#7f8ba3"),
        Accent = FromHex("#5182f4"),
        AccentForeground = FromHex("#f5f8ff"),
        Destructive = FromHex("#ef4444"),
        DestructiveForeground = FromHex("#ffffff"),
        Success = FromHex("#22c55e"),
        SuccessForeground = FromHex("#ffffff"),
        Warning = FromHex("#f59e0b"),
        WarningForeground = FromHex("#050912"),
        Info = FromHex("#3b82f6"),
        InfoForeground = FromHex("#ffffff"),
        Border = FromHex("#192335"),
        Input = FromHex("#192335"),
        Ring = FromHex("#5182f4"),
        Overlay = new Color(0, 0, 0, 0.5f),
        Shadow = new Color(0, 0, 0, 0.3f),
        Radius = 0.5f
    };

    public static Theme Rose => new()
    {
        Name = "rose",
        Background = FromHex("#070406"),
        Foreground = FromHex("#faf5f8"),
        Card = FromHex("#130b10"),
        CardForeground = FromHex("#faf5f8"),
        Popover = FromHex("#130b10"),
        PopoverForeground = FromHex("#faf5f8"),
        Primary = FromHex("#211319"),
        PrimaryForeground = FromHex("#faf5f8"),
        Secondary = FromHex("#211319"),
        SecondaryForeground = FromHex("#faf5f8"),
        Muted = FromHex("#0e090c"),
        MutedForeground = FromHex("#977784"),
        Accent = FromHex("#f45c89"),
        AccentForeground = FromHex("#070406"),
        Destructive = FromHex("#ef4444"),
        DestructiveForeground = FromHex("#ffffff"),
        Success = FromHex("#22c55e"),
        SuccessForeground = FromHex("#ffffff"),
        Warning = FromHex("#f59e0b"),
        WarningForeground = FromHex("#070406"),
        Info = FromHex("#3b82f6"),
        InfoForeground = FromHex("#ffffff"),
        Border = FromHex("#211319"),
        Input = FromHex("#211319"),
        Ring = FromHex("#f45c89"),
        Overlay = new Color(0, 0, 0, 0.5f),
        Shadow = new Color(0, 0, 0, 0.3f),
        Radius = 0.5f
    };

    public static Theme Violet => new()
    {
        Name = "violet",
        Background = FromHex("#050409"),
        Foreground = FromHex("#f8f5ff"),
        Card = FromHex("#0f0b14"),
        CardForeground = FromHex("#f8f5ff"),
        Popover = FromHex("#0f0b14"),
        PopoverForeground = FromHex("#f8f5ff"),
        Primary = FromHex("#1d1326"),
        PrimaryForeground = FromHex("#f8f5ff"),
        Secondary = FromHex("#1d1326"),
        SecondaryForeground = FromHex("#f8f5ff"),
        Muted = FromHex("#0b0810"),
        MutedForeground = FromHex("#8b779f"),
        Accent = FromHex("#9b5cf4"),
        AccentForeground = FromHex("#f8f5ff"),
        Destructive = FromHex("#ef4444"),
        DestructiveForeground = FromHex("#ffffff"),
        Success = FromHex("#22c55e"),
        SuccessForeground = FromHex("#ffffff"),
        Warning = FromHex("#f59e0b"),
        WarningForeground = FromHex("#050409"),
        Info = FromHex("#3b82f6"),
        InfoForeground = FromHex("#ffffff"),
        Border = FromHex("#1d1326"),
        Input = FromHex("#1d1326"),
        Ring = FromHex("#9b5cf4"),
        Overlay = new Color(0, 0, 0, 0.5f),
        Shadow = new Color(0, 0, 0, 0.3f),
        Radius = 0.5f
    };

    #endregion
}
