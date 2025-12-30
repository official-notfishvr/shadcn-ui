using System;
using UnityEngine;
#if IL2CPP_MELONLOADER_PRE57
using UnhollowerBaseLib;
using shadcnui.GUIComponents;
#endif

namespace shadcnui.GUIComponents.Core.Utils
{
    public static class UnityHelpers
    {
        public class GUIStyle
        {
            private UnityEngine.GUIStyle _style;

            public float fixedWidth
            {
                get => _style.fixedWidth;
                set => _style.fixedWidth = value;
            }
            public float fixedHeight
            {
                get => _style.fixedHeight;
                set => _style.fixedHeight = value;
            }
            public UnityEngine.RectOffset margin
            {
                get => _style.margin;
                set => _style.margin = value;
            }
            public bool richText
            {
                get => _style.richText;
                set => _style.richText = value;
            }
            public UnityEngine.RectOffset padding
            {
                get => _style.padding;
                set => _style.padding = value;
            }
            public UnityEngine.RectOffset border
            {
                get => _style.border;
                set => _style.border = value;
            }
            public UnityEngine.RectOffset overflow
            {
                get => _style.overflow;
                set => _style.overflow = value;
            }
            public TextAnchor alignment
            {
                get => _style.alignment;
                set => _style.alignment = value;
            }
            public FontStyle fontStyle
            {
                get => _style.fontStyle;
                set => _style.fontStyle = value;
            }
            public int fontSize
            {
                get => _style.fontSize;
                set => _style.fontSize = value;
            }
            public UnityEngine.Font font
            {
                get => _style.font;
                set => _style.font = value;
            }
            public GUIStyleState normal
            {
                get => _style.normal;
                set => _style.normal = value;
            }
            public GUIStyleState hover
            {
                get => _style.hover;
                set => _style.hover = value;
            }
            public GUIStyleState active
            {
                get => _style.active;
                set => _style.active = value;
            }
            public GUIStyleState focused
            {
                get => _style.focused;
                set => _style.focused = value;
            }
            public GUIStyleState onNormal
            {
                get => _style.onNormal;
                set => _style.onNormal = value;
            }
            public GUIStyleState onHover
            {
                get => _style.onHover;
                set => _style.onHover = value;
            }
            public GUIStyleState onActive
            {
                get => _style.onActive;
                set => _style.onActive = value;
            }
            public GUIStyleState onFocused
            {
                get => _style.onFocused;
                set => _style.onFocused = value;
            }
            public TextClipping clipping
            {
                get => _style.clipping;
                set => _style.clipping = value;
            }
            public ImagePosition imagePosition
            {
                get => _style.imagePosition;
                set => _style.imagePosition = value;
            }
            public Vector2 contentOffset
            {
                get => _style.contentOffset;
                set => _style.contentOffset = value;
            }
            public bool wordWrap
            {
                get => _style.wordWrap;
                set => _style.wordWrap = value;
            }
            public bool stretchWidth
            {
                get => _style.stretchWidth;
                set => _style.stretchWidth = value;
            }
            public bool stretchHeight
            {
                get => _style.stretchHeight;
                set => _style.stretchHeight = value;
            }
            public string name
            {
                get => _style.name;
                set => _style.name = value;
            }
            public float lineHeight => _style.lineHeight;

            public GUIStyle(UnityEngine.GUIStyle style)
            {
#if IL2CPP_BEPINEX
                _style = new UnityEngine.GUIStyle();
                _style.m_Ptr = style.m_Ptr;
#else
                _style = new UnityEngine.GUIStyle(style);
#endif
            }

            public GUIStyle()
            {
                _style = new UnityEngine.GUIStyle();
            }

            public static implicit operator UnityEngine.GUIStyle(GUIStyle style) => style?._style;

            public static implicit operator GUIStyle(UnityEngine.GUIStyle style) => style != null ? new GUIStyle(style) : null;

            public UnityEngine.GUIStyle GetInternalStyle() => _style;

