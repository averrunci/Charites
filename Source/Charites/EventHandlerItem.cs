// Copyright (C) 2022 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
namespace Charites.Windows.Mvc;

/// <summary>
/// Represents an item of an event handler.
/// </summary>
/// <typeparam name="TElement">The base type of the view.</typeparam>
public abstract class EventHandlerItem<TElement> where TElement : class
{
    private readonly string elementName;
    private readonly TElement? element;
    private readonly string eventName;
    private readonly Delegate? handler;
    private readonly bool handledEventsToo;
    private readonly IEnumerable<IEventHandlerParameterResolver> parameterResolver;

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
    protected EventHandlerItem(string elementName, TElement? element, string eventName, Delegate? handler, bool handledEventsToo)
    {
        this.elementName = elementName;
        this.element = element;
        this.eventName = eventName;
        this.handler = handler;
        this.handledEventsToo = handledEventsToo;
        parameterResolver = Enumerable.Empty<IEventHandlerParameterResolver>();
    }

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
    /// <param name="parameterResolver">The resolver to resolve parameters.</param>
    protected EventHandlerItem(string elementName, TElement? element, string eventName, Delegate? handler, bool handledEventsToo, IEnumerable<IEventHandlerParameterResolver> parameterResolver)
    {
        this.elementName = elementName;
        this.element = element;
        this.eventName = eventName;
        this.handler = handler;
        this.handledEventsToo = handledEventsToo;
        this.parameterResolver = parameterResolver;
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
    public bool Has(string? elementName) => this.elementName == (elementName ?? string.Empty);

    /// <summary>
    /// Adds the event handler to the element.
    /// </summary>
    public void AddEventHandler()
    {
        if (element is null || handler is null) return;

        AddEventHandler(element, handler, handledEventsToo);
    }

    /// <summary>
    /// Removes the event handler from the element.
    /// </summary>
    public void RemoveEventHandler()
    {
        if (element is null || handler is null) return;

        RemoveEventHandler(element, handler);
    }

    /// <summary>
    /// Raises the event of the specified name.
    /// </summary>
    /// <param name="eventName">The name of the event to raise.</param>
    /// <param name="sender">The object where the event handler is attached.</param>
    /// <param name="e">The event data.</param>
    public void Raise(string eventName, object? sender, object? e)
    {
        if (this.eventName != eventName || handler is null) return;

        Handle(handler, sender, e);
    }

    /// <summary>
    /// Raises the event of the specified name.
    /// </summary>
    /// <param name="eventName">The name of the event to raise.</param>
    /// <param name="sender">the object where the event handler is attached.</param>
    /// <param name="e">The event data./</param>
    /// <param name="dependencyResolver">The resolver to resolve dependencies of parameters.</param>
    [Obsolete("This method is obsolete. Use the Raise(string, object?, object?, IDictionary<Type, IDictionary<Type, Func<object?>>>) method instead.")]
    public void Raise(string eventName, object? sender, object? e, IDictionary<Type, Func<object?>> dependencyResolver)
    {
        if (this.eventName != eventName || handler is null) return;

        Handle(handler, sender, e, dependencyResolver);
    }

    /// <summary>
    /// Raises the event of the specified name.
    /// </summary>
    /// <param name="eventName">The name of the event to raise.</param>
    /// <param name="sender">The object where the event handler is attached.</param>
    /// <param name="e">The event data.</param>
    /// <param name="preferredParameterResolver">The resolver to resolve parameters.</param>
    public void Raise(string eventName, object? sender, object? e, IDictionary<Type, IDictionary<Type, Func<object?>>> preferredParameterResolver)
    {
        if (this.eventName != eventName || handler is null) return;

        Handle(handler, sender, e, preferredParameterResolver);
    }

    /// <summary>
    /// Raises the event of the specified name asynchronously.
    /// </summary>
    /// <param name="eventName">The name of the event to raise.</param>
    /// <param name="sender">The object where the event handler is attached.</param>
    /// <param name="e">The event data.</param>
    /// <returns>A task that represents the asynchronous raise operation.</returns>
    public async Task RaiseAsync(string eventName, object? sender, object? e)
    {
        if (this.eventName != eventName || handler is null) return;

        if (Handle(handler, sender, e) is Task task)
        {
            await task;
        }
    }

    /// <summary>
    /// Raises the event of the specified name asynchronously.
    /// </summary>
    /// <param name="eventName">the name of the event to raise.</param>
    /// <param name="sender">The object where the event handler is attached.</param>
    /// <param name="e">The event data.</param>
    /// <param name="dependencyResolver">The resolver to resolve dependencies of parameters.</param>
    /// <returns>A task that represents the asynchronous raise operation.</returns>
    [Obsolete("This method is obsolete. Use the RaiseAsync(string, object?, object?, IDictionary<Type, IDictionary<Type, Func<object?>>>) method instead.")]
    public async Task RaiseAsync(string eventName, object? sender, object? e, IDictionary<Type, Func<object?>>? dependencyResolver)
    {
        if (this.eventName != eventName || handler is null) return;

        if (Handle(handler, sender, e, dependencyResolver) is Task task)
        {
            await task;
        }
    }

    /// <summary>
    /// Raises the event of the specified name asynchronously.
    /// </summary>
    /// <param name="eventName">The name of the event to raise.</param>
    /// <param name="sender">The object where the event handler is attached.</param>
    /// <param name="e">The event data.</param>
    /// <param name="preferredParameterResolver">The resolver to resolve parameters.</param>
    /// <returns>A task that represents the asynchronous raise operation.</returns>
    public async Task RaiseAsync(string eventName, object? sender, object? e, IDictionary<Type, IDictionary<Type, Func<object?>>> preferredParameterResolver)
    {
        if (this.eventName != eventName || handler is null) return;

        if (Handle(handler, sender, e, preferredParameterResolver) is Task task)
        {
            await task;
        }
    }

    /// <summary>
    /// Creates the resolver to resolve dependencies of parameters.
    /// </summary>
    /// <param name="dependencyResolver">The resolver to resolve dependencies of parameter.</param>
    /// <returns>The resolver to resolve dependencies of parameters.</returns>
    [Obsolete("This method is obsolete. Use the CreateParameterDependencyResolver(IDictionary<Type, IDictionary<Type, Func<object?>>>) method instead.")]
    protected virtual IParameterDependencyResolver CreateParameterDependencyResolver(IDictionary<Type, Func<object?>>? dependencyResolver)
    {
        return dependencyResolver is null ? new ParameterDependencyResolver(parameterResolver) : new ParameterDependencyResolver(parameterResolver, dependencyResolver);
    }

    /// <summary>
    /// Creates the resolver to resolve dependencies of parameters.
    /// </summary>
    /// <param name="preferredParameterResolver">The resolver to resolve parameters.</param>
    /// <returns>The resolver to resolve dependencies of parameters.</returns>
    protected virtual IParameterDependencyResolver CreateParameterDependencyResolver(IDictionary<Type, IDictionary<Type, Func<object?>>>? preferredParameterResolver)
    {
        return preferredParameterResolver is null ? new ParameterDependencyResolver(parameterResolver) : new ParameterDependencyResolver(parameterResolver, preferredParameterResolver);
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
    protected virtual object? Handle(Delegate handler, object? sender, object? e)
        => Handle(handler, sender, e, (IDictionary<Type, IDictionary<Type, Func<object?>>>?)null);

    /// <summary>
    /// Handles the event with the specified object where the event handler is attached,
    /// event data, and resolver to resolve dependencies of parameters.
    /// </summary>
    /// <param name="handler">The event handler to handle the event.</param>
    /// <param name="sender">The object where the event handler is attached.</param>
    /// <param name="e">The event data.</param>
    /// <param name="dependencyResolver">The resolver to resolve dependencies of parameters.</param>
    /// <returns>An object containing the return value of the handler method.</returns>
    [Obsolete("This method is obsolete. Use the Handle(Delegate, object?, object?, IDictionary<Type, IDictionary<Type, Func<object?>>>?) method instead.")]
    protected virtual object? Handle(Delegate handler, object? sender, object? e, IDictionary<Type, Func<object?>>? dependencyResolver)
        => handler.Target is EventHandlerAction eventHandlerAction ?
            eventHandlerAction.Handle(sender, e, dependencyResolver) :
            handler.DynamicInvoke(CreateParameterDependencyResolver(dependencyResolver).Resolve(handler.Method, sender, e));

    /// <summary>
    /// Handles the event with the specified object where the event handler is attached,
    /// event data, and resolver to resolve dependencies of parameters.
    /// </summary>
    /// <param name="handler">The event handler to handle the event.</param>
    /// <param name="sender">The object where the event handler is attached.</param>
    /// <param name="e">The event data.</param>
    /// <param name="preferredParameterResolver">The resolver to resolve parameters.</param>
    /// <returns>An object containing the return value of the handler method.</returns>
    protected virtual object? Handle(Delegate handler, object? sender, object? e, IDictionary<Type, IDictionary<Type, Func<object?>>>? preferredParameterResolver)
    {
        var parameterDependencyResolver = CreateParameterDependencyResolver(preferredParameterResolver);
        return handler.Target is EventHandlerAction eventHandlerAction ?
            eventHandlerAction.Handle(sender, e, parameterDependencyResolver) :
            handler.DynamicInvoke(parameterDependencyResolver.Resolve(handler.Method, sender, e));
    }
}