---
name: shadcnui-gui-lib
description: Guides agents to write, migrate, and debug UI code against this repo's current Unity IMGUI `GUIHelper` API. Use when editing demos, adding components, converting old helper calls, or generating new shadcnui-based interfaces in C#.
---

# shadcnui GUI Lib

## Quick Start

Use `GUIHelper` inside a Unity `OnGUI` window:

```csharp
private GUIHelper _gui;

void Start() => _gui = new GUIHelper();

void OnGUI()
{
    _windowRect = GUI.Window(1, _windowRect, DrawWindow, "Demo");
    _gui.DrawOverlays();
}

void DrawWindow(int id)
{
    _gui.UpdateGUI(true);
    if (!_gui.BeginGUI())
        return;

    _enabled = _gui.Toggle("Enabled", _enabled);
    _name = _gui.Input(_name).Label("Name");
    if (_gui.Button("Save", ControlVariant.Outline, ControlSize.Small))
        Save();

    _gui.EndGUI();
    GUI.DragWindow();
}
```

## Rules

- Prefer the current `GUIHelper` API. Do not reintroduce removed legacy helpers unless the repo already restored them on purpose.
- Builders are the default composition style: `_gui.Button("Save").Outline().Small()`.
- Direct helpers are valid for common calls: `_gui.Button("Save", ControlVariant.Outline, ControlSize.Small)`.
- Value-returning builders do not require `.Render()` because implicit conversion is supported.
- Void-like builders still need `.Render()` unless a direct wrapper exists.
- Preserve `BeginGUI()` / `EndGUI()` and call `_gui.DrawOverlays()` after the window for dialogs, popovers, tooltips, and toasts.

## Common Patterns

### Value-returning, no `.Render()`

```csharp
_featureToggle = _gui.Toggle("Feature Flag", _featureToggle);
_email = _gui.Input(_email).Label("Email").Placeholder("name@example.com");
_volume = _gui.Slider(_volume).Label("Volume").Range(0f, 1f).Step(0.05f).ShowValue();
_tabIndex = _gui.Tabs().Items("Overview", "Settings").SelectedIndex(_tabIndex).Content(DrawTab);
```

### Void builder, with `.Render()`

```csharp
_gui.Badge("Online").StatusDot().Render();
_gui.Card().Title("Relay").Content("All systems nominal").Render();
_gui.Toast().Title("Saved").Description("Changes applied").Variant(ToastVariant.Success).Render();
```

### Direct convenience API

```csharp
_gui.Heading("Launcher");
_gui.MutedLabel("Choose a demo");
_gui.CountBadge(4, ControlVariant.Secondary);
_gui.HorizontalSeparator();
_gui.AddSpace(12f);
```

## Tabs

- Horizontal tabs: use `.Position(TabPosition.Top|Bottom)`.
- Vertical tabs: use `.Side(TabSide.Left|Right)` or `.Position(TabPosition.Left|Right)`.
- `Side(...)` is the preferred builder call for vertical tabs in demos.

## Known Good Surface

- Layout: `BeginHorizontalGroup`, `BeginVerticalGroup`, `EndHorizontalGroup`, `EndVerticalGroup`, `AddSpace`, `ScrollView`
- Text/display: `Label`, `Heading`, `Caption`, `MutedLabel`, `Badge`, `CountBadge`, `StatusBadge`
- Controls: `Button`, `Input`, `Checkbox`, `Switch`, `Toggle`, `Slider`, `RangeSlider`, `Select`, `DropdownMenu`
- Data/display: `TextArea`, `DatePicker`, `Table`, `DataTable`, `Chart`
- Overlay: `Dialog`, `Popover`, `Toast`, `DrawOverlays`
- Cards: `Card()` builder and `BeginCard` / `CardHeader` / `CardContent` / `CardFooter` / `EndCard`

## Migration Guidance

- Replace stale calls like `DrawButton`, `DrawCard`, `BeginAnimatedGUI`, `UpdateAnimations`, and similar removed APIs with the current `GUIHelper` surface.
- If demo code expects direct-return behavior, first check whether `GUIHelper` already has a direct wrapper or whether the builder has implicit conversion.
- When changing tabs, ensure vertical layouts use `Side(...)` or `Position(Left|Right)`, not only a side flag stored in config.

## References

- See [REFERENCE.md](REFERENCE.md) for a larger API map and examples.
