// Copyright (C) 2018 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System;
using System.Reflection;
using Carna;

namespace Charites.Windows.Mvc
{
    [Specification("EventHandlerAction Spec")]
    class EventHandlerActionSpec : FixtureSteppable
    {
        EventHandlerAction Action { get; set; }

        object Sender { get; set; } = new object();
        object Args { get; set; } = new object();

        bool NoArgumentMethodCalled { get; set; }
        bool OneArgumentMethodCalled { get; set; }
        bool TwoArgumentMethodCalled { get; set; }
        bool Result { get; set; }

        bool NoArgumentMethod()
        {
            NoArgumentMethodCalled = true;
            return NoArgumentMethodCalled;
        }

        bool OneArgumentMethod(object e)
        {
            OneArgumentMethodCalled = e == Args;
            return OneArgumentMethodCalled;
        }

        bool TwoArgumentsMethod(object sender, object e)
        {
            TwoArgumentMethodCalled = sender == Sender && e == Args;
            return TwoArgumentMethodCalled;
        }

        bool ThreeArgumentsMethod(object sender, object e, object param)
        {
            return false;
        }

        MethodInfo GetMethodInfo(string name) => GetType().GetMethod(name, BindingFlags.NonPublic | BindingFlags.Instance);

        [Example("When a method has no argument")]
        void Ex01()
        {
            Given("an EventHandlerAction that has a method whose parameter count is zero", () => Action = new EventHandlerAction(GetMethodInfo(nameof(NoArgumentMethod)), this));
            When("a sender and an event data are handled", () => Action.OnHandled(Sender, Args));
            Then("the specified method should be invoked", () => NoArgumentMethodCalled && !OneArgumentMethodCalled && !TwoArgumentMethodCalled);
            When("the EventHandlerAction handles a sender and an event data", () => Result = (bool)Action.Handle(Sender, Args));
            Then("the specified method should be invoked", () => Result);
        }

        [Example("When a method has one argument")]
        void Ex02()
        {
            Given("an EventHandlerAction that has a method whose parameter is an event data", () => Action = new EventHandlerAction(GetMethodInfo(nameof(OneArgumentMethod)), this));
            When("a sender and an event data are handled", () => Action.OnHandled(Sender, Args));
            Then("the specified method should be invoked", () => !NoArgumentMethodCalled && OneArgumentMethodCalled && !TwoArgumentMethodCalled);
            When("the EventHandlerAction handles a sender and an event data", () => Result = (bool)Action.Handle(Sender, Args));
            Then("the specified method should be invoked", () => Result);
        }

        [Example("When a method has two arguments")]
        void Ex03()
        {
            Given("an EventHandlerAction that has a method whose parameters are a sender object and an event data", () => Action = new EventHandlerAction(GetMethodInfo(nameof(TwoArgumentsMethod)), this));
            When("a sender and an event data are handled", () => Action.OnHandled(Sender, Args));
            Then("the specified method should be invoked", () => !NoArgumentMethodCalled && !OneArgumentMethodCalled && TwoArgumentMethodCalled);
            When("the EventHandlerAction handles a sender and an event data", () => Result = (bool)Action.Handle(Sender, Args));
            Then("the specified method should be invoked", () => Result);
        }

        [Example("When a method has three arguments")]
        void Ex04()
        {
            Given("an EventHandlerAction that has a method whose parameter count is three", () => Action = new EventHandlerAction(GetMethodInfo(nameof(ThreeArgumentsMethod)), this));
            When("a sender and an event data are handled", () => Action.OnHandled(Sender, Args));
            Then<InvalidOperationException>($"{typeof(InvalidOperationException)} should be thrown.");
            When("the EventHandlerAction handles a sender and an event data", () => Result = (bool)Action.Handle(Sender, Args));
            Then<InvalidOperationException>($"{typeof(InvalidOperationException)} should be thrown.");
        }
    }
}
