using System;
using Avalonia.VisualTree;

namespace VirsTimer.DesktopApp.Extensions
{
    public static class ControlsExtensions
    {
        public static Related FindRelatedControl<Control, Parent, Related>(
            this Control control,
            Action<Related>? onContinuation = null)
            where Control : Avalonia.Controls.Control
            where Parent : Avalonia.Controls.Control
            where Related : Avalonia.Controls.Control
        {
            var parent = control.FindAncestorOfType<Parent>();
            var related = parent.FindDescendantOfType<Related>();
            onContinuation?.Invoke(related);

            return related;
        }
    }
}