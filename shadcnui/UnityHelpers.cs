#if IL2CPP_MELONLOADER
using UnhollowerBaseLib;
#endif
using UnityEngine;

namespace shadcnui
{
    public static class UnityHelpers
    {
        public class GUIStyle
        {
            private UnityEngine.GUIStyle _guiStyle;

            public float fixedWidth
            {
                get { return _guiStyle.fixedWidth; }
                set { _guiStyle.fixedWidth = value; }
            }

            public float fixedHeight
            {
                get { return _guiStyle.fixedHeight; }
                set { _guiStyle.fixedHeight = value; }
            }

            public UnityEngine.RectOffset margin
            {
                get { return _guiStyle.margin; }
                set { _guiStyle.margin = value; }
            }

            public UnityEngine.RectOffset padding
            {
                get { return _guiStyle.padding; }
                set { _guiStyle.padding = value; }
            }

            public UnityEngine.RectOffset border
            {
                get { return _guiStyle.border; }
                set { _guiStyle.border = value; }
            }

            public UnityEngine.RectOffset overflow
            {
                get { return _guiStyle.overflow; }
                set { _guiStyle.overflow = value; }
            }

            public TextAnchor alignment
            {
                get { return _guiStyle.alignment; }
                set { _guiStyle.alignment = value; }
            }

            public FontStyle fontStyle
            {
                get { return _guiStyle.fontStyle; }
                set { _guiStyle.fontStyle = value; }
            }

            public int fontSize
            {
                get { return _guiStyle.fontSize; }
                set { _guiStyle.fontSize = value; }
            }

            public UnityEngine.Font font
            {
                get { return _guiStyle.font; }
                set { _guiStyle.font = value; }
            }

            public GUIStyleState normal
            {
                get { return _guiStyle.normal; }
                set { _guiStyle.normal = value; }
            }

            public GUIStyleState hover
            {
                get { return _guiStyle.hover; }
                set { _guiStyle.hover = value; }
            }

            public GUIStyleState active
            {
                get { return _guiStyle.active; }
                set { _guiStyle.active = value; }
            }

            public GUIStyleState focused
            {
                get { return _guiStyle.focused; }
                set { _guiStyle.focused = value; }
            }

            public GUIStyleState onNormal
            {
                get { return _guiStyle.onNormal; }
                set { _guiStyle.onNormal = value; }
            }

            public GUIStyleState onHover
            {
                get { return _guiStyle.onHover; }
                set { _guiStyle.onHover = value; }
            }

            public GUIStyleState onActive
            {
                get { return _guiStyle.onActive; }
                set { _guiStyle.onActive = value; }
            }

            public GUIStyleState onFocused
            {
                get { return _guiStyle.onFocused; }
                set { _guiStyle.onFocused = value; }
            }

            public TextClipping clipping
            {
                get { return _guiStyle.clipping; }
                set { _guiStyle.clipping = value; }
            }

            public ImagePosition imagePosition
            {
                get { return _guiStyle.imagePosition; }
                set { _guiStyle.imagePosition = value; }
            }

            public Vector2 contentOffset
            {
                get { return _guiStyle.contentOffset; }
                set { _guiStyle.contentOffset = value; }
            }

            public bool wordWrap
            {
                get { return _guiStyle.wordWrap; }
                set { _guiStyle.wordWrap = value; }
            }

            public bool stretchWidth
            {
                get { return _guiStyle.stretchWidth; }
                set { _guiStyle.stretchWidth = value; }
            }

            public bool stretchHeight
            {
                get { return _guiStyle.stretchHeight; }
                set { _guiStyle.stretchHeight = value; }
            }

            public string name
            {
                get { return _guiStyle.name; }
                set { _guiStyle.name = value; }
            }

            public GUIStyle(UnityEngine.GUIStyle style)
            {
#if IL2CPP_BEPINEX
                _guiStyle = new UnityEngine.GUIStyle();
                _guiStyle.m_Ptr = style.m_Ptr;
#else
                _guiStyle = new UnityEngine.GUIStyle(style);
#endif
            }

            public GUIStyle()
            {
                _guiStyle = new UnityEngine.GUIStyle();
            }

            public static implicit operator UnityEngine.GUIStyle(GUIStyle style)
            {
                return style._guiStyle;
            }

