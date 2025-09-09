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

### Basic Example (Unity)

To use `shadcnui` components in your Unity project, you'll typically instantiate `GUIHelper` and use its methods within an `OnGUI()` function or a custom GUI window.

```csharp
using UnityEngine;
using shadcnui;
using shadcnui.GUIComponents;

public class MyUIExample : MonoBehaviour
{
    private GUIHelper guiHelper;
    private bool checkboxValue = false;
    private float sliderValue = 50f;
    private string inputValue = "Hello shadcnui!";

    void Start()
    {
        guiHelper = new GUIHelper();
    }

    void OnGUI()
    {
        // Begin a vertical layout group
        GUILayout.BeginVertical(GUILayout.Width(300));

        // Display a label
        guiHelper.Label("Welcome to shadcnui!", LabelVariant.Default);
        guiHelper.MutedLabel("A C# UI component library for Unity.");

        guiHelper.HorizontalSeparator();

        // Example Button
        if (guiHelper.Button("Click Me!"))
        {
            Debug.Log("Button Clicked!");
        }

        guiHelper.HorizontalSeparator();

        // Example Checkbox
        checkboxValue = guiHelper.Checkbox("Enable Feature", checkboxValue);
        guiHelper.Label($"Feature Enabled: {checkboxValue}");

        guiHelper.HorizontalSeparator();

        // Example Slider
        sliderValue = guiHelper.DrawSlider(280, "Volume", ref sliderValue, 0, 100);
        guiHelper.Label($"Volume: {sliderValue:F0}");

        guiHelper.HorizontalSeparator();

        // Example Input Field
        inputValue = guiHelper.RenderGlowInputField(inputValue, 0, "Enter text...", 280);
        guiHelper.Label($"Input Text: {inputValue}");

        // End the vertical layout group
        GUILayout.EndVertical();
    }
}
```

the `shadcnui testing/Menu/UI.cs` file has more examples.

## Roadmap

* Fix ALL Bugs

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
