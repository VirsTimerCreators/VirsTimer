using ReactiveUI;
using System;

namespace VirsTimer.DesktopApp.ViewModels
{
    public class ViewModelBase : ReactiveObject
    {
        protected event EventHandler Constructed;
        protected virtual void OnConstructedAsync(object? sender, EventArgs e) { }

        public ViewModelBase()
        {
            Constructed += OnConstructedAsync;
        }
    }
}
