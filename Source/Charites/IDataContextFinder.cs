// Copyright (C) 2018 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
namespace Charites.Windows.Mvc
{
    /// <summary>
    /// Provides the function to find a data context in a view.
    /// </summary>
    /// <typeparam name="TElement">The base type of a view that has a data context.</typeparam>
    public interface IDataContextFinder<in TElement> where TElement : class
    {
        /// <summary>
        /// Finds a data context in the specified view.
        /// </summary>
        /// <param name="view">The view that has a data context.</param>
        /// <returns>The data context in the specified view.</returns>
        object Find(TElement view);
    }
}
