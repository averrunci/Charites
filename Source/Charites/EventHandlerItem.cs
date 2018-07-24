﻿// Copyright (C) 2018 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System;
using System.Threading.Tasks;

namespace Charites.Windows.Mvc
{
    /// <summary>
    /// Represents an item of an event handler.
    /// </summary>
    /// <typeparam name="TElement">The base type of the view.</typeparam>
    public abstract class EventHandlerItem<TElement> where TElement : class
    {
        private readonly string elementName;
        private readonly TElement element;
        private readonly string eventName;
        private readonly Delegate handler;
        private readonly bool handledEventsToo;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventHandlerItem{TElement}"/> class
        /// with the specified element name, element, event name, event handler, and
        /// a value that indicates whether to register the handler such that
        /// it is invoked even when the event is marked handled in its event data.
        /// </summary>
        /// <param name="elementName">The name of the element that raises the event.</param>
        /// <param name="element">The element that raises the event.</param>
        /// <param name="eventName">The name of the event.</param>
        /// <param name="handler">The handler of the event.</param>
        /// <param name="handledEventsToo">
        /// <c>true</c> to register the handler such that it is invoked even when the
        /// event is marked handled in its event data; <c>false</c> to register the
        /// handler with the default condition that it will not be invoked if the event
        /// is already marked handled.
        /// </param>
        protected EventHandlerItem(string elementName, TElement element, string eventName, Delegate handler, bool handledEventsToo)
        {
            this.elementName = elementName;
            this.element = element;
            this.eventName = eventName;
            this.handler = handler;
            this.handledEventsToo = handledEventsToo;
        }

        /// <summary>
        /// Gets a value that indicates whether <see cref="EventHandlerItem{TElement}"/> has the specified element name.
        /// </summary>
        /// <remarks>
        /// If the specified element name is <c>null</c>, it is converted to <see cref="String.Empty"/>.
        /// </remarks>
        /// <param name="elementName">The name of the element.</param>
        /// <returns>
        /// <c>true</c> if <see cref="EventHandlerItem{TElement}"/> has the specified element name;
        /// otherwise, <c>false</c>.
        /// </returns>
        public bool Has(string elementName) => this.elementName == (elementName ?? string.Empty);

        /// <summary>
        /// Adds the event handler to the element.
        /// </summary>
        public void AddEventHandler() => AddEventHandler(element, handler, handledEventsToo);

        /// <summary>
        /// Removes the event handler from the element.
        /// </summary>
        public void RemoveEventHandler() => RemoveEventHandler(element, handler);

        /// <summary>
        /// Raises the event of the specified name.
        /// </summary>
        /// <param name="eventName">The name of the event to raise.</param>
        /// <param name="sender">The object where the event handler is attached.</param>
        /// <param name="e">The event data.</param>
        public void Raise(string eventName, object sender, object e)
        {
            if (this.eventName != eventName || handler == null) return;

            switch (handler.Method.GetParameters().Length)
            {
                case 0:
                    handler.DynamicInvoke();
                    break;
                case 1:
                    handler.DynamicInvoke(e);
                    break;
                case 2:
                    handler.DynamicInvoke(sender, e);
                    break;
            }
        }

        /// <summary>
        /// Raises the event of the specified name asynchronously.
        /// </summary>
        /// <param name="eventName">The name of the event to raise.</param>
        /// <param name="sender">The object where the event handler is attached.</param>
        /// <param name="e">The event data.</param>
        /// <returns>A task that represents the asynchronous raise operation.</returns>
        public async Task RaiseAsync(string eventName, object sender, object e)
        {
            if (this.eventName != eventName || handler == null) return;

            if (Handle(handler, sender, e) is Task task)
            {
                await task;
            }
        }

        /// <summary>
        /// Adds the specified event handler to the specified element.
        /// </summary>
        /// <param name="element">The element to which the specified event handler is added.</param>
        /// <param name="handler">The event handler to add.</param>
        /// <param name="handledEventsToo">
        /// <c>true</c> to register the handler such that it is invoked even when the
        /// event is marked handled in its event data; <c>false</c> to register the
        /// handler with the default condition that it will not be invoked if the event
        /// is already marked handled.
        /// </param>
        protected abstract void AddEventHandler(TElement element, Delegate handler, bool handledEventsToo);

        /// <summary>
        /// Removes the specified event handler from the specified element.
        /// </summary>
        /// <param name="element">The element from which the specified event handler is removed.</param>
        /// <param name="handler">The event handler to remove.</param>
        protected abstract void RemoveEventHandler(TElement element, Delegate handler);

        /// <summary>
        /// Handles the event with the specified object where the event handler is attached
        /// and event data.
        /// </summary>
        /// <param name="handler">The event handler to handle the event.</param>
        /// <param name="sender">The object where the event handler is attached.</param>
        /// <param name="e">The event data.</param>
        /// <returns>An object containing the return value of the handler method.</returns>
        protected virtual object Handle(Delegate handler, object sender, object e)
            => (handler.Target as EventHandlerAction)?.Handle(sender, e);
    }
}
