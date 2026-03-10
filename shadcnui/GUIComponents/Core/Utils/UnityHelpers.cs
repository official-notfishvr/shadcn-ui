using System;
using UnityEngine;
#if IL2CPP_MELONLOADER_PRE57
using UnhollowerBaseLib;
#endif

namespace shadcnui.GUIComponents.Core.Utils
{
    public static class UnityHelpers
    {
        public class GUIStyle
        {
            private readonly UnityEngine.GUIStyle _style;

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

            public GUIStyle()
            {
                _style = new UnityEngine.GUIStyle();
            }

            public GUIStyle(UnityEngine.GUIStyle style)
            {
                _style = style != null ? new UnityEngine.GUIStyle(style) : new UnityEngine.GUIStyle();
            }

            public GUIStyle(GUIStyle style)
            {
                _style = style != null ? new UnityEngine.GUIStyle(style._style) : new UnityEngine.GUIStyle();
            }

            public static implicit operator UnityEngine.GUIStyle(GUIStyle style) => style?._style;

            public static implicit operator GUIStyle(UnityEngine.GUIStyle style) => style != null ? new GUIStyle(style) : null;

            public UnityEngine.GUIStyle GetInternalStyle() => _style;

            public Vector2 CalcSize(GUIContent content) => _style.CalcSize(content);

            public Vector2 CalcSize(string text) => _style.CalcSize(new UnityEngine.GUIContent(text));

            public float CalcHeight(GUIContent content, float width) => _style.CalcHeight(content, width);
        }

        public class RectOffset
        {
            private readonly UnityEngine.RectOffset _offset;

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

            public RectOffset()
            {
                _offset = new UnityEngine.RectOffset();
            }

            public RectOffset(int left, int right, int top, int bottom)
            {
                _offset = new UnityEngine.RectOffset(left, right, top, bottom);
            }

            public static implicit operator UnityEngine.RectOffset(RectOffset offset) => offset?._offset;

            public static implicit operator RectOffset(UnityEngine.RectOffset offset) => offset != null ? new RectOffset(offset.left, offset.right, offset.top, offset.bottom) : null;
        }

        public class Font
        {
            private readonly UnityEngine.Font _font;

            public string name => _font?.name ?? string.Empty;
            public int fontSize => _font?.fontSize ?? 0;
            public bool dynamic => _font != null && _font.dynamic;

            public Font(string name)
            {
                _font = UnityEngine.Font.CreateDynamicFontFromOSFont(name, 14);
            }

            public Font(UnityEngine.Font font)
            {
                _font = font;
            }

            public static implicit operator UnityEngine.Font(Font font) => font?._font;

            public static implicit operator Font(UnityEngine.Font font) => font != null ? new Font(font) : null;

            public static Font CreateDynamicFontFromOSFont(string fontName, int size) => new(UnityEngine.Font.CreateDynamicFontFromOSFont(fontName, size));

            public static Font CreateDynamicFontFromOSFont(string[] fontNames, int size) => new(UnityEngine.Font.CreateDynamicFontFromOSFont(fontNames, size));

            public static string[] GetOSInstalledFontNames() => UnityEngine.Font.GetOSInstalledFontNames();
        }

        public class GUIContent
        {
            private readonly UnityEngine.GUIContent _content;

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
                _content = new UnityEngine.GUIContent(text, image);
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

            public static implicit operator GUIContent(string text) => new(text);

            private static GUIContent _none;
            public static GUIContent none => _none ??= new GUIContent(string.Empty);
            public static GUIContent empty => none;
        }

#if IL2CPP_MELONLOADER_PRE57
        private static Il2CppReferenceArray<GUILayoutOption> ToArray(GUILayoutOption[] options) => options != null ? new Il2CppReferenceArray<GUILayoutOption>(options) : null;
#endif

        public static bool Button(string text, GUIStyle style, params GUILayoutOption[] options)
        {
#if IL2CPP_MELONLOADER_PRE57
            return GUILayout.Button(text, style, ToArray(options));
#else
            return GUILayout.Button(text, style, options);
#endif
        }

        public static bool Button(GUIContent content, GUIStyle style, params GUILayoutOption[] options)
        {
#if IL2CPP_MELONLOADER_PRE57
            return GUILayout.Button(content, style, ToArray(options));
#else
            return GUILayout.Button(content, style, options);
#endif
        }

