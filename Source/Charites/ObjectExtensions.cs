// Copyright (C) 2022 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
namespace Charites.Windows.Mvc;

/// <summary>
/// Provides utility methods for an object.
/// </summary>
public static class ObjectExtensions
{
    /// <summary>
    /// Invokes the specified action with the specified parameter if it is present.
    /// </summary>
    /// <typeparam name="T">The type of the parameter with which the action is invoked.</typeparam>
    /// <param name="this">The parameter with which the action is invoked.</param>
    /// <param name="action">The action invoked with the specified parameter.</param>
    public static void IfPresent<T>(this T? @this, Action<T> action) { if (@this is not null) action(@this); }

    /// <summary>
    /// Invokes the specified action with the specified two parameters if they are present.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter with which the action is invoked.</typeparam>
    /// <typeparam name="T2">The type of the second parameter with which the action is invoked.</typeparam>
    /// <param name="this">The first parameter with which the action is invoked.</param>
    /// <param name="arg">The second parameter with which the action is invoked.</param>
    /// <param name="action">The action invoked with the specified two parameters.</param>
    public static void IfPresent<T1, T2>(this T1? @this, T2? arg, Action<T1, T2> action) { if (@this is not null && arg is not null) action(@this, arg); }

    /// <summary>
    /// Invokes the specified action with the specified three parameters if they are present.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter with which the action is invoked.</typeparam>
    /// <typeparam name="T2">The type of the second parameter with which the action is invoked.</typeparam>
    /// <typeparam name="T3">The type of the third parameter with which the action is invoked.</typeparam>
    /// <param name="this">The first parameter with which the action is invoked.</param>
    /// <param name="arg1">The second parameter with which the action is invoked.</param>
    /// <param name="arg2">The third parameter with which the action is invoked.</param>
    /// <param name="action">The action invoked with the specified three parameters.</param>
    public static void IfPresent<T1, T2, T3>(this T1? @this, T2? arg1, T3? arg2, Action<T1, T2, T3> action) { if (@this is not null && arg1 is not null && arg2 is not null) action(@this, arg1, arg2); }

    /// <summary>
    /// Invokes the specified action with the specified four parameters if they are present.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter with which the action is invoked.</typeparam>
    /// <typeparam name="T2">The type of the second parameter with which the action is invoked.</typeparam>
    /// <typeparam name="T3">The type of the third parameter with which the action is invoked.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter with which the action is invoked.</typeparam>
    /// <param name="this">The first parameter with which the action is invoked.</param>
    /// <param name="arg1">The second parameter with which the action is invoked.</param>
    /// <param name="arg2">The third parameter with which the action is invoked.</param>
    /// <param name="arg3">The fourth parameter with which the action is invoked.</param>
    /// <param name="action">The action invoked with the specified four parameters.</param>
    public static void IfPresent<T1, T2, T3, T4>(this T1? @this, T2? arg1, T3? arg2, T4? arg3, Action<T1, T2, T3, T4> action) { if (@this is not null && arg1 is not null && arg2 is not null && arg3 is not null) action(@this, arg1, arg2, arg3); }

    /// <summary>
    /// Invokes the specified action with the specified five parameters if they are present.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter with which the action is invoked.</typeparam>
    /// <typeparam name="T2">The type of the second parameter with which the action is invoked.</typeparam>
    /// <typeparam name="T3">The type of the third parameter with which the action is invoked.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter with which the action is invoked.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter with which the action is invoked.</typeparam>
    /// <param name="this">The first parameter with which the action is invoked.</param>
    /// <param name="arg1">The second parameter with which the action is invoked.</param>
    /// <param name="arg2">The third parameter with which the action is invoked.</param>
    /// <param name="arg3">the fourth parameter with which the action is invoked.</param>
    /// <param name="arg4">The fifth parameter with which the action is invoked.</param>
    /// <param name="action">The action invoked with the specified five parameters.</param>
    public static void IfPresent<T1, T2, T3, T4, T5>(this T1? @this, T2? arg1, T3? arg2, T4? arg3, T5? arg4, Action<T1, T2, T3, T4, T5> action) { if (@this is not null && arg1 is not null && arg2 is not null && arg3 is not null && arg4 is not null) action(@this, arg1, arg2, arg3, arg4); }

