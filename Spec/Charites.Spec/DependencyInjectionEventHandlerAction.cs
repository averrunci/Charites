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
        private readonly IDictionary<Type, Func<object>> dependencyResolver;

        public DependencyInjectionEventHandlerAction(MethodInfo method, object target, IDictionary<Type, Func<object>> dependencyResolver) : base(method, target)
        {
            this.dependencyResolver = dependencyResolver;
        }

        protected override IParameterDependencyResolver CreateParameterDependencyResolver()
            => new ParameterDependencyResolverTss(dependencyResolver);
    }
}
