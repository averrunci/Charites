// Copyright (C) 2022 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System.Collections.Concurrent;

namespace Charites.Windows.Mvc;

internal sealed class EventHandlerExtensionTss : EventHandlerExtension<TestElement, EventHandlerItemTss>
{
    private IDictionary<object, EventHandlerBase<TestElement, EventHandlerItemTss>>? eventHandlerBases;

    public bool AssertParameterResolverTypes(params Type[] parameterResolverTypes)
        => ParameterResolverTypes.SequenceEqual(parameterResolverTypes);

    protected override IDictionary<object, EventHandlerBase<TestElement, EventHandlerItemTss>> EnsureEventHandlerBases(TestElement? element)
        => eventHandlerBases ??= new ConcurrentDictionary<object, EventHandlerBase<TestElement, EventHandlerItemTss>>();

    protected override void AddEventHandler(TestElement? element, EventHandlerAttribute eventHandlerAttribute, Func<Type?, Delegate?> handlerCreator, EventHandlerBase<TestElement, EventHandlerItemTss> eventHandlers)
    {
        eventHandlers.Add(new EventHandlerItemTss(
            eventHandlerAttribute.ElementName, element,
            eventHandlerAttribute.Event,  handlerCreator(null), eventHandlerAttribute.HandledEventsToo,
            CreateParameterResolver()
        ));
    }
}