    /// <summary>
    /// Invokes the specified action with the specified six parameters if they are present.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter with which the action is invoked.</typeparam>
    /// <typeparam name="T2">The type of the second parameter with which the action is invoked.</typeparam>
    /// <typeparam name="T3">The type of the third parameter with which the action is invoked.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter with which the action is invoked.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter with which the action is invoked.</typeparam>
    /// <typeparam name="T6">The type of the sixth parameter with which the action is invoked.</typeparam>
    /// <param name="this">The first parameter with which the action is invoked.</param>
    /// <param name="arg1">The second parameter with which the action is invoked.</param>
    /// <param name="arg2">The third parameter with which the action is invoked.</param>
    /// <param name="arg3">The fourth parameter with which the action is invoked.</param>
    /// <param name="arg4">The fifth parameter with which the action is invoked.</param>
    /// <param name="arg5">The sixth parameter with which the action is invoked.</param>
    /// <param name="action">The action invoked with the specified six parameters.</param>
    public static void IfPresent<T1, T2, T3, T4, T5, T6>(this T1? @this, T2? arg1, T3? arg2, T4? arg3, T5? arg4, T6? arg5, Action<T1, T2, T3, T4, T5, T6> action) { if (@this is not null && arg1 is not null && arg2 is not null && arg3 is not null && arg4 is not null && arg5 is not null) action(@this, arg1, arg2, arg3, arg4, arg5); }

    /// <summary>
    /// Invokes the specified action with the specified seven parameters if they are present.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter with which the action is invoked.</typeparam>
    /// <typeparam name="T2">The type of the second parameter with which the action is invoked.</typeparam>
    /// <typeparam name="T3">The type of the third parameter with which the action is invoked.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter with which the action is invoked.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter with which the action is invoked.</typeparam>
    /// <typeparam name="T6">The type of the sixth parameter with which the action is invoked.</typeparam>
    /// <typeparam name="T7">The type of the seventh parameter with which the action is invoked.</typeparam>
    /// <param name="this">The first parameter with which the action is invoked.</param>
    /// <param name="arg1">The second parameter with which the action is invoked.</param>
    /// <param name="arg2">The third parameter with which the action is invoked.</param>
    /// <param name="arg3">The fourth parameter with which the action is invoked.</param>
    /// <param name="arg4">The fifth parameter with which the action is invoked.</param>
    /// <param name="arg5">The sixth parameter with which the action is invoked.</param>
    /// <param name="arg6">The seventh parameter with which the action is invoked.</param>
    /// <param name="action">The action invoked with the specified seven parameters.</param>
    public static void IfPresent<T1, T2, T3, T4, T5, T6, T7>(this T1? @this, T2? arg1, T3? arg2, T4? arg3, T5? arg4, T6? arg5, T7? arg6, Action<T1, T2, T3, T4, T5, T6, T7> action) { if (@this is not null && arg1 is not null && arg2 is not null && arg3 is not null && arg4 is not null && arg5 is not null && arg6 is not null) action(@this, arg1, arg2, arg3, arg4, arg5, arg6); }

