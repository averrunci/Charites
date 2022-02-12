// Copyright (C) 2022 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
namespace Charites.Windows.Mvc;

/// <summary>
/// Provides the function to navigate a content.
/// </summary>
public class ContentNavigator : IContentNavigator
{
    /// <summary>
    /// Occurs when a navigation state is changed.
    /// </summary>
    public event EventHandler? NavigationStateChanged;

    /// <summary>
    /// Occurs when a new navigation is requested.
    /// </summary>
    public event EventHandler<ContentNavigatingEventArgs>? Navigating;

    /// <summary>
    /// Occurs when the content has been navigated and the navigation history has been stacked.
    /// </summary>
    public event EventHandler<ContentNavigatedEventArgs>? Navigated;

    /// <summary>
    /// Gets an initial navigation content.
    /// </summary>
    public static object InitialContent { get; } = new();

    /// <summary>
    /// Gets a value that indicates whether there is at least one entry in backward navigation history.
    /// </summary>
    protected virtual bool CanGoBackward => BackwardStack.Any();

    /// <summary>
    /// Gets a value that indicates whether there is at least one entry in forward navigation history.
    /// </summary>
    protected virtual bool CanGoForward => ForwardStack.Any();

    /// <summary>
    /// Gets or sets a value that indicates whether the navigation is recorded in the ForwardStack or BackwardStack.
    /// </summary>
    protected virtual bool IsNavigationStackEnabled { get; set; } = true;

    /// <summary>
    /// Gets or sets a content that is currently active.
    /// </summary>
    protected virtual object CurrentContent { get; set; } = InitialContent;

    /// <summary>
    /// Gets a collection representing the backward navigation history.
    /// </summary>
    protected virtual Stack<object> BackwardStack { get; } = new();

    /// <summary>
    /// Gets a collection representing the forward navigation history.
    /// </summary>
    protected virtual Stack<object> ForwardStack { get; } = new();

    /// <summary>
    /// Gets a value that indicates whether to be able to navigate to the specified content.
    /// </summary>
    /// <param name="content">The content to navigate.</param>
    /// <returns>
    /// <c>true</c> if the specified content is not the <see cref="InitialContent"/> and
    /// its type is different from the current content type; otherwise, <c>false</c>.
    /// </returns>
    protected virtual bool CanNavigateTo(object content) => content != InitialContent && content.GetType() != CurrentContent.GetType();

    /// <summary>
    /// Navigates to the specified content.
    /// </summary>
    /// <param name="content">The content to navigate.</param>
    /// <returns>
    /// <c>false</c> if the specified content is the <see cref="InitialContent"/>, its type is the same type of the current content,
    /// or the navigation is canceled; otherwise, <c>true</c>.
    /// </returns>
    protected virtual bool NavigateTo(object content)
        => CanNavigateTo(content) && Navigate(ContentNavigationMode.New, content, sourceContent =>
        {
            ForwardStack.Clear();
            BackwardStack.Push(sourceContent);
        });

    /// <summary>
    /// Navigates to the most recent item in backward navigation history.
    /// </summary>
    /// <returns>
    /// <c>false</c> if the backward navigation history is empty or the navigation is canceled; otherwise, <c>true</c>.
    /// </returns>
    protected virtual bool GoBackward()
        => CanGoBackward && Navigate(ContentNavigationMode.Backward, BackwardStack.Peek(), sourceContent =>
        {
            BackwardStack.Pop();
            ForwardStack.Push(sourceContent);
        });

    /// <summary>
    /// Navigates to the most recent item in forward navigation history.
    /// </summary>
    /// <returns>
    /// <c>false</c> if the forward navigation history is empty or the navigation is canceled; otherwise, <c>true</c>.
    /// </returns>
    protected virtual bool GoForward()
        => CanGoForward && Navigate(ContentNavigationMode.Forward, ForwardStack.Peek(), sourceContent =>
        {
            ForwardStack.Pop();
            BackwardStack.Push(sourceContent);
        });

    /// <summary>
    /// Performs the navigation with the specified navigation context.
    /// </summary>
    /// <param name="mode">The navigation mode.</param>
    /// <param name="content">The content to navigate.</param>
    /// <param name="navigationAction">The navigation action.</param>
    /// <returns>
    /// <c>false</c> if the navigation is canceled; otherwise <c>true</c>.
    /// </returns>
    protected virtual bool Navigate(ContentNavigationMode mode, object content, Action<object> navigationAction)
    {
        var sourceContent = CurrentContent;
        var navigatingEventArgs = new ContentNavigatingEventArgs(mode, content, sourceContent);
        OnNavigating(navigatingEventArgs);
        if (navigatingEventArgs.Cancel) return false;

        if (sourceContent != InitialContent && IsNavigationStackEnabled) navigationAction(sourceContent);
        CurrentContent = content;
        OnNavigationStateChanged(EventArgs.Empty);
        OnNavigated(new ContentNavigatedEventArgs(mode, content, sourceContent));

        return true;
    }

    /// <summary>
    /// Raises the <see cref="NavigationStateChanged"/> event with the specified event data.
    /// </summary>
    /// <param name="e">The event data.</param>
    protected virtual void OnNavigationStateChanged(EventArgs e) => NavigationStateChanged?.Invoke(this, e);

    /// <summary>
    /// Raises the <see cref="Navigating"/> event with the specified event data.
    /// </summary>
    /// <param name="e">The event data.</param>
    protected virtual void OnNavigating(ContentNavigatingEventArgs e) => Navigating?.Invoke(this, e);

    /// <summary>
    /// Raises the <see cref="Navigated"/> event with the specified event data.
    /// </summary>
    /// <param name="e">The event data.</param>
    protected virtual void OnNavigated(ContentNavigatedEventArgs e) => Navigated?.Invoke(this, e);

    bool IContentNavigator.CanGoBackward => CanGoBackward;
    bool IContentNavigator.CanGoForward => CanGoForward;
    bool IContentNavigator.IsNavigationStackEnabled
    {
        get => IsNavigationStackEnabled;
        set => IsNavigationStackEnabled = value;
    }

    object IContentNavigator.CurrentContent => CurrentContent;
    Stack<object> IContentNavigator.BackwardStack => BackwardStack;
    Stack<object> IContentNavigator.ForwardStack => ForwardStack;

    bool IContentNavigator.NavigateTo(object content) => NavigateTo(content);
    bool IContentNavigator.GoBackward() => GoBackward();
    bool IContentNavigator.GoForward() => GoForward();
}