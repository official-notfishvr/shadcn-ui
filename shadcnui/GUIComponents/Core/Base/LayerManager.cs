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
        private readonly List<string> _layerOrder = new List<string>();

        private class LayerState
        {
            public string Id;
            public Vector2 OpenPosition;
            public float Width;
            public float Height;
            public int ZIndex;
            public bool CloseOnClickOutside;
            public bool ShowOverlay;
            public Action Content;
            public Action OnClose;
            public bool IsOpen;
        }

        public void Open(LayerConfig config)
        {
            if (config == null || string.IsNullOrEmpty(config.Id))
                return;

            var state = new LayerState
            {
                Id = config.Id,
                OpenPosition = config.OpenPosition,
                Width = config.Width,
                Height = config.Height,
                ZIndex = config.ZIndex,
                CloseOnClickOutside = config.CloseOnClickOutside,
                ShowOverlay = config.ShowOverlay,
                Content = config.Content,
                OnClose = config.OnClose,
                IsOpen = true,
            };

            _layers[config.Id] = state;

            if (!_layerOrder.Contains(config.Id))
                _layerOrder.Add(config.Id);

            SortLayers();
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
            if (_layers.TryGetValue(id, out var state))
            {
                state.IsOpen = false;
                state.OnClose?.Invoke();
                _layers.Remove(id);
                _layerOrder.Remove(id);
            }
        }

        public void CloseAll()
        {
            foreach (var id in new List<string>(_layerOrder))
            {
                Close(id);
            }
        }

        public bool IsOpen(string id)
        {
            return _layers.TryGetValue(id, out var state) && state.IsOpen;
        }

        public Vector2 GetOpenPosition(string id)
        {
            if (_layers.TryGetValue(id, out var state))
                return state.OpenPosition;
            return Vector2.zero;
        }

        public void SetPosition(string id, Vector2 position)
        {
            if (_layers.TryGetValue(id, out var state))
                state.OpenPosition = position;
        }

        public void BringToFront(string id)
        {
            if (_layers.TryGetValue(id, out var state))
            {
                int maxZ = 100;
                foreach (var layer in _layers.Values)
                {
                    if (layer.ZIndex > maxZ)
                        maxZ = layer.ZIndex;
                }
                state.ZIndex = maxZ + 1;
                SortLayers();
            }
        }

        public void DrawLayers()
        {
            if (_layerOrder.Count == 0)
                return;

            string clickedOutsideLayer = null;

            foreach (var id in _layerOrder)
            {
                if (!_layers.TryGetValue(id, out var state) || !state.IsOpen)
                    continue;

                if (state.ShowOverlay)
                {
                    DrawOverlay();
                }

                Rect layerRect = new Rect(state.OpenPosition.x, state.OpenPosition.y, state.Width, state.Height);

                layerRect = ClampToScreen(layerRect);

                if (state.CloseOnClickOutside && Event.current.type == EventType.MouseUp && Event.current.button == 0)
                {
                    if (!layerRect.Contains(Event.current.mousePosition))
                    {
                        clickedOutsideLayer = id;
                    }
                }

                int windowId = GetWindowId(id);

                GUI.Window(
                    windowId,
                    layerRect,
                    (GUI.WindowFunction)(
                        wid =>
                        {
                            var theme = ThemeManager.Instance.CurrentTheme;
                            Color prevColor = GUI.color;
                            GUI.color = theme.Elevated;
                            GUI.DrawTexture(new Rect(0, 0, state.Width, state.Height), Texture2D.whiteTexture);

                            GUI.color = theme.Border;
                            float borderWidth = 1f;
                            GUI.DrawTexture(new Rect(0, 0, state.Width, borderWidth), Texture2D.whiteTexture);
                            GUI.DrawTexture(new Rect(0, state.Height - borderWidth, state.Width, borderWidth), Texture2D.whiteTexture);
                            GUI.DrawTexture(new Rect(0, 0, borderWidth, state.Height), Texture2D.whiteTexture);
                            GUI.DrawTexture(new Rect(state.Width - borderWidth, 0, borderWidth, state.Height), Texture2D.whiteTexture);

                            GUI.color = prevColor;

                            state.Content?.Invoke();
                        }
                    ),
                    GUIContent.none,
                    GUIStyle.none
                );

                GUI.BringWindowToFront(windowId);
            }

            if (clickedOutsideLayer != null)
            {
                Close(clickedOutsideLayer);
                Event.current.Use();
            }
        }

        private int GetWindowId(string id)
        {
            return 50000 + Math.Abs(id.GetHashCode() % 10000);
        }

        public void DrawLayer(string id, Action content)
        {
            if (!_layers.TryGetValue(id, out var state) || !state.IsOpen)
                return;

            if (state.ShowOverlay)
            {
                DrawOverlay();
            }

            Rect layerRect = new Rect(state.OpenPosition.x, state.OpenPosition.y, state.Width, state.Height);
            layerRect = ClampToScreen(layerRect);

            if (state.CloseOnClickOutside && Event.current.type == EventType.MouseDown)
            {
                if (!layerRect.Contains(Event.current.mousePosition))
                {
                    Close(id);
                    Event.current.Use();
                    return;
                }
            }

            GUILayout.BeginArea(layerRect);
            content?.Invoke();
            GUILayout.EndArea();
        }

        private void DrawOverlay()
        {
            Color prev = GUI.color;
            Color overlayColor = ThemeManager.Instance.CurrentTheme.Overlay;
            GUI.color = overlayColor;
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), Texture2D.whiteTexture);
            GUI.color = prev;
        }

        private Rect ClampToScreen(Rect rect)
        {
            float x = rect.x;
            float y = rect.y;

            if (x + rect.width > Screen.width)
                x = Screen.width - rect.width;
            if (y + rect.height > Screen.height)
                y = Screen.height - rect.height;
            if (x < 0)
                x = 0;
            if (y < 0)
                y = 0;

            return new Rect(x, y, rect.width, rect.height);
        }

        private void SortLayers()
        {
            _layerOrder.Sort(
                (a, b) =>
                {
                    int zA = _layers.TryGetValue(a, out var stateA) ? stateA.ZIndex : 0;
                    int zB = _layers.TryGetValue(b, out var stateB) ? stateB.ZIndex : 0;
                    return zA.CompareTo(zB);
                }
            );
        }

        public int GetLayerCount() => _layerOrder.Count;

        public bool HasOpenLayers() => _layerOrder.Count > 0;
    }
}
