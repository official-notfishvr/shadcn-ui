# Project structure design

## Caller usage

Existing consumers should continue to write the same code:

```csharp
using shadcnui.GUIComponents.Core.Base;
using shadcnui.GUIComponents.Controls;

var gui = new GUIHelper();
var value = gui.Input("name");
```

The build should continue to produce one `shadcnui.dll`, and the demo plugin should continue to embed that assembly under the same resource name.

## Current shape

The repository mixes source projects, samples, build configuration, Unity reference assemblies, screenshots, and generated `bin`/`obj` output at the root. The library itself is one assembly. Its public namespace layout is already used by the README and by the demos:

```text
shadcnui.GUIComponents
├── Core.Base       runtime, builders, GUIHelper, layers
├── Core.Styling    styles, tokens, textures
├── Core.Theming    themes
├── Core.Utils      configs, animation, Unity helpers, logging
├── Controls
├── Data
├── Display
└── Layout
```

The assembly and namespace layout are deployment-facing. The directory names around the projects are not.

## Candidate A: split the library into assemblies

```text
src/
├── ShadcnUi.Core/
├── ShadcnUi.Controls/
├── ShadcnUi.Data/
├── ShadcnUi.Display/
└── ShadcnUi.Layout/
samples/
└── ShadcnUi.Demo/
```

Each feature area would become a project and reference the core project. This gives compile-time dependency boundaries, but it changes the plugin packaging model from one embedded DLL to several assemblies. The current code also shares configs, builders, styling, and rendering helpers across every feature area, so the split would mostly create project references rather than meaningful ownership.

## Candidate B: organize projects by responsibility, keep one library assembly

```text
src/
└── ShadcnUi/
    └── GUIComponents/
        ├── Core/
        ├── Controls/
        ├── Data/
        ├── Display/
        └── Layout/
samples/
├── ShadcnUi.Demo/
└── MonoBepInEx/
References/
docs/
Screenshots/
Directory.Build.props
build/
shadcnui.slnx
```

The runtime library remains one project and one assembly. Source categories remain recognizable, while project-only concerns move out of the repository root. Shared MSBuild settings live once at the root. Project-specific packaging targets stay with the project that owns the package.

## Type and module sketch

No new runtime types are needed. The existing public contract remains the contract:

```text
GUIHelper                owns the public fluent/direct rendering entry points
BaseComponent            owns component lifetime and shared rendering state
ComponentBuilder<...>    owns fluent configuration and value conversion
LayerManager             owns overlays and z-order
StyleManager             owns appearance and design tokens
ThemeManager             owns theme selection
Controls/Data/Display/Layout
                          own concrete component implementations
```

The project boundary is `ShadcnUi`, not each component category. Moving a file must not require a namespace change. The demo and BepInEx sample are consumers, not library source.

## Synthesis decision

Use Candidate B. It gives the repository a clear top-level grammar and removes duplicated build configuration without changing the API or plugin deployment format. Candidate A has a smaller source tree on paper, but its extra assembly boundaries expose packaging and load-order problems that the current library does not need.

The implementation will:

1. Move the library project under `src/ShadcnUi`.
2. Move the demo and standalone example under `samples`.
3. Keep `GUIComponents` and all `shadcnui.GUIComponents.*` namespaces unchanged.
4. Add root MSBuild configuration for shared game/reference paths.
5. Keep demo/example resource names and project assembly names unchanged.
6. Include both sample projects in `shadcnui.slnx`.
7. Leave generated output out of the source layout and verify the solution after the move.
