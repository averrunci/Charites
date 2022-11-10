// Copyright (C) 2022 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
namespace Charites.Windows.Mvc;

/// <summary>
/// Represents a base class for a controller associated with a view that has a data context of the specified type.
/// </summary>
/// <typeparam name="TDataContext">The type of a data context.</typeparam>
public class ControllerBase<TDataContext> where TDataContext : class
{
    /// <summary>
    /// Gets or sets a data context.
    /// </summary>
    protected TDataContext? DataContext { get; set; }

    /// <summary>
    /// Sets the specified data context.
    /// </summary>
    /// <param name="dataContext">The data context to set.</param>
    protected virtual void SetDataContext(TDataContext? dataContext)
    {
        if (DataContext == dataContext) return;

        DataContext.IfPresent(UnsubscribeFromEvents);
        DataContext = dataContext;
        DataContext.IfPresent(SubscribeToEvents);
    }

    /// <summary>
    /// Subscribes to events of the specified data context.
    /// </summary>
    /// <param name="dataContext">The data context that has events to subscribe to.</param>
    protected virtual void SubscribeToEvents(TDataContext dataContext)
    {
    }

    /// <summary>
    /// Unsubscribes from events of the specified data context.
    /// </summary>
    /// <param name="dataContext">The data context that has events to unsubscribe from.</param>
    protected virtual void UnsubscribeFromEvents(TDataContext dataContext)
    {
    }
}