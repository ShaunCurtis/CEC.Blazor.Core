using Microsoft.AspNetCore.Components;
using System;

namespace CEC.Blazor.Core
{
    public class ViewBase : Component, IView
    {
        [CascadingParameter]
        public ViewManager ViewManager { get; set; }
    }
}
