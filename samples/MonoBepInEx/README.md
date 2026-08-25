# mono bepinex example

Release builds keep a DLL-specific linker pass enabled by default. It analyzes the
compiled sample plugin, removes unreachable library methods from
`Libs/shadcnui.dll`, writes the reduced working copy under `obj`, and replaces
the embedded resource in the built plugin. No additional DLL is required in
`Libs`. Set `TrimShadcnUi=false` to skip the pass when debugging the full library.