            public Vector2 CalcSize(GUIContent content) => _style.CalcSize((UnityEngine.GUIContent)content);

            public Vector2 CalcSize(string text) => _style.CalcSize(new UnityEngine.GUIContent(text));

            public float CalcHeight(GUIContent content, float width) => _style.CalcHeight((UnityEngine.GUIContent)content, width);

            public void CopyFrom(GUIStyle other)
            {
                if (other == null)
                    return;
                fixedWidth = other.fixedWidth;
                fixedHeight = other.fixedHeight;
                richText = other.richText;
                alignment = other.alignment;
                fontStyle = other.fontStyle;
                fontSize = other.fontSize;
                font = other.font;
                clipping = other.clipping;
                imagePosition = other.imagePosition;
                contentOffset = other.contentOffset;
                wordWrap = other.wordWrap;
                stretchWidth = other.stretchWidth;
                stretchHeight = other.stretchHeight;
            }
        }

        public class RectOffset
        {
            private UnityEngine.RectOffset _offset;

            public int left
            {
                get => _offset.left;
                set => _offset.left = value;
            }
            public int right
            {
                get => _offset.right;
                set => _offset.right = value;
            }
            public int top
            {
                get => _offset.top;
                set => _offset.top = value;
            }
            public int bottom
            {
                get => _offset.bottom;
                set => _offset.bottom = value;
            }
            public int horizontal => _offset.horizontal;
            public int vertical => _offset.vertical;

            public RectOffset(int left, int right, int top, int bottom)
            {
                _offset = new UnityEngine.RectOffset
                {
                    left = left,
                    right = right,
                    top = top,
                    bottom = bottom,
                };
            }

            public RectOffset()
            {
                _offset = new UnityEngine.RectOffset();
            }

            public static implicit operator UnityEngine.RectOffset(RectOffset offset) => offset?._offset;

            public static implicit operator RectOffset(UnityEngine.RectOffset offset) => offset != null ? new RectOffset(offset.left, offset.right, offset.top, offset.bottom) : null;

            public UnityEngine.RectOffset GetInternalRectOffset() => _offset;

            public void Set(int left, int right, int top, int bottom)
            {
                _offset.left = left;
                _offset.right = right;
                _offset.top = top;
                _offset.bottom = bottom;
            }

            public void SetAll(int value) => Set(value, value, value, value);
        }

        public class Font
        {
            private UnityEngine.Font _font;

            public string name => _font?.name ?? string.Empty;
            public int fontSize => _font?.fontSize ?? 0;
            public bool dynamic => _font?.dynamic ?? false;

            public Font(string name)
            {
#if IL2CPP_BEPINEX || IL2CPP_MELONLOADER_PRE57 || IL2CPP_MELONLOADER
                _font = UnityEngine.Font.CreateDynamicFontFromOSFont(name, 14);
#else
                _font = new UnityEngine.Font(name);
#endif
            }

            public Font(UnityEngine.Font font)
            {
                _font = font;
            }

            public static implicit operator UnityEngine.Font(Font font) => font?._font;

            public static implicit operator Font(UnityEngine.Font font) => font != null ? new Font(font) : null;

            public UnityEngine.Font GetInternalFont() => _font;

            public static Font CreateDynamicFontFromOSFont(string fontname, int size) => new Font(UnityEngine.Font.CreateDynamicFontFromOSFont(fontname, size));

            public static Font CreateDynamicFontFromOSFont(string[] fontnames, int size) => new Font(UnityEngine.Font.CreateDynamicFontFromOSFont(fontnames, size));

            public static string[] GetOSInstalledFontNames() => UnityEngine.Font.GetOSInstalledFontNames();
        }

        public class GUIContent
        {
            private UnityEngine.GUIContent _content;

            public string text
            {
                get => _content.text;
                set => _content.text = value;
            }
            public Texture image
            {
                get => _content.image;
                set => _content.image = value;
            }
            public string tooltip
            {
                get => _content.tooltip;
                set => _content.tooltip = value;
            }

