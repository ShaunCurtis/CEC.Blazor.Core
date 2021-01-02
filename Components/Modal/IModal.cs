/// =================================
/// Author: Shaun Curtis, Cold Elm
/// License: MIT
/// ==================================

using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace CEC.Blazor.Core
{
    /// <summary>
    /// Interface defining the core functionality of a Modal Dialog
    /// </summary>
    public interface IModal
    {
        /// <summary>
        /// Options class for defining 
        /// </summary>
        ModalOptions Options { get; set; }

        /// <summary>
        /// Method called to display the modal dialog based on the Options settings
        /// </summary>
        /// <typeparam name="TModal"></typeparam>
        /// <param name="options"></param>
        /// <returns></returns>
        Task<ModalResult> ShowAsync<TModal>(ModalOptions options) where TModal : IComponent;

        /// <summary>
        /// Method called to update whatever is being displayed
        /// </summary>
        /// <param name="options"></param>
        void Update(ModalOptions options = null);

        /// <summary>
        /// Method called to dismiss the modal
        /// called by the dismiss button/link in the title
        /// </summary>
        void Dismiss();

        /// <summary>
        /// Method called to close the modal
        /// </summary>
        /// <param name="result"></param>
        void Close(ModalResult result);
    }
}
