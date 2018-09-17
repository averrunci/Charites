﻿// Copyright (C) 2018 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System;
using System.Reflection;

namespace Charites.Windows.Mvc
{
    /// <summary>
    /// Represents the action of an event handler.
    /// </summary>
    public class EventHandlerAction
    {
        private readonly MethodInfo method;
        private readonly object target;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventHandlerAction"/> class
        /// with the specified method to handle an event and target to invoke it.
        /// </summary>
        /// <param name="method">The method to handle an event.</param>
        /// <param name="target">The target object to invoke the method to handle an event.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="method"/> is <c>null</c>.
        /// </exception>
        public EventHandlerAction(MethodInfo method, object target)
        {
            this.method = method ?? throw new ArgumentNullException(nameof(method));
            this.target = target;
        }

        /// <summary>
        /// Handles the event with the specified source of the event and event data.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        public void OnHandled(object sender, object e) => Handle(sender, e);

        /// <summary>
        /// Handles the event with the specified source of the event and event data.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        /// <returns>An object containing the return value of the invoked method.</returns>
        public object Handle(object sender, object e)
        {
            switch (method.GetParameters().Length)
            {
                case 0: return Handle(null);
                case 1: return Handle(new[] { e });
                case 2: return Handle(new[] { sender, e });
                default: throw new InvalidOperationException("The length of the method parameters must be less than 3.");
            }
        }

        /// <summary>
        /// Handles the event with the specified parameters.
        /// </summary>
        /// <param name="parameters">The parameters to handle the event.</param>
        /// <returns>An object containing the return value of the invoked method.</returns>
        protected virtual object Handle(object[] parameters) => method.Invoke(target, parameters);
    }
}
