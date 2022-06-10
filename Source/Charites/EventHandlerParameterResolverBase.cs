// Copyright (C) 2022 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System.Reflection;

namespace Charites.Windows.Mvc;

/// <summary>
/// Represents the base of resolvers to resolve parameters of an event handler.
/// </summary>
public class EventHandlerParameterResolverBase : IEventHandlerParameterResolver
{
    /// <summary>
    /// Gets resolvers to resolve parameters an event handler.
    /// </summary>
    protected IDictionary<Type, ICollection<IEventHandlerParameterResolver>> Resolvers { get; } = new Dictionary<Type, ICollection<IEventHandlerParameterResolver>>();

    /// <summary>
    /// Initializes a new instance of the <see cref="EventHandlerParameterResolverBase"/> class.
    /// </summary>
    public EventHandlerParameterResolverBase()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="EventHandlerParameterResolverBase"/> class
    /// with the specified resolvers to resolve parameters of an event handler.
    /// </summary>
    /// <param name="resolvers">The resolvers to resolve parameters of an event handler.</param>
    public EventHandlerParameterResolverBase(params Tuple<Type, IEnumerable<IEventHandlerParameterResolver>>[] resolvers)
    {
        resolvers.ForEach(x=> x.Item2.ForEach(resolver => Add(x.Item1, resolver)));
    }

    /// <summary>
    /// Adds the specified resolver to resolve parameters specified by the attribute whose type is specified.
    /// </summary>
    /// <typeparam name="TAttribute">The type of the attribute that specifies to the parameter.</typeparam>
    /// <param name="resolver">The resolver to resolve parameters of an event handler.</param>
    public void Add<TAttribute>(IEventHandlerParameterResolver resolver) where TAttribute : Attribute
    {
        Add(typeof(TAttribute), resolver);
    }

    /// <summary>
    /// Adds the specified resolver to resolve parameters specified by the attribute whose type is specified.
    /// </summary>
    /// <param name="type">The type of the attribute that specifies to the parameter.</param>
    /// <param name="resolver">The resolver to resolve parameters of an event handler.</param>
    /// <exception cref="ArgumentException">
    /// If the <paramref name="type"/> is not derived from the <see cref="Attribute"/> class.
    /// </exception>
    public void Add(Type type, IEventHandlerParameterResolver resolver)
    {
        if (!typeof(Attribute).IsAssignableFrom(type)) throw new ArgumentException($"The type ({type}) must be derived from the Attribute class.", nameof(type));

        AddResolver(type, resolver);
    }

    /// <summary>
    /// Removes the specified resolver to resolve parameters specified by the attribute whose type is specified.
    /// </summary>
    /// <typeparam name="TAttribute">The type of the attribute that specifies to the parameter.</typeparam>
    /// <param name="resolver">The resolver to resolve parameters of an event handler.</param>
    public void Remove<TAttribute>(IEventHandlerParameterResolver resolver) where TAttribute : Attribute
    {
        Remove(typeof(TAttribute), resolver);
    }

    /// <summary>
    /// Removes the specified resolver to resolve parameters specified by the attribute whose type is specified.
    /// </summary>
    /// <param name="type">The type of the attribute that specifies to the parameter.</param>
    /// <param name="resolver">The resolver to resolve parameters of an event handler.</param>
    public void Remove(Type type, IEventHandlerParameterResolver resolver)
    {
        RemoveResolver(type, resolver);
    }

    /// <summary>
    /// Adds the specified resolver to resolve parameters specified by the attribute whose type is specified.
    /// </summary>
    /// <param name="type">The type of the attribute that specifies to the parameter.</param>
    /// <param name="resolver">The resolver to resolve parameters of an event handler.</param>
    protected virtual void AddResolver(Type type, IEventHandlerParameterResolver resolver)
    {
        if (!Resolvers.ContainsKey(type)) Resolvers[type] = new List<IEventHandlerParameterResolver>();

        Resolvers[type].Add(resolver);
    }

    /// <summary>
    /// Removes the specified resolver to resolve parameters specified by the attribute whose type is specified.
    /// </summary>
    /// <param name="type">The type of the attribute that specifies to the parameter.</param>
    /// <param name="resolver">The resolver to resolve parameters of an event handler.</param>
    protected virtual void RemoveResolver(Type type, IEventHandlerParameterResolver resolver)
    {
        if (!Resolvers.ContainsKey(type)) return;

        Resolvers[type].Remove(resolver);
    }

    /// <summary>
    /// Resolves a parameter with the specified <see cref="ParameterInfo"/>.
    /// </summary>
    /// <param name="parameter">The <see cref="ParameterInfo"/> from which a parameter is resolved.</param>
    /// <returns>A parameter of the invoked event handler.</returns>
    protected virtual object? Resolve(ParameterInfo parameter)
        => Resolvers.Where(resolver => parameter.GetCustomAttribute(resolver.Key) is not null)
            .SelectMany(resolver => resolver.Value)
            .Select(resolver => resolver.Resolve(parameter))
            .FirstOrDefault(parameterValue => parameterValue is not null);

    object? IEventHandlerParameterResolver.Resolve(ParameterInfo parameter) => Resolve(parameter);
}