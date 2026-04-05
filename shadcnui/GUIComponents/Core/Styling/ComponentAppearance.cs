using System;
using shadcnui.GUIComponents.Core.Base;
using shadcnui.GUIComponents.Core.Theming;
using shadcnui.GUIComponents.Core.Utils;
using UnityEngine;

namespace shadcnui.GUIComponents.Core.Styling
{
    public delegate void StatefulStyleModifier(GUIStyle style, Theme theme, GUIHelper helper, int state);

    public sealed class ComponentAppearance
    {
        public string StyleId { get; set; }
        public UnityHelpers.GUIStyle TemplateStyle { get; set; }
        public bool ReplaceBaseStyle { get; set; }
        public Color? BackgroundColor { get; set; }
        public Color? ForegroundColor { get; set; }
        public Color? BorderColor { get; set; }
        public Color? AccentColor { get; set; }
        public float? BorderRadius { get; set; }
        public float? BorderThickness { get; set; }
        public StatefulStyleModifier Modifier { get; set; }

        internal bool IsInlineOverride => TemplateStyle != null || ReplaceBaseStyle || BackgroundColor.HasValue || ForegroundColor.HasValue || BorderColor.HasValue || AccentColor.HasValue || BorderRadius.HasValue || BorderThickness.HasValue || Modifier != null;
    }
}
