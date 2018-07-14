// Copyright (C) 2018 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
namespace Charites.Windows.Mvc
{
    /// <summary>
    /// Provides the function to inject elements in a view to a controller.
    /// </summary>
    /// <typeparam name="TElement">The base type of the view that contains elements.</typeparam>
    public interface IElementInjector<in TElement> where TElement : class
    {
        /// <summary>
        /// Injects elements in the specified element to the specified controller.
        /// </summary>
        /// <param name="rootElement">The element that contains elements injected to the controller.</param>
        /// <param name="controller">The controller to inject elements.</param>
        /// <param name="foundElementOnly">
        /// If <c>true</c>, an element is not set to the controller when it is not found in the specified element;
        /// otherwise, <c>null</c> is set.
        /// </param>
        void Inject(TElement rootElement, object controller, bool foundElementOnly = false);
    }
}
