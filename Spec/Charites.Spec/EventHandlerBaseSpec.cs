// Copyright (C) 2022 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System.Reflection;
using Carna;

namespace Charites.Windows.Mvc;

[Specification("EventHandlerBase Spec")]
class EventHandlerBaseSpec : FixtureSteppable
{
    EventHandlerBase<TestElement, EventHandlerItemTss> EventHandlerBase { get; } = new();

    EventHandlerItemTss Item1 { get; }
    EventHandlerItemTss Item2 { get; } 
    EventHandlerItemTss Item3 { get; }
    EventHandlerItemTss Item4 { get; }
    EventHandlerItemTss Item5 { get; }
    EventHandlerItemTss Item6 { get; }
    EventHandlerItemTss Item7 { get; }
    EventHandlerItemTss Item8 { get; }
    EventHandlerItemTss Item11 { get; }
    EventHandlerItemTss Item12 { get; }
    EventHandlerItemTss Item13 { get; }
    EventHandlerItemTss Item14 { get; }
    EventHandlerItemTss Item15 { get; }
    EventHandlerItemTss Item16 { get; }
    EventHandlerItemTss Item17 { get; }
    EventHandlerItemTss Item18 { get; }

    EventHandlerItemTss[] Items { get; }

    TestElement Element1 { get; } = new("Element1");
    TestElement Element2 { get; } = new("Element2");
    TestElement Element3 { get; } = new("Element3");
    TestElement Element4 { get; } = new("Element4");
    TestElement Element5 { get; } = new("Element5");
    TestElement Element6 { get; } = new("Element6");

    string Event1 { get; } = "Event1Raised";
    string Event2 { get; } = "Event2Raised";
    string Event3 { get; } = "Event3Raised";
    string Event4 { get; } = "Event4Raised";
    string Event5 { get; } = "Event5Raised";
    string Event6 { get; } = "Event6Raised";

    object Sender { get; } = new();
    object Args { get; } = new();

    interface IDependency1 { }
    interface IDependency2 { }
    interface IDependency3 { }
    class Dependency1Implementation : IDependency1 { }
    class Dependency2Implementation : IDependency2 { }
    class Dependency3Implementation : IDependency3 { }

    IDependency1 Dependency1 { get; } = new Dependency1Implementation();
    IDependency2 Dependency2 { get; } = new Dependency2Implementation();
    IDependency3 Dependency3 { get; } = new Dependency3Implementation();

    int HandleEvent1CalledCount { get; set; }
    int HandleEvent2CalledCount { get; set; }
    int HandleEvent3CalledCount { get; set; }
    int HandleEvent4CalledCount { get; set; }
    int HandleEvent5CalledCount { get; set; }
    int HandleEvent6CalledCount { get; set; }

    Delegate Event1Handler { get; }
    Delegate Event2Handler { get; }
    Delegate Event3Handler { get; }
    Delegate Event4Handler { get; }
    Delegate Event5Handler { get; }
    Delegate Event6Handler { get; }

    Delegate Event1AsyncHandler { get; }
    Delegate Event2AsyncHandler { get; }
    Delegate Event3AsyncHandler { get; }
    Delegate Event4AsyncHandler { get; }
    Delegate Event5AsyncHandler { get; }
    Delegate Event6AsyncHandler { get; }

    delegate void HandleEvent1Callback();
    delegate void HandleEvent2Callback(object e);
    delegate void HandleEvent3Callback(object sender, object e);
    delegate void HandleEvent4Callback(IDependency1 dependency1, IDependency2 dependency2, IDependency3 dependency3);
    delegate void HandleEvent5Callback(IDependency1 dependency1, object e, IDependency2 dependency2, IDependency3 dependency3);
    delegate void HandleEvent6Callback(IDependency1 dependency1, object sender, IDependency2 dependency2, object e, IDependency3 dependency3);

    void HandleEvent1()
    {
        ++HandleEvent1CalledCount;
    }

    void HandleEvent2(object e)
    {
        if (e == Args) ++HandleEvent2CalledCount;
    }

    void HandleEvent3(object sender, object e)
    {
        if (sender == Sender && e == Args) ++HandleEvent3CalledCount;
    }

    void HandleEvent4([FromDI] IDependency1 dependency1, [FromDI] IDependency2 dependency2, [FromDI] IDependency3 dependency3)
    {
        if (dependency1 == Dependency1 && dependency2 == Dependency2 && dependency3 == Dependency3) ++HandleEvent4CalledCount;
    }

