// Copyright (C) 2022 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
namespace Charites.Windows.Mvc;

/// <summary>
/// Provides the function to find an element in a view.
/// </summary>
/// <typeparam name="TElement">The base type of the view that contains elements.</typeparam>
public interface IElementFinder<in TElement> where TElement : class
{
    /// <summary>
    /// Finds an element of the specified name in the specified element.
    /// </summary>
    /// <param name="rootElement">The element that contains an element of the specified name.</param>
    /// <param name="elementName">The name of an element.</param>
    /// <returns>
    /// The element of the specified name in the specified element.
    /// If not found, <c>null</c> is returned.
    /// </returns>
    object? FindElement(TElement? rootElement, string elementName);
}