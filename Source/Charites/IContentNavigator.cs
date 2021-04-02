// Copyright (C) 2021 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System;
using System.Collections.Generic;

namespace Charites.Windows.Mvc
{
    /// <summary>
    /// Provides the function to navigate a content.
    /// </summary>
    public interface IContentNavigator
    {
        /// <summary>
        /// Occurs when a navigation state is changed.
        /// </summary>
        event EventHandler NavigationStateChanged;

        /// <summary>
        /// Occurs when a new navigation is requested.
        /// </summary>
        event EventHandler<ContentNavigatingEventArgs> Navigating;

        /// <summary>
        /// Occurs when the content has been navigated and the navigation history has been stacked.
        /// </summary>
        event EventHandler<ContentNavigatedEventArgs> Navigated;

        /// <summary>
        /// Gets a value that indicates whether there is at least one entry in backward navigation history.
        /// </summary>
        bool CanGoBackward { get; }

        /// <summary>
        /// Gets a value that indicates whether there is at least one entry in forward navigation history.
        /// </summary>
        bool CanGoForward { get; }

        /// <summary>
        /// Gets or sets a value that indicates whether the navigation is recorded in the ForwardStack or BackwardStack.
        /// </summary>
        bool IsNavigationStackEnabled { get; set; }

        /// <summary>
        /// Gets a content that is currently active.
        /// </summary>
        object CurrentContent { get; }

        /// <summary>
        /// Gets a collection representing the backward navigation history.
        /// </summary>
        Stack<object> BackwardStack { get; }

        /// <summary>
        /// Gets a collection representing the forward navigation history.
        /// </summary>
        Stack<object> ForwardStack { get; }

        /// <summary>
        /// Navigates to the specified content.
        /// </summary>
        /// <param name="content">The content to navigate.</param>
        /// <returns>
        /// <c>false</c> if the specified content is null, its type is the same type of the current content,
        /// or the navigation is canceled; otherwise, <c>true</c>.
        /// </returns>
        bool NavigateTo(object content);

        /// <summary>
        /// Navigates to the most recent item in backward navigation history.
        /// </summary>
        /// <returns>
        /// <c>false</c> if the backward navigation history is empty or the navigation is canceled; otherwise, <c>true</c>.
        /// </returns>
        bool GoBackward();

        /// <summary>
        /// Navigates to the most recent item in forward navigation history.
        /// </summary>
        /// <returns>
        /// <c>false</c> if the forward navigation history is empty or the navigation is canceled; otherwise, <c>true</c>.
        /// </returns>
        bool GoForward();
    }
}
