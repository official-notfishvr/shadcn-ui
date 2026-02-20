using System;
using System.Collections.Generic;
using shadcnui.GUIComponents.Core.Theming;
using shadcnui.GUIComponents.Core.Utils;
using UnityEngine;

namespace shadcnui.GUIComponents.Core.Base
{
    public class LayerManager
    {
        private static LayerManager _instance;
        public static LayerManager Instance => _instance ??= new LayerManager();

        private readonly Dictionary<string, LayerState> _layers = new Dictionary<string, LayerState>();
        private readonly List<string> _drawOrder = new List<string>();
        private readonly Dictionary<string, int> _windowIds = new Dictionary<string, int>();
        private int _nextWindowId = 50000;
        private bool _isDrawing;
        private readonly List<string> _pendingCloses = new List<string>();

        private sealed class LayerState
        {
            public Vector2 Position;
            public float Width;
            public float Height;
            public int ZIndex;
            public bool CloseOnClickOutside;
            public bool ShowOverlay;
            public bool DrawChrome;
            public Action Content;
            public Action OnClose;
        }

        public void Open(LayerConfig config)
        {
            if (config == null || string.IsNullOrEmpty(config.Id))
                return;

            var state = new LayerState
            {
                Position = config.OpenPosition,
                Width = Mathf.Max(1f, config.Width),
                Height = Mathf.Max(1f, config.Height),
                ZIndex = config.ZIndex,
                CloseOnClickOutside = config.CloseOnClickOutside,
                ShowOverlay = config.ShowOverlay,
                DrawChrome = config.DrawChrome,
                Content = config.Content,
                OnClose = config.OnClose,
            };

            _layers[config.Id] = state;
            if (!_drawOrder.Contains(config.Id))
                _drawOrder.Add(config.Id);
            if (!_windowIds.ContainsKey(config.Id))
                _windowIds[config.Id] = _nextWindowId++;

            SortDrawOrder();
        }

        public void Open(string id, Vector2 position, Action content, float width = 200f, float height = 150f)
        {
            Open(
                new LayerConfig
                {
                    Id = id,
                    OpenPosition = position,
                    Content = content,
                    Width = width,
                    Height = height,
                }
            );
        }

        public void Close(string id)
        {
            if (_isDrawing)
            {
                if (!_pendingCloses.Contains(id))
                    _pendingCloses.Add(id);
                return;
            }
            DoClose(id);
        }

        private void DoClose(string id)
        {
            if (!_layers.TryGetValue(id, out var state))
                return;
            try
            {
                state.OnClose?.Invoke();
            }
            finally
            {
                _layers.Remove(id);
                _drawOrder.Remove(id);
            }
        }

        public void CloseAll()
        {
            if (_isDrawing)
            {
                _pendingCloses.Clear();
                _pendingCloses.AddRange(_drawOrder);
                return;
            }
            foreach (var id in new List<string>(_drawOrder))
                DoClose(id);
        }

        public bool IsOpen(string id)
        {
            return _layers.ContainsKey(id);
        }

        public Vector2 GetOpenPosition(string id)
        {
            return _layers.TryGetValue(id, out var state) ? state.Position : Vector2.zero;
        }

        public void SetPosition(string id, Vector2 position)
        {
            if (_layers.TryGetValue(id, out var state))
                state.Position = position;
        }

        public void BringToFront(string id)
        {
            if (!_layers.TryGetValue(id, out var state))
                return;
            int maxZ = 0;
            foreach (var layer in _layers.Values)
            {
                if (layer.ZIndex > maxZ)
                    maxZ = layer.ZIndex;
            }
            state.ZIndex = maxZ + 1;
            SortDrawOrder();
        }