        public static bool Button(Rect position, string text, GUIStyle style) => GUI.Button(position, text, style);

        public static bool Button(Rect position, GUIContent content, GUIStyle style) => GUI.Button(position, content, style);

        public static void Label(UnityEngine.GUIContent content, GUIStyle style)
        {
#if IL2CPP_MELONLOADER_PRE57
            GUILayout.Label(content, style, ToArray(null));
#else
            GUILayout.Label(content, style);
#endif
        }

        public static void Label(string text, params GUILayoutOption[] options)
        {
#if IL2CPP_MELONLOADER_PRE57
            GUILayout.Label(text, ToArray(options));
#else
            GUILayout.Label(text, options);
#endif
        }

        public static void Label(string text, GUIStyle style, params GUILayoutOption[] options)
        {
#if IL2CPP_MELONLOADER_PRE57
            GUILayout.Label(text, style, ToArray(options));
#else
            GUILayout.Label(text, style, options);
#endif
        }

        public static void Label(Rect position, string text, GUIStyle style) => GUI.Label(position, text, style);

        public static void Label(Rect position, GUIContent content, GUIStyle style) => GUI.Label(position, content, style);

        public static void Label(Texture image, params GUILayoutOption[] options)
        {
#if IL2CPP_MELONLOADER_PRE57
            GUILayout.Label(image, ToArray(options));
#else
            GUILayout.Label(image, options);
#endif
        }

        public static void Label(Texture image, GUIStyle style, params GUILayoutOption[] options)
        {
#if IL2CPP_MELONLOADER_PRE57
            GUILayout.Label(image, style, ToArray(options));
#else
            GUILayout.Label(image, style, options);
#endif
        }

        public static bool Toggle(bool value, string text, GUIStyle style, params GUILayoutOption[] options)
        {
#if IL2CPP_MELONLOADER_PRE57
            return GUILayout.Toggle(value, text, style, ToArray(options));
#else
            return GUILayout.Toggle(value, text, style, options);
#endif
        }

        public static bool Toggle(bool value, GUIContent content, GUIStyle style, params GUILayoutOption[] options)
        {
#if IL2CPP_MELONLOADER_PRE57
            return GUILayout.Toggle(value, content, style, ToArray(options));
#else
            return GUILayout.Toggle(value, content, style, options);
#endif
        }

        public static bool Toggle(Rect position, bool value, string text, GUIStyle style) => GUI.Toggle(position, value, text, style);

        public static bool Toggle(Rect position, bool value, GUIContent content, GUIStyle style) => GUI.Toggle(position, value, content, style);

        public static void Box(string text, GUIStyle style, params GUILayoutOption[] options)
        {
#if IL2CPP_MELONLOADER_PRE57
            GUILayout.Box(text, style, ToArray(options));
#else
            GUILayout.Box(text, style, options);
#endif
        }

        public static void Box(GUIContent content, GUIStyle style, params GUILayoutOption[] options)
        {
#if IL2CPP_MELONLOADER_PRE57
            GUILayout.Box(content, style, ToArray(options));
#else
            GUILayout.Box(content, style, options);
#endif
        }

        public static string TextField(string text, GUIStyle style, params GUILayoutOption[] options)
        {
#if IL2CPP_MELONLOADER_PRE57
            return GUILayout.TextField(text, style, ToArray(options));
#else
            return GUILayout.TextField(text, style, options);
#endif
        }

        public static string TextField(string text, int maxLength, GUIStyle style, params GUILayoutOption[] options)
        {
#if IL2CPP_MELONLOADER_PRE57
            return GUILayout.TextField(text, maxLength, style, ToArray(options));
#else
            return GUILayout.TextField(text, maxLength, style, options);
#endif
        }

        public static string PasswordField(string password, char maskChar, GUIStyle style, params GUILayoutOption[] options)
        {
#if IL2CPP_MELONLOADER_PRE57
            return GUILayout.PasswordField(password, maskChar, style, ToArray(options));
#else
            return GUILayout.PasswordField(password, maskChar, style, options);
#endif
        }

        public static string PasswordField(string password, char maskChar, int maxLength, GUIStyle style, params GUILayoutOption[] options)
        {
#if IL2CPP_MELONLOADER_PRE57
            return GUILayout.PasswordField(password, maskChar, maxLength, style, ToArray(options));
#else
            return GUILayout.PasswordField(password, maskChar, maxLength, style, options);
#endif
        }

