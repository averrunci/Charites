// Copyright (C) 2022 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
namespace Charites.Windows.Mvc;

/// <summary>
/// Provides the function that is added to a controller.
/// </summary>
/// <typeparam name="TElement">The base type of the view.</typeparam>
public interface IControllerExtension<in TElement> where TElement : class
{
    /// <summary>
    /// Attaches an extension that is defined in the specified controller to the specified element.
    /// </summary>
    /// <param name="controller">The controller in which an extension is defined.</param>
    /// <param name="element">The element to which an extension is attached.</param>
    void Attach(object controller, TElement element);

    /// <summary>
    /// Detaches an extension that is defined in the specified controller from the specified element.
    /// </summary>
    /// <param name="controller">The controller in which an extension is defined.</param>
    /// <param name="element">The element form which an extension is detached.</param>
    void Detach(object controller, TElement element);

    /// <summary>
    /// Retrieves a container of an extension that is defined in the specified controller.
    /// </summary>
    /// <param name="controller">The controller in which an extension is defined.</param>
    /// <returns>The container of the retrieved extension.</returns>
    object Retrieve(object controller);
}