        public void DrawLayers()
        {
            if (_drawOrder.Count == 0)
                return;

            _isDrawing = true;
            _pendingCloses.Clear();

            bool anyOverlay = false;
            for (int i = _drawOrder.Count - 1; i >= 0 && !anyOverlay; i--)
            {
                if (_layers.TryGetValue(_drawOrder[i], out var s) && s.ShowOverlay)
                    anyOverlay = true;
            }

            if (anyOverlay)
                DrawOverlayOnce();

            var orderSnapshot = new List<string>(_drawOrder);
            int topmostWindowId = -1;

            for (int i = 0; i < orderSnapshot.Count; i++)
            {
                var id = orderSnapshot[i];
                if (!_layers.TryGetValue(id, out var state))
                    continue;

                Rect rect = ClampToScreen(new Rect(state.Position.x, state.Position.y, state.Width, state.Height));
                int windowId = _windowIds.TryGetValue(id, out var wid) ? wid : _nextWindowId++;

                GUI.Window(windowId, rect, (GUI.WindowFunction)(_ => DrawLayerWindow(state)), GUIContent.none, GUIStyle.none);

                topmostWindowId = windowId;
            }

            if (topmostWindowId >= 0)
                GUI.BringWindowToFront(topmostWindowId);

            if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
            {
                Vector2 mouse = Event.current.mousePosition;
                string topmostCloseable = null;
                for (int i = orderSnapshot.Count - 1; i >= 0; i--)
                {
                    var id = orderSnapshot[i];
                    if (!_layers.TryGetValue(id, out var state))
                        continue;
                    Rect r = ClampToScreen(new Rect(state.Position.x, state.Position.y, state.Width, state.Height));
                    if (r.Contains(mouse))
                        break;
                    if (state.CloseOnClickOutside)
                    {
                        topmostCloseable = id;
                        break;
                    }
                }
                if (topmostCloseable != null)
                {
                    _pendingCloses.Add(topmostCloseable);
                    Event.current.Use();
                }
            }

            _isDrawing = false;
            foreach (var id in _pendingCloses)
                DoClose(id);
        }

        private void DrawLayerWindow(LayerState state)
        {
            if (!state.DrawChrome)
            {
                state.Content?.Invoke();
                return;
            }
            var theme = ThemeManager.Instance?.CurrentTheme;
            if (theme == null)
            {
                state.Content?.Invoke();
                return;
            }

            Color prev = GUI.color;
            Rect full = new Rect(0, 0, state.Width, state.Height);

            GUI.color = theme.Elevated;
            GUI.DrawTexture(full, Texture2D.whiteTexture);

            float border = 1f;
            GUI.color = theme.Border;
            GUI.DrawTexture(new Rect(0, 0, state.Width, border), Texture2D.whiteTexture);
            GUI.DrawTexture(new Rect(0, state.Height - border, state.Width, border), Texture2D.whiteTexture);
            GUI.DrawTexture(new Rect(0, 0, border, state.Height), Texture2D.whiteTexture);
            GUI.DrawTexture(new Rect(state.Width - border, 0, border, state.Height), Texture2D.whiteTexture);

            GUI.color = prev;

            state.Content?.Invoke();
        }

        private void DrawOverlayOnce()
        {
            var theme = ThemeManager.Instance?.CurrentTheme;
            if (theme == null)
                return;
            Color prev = GUI.color;
            GUI.color = theme.Overlay;
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), Texture2D.whiteTexture);
            GUI.color = prev;
        }

        private Rect ClampToScreen(Rect rect, float margin = 8f)
        {
            float maxW = Screen.width - margin;
            float maxH = Screen.height - margin;
            float x = Mathf.Clamp(rect.x, margin, maxW - rect.width);
            float y = Mathf.Clamp(rect.y, margin, maxH - rect.height);
            return new Rect(x, y, rect.width, rect.height);
        }

        private void SortDrawOrder()
        {
            _drawOrder.Sort(
                (a, b) =>
                {
                    int zA = _layers.TryGetValue(a, out var sa) ? sa.ZIndex : 0;
                    int zB = _layers.TryGetValue(b, out var sb) ? sb.ZIndex : 0;
                    return zA.CompareTo(zB);
                }
            );
        }

        public int GetLayerCount() => _drawOrder.Count;

        public bool HasOpenLayers() => _drawOrder.Count > 0;
    }
}
