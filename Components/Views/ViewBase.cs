/// =================================
/// Author: Shaun Curtis, Cold Elm
/// License: MIT
/// ==================================

using Microsoft.AspNetCore.Components;

namespace CEC.Blazor.Core
{
    public class ViewBase : Component, IView
    {
        [CascadingParameter]
        public ViewManager ViewManager { get; set; }
    }
}
