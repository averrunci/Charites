// Copyright (C) 2022 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System.Reflection;
using Carna;

namespace Charites.Windows.Mvc;

[Specification("EventHandlerItem Spec")]
class EventHandlerItemSpec : FixtureSteppable
{
    EventHandlerItemTss Item { get; set; } = default!;

    TestElement Element { get; } = new("Element1");
    string Event { get; } = "EventRaised";

    delegate void NoArgumentCallback();
    delegate void OneArgumentCallback(object e);
    delegate void TwoArgumentsCallback(object sender, object e);

    Delegate NoArgumentHandler { get; }
    Delegate OneArgumentHandler { get; }
    Delegate TwoArgumentsHandler { get; }

    Delegate NoArgumentAsyncHandler { get; }
    Delegate OneArgumentAsyncHandler { get; }
    Delegate TwoArgumentsAsyncHandler { get; }

    object Sender { get; } = new();
    object Args { get; } = new();

    bool NoArgumentMethodCalled { get; set; }
    bool OneArgumentMethodCalled { get; set; }
    bool TwoArgumentMethodCalled { get; set; }

    bool Result { get; set; }

    public EventHandlerItemSpec()
    {
        NoArgumentHandler = new NoArgumentCallback(NoArgumentMethod);
        OneArgumentHandler = new OneArgumentCallback(OneArgumentMethod);
        TwoArgumentsHandler = new TwoArgumentsCallback(TwoArgumentsMethod);

        NoArgumentAsyncHandler = CreateAsyncDelegate(nameof(NoArgumentMethodAsync));
        OneArgumentAsyncHandler = CreateAsyncDelegate(nameof(OneArgumentMethodAsync));
        TwoArgumentsAsyncHandler = CreateAsyncDelegate(nameof(TwoArgumentsMethodAsync));
    }

    void NoArgumentMethod()
    {
        NoArgumentMethodCalled = true;
    }

    void OneArgumentMethod(object e)
    {
        OneArgumentMethodCalled = e == Args;
    }

    void TwoArgumentsMethod(object sender, object e)
    {
        TwoArgumentMethodCalled = sender == Sender && e == Args;
    }

    async Task NoArgumentMethodAsync()
    {
        await Task.Delay(10);
        NoArgumentMethod();
    }

    async Task OneArgumentMethodAsync(object e)
    {
        await Task.Delay(10);
        OneArgumentMethod(e);
    }

    async Task TwoArgumentsMethodAsync(object sender, object e)
    {
        await Task.Delay(10);
        TwoArgumentsMethod(sender, e);
    }

    Delegate CreateAsyncDelegate(string name) => typeof(EventHandlerAction).GetMethod(nameof(EventHandlerAction.OnHandled))?.CreateDelegate(typeof(TwoArgumentsCallback), new EventHandlerAction(GetType().GetMethod(name, BindingFlags.NonPublic | BindingFlags.Instance) ?? throw new InvalidOperationException(), this)) ?? throw new InvalidOperationException();

    [Example("Indicates whether an item has an element of the specified name when an element name is not null or empty")]
    void Ex01()
    {
        Given("an item that has an element name that is not null or empty", () => Item = new EventHandlerItemTss(Element.Name, Element, Event, NoArgumentHandler, true));
        When("the specified name is equal to the element name", () => Result = Item.Has(Element.Name));
        Then("the result should be true", () => Result);
        When("the specified name is not equal to the element name", () => Result = Item.Has(Element.Name + "?"));
        Then("the result should be false", () => !Result);
    }

    [Example("Indicates whether an item has an element of the specified name when an element name is empty")]
    void Ex02()
    {
        Given("an item that has an element name that is empty", () => Item = new EventHandlerItemTss(string.Empty, Element, Event, NoArgumentHandler, true));
        When("the specified name is empty", () => Result = Item.Has(string.Empty));
        Then("the result should be true", () => Result);
        When("the specified name is not empty", () => Result = Item.Has(Element.Name));
        Then("the result should be false", () => !Result);
        When("the specified name is null", () => Result = Item.Has(null));
        Then("the result should be true", () => Result);
    }

    [Example("Adds and removes an event handler")]
    void Ex03()
    {
        Given("an item", () => Item = new EventHandlerItemTss(Element.Name, Element, Event, NoArgumentHandler, true));
        When("the event handler is added", () => Item.AddEventHandler());
        Then("the event handler should be added", () => Item.AddEventHandlerCalled);
        Item.ClearCalled();
        When("the event handler is removed", () => Item.RemoveEventHandler());
        Then("the event handler should be removed", () => Item.RemoveEventHandlerCalled);
    }