    /// <summary>
    /// Invokes the specified action with the specified eight parameters if the are present.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter with which the action is invoked.</typeparam>
    /// <typeparam name="T2">The type of the second parameter with which the action is invoked.</typeparam>
    /// <typeparam name="T3">The type of the third parameter with which the action is invoked.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter with which the action is invoked.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter with which the action is invoked.</typeparam>
    /// <typeparam name="T6">The type of the sixth parameter with which the action is invoked.</typeparam>
    /// <typeparam name="T7">The type of the seventh parameter with which the action is invoked.</typeparam>
    /// <typeparam name="T8">The type of the eighth parameter with which the action is invoked.</typeparam>
    /// <param name="this">The first parameter with which the action is invoked.</param>
    /// <param name="arg1">The second parameter with which the action is invoked.</param>
    /// <param name="arg2">The third parameter with which the action is invoked.</param>
    /// <param name="arg3">The fourth parameter with which the action is invoked.</param>
    /// <param name="arg4">The fifth parameter with which the action is invoked.</param>
    /// <param name="arg5">The sixth parameter with which the action is invoked.</param>
    /// <param name="arg6">The seventh parameter with which the action is invoked.</param>
    /// <param name="arg7">The eighth parameter with which the action is invoked.</param>
    /// <param name="action">The action invoked with the specified eight parameters.</param>
    public static void IfPresent<T1, T2, T3, T4, T5, T6, T7, T8>(this T1? @this, T2? arg1, T3? arg2, T4? arg3, T5? arg4, T6? arg5, T7? arg6, T8? arg7, Action<T1, T2, T3, T4, T5, T6, T7, T8> action) { if (@this is not null && arg1 is not null && arg2 is not null && arg3 is not null && arg4 is not null && arg5 is not null && arg6 is not null && arg7 is not null) action(@this, arg1, arg2, arg3, arg4, arg5, arg6, arg7); }

    /// <summary>
    /// Invokes the specified asynchronous action with the specified parameter if it is present.
    /// </summary>
    /// <typeparam name="T">The type of the first parameter with which the asynchronous action is invoked.</typeparam>
    /// <param name="this">The first parameter with which the asynchronous action is invoked.</param>
    /// <param name="action">The asynchronous action invoked with the specified parameter.</param>
    /// <returns>A task that represents the asynchronous action.</returns>
    public static Task IfPresentAsync<T>(this T? @this, Func<T, Task> action) => @this is null ? Task.CompletedTask : action(@this);

    /// <summary>
    /// Invokes the specified asynchronous action with the specified two parameters if they are present.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter with which the asynchronous action is invoked.</typeparam>
    /// <typeparam name="T2">The type of the second parameter with which the asynchronous action is invoked.</typeparam>
    /// <param name="this">The first parameter with which the asynchronous action is invoked.</param>
    /// <param name="arg">The second parameter with which the asynchronous action is invoked.</param>
    /// <param name="action">The asynchronous action invoked with the specified two parameters.</param>
    /// <returns>A task that represents the asynchronous action.</returns>
    public static Task IfPresentAsync<T1, T2>(this T1? @this, T2? arg, Func<T1, T2, Task> action) => @this is null || arg is null ? Task.CompletedTask : action(@this, arg);

    /// <summary>
    /// Invokes the specified asynchronous action with the specified three parameters if they are present.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter with which the asynchronous action is invoked.</typeparam>
    /// <typeparam name="T2">The type of the second parameter with which the asynchronous action is invoked.</typeparam>
    /// <typeparam name="T3">The type of the third parameter with which the asynchronous action is invoked.</typeparam>
    /// <param name="this">The first parameter with which the asynchronous action is invoked.</param>
    /// <param name="arg1">The second parameter with which the asynchronous action is invoked.</param>
    /// <param name="arg2">The third parameter with which the asynchronous action is invoked.</param>
    /// <param name="action">The asynchronous action invoked with the specified three parameters.</param>
    /// <returns>A task that represents the asynchronous action.</returns>
    public static Task IfPresentAsync<T1, T2, T3>(this T1? @this, T2? arg1, T3? arg2, Func<T1, T2, T3, Task> action) => @this is null || arg1 is null || arg2 is null ? Task.CompletedTask : action(@this, arg1, arg2);

