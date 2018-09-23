// Copyright (C) 2018 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System;
using System.Reflection;

namespace Charites.Windows.Mvc
{
    internal sealed class ExceptionHandlingEventHandlerAction : EventHandlerAction
    {
        private readonly Func<Exception, bool> unhandledExceptionHandler;

        public ExceptionHandlingEventHandlerAction(MethodInfo method, object target, Func<Exception, bool> unhandledExceptionHandler) : base(method, target)
        {
            this.unhandledExceptionHandler = unhandledExceptionHandler;
        }

        protected override bool HandleUnhandledException(Exception exc) => unhandledExceptionHandler(exc);
    }
}
