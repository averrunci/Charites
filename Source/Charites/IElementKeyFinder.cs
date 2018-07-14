// Copyright (C) 2018 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
namespace Charites.Windows.Mvc
{
    /// <summary>
    /// Provides the function to find a key of an element.
    /// </summary>
    /// <typeparam name="TElement">The base type of a view in which the key is defined.</typeparam>
    public interface IElementKeyFinder<in TElement> where TElement : class
    {
        /// <summary>
        /// Finds the key of the specified element.
        /// </summary>
        /// <param name="element">The element in which the key is defined.</param>
        /// <returns>The key of the specified element.</returns>
        string FindKey(TElement element);
    }
}