        public static string TextArea(string text, GUIStyle style, params GUILayoutOption[] options)
        {
#if IL2CPP_MELONLOADER_PRE57
            return GUILayout.TextArea(text, style, ToArray(options));
#else
            return GUILayout.TextArea(text, style, options);
#endif
        }

        public static string TextArea(string text, int maxLength, GUIStyle style, params GUILayoutOption[] options)
        {
#if IL2CPP_MELONLOADER_PRE57
            return GUILayout.TextArea(text, maxLength, style, ToArray(options));
#else
            return GUILayout.TextArea(text, maxLength, style, options);
#endif
        }

        public static float HorizontalSlider(float value, float leftValue, float rightValue, GUIStyle slider, GUIStyle thumb, params GUILayoutOption[] options)
        {
#if IL2CPP_MELONLOADER_PRE57
            return GUILayout.HorizontalSlider(value, leftValue, rightValue, slider, thumb, ToArray(options));
#else
            return GUILayout.HorizontalSlider(value, leftValue, rightValue, slider, thumb, options);
#endif
        }

        public static void BeginHorizontal(GUIStyle style, params GUILayoutOption[] options)
        {
#if IL2CPP_MELONLOADER_PRE57
            GUILayout.BeginHorizontal(style, ToArray(options));
#else
            GUILayout.BeginHorizontal(style, options);
#endif
        }

        public static void BeginHorizontal(params GUILayoutOption[] options)
        {
#if IL2CPP_MELONLOADER_PRE57
            GUILayout.BeginHorizontal(ToArray(options));
#else
            GUILayout.BeginHorizontal(options);
#endif
        }

        public static void EndHorizontal() => GUILayout.EndHorizontal();

        public static void BeginVertical(GUIStyle style, params GUILayoutOption[] options)
        {
#if IL2CPP_MELONLOADER_PRE57
            GUILayout.BeginVertical(style, ToArray(options));
#else
            GUILayout.BeginVertical(style, options);
#endif
        }

        public static void BeginVertical(params GUILayoutOption[] options)
        {
#if IL2CPP_MELONLOADER_PRE57
            GUILayout.BeginVertical(ToArray(options));
#else
            GUILayout.BeginVertical(options);
#endif
        }

        public static void EndVertical() => GUILayout.EndVertical();

        public static Vector2 BeginScrollView(Vector2 scrollPosition, GUIStyle style, params GUILayoutOption[] options)
        {
#if IL2CPP_MELONLOADER_PRE57
            return GUILayout.BeginScrollView(scrollPosition, style, ToArray(options));
#else
            return GUILayout.BeginScrollView(scrollPosition, style, options);
#endif
        }

        public static Vector2 BeginScrollView(Vector2 scrollPosition, GUIStyle horizontalScrollbar, GUIStyle verticalScrollbar, params GUILayoutOption[] options)
        {
#if IL2CPP_MELONLOADER_PRE57
            return GUILayout.BeginScrollView(scrollPosition, horizontalScrollbar, verticalScrollbar, ToArray(options));
#else
            return GUILayout.BeginScrollView(scrollPosition, horizontalScrollbar, verticalScrollbar, options);
#endif
        }

        public static Vector2 BeginScrollView(Vector2 scrollPosition, bool alwaysShowHorizontal, bool alwaysShowVertical, params GUILayoutOption[] options)
        {
#if IL2CPP_MELONLOADER_PRE57
            return GUILayout.BeginScrollView(scrollPosition, alwaysShowHorizontal, alwaysShowVertical, ToArray(options));
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
            return GUILayout.SelectionGrid(selected, texts, xCount, style, ToArray(options));
#else
            return GUILayout.SelectionGrid(selected, texts, xCount, style, options);
#endif
        }

        public static int Toolbar(int selected, string[] texts, GUIStyle style, params GUILayoutOption[] options)
        {
#if IL2CPP_MELONLOADER_PRE57
            return GUILayout.Toolbar(selected, texts, style, ToArray(options));
#else
            return GUILayout.Toolbar(selected, texts, style, options);
#endif
        }

        public static bool RepeatButton(string text, GUIStyle style, params GUILayoutOption[] options)
        {
#if IL2CPP_MELONLOADER_PRE57
            return GUILayout.RepeatButton(text, style, ToArray(options));
#else
            return GUILayout.RepeatButton(text, style, options);
#endif
        }
    }
}
