using System;
using System.Collections.Generic;
using shadcnui.GUIComponents.Core.Theming;
using shadcnui.GUIComponents.Core.Utils;
using UnityEngine;

namespace shadcnui.GUIComponents.Core.Base
{
    public sealed class LayerManager
    {
        private sealed class LayerState
        {
            public string Id;
            public Vector2 Position;
            public float Width;
            public float Height;
            public int ZIndex;
            public bool CloseOnClickOutside;
            public bool ShowOverlay;
            public bool DrawChrome;
            public Action Content;
            public Action OnClose;
            public int WindowId;
        }

        private static readonly Lazy<LayerManager> _instance = new(() => new LayerManager());

        public static LayerManager Instance => _instance.Value;

        private readonly Dictionary<string, LayerState> _layers = new();
        private readonly List<string> _drawOrder = new();
        private readonly List<string> _pendingClose = new();
        private int _nextWindowId = 50000;
        private bool _drawing;

        private LayerManager() { }

        public void Open(LayerConfig config)
        {
            if (config == null || string.IsNullOrWhiteSpace(config.Id))
                return;

            var state = new LayerState
            {
                Id = config.Id,
                Position = config.OpenPosition,
                Width = Mathf.Max(1f, config.Width),
                Height = Mathf.Max(1f, config.Height),
                ZIndex = config.ZIndex,
                CloseOnClickOutside = config.CloseOnClickOutside,
                ShowOverlay = config.ShowOverlay,
                DrawChrome = config.DrawChrome,
                Content = config.Content,
                OnClose = config.OnClose,
                WindowId = _layers.TryGetValue(config.Id, out var existing) ? existing.WindowId : _nextWindowId++,
            };

            _layers[config.Id] = state;
            if (!_drawOrder.Contains(config.Id))
                _drawOrder.Add(config.Id);

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
            if (string.IsNullOrWhiteSpace(id))
                return;

            if (_drawing)
            {
                if (!_pendingClose.Contains(id))
                    _pendingClose.Add(id);
                return;
            }

            CloseNow(id);
        }

        public void CloseAll()
        {
            foreach (var id in new List<string>(_drawOrder))
                Close(id);
        }

        public bool IsOpen(string id) => !string.IsNullOrWhiteSpace(id) && _layers.ContainsKey(id);

        public bool HasOpenLayers() => _drawOrder.Count > 0;

        public int GetLayerCount() => _drawOrder.Count;

        public Vector2 GetOpenPosition(string id)
        {
            return _layers.TryGetValue(id, out var layer) ? layer.Position : Vector2.zero;
        }

        public void SetPosition(string id, Vector2 position)
        {
            if (_layers.TryGetValue(id, out var layer))
                layer.Position = position;
        }

        public void BringToFront(string id)
        {
            if (!_layers.TryGetValue(id, out var layer))
                return;

            int max = 0;
            foreach (var item in _layers.Values)
                max = Mathf.Max(max, item.ZIndex);

            layer.ZIndex = max + 1;
            SortDrawOrder();
        }

        public void DrawLayers()
        {
            if (_drawOrder.Count == 0)
                return;

            _drawing = true;
            _pendingClose.Clear();

            if (NeedsOverlay())
                DrawOverlay();

            var order = new List<string>(_drawOrder);
            var topWindowId = -1;

            foreach (var id in order)
            {
                if (!_layers.TryGetValue(id, out var layer))
                    continue;

                var rect = ClampToScreen(new Rect(layer.Position.x, layer.Position.y, layer.Width, layer.Height));
                layer.Position = rect.position;
                GUI.Window(layer.WindowId, rect, _ => DrawLayer(layer), GUIContent.none, GUIStyle.none);
                topWindowId = layer.WindowId;
            }

            if (topWindowId >= 0)
                GUI.BringWindowToFront(topWindowId);

            HandleOutsideClick(order);

            _drawing = false;

            foreach (var id in _pendingClose)
                CloseNow(id);
        }

        private void DrawLayer(LayerState layer)
        {
            if (layer.DrawChrome)
                DrawChrome(layer.Width, layer.Height);

            layer.Content?.Invoke();
        }

        private void DrawChrome(float width, float height)
        {
            var theme = ThemeManager.Instance.CurrentTheme;
            var rect = new Rect(0f, 0f, width, height);
            var previous = GUI.color;
            var border = Mathf.Max(1f, theme.Metrics.BorderWidth);

            GUI.color = theme.Elevated;
            GUI.DrawTexture(rect, Texture2D.whiteTexture);

            GUI.color = theme.Border;
            GUI.DrawTexture(new Rect(rect.x, rect.y, rect.width, border), Texture2D.whiteTexture);
            GUI.DrawTexture(new Rect(rect.x, rect.yMax - border, rect.width, border), Texture2D.whiteTexture);
            GUI.DrawTexture(new Rect(rect.x, rect.y, border, rect.height), Texture2D.whiteTexture);
            GUI.DrawTexture(new Rect(rect.xMax - border, rect.y, border, rect.height), Texture2D.whiteTexture);
            GUI.color = previous;
        }

        private void DrawOverlay()
        {
            var theme = ThemeManager.Instance.CurrentTheme;
            var previous = GUI.color;
            GUI.color = theme.Overlay;
            GUI.DrawTexture(new Rect(0f, 0f, Screen.width, Screen.height), Texture2D.whiteTexture);
            GUI.color = previous;
        }

        private bool NeedsOverlay()
        {
            foreach (var id in _drawOrder)
            {
                if (_layers.TryGetValue(id, out var layer) && layer.ShowOverlay)
                    return true;
            }

            return false;
        }

        private void HandleOutsideClick(List<string> order)
        {
            if (Event.current.type != EventType.MouseDown || Event.current.button != 0)
                return;

            var mouse = Event.current.mousePosition;

            for (int i = order.Count - 1; i >= 0; i--)
            {
                var id = order[i];
                if (!_layers.TryGetValue(id, out var layer))
                    continue;

                var rect = new Rect(layer.Position.x, layer.Position.y, layer.Width, layer.Height);
                if (rect.Contains(mouse))
                    return;

                if (layer.CloseOnClickOutside)
                {
                    _pendingClose.Add(id);
                    Event.current.Use();
                    return;
                }
            }
        }

        private void CloseNow(string id)
        {
            if (!_layers.TryGetValue(id, out var layer))
                return;

            try
            {
                layer.OnClose?.Invoke();
            }
            catch (Exception ex)
            {
                GUILogger.LogException(ex, nameof(CloseNow), nameof(LayerManager));
            }
            finally
            {
                _layers.Remove(id);
                _drawOrder.Remove(id);
            }
        }

        private Rect ClampToScreen(Rect rect)
        {
            const float margin = 8f;
            float x = Mathf.Clamp(rect.x, margin, Mathf.Max(margin, Screen.width - rect.width - margin));
            float y = Mathf.Clamp(rect.y, margin, Mathf.Max(margin, Screen.height - rect.height - margin));
            return new Rect(x, y, rect.width, rect.height);
        }

        private void SortDrawOrder()
        {
            _drawOrder.Sort(
                (left, right) =>
                {
                    var leftZ = _layers.TryGetValue(left, out var leftLayer) ? leftLayer.ZIndex : 0;
                    var rightZ = _layers.TryGetValue(right, out var rightLayer) ? rightLayer.ZIndex : 0;
                    return leftZ.CompareTo(rightZ);
                }
            );
        }
    }
}
