// Copyright (C) 2022 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System.Reflection;

namespace Charites.Windows.Mvc;

/// <summary>
/// Provides the function to resolve parameters of an event handler from the element.
/// </summary>
public abstract class EventHandlerParameterFromElementResolver : EventHandlerParameterResolver<FromElementAttribute>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EventHandlerParameterFromElementResolver"/> class
    /// with the specified element to which the controller that has the event handler whose parameter
    /// is resolved by this resolver is attached.
    /// </summary>
    /// <param name="associatedElement">
    /// The element to which the controller that has the event handler is attached whose parameter is resolved by this resolver is attached.
    /// </param>
    protected EventHandlerParameterFromElementResolver(object? associatedElement) : base(associatedElement)
    {
    }

    /// <summary>
    /// Finds an element of the specified name.
    /// </summary>
    /// <param name="name">The name of an element.</param>
    /// <returns>
    /// The element of the specified name.
    /// If not found, <c>null</c> is returned.
    /// </returns>
    protected abstract object? FindElement(string name);

    /// <summary>
    /// Resolves a parameter with the specified <see cref="ParameterInfo"/>.
    /// </summary>
    /// <param name="parameter">The <see cref="ParameterInfo"/> from which a parameter is resolved.</param>
    /// <returns>A parameter of the invoked event handler.</returns>
    protected override object? Resolve(ParameterInfo parameter)
        => FindElement(parameter.GetCustomAttribute<FromElementAttribute>()?.Name ?? parameter.Name ?? string.Empty);
}