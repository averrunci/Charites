// Copyright (C) 2022-2023 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System.Reflection;
using Carna;

namespace Charites.Windows.Mvc;

[Specification("EventHandlerParameterResolver Customization Spec")]
class CustomEventHandlerParameterResolverSpec : FixtureSteppable
{
    EventHandlerBase<TestElement, EventHandlerItemTss> EventHandlerBase { get; } = new();

    EventHandlerItemTss Item { get; }
    TestElement Element { get; } = new("Element");
    string Event => "EventRaised";
    Delegate EventHandler { get; }
    delegate void HandleEventCallback(int _);
    void HandleEvent([FromConstant] int parameter) => EventHandled = parameter == 777;

    bool EventHandled { get; set; }

    public CustomEventHandlerParameterResolverSpec()
    {
        EventHandler = new HandleEventCallback(HandleEvent);
        Item = new EventHandlerItemTss(Element.Name, Element, Event, EventHandler, true);
        EventHandlerBase.Add(Item);
    }

    [Example("Handles an event handler that have a parameter specified by the custom attribute")]
    void Ex01()
    {
        When("the event is raised for the specified element with parameters specified by the custom attribute", () =>
            EventHandlerBase.GetBy(Element.Name)
                .ResolveFromConstant(777)
                .Raise(Event)
        );
        Then("the event should be handled with the parameters specified by the custom attribute", () => EventHandled);
    }
}

[AttributeUsage(AttributeTargets.Parameter)]
class FromConstantAttribute : EventHandlerParameterAttribute;

class DefaultEventHandlerParameterFromConstantResolver(int value) : IEventHandlerParameterResolver
{
    public object Resolve(ParameterInfo parameter) => value;
}

static class EventHandlerBaseExecutorExtensions
{
    public static EventHandlerBase<TestElement, EventHandlerItemTss>.Executor ResolveFromConstant(this EventHandlerBase<TestElement, EventHandlerItemTss>.Executor @this, int value)
    {
        return @this.Resolve<FromConstantAttribute>(new DefaultEventHandlerParameterFromConstantResolver(value));
    }
}