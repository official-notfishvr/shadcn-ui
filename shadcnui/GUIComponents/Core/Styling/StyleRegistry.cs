using System.Collections.Generic;
using shadcnui.GUIComponents.Core.Base;
using shadcnui.GUIComponents.Core.Theming;
using UnityEngine;

namespace shadcnui.GUIComponents.Core.Styling
{
    public delegate void StyleModifier(GUIStyle style, Theme theme, GUIHelper helper);

    public class StyleRegistry
    {
        private Dictionary<(StyleComponentType, ControlVariant), StyleModifier> variantModifiers = new Dictionary<(StyleComponentType, ControlVariant), StyleModifier>();
        private Dictionary<(StyleComponentType, ControlSize), StyleModifier> sizeModifiers = new Dictionary<(StyleComponentType, ControlSize), StyleModifier>();

        private Dictionary<ControlVariant, StyleModifier> globalVariantModifiers = new Dictionary<ControlVariant, StyleModifier>();
        private Dictionary<ControlSize, StyleModifier> globalSizeModifiers = new Dictionary<ControlSize, StyleModifier>();

        public void RegisterVariant(StyleComponentType type, ControlVariant variant, StyleModifier modifier)
        {
            variantModifiers[(type, variant)] = modifier;
        }

        public void RegisterSize(StyleComponentType type, ControlSize size, StyleModifier modifier)
        {
            sizeModifiers[(type, size)] = modifier;
        }

        public StyleModifier GetVariantModifier(StyleComponentType type, ControlVariant variant)
        {
            if (variantModifiers.TryGetValue((type, variant), out var modifier))
            {
                return modifier;
            }

            if (globalVariantModifiers.TryGetValue(variant, out var globalModifier))
            {
                return globalModifier;
            }

            return null;
        }

        public StyleModifier GetSizeModifier(StyleComponentType type, ControlSize size)
        {
            if (sizeModifiers.TryGetValue((type, size), out var modifier))
            {
                return modifier;
            }

            if (globalSizeModifiers.TryGetValue(size, out var globalModifier))
            {
                return globalModifier;
            }

            return null;
        }

        public void Clear()
        {
            variantModifiers.Clear();
            sizeModifiers.Clear();
            globalVariantModifiers.Clear();
            globalSizeModifiers.Clear();
        }
    }
}
