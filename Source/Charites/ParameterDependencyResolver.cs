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
    private readonly IEnumerable<IEventHandlerParameterResolver> parameterResolver;
    private readonly IDictionary<Type, IDictionary<Type, Func<object?>>> preferredParameterResolver;

    /// <summary>
    /// Initializes a new instance of the <see cref="ParameterDependencyResolver"/> class.
    /// </summary>
    public ParameterDependencyResolver()
    {
        parameterResolver = Enumerable.Empty<IEventHandlerParameterResolver>();
        preferredParameterResolver = new Dictionary<Type, IDictionary<Type, Func<object?>>>();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ParameterDependencyResolver"/> class
    /// with the specified parameter resolver.
    /// </summary>
    /// <param name="parameterResolver">The resolver to resolve parameters.</param>
    public ParameterDependencyResolver(IEnumerable<IEventHandlerParameterResolver> parameterResolver) : this(parameterResolver, new Dictionary<Type, IDictionary<Type, Func<object?>>>())
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ParameterDependencyResolver"/> class
    /// with the specified parameter resolvers.
    /// </summary>
    /// <param name="parameterResolver">The resolver to resolve parameters.</param>
    /// <param name="preferredParameterResolver">The preferred resolver to resolve parameters.</param>
    public ParameterDependencyResolver(IEnumerable<IEventHandlerParameterResolver> parameterResolver, IDictionary<Type, IDictionary<Type, Func<object?>>> preferredParameterResolver)
    {
        this.parameterResolver = parameterResolver;
        this.preferredParameterResolver = preferredParameterResolver;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ParameterDependencyResolver"/> class
    /// with the specified resolver to resolver parameters from the dependency injection.
    /// </summary>
    /// <param name="dependencyInjectionResolver">The resolver to resolve parameters from the dependency injection.</param>
    [Obsolete("This constructor is obsolete. Use the .ctor(IEnumerable<IEventHandlerParameterResolver>, IDictionary<Type, IDictionary<Type, Func<object?>>>) instead.")]
    public ParameterDependencyResolver(IDictionary<Type, Func<object?>> dependencyInjectionResolver) : this(Enumerable.Empty<IEventHandlerParameterResolver>(), dependencyInjectionResolver)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ParameterDependencyResolver"/> class
    /// with the specified resolvers to resolver parameters.
    /// </summary>
    /// <param name="parameterResolver">The resolver to resolve parameters.</param>
    /// <param name="dependencyInjectionResolver">The resolver to resolve parameters from the dependency injection.</param>
    [Obsolete("This constructor is obsolete. Use the .ctor(IEnumerable<IEventHandlerParameterResolver>, IDictionary<Type, IDictionary<Type, Func<object?>>>) instead.")]
    public ParameterDependencyResolver(IEnumerable<IEventHandlerParameterResolver> parameterResolver, IDictionary<Type, Func<object?>> dependencyInjectionResolver)
    {
        this.parameterResolver = parameterResolver;
        preferredParameterResolver = new Dictionary<Type, IDictionary<Type, Func<object?>>> { [typeof(FromDIAttribute)] = dependencyInjectionResolver };
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
        var parameterCountExceptDependencyInjection = parameters.Count(p => p.GetCustomAttribute<Attribute>() is null);
        if (parameterCountExceptDependencyInjection > 2)
            throw new InvalidOperationException("The length of the method parameters except ones specified by an attribute must be less than 3.");

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
        return parameter.GetCustomAttribute<Attribute>() is null ? specificParameterQueue.Dequeue() : ResolveParameterFromDependency(parameter);
    }

    /// <summary>
    /// Resolves a parameter with the specified <see cref="ParameterInfo"/> using the dependency resolver.
    /// </summary>
    /// <param name="parameter">The <see cref="ParameterInfo"/> from which a parameter is resolved.</param>
    /// <returns>A parameter of the invoked method.</returns>
    protected virtual object? ResolveParameterFromDependency(ParameterInfo parameter)
    {
        return preferredParameterResolver
                .Where(resolver => parameter.GetCustomAttribute(resolver.Key) is not null)
                .Select(resolver => resolver.Value.ContainsKey(parameter.ParameterType) ? resolver.Value[parameter.ParameterType]() : null)
                .FirstOrDefault(parameterValue => parameterValue is not null) ??
            parameterResolver
                .Select(resolver => resolver.Resolve(parameter))
                .FirstOrDefault(parameterValue => parameterValue is not null);
    }

    object?[] IParameterDependencyResolver.Resolve(MethodInfo method, object? sender, object? e) => Resolve(method, sender, e);
}