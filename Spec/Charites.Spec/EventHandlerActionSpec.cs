// Copyright (C) 2022 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System.Reflection;
using Carna;
using NSubstitute;

namespace Charites.Windows.Mvc;

[Specification("EventHandlerAction Spec")]
class EventHandlerActionSpec : FixtureSteppable
{
    EventHandlerAction Action { get; set; } = default!;

    Func<Exception, bool> UnhandledExceptionHandler { get; } = Substitute.For<Func<Exception, bool>>();

    object Sender { get; } = new();
    object Args { get; } = new();

    bool NoArgumentMethodCalled { get; set; }
    bool OneArgumentMethodCalled { get; set; }
    bool TwoArgumentsMethodCalled { get; set; }
    bool DIParametersMethodCalled { get; set; }
    bool OneArgumentDIParametersMethodCalled { get; set; }
    bool TwoArgumentsDIParametersMethodCalled { get; set; }
    bool Result { get; set; }

    IDictionary<Type, Func<object?>> DependencyResolver { get; } = new Dictionary<Type, Func<object?>>
    {
        [typeof(IDependency1)] = () => new Dependency1Implementation(),
        [typeof(IDependency2)] = () => new Dependency2Implementation(),
        [typeof(IDependency3)] = () => new Dependency3Implementation()
    };

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
        TwoArgumentsMethodCalled = sender == Sender && e == Args;
        return TwoArgumentsMethodCalled;
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

    bool DIParametersMethod([FromDI] IDependency1 dependency1, [FromDI] IDependency2 dependency2, [FromDI] IDependency3 dependency3)
    {
        DIParametersMethodCalled = dependency1.GetType() == typeof(Dependency1Implementation) &&
            dependency2.GetType() == typeof(Dependency2Implementation) &&
            dependency3.GetType() == typeof(Dependency3Implementation);
        return DIParametersMethodCalled;
    }

    bool OneArgumentDIParametersMethod([FromDI] IDependency1 dependency1, object e, [FromDI] IDependency2 dependency2, [FromDI] IDependency3 dependency3)
    {
        OneArgumentDIParametersMethodCalled = dependency1.GetType() == typeof(Dependency1Implementation) &&
            dependency2.GetType() == typeof(Dependency2Implementation) &&
            dependency3.GetType() == typeof(Dependency3Implementation) &&
            e == Args;
        return OneArgumentDIParametersMethodCalled;
    }

    bool TwoArgumentsDIParametersMethod([FromDI] IDependency1 dependency1, object sender, [FromDI] IDependency2 dependency2, object e, [FromDI] IDependency3 dependency3)
    {
        TwoArgumentsDIParametersMethodCalled = dependency1.GetType() == typeof(Dependency1Implementation) &&
            dependency2.GetType() == typeof(Dependency2Implementation) &&
            dependency3.GetType() == typeof(Dependency3Implementation) &&
            sender == Sender && e == Args;
        return TwoArgumentsDIParametersMethodCalled;
    }

    MethodInfo GetMethodInfo(string name) => GetType().GetMethod(name, BindingFlags.NonPublic | BindingFlags.Instance) ?? throw new InvalidOperationException();

    interface IDependency1 { }
    interface IDependency2 { }
    interface IDependency3 { }
    class Dependency1Implementation : IDependency1 { }
    class Dependency2Implementation : IDependency2 { }
    class Dependency3Implementation : IDependency3 { }

    [Example("When a method has no argument")]
    void Ex01()
    {
        Given("an EventHandlerAction that has a method whose parameter count is zero", () => Action = new EventHandlerAction(GetMethodInfo(nameof(NoArgumentMethod)), this));
        When("a sender and an event data are handled", () => Action.OnHandled(Sender, Args));
        Then("the specified method should be invoked", () => NoArgumentMethodCalled && !OneArgumentMethodCalled && !TwoArgumentsMethodCalled);
        When("the EventHandlerAction handles a sender and an event data", () => Result = (bool)(Action.Handle(Sender, Args) ?? false));
        Then("the specified method should be invoked", () => Result);
    }

