// Copyright (C) 2022 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
namespace Charites.Windows.Mvc;

/// <summary>
/// Provides the data for the <see cref="IContentNavigator.Navigating"/> event.
/// </summary>
public class ContentNavigatingEventArgs : EventArgs
{
    /// <summary>
    /// Gets or sets a value that indicates whether a pending navigation should be canceled.
    /// </summary>
    public bool Cancel { get; set; }

    /// <summary>
    /// Gets a value that indicates the direction of movement during navigation.
    /// </summary>
    public ContentNavigationMode NavigationMode { get; }

    /// <summary>
    /// Gets the navigated content.
    /// </summary>
    public object Content { get; }

    /// <summary>
    /// Gets the source content.
    /// </summary>
    public object SourceContent { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ContentNavigatingEventArgs"/> class
    /// with the specified navigation mode, navigated content, and source content.
    /// </summary>
    /// <param name="navigationMode">The navigation mode.</param>
    /// <param name="content">The navigated content.</param>
    /// <param name="sourceContent">The source content.</param>
    public ContentNavigatingEventArgs(ContentNavigationMode navigationMode, object content, object sourceContent)
    {
        NavigationMode = navigationMode;
        Content = content;
        SourceContent = sourceContent;
    }
}