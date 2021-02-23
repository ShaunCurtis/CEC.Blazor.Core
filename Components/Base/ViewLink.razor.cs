using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace CEC.Blazor.Core
{
    public partial class ViewLink : BaseComponent
    {
        /// <summary>
        /// View Type to Load
        /// </summary>
        [Parameter] public Type ViewType { get; set; }

        /// <summary>
        /// View Paremeters for the View
        /// </summary>
        [Parameter] public Dictionary<string, object> ViewParameters { get; set; } = new Dictionary<string, object>();

        /// <summary>
        /// Child Content to add to Component
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Cascaded ViewManager
        /// </summary>
        [CascadingParameter] public ViewManager ViewManager { get; set; }

        [Parameter(CaptureUnmatchedValues = true)] public IDictionary<string, object> AdditionalAttributes { get; set; }

        /// <summary>
        /// Boolean to control Display State
        /// </summary>
        [Parameter] public bool Display { get; set; } = true;

        /// <summary>
        /// Boolean to check if the ViewType is the current loaded view
        /// if so it's used to mark this component's CSS with "active" 
        /// </summary>
        private bool IsActive => this.ViewManager.IsCurrentView(this.ViewType);

        /// <summary>
        /// Base CSS string- check if we're active
        /// </summary>
        private string _baseClass => this.IsActive ? "view-link active" : "view-link";

        /// <summary>
        /// ViewData to be returned to the ViewManager
        /// </summary>
        private ViewData viewData => new ViewData(ViewType, ViewParameters);


        private string CssClass
            => (AdditionalAttributes != null && AdditionalAttributes.TryGetValue("class", out var obj))
                    ? $"{ Convert.ToString(obj, CultureInfo.InvariantCulture)} {this._baseClass}"
                    : this._baseClass;

        protected void LoadView(ViewData viewData)
            => this.ViewManager.LoadViewAsync(viewData);

    }
}