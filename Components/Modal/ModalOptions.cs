/// =================================
/// Author: Shaun Curtis, Cold Elm
/// License: MIT
/// ==================================

using System.Collections.Generic;

namespace CEC.Blazor.Core
{
    public class ModalOptions
    {
        /// <summary>
        /// Modal Dialog Title
        /// </summary>
        public string Title { get; set; } = "Modal Dialog";

        /// <summary>
        /// Bool to control the close button in the Header
        /// </summary>
        public bool ShowCloseButton { get; set; }

        /// <summary>
        /// Bool to control display of the header
        /// </summary>
        public bool HideHeader { get; set; }

        /// <summary>
        /// Dictionary of additional Parameters
        /// Read only.  Use the Dictionary Extension Setters and getters provided through CEC.Blazor.Core namespace
        /// </summary>
        public Dictionary<string, object> Parameters { get; } = new Dictionary<string, object>();
    }
}
