// Copyright (C) 2022 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
namespace Charites.Windows.Mvc;

internal static class Extensions
{
    public static void ForEach<T>(this IEnumerable<T>? @this, Action<T> action)
    {
        if (@this is null) return;

        foreach (var item in @this)
        {
            action(item);
        }
    }

    public static string GetFullNameWithoutParameters(this Type @this)
    {
        var dataTypeFullName = @this.ToString();
        var parameterStartIndex = dataTypeFullName.IndexOf('[');
        return parameterStartIndex < 0 ? dataTypeFullName : dataTypeFullName[..parameterStartIndex];
    }
}