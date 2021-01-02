/// =================================
/// Author: Shaun Curtis, Cold Elm
/// License: MIT
/// ==================================

using System;
using System.Collections.Generic;

namespace CEC.Blazor.Core
{
    /// <summary>
    /// Describes information for a view
    /// </summary>
    public sealed class ViewData
    {
        /// <summary>
        /// Gets the type of the View.
        /// </summary>
        public Type ViewType { get; set; }

        /// <summary>
        /// Gets the type of the page matching the route.
        /// </summary>
        public Type LayoutType { get; set; }

        /// <summary>
        /// Parameter values to add to the Route when created
        /// </summary>
        public Dictionary<string, object> ViewParameters { get; private set; } = new Dictionary<string, object>();

        /// <summary>
        /// View values that can be used by the view and subcomponents
        /// </summary>
        public Dictionary<string, object> ViewFields { get; private set; } = new Dictionary<string, object>();

        /// <summary>
        /// Constructs an instance of <see cref="ViewData"/>.
        /// </summary>
        /// <param name="viewType">The type of the view, which must implement <see cref="IView"/>.</param>
        /// <param name="viewValues">The view parameter values.</param>
        public ViewData(Type viewType, Dictionary<string, object> viewValues)
        {
            if (viewType == null) throw new ArgumentNullException(nameof(viewType));
            if (!typeof(IView).IsAssignableFrom(viewType)) throw new ArgumentException($"The view must implement {nameof(IView)}.", nameof(viewType));
            this.ViewType = viewType;
            if (viewValues != null) this.ViewParameters = viewValues;
        }
    }
}
