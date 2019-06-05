// Copyright (C) 2019 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System;

namespace Charites.Windows.Mvc
{
    /// <summary>
    /// Specifies the parameter to inject from the DI container.
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter)]
    public class FromDIAttribute : Attribute
    {
    }
}
