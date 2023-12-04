// Copyright (C) 2022-2023 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System.Reflection;

namespace Charites.Windows.Mvc;

internal sealed class ExceptionHandlingEventHandlerAction(
    MethodInfo method,
    object target,
    Func<Exception, bool> unhandledExceptionHandler
) : EventHandlerAction(method, target)
{
    protected override bool HandleUnhandledException(Exception exc) => unhandledExceptionHandler(exc);
}