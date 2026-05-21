# Reference

## Current API Shape

The repo uses a hybrid API:

- Builder-first entrypoints on `GUIHelper`
- Direct convenience wrappers for common one-line calls
- Implicit builder conversion for value-returning controls

## Builder Entry Points

Value-returning builders:

- `Button(string)` -> `bool`
- `Input(string)` -> `string`
- `Checkbox(string, bool)` -> `bool`
- `Switch(string, bool)` -> `bool`
- `Toggle(string, bool)` -> `bool`
- `Slider(float)` -> `float`
- `RangeSlider(float, float)` -> `Vector2`
- `Select()` -> `int`
- `TextArea(string)` -> `string`
- `Calendar()` -> `DateTime?`
- `DatePicker()` -> `DateTime?`
- `Tabs()` -> `int`
- `Navigation()` -> `int`

Void-like builders:

- `DropdownMenu()`
- `ThemeChanger()`
- `FontChanger()`
- `DataTable(string id)`
- `Label(string)`
- `Badge(string)`
- `Avatar()`
- `Progress(float)`
- `Chart()`
- `Dialog(string id)`
- `Popover(string id)`
- `Toast()`
- `Card()`
- `Separator()`
- `Table()`
- `MenuBar()`

## Direct Convenience Methods Restored

- `Button(string, ControlVariant, ControlSize, ...)`
- `Input(string value, string placeholder = null, bool disabled = false, GUILayoutOption[] opts = null)`
- `Checkbox(...)`
- `Switch(...)`
- `Toggle(string, bool, ControlVariant, ControlSize, ...)`
- `Label(string, ControlVariant, ...)`
- `Heading(string)`
- `Caption(string)`
- `MutedLabel(string)`
- `ErrorAlert(string)`
- `Badge(string, ControlVariant, ControlSize, ...)`
- `CountBadge(...)`
- `StatusBadge(...)`
- `Progress(float value, float width)`
- `BeginCard(...)`, `CardHeader(...)`, `CardContent(...)`, `CardFooter(...)`, `EndCard()`
- `BeginHorizontalGroup`, `EndHorizontalGroup`, `BeginVerticalGroup`, `EndVerticalGroup`
- `HorizontalSeparator`, `VerticalSeparator`, `AddSpace`

## Overlay / Lifecycle Notes

- Window draw flow:
  1. `UpdateGUI(true)`
  2. `BeginGUI()`
  3. Draw content
  4. `EndGUI()`
  5. Outside the window callback, call `DrawOverlays()`

- Public overlay helpers include:
  - `OpenDialog`, `CloseDialog`
  - `OpenPopover`, `ClosePopover`, `IsPopoverOpen`
  - `CloseSelect`, `IsSelectOpen`, `CloseDropdownMenu`
  - `CloseDatePicker`, `IsDatePickerOpen`
  - `ShowToast`, `DismissToast`, `DismissAllToasts`, `GetActiveToastCount`

## Examples

### Direct style

```csharp
_gui.Heading("Demo Launcher");
_gui.Badge("Flagship", ControlVariant.Secondary, ControlSize.Small);
_gui.HorizontalSeparator();

if (_gui.Button("Open", ControlVariant.Default, ControlSize.Small))
    OpenDemo();
```

### Builder style

```csharp
_selected = _gui.Select()
    .Label("Priority")
    .Items("Low", "Normal", "High")
    .SelectedIndex(_selected)
    .Width(240f);

_gui.Card()
    .Title("Relay")
    .Subtitle("North Wing")
    .Content("Builder-composed card")
    .Size(220f, 160f)
    .Render();
```

### Vertical tabs

```csharp
_tabIndex = _gui.Tabs()
    .Items("Overview", "Controls", "Overlay")
    .SelectedIndex(_tabIndex)
    .Side(TabSide.Left)
    .Content(DrawCurrentTab);
```

## Avoid

- `UpdateAnimations`
- `BeginAnimatedGUI`
- `EndAnimatedGUI`
- `DrawCard` / `DrawSimpleCard` as preferred examples
- old demo-only helper names unless they still exist in `GUIHelper`
- assuming every builder needs `.Render()`; value-returning ones do not
