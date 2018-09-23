// Copyright (C) 2018 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System;
using System.Reflection;
using Carna;
using NSubstitute;

namespace Charites.Windows.Mvc
{
    [Specification("EventHandlerAction Spec")]
    class EventHandlerActionSpec : FixtureSteppable
    {
        EventHandlerAction Action { get; set; }

        Func<Exception, bool> UnhandledExceptionHandler { get; } = Substitute.For<Func<Exception, bool>>();

        object Sender { get; } = new object();
        object Args { get; } = new object();

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

        void NoArgumentExceptionMethod()
        {
            throw new Exception();
        }

        void OneArgumentExceptionMethod(object e)
        {
            throw new Exception();
        }

        void TwoArgumentsExceptionMethod(object sender, object e)
        {
            throw new Exception();
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

        [Example("When a method that has no argument throws an exception and it is handled")]
        void Ex05()
        {
            UnhandledExceptionHandler.Invoke(Arg.Any<TargetInvocationException>()).Returns(true);

            Given("an EventHandlerAction that has a method whose parameter count is zero", () => Action = new ExceptionHandlingEventHandlerAction(GetMethodInfo(nameof(NoArgumentExceptionMethod)), this, UnhandledExceptionHandler));
            When("a sender and an event data are handled", () => Action.OnHandled(Sender, Args));
            Then("the exception should not be thrown", () => true);
        }

        [Example("When a method that has no argument throws an exception and it is not handled")]
        void Ex06()
        {
            UnhandledExceptionHandler.Invoke(Arg.Any<TargetInvocationException>()).Returns(false);

            Given("an EventHandlerAction that has a method whose parameter count is zero", () => Action = new ExceptionHandlingEventHandlerAction(GetMethodInfo(nameof(NoArgumentExceptionMethod)), this, UnhandledExceptionHandler));
            When("a sender and an event data are handled", () => Action.OnHandled(Sender, Args));
            Then<TargetInvocationException>("the exception should be thrown");
        }

        [Example("When a method that has one argument throws an exception and it is handled")]
        void Ex07()
        {
            UnhandledExceptionHandler.Invoke(Arg.Any<TargetInvocationException>()).Returns(true);

            Given("an EventHandlerAction that has a method whose parameter count is zero", () => Action = new ExceptionHandlingEventHandlerAction(GetMethodInfo(nameof(OneArgumentExceptionMethod)), this, UnhandledExceptionHandler));
            When("a sender and an event data are handled", () => Action.OnHandled(Sender, Args));
            Then("the exception should not be thrown", () => true);
        }

        [Example("When a method that has one argument throws an exception and it is not handled")]
        void Ex08()
        {
            UnhandledExceptionHandler.Invoke(Arg.Any<TargetInvocationException>()).Returns(false);

            Given("an EventHandlerAction that has a method whose parameter count is zero", () => Action = new ExceptionHandlingEventHandlerAction(GetMethodInfo(nameof(OneArgumentExceptionMethod)), this, UnhandledExceptionHandler));
            When("a sender and an event data are handled", () => Action.OnHandled(Sender, Args));
            Then<TargetInvocationException>("the exception should be thrown");
        }

        [Example("When a method that has two arguments throws an exception and it is handled")]
        void Ex09()
        {
            UnhandledExceptionHandler.Invoke(Arg.Any<TargetInvocationException>()).Returns(true);

            Given("an EventHandlerAction that has a method whose parameter count is zero", () => Action = new ExceptionHandlingEventHandlerAction(GetMethodInfo(nameof(TwoArgumentsExceptionMethod)), this, UnhandledExceptionHandler));
            When("a sender and an event data are handled", () => Action.OnHandled(Sender, Args));
            Then("the exception should not be thrown", () => true);
        }

        [Example("When a method that has two arguments throws an exception and it is not handled")]
        void Ex10()
        {
            UnhandledExceptionHandler.Invoke(Arg.Any<TargetInvocationException>()).Returns(false);

            Given("an EventHandlerAction that has a method whose parameter count is zero", () => Action = new ExceptionHandlingEventHandlerAction(GetMethodInfo(nameof(TwoArgumentsExceptionMethod)), this, UnhandledExceptionHandler));
            When("a sender and an event data are handled", () => Action.OnHandled(Sender, Args));
            Then<TargetInvocationException>("the exception should be thrown");
        }
    }
}
