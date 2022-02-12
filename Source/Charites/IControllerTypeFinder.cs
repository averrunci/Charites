// Copyright (C) 2022 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
namespace Charites.Windows.Mvc;

/// <summary>
/// Provides the function to find a type of a controller that controls the view.
/// </summary>
/// <typeparam name="TElement">The base type of the view.</typeparam>
public interface IControllerTypeFinder<in TElement> where TElement : class
{
    /// <summary>
    /// Finds types of controllers that control the specified view.
    /// </summary>
    /// <param name="view">The view that is controlled by controllers.</param>
    /// <returns>
    /// Types of controllers that control the specified view.
    /// If not found, the empty enumerable is returned.
    /// </returns>
    IEnumerable<Type> Find(TElement view);
}