    /// <summary>
    /// Invokes the specified asynchronous action with the specified four parameters if they are present.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter with which the asynchronous action is invoked.</typeparam>
    /// <typeparam name="T2">The type of the second parameter with which the asynchronous action is invoked.</typeparam>
    /// <typeparam name="T3">The type of the third parameter with which the asynchronous action is invoked.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter with which the asynchronous action is invoked.</typeparam>
    /// <param name="this">The first parameter with which the asynchronous action is invoked.</param>
    /// <param name="arg1">The second parameter with which the asynchronous action is invoked.</param>
    /// <param name="arg2">The third parameter with which the asynchronous action is invoked.</param>
    /// <param name="arg3">The fourth parameter with which the asynchronous action is invoked.</param>
    /// <param name="action">The asynchronous action invoked with the specified four parameters.</param>
    /// <returns>A task that represents the asynchronous action.</returns>
    public static Task IfPresentAsync<T1, T2, T3, T4>(this T1? @this, T2? arg1, T3? arg2, T4? arg3, Func<T1, T2, T3, T4, Task> action) => @this is null || arg1 is null || arg2 is null || arg3 is null ? Task.CompletedTask : action(@this, arg1, arg2, arg3);

    /// <summary>
    /// Invokes the specified asynchronous action with the specified five parameters if they are present.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter with which the asynchronous action is invoked.</typeparam>
    /// <typeparam name="T2">The type of the second parameter with which the asynchronous action is invoked.</typeparam>
    /// <typeparam name="T3">The type of the third parameter with which the asynchronous action is invoked.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter with which the asynchronous action is invoked.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter with which the asynchronous action is invoked.</typeparam>
    /// <param name="this">The first parameter with which the asynchronous action is invoked.</param>
    /// <param name="arg1">The second parameter with which the asynchronous action is invoked.</param>
    /// <param name="arg2">The third parameter with which the asynchronous action is invoked.</param>
    /// <param name="arg3">The fourth parameter with which the asynchronous action is invoked.</param>
    /// <param name="arg4">The fifth parameter with which the asynchronous action is invoked.</param>
    /// <param name="action">The asynchronous action invoked with the specified five parameters.</param>
    /// <returns>A task that represents the asynchronous action.</returns>
    public static Task IfPresentAsync<T1, T2, T3, T4, T5>(this T1? @this, T2? arg1, T3? arg2, T4? arg3, T5? arg4, Func<T1, T2, T3, T4, T5, Task> action) => @this is null || arg1 is null || arg2 is null || arg3 is null || arg4 is null ? Task.CompletedTask : action(@this, arg1, arg2, arg3, arg4);

    /// <summary>
    /// Invokes the specified asynchronous action with the specified six parameters if they are present.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter with which the asynchronous action is invoked.</typeparam>
    /// <typeparam name="T2">The type of the second parameter with which the asynchronous action is invoked.</typeparam>
    /// <typeparam name="T3">The type of the third parameter with which the asynchronous action is invoked.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter with which the asynchronous action is invoked.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter with which the asynchronous action is invoked.</typeparam>
    /// <typeparam name="T6">The type of the sixth parameter with which the asynchronous action is invoked.</typeparam>
    /// <param name="this">The first parameter with which the asynchronous action is invoked.</param>
    /// <param name="arg1">The second parameter with which the asynchronous action is invoked.</param>
    /// <param name="arg2">The third parameter with which the asynchronous action is invoked.</param>
    /// <param name="arg3">The fourth parameter with which the asynchronous action is invoked.</param>
    /// <param name="arg4">The fifth parameter with which the asynchronous action is invoked.</param>
    /// <param name="arg5">The sixth parameter with which the asynchronous action is invoked.</param>
    /// <param name="action">The asynchronous action invoked with the specified six parameters.</param>
    /// <returns>A task that represents the asynchronous action.</returns>
    public static Task IfPresentAsync<T1, T2, T3, T4, T5, T6>(this T1? @this, T2? arg1, T3? arg2, T4? arg3, T5? arg4, T6? arg5, Func<T1, T2, T3, T4, T5, T6, Task> action) => @this is null || arg1 is null || arg2 is null || arg3 is null || arg4 is null || arg5 is null ? Task.CompletedTask : action(@this, arg1, arg2, arg3, arg4, arg5);

