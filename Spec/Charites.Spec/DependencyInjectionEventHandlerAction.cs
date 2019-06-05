// Copyright (C) 2019 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Charites.Windows.Mvc
{
    internal sealed class DependencyInjectionEventHandlerAction : EventHandlerAction
    {
        private readonly IDictionary<Type, Func<object>> diContainer;

        public DependencyInjectionEventHandlerAction(MethodInfo method, object target, IDictionary<Type, Func<object>> diContainer) : base(method, target)
        {
            this.diContainer = diContainer;
        }

        protected override object ResolveParameterFromDI(ParameterInfo parameter)
        {
            return diContainer.ContainsKey(parameter.ParameterType) ? diContainer[parameter.ParameterType]() : base.ResolveParameterFromDI(parameter);
        }
    }
}
