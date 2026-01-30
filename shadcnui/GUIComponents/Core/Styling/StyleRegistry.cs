using System.Collections.Generic;
using shadcnui.GUIComponents.Core.Base;
using shadcnui.GUIComponents.Core.Theming;
using UnityEngine;

namespace shadcnui.GUIComponents.Core.Styling
{
    public delegate void StyleModifier(GUIStyle style, Theme theme, GUIHelper helper);

    public class StyleRegistry
    {
        private readonly Dictionary<(StyleComponentType, ControlVariant), StyleModifier> _variantModifiers = new();
        private readonly Dictionary<(StyleComponentType, ControlSize), StyleModifier> _sizeModifiers = new();
        private readonly Dictionary<ControlVariant, StyleModifier> _globalVariantModifiers = new();
        private readonly Dictionary<ControlSize, StyleModifier> _globalSizeModifiers = new();

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
            if (_variantModifiers.TryGetValue((type, variant), out var modifier))
                return modifier;

            if (_globalVariantModifiers.TryGetValue(variant, out var globalModifier))
                return globalModifier;

            return null;
        }

        public StyleModifier GetSizeModifier(StyleComponentType type, ControlSize size)
        {
            if (_sizeModifiers.TryGetValue((type, size), out var modifier))
                return modifier;

            if (_globalSizeModifiers.TryGetValue(size, out var globalModifier))
                return globalModifier;

            return null;
        }

        public void Clear()
        {
            _variantModifiers.Clear();
            _sizeModifiers.Clear();
            _globalVariantModifiers.Clear();
            _globalSizeModifiers.Clear();
        }
    }
}
