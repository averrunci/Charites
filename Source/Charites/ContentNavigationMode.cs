// Copyright (C) 2021 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
namespace Charites.Windows.Mvc
{
    /// <summary>
    /// Specifies the navigation stack characteristics of a navigation.
    /// </summary>
    public enum ContentNavigationMode
    {
        /// <summary>
        /// Navigation is to a new instance of a content (not going forward or backward in the stack).
        /// </summary>
        New,

        /// <summary>
        /// Navigation is going backward in the stack.
        /// </summary>
        Backward,

        /// <summary>
        /// Navigation is going forward in the stack.
        /// </summary>
        Forward
    }
}