    void HandleEvent5([FromDI] IDependency1 dependency1, object e, [FromDI] IDependency2 dependency2, [FromDI] IDependency3 dependency3)
    {
        if (e == Args && dependency1 == Dependency1 && dependency2 == Dependency2 && dependency3 == Dependency3) ++HandleEvent5CalledCount;
    }

    void HandleEvent6([FromDI] IDependency1 dependency1, object sender, [FromDI] IDependency2 dependency2, object e, [FromDI] IDependency3 dependency3)
    {
        if (sender == Sender && e == Args && dependency1 == Dependency1 && dependency2 == Dependency2 && dependency3 == Dependency3) ++HandleEvent6CalledCount;
    }

    async Task HandleEvent1Async()
    {
        await Task.Delay(10);
        HandleEvent1();
    }

    async Task HandleEvent2Async(object e)
    {
        await Task.Delay(10);
        HandleEvent2(e);
    }

    async Task HandleEvent3Async(object sender, object e)
    {
        await Task.Delay(10);
        HandleEvent3(sender, e);
    }

    async Task HandleEvent4Async([FromDI] IDependency1 dependency1, [FromDI] IDependency2 dependency2, [FromDI] IDependency3 dependency3)
    {
        await Task.Delay(10);
        HandleEvent4(dependency1, dependency2, dependency3);
    }

    async Task HandleEvent5Async([FromDI] IDependency1 dependency1, object e, [FromDI] IDependency2 dependency2, [FromDI] IDependency3 dependency3)
    {
        await Task.Delay(10);
        HandleEvent5(dependency1, e, dependency2, dependency3);
    }

    async Task HandleEvent6Async([FromDI] IDependency1 dependency1, object sender, [FromDI] IDependency2 dependency2, object e, [FromDI] IDependency3 dependency3)
    {
        await Task.Delay(10);
        HandleEvent6(dependency1, sender, dependency2, e, dependency3);
    }

    Delegate CreateAsyncDelegate(string name) => typeof(EventHandlerAction).GetMethod(nameof(EventHandlerAction.OnHandled))?.CreateDelegate(typeof(HandleEvent3Callback), new EventHandlerAction(GetType().GetMethod(name, BindingFlags.NonPublic | BindingFlags.Instance) ?? throw new InvalidOperationException(), this)) ?? throw new InvalidOperationException();

    [Background("Adds items that have the dedicate event handler (different parameters  or asynchronous)")]
    public EventHandlerBaseSpec()
    {
        Event1Handler = new HandleEvent1Callback(HandleEvent1);
        Event2Handler = new HandleEvent2Callback(HandleEvent2);
        Event3Handler = new HandleEvent3Callback(HandleEvent3);
        Event4Handler = new HandleEvent4Callback(HandleEvent4);
        Event5Handler = new HandleEvent5Callback(HandleEvent5);
        Event6Handler = new HandleEvent6Callback(HandleEvent6);

        Event1AsyncHandler = CreateAsyncDelegate(nameof(HandleEvent1Async));
        Event2AsyncHandler = CreateAsyncDelegate(nameof(HandleEvent2Async));
        Event3AsyncHandler = CreateAsyncDelegate(nameof(HandleEvent3Async));
        Event4AsyncHandler = CreateAsyncDelegate(nameof(HandleEvent4Async));
        Event5AsyncHandler = CreateAsyncDelegate(nameof(HandleEvent5Async));
        Event6AsyncHandler = CreateAsyncDelegate(nameof(HandleEvent6Async));

        Item1 = new EventHandlerItemTss(Element1.Name, Element1, Event1, Event1Handler, true);
        Item2 = new EventHandlerItemTss(Element2.Name, Element2, Event2, Event2Handler, true);
        Item3 = new EventHandlerItemTss(Element3.Name, Element3, Event3, Event3Handler, true);
        Item4 = new EventHandlerItemTss(Element3.Name, Element3, Event3, Event3Handler, true);
        Item5 = new EventHandlerItemTss(Element4.Name, Element4, Event1, Event1AsyncHandler, true);
        Item6 = new EventHandlerItemTss(Element5.Name, Element5, Event2, Event2AsyncHandler, true);
        Item7 = new EventHandlerItemTss(Element6.Name, Element6, Event3, Event3AsyncHandler, true);
        Item8 = new EventHandlerItemTss(Element6.Name, Element6, Event3, Event3AsyncHandler, true);
        Item11 = new EventHandlerItemTss(Element1.Name, Element1, Event4, Event4Handler, true);
        Item12 = new EventHandlerItemTss(Element2.Name, Element2, Event5, Event5Handler, true);
        Item13 = new EventHandlerItemTss(Element3.Name, Element3, Event6, Event6Handler, true);
        Item14 = new EventHandlerItemTss(Element3.Name, Element3, Event6, Event6Handler, true);
        Item15 = new EventHandlerItemTss(Element4.Name, Element4, Event4, Event4AsyncHandler, true);
        Item16 = new EventHandlerItemTss(Element5.Name, Element5, Event5, Event5AsyncHandler, true);
        Item17 = new EventHandlerItemTss(Element6.Name, Element6, Event6, Event6AsyncHandler, true);
        Item18 = new EventHandlerItemTss(Element6.Name, Element6, Event6, Event6AsyncHandler, true);
        Items = new[] { Item1, Item2, Item3, Item4, Item5, Item6, Item7, Item8, Item11, Item12, Item13, Item14, Item15, Item16, Item17, Item18 };

        EventHandlerBase.Add(Item1);
        EventHandlerBase.Add(Item2);
        EventHandlerBase.Add(Item3);
        EventHandlerBase.Add(Item4);
        EventHandlerBase.Add(Item5);
        EventHandlerBase.Add(Item6);
        EventHandlerBase.Add(Item7);
        EventHandlerBase.Add(Item8);
        EventHandlerBase.Add(Item11);
        EventHandlerBase.Add(Item12);
        EventHandlerBase.Add(Item13);
        EventHandlerBase.Add(Item14);
        EventHandlerBase.Add(Item15);
        EventHandlerBase.Add(Item16);
        EventHandlerBase.Add(Item17);
        EventHandlerBase.Add(Item18);
    }

