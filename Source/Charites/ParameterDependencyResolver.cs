// Copyright (C) 2022 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System.Collections;
using System.Reflection;

namespace Charites.Windows.Mvc;

/// <summary>
/// Provides the function to resolve dependencies of parameters of the invoked method.
/// </summary>
public class ParameterDependencyResolver : IParameterDependencyResolver
{
    private readonly IDictionary<Type, Func<object?>>? dependencyResolver;

    /// <summary>
    /// Initializes a new instance of the <see cref="ParameterDependencyResolver"/> class.
    /// </summary>
    public ParameterDependencyResolver()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ParameterDependencyResolver"/> class
    /// with the specified resolver to resolver dependencies of parameters.
    /// </summary>
    /// <param name="dependencyResolver">The resolver to resolve dependencies of parameters.</param>
    public ParameterDependencyResolver(IDictionary<Type, Func<object?>> dependencyResolver)
    {
        this.dependencyResolver = dependencyResolver;
    }

    /// <summary>
    /// Resolves parameters with the specified method metadata, source of the event, and event data.
    /// </summary>
    /// <param name="method">The invoked method metadata.</param>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    /// <returns>Parameters of the invoked method.</returns>
    protected virtual object?[] Resolve(MethodInfo method, object? sender, object? e)
    {
        var parameters = method.GetParameters();
        var parameterCountExceptDependencyInjection = parameters.Count(p => p.GetCustomAttribute<FromDIAttribute>() is null);
        if (parameterCountExceptDependencyInjection > 2)
            throw new InvalidOperationException("The length of the method parameters except ones attributed by FromDIAttribute attribute must be less than 3.");

        var specificParameterQueue = new Queue();
        switch (parameterCountExceptDependencyInjection)
        {
            case 1:
                specificParameterQueue.Enqueue(e);
                break;
            case 2:
                specificParameterQueue.Enqueue(sender);
                specificParameterQueue.Enqueue(e);
                break;
        }
        return parameters.Select(parameter => ResolveParameter(parameter, specificParameterQueue)).ToArray();
    }

    /// <summary>
    /// Resolves a parameter with the specified <see cref="ParameterInfo"/> and queue that
    /// contains the source of the event and event data.
    /// </summary>
    /// <param name="parameter">The <see cref="ParameterInfo"/> from which a parameter is resolved.</param>
    /// <param name="specificParameterQueue">The queue that contains the source of the event and event data.</param>
    /// <returns>A parameter of the invoked method.</returns>
    protected virtual object? ResolveParameter(ParameterInfo parameter, Queue specificParameterQueue)
    {
        return parameter.GetCustomAttribute<FromDIAttribute>() is null ? specificParameterQueue.Dequeue() : ResolveParameterFromDependency(parameter);
    }

    /// <summary>
    /// Resolves a parameter with the specified <see cref="ParameterInfo"/> using the dependency resolver.
    /// </summary>
    /// <param name="parameter">The <see cref="ParameterInfo"/> from which a parameter is resolved.</param>
    /// <returns>A parameter of the invoked method.</returns>
    protected virtual object? ResolveParameterFromDependency(ParameterInfo parameter)
    {
        return dependencyResolver?.ContainsKey(parameter.ParameterType) ?? false ? dependencyResolver[parameter.ParameterType]() : null;
    }

    object?[] IParameterDependencyResolver.Resolve(MethodInfo method, object? sender, object? e) => Resolve(method, sender, e);
}