using System.Collections.Generic;
using shadcnui.GUIComponents.Core.Base;
using shadcnui.GUIComponents.Core.Theming;
using UnityEngine;

namespace shadcnui.GUIComponents.Core.Styling
{
    public delegate void StyleModifier(GUIStyle style, Theme theme, GUIHelper helper);

    public sealed class StyleRegistry
    {
        private readonly Dictionary<(StyleComponentType Type, ControlVariant Variant), StyleModifier> _variantModifiers = new();
        private readonly Dictionary<(StyleComponentType Type, ControlSize Size), StyleModifier> _sizeModifiers = new();

        public void RegisterVariant(StyleComponentType type, ControlVariant variant, StyleModifier modifier)
        {
            _variantModifiers[(type, variant)] = modifier;
        }

        public void RegisterSize(StyleComponentType type, ControlSize size, StyleModifier modifier)
        {
            _sizeModifiers[(type, size)] = modifier;
        }

        public StyleModifier GetVariantModifier(StyleComponentType type, ControlVariant variant)
        {
            return _variantModifiers.TryGetValue((type, variant), out var modifier) ? modifier : null;
        }

        public StyleModifier GetSizeModifier(StyleComponentType type, ControlSize size)
        {
            return _sizeModifiers.TryGetValue((type, size), out var modifier) ? modifier : null;
        }

        public void Clear()
        {
            _variantModifiers.Clear();
            _sizeModifiers.Clear();
        }
    }
}
