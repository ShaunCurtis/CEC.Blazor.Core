﻿/// =================================
/// Author: Shaun Curtis, Cold Elm
/// License: MIT
/// ==================================

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Threading.Tasks;

namespace CEC.Blazor.Core
{
    /// <summary>
    /// Abstract Base Class implementing basic IComponent functions
    /// Much of the code is common with ComponentBase
    /// Blazor Team copyright notices recognised.
    /// </summary>
    public abstract class Component : IComponent, IHandleEvent, IHandleAfterRender
    {
        // The Render Fragment passed by the component to the Renderer when the component initialiates a Render
        private readonly RenderFragment _renderFragment;
        
        // The RenderHandle passed to the component by the Renderer when the Cpmponent is attached to the RenderTree
        private RenderHandle _renderHandle;

        // Bool global used to track when the component is it's first render cycle
        private bool _firstRender = true;

        // Bool global set to false when _renderFragment is first run by the Renderer
        private bool _hasNeverRendered = true;

        // Bool gobal set when the component queues the _renderfragment onto the Render Queue.  Set back to false by the _renderfragment when it's run by the Renderer.
        private bool _hasPendingQueuedRender;

        // Bool global to track the after render process 
        private bool _hasCalledOnAfterRender;

        /// <summary>
        /// Property to check if the component is loading -  set internally
        /// </summary>
        public bool Loading { get; protected set; } = true;

        /// <summary>
        /// Constructs an instance of <see cref="ControlBase"/>.
        /// </summary>
        public Component()
        {
            _renderFragment = builder =>
            {
                _hasPendingQueuedRender = false;
                _hasNeverRendered = false;
                BuildRenderTree(builder);
            };
        }

        /// <summary>
        /// Renders the component to the supplied <see cref="RenderTreeBuilder"/>.
        /// </summary>
        /// <param name="builder">A <see cref="RenderTreeBuilder"/> that will receive the render output.</param>
        protected virtual void BuildRenderTree(RenderTreeBuilder builder)
        {
            // Developers can either override this method in derived classes, or can use Razor
            // syntax to define a derived class and have the compiler generate the method.

            // Other code within this class should *not* invoke BuildRenderTree directly,
            // but instead should invoke the _renderFragment field.
        }


        /// <summary>
        /// Method called when things have changed on the component and it needs queue a render event on the Render Queue.
        /// It checks if no request is already queued and passes the Render Queue the _renderFragment.
        /// If there's already a render request queued then the current changes will be handled by the queued  request.
        /// </summary>
        protected void Render()
        {
            if (_hasPendingQueuedRender) return;
            if (_hasNeverRendered || ShouldRender())
            {
                _hasPendingQueuedRender = true;
                try
                {
                    _renderHandle.Render(_renderFragment);
                }
                catch
                {
                    _hasPendingQueuedRender = false;
                    throw;
                }
            }
        }

        /// <summary>
        /// Method Called on a Component by the Renderer when it's attached to the RenderTree
        /// Only called once
        /// </summary>
        /// <returns></returns>
        protected virtual Task OnAttachAsync() => Task.CompletedTask;

        /// <summary>
        /// Runs the Render Method on the UI Thread
        /// </summary>
        /// <returns></returns>
        protected async Task RenderAsync() => await this.InvokeAsync(Render);

        /// <summary>
        /// Event handler version to run Render on the UI Thread
        /// </summary>
        /// <returns></returns>
        protected async void RenderAsync(object sender, EventArgs e) => await this.InvokeAsync(Render);

        /// <summary>
        /// Returns a flag to indicate whether the component should render.
        /// </summary>
        /// <returns></returns>
        protected virtual bool ShouldRender() => true;

        /// <summary>
        /// Public Method called after the component has been rendered
        /// </summary>
        /// <param name="firstRender"></param>
        /// <returns></returns>
        protected virtual Task OnAfterRenderAsync(bool firstRender)
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// Executes the supplied work item on the associated renderer's
        /// synchronization context.
        /// </summary>
        /// <param name="workItem">The work item to execute.</param>
        protected Task InvokeAsync(Action workItem) => _renderHandle.Dispatcher.InvokeAsync(workItem);

        /// <summary>
        /// Executes the supplied work item on the associated renderer's
        /// synchronization context.
        /// </summary>
        /// <param name="workItem">The work item to execute.</param>
        protected Task InvokeAsync(Func<Task> workItem) => _renderHandle.Dispatcher.InvokeAsync(workItem);

        /// <summary>
        /// IComponent Attach implementation
        /// Called by the Renderer when the component is attached to the Render Tree
        /// </summary>
        /// <param name="renderHandle"></param>
        async void IComponent.Attach(RenderHandle renderHandle)
        {
            if (_renderHandle.IsInitialized)
            {
                throw new InvalidOperationException($"The render handle is already set. Cannot initialize a {nameof(ComponentBase)} more than once.");
            }
            _firstRender = true;
            _renderHandle = renderHandle;
            await OnAttachAsync();
        }
        
        /// <summary>
        /// Implementation of the IComponent SetParametersAsync
        /// Called the the Renderer when one of more externally linked Parameters have changed
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task SetParametersAsync(ParameterView parameters)
        {
            parameters.SetParameterProperties(this);
            await this._StartRenderAsync();
        }

        /// <summary>
        /// Method to reset the component and render from scatch
        /// </summary>
        /// <returns></returns>
        public virtual async Task ResetAsync()
        {
            this._firstRender = true;
            this.Loading = true;
            await this._StartRenderAsync();
        }

        /// <summary>
        /// Internal Method called from SetParametersAsync to begin the render process
        /// Tracks if this is the first or subsequnet renders
        /// </summary>
        /// <returns></returns>
        private async Task _StartRenderAsync()
        {
            this.Loading = true;
            await RenderAsync();
            await this.OnRenderAsync(this._firstRender);
            this._firstRender = false;
            this.Loading = false;
            await RenderAsync();
        }

        /// <summary>
        /// Exposed Render "Event"
        /// Gets called when SetParametersAsync has been called by the Renderer
        /// </summary>
        /// <param name="firstRender"></param>
        /// <returns></returns>
        protected virtual Task OnRenderAsync(bool firstRender) => Task.CompletedTask;

        /// <summary>
        /// Internal method to track the render event
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="arg"></param>
        /// <returns></returns>
        Task IHandleEvent.HandleEventAsync(EventCallbackWorkItem callback, object arg)
        {
            var task = callback.InvokeAsync(arg);
            var shouldAwaitTask = task.Status != TaskStatus.RanToCompletion && task.Status != TaskStatus.Canceled;
            InvokeAsync(Render);
            return shouldAwaitTask ? CallRenderOnAsyncCompletion(task) : Task.CompletedTask;
        }

        /// <summary>
        /// Internal Method triggered after the component has rendered calling OnAfterRenderAsync
        /// </summary>
        /// <returns></returns>
        Task IHandleAfterRender.OnAfterRenderAsync()
        {
            var firstRender = !_hasCalledOnAfterRender;
            _hasCalledOnAfterRender |= true;
            return OnAfterRenderAsync(firstRender);
        }

        /// <summary>
        /// Internal method to handle render completion
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        private async Task CallRenderOnAsyncCompletion(Task task)
        {
            try { await task; }
            catch
            {
                if (task.IsCanceled) return;
                else throw;
            }
            await InvokeAsync(Render);
        }
    }
}
