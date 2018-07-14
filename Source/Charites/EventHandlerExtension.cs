// Copyright (C) 2018 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Charites.Windows.Mvc
{
    /// <summary>
    /// Represents an extension to handle an event.
    /// </summary>
    /// <typeparam name="TElement">The base type of the view.</typeparam>
    /// <typeparam name="TItem">The type of the item of the event handler.</typeparam>
    public abstract class EventHandlerExtension<TElement, TItem> : IControllerExtension<TElement> where TElement : class where TItem : EventHandlerItem<TElement>
    {
        /// <summary>
        /// Gets the <see cref="BindingFlags"/> for an event handler.
        /// </summary>
        protected virtual BindingFlags EventHandlerBindingFlags { get; } = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;

        /// <summary>
        /// Ensures <see cref="EventHandlerBase{TElement,TItem}"/> associated with the specified element.
        /// </summary>
        /// <param name="element">The element that associates <see cref="EventHandlerBase{TElement,TItem}"/>.</param>
        /// <returns>
        /// If <see cref="EventHandlerBase{TElement,TItem}"/> associated with the specified element exists, it is returned;
        /// otherwise, a new instance of the <see cref="EventHandlerBase{TElement,TItem}"/> is returned.
        /// </returns>
        protected abstract IDictionary<object, EventHandlerBase<TElement, TItem>> EnsureEventHandlerBases(TElement element);

        /// <summary>
        /// Adds an event handler to the specified <see cref="EventHandlerBase{TElement,TItem}"/> using the specified element,
        /// <see cref="EventHandlerAttribute"/>, and the function to create a delegate of the event handler.
        /// </summary>
        /// <param name="element">The element that raises the event.</param>
        /// <param name="eventHandlerAttribute">The <see cref="EventHandlerAttribute"/> specified to the method that handles the event.</param>
        /// <param name="handlerCreator">The function to create a delegate of the event handler.</param>
        /// <param name="eventHandlers">The <see cref="EventHandlerBase{TElement,TItem}"/> to add the event handler.</param>
        protected abstract void AddEventHandler(TElement element, EventHandlerAttribute eventHandlerAttribute, Func<Type, Delegate> handlerCreator, EventHandlerBase<TElement, TItem> eventHandlers);

        /// <summary>
        /// Adds event handlers to the specified element.
        /// </summary>
        /// <param name="controller">The controller in which event handlers are defined.</param>
        /// <param name="element">The element to which event handlers are added.</param>
        protected virtual void Attach(object controller, TElement element)
        {
            var eventHandlers = RetrieveEventHandlers(controller, element);
            eventHandlers.AddEventHandlers();

            OnEventHandlerAdded(eventHandlers, element);
        }

        /// <summary>
        /// Removes event handlers from the specified element.
        /// </summary>
        /// <param name="controller">The controller in which event handlers are defined.</param>
        /// <param name="element">The element form which event handlers are removed.</param>
        protected virtual void Detach(object controller, TElement element)
        {
            var eventHandlers = RetrieveEventHandlers(controller, element);
            eventHandlers.RemoveEventHandlers();
            eventHandlers.Clear();

            OnEventHandlerRemoved(eventHandlers, element);
        }

        /// <summary>
        /// Retrieves <see cref="EventHandlerBase{TElement,TItem}"/> that contains event handlers.
        /// </summary>
        /// <param name="controller">The controller in which event handlers are defined.</param>
        /// <returns>The <see cref="EventHandlerBase{TElement,TItem}"/> that contains event handlers.</returns>
        protected virtual object Retrieve(object controller)
            => controller == null ? new EventHandlerBase<TElement, TItem>() : RetrieveEventHandlers(controller, null);

        /// <summary>
        /// Retrieves event handlers from the specified controller.
        /// </summary>
        /// <param name="controller">The controller in which event handlers are defined.</param>
        /// <param name="element">The element that raises the event.</param>
        /// <returns>The <see cref="EventHandlerBase{TElement,TItem}"/> to add retrieved event handlers.</returns>
        protected virtual EventHandlerBase<TElement, TItem> RetrieveEventHandlers(object controller, TElement element)
        {
            var eventHandlerBases = EnsureEventHandlerBases(element);
            if (eventHandlerBases.ContainsKey(controller)) return eventHandlerBases[controller];

            var eventHandlers = new EventHandlerBase<TElement, TItem>();
            eventHandlerBases[controller] = eventHandlers;

            RetrieveEventHandlersFromField(controller, element, eventHandlers);
            RetrieveEventHandlersFromProperty(controller, element, eventHandlers);
            RetrieveEventHandlersFromMethod(controller, element, eventHandlers);

            return eventHandlers;
        }

       /// <summary>
       /// Retrieves event handlers from fields that are defined in the specified controller.
       /// </summary>
       /// <param name="controller">The controller in which event handlers are defined.</param>
       /// <param name="element">The element that raises the event.</param>
       /// <param name="eventHandlers">The <see cref="EventHandlerBase{TElement,TItem}"/> to add retrieved event handlers.</param>
        protected virtual void RetrieveEventHandlersFromField(object controller, TElement element, EventHandlerBase<TElement, TItem> eventHandlers)
            => controller.GetType()
                .GetFields(EventHandlerBindingFlags)
                .Where(field => field.GetCustomAttributes<EventHandlerAttribute>(true).Any())
                .ForEach(field => AddEventHandlers(field, element, handlerType => CreateEventHandler(field.GetValue(controller) as Delegate, handlerType), eventHandlers));

        /// <summary>
        /// Retrieves event handlers from properties that are defined in the specified controller.
        /// </summary>
        /// <param name="controller">The controller in which event handlers are defined.</param>
        /// <param name="element">The element that raises the event.</param>
        /// <param name="eventHandlers">The <see cref="EventHandlerBase{TElement,TItem}"/> to add retrieved event handlers.</param>
        protected virtual void RetrieveEventHandlersFromProperty(object controller, TElement element, EventHandlerBase<TElement, TItem> eventHandlers)
            => controller.GetType()
                .GetProperties(EventHandlerBindingFlags)
                .Where(property => property.GetCustomAttributes<EventHandlerAttribute>(true).Any())
                .Where(property => property.CanRead)
                .ForEach(property => AddEventHandlers(property, element, handlerType => CreateEventHandler(property.GetValue(controller) as Delegate, handlerType), eventHandlers));

        /// <summary>
        /// Retrieves event handlers from methods that are defined in the specified controller.
        /// </summary>
        /// <param name="controller">The controller in which event handlers are defined.</param>
        /// <param name="element">The element that raises the event.</param>
        /// <param name="eventHandlers">The <see cref="EventHandlerBase{TElement,TItem}"/> to add retrieved event handlers.</param>
        protected virtual void RetrieveEventHandlersFromMethod(object controller, TElement element, EventHandlerBase<TElement, TItem> eventHandlers)
            => controller.GetType()
                .GetMethods(EventHandlerBindingFlags)
                .Where(method => method.GetCustomAttributes<EventHandlerAttribute>(true).Any())
                .ForEach(method => AddEventHandlers(method, element, handlerType => CreateEventHandler(method, controller, handlerType), eventHandlers));

        /// <summary>
        /// Adds event handlers that are defined in the specified <see cref="MemberInfo"/> to the specified <see cref="EventHandlerBase{TElement,TItem}"/>
        /// using the specified element and the function to create an event handler.
        /// </summary>
        /// <param name="member">The <see cref="MethodInfo"/> that defines event handlers.</param>
        /// <param name="element">The element that raises the event.</param>
        /// <param name="handlerCreator">The function to create a delegate of the event handler.</param>
        /// <param name="eventHandlers">The <see cref="EventHandlerBase{TElement,TItem}"/> to add event handlers.</param>
        protected virtual void AddEventHandlers(MemberInfo member, TElement element, Func<Type, Delegate> handlerCreator, EventHandlerBase<TElement, TItem> eventHandlers)
        {
            if (eventHandlers == null) return;

            member.GetCustomAttributes<EventHandlerAttribute>(true)
                .ForEach(eventHandlerAttribute => AddEventHandler(element, eventHandlerAttribute, handlerCreator, eventHandlers));
        }

        /// <summary>
        /// Creates an event handler with the specified delegate and type of the handler.
        /// </summary>
        /// <param name="delegate">The delegate of the method to handle the event.</param>
        /// <param name="handlerType">The type of the event handler.</param>
        /// <returns>The delegate to handle the event.</returns>
        protected Delegate CreateEventHandler(Delegate @delegate, Type handlerType) => @delegate == null ? null : CreateEventHandler(@delegate.Method, @delegate.Target, handlerType);

        /// <summary>
        /// Creates an event handler with the specified method, target, and type of the handler.
        /// </summary>
        /// <param name="method">The method to handle the event.</param>
        /// <param name="target">The target object to invoke the method to handle the event.</param>
        /// <param name="handlerType">The type of the event handler.</param>
        /// <returns>The delegate to handle the event.</returns>
        protected virtual Delegate CreateEventHandler(MethodInfo method, object target, Type handlerType)
        {
            if (method == null) return null;

            var action = new EventHandlerAction(method, target);
            return action.GetType()
                .GetMethod(nameof(EventHandlerAction.OnHandled))
                ?.CreateDelegate(handlerType ?? typeof(Handler), action);
        }

        /// <summary>
        /// Handles the process when event handlers are attached.
        /// </summary>
        /// <param name="eventHandlers">The <see cref="EventHandlerBase{TElement,TItem}"/> in which event handlers are added.</param>
        /// <param name="element">The element that raises the event.</param>
        protected virtual void OnEventHandlerAdded(EventHandlerBase<TElement, TItem> eventHandlers, TElement element)
        {
        }

        /// <summary>
        /// Handles the process when event handlers are detached.
        /// </summary>
        /// <param name="eventHandlers">The <see cref="EventHandlerBase{TElement,TItem}"/> in which event handlers are removed.</param>
        /// <param name="element">The element that raises the event.</param>
        protected virtual void OnEventHandlerRemoved(EventHandlerBase<TElement, TItem> eventHandlers, TElement element)
        {
        }

        private delegate void Handler(object sender, object e);

        void IControllerExtension<TElement>.Attach(object controller, TElement element)
        {
            if (controller == null || element == null) return;

            Attach(controller, element);
        }

        void IControllerExtension<TElement>.Detach(object controller, TElement element)
        {
            if (controller == null || element == null) return;

            Detach(controller, element);
        }

        object IControllerExtension<TElement>.Retrieve(object controller) => Retrieve(controller);
    }
}
