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
        private readonly IDictionary<Type, Func<object>> dependencyResolver;

        public ParameterDependencyResolverTss(IDictionary<Type, Func<object>> dependencyResolver)
        {
            this.dependencyResolver = dependencyResolver;
        }

        protected override object ResolveParameterFromDependency(ParameterInfo parameter)
            => dependencyResolver.ContainsKey(parameter.ParameterType) ?
                dependencyResolver[parameter.ParameterType]() :
                base.ResolveParameterFromDependency(parameter);
    }
}