    [Example("When a method has one argument")]
    void Ex02()
    {
        Given("an EventHandlerAction that has a method whose parameter is an event data", () => Action = new EventHandlerAction(GetMethodInfo(nameof(OneArgumentMethod)), this));
        When("a sender and an event data are handled", () => Action.OnHandled(Sender, Args));
        Then("the specified method should be invoked", () => !NoArgumentMethodCalled && OneArgumentMethodCalled && !TwoArgumentsMethodCalled);
        When("the EventHandlerAction handles a sender and an event data", () => Result = (bool)(Action.Handle(Sender, Args) ?? false));
        Then("the specified method should be invoked", () => Result);
    }

    [Example("When a method has two arguments")]
    void Ex03()
    {
        Given("an EventHandlerAction that has a method whose parameters are a sender object and an event data", () => Action = new EventHandlerAction(GetMethodInfo(nameof(TwoArgumentsMethod)), this));
        When("a sender and an event data are handled", () => Action.OnHandled(Sender, Args));
        Then("the specified method should be invoked", () => !NoArgumentMethodCalled && !OneArgumentMethodCalled && TwoArgumentsMethodCalled);
        When("the EventHandlerAction handles a sender and an event data", () => Result = (bool)(Action.Handle(Sender, Args) ?? false));
        Then("the specified method should be invoked", () => Result);
    }

    [Example("When a method has three arguments")]
    void Ex04()
    {
        Given("an EventHandlerAction that has a method whose parameter count is three", () => Action = new EventHandlerAction(GetMethodInfo(nameof(ThreeArgumentsMethod)), this));
        When("a sender and an event data are handled", () => Action.OnHandled(Sender, Args));
        Then<InvalidOperationException>($"{typeof(InvalidOperationException)} should be thrown.");
        When("the EventHandlerAction handles a sender and an event data", () => Result = (bool)(Action.Handle(Sender, Args) ?? false));
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

    [Example("When a method has DI parameters")]
    void Ex11()
    {
        Given("an EventHandlerAction that has a method whose parameters are DI parameters", () => Action = new DependencyInjectionEventHandlerAction(GetMethodInfo(nameof(DIParametersMethod)), this, DependencyResolver));
        When("a sender and an event data are handled", () => Action.OnHandled(Sender, Args));
        Then("the specified method should be invoked", () => !NoArgumentMethodCalled && !OneArgumentMethodCalled && !TwoArgumentsMethodCalled && DIParametersMethodCalled);
        When("the EventHandlerAction handles a sender and an event data", () => Result = (bool)(Action.Handle(Sender, Args) ?? false));
        Then("the specified method should be invoked", () => Result);
    }

    [Example("When a method has one argument and DI parameters")]
    void Ex12()
    {
        Given("an EventHandlerAction that has a method whose parameters are one argument and DI parameters", () => Action = new DependencyInjectionEventHandlerAction(GetMethodInfo(nameof(OneArgumentDIParametersMethod)), this, DependencyResolver));
        When("a sender and an event data are handled", () => Action.OnHandled(Sender, Args));
        Then("the specified method should be invoked", () => !NoArgumentMethodCalled && !OneArgumentMethodCalled && !TwoArgumentsMethodCalled && OneArgumentDIParametersMethodCalled);
        When("the EventHandlerAction handles a sender and an event data", () => Result = (bool)(Action.Handle(Sender, Args) ?? false));
        Then("the specified method should be invoked", () => Result);
    }

    [Example("When a method has two arguments and DI parameters")]
    void Ex13()
    {
        Given("an EventHandlerAction that has a method whose parameters are two arguments and DI parameters", () => Action = new DependencyInjectionEventHandlerAction(GetMethodInfo(nameof(TwoArgumentsDIParametersMethod)), this, DependencyResolver));
        When("a sender and an event data are handled", () => Action.OnHandled(Sender, Args));
        Then("the specified method should be invoked", () => !NoArgumentMethodCalled && !OneArgumentMethodCalled && !TwoArgumentsMethodCalled && TwoArgumentsDIParametersMethodCalled);
        When("the EventHandlerAction handles a sender and an event data", () => Result = (bool)(Action.Handle(Sender, Args) ?? false));
        Then("the specified method should be invoked", () => Result);
    }
}