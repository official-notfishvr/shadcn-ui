using UnityEngine;

namespace ShadcnUI.Core.Styling;

/// <summary>
/// Component variant types.
/// </summary>
public enum ControlVariant
{
    Default,
    Primary,
    Secondary,
    Destructive,
    Outline,
    Ghost,
    Link,
    Muted,
    Success
}

/// <summary>
/// Component size types.
/// </summary>
public enum ControlSize
{
    Default,
    Small,
    Large,
    Mini,
    Icon
}

/// <summary>
/// Component state for styling.
/// </summary>
public enum ComponentState
{
    Default,
    Hover,
    Active,
    Focused,
    Disabled,
    Checked,
    Selected
}

/// <summary>
/// Style key for caching component styles.
/// </summary>
public readonly record struct StyleKey(
    StyleComponentType Type,
    ControlVariant Variant = ControlVariant.Default,
    ControlSize Size = ControlSize.Default,
    ComponentState State = ComponentState.Default
) : IEquatable<StyleKey>;

/// <summary>
/// Component types for style lookup.
/// </summary>
public enum StyleComponentType
{
    Button,
    Input,
    InputFocused,
    Label,
    Checkbox,
    CheckboxSolid,
    Switch,
    Select,
    SelectItem,
    Slider,
    SliderTrack,
    SliderThumb,
    SliderFill,
    Card,
    CardHeader,
    CardTitle,
    CardDescription,
    CardContent,
    CardFooter,
    Tabs,
    TabsList,
    TabsTrigger,
    TabsContent,
    Separator,
    Badge,
    Avatar,
    Progress,
    ProgressCircular,
    Table,
    TableHeader,
    TableRow,
    TableCell,
    Dialog,
    DialogHeader,
    DialogFooter,
    Popover,
    Tooltip,
    DropdownMenu,
    DropdownMenuItem,
    Toast,
    Alert,
    Skeleton,
    Navigation,
    MenuBar,
    MenuBarItem,
    Breadcrumb,
    RadioGroup,
    RadioItem,
    Calendar,
    Chart,
    Text,
    Stack
}

/// <summary>
/// Style override options for runtime customization.
/// </summary>
public readonly record struct StyleOverride(
    Color? BackgroundColor = null,
    Color? ForegroundColor = null,
    Color? BorderColor = null,
    float? BorderRadius = null,
    float? BorderWidth = null,
    GUIStyle? BaseStyle = null
);