    /// <summary>
    /// Invokes the specified asynchronous action with the specified seven parameters if they are present.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter with which the asynchronous action is invoked.</typeparam>
    /// <typeparam name="T2">The type of the second parameter with which the asynchronous action is invoked.</typeparam>
    /// <typeparam name="T3">The type of the third parameter with which the asynchronous action is invoked.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter with which the asynchronous action is invoked.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter with which the asynchronous action is invoked.</typeparam>
    /// <typeparam name="T6">The type of the sixth parameter with which the asynchronous action is invoked.</typeparam>
    /// <typeparam name="T7">The type of the seventh parameter with which the asynchronous action is invoked.</typeparam>
    /// <param name="this">The first parameter with which the asynchronous action is invoked.</param>
    /// <param name="arg1">The second parameter with which the asynchronous action is invoked.</param>
    /// <param name="arg2">The third parameter with which the asynchronous action is invoked.</param>
    /// <param name="arg3">The fourth parameter with which the asynchronous action is invoked.</param>
    /// <param name="arg4">The fifth parameter with which the asynchronous action is invoked.</param>
    /// <param name="arg5">The sixth parameter with which the asynchronous action is invoked.</param>
    /// <param name="arg6">The seventh parameter with which the asynchronous action is invoked.</param>
    /// <param name="action">The asynchronous action invoked with the specified seven parameters.</param>
    /// <returns>A task that represents the asynchronous action.</returns>
    public static Task IfPresentAsync<T1, T2, T3, T4, T5, T6, T7>(this T1? @this, T2? arg1, T3? arg2, T4? arg3, T5? arg4, T6? arg5, T7? arg6, Func<T1, T2, T3, T4, T5, T6, T7, Task> action) => @this is null || arg1 is null || arg2 is null || arg3 is null || arg4 is null || arg5 is null || arg6 is null ? Task.CompletedTask : action(@this, arg1, arg2, arg3, arg4, arg5, arg6);

    /// <summary>
    /// Invokes the specified asynchronous action with the specified eighth parameters if they are present.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter with which the asynchronous action is invoked.</typeparam>
    /// <typeparam name="T2">The type of the second parameter with which the asynchronous action is invoked.</typeparam>
    /// <typeparam name="T3">The type of the third parameter with which the asynchronous action is invoked.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter with which the asynchronous action is invoked.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter with which the asynchronous action is invoked.</typeparam>
    /// <typeparam name="T6">The type of the sixth parameter with which the asynchronous action is invoked.</typeparam>
    /// <typeparam name="T7">The type of the seventh parameter with which the asynchronous action is invoked.</typeparam>
    /// <typeparam name="T8">The type of the eighth parameter with which the asynchronous action is invoked.</typeparam>
    /// <param name="this">The first parameter with which the asynchronous action is invoked.</param>
    /// <param name="arg1">The second parameter with which the asynchronous action is invoked.</param>
    /// <param name="arg2">The third parameter with which the asynchronous action is invoked.</param>
    /// <param name="arg3">The fourth parameter with which the asynchronous action is invoked.</param>
    /// <param name="arg4">The fifth parameter with which the asynchronous action is invoked.</param>
    /// <param name="arg5">The sixth parameter with which the asynchronous action is invoked.</param>
    /// <param name="arg6">The seventh parameter with which the asynchronous action is invoked.</param>
    /// <param name="arg7">The eighth parameter with which the asynchronous action is invoked.</param>
    /// <param name="action">The asynchronous action invoked with the specified eight parameters.</param>
    /// <returns>A task that represents the asynchronous action.</returns>
    public static Task IfPresentAsync<T1, T2, T3, T4, T5, T6, T7, T8>(this T1? @this, T2? arg1, T3? arg2, T4? arg3, T5? arg4, T6? arg5, T7? arg6, T8? arg7, Func<T1, T2, T3, T4, T5, T6, T7, T8, Task> action) => @this is null || arg1 is null || arg2 is null || arg3 is null || arg4 is null || arg5 is null || arg6 is null || arg7 is null ? Task.CompletedTask : action(@this, arg1, arg2, arg3, arg4, arg5, arg6, arg7);
}