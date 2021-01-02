using System;

namespace CEC.Blazor.Core
{
    /// <summary>
    /// Interface used to label a Form for Identification in the UI
    /// Implemented now so we don't need to backtrack and re-code for it down the road!
    /// </summary>
    public interface IForm
    {
        public Guid GUID => Guid.NewGuid();
    }
}