            public GUIContent()
            {
                _content = new UnityEngine.GUIContent();
            }

            public GUIContent(string text)
            {
                _content = new UnityEngine.GUIContent(text);
            }

            public GUIContent(string text, Texture image)
            {
#if IL2CPP_BEPINEX || IL2CPP_MELONLOADER_PRE57 || IL2CPP_MELONLOADER
                _content = new UnityEngine.GUIContent(text, image, "");
#else
                _content = new UnityEngine.GUIContent(text, image);
#endif
            }

            public GUIContent(string text, string tooltip)
            {
                _content = new UnityEngine.GUIContent(text) { tooltip = tooltip };
            }

            public GUIContent(string text, Texture image, string tooltip)
            {
                _content = new UnityEngine.GUIContent(text, image, tooltip);
            }

            public static implicit operator UnityEngine.GUIContent(GUIContent content) => content?._content;

            public static implicit operator GUIContent(string text) => new GUIContent(text);

            public UnityEngine.GUIContent GetInternalGUIContent() => _content;

            private static GUIContent _none;
            public static GUIContent none => _none ?? (_none = new GUIContent(""));
            public static GUIContent empty => none;
        }

#if IL2CPP_MELONLOADER_PRE57
        private static Il2CppReferenceArray<GUILayoutOption> ToIL2CPP(GUILayoutOption[] options) => options != null ? new Il2CppReferenceArray<GUILayoutOption>(options) : null;
#endif

        public static bool Button(string text, GUIStyle style, params GUILayoutOption[] options)
        {
#if IL2CPP_MELONLOADER_PRE57
            return GUILayout.Button(text, style, ToIL2CPP(options));
#else
            return GUILayout.Button(text, style, options);
#endif
        }

        public static bool Button(GUIContent content, GUIStyle style, params GUILayoutOption[] options)
        {
#if IL2CPP_MELONLOADER_PRE57
            return GUILayout.Button(content, style, ToIL2CPP(options));
#else
            return GUILayout.Button(content, style, options);
#endif
        }

        public static bool Button(Rect position, string text, GUIStyle style) => GUI.Button(position, text, style);

        public static bool Button(Rect position, GUIContent content, GUIStyle style) => GUI.Button(position, content, style);

        public static bool Button(string text, params GUILayoutOption[] options)
        {
#if IL2CPP_MELONLOADER_PRE57
            return GUILayout.Button(text, ToIL2CPP(options));
#else
            return GUILayout.Button(text, options);
#endif
        }

        public static bool Button(string text, GUIStyle style)
        {
#if IL2CPP_MELONLOADER_PRE57
            return GUILayout.Button(text, (UnityEngine.GUIStyle)style, new Il2CppReferenceArray<GUILayoutOption>(shadcnui.GUIComponents.Layout.Layout.EmptyOptions));
#else
            return GUILayout.Button(text, style);
#endif
        }

        public static bool Button(GUIContent content, GUIStyle style)
        {
#if IL2CPP_MELONLOADER_PRE57
            return GUILayout.Button(content, (UnityEngine.GUIStyle)style, new Il2CppReferenceArray<GUILayoutOption>(shadcnui.GUIComponents.Layout.Layout.EmptyOptions));
#else
            return GUILayout.Button(content, style);
#endif
        }

        public static void Label(UnityEngine.GUIContent content, GUIStyle style)
        {
#if IL2CPP_MELONLOADER_PRE57
            GUILayout.Label(content, style, ToIL2CPP(null));
#else
            GUILayout.Label(content, style);
#endif
        }

        public static void Label(string text, params GUILayoutOption[] options)
        {
#if IL2CPP_MELONLOADER_PRE57
            GUILayout.Label(text, ToIL2CPP(options));
#else
            GUILayout.Label(text, options);
#endif
        }

        public static void Label(string text, GUIStyle style)
        {
#if IL2CPP_MELONLOADER_PRE57
            GUILayout.Label(text, style, ToIL2CPP(null));
#else
            GUILayout.Label(text, style);
#endif
        }

