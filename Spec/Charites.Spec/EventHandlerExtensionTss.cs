// Copyright (C) 2018 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Charites.Windows.Mvc
{
    internal sealed class EventHandlerExtensionTss : EventHandlerExtension<TestElement, EventHandlerItemTss>
    {
        private IDictionary<object, EventHandlerBase<TestElement, EventHandlerItemTss>> eventHandlerBases;

        protected override IDictionary<object, EventHandlerBase<TestElement, EventHandlerItemTss>> EnsureEventHandlerBases(TestElement element)
             => eventHandlerBases ?? (eventHandlerBases = new ConcurrentDictionary<object, EventHandlerBase<TestElement, EventHandlerItemTss>>());

        protected override void AddEventHandler(TestElement element, EventHandlerAttribute eventHandlerAttribute, Func<Type, Delegate> handlerCreator, EventHandlerBase<TestElement, EventHandlerItemTss> eventHandlers)
        {
            eventHandlers.Add(new EventHandlerItemTss(
                eventHandlerAttribute.ElementName, element,
                eventHandlerAttribute.Event,  handlerCreator(null), eventHandlerAttribute.HandledEventsToo
            ));
        }
    }
}
