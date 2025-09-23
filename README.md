# shadcnui

A C# UI component library, inspired by the design principles of shadcn/ui, providing a set of reusable and customizable GUI components for .NET applications.

## Installation

To use `shadcnui` in your C# project:

1.  **Clone the repository:**
    ```bash
    git clone https://github.com/official-notfishvr/shadcn-ui.git
    cd shadcnui
    ```

2.  **Open the solution:**
    Open `shadcnui.sln` in Visual Studio.

3.  **Build the solution:**
    Build the `shadcnui` project to compile the component library.

4.  **Reference the library:**
    In your target C# project, add a reference to the compiled `shadcnui.dll` (found in `shadcnui/bin/Debug/` or `shadcnui/bin/Release/` after building).

## Usage

Once referenced, you can integrate `shadcnui` components into your application. This library is designed with Unity's IMGUI system in mind, leveraging `GUILayout` for flexible UI layouts.

For more examples, please see the `shadcnui testing` project in the solution.

## Example

Here is a basic example of how to use the `Button` component:

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
            windowRect = GUI.Window(102, windowRect, (GUI.WindowFunction)DrawWindow, "Button Demo");
        }
    }

    void DrawWindow(int windowID)
    {
        guiHelper.BeginVerticalGroup(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
        guiHelper.Label("Button", LabelVariant.Default);
        guiHelper.MutedLabel("Displays a button or a clickable element that activates an event.");
        guiHelper.HorizontalSeparator();

        guiHelper.Label("Button Variants", LabelVariant.Default);
        guiHelper.BeginHorizontalGroup();
        guiHelper.Button("Default");
        guiHelper.Button("Destructive", ButtonVariant.Destructive);
        guiHelper.Button("Outline", ButtonVariant.Outline);
        guiHelper.Button("Secondary", ButtonVariant.Secondary);
        guiHelper.Button("Ghost", ButtonVariant.Ghost);
        guiHelper.Button("Link", ButtonVariant.Link);
        guiHelper.EndHorizontalGroup();
        guiHelper.Label("Code: guiHelper.Button(label, variant, size, onClick, disabled);", LabelVariant.Muted);
        guiHelper.HorizontalSeparator();

        guiHelper.Label("Button Sizes", LabelVariant.Default);
        guiHelper.BeginHorizontalGroup();
        guiHelper.Button("Default", ButtonVariant.Default, ButtonSize.Default);
        guiHelper.Button("Small", ButtonVariant.Default, ButtonSize.Small);
        guiHelper.Button("Large", ButtonVariant.Default, ButtonSize.Large);
        guiHelper.Button("Icon", ButtonVariant.Default, ButtonSize.Icon);
        guiHelper.EndHorizontalGroup();
        guiHelper.Label("Code: guiHelper.Button(label, variant, size);", LabelVariant.Muted);
        guiHelper.HorizontalSeparator();
        
        GUILayout.EndVertical();
        GUI.DragWindow();
    }
}
```

## Todo

* Fix Styles that are borken if there is any
* Fix the GUIComponents not ending of the end of the gui
* Fix the Custom Font not working for table

## Contribution Guidelines

We welcome contributions to the `shadcnui` project! To contribute:

1.  **Fork the repository.**
2.  **Create a new branch** for your feature or bug fix: `git checkout -b feature/your-feature-name` or `bugfix/your-bug-fix`.
3.  **Make your changes** and ensure they adhere to the existing coding style.
4.  **Write unit tests** for new features or bug fixes.
5.  **Ensure all tests pass.**
6.  **Commit your changes** with a clear and concise commit message.
7.  **Push your branch** to your forked repository.
8.  **Open a Pull Request** to the `main` branch of the original repository.

Please ensure your pull requests are well-documented and address a specific issue or feature.
