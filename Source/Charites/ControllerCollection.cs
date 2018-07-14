// Copyright (C) 2018 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Charites.Windows.Mvc
{
    /// <summary>
    /// Represents a collection of controller objects.
    /// </summary>
    /// <typeparam name="TElement">The base type of the view.</typeparam>
    public abstract class ControllerCollection<TElement> : Collection<object> where TElement : class
    {
        private readonly IDataContextFinder<TElement> dataContextFinder;
        private readonly IDataContextInjector dataContextInjector;
        private readonly IElementInjector<TElement> elementInjector;
        private readonly IEnumerable<IControllerExtension<TElement>> extensions;

        private TElement associatedElement;
        private bool isAssociatedElementEventsSubscribed;

        /// <summary>
        /// Initializes a new instance of the <see cref="ControllerCollection{TElement}"/> class
        /// with the specified <see cref="IDataContextFinder{TElement}"/>, <see cref="IDataContextInjector"/>,
        /// <see cref="IElementInjector{TElement}"/>, and the enumerable of the <see cref="IControllerExtension{TElement}"/>.
        /// </summary>
        /// <param name="dataContextFinder">The finder to find a data context.</param>
        /// <param name="dataContextInjector">The injector to inject a data context.</param>
        /// <param name="elementInjector">The injector to inject elements.</param>
        /// <param name="extensions">The extensions for a controller.</param>
        protected ControllerCollection(IDataContextFinder<TElement> dataContextFinder, IDataContextInjector dataContextInjector, IElementInjector<TElement> elementInjector, IEnumerable<IControllerExtension<TElement>> extensions)
        {
            this.dataContextFinder = dataContextFinder;
            this.dataContextInjector = dataContextInjector;
            this.elementInjector = elementInjector;
            this.extensions = extensions;
        }

        /// <summary>
        /// Attaches controllers to the specified element.
        /// </summary>
        /// <param name="element">The element to which controllers are attached.</param>
        public void AttachTo(TElement element)
        {
            if (Equals(element, associatedElement)) return;
            if (associatedElement != null) throw new InvalidOperationException("Associated element must be null.");

            associatedElement = element;
            this.ForEach(AttachToAssociatedElement);
        }

        /// <summary>
        /// Detaches controllers from the element to which controllers are attached.
        /// </summary>
        public void Detach()
        {
            if (associatedElement == null) return;

            this.ForEach(DetachFromAssociatedElement);
            associatedElement = null;
        }

        /// <summary>
        /// Sets the specified data context to controllers.
        /// </summary>
        /// <param name="dataContext">The data context to set to controllers.</param>
        public void SetDataContext(object dataContext) => this.ForEach(controller => SetDataContext(dataContext, controller));

        /// <summary>
        /// Sets the specified element to controllers.
        /// </summary>
        /// <param name="element">The element to set to controllers.</param>
        /// <param name="foundElementOnly">
        /// If <c>true</c>, an element is not set to the controller when it is not found in the specified element;
        /// otherwise, <c>null</c> is set.
        /// </param>
        public void SetElement(TElement element, bool foundElementOnly = false) => this.ForEach(controller => SetElement(element, controller, foundElementOnly));

        /// <summary>
        /// Removes all controllers of the <see cref="ControllerCollection{TElement}"/>.
        /// </summary>
        protected override void ClearItems()
        {
            Detach();

            base.ClearItems();
        }

        /// <summary>
        /// Inserts the specified controller into the <see cref="ControllerCollection{TElement}"/> at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which <paramref name="item"/> should be inserted.</param>
        /// <param name="item">The controller to insert.</param>
        protected override void InsertItem(int index, object item)
        {
            base.InsertItem(index, item);

            AttachToAssociatedElement(item);
        }

        /// <summary>
        /// Removes the controller at the specified index of the <see cref="ControllerCollection{TElement}"/>.
        /// </summary>
        /// <param name="index">The zero-based index of the controller to remove.</param>
        protected override void RemoveItem(int index)
        {
            DetachFromAssociatedElement(this[index]);

            base.RemoveItem(index);
        }

        /// <summary>
        /// Gets the value that indicates whether the element to which controllers are attached is loaded.
        /// </summary>
        /// <param name="associatedElement">The element to which controllers are attached.</param>
        /// <returns>
        /// <c>true</c> if the element to which controllers are attached is loaded;
        /// otherwise, <c>false</c> is returned.
        /// </returns>
        protected abstract bool IsAssociatedElementLoaded(TElement associatedElement);

        /// <summary>
        /// Subscribes events of the element to which controllers are attached.
        /// </summary>
        /// <param name="associatedElement">The element to which controllers are attached.</param>
        protected abstract void SubscribeAssociatedElementEvents(TElement associatedElement);

        /// <summary>
        /// Unsubscribes events of the element to which controllers are attached.
        /// </summary>
        /// <param name="associatedElement">The element to which controllers are attached.</param>
        protected abstract void UnsubscribeAssociatedElementEvents(TElement associatedElement);

        /// <summary>
        /// Sets the specified data context to the specified controller.
        /// </summary>
        /// <param name="dataContext">The data context to set to the controller.</param>
        /// <param name="controller">The controller to which the data context is set.</param>
        protected virtual void SetDataContext(object dataContext, object controller) => dataContextInjector?.Inject(dataContext, controller);

        /// <summary>
        /// Sets the specified element to the specified controller.
        /// </summary>
        /// <param name="element">The element to set to the controller.</param>
        /// <param name="controller">The controller to which the element is set.</param>
        /// <param name="foundElementOnly">
        /// If <c>true</c>, an element is not set to the controller when it is not found in the specified element;
        /// otherwise, <c>null</c> is set.
        /// </param>
        protected virtual void SetElement(TElement element, object controller, bool foundElementOnly = false) => elementInjector?.Inject(element, controller, foundElementOnly);

        /// <summary>
        /// Attaches extensions that are defined in controllers.
        /// </summary>
        protected void AttachExtensions() => this.ForEach(AttachExtensions);

        /// <summary>
        /// Detaches extensions that are defined in controllers.
        /// </summary>
        protected void DetachExtensions() => this.ForEach(DetachExtensions);

        /// <summary>
        /// Attaches extensions that are defined in the specified controller.
        /// </summary>
        /// <param name="controller">The controller in which extensions are defined.</param>
        protected void AttachExtensions(object controller) => extensions.ForEach(extension => AttachExtension(extension, controller, associatedElement));

        /// <summary>
        /// Detaches extensions that are defined in the specified controller.
        /// </summary>
        /// <param name="controller">The controller in which extensions are defined.</param>
        protected void DetachExtensions(object controller) => extensions.ForEach(extension => DetachExtension(extension, controller, associatedElement));

        /// <summary>
        /// Attaches the specified extension that is defined in the specified controller to the specified element.
        /// </summary>
        /// <param name="extension">The extension that is defined in the controller.</param>
        /// <param name="controller">The controller in which the extension is defined.</param>
        /// <param name="associatedElement">The element to which the extension is attached.</param>
        protected virtual void AttachExtension(IControllerExtension<TElement> extension, object controller, TElement associatedElement) => extension.Attach(controller, associatedElement);

        /// <summary>
        /// Detaches the specified extension that is defined in the specified controller from the specified element.
        /// </summary>
        /// <param name="extension">The extension that is defined in the controller.</param>
        /// <param name="controller">The controller in which the extension is defined.</param>
        /// <param name="associatedElement">The element from which the extension is detached.</param>
        protected virtual void DetachExtension(IControllerExtension<TElement> extension, object controller, TElement associatedElement) => extension.Detach(controller, associatedElement);

        private void AttachToAssociatedElement(object controller)
        {
            if (controller == null || associatedElement == null) return;

            if (!isAssociatedElementEventsSubscribed)
            {
                SubscribeAssociatedElementEvents(associatedElement);
                isAssociatedElementEventsSubscribed = true;
            }

            SetDataContext(dataContextFinder.Find(associatedElement), controller);
            if (!IsAssociatedElementLoaded(associatedElement)) return;

            SetElement(associatedElement, controller);
            AttachExtensions(controller);
        }

        private void DetachFromAssociatedElement(object controller)
        {
            if (controller == null || associatedElement == null) return;

            if (isAssociatedElementEventsSubscribed)
            {
                UnsubscribeAssociatedElementEvents(associatedElement);
                isAssociatedElementEventsSubscribed = false;
            }

            SetDataContext(null, controller);
            SetElement(null, controller);
            DetachExtensions(controller);

            (controller as IDisposable)?.Dispose();
        }
    }
}
