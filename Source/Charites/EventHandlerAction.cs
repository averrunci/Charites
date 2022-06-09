// Copyright (C) 2022 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System.Reflection;

namespace Charites.Windows.Mvc;

/// <summary>
/// Represents the action of an event handler.
/// </summary>
public class EventHandlerAction
{
    private readonly MethodInfo method;
    private readonly object? target;
    private readonly IParameterDependencyResolver parameterDependencyResolver;

    /// <summary>
    /// Initializes a new instance of the <see cref="EventHandlerAction"/> class
    /// with the specified method to handle an event and target to invoke it.
    /// </summary>
    /// <param name="method">The method to handle an event.</param>
    /// <param name="target">The target object to invoke the method to handle an event.</param>
    public EventHandlerAction(MethodInfo method, object? target) : this(method, target, new ParameterDependencyResolver())
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="EventHandlerAction"/> class
    /// with the specified method to handle an event, target to invoke it, and
    /// resolver to resolve parameters of the invoked method.
    /// </summary>
    /// <param name="method">The method to handle an event.</param>
    /// <param name="target">The target object to invoke the method to handle an event.</param>
    /// <param name="parameterDependencyResolver">The resolver to resolve parameters of the invoked method.</param>
    public EventHandlerAction(MethodInfo method, object? target, IParameterDependencyResolver parameterDependencyResolver)
    {
        this.method = method;
        this.target = target;
        this.parameterDependencyResolver = parameterDependencyResolver;
    }

    /// <summary>
    /// Handles the event with the specified source of the event and event data.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    public void OnHandled(object? sender, object? e) => Handle(sender, e);

    /// <summary>
    /// Handles the event with the specified source of the event and event data.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    /// <returns>An object containing the return value of the invoked method.</returns>
    public object? Handle(object? sender, object? e)
    {
        return Handle(sender, e, parameterDependencyResolver);
    }

    /// <summary>
    /// Handles the event with the specified source of the event, event data, and
    /// resolver to resolve dependencies of parameters.
    /// </summary>
    /// <param name="sender">The source of teh event.</param>
    /// <param name="e">The event data.</param>
    /// <param name="dependencyResolver">The resolver to resolve dependencies of parameters.</param>
    /// <returns>An object containing the return value of the invoked method.</returns>
    [Obsolete("This method is obsolete. Use the Handle(object?, object? IParameterDependencyResolver) method instead.")]
    public object? Handle(object? sender, object? e, IDictionary<Type, Func<object?>>? dependencyResolver)
    {
        return Handle(CreateParameterDependencyResolver(dependencyResolver).Resolve(method, sender, e));
    }

    /// <summary>
    /// Handles the event with the specified source of the event, event data, and
    /// resolver to resolve dependencies of parameters.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    /// <param name="parameterDependency">The resolver to resolve dependencies of parameters.</param>
    /// <returns>An object containing the return value of the invoked method.</returns>
    public object? Handle(object? sender, object? e, IParameterDependencyResolver parameterDependency)
    {
        return Handle(parameterDependency.Resolve(method, sender, e));
    }

    /// <summary>
    /// Handles the event with the specified parameters.
    /// </summary>
    /// <param name="parameters">The parameters to invoke method.</param>
    /// <returns>An object containing the return value of the invoked method.</returns>
    protected virtual object? Handle(object?[] parameters)
    {
        try
        {
            var returnValue = method.Invoke(target, parameters);
            if (returnValue is Task task) Await(task);
            return returnValue;
        }
        catch (Exception exc)
        {
            if (!HandleUnhandledException(exc)) throw;

            return null;
        }
    }

    /// <summary>
    /// Creates the resolver to resolve dependencies of parameters.
    /// </summary>
    /// <param name="dependencyResolver">The resolver to resolve dependencies of parameter.</param>
    /// <returns>The resolver to resolve dependencies of parameters.</returns>
    [Obsolete("This method is obsolete. The IParameterDependencyResolver is specified by a constructor.")]
    protected virtual IParameterDependencyResolver CreateParameterDependencyResolver(IDictionary<Type, Func<object?>>? dependencyResolver)
    {
        return dependencyResolver is null ? new ParameterDependencyResolver() : new ParameterDependencyResolver(dependencyResolver);
    }

    private async void Await(Task task)
    {
        try
        {
            await task;
        }
        catch (Exception exc)
        {
            if (!HandleUnhandledException(exc)) throw;
        }
    }

    /// <summary>
    /// Handles an unhandled exception that occurred when the action of the event handler is performed.
    /// </summary>
    /// <param name="exc">
    /// An unhandled exception that occurred when the action of the event handler is performed.
    /// </param>
    /// <returns>
    /// <c>true</c> if the exception is handled; otherwise, <c>false</c>.
    /// </returns>
    protected virtual bool HandleUnhandledException(Exception exc) => false;
}