    bool AssertEventHandlerCalled(int expectedEvent1HandledCount, int expectedEvent2HandledCount, int expectedEvent3HandledCount)
    {
        var result = HandleEvent1CalledCount == expectedEvent1HandledCount && HandleEvent2CalledCount == expectedEvent2HandledCount && HandleEvent3CalledCount == expectedEvent3HandledCount;
        HandleEvent1CalledCount = HandleEvent2CalledCount = HandleEvent3CalledCount = 0;
        return result;
    }

    bool AssertAttributedParameterEventHandlerCalled(int expectedEvent4HandledCount, int expectedEvent5HandledCount, int expectedEvent6HandledCount)
    {
        var result = HandleEvent4CalledCount == expectedEvent4HandledCount && HandleEvent5CalledCount == expectedEvent5HandledCount && HandleEvent6CalledCount == expectedEvent6HandledCount;
        HandleEvent4CalledCount = HandleEvent5CalledCount = HandleEvent6CalledCount = 0;
        return result;
    }

    [Example("Adds / Removes event handlers")]
    void Ex01()
    {
        When("event handlers are added", () => EventHandlerBase.AddEventHandlers());
        Then("the event handlers should be added", () => Items.All(item => item.AddEventHandlerCalled));
        Items.ForEach(item => item.ClearCalled());
        When("event handlers are removed", () => EventHandlerBase.RemoveEventHandlers());
        Then("the event handlers should be removed", () => Items.All(item => item.RemoveEventHandlerCalled));
    }

    [Example("Raises an event")]
    void Ex02()
    {
        When("the event is raised for the specified element", () => EventHandlerBase.GetBy(Element1.Name).Raise(Event1));
        Then("the event should be handled", () => AssertEventHandlerCalled(1, 0, 0));
        When("the event is raised for the specified element with an event data", () => EventHandlerBase.GetBy(Element2.Name).With(Args).Raise(Event2));
        Then("the event should be handled with the specified event data", () => AssertEventHandlerCalled(0, 1, 0));
        When("the event is raised for the specified element with a sender object and an event data", () => EventHandlerBase.GetBy(Element3.Name).From(Sender).With(Args).Raise(Event3));
        Then("the event should be handled with the specified sender object and event data", () => AssertEventHandlerCalled(0, 0, 2));

        When("the event is raised for the specified element with parameters specified by the attribute", () =>
            EventHandlerBase.GetBy(Element1.Name)
                .Resolve<FromDIAttribute, IDependency1>(() => Dependency1)
                .Resolve<FromDIAttribute, IDependency2>(() => Dependency2)
                .Resolve<FromDIAttribute, IDependency3>(() => Dependency3)
                .Raise(Event4)
        );
        Then("the event should be handled with the parameters specified by the attribute", () => AssertAttributedParameterEventHandlerCalled(1, 0, 0));
        When("the event is raised for the specified element with parameters specified by the attribute and an event data", () =>
            EventHandlerBase.GetBy(Element2.Name)
                .With(Args)
                .Resolve<FromDIAttribute, IDependency1>(() => Dependency1)
                .Resolve<FromDIAttribute, IDependency2>(() => Dependency2)
                .Resolve<FromDIAttribute, IDependency3>(() => Dependency3)
                .Raise(Event5)
        );
        Then("the event should be handled with the parameters specified by the attribute and event data", () => AssertAttributedParameterEventHandlerCalled(0, 1, 0));
        When("the event is raised for the specified element with parameters specified by the attribute, a sender object and an event data", () =>
            EventHandlerBase.GetBy(Element3.Name)
                .From(Sender)
                .With(Args)
                .Resolve<FromDIAttribute, IDependency1>(() => Dependency1)
                .Resolve<FromDIAttribute, IDependency2>(() => Dependency2)
                .Resolve<FromDIAttribute, IDependency3>(() => Dependency3)
                .Raise(Event6)
        );
        Then("the event should be handled with the parameters specified by the attribute, sender object and event data", () => AssertAttributedParameterEventHandlerCalled(0, 0, 2));
    }

