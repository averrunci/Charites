// Copyright (C) 2022 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
namespace Charites.Windows.Mvc;

/// <summary>
/// Specifies the parameter to inject from the data context.
/// </summary>
[AttributeUsage(AttributeTargets.Parameter)]
public class FromDataContextAttribute : Attribute
{
}