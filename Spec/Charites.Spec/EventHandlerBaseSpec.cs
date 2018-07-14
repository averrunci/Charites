// Copyright (C) 2018 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Carna;

namespace Charites.Windows.Mvc
{
    [Specification("EventHandlerBase Spec")]
    class EventHandlerBaseSpec : FixtureSteppable
    {
        EventHandlerBase<TestElement, EventHandlerItemTss> EventHandlerBase { get; } = new EventHandlerBase<TestElement, EventHandlerItemTss>();

        EventHandlerItemTss Item1 { get; }
        EventHandlerItemTss Item2 { get; } 
        EventHandlerItemTss Item3 { get; }
        EventHandlerItemTss Item4 { get; }
        EventHandlerItemTss Item5 { get; }
        EventHandlerItemTss Item6 { get; }
        EventHandlerItemTss Item7 { get; }
        EventHandlerItemTss Item8 { get; }

        EventHandlerItemTss[] Items { get; }

        TestElement Element1 { get; } = new TestElement("Element1");
        TestElement Element2 { get; } = new TestElement("Element2");
        TestElement Element3 { get; } = new TestElement("Element3");
        TestElement Element4 { get; } = new TestElement("Element4");
        TestElement Element5 { get; } = new TestElement("Element5");
        TestElement Element6 { get; } = new TestElement("Element6");

        string Event1 { get; } = "Event1Raised";
        string Event2 { get; } = "Event2Raised";
        string Event3 { get; } = "Event3Raised";

        object Sender { get; } = new object();
        object Args { get; } = new object();

        int HandleEvent1CalledCount { get; set; }
        int HandleEvent2CalledCount { get; set; }
        int HandleEvent3CalledCount { get; set; }

        Delegate Event1Handler { get; }
        Delegate Event2Handler { get; }
        Delegate Event3Handler { get; }

        Delegate Event1AsyncHandler { get; }
        Delegate Event2AsyncHandler { get; }
        Delegate Event3AsyncHandler { get; }

        delegate void HandleEvent1Callback();
        delegate void HandleEvent2Callback(object e);
        delegate void HandleEvent3Callback(object sender, object e);

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

        Delegate CreateAsyncDelegate(string name) => typeof(EventHandlerAction).GetMethod(nameof(EventHandlerAction.OnHandled)).CreateDelegate(typeof(HandleEvent3Callback), new EventHandlerAction(GetType().GetMethod(name, BindingFlags.NonPublic | BindingFlags.Instance), this));

        [Background("Adds items that have the dedicate event handler (different parameters  or asynchronous)")]
        public EventHandlerBaseSpec()
        {
            Event1Handler = new HandleEvent1Callback(HandleEvent1);
            Event2Handler = new HandleEvent2Callback(HandleEvent2);
            Event3Handler = new HandleEvent3Callback(HandleEvent3);

            Event1AsyncHandler = CreateAsyncDelegate(nameof(HandleEvent1Async));
            Event2AsyncHandler = CreateAsyncDelegate(nameof(HandleEvent2Async));
            Event3AsyncHandler = CreateAsyncDelegate(nameof(HandleEvent3Async));

            Item1 = new EventHandlerItemTss(Element1.Name, Element1, Event1, Event1Handler, true);
            Item2 = new EventHandlerItemTss(Element2.Name, Element2, Event2, Event2Handler, true);
            Item3 = new EventHandlerItemTss(Element3.Name, Element3, Event3, Event3Handler, true);
            Item4 = new EventHandlerItemTss(Element3.Name, Element3, Event3, Event3Handler, true);
            Item5 = new EventHandlerItemTss(Element4.Name, Element4, Event1, Event1AsyncHandler, true);
            Item6 = new EventHandlerItemTss(Element5.Name, Element5, Event2, Event2AsyncHandler, true);
            Item7 = new EventHandlerItemTss(Element6.Name, Element6, Event3, Event3AsyncHandler, true);
            Item8 = new EventHandlerItemTss(Element6.Name, Element6, Event3, Event3AsyncHandler, true);
            Items = new[] { Item1, Item2, Item3, Item4, Item5, Item6, Item7, Item8 };

            EventHandlerBase.Add(Item1);
            EventHandlerBase.Add(Item2);
            EventHandlerBase.Add(Item3);
            EventHandlerBase.Add(Item4);
            EventHandlerBase.Add(Item5);
            EventHandlerBase.Add(Item6);
            EventHandlerBase.Add(Item7);
            EventHandlerBase.Add(Item8);
        }

        bool AssertEventHandlerCalled(int expectedEvent1HandledCount, int expectedEvent2HandledCount, int expectedEvent3HandledCount)
        {
            var result = HandleEvent1CalledCount == expectedEvent1HandledCount && HandleEvent2CalledCount == expectedEvent2HandledCount && HandleEvent3CalledCount == expectedEvent3HandledCount;
            HandleEvent1CalledCount = HandleEvent2CalledCount = HandleEvent3CalledCount = 0;
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
}
