﻿// Copyright (C) 2018 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System;
using System.Linq;
using System.Reflection;

namespace Charites.Windows.Mvc
{
    /// <summary>
    /// Provides the function to inject elements in a view to a controller.
    /// </summary>
    /// <typeparam name="TElement">The base type of the view that contains elements</typeparam>
    public abstract class ElementInjector<TElement> : IElementInjector<TElement> where TElement : class
    {
        /// <summary>
        /// Gets the <see cref="BindingFlags"/> for an element.
        /// </summary>
        protected virtual BindingFlags ElementBindingFlags { get; } = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;

        /// <summary>
        /// Finds an element of the specified name in the specified element.
        /// </summary>
        /// <param name="rootElement">The element that contains an element of the specified name.</param>
        /// <param name="elementName">The name of an element.</param>
        /// <returns>
        /// The element of the specified name in the specified element.
        /// If not found, <c>null</c> is returned.
        /// </returns>
        protected abstract object FindElement(TElement rootElement, string elementName);

        /// <summary>
        /// Injects elements in the specified element to fields of the specified controller.
        /// </summary>
        /// <param name="rootElement">The element that contains elements injected to the controller.</param>
        /// <param name="controller">The controller to inject elements.</param>
        /// <param name="foundElementOnly">
        /// If <c>true</c>, an element is not set to the controller when it is not found in the specified element;
        /// otherwise, <c>null</c> is set.
        /// </param>
        protected virtual void InjectElementToField(TElement rootElement, object controller, bool foundElementOnly)
            => controller.GetType()
                .GetFields(ElementBindingFlags)
                .Select(field => new { Field = field, Attribute = field.GetCustomAttribute<ElementAttribute>(true) })
                .Where(t => t.Attribute != null)
                .ForEach(t => InjectElement(rootElement, t.Attribute.Name ?? t.Field.Name, controller, foundElementOnly, element => t.Field.SetValue(controller, element)));

        /// <summary>
        /// Injects elements in the specified element to properties of the specified controller.
        /// </summary>
        /// <param name="rootElement">The element that contains elements injected to the controller.</param>
        /// <param name="controller">The controller to inject elements.</param>
        /// <param name="foundElementOnly">
        /// If <c>true</c>, an element is not set to the controller when it is not found in the specified element;
        /// otherwise, <c>null</c> is set.
        /// </param>
        protected virtual void InjectElementToProperty(TElement rootElement, object controller, bool foundElementOnly)
            => controller.GetType()
                .GetProperties(ElementBindingFlags)
                .Select(property => new{ Property = property, Attribute = property.GetCustomAttribute<ElementAttribute>(true) })
                .Where(t => t.Attribute != null)
                .ForEach(t => InjectElement(rootElement, t.Attribute.Name ?? t.Property.Name, controller, foundElementOnly, element => t.Property.SetValue(controller, element)));

        /// <summary>
        /// Injects elements in the specified element to methods of the specified controller.
        /// </summary>
        /// <param name="rootElement">The element that contains elements injected to the controller.</param>
        /// <param name="controller">The controller to inject elements.</param>
        /// <param name="foundElementOnly">
        /// If <c>true</c>, an element is not set to the controller when it is not found in the specified element;
        /// otherwise, <c>null</c> is set.
        /// </param>
        protected virtual void InjectElementToMethod(TElement rootElement, object controller, bool foundElementOnly)
            => controller.GetType()
                .GetMethods(ElementBindingFlags)
                .Select(method => new { Method = method, Attribute = method.GetCustomAttribute<ElementAttribute>(true) })
                .Where(t => t.Attribute != null)
                .ForEach(t => InjectElement(rootElement, t.Attribute.Name ?? ResolveElementMethodName(t.Method, t.Attribute), controller, foundElementOnly, element => t.Method.Invoke(controller, new[] { element })));

        /// <summary>
        /// Resolves an element name from a method to get it.
        /// </summary>
        /// <param name="method">The <see cref="MethodInfo"/> of a method to get an element.</param>
        /// <param name="attribute">The attribute of an element.</param>
        /// <returns>
        /// The element name that is resolved from the specified method to get the element.
        /// </returns>
        protected virtual string ResolveElementMethodName(MethodInfo method, ElementAttribute attribute)
            => attribute.Name ?? (method.Name.StartsWith("Set") ? method.Name.Substring(3) : method.Name);

        /// <summary>
        /// Injects the element of the specified name using the specified action.
        /// </summary>
        /// <param name="rootElement">The element that contains elements injected to the controller.</param>
        /// <param name="elementName">The name of the element.</param>
        /// <param name="controller">The controller to inject elements.</param>
        /// <param name="foundElementOnly">
        /// If <c>true</c>, an element is not set to the controller when it is not found in the specified element;
        /// otherwise, <c>null</c> is set.
        /// </param>
        /// <param name="action">The action that injects the element.</param>
        protected void InjectElement(TElement rootElement, string elementName, object controller, bool foundElementOnly, Action<object> action)
        {
            try
            {
                var element = FindElement(rootElement, elementName);
                if (foundElementOnly && element == null) return;

                action(element);
            }
            catch (Exception exc)
            {
                throw new ElementInjectionException($"The injection of {elementName} to {controller.GetType()} is failed.", exc);
            }
        }

        void IElementInjector<TElement>.Inject(TElement rootElement, object controller, bool foundElementOnly)
        {
            if (controller == null) return;

            InjectElementToField(rootElement, controller, foundElementOnly);
            InjectElementToProperty(rootElement, controller, foundElementOnly);
            InjectElementToMethod(rootElement, controller, foundElementOnly);
        }
    }
}
