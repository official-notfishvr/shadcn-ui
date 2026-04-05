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
        private readonly Dictionary<(StyleComponentType Type, string StyleId), ComponentAppearance> _styles = new();

        public StyleModifier GetVariantModifier(StyleComponentType type, ControlVariant variant)
        {
            return _variantModifiers.TryGetValue((type, variant), out var modifier) ? modifier : null;
        }

        public StyleModifier GetSizeModifier(StyleComponentType type, ControlSize size)
        {
            return _sizeModifiers.TryGetValue((type, size), out var modifier) ? modifier : null;
        }

        public void RegisterStyle(StyleComponentType type, string styleId, ComponentAppearance profile)
        {
            if (string.IsNullOrWhiteSpace(styleId) || profile == null)
                return;

            _styles[(type, styleId)] = profile;
        }

        public void RegisterStyle(StyleComponentType type, string styleId, StatefulStyleModifier modifier)
        {
            if (modifier == null)
                return;

            RegisterStyle(type, styleId, new ComponentAppearance { Modifier = modifier });
        }

        public bool UnregisterStyle(StyleComponentType type, string styleId)
        {
            return !string.IsNullOrWhiteSpace(styleId) && _styles.Remove((type, styleId));
        }

        public ComponentAppearance GetStyle(StyleComponentType type, string styleId)
        {
            return !string.IsNullOrWhiteSpace(styleId) && _styles.TryGetValue((type, styleId), out var profile) ? profile : null;
        }

        public void Clear()
        {
            _variantModifiers.Clear();
            _sizeModifiers.Clear();
            _styles.Clear();
        }
    }
}
