// Copyright (C) 2019 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Charites.Windows.Mvc
{
    internal sealed class ParameterDependencyResolverTss : ParameterDependencyResolver
    {
        private readonly IDictionary<Type, Func<object>> dependencyContainer;

        public ParameterDependencyResolverTss(IDictionary<Type, Func<object>> dependencyContainer)
        {
            this.dependencyContainer = dependencyContainer;
        }

        protected override object ResolveParameterFromDependency(ParameterInfo parameter)
            => dependencyContainer.ContainsKey(parameter.ParameterType) ?
                dependencyContainer[parameter.ParameterType]() :
                base.ResolveParameterFromDependency(parameter);
    }
}
