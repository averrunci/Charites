// Copyright (C) 2019 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System;
using System.Collections;
using System.Linq;
using System.Reflection;

namespace Charites.Windows.Mvc
{
    /// <summary>
    /// Provides the function to resolve parameters from the dependency injection.
    /// </summary>
    public class ParameterDependencyResolver : IParameterDependencyResolver
    {
        /// <summary>
        /// Resolves parameters with the specified method metadata, source of the event, and event data.
        /// </summary>
        /// <param name="method">The invoked method metadata.</param>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        /// <returns>Parameters of the invoked method.</returns>
        protected virtual object[] Resolve(MethodInfo method, object sender, object e)
        {
            var parameters = method.GetParameters();
            var parameterCountExceptDependencyInjection = parameters.Count(p => p.GetCustomAttribute<FromDIAttribute>() == null);
            if (parameterCountExceptDependencyInjection > 2)
                throw new InvalidOperationException("The length of the method parameters except ones attributed by FromDIAttribute attribute must be less than 3.");

            var specificParameterQueue = new Queue();
            switch (parameterCountExceptDependencyInjection)
            {
                case 1:
                    specificParameterQueue.Enqueue(e);
                    break;
                case 2:
                    specificParameterQueue.Enqueue(sender);
                    specificParameterQueue.Enqueue(e);
                    break;
            }
            return parameters.Select(parameter => ResolveParameter(parameter, specificParameterQueue)).ToArray();
        }

        /// <summary>
        /// Resolves a parameter with the specified <see cref="ParameterInfo"/> and queue that
        /// contains the source of the event and event data.
        /// </summary>
        /// <param name="parameter">The <see cref="ParameterInfo"/> from which a parameter is resolved.</param>
        /// <param name="specificParameterQueue">The queue that contains the source of the event and event data.</param>
        /// <returns>A parameter of the invoked method.</returns>
        protected virtual object ResolveParameter(ParameterInfo parameter, Queue specificParameterQueue)
        {
            return parameter.GetCustomAttribute<FromDIAttribute>() == null ? specificParameterQueue.Dequeue() : ResolveParameterFromDependency(parameter);
        }

        /// <summary>
        /// Resolves a parameter with the specified <see cref="ParameterInfo"/> from the dependency injection.
        /// </summary>
        /// <param name="parameter">The <see cref="ParameterInfo"/> from which a parameter is resolved.</param>
        /// <returns>A parameter of the invoked method.</returns>
        protected virtual object ResolveParameterFromDependency(ParameterInfo parameter)
        {
            return null;
        }

        object[] IParameterDependencyResolver.Resolve(MethodInfo method, object sender, object e) => Resolve(method, sender, e);
    }
}