    [Example("Raises an event asynchronously")]
    void Ex03()
    {
        When("the event is raised for the specified element asynchronously", async () => await EventHandlerBase.GetBy(Element4.Name).RaiseAsync(Event1));
        Then("the event should be handled", () => AssertEventHandlerCalled(1, 0, 0));
        When("the event is raised for the specified element with an event data asynchronously", async () => await EventHandlerBase.GetBy(Element5.Name).With(Args).RaiseAsync(Event2));
        Then("the event should be handled with the specified event data", () => AssertEventHandlerCalled(0, 1, 0));
        When("the event is raised for the specified element with a sender object and an event data", async () => await EventHandlerBase.GetBy(Element6.Name).From(Sender).With(Args).RaiseAsync(Event3));
        Then("the event should be handled with the specified sender object and event data", () => AssertEventHandlerCalled(0, 0, 2));

        When("the event is raised for the specified element with parameters specified by the attribute asynchronously", async () =>
            await EventHandlerBase.GetBy(Element4.Name)
                .Resolve<FromDIAttribute, IDependency1>(() => Dependency1)
                .Resolve<FromDIAttribute, IDependency2>(() => Dependency2)
                .Resolve<FromDIAttribute, IDependency3>(() => Dependency3)
                .RaiseAsync(Event4)
        );
        Then("the event should be handled with the parameters specified by the attribute", () => AssertAttributedParameterEventHandlerCalled(1, 0, 0));
        When("the event is raised for the specified element with parameters specified by the attribute and an event data asynchronously", async () =>
            await EventHandlerBase.GetBy(Element5.Name)
                .With(Args)
                .Resolve<FromDIAttribute, IDependency1>(() => Dependency1)
                .Resolve<FromDIAttribute, IDependency2>(() => Dependency2)
                .Resolve<FromDIAttribute, IDependency3>(() => Dependency3)
                .RaiseAsync(Event5)
        );
        Then("the event should be handled with the parameters specified by the attribute and event data", () => AssertAttributedParameterEventHandlerCalled(0, 1, 0));
        When("the event is raised for the specified element with parameters specified by the attribute, a sender object and an event data", async () =>
            await EventHandlerBase.GetBy(Element6.Name)
                .From(Sender)
                .With(Args)
                .Resolve<FromDIAttribute, IDependency1>(() => Dependency1)
                .Resolve<FromDIAttribute, IDependency2>(() => Dependency2)
                .Resolve<FromDIAttribute, IDependency3>(() => Dependency3)
                .RaiseAsync(Event6)
        );
        Then("the event should be handled with the parameters specified by the attribute, sender object and event data", () => AssertAttributedParameterEventHandlerCalled(0, 0, 2));
    }

    [Example("Does not raise an event when the specified element does not exist")]
    void Ex04()
    {
        When("the item that has an event handler of the Element1 is removed", () => EventHandlerBase.Remove(Item1));
        When("the event of the Element1 is raised", () => EventHandlerBase.GetBy(Element1.Name).Raise(Event1));
        Then("the event should not be handled", () => AssertEventHandlerCalled(0, 0, 0));
        When("the item is cleared", () => EventHandlerBase.Clear());
        When("the event is raised for the specified element asynchronously", async () => await EventHandlerBase.GetBy(Element4.Name).RaiseAsync(Event1));
        Then("the event should not be handled", () => AssertEventHandlerCalled(0, 0, 0));
    }
}