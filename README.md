# shadcnui

A C# UI component library inspired by shadcn/ui. for .NET applications using Unity's IMGUI system.

## Installation

Clone the repo and open the solution in Visual Studio:

```bash
git clone https://github.com/official-notfishvr/shadcn-ui.git
cd shadcnui
```

Then open `shadcnui.sln` and build the project. You'll find `shadcnui.dll` in `bin/Debug/` or `bin/Release/`.

Add it as a reference to your C# project.

## Usage

```csharp
using shadcnui;
using shadcnui.GUIComponents;
using UnityEngine;

public class ExampleUI : MonoBehaviour
{
    private GUIHelper guiHelper;
    private Rect windowRect = new Rect(20, 20, 400, 500);
    private bool showWindow = true;

    void Start()
    {
        guiHelper = new GUIHelper();
    }

    void OnGUI()
    {
        if (showWindow)
        {
            windowRect = GUI.Window(0, windowRect, DrawWindow, "UI Demo");
        }
    }

    void DrawWindow(int id)
    {
        guiHelper.UpdateAnimations(showWindow);
        if (guiHelper.BeginAnimatedGUI())
        {
            guiHelper.BeginVerticalGroup();
            
            guiHelper.Label("Buttons", ControlVariant.Default);
            guiHelper.Button("Default");
            guiHelper.Button("Destructive", ControlVariant.Destructive);
            guiHelper.Button("Secondary", ControlVariant.Secondary);
            
            guiHelper.EndVerticalGroup();
            guiHelper.EndAnimatedGUI();
        }
        GUI.DragWindow();
    }
}
```

## Components

Buttons, cards, inputs, badges, toggles, tables, dialogs, tabs, and more.

### Buttons

```csharp
guiHelper.Button("Default");
guiHelper.Button("Destructive", ControlVariant.Destructive);
guiHelper.Button("Small", ControlVariant.Default, ControlSize.Small);
guiHelper.Button("Large", ControlVariant.Default, ControlSize.Large);
```

### Cards

```csharp
guiHelper.DrawCard("Title", "Subtitle", "Content here", () => guiHelper.Button("Action"), 200, 150);
guiHelper.DrawSimpleCard("Simple content", 200, 100);

guiHelper.BeginCard(200, 150);
guiHelper.CardHeader(() => guiHelper.CardTitle("Title"));
guiHelper.CardContent(() => guiHelper.Label("Content"));
guiHelper.CardFooter(() => guiHelper.Button("Button"));
guiHelper.EndCard();
```

### Inputs

```csharp
string password = "";
guiHelper.DrawPasswordField(300, "Password", ref password);

string text = "";
text = guiHelper.TextArea(text, ControlVariant.Default, "Placeholder");
text = guiHelper.OutlineTextArea(text, "Outline");

float height = 100f;
text = guiHelper.ResizableTextArea(text, ref height, "Resize me");
```

## Gallery

Check out what's possible with shadcnui:

<div align="center">

<img src="Screenshots/FullDemo_01_Button.png" alt="Button" width="45%">
<img src="Screenshots/FullDemo_04_Toggle.png" alt="Toggle" width="45%">

<img src="Screenshots/FullDemo_09_Card.png" alt="Card" width="45%">
<img src="Screenshots/FullDemo_12_Label.png" alt="Label" width="45%">

<img src="Screenshots/FullDemo_05_Checkbox.png" alt="Checkbox" width="45%">

</div>

## Embedding the Library

To bundle shadcnui.dll with your project for distribution:

1. Copy `shadcnui.dll` to a `Libs` folder in your project
2. Update your `.csproj`:

```xml
<ItemGroup>
    <Reference Include="shadcnui">
        <HintPath>Libs/shadcnui.dll</HintPath>
        <Private>false</Private>
    </Reference>
    <EmbeddedResource Include="Libs/shadcnui.dll" />
</ItemGroup>
```

3. Add the assembly loader to your project:

```csharp
using System;
using System.Reflection;

namespace YourNamespace
{
    public static class AssemblyLoader
    {
        static AssemblyLoader()
        {
            AppDomain.CurrentDomain.AssemblyResolve += (sender, args) =>
            {
                if (args.Name.Contains("shadcnui"))
                {
                    using (var stream = Assembly.GetExecutingAssembly()
                        .GetManifestResourceStream("YourNamespace.Libs.shadcnui.dll"))
                    {
                        if (stream == null) return null;
                        byte[] data = new byte[stream.Length];
                        stream.Read(data, 0, data.Length);
                        return Assembly.Load(data);
                    }
                }
                return null;
            };
        }

        public static void Init() { }
    }
}
```

The resource name follows the pattern: `{YourNamespace}.{PathToFile}` with slashes replaced by dots. For example, if your namespace is `MyApp` and the DLL is at `Libs/shadcnui.dll`, use `MyApp.Libs.shadcnui.dll`.

4. Call `AssemblyLoader.Init()` in your entry point:

```csharp
static void Main()
{
    AssemblyLoader.Init();
    // ...
}
```

## Known Issues

- Some styles may have edge cases that need fixing
- Custom fonts don't fully work with IL2CPP

## Star History

[![Star History Chart](https://api.star-history.com/svg?repos=official-notfishvr/shadcn-ui&type=Timeline)](https://star-history.com/#official-notfishvr/shadcn-ui&Timeline)


## Contributing

1. Fork the repo
2. Create a branch: `git checkout -b feature/your-feature`
3. Make your changes and add tests
4. Commit: `git commit -m "description"`
5. Push: `git push origin feature/your-feature`
6. Open a PR to `main`

Make sure your code follows the existing style and that tests pass.

## License

MIT
