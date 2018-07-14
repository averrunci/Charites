// Copyright (C) 2018 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System;
using System.Collections.Generic;

namespace Charites.Windows.Mvc
{
    internal static class Extensions
    {
        public static void ForEach<T>(this IEnumerable<T> @this, Action<T> action)
        {
            if (@this == null) return;

            foreach (var item in @this)
            {
                action(item);
            }
        }
    }
}
