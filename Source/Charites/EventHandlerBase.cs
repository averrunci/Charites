// Copyright (C) 2018 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Charites.Windows.Mvc
{
    /// <summary>
    /// Represents the base of event handler.
    /// </summary>
    /// <typeparam name="TElement">The base type of the view.</typeparam>
    /// <typeparam name="TItem">The type of the item that contains the context of the event handler.</typeparam>
    public class EventHandlerBase<TElement, TItem> where TElement : class where TItem : EventHandlerItem<TElement>
    {
        private readonly ICollection<TItem> items = new Collection<TItem>();

        /// <summary>
        /// Initializes a new instance of the <see cref="EventHandlerBase{TElement,TItem}"/> class.
        /// </summary>
        public EventHandlerBase()
        {
        }

        /// <summary>
        /// Adds an item that contains the context of the event handler.
        /// </summary>
        /// <param name="item">The item that contains the context of the event handler to add.</param>
        public void Add(TItem item) => items.Add(item);

        /// <summary>
        /// Removes an item that contains the context of the event handler.
        /// </summary>
        /// <param name="item">The item that contains the context of the event handler to remove.</param>
        public void Remove(TItem item) => items.Remove(item);

        /// <summary>
        /// Removes all items from the <see cref="EventHandlerBase{TElement,TItem}"/>
        /// </summary>
        public void Clear() => items.Clear();

        /// <summary>
        /// Gets an executor that raises an event for the specified name of the element.
        /// </summary>
        /// <param name="elementName">The name of the element that has event handlers.</param>
        /// <returns><see cref="Executor"/> that raises an event.</returns>
        public Executor GetBy(string elementName) => new Executor(items.Where(item => item.Has(elementName)).ToList().AsReadOnly());

        /// <summary>
        /// Adds event handlers to the element.
        /// </summary>
        public void AddEventHandlers() => items.ForEach(item => item.AddEventHandler());

        /// <summary>
        /// Removes event handlers from the element.
        /// </summary>
        public void RemoveEventHandlers() => items.ForEach(item => item.RemoveEventHandler());

        /// <summary>
        /// Provides an event execution.
        /// </summary>
        public sealed class Executor
        {
            private readonly IEnumerable<TItem> items;

            private object sender;
            private object e;

            /// <summary>
            /// Initializes a new instance of the <see cref="Executor"/> class
            /// using the supplied items.
            /// </summary>
            /// <param name="items">The items that contain the context of the event handler.</param>
            /// <exception cref="ArgumentNullException">
            /// <paramref name="items"/> is <c>null</c>.
            /// </exception>
            public Executor(IEnumerable<TItem> items)
            {
                this.items = items ?? throw new ArgumentNullException(nameof(items));
            }

            /// <summary>
            /// Sets the object where the event handler is attached.
            /// </summary>
            /// <param name="sender">The object where the event handler is attached.</param>
            /// <returns>The instance of the <see cref="Executor"/> class.</returns>
            public Executor From(object sender)
            {
                this.sender = sender;
                return this;
            }

            /// <summary>
            /// Sets the event data.
            /// </summary>
            /// <param name="args">The event data.</param>
            /// <returns>The instance of the <see cref="Executor"/> class.</returns>
            public Executor With(object args)
            {
                e = args;
                return this;
            }

            /// <summary>
            /// Raises the event of the specified name.
            /// </summary>
            /// <param name="eventName">The name of the event to raise.</param>
            public void Raise(string eventName) => items.ForEach(item => item.Raise(eventName, sender, e));

            /// <summary>
            /// Raises the event of the specified name asynchronously.
            /// </summary>
            /// <param name="eventName">The name of the event to raise.</param>
            /// <returns>A task that represents the asynchronous raise operation.</returns>
            public async Task RaiseAsync(string eventName)
            {
                foreach (var item in items)
                {
                    await item.RaiseAsync(eventName, sender, e);
                }
            }
        }
    }
}
