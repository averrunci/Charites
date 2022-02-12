// Copyright (C) 2022 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System.Reflection;

namespace Charites.Windows.Mvc;

/// <summary>
/// Provides the function to find a type of a controller that controls the view.
/// </summary>
/// <typeparam name="TElement">The base type of the element.</typeparam>
public abstract class ControllerTypeFinder<TElement> : IControllerTypeFinder<TElement> where TElement : class
{
    /// <summary>
    /// Gets a filter for the type of the view.
    /// </summary>
    protected virtual Func<Type?, TElement, bool> ViewTypeFilter { get; } = (viewType, view) => viewType?.IsInstanceOfType(view) ?? true;

    /// <summary>
    /// Gets a filter for the key of the view.
    /// </summary>
    protected virtual Func<string?, TElement, bool> KeyFilter { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ControllerTypeFinder{TElement}"/> class
    /// with the specified finder to find a data context in the view.
    /// </summary>
    /// <param name="elementKeyFinder">The finder to find a key of the view.</param>
    /// <param name="dataContextFinder">The finder to find a data context in the view.</param>
    protected ControllerTypeFinder(IElementKeyFinder<TElement> elementKeyFinder, IDataContextFinder<TElement> dataContextFinder)
    {
        KeyFilter = (key, element) =>
        {
            if (key is null) return true;

            var elementKey = elementKeyFinder.FindKey(element);
            return elementKey is null ? IsKeyDataContextType(key, dataContextFinder.Find(element)?.GetType()) : Equals(key, elementKey);
        };
    }

    /// <summary>
    /// Finds types of controllers that are candidates of the target controller types.
    /// </summary>
    /// <param name="view">The view that is controlled by controllers.</param>
    /// <returns>
    /// Types of controllers that are candidates of the target controller types.
    /// If not found, the empty enumerable is returned.
    /// </returns>
    protected abstract IEnumerable<Type> FindControllerTypeCandidates(TElement view);

    /// <summary>
    /// Finds types of controllers that control the specified view.
    /// </summary>
    /// <param name="view">The view that is controlled by controllers.</param>
    /// <returns>
    /// Types of controllers that control the specified view.
    /// If not found, the empty enumerable is returned.
    /// </returns>
    protected virtual IEnumerable<Type> FindControllerTypes(TElement view)
        => FindControllerTypeCandidates(view)
            .Select(t => new { Type = t, Attributes = t.GetTypeInfo().GetCustomAttributes<ViewAttribute>(true) })
            .Where(t => t.Attributes.Any(a => ViewTypeFilter(a.ViewType, view) && KeyFilter(a.Key, view)))
            .Select(t => t.Type)
            .ToList()
            .AsReadOnly();

    /// <summary>
    /// Gets the value that indicates whether the specified key is one that expresses the specified data context type.
    /// </summary>
    /// <param name="key">The key that expresses the specified data context type.</param>
    /// <param name="dataContextType">The type of the data context.</param>
    /// <returns>
    /// <c>true</c> if the specified key is an expression for the specified data context type; otherwise <c>false</c>.
    /// </returns>
    protected virtual bool IsKeyDataContextType(string key, Type? dataContextType)
        => dataContextType != null && (
            Equals(key, dataContextType.Name) ||
            Equals(key, dataContextType.ToString()) ||
            dataContextType.IsConstructedGenericType && Equals(key, dataContextType.GetFullNameWithoutParameters()) ||
            IsKeyDataContextType(key, dataContextType.BaseType) ||
            dataContextType.GetInterfaces().Any(i => IsKeyDataContextType(key, i)));

    IEnumerable<Type> IControllerTypeFinder<TElement>.Find(TElement view) => FindControllerTypes(view);
}