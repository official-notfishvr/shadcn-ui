using UnityEngine;
using ShadcnUI.Core.Styling;

namespace ShadcnUI;

#region Variants and Styles

/// <summary>
/// Dialog variant configuration.
/// </summary>
public readonly record struct DialogVariants(
    ControlVariant Variant = ControlVariant.Default
);

/// <summary>
/// Dialog style helper.
/// </summary>
public static class DialogStyles
{
    public static GUIStyle GetContainerStyle(DialogVariants variants)
    {
        return Shadcn.GetStyle(StyleComponentType.Dialog, variants.Variant);
    }

    public static GUIStyle GetHeaderStyle()
    {
        return Shadcn.GetStyle(StyleComponentType.DialogHeader);
    }

    public static GUIStyle GetFooterStyle()
    {
        return Shadcn.GetStyle(StyleComponentType.DialogFooter);
    }
}

#endregion

#region Dialog Component

/// <summary>
/// Dialog component for modal overlays and confirmations.
/// </summary>
public static partial class Shadcn
{
    private static readonly Dictionary<string, bool> _dialogStates = new();
    private static string? _activeModalDialog;

    /// <summary>
    /// Opens a dialog by ID.
    /// </summary>
    public static void OpenDialog(string id)
    {
        _dialogStates[id] = true;
        _activeModalDialog = id;
    }

    /// <summary>
    /// Closes a dialog by ID.
    /// </summary>
    public static void CloseDialog(string id)
    {
        _dialogStates[id] = false;
        if (_activeModalDialog == id)
        {
            _activeModalDialog = null;
        }
    }

    /// <summary>
    /// Checks if a dialog is open.
    /// </summary>
    public static bool IsDialogOpen(string id)
    {
        return _dialogStates.TryGetValue(id, out var isOpen) && isOpen;
    }

    /// <summary>
    /// Renders a dialog if it's open.
    /// </summary>
    /// <param name="id">Unique dialog ID</param>
    /// <param name="content">Dialog content renderer</param>
    /// <param name="title">Dialog title</param>
    /// <param name="width">Dialog width</param>
    /// <param name="variants">Visual variants</param>
    /// <param name="closeOnOverlayClick">Close when clicking outside</param>
    /// <param name="showOverlay">Show dark overlay</param>
    public static void Dialog(
        string id,
        Action<DialogContext> content,
        string? title = null,
        float width = 400f,
        DialogVariants? variants = null,
        bool closeOnOverlayClick = true,
        bool showOverlay = true)
    {
        if (!IsDialogOpen(id)) return;

        var v = variants ?? new DialogVariants();
        var theme = CurrentTheme;
        
        // Overlay
        if (showOverlay)
        {
            var overlayRect = new Rect(0, 0, Screen.width, Screen.height);
            GUI.DrawTexture(overlayRect, RenderHelpers.CreateSolidTexture(theme.Overlay));
            
            if (closeOnOverlayClick && Event.current.type == EventType.MouseDown 
                && overlayRect.Contains(Event.current.mousePosition))
            {
                CloseDialog(id);
                Event.current.Use();
                return;
            }
        }

        // Dialog container
        var dialogWidth = width * UIScale;
        var dialogRect = new Rect(
            (Screen.width - dialogWidth) / 2,
            Screen.height * 0.2f,
            dialogWidth,
            100 // Initial height, will expand
        );

        // Draw dialog background
        var bgStyle = new GUIStyle(GUI.skin.box)
        {
            normal = { background = RenderHelpers.CreateSolidTexture(theme.Card) }
        };

        GUILayout.BeginArea(dialogRect, bgStyle);
        
        // Header
        if (!string.IsNullOrEmpty(title))
        {
            DialogHeader(title);
        }

        // Content
        var context = new DialogContext(id);
        content?.Invoke(context);
        
        GUILayout.EndArea();
    }

    /// <summary>
    /// Renders a simple confirmation dialog.
    /// </summary>
    public static void ConfirmDialog(
        string id,
        string title,
        string message,
        Action onConfirm,
        Action? onCancel = null,
        string confirmText = "Confirm",
        string cancelText = "Cancel",
        ControlVariant confirmVariant = ControlVariant.Primary)
    {
        Dialog(id, context =>
        {
            DialogHeader(title);
            
            DialogContent(() =>
            {
                Label(message);
            });
            
            DialogFooter(() =>
            {
                FlexibleSpace();
                
                if (Button(cancelText, () =>
                {
                    onCancel?.Invoke();
                    CloseDialog(id);
                }, new ButtonVariants(ControlVariant.Ghost)))
                {
                    // Handled in callback
                }
                
                Space(DesignTokens.Spacing.SM);
                
                if (Button(confirmText, () =>
                {
                    onConfirm();
                    CloseDialog(id);
                }, new ButtonVariants(confirmVariant)))
                {
                    // Handled in callback
                }
            });
        }, width: 400f);
    }

    #region Dialog Parts

    /// <summary>
    /// Renders a dialog header.
    /// </summary>
    public static void DialogHeader(string title)
    {
        var theme = CurrentTheme;
        var style = new GUIStyle(GUI.skin.label)
        {
            fontSize = Mathf.RoundToInt(DesignTokens.FontSize.LG * UIScale),
            fontStyle = FontStyle.Bold,
            normal = { textColor = theme.Foreground }
        };
        
        GUILayout.Label(title, style);
        Space(DesignTokens.Spacing.MD);
    }

    /// <summary>
    /// Renders dialog content area.
    /// </summary>
    public static void DialogContent(Action content)
    {
        content?.Invoke();
        Space(DesignTokens.Spacing.MD);
    }

    /// <summary>
    /// Renders a dialog footer with actions.
    /// </summary>
    public static void DialogFooter(Action content)
    {
        Space(DesignTokens.Spacing.MD);
        content?.Invoke();
    }

    #endregion

    #region Helper Methods

    #endregion
}

/// <summary>
/// Context passed to dialog content for accessing dialog operations.
/// </summary>
public sealed class DialogContext
{
    public string Id { get; }

    public DialogContext(string id)
    {
        Id = id;
    }

    /// <summary>
    /// Closes this dialog.
    /// </summary>
    public void Close()
    {
        Shadcn.CloseDialog(Id);
    }
}

#endregion