    [Example("Raises the specified event whose handler has no argument")]
    void Ex04()
    {
        Given("an item that has an event handler that has no argument", () => Item = new EventHandlerItemTss(Element.Name, Element, Event, NoArgumentHandler, true));
        When("the event is raised", () => Item.Raise(Event, Sender, Args));
        Then("the event handler should be called", () => NoArgumentMethodCalled && !OneArgumentMethodCalled && !TwoArgumentMethodCalled);
        NoArgumentMethodCalled = false;
        When("the event that is not an event of the item is raised", () => Item.Raise(Event + "?", Sender, Args));
        Then("the event handler should not be called", () => !NoArgumentMethodCalled && !OneArgumentMethodCalled && !TwoArgumentMethodCalled);
    }

    [Example("Raises the specified event whose handler has no argument asynchronously")]
    void Ex05()
    {
        Given("an item that has an event handler that has no argument", () => Item = new EventHandlerItemTss(Element.Name, Element, Event, NoArgumentAsyncHandler, true));
        When("the event is raised asynchronously", async () => await Item.RaiseAsync(Event, Sender, Args));
        Then("the event handler should be called", () => NoArgumentMethodCalled && !OneArgumentMethodCalled && !TwoArgumentMethodCalled);
        NoArgumentMethodCalled = false;
        When("the event that is not an event of the item is raised asynchronously", async () => await Item.RaiseAsync(Event + "?", Sender, Args));
        Then("the event handler should not be called", () => !NoArgumentMethodCalled && !OneArgumentMethodCalled && !TwoArgumentMethodCalled);
    }

    [Example("Raises the specified event whose handler has one argument")]
    void Ex06()
    {
        Given("an item that has an event handler that has one argument", () => Item = new EventHandlerItemTss(Element.Name, Element, Event, OneArgumentHandler, true));
        When("the event is raised", () => Item.Raise(Event, Sender, Args));
        Then("the event handler should be called", () => !NoArgumentMethodCalled && OneArgumentMethodCalled && !TwoArgumentMethodCalled);
        OneArgumentMethodCalled = false;
        When("the event that is not an event of the item is raised", () => Item.Raise(Event + "?", Sender, Args));
        Then("the event handler should not be called", () => !NoArgumentMethodCalled && !OneArgumentMethodCalled && !TwoArgumentMethodCalled);
    }

    [Example("Raises the specified event whose handler has one argument asynchronously")]
    void Ex07()
    {
        Given("an item that has an event handler that has one argument", () => Item = new EventHandlerItemTss(Element.Name, Element, Event, OneArgumentAsyncHandler, true));
        When("the event is raised asynchronously", async () => await Item.RaiseAsync(Event, Sender, Args));
        Then("the event handler should be called", () => !NoArgumentMethodCalled && OneArgumentMethodCalled && !TwoArgumentMethodCalled);
        OneArgumentMethodCalled = false;
        When("the event that is not an event of the item is raised asynchronously", async () => await Item.RaiseAsync(Event + "?", Sender, Args));
        Then("the event handler should not be called", () => !NoArgumentMethodCalled && !OneArgumentMethodCalled && !TwoArgumentMethodCalled);
    }

    [Example("Raises the specified event whose handler has two arguments")]
    void Ex08()
    {
        Given("an item that has an event handler that has two arguments", () => Item = new EventHandlerItemTss(Element.Name, Element, Event, TwoArgumentsHandler, true));
        When("the event is raised", () => Item.Raise(Event, Sender, Args));
        Then("the event handler should be called", () => !NoArgumentMethodCalled && !OneArgumentMethodCalled && TwoArgumentMethodCalled);
        TwoArgumentMethodCalled = false;
        When("the event that is not an event of the item is raised", () => Item.Raise(Event + "?", Sender, Args));
        Then("the event handler should not be called", () => !NoArgumentMethodCalled && !OneArgumentMethodCalled && !TwoArgumentMethodCalled);
    }

    [Example("Raises the specified event whose handler has two arguments asynchronously")]
    void Ex09()
    {
        Given("an item that has an event handler that has two arguments", () => Item = new EventHandlerItemTss(Element.Name, Element, Event, TwoArgumentsAsyncHandler, true));
        When("the event is raised asynchronously", async () => await Item.RaiseAsync(Event, Sender, Args));
        Then("the event handler should be called", () => !NoArgumentMethodCalled && !OneArgumentMethodCalled && TwoArgumentMethodCalled);
        TwoArgumentMethodCalled = false;
        When("the event that is not an event of the item is raised asynchronously", async () => await Item.RaiseAsync(Event + "?", Sender, Args));
        Then("the event handler should not be called", () => !NoArgumentMethodCalled && !OneArgumentMethodCalled && !TwoArgumentMethodCalled);
    }
}