/// =================================
/// Author: Shaun Curtis, Cold Elm
/// License: MIT
/// ==================================

using Microsoft.AspNetCore.Components;
using System;

namespace CEC.Blazor.Core
{
    /// <summary>
    /// Base View Component  All views should inherit from this
    /// Implements IView
    /// </summary>
    public interface IView : IComponent
    {
        public Guid GUID => Guid.NewGuid();

        [CascadingParameter] public ViewManager ViewManager { get; set; }

    }
}
