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
        public Color? HoverBackgroundColor { get; set; }
        public Color? ActiveBackgroundColor { get; set; }
        public Color? FocusedBackgroundColor { get; set; }
        public Color? ForegroundColor { get; set; }
        public Color? HoverForegroundColor { get; set; }
        public Color? ActiveForegroundColor { get; set; }
        public Color? FocusedForegroundColor { get; set; }
        public Color? BorderColor { get; set; }
        public Color? HoverBorderColor { get; set; }
        public Color? ActiveBorderColor { get; set; }
        public Color? AccentColor { get; set; }
        public float? BorderRadius { get; set; }
        public float? BorderThickness { get; set; }
        public StatefulStyleModifier Modifier { get; set; }

        internal bool IsInlineOverride =>
            TemplateStyle != null
            || ReplaceBaseStyle
            || BackgroundColor.HasValue
            || HoverBackgroundColor.HasValue
            || ActiveBackgroundColor.HasValue
            || FocusedBackgroundColor.HasValue
            || ForegroundColor.HasValue
            || HoverForegroundColor.HasValue
            || ActiveForegroundColor.HasValue
            || FocusedForegroundColor.HasValue
            || BorderColor.HasValue
            || HoverBorderColor.HasValue
            || ActiveBorderColor.HasValue
            || AccentColor.HasValue
            || BorderRadius.HasValue
            || BorderThickness.HasValue
            || Modifier != null;
    }
}