            public static implicit operator GUIStyle(UnityEngine.GUIStyle style)
            {
                return new GUIStyle(style);
            }

            public UnityEngine.GUIStyle GetInternalStyle()
            {
                return _guiStyle;
            }
        }

        public class RectOffset
        {
            private UnityEngine.RectOffset _rectOffset;

            public int left
            {
                get { return _rectOffset.left; }
                set { _rectOffset.left = value; }
            }

            public int right
            {
                get { return _rectOffset.right; }
                set { _rectOffset.right = value; }
            }

            public int top
            {
                get { return _rectOffset.top; }
                set { _rectOffset.top = value; }
            }

            public int bottom
            {
                get { return _rectOffset.bottom; }
                set { _rectOffset.bottom = value; }
            }

            public int horizontal
            {
                get { return _rectOffset.horizontal; }
            }

            public int vertical
            {
                get { return _rectOffset.vertical; }
            }

            public RectOffset(int left, int right, int top, int bottom)
            {
                _rectOffset = new UnityEngine.RectOffset();
                _rectOffset.left = left;
                _rectOffset.right = right;
                _rectOffset.top = top;
                _rectOffset.bottom = bottom;
            }

            public RectOffset()
            {
                _rectOffset = new UnityEngine.RectOffset();
            }

            public static implicit operator UnityEngine.RectOffset(RectOffset rectOffset)
            {
                return rectOffset._rectOffset;
            }

            public static implicit operator RectOffset(UnityEngine.RectOffset rectOffset)
            {
                return new RectOffset(rectOffset.left, rectOffset.right, rectOffset.top, rectOffset.bottom);
            }

            public UnityEngine.RectOffset GetInternalRectOffset()
            {
                return _rectOffset;
            }
        }

        public class Font
        {
            private UnityEngine.Font _font;

            public string name
            {
                get { return _font.name; }
            }


            public Font(string name)
            {
#if IL2CPP_BEPINEX || IL2CPP_MELONLOADER
                _font = UnityEngine.Font.CreateDynamicFontFromOSFont(name, 14);
#else
                _font = new UnityEngine.Font(name);
#endif
            }

            public Font(UnityEngine.Font font)
            {
                _font = font;
            }

            public static implicit operator UnityEngine.Font(Font font)
            {
                return font._font;
            }

            public static implicit operator Font(UnityEngine.Font font)
            {
                return new Font(font);
            }

            public UnityEngine.Font GetInternalFont()
            {
                return _font;
            }

            public static Font CreateDynamicFontFromOSFont(string fontname, int size)
            {
                return new Font(UnityEngine.Font.CreateDynamicFontFromOSFont(fontname, size));
            }

            public static Font CreateDynamicFontFromOSFont(string[] fontnames, int size)
            {
                return new Font(UnityEngine.Font.CreateDynamicFontFromOSFont(fontnames, size));
            }
        }

        public class GUIContent
        {
            private UnityEngine.GUIContent _guiContent;

            public string text
            {
                get { return _guiContent.text; }
                set { _guiContent.text = value; }
            }

            public Texture image
            {
                get { return _guiContent.image; }
                set { _guiContent.image = value; }
            }

            public string tooltip
            {
                get { return _guiContent.tooltip; }
                set { _guiContent.tooltip = value; }
            }

            public GUIContent()
            {
                _guiContent = new UnityEngine.GUIContent();
            }

            public GUIContent(string text)
            {
                _guiContent = new UnityEngine.GUIContent(text);
            }

            public GUIContent(string text, Texture image)
            {
#if IL2CPP_BEPINEX || IL2CPP_MELONLOADER
                _guiContent = new UnityEngine.GUIContent(text, image, "");
#else
        _guiContent = new UnityEngine.GUIContent(text, image);
#endif
            }

            public GUIContent(string text, Texture image, string tooltip)
            {
                _guiContent = new UnityEngine.GUIContent(text, image, tooltip);
            }

            public static implicit operator UnityEngine.GUIContent(GUIContent content)
            {
                return content._guiContent;
            }

            public UnityEngine.GUIContent GetInternalGUIContent()
            {
                return _guiContent;
            }

            private static GUIContent _none;
            public static GUIContent none
            {
                get
                {
                    if (_none == null)
                        _none = new GUIContent("");
                    return _none;
                }
            }
        }

    }
}