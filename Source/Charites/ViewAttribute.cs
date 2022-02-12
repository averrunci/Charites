// Copyright (C) 2022 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
namespace Charites.Windows.Mvc;

/// <summary>
/// Specifies the controller that controls the target view.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class ViewAttribute : Attribute
{
    /// <summary>
    /// Gets or sets the type of the view.
    /// </summary>
    public Type? ViewType { get; set; }

    /// <summary>
    /// Gets or sets the key of the view.
    /// </summary>
    public string? Key { get; set; }
}