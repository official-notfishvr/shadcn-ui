# AGENTS

## Agent Skills

### `shadcnui-gui-lib`
Location: [docs/agents/shadcnui-gui-lib/SKILL.md](docs/agents/shadcnui-gui-lib/SKILL.md)

Teaches agents how to use and extend this Unity IMGUI library's current `GUIHelper` API. Use when adding demos, fixing component calls, migrating old helper usage, or generating new UI with this library.

### Notes

- Prefer the current `GUIHelper` API, not the older removed helper surface shown in stale docs or screenshots.
- The library supports both builder-style calls and direct convenience methods.
- Value-returning builders support implicit conversion, so assignments and `if` conditions do not need `.Render()`.
- For details and examples, read the skill file above before editing demo or library usage.
