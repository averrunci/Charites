// Copyright (C) 2022 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System.Reflection;

namespace Charites.Windows.Mvc;

/// <summary>
/// Provides the function to resolve parameters of an event handler from the dependency injection.
/// </summary>
public abstract class EventHandlerParameterFromDIResolver : EventHandlerParameterResolver<FromDIAttribute>
{
    /// <summary>
    /// Creates a parameter of the specified parameterType
    /// </summary>
    /// <param name="parameterType">The type of the parameter.</param>
    /// <returns>The new instance of a parameter of the specified type.</returns>
    protected abstract object? CreateParameter(Type parameterType);

    /// <summary>
    /// Resolves a parameter with the specified <see cref="ParameterInfo"/>.
    /// </summary>
    /// <param name="parameter">The <see cref="ParameterInfo"/> from which a parameter is resolved.</param>
    /// <returns>A parameter of the invoked event handler.</returns>
    protected override object? Resolve(ParameterInfo parameter) => CreateParameter(parameter.ParameterType);
}