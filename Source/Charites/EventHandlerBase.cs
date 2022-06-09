// Copyright (C) 2022 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System.Collections.ObjectModel;

namespace Charites.Windows.Mvc;

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
    public Executor GetBy(string? elementName) => new(items.Where(item => item.Has(elementName)).ToList().AsReadOnly());

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

        private object? sender;
        private object? e;

        private readonly IDictionary<Type, IDictionary<Type, Func<object?>>> parameterResolver = new Dictionary<Type, IDictionary<Type, Func<object?>>>();

        /// <summary>
        /// Initializes a new instance of the <see cref="Executor"/> class
        /// using the supplied items.
        /// </summary>
        /// <param name="items">The items that contain the context of the event handler.</param>
        public Executor(IEnumerable<TItem> items)
        {
            this.items = items;
        }

        /// <summary>
        /// Sets the object where the event handler is attached.
        /// </summary>
        /// <param name="sender">The object where the event handler is attached.</param>
        /// <returns>The instance of the <see cref="Executor"/> class.</returns>
        public Executor From(object? sender)
        {
            this.sender = sender;
            return this;
        }

        /// <summary>
        /// Sets the event data.
        /// </summary>
        /// <param name="args">The event data.</param>
        /// <returns>The instance of the <see cref="Executor"/> class.</returns>
        public Executor With(object? args)
        {
            e = args;
            return this;
        }

        /// <summary>
        /// Resolves a parameter of the specified type using the specified resolver.
        /// </summary>
        /// <typeparam name="T">The type of the parameter to inject to.</typeparam>
        /// <param name="resolver">The function to resolve the parameter of the specified type.</param>
        /// <returns>The instance of the <see cref="Executor"/></returns>
        [Obsolete("This method is obsolete. Use the Resolve<TAttribute, TParameter>(Func<object?>) method instead.")]
        public Executor Resolve<T>(Func<object?> resolver)
        {
            return Resolve<FromDIAttribute, T>(resolver);
        }

        /// <summary>
        /// Resolves a parameter of the specified attribute type and parameter type using the specified resolver.
        /// </summary>
        /// <typeparam name="TAttribute">The type of the attribute that specifies the parameter.</typeparam>
        /// <typeparam name="TParameter">The type of the parameter that is resolved by the specified resolver.</typeparam>
        /// <param name="resolver">The function to resolve the parameter of the specified type.</param>
        /// <returns>The instance of the <see cref="Executor"/>.</returns>
        public Executor Resolve<TAttribute, TParameter>(Func<object?> resolver) where TAttribute : Attribute
        {
            if (!parameterResolver.ContainsKey(typeof(TAttribute)))
            {
                parameterResolver[typeof(TAttribute)] = new Dictionary<Type, Func<object?>>();
            }
            parameterResolver[typeof(TAttribute)][typeof(TParameter)] = resolver;
            return this;
        }

        /// <summary>
        /// Raises the event of the specified name.
        /// </summary>
        /// <param name="eventName">The name of the event to raise.</param>
        public void Raise(string eventName) => items.ForEach(item => item.Raise(eventName, sender, e, parameterResolver));

        /// <summary>
        /// Raises the event of the specified name asynchronously.
        /// </summary>
        /// <param name="eventName">The name of the event to raise.</param>
        /// <returns>A task that represents the asynchronous raise operation.</returns>
        public async Task RaiseAsync(string eventName)
        {
            foreach (var item in items)
            {
                await item.RaiseAsync(eventName, sender, e, parameterResolver);
            }
        }
    }
}