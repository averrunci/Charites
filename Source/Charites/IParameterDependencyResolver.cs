﻿// Copyright (C) 2022 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System.Reflection;

namespace Charites.Windows.Mvc;

/// <summary>
/// Provides the function to resolve dependencies of parameters of the invoked method.
/// </summary>
public interface IParameterDependencyResolver
{
    /// <summary>
    /// Resolves parameters with the specified method metadata, source of the event, and event data.
    /// </summary>
    /// <param name="method">The invoked method metadata.</param>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    /// <returns>Parameters of the invoked method.</returns>
    object?[] Resolve(MethodInfo method, object? sender, object? e);
}