        public static void Label(string text, GUIStyle style, params GUILayoutOption[] options)
        {
#if IL2CPP_MELONLOADER_PRE57
            GUILayout.Label(text, style, ToIL2CPP(options));
#else
            GUILayout.Label(text, style, options);
#endif
        }

        public static void Label(Rect position, string text, GUIStyle style) => GUI.Label(position, text, style);

        public static void Label(Rect position, GUIContent content, GUIStyle style) => GUI.Label(position, content, style);

        public static void Label(Texture image, params GUILayoutOption[] options)
        {
#if IL2CPP_MELONLOADER_PRE57
            GUILayout.Label(image, ToIL2CPP(options));
#else
            GUILayout.Label(image, options);
#endif
        }

        public static void Label(Texture image, GUIStyle style, params GUILayoutOption[] options)
        {
#if IL2CPP_MELONLOADER_PRE57
            GUILayout.Label(image, style, ToIL2CPP(options));
#else
            GUILayout.Label(image, style, options);
#endif
        }

        public static bool Toggle(bool value, string text, params GUILayoutOption[] options)
        {
#if IL2CPP_MELONLOADER_PRE57
            return GUILayout.Toggle(value, text, ToIL2CPP(options));
#else
            return GUILayout.Toggle(value, text, options);
#endif
        }

        public static bool Toggle(bool value, string text, GUIStyle style)
        {
#if IL2CPP_MELONLOADER_PRE57
            return GUILayout.Toggle(value, text, style, ToIL2CPP(null));
#else
            return GUILayout.Toggle(value, text, style);
#endif
        }

        public static bool Toggle(bool value, string text, GUIStyle style, params GUILayoutOption[] options)
        {
#if IL2CPP_MELONLOADER_PRE57
            return GUILayout.Toggle(value, text, style, ToIL2CPP(options));
#else
            return GUILayout.Toggle(value, text, style, options);
#endif
        }

        public static bool Toggle(bool value, GUIContent content, GUIStyle style, params GUILayoutOption[] options)
        {
#if IL2CPP_MELONLOADER_PRE57
            return GUILayout.Toggle(value, content, style, ToIL2CPP(options));
#else
            return GUILayout.Toggle(value, content, style, options);
#endif
        }

        public static bool Toggle(Rect position, bool value, string text, GUIStyle style) => GUI.Toggle(position, value, text, style);

        public static bool Toggle(Rect position, bool value, GUIContent content, GUIStyle style) => GUI.Toggle(position, value, content, style);

        public static void Box(string text, GUIStyle style, params GUILayoutOption[] options)
        {
#if IL2CPP_MELONLOADER_PRE57
            GUILayout.Box(text, style, ToIL2CPP(options));
#else
            GUILayout.Box(text, style, options);
#endif
        }

        public static void Box(GUIContent content, GUIStyle style, params GUILayoutOption[] options)
        {
#if IL2CPP_MELONLOADER_PRE57
            GUILayout.Box(content, style, ToIL2CPP(options));
#else
            GUILayout.Box(content, style, options);
#endif
        }

        public static string TextField(string text, GUIStyle style, params GUILayoutOption[] options)
        {
#if IL2CPP_MELONLOADER_PRE57
            return GUILayout.TextField(text, style, ToIL2CPP(options));
#else
            return GUILayout.TextField(text, style, options);
#endif
        }

        public static string TextField(string text, int maxLength, GUIStyle style, params GUILayoutOption[] options)
        {
#if IL2CPP_MELONLOADER_PRE57
            return GUILayout.TextField(text, maxLength, style, ToIL2CPP(options));
#else
            return GUILayout.TextField(text, maxLength, style, options);
#endif
        }

        public static string PasswordField(string password, char maskChar, GUIStyle style, params GUILayoutOption[] options)
        {
#if IL2CPP_MELONLOADER_PRE57
            return GUILayout.PasswordField(password, maskChar, style, ToIL2CPP(options));
#else
            return GUILayout.PasswordField(password, maskChar, style, options);
#endif
        }

        public static string PasswordField(string password, char maskChar, int maxLength, GUIStyle style, params GUILayoutOption[] options)
        {
#if IL2CPP_MELONLOADER_PRE57
            return GUILayout.PasswordField(password, maskChar, maxLength, style, ToIL2CPP(options));
#else
            return GUILayout.PasswordField(password, maskChar, maxLength, style, options);
#endif
        }

        public static string TextArea(string text, params GUILayoutOption[] options)
        {
#if IL2CPP_MELONLOADER_PRE57
            return GUILayout.TextArea(text, ToIL2CPP(options));
#else
            return GUILayout.TextArea(text, options);
#endif
        }

        public static string TextArea(string text, GUIStyle style, params GUILayoutOption[] options)
        {
#if IL2CPP_MELONLOADER_PRE57
            return GUILayout.TextArea(text, style, ToIL2CPP(options));
#else
            return GUILayout.TextArea(text, style, options);
#endif
        }

        public static string TextArea(string text, int maxLength, GUIStyle style, params GUILayoutOption[] options)
        {
#if IL2CPP_MELONLOADER_PRE57
            return GUILayout.TextArea(text, maxLength, style, ToIL2CPP(options));
#else
            return GUILayout.TextArea(text, maxLength, style, options);
#endif
        }

        public static float HorizontalSlider(float value, float leftValue, float rightValue, GUIStyle slider, GUIStyle thumb, params GUILayoutOption[] options)
        {
#if IL2CPP_MELONLOADER_PRE57
            return GUILayout.HorizontalSlider(value, leftValue, rightValue, slider, thumb, ToIL2CPP(options));
#else
            return GUILayout.HorizontalSlider(value, leftValue, rightValue, slider, thumb, options);
#endif
        }

        public static float HorizontalSlider(float value, float leftValue, float rightValue, params GUILayoutOption[] options)
        {
#if IL2CPP_MELONLOADER_PRE57
            return GUILayout.HorizontalSlider(value, leftValue, rightValue, ToIL2CPP(options));
#else
            return GUILayout.HorizontalSlider(value, leftValue, rightValue, options);
#endif
        }

        public static float VerticalSlider(float value, float topValue, float bottomValue, GUIStyle slider, GUIStyle thumb, params GUILayoutOption[] options)
        {
#if IL2CPP_MELONLOADER_PRE57
            return GUILayout.VerticalSlider(value, topValue, bottomValue, slider, thumb, ToIL2CPP(options));
#else
            return GUILayout.VerticalSlider(value, topValue, bottomValue, slider, thumb, options);
#endif
        }

        public static float VerticalSlider(float value, float topValue, float bottomValue, params GUILayoutOption[] options)
        {
#if IL2CPP_MELONLOADER_PRE57
            return GUILayout.VerticalSlider(value, topValue, bottomValue, ToIL2CPP(options));
#else
            return GUILayout.VerticalSlider(value, topValue, bottomValue, options);
#endif
        }

        public static void BeginHorizontal(GUIStyle style, params GUILayoutOption[] options)
        {
#if IL2CPP_MELONLOADER_PRE57
            GUILayout.BeginHorizontal(style, ToIL2CPP(options));
#else
            GUILayout.BeginHorizontal(style, options);
#endif
        }

        public static void BeginHorizontal(params GUILayoutOption[] options)
        {
#if IL2CPP_MELONLOADER_PRE57
            GUILayout.BeginHorizontal(ToIL2CPP(options));
#else
            GUILayout.BeginHorizontal(options);
#endif
        }

        public static void EndHorizontal() => GUILayout.EndHorizontal();

        public static void BeginVertical(GUIStyle style, params GUILayoutOption[] options)
        {
#if IL2CPP_MELONLOADER_PRE57
            GUILayout.BeginVertical(style, ToIL2CPP(options));
#else
            GUILayout.BeginVertical(style, options);
#endif
        }

        public static void BeginVertical(params GUILayoutOption[] options)
        {
#if IL2CPP_MELONLOADER_PRE57
            GUILayout.BeginVertical(ToIL2CPP(options));
#else
            GUILayout.BeginVertical(options);
#endif
        }

        public static void EndVertical() => GUILayout.EndVertical();

        public static Vector2 BeginScrollView(Vector2 scrollPosition, GUIStyle style, params GUILayoutOption[] options)
        {
#if IL2CPP_MELONLOADER_PRE57
            return GUILayout.BeginScrollView(scrollPosition, style, ToIL2CPP(options));
#else
            return GUILayout.BeginScrollView(scrollPosition, style, options);
#endif
        }

        public static Vector2 BeginScrollView(Vector2 scrollPosition, params GUILayoutOption[] options)
        {
#if IL2CPP_MELONLOADER_PRE57
            return GUILayout.BeginScrollView(scrollPosition, ToIL2CPP(options));
#else
            return GUILayout.BeginScrollView(scrollPosition, options);
#endif
        }

        public static Vector2 BeginScrollView(Vector2 scrollPosition, bool alwaysShowHorizontal, bool alwaysShowVertical, params GUILayoutOption[] options)
        {
#if IL2CPP_MELONLOADER_PRE57
            return GUILayout.BeginScrollView(scrollPosition, alwaysShowHorizontal, alwaysShowVertical, ToIL2CPP(options));
#else
            return GUILayout.BeginScrollView(scrollPosition, alwaysShowHorizontal, alwaysShowVertical, options);
#endif
        }

        public static void EndScrollView() => GUILayout.EndScrollView();

        public static void Space(float pixels) => GUILayout.Space(pixels);

        public static void FlexibleSpace() => GUILayout.FlexibleSpace();

        public static Rect BeginArea(Rect screenRect, GUIStyle style)
        {
            GUILayout.BeginArea(screenRect, style);
            return screenRect;
        }

        public static Rect BeginArea(Rect screenRect)
        {
            GUILayout.BeginArea(screenRect);
            return screenRect;
        }

        public static void EndArea() => GUILayout.EndArea();

        public static int SelectionGrid(int selected, string[] texts, int xCount, GUIStyle style, params GUILayoutOption[] options)
        {
#if IL2CPP_MELONLOADER_PRE57
            return GUILayout.SelectionGrid(selected, texts, xCount, style, ToIL2CPP(options));
#else
            return GUILayout.SelectionGrid(selected, texts, xCount, style, options);
#endif
        }

        public static int SelectionGrid(int selected, GUIContent[] contents, int xCount, GUIStyle style, params GUILayoutOption[] options)
        {
#if IL2CPP_MELONLOADER_PRE57
            var unityContents = new UnityEngine.GUIContent[contents.Length];
            for (int i = 0; i < contents.Length; i++)
                unityContents[i] = contents[i];
            return GUILayout.SelectionGrid(selected, unityContents, xCount, style, ToIL2CPP(options));
#else
            var unityContents = new UnityEngine.GUIContent[contents.Length];
            for (int i = 0; i < contents.Length; i++)
                unityContents[i] = contents[i];
            return GUILayout.SelectionGrid(selected, unityContents, xCount, style, options);
#endif
        }

        public static int Toolbar(int selected, string[] texts, GUIStyle style, params GUILayoutOption[] options)
        {
#if IL2CPP_MELONLOADER_PRE57
            return GUILayout.Toolbar(selected, texts, style, ToIL2CPP(options));
#else
            return GUILayout.Toolbar(selected, texts, style, options);
#endif
        }

        public static bool RepeatButton(string text, GUIStyle style, params GUILayoutOption[] options)
        {
#if IL2CPP_MELONLOADER_PRE57
            return GUILayout.RepeatButton(text, style, ToIL2CPP(options));
#else
            return GUILayout.RepeatButton(text, style, options);
#endif
        }
    }
}
