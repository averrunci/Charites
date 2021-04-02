// Copyright (C) 2021 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System;

namespace Charites.Windows.Mvc
{
    /// <summary>
    /// Provides the data for the <see cref="IContentNavigator.Navigated"/> event.
    /// </summary>
    public class ContentNavigatedEventArgs : EventArgs
    {
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
        /// Initializes a new instance of the <see cref="ContentNavigatedEventArgs"/> class
        /// with the specified navigation mode, content, and source content.
        /// </summary>
        /// <param name="navigationMode">The navigation mode.</param>
        /// <param name="content">The navigated content.</param>
        /// <param name="sourceContent">The source content.</param>
        public ContentNavigatedEventArgs(ContentNavigationMode navigationMode, object content, object sourceContent)
        {
            NavigationMode = navigationMode;
            Content = content;
            SourceContent = sourceContent;
        }
    }
}
