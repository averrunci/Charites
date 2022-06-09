// Copyright (C) 2022 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System.Reflection;

namespace Charites.Windows.Mvc;

/// <summary>
/// Provides the function to resolve parameters of an event handler.
/// </summary>
public abstract class EventHandlerParameterResolver<TAttribute> : IEventHandlerParameterResolver where TAttribute: Attribute
{
    /// <summary>
    /// Resolves a parameter with the specified <see cref="ParameterInfo"/>.
    /// </summary>
    /// <param name="parameter">The <see cref="ParameterInfo"/> from which a parameter is resolved.</param>
    /// <returns>A parameter of the invoked event handler.</returns>
    protected abstract object? Resolve(ParameterInfo parameter);

    object? IEventHandlerParameterResolver.Resolve(ParameterInfo parameter)
        => parameter.GetCustomAttribute<TAttribute>() is null ? null : Resolve(parameter);
}