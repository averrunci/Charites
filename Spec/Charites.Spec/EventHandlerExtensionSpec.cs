// Copyright (C) 2022 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using Carna;

namespace Charites.Windows.Mvc;

[Specification("EventHandlerExtension Spec")]
class EventHandlerExtensionSpec : FixtureSteppable
{
    IControllerExtension<TestElement> EventHandlerExtension { get; } = new EventHandlerExtensionTss();

    TestElement RootElement { get; } = new("RootElement");
    object Controller { get; set; } = default!;

    EventHandlerBase<TestElement, EventHandlerItemTss> EventHandlerBase { get; set; } = default!;

    Action NoArgumentAssertionHandler { get; }
    Action<object> OneArgumentAssertionHandler { get; }
    Action<object, object> TwoArgumentsAssertionHandler { get; }
    Action<TestControllers.IDependency1, TestControllers.IDependency2, TestControllers.IDependency3> DependencyArgumentsAssertionHandler { get; }

    bool NoArgumentHandlerCalled { get; set; }
    bool OneArgumentHandlerCalled { get; set; }
    bool TwoArgumentsHandlerCalled { get; set; }
    bool DependencyArgumentsHandlerCalled { get; set; }

    object? Sender { get; set; }
    object? Args { get; set; }

    TestControllers.IDependency1 Dependency1 { get; } = new TestControllers.Dependency1();
    TestControllers.IDependency2 Dependency2 { get; } = new TestControllers.Dependency2();
    TestControllers.IDependency3 Dependency3 { get; } = new TestControllers.Dependency3();


    public EventHandlerExtensionSpec()
    {
        NoArgumentAssertionHandler = () => NoArgumentHandlerCalled = true;
        OneArgumentAssertionHandler = e => OneArgumentHandlerCalled = e == Args;
        TwoArgumentsAssertionHandler = (sender, e) => TwoArgumentsHandlerCalled = sender == Sender && e == Args;
        DependencyArgumentsAssertionHandler = (dependency1, dependency2, dependency3) => DependencyArgumentsHandlerCalled = dependency1 == Dependency1 && dependency2 == Dependency2 && dependency3 == Dependency3;
    }

    bool AssertEventHandlerCalled(bool expectedNoArgumentHandlerCalled, bool expectedOneArgumentHandlerCalled, bool expectedTwoArgumentsHandlerCalled)
    {
        var result = NoArgumentHandlerCalled == expectedNoArgumentHandlerCalled && OneArgumentHandlerCalled == expectedOneArgumentHandlerCalled && TwoArgumentsHandlerCalled == expectedTwoArgumentsHandlerCalled;
        NoArgumentHandlerCalled = OneArgumentHandlerCalled = TwoArgumentsHandlerCalled = false;
        return result;
    }

    bool AssertEventHandlerCalled(bool expectedNoArgumentHandlerCalled, bool expectedOneArgumentHandlerCalled, bool expectedTwoArgumentsHandlerCalled, bool expectedDependencyArgumentsHandlerCalled)
    {
        var result = NoArgumentHandlerCalled == expectedNoArgumentHandlerCalled && OneArgumentHandlerCalled == expectedOneArgumentHandlerCalled && TwoArgumentsHandlerCalled == expectedTwoArgumentsHandlerCalled && DependencyArgumentsHandlerCalled == expectedDependencyArgumentsHandlerCalled;
        NoArgumentHandlerCalled = OneArgumentHandlerCalled = TwoArgumentsHandlerCalled = DependencyArgumentsHandlerCalled = false;
        return result;
    }

    [Example("When event handlers are attributed to the field")]
    void Ex01()
    {
        Given("a controller that has event handlers attributed to the field", () => Controller = new TestControllers.EventHandlerAttributedToFieldController(NoArgumentAssertionHandler, OneArgumentAssertionHandler, TwoArgumentsAssertionHandler));
        When("the controller is attached", () => EventHandlerExtension.Attach(Controller, RootElement));
        When("the EventHandlerBase is retrieved", () => EventHandlerBase = (EventHandlerBase<TestElement, EventHandlerItemTss>)EventHandlerExtension.Retrieve(Controller));

        When("the event is raised", () => EventHandlerBase.GetBy("Element1").Raise("Click"));
        Then("the event should be handled", () => AssertEventHandlerCalled(true, true, true));
        Args = new object();
        When("the event is raised with an event data", () => EventHandlerBase.GetBy("Element2").With(Args).Raise("Click"));
        Then("the event should be handled", () => AssertEventHandlerCalled(false, true ,true));
        Sender = new object();
        When("the event is raised with a sender object and event data", () => EventHandlerBase.GetBy("Element3").From(Sender).With(Args).Raise("Click"));
        Then("the event should be handled", () => AssertEventHandlerCalled(false, false, true));

        When("the controller is detached", () => EventHandlerExtension.Detach(Controller, RootElement));
        When("the EventHandlerBase is retrieved", () => EventHandlerBase = (EventHandlerBase<TestElement, EventHandlerItemTss>)EventHandlerExtension.Retrieve(Controller));

        Sender = null;
        Args = null;

        When("the event is raised", () => EventHandlerBase.GetBy("Element1").Raise("Click"));
        Then("the event should not be handled", () => AssertEventHandlerCalled(false, false, false));
        Args = new object();
        When("the event is raised with an event data", () => EventHandlerBase.GetBy("Element2").With(Args).Raise("Click"));
        Then("the event should not be handled", () => AssertEventHandlerCalled(false, false, false));
        Sender = new object();
        When("the event is raised with a sender object and event data", () => EventHandlerBase.GetBy("Element3").From(Sender).With(Args).Raise("Click"));
        Then("the event should not be handled", () => AssertEventHandlerCalled(false, false, false));
    }

    [Example("When event handlers are not attributed to the field")]
    void Ex02()
    {
        Given("a controller that does not have event handlers attributed to the field", () => Controller = new TestControllers.EventHandlerNotAttributedToFieldController(NoArgumentAssertionHandler, OneArgumentAssertionHandler, TwoArgumentsAssertionHandler));
        When("the controller is attached", () => EventHandlerExtension.Attach(Controller, RootElement));
        When("the EventHandlerBase is retrieved", () => EventHandlerBase = (EventHandlerBase<TestElement, EventHandlerItemTss>)EventHandlerExtension.Retrieve(Controller));

        When("the event is raised", () => EventHandlerBase.GetBy("Element1").Raise("Click"));
        Then("the event should not be handled", () => AssertEventHandlerCalled(false, false, false));
        Args = new object();
        When("the event is raised with an event data", () => EventHandlerBase.GetBy("Element2").With(Args).Raise("Click"));
        Then("the event should not be handled", () => AssertEventHandlerCalled(false, false, false));
        Sender = new object();
        When("the event is raised with a sender object and event data", () => EventHandlerBase.GetBy("Element3").From(Sender).With(Args).Raise("Click"));
        Then("the event should not be handled", () => AssertEventHandlerCalled(false, false, false));
    }

    [Example("When event handlers are attributed to the property")]
    void Ex03()
    {
        Given("a controller that has event handlers attributed to the property", () => Controller = new TestControllers.EventHandlerAttributedToPropertyController(NoArgumentAssertionHandler, OneArgumentAssertionHandler, TwoArgumentsAssertionHandler));
        When("the controller is attached", () => EventHandlerExtension.Attach(Controller, RootElement));
        When("the EventHandlerBase is retrieved", () => EventHandlerBase = (EventHandlerBase<TestElement, EventHandlerItemTss>)EventHandlerExtension.Retrieve(Controller));

        When("the event is raised", () => EventHandlerBase.GetBy("Element1").Raise("Click"));
        Then("the event should be handled", () => AssertEventHandlerCalled(true, true, true));
        Args = new object();
        When("the event is raised with an event data", () => EventHandlerBase.GetBy("Element2").With(Args).Raise("Click"));
        Then("the event should be handled", () => AssertEventHandlerCalled(false, true, true));
        Sender = new object();
        When("the event is raised with a sender object and event data", () => EventHandlerBase.GetBy("Element3").From(Sender).With(Args).Raise("Click"));
        Then("the event should be handled", () => AssertEventHandlerCalled(false, false, true));

        When("the controller is detached", () => EventHandlerExtension.Detach(Controller, RootElement));
        When("the EventHandlerBase is retrieved", () => EventHandlerBase = (EventHandlerBase<TestElement, EventHandlerItemTss>)EventHandlerExtension.Retrieve(Controller));

        Sender = null;
        Args = null;

        When("the event is raised", () => EventHandlerBase.GetBy("Element1").Raise("Click"));
        Then("the event should not be handled", () => AssertEventHandlerCalled(false, false, false));
        Args = new object();
        When("the event is raised with an event data", () => EventHandlerBase.GetBy("Element2").With(Args).Raise("Click"));
        Then("the event should not be handled", () => AssertEventHandlerCalled(false, false, false));
        Sender = new object();
        When("the event is raised with a sender object and event data", () => EventHandlerBase.GetBy("Element3").From(Sender).With(Args).Raise("Click"));
        Then("the event should not be handled", () => AssertEventHandlerCalled(false, false, false));
    }

    [Example("When event handlers are not attributed to the property")]
    void Ex04()
    {
        Given("a controller that does not have event handlers attributed to the property", () => Controller = new TestControllers.EventHandlerNotAttributedToPropertyController(NoArgumentAssertionHandler, OneArgumentAssertionHandler, TwoArgumentsAssertionHandler));
        When("the controller is attached", () => EventHandlerExtension.Attach(Controller, RootElement));
        When("the EventHandlerBase is retrieved", () => EventHandlerBase = (EventHandlerBase<TestElement, EventHandlerItemTss>)EventHandlerExtension.Retrieve(Controller));

        When("the event is raised", () => EventHandlerBase.GetBy("Element1").Raise("Click"));
        Then("the event should not be handled", () => AssertEventHandlerCalled(false, false, false));
        Args = new object();
        When("the event is raised with an event data", () => EventHandlerBase.GetBy("Element2").With(Args).Raise("Click"));
        Then("the event should not be handled", () => AssertEventHandlerCalled(false, false, false));
        Sender = new object();
        When("the event is raised with a sender object and event data", () => EventHandlerBase.GetBy("Element3").From(Sender).With(Args).Raise("Click"));
        Then("the event should not be handled", () => AssertEventHandlerCalled(false, false, false));
    }

    [Example("When event handlers are attributed to the write only property")]
    void Ex05()
    {
        Given("a controller that have event handlers attributed to the write only property", () => Controller = new TestControllers.EventHandlerAttributedToWriteOnlyPropertyController(NoArgumentAssertionHandler, OneArgumentAssertionHandler, TwoArgumentsAssertionHandler));
        When("the controller is attached", () => EventHandlerExtension.Attach(Controller, RootElement));
        When("the EventHandlerBase is retrieved", () => EventHandlerBase = (EventHandlerBase<TestElement, EventHandlerItemTss>)EventHandlerExtension.Retrieve(Controller));

        When("the event is raised", () => EventHandlerBase.GetBy("Element1").Raise("Click"));
        Then("the event should not be handled", () => AssertEventHandlerCalled(false, false, false));
        Args = new object();
        When("the event is raised with an event data", () => EventHandlerBase.GetBy("Element2").With(Args).Raise("Click"));
        Then("the event should not be handled", () => AssertEventHandlerCalled(false, false, false));
        Sender = new object();
        When("the event is raised with a sender object and event data", () => EventHandlerBase.GetBy("Element3").From(Sender).With(Args).Raise("Click"));
        Then("the event should not be handled", () => AssertEventHandlerCalled(false, false, false));
    }

    [Example("When event handlers are attributed to the method")]
    void Ex06()
    {
        Given("a controller that has event handlers attributed to the method", () => Controller = new TestControllers.EventHandlerAttributedToMethodController(NoArgumentAssertionHandler, OneArgumentAssertionHandler, TwoArgumentsAssertionHandler));
        When("the controller is attached", () => EventHandlerExtension.Attach(Controller, RootElement));
        When("the EventHandlerBase is retrieved", () => EventHandlerBase = (EventHandlerBase<TestElement, EventHandlerItemTss>)EventHandlerExtension.Retrieve(Controller));

        When("the event is raised", () => EventHandlerBase.GetBy("Element1").Raise("Click"));
        Then("the event should be handled", () => AssertEventHandlerCalled(true, true, true));
        Args = new object();
        When("the event is raised with an event data", () => EventHandlerBase.GetBy("Element2").With(Args).Raise("Click"));
        Then("the event should be handled", () => AssertEventHandlerCalled(false, true, true));
        Sender = new object();
        When("the event is raised with a sender object and event data", () => EventHandlerBase.GetBy("Element3").From(Sender).With(Args).Raise("Click"));
        Then("the event should be handled", () => AssertEventHandlerCalled(false, false, true));

        When("the controller is detached", () => EventHandlerExtension.Detach(Controller, RootElement));
        When("the EventHandlerBase is retrieved", () => EventHandlerBase = (EventHandlerBase<TestElement, EventHandlerItemTss>)EventHandlerExtension.Retrieve(Controller));

        Sender = null;
        Args = null;

        When("the event is raised", () => EventHandlerBase.GetBy("Element1").Raise("Click"));
        Then("the event should not be handled", () => AssertEventHandlerCalled(false, false, false));
        Args = new object();
        When("the event is raised with an event data", () => EventHandlerBase.GetBy("Element2").With(Args).Raise("Click"));
        Then("the event should not be handled", () => AssertEventHandlerCalled(false, false, false));
        Sender = new object();
        When("the event is raised with a sender object and event data", () => EventHandlerBase.GetBy("Element3").From(Sender).With(Args).Raise("Click"));
        Then("the event should not be handled", () => AssertEventHandlerCalled(false, false, false));
    }

    [Example("When event handlers are not attributed to the method")]
    void Ex07()
    {
        Given("a controller that does not have event handlers attributed to the method", () => Controller = new TestControllers.EventHandlerNotAttributedToMethodController(NoArgumentAssertionHandler, OneArgumentAssertionHandler, TwoArgumentsAssertionHandler));
        When("the controller is attached", () => EventHandlerExtension.Attach(Controller, RootElement));
        When("the EventHandlerBase is retrieved", () => EventHandlerBase = (EventHandlerBase<TestElement, EventHandlerItemTss>)EventHandlerExtension.Retrieve(Controller));

        When("the event is raised", () => EventHandlerBase.GetBy("Element1").Raise("Click"));
        Then("the event should not be handled", () => AssertEventHandlerCalled(false, false, false));
        Args = new object();
        When("the event is raised with an event data", () => EventHandlerBase.GetBy("Element2").With(Args).Raise("Click"));
        Then("the event should not be handled", () => AssertEventHandlerCalled(false, false, false));
        Sender = new object();
        When("the event is raised with a sender object and event data", () => EventHandlerBase.GetBy("Element3").From(Sender).With(Args).Raise("Click"));
        Then("the event should not be handled", () => AssertEventHandlerCalled(false, false, false));
    }

    [Example("When event handlers are attributed to the method whose parameter is wrong")]
    void Ex08()
    {
        Given("a controller that has event handlers attributed to the method whose parameter is wrong", () => Controller = new TestControllers.EventHandlerAttributedToWrongArgumentMethodController(NoArgumentAssertionHandler, OneArgumentAssertionHandler, TwoArgumentsAssertionHandler));
        When("the controller is attached", () => EventHandlerExtension.Attach(Controller, RootElement));
        When("the EventHandlerBase is retrieved", () => EventHandlerBase = (EventHandlerBase<TestElement, EventHandlerItemTss>)EventHandlerExtension.Retrieve(Controller));

        When("the event is raised", () => EventHandlerBase.GetBy("Element1").Raise("Click"));
        Then<InvalidOperationException>($"{typeof(InvalidOperationException)} should be thrown", exc => exc.GetType() == typeof(InvalidOperationException));
    }

    [Example("When event handlers are methods using a naming convention")]
    void Ex09()
    {
        Given("a controller that has event handlers of the method using a naming convention", () => Controller = new TestControllers.EventHandlerOfMethodUsingNamingConventionController(NoArgumentAssertionHandler, OneArgumentAssertionHandler, TwoArgumentsAssertionHandler));
        When("the controller is attached", () => EventHandlerExtension.Attach(Controller, RootElement));
        When("the EventHandlerBase is retrieved", () => EventHandlerBase = (EventHandlerBase<TestElement, EventHandlerItemTss>)EventHandlerExtension.Retrieve(Controller));

        When("the event is raised", () => EventHandlerBase.GetBy("Element1").Raise("Click"));
        Then("the event should be handled", () => AssertEventHandlerCalled(true, true, true));
        Args = new object();
        When("the event is raised with an event data", () => EventHandlerBase.GetBy("Element2").With(Args).Raise("Click"));
        Then("the event should be handled", () => AssertEventHandlerCalled(false, true, true));
        Sender = new object();
        When("the event is raised with a sender object and event data", () => EventHandlerBase.GetBy("Element3").From(Sender).With(Args).Raise("Click"));
        Then("the event should be handled", () => AssertEventHandlerCalled(false, false, true));

        When("the controller is detached", () => EventHandlerExtension.Detach(Controller, RootElement));
        When("the EventHandlerBase is retrieved", () => EventHandlerBase = (EventHandlerBase<TestElement, EventHandlerItemTss>)EventHandlerExtension.Retrieve(Controller));

        Sender = null;
        Args = null;

        When("the event is raised", () => EventHandlerBase.GetBy("Element1").Raise("Click"));
        Then("the event should not be handled", () => AssertEventHandlerCalled(false, false, false));
        Args = new object();
        When("the event is raised with an event data", () => EventHandlerBase.GetBy("Element2").With(Args).Raise("Click"));
        Then("the event should not be handled", () => AssertEventHandlerCalled(false, false, false));
        Sender = new object();
        When("the event is raised with a sender object and event data", () => EventHandlerBase.GetBy("Element3").From(Sender).With(Args).Raise("Click"));
        Then("the event should not be handled", () => AssertEventHandlerCalled(false, false, false));
    }

    [Example("When event handlers are async methods using a naming convention")]
    void Ex10()
    {
        Given("a controller that has event handlers of the async method using a naming convention", () => Controller = new TestControllers.EventHandlerOfAsyncMethodUsingNamingConventionController(NoArgumentAssertionHandler, OneArgumentAssertionHandler, TwoArgumentsAssertionHandler));
        When("the controller is attached", () => EventHandlerExtension.Attach(Controller, RootElement));
        When("the EventHandlerBase is retrieved", () => EventHandlerBase = (EventHandlerBase<TestElement, EventHandlerItemTss>)EventHandlerExtension.Retrieve(Controller));

        When("the event is raised", async () => await EventHandlerBase.GetBy("Element1").RaiseAsync("Click"));
        Then("the event should be handled", () => AssertEventHandlerCalled(true, true, true));
        Args = new object();
        When("the event is raised with an event data", async () => await EventHandlerBase.GetBy("Element2").With(Args).RaiseAsync("Click"));
        Then("the event should be handled", () => AssertEventHandlerCalled(false, true, true));
        Sender = new object();
        When("the event is raised with a sender object and event data", async () => await EventHandlerBase.GetBy("Element3").From(Sender).With(Args).RaiseAsync("Click"));
        Then("the event should be handled", () => AssertEventHandlerCalled(false, false, true));

        When("the controller is detached", () => EventHandlerExtension.Detach(Controller, RootElement));
        When("the EventHandlerBase is retrieved", () => EventHandlerBase = (EventHandlerBase<TestElement, EventHandlerItemTss>)EventHandlerExtension.Retrieve(Controller));

        Sender = null;
        Args = null;

        When("the event is raised", async () => await EventHandlerBase.GetBy("Element1").RaiseAsync("Click"));
        Then("the event should not be handled", () => AssertEventHandlerCalled(false, false, false));
        Args = new object();
        When("the event is raised with an event data", async () => await EventHandlerBase.GetBy("Element2").With(Args).RaiseAsync("Click"));
        Then("the event should not be handled", () => AssertEventHandlerCalled(false, false, false));
        Sender = new object();
        When("the event is raised with a sender object and event data", async () => await EventHandlerBase.GetBy("Element3").From(Sender).With(Args).RaiseAsync("Click"));
        Then("the event should not be handled", () => AssertEventHandlerCalled(false, false, false));
    }

    [Example("When event handlers are methods using a wrong naming convention")]
    void Ex11()
    {
        Given("a controller that has event handlers of the method using a wrong naming convention", () => Controller = new TestControllers.EventHandlerOfMethodUsingWrongNamingConventionController(NoArgumentAssertionHandler, OneArgumentAssertionHandler, TwoArgumentsAssertionHandler));
        When("the controller is attached", () => EventHandlerExtension.Attach(Controller, RootElement));
        When("the EventHandlerBase is retrieved", () => EventHandlerBase = (EventHandlerBase<TestElement, EventHandlerItemTss>)EventHandlerExtension.Retrieve(Controller));

        When("the event is raised", () => EventHandlerBase.GetBy("Element1").Raise("Click"));
        Then("the event should not be handled", () => AssertEventHandlerCalled(false, false, false));
        Args = new object();
        When("the event is raised with an event data", () => EventHandlerBase.GetBy("Element2").With(Args).Raise("Click"));
        Then("the event should not be handled", () => AssertEventHandlerCalled(false, false, false));
        Sender = new object();
        When("the event is raised with a sender object and event data", () => EventHandlerBase.GetBy("Element3").From(Sender).With(Args).Raise("Click"));
        Then("the event should not be handled", () => AssertEventHandlerCalled(false, false, false));
    }

    [Example("When event handlers are methods whose parameter are wrong using a naming convention")]
    void Ex12()
    {
        Given("a controller that has event handlers of the method whose parameter is wrong using a naming convention", () => Controller = new TestControllers.EventHandlerOfWrongArgumentMethodUsingNamingConventionController(NoArgumentAssertionHandler, OneArgumentAssertionHandler, TwoArgumentsAssertionHandler));
        When("the controller is attached", () => EventHandlerExtension.Attach(Controller, RootElement));
        When("the EventHandlerBase is retrieved", () => EventHandlerBase = (EventHandlerBase<TestElement, EventHandlerItemTss>)EventHandlerExtension.Retrieve(Controller));

        When("the event is raised", () => EventHandlerBase.GetBy("Element1").Raise("Click"));
        Then<InvalidOperationException>($"{typeof(InvalidOperationException)} should be thrown", exc => exc.GetType() == typeof(InvalidOperationException));
    }

    [Example("When event handlers are methods that have dependency parameters")]
    void Ex13()
    {
        Given("a controller that has event handlers of the method that have dependency parameters", () => Controller = new TestControllers.EventHandlerOfMethodWithDependencyParametersController(NoArgumentAssertionHandler, OneArgumentAssertionHandler, TwoArgumentsAssertionHandler, DependencyArgumentsAssertionHandler));
        When("the controller is attached", () => EventHandlerExtension.Attach(Controller, RootElement));
        When("the EventHandlerBase is retrieved", () => EventHandlerBase = (EventHandlerBase<TestElement, EventHandlerItemTss>)EventHandlerExtension.Retrieve(Controller));

        When("the event is raised", () =>
            EventHandlerBase.GetBy("Element1")
                .Resolve<TestControllers.IDependency1>(() => Dependency1)
                .Resolve<TestControllers.IDependency2>(() => Dependency2)
                .Resolve<TestControllers.IDependency3>(() => Dependency3)
                .Raise("Click")
        );
        Then("the event should be handled", () => AssertEventHandlerCalled(true, false, false, true));
        When("the event is raised asynchronously", async () =>
            await EventHandlerBase.GetBy("Element4")
                .Resolve<TestControllers.IDependency1>(() => Dependency1)
                .Resolve<TestControllers.IDependency2>(() => Dependency2)
                .Resolve<TestControllers.IDependency3>(() => Dependency3)
                .RaiseAsync("Click")
        );
        Then("the event should be handled", () => AssertEventHandlerCalled(true, false, false, true));

        Args = new object();
        When("the event is raised with an event data", () =>
            EventHandlerBase.GetBy("Element2")
                .With(Args)
                .Resolve<TestControllers.IDependency1>(() => Dependency1)
                .Resolve<TestControllers.IDependency2>(() => Dependency2)
                .Resolve<TestControllers.IDependency3>(() => Dependency3)
                .Raise("Click")
        );
        Then("the event should be handled", () => AssertEventHandlerCalled(false, true, false, true));
        When("the event is raised with an event data asynchronously", async () =>
            await EventHandlerBase.GetBy("Element5")
                .With(Args)
                .Resolve<TestControllers.IDependency1>(() => Dependency1)
                .Resolve<TestControllers.IDependency2>(() => Dependency2)
                .Resolve<TestControllers.IDependency3>(() => Dependency3)
                .RaiseAsync("Click")
        );
        Then("the event should be handled", () => AssertEventHandlerCalled(false, true, false, true));

        Sender = new object();
        When("the event is raised with a sender object and event data", () =>
            EventHandlerBase.GetBy("Element3")
                .From(Sender)
                .With(Args)
                .Resolve<TestControllers.IDependency1>(() => Dependency1)
                .Resolve<TestControllers.IDependency2>(() => Dependency2)
                .Resolve<TestControllers.IDependency3>(() => Dependency3)
                .Raise("Click")
        );
        Then("the event should be handled", () => AssertEventHandlerCalled(false, false, true, true));
        When("the event is raised with a sender object and event data asynchronously", async () =>
            await EventHandlerBase.GetBy("Element6")
                .From(Sender)
                .With(Args)
                .Resolve<TestControllers.IDependency1>(() => Dependency1)
                .Resolve<TestControllers.IDependency2>(() => Dependency2)
                .Resolve<TestControllers.IDependency3>(() => Dependency3)
                .RaiseAsync("Click")
        );
        Then("the event should be handled", () => AssertEventHandlerCalled(false, false, true, true));

        When("the controller is detached", () => EventHandlerExtension.Detach(Controller, RootElement));
        When("the EventHandlerBase is retrieved", () => EventHandlerBase = (EventHandlerBase<TestElement, EventHandlerItemTss>)EventHandlerExtension.Retrieve(Controller));

        Sender = null;
        Args = null;

        When("the event is raised", () =>
            EventHandlerBase.GetBy("Element1")
                .Resolve<TestControllers.IDependency1>(() => Dependency1)
                .Resolve<TestControllers.IDependency2>(() => Dependency2)
                .Resolve<TestControllers.IDependency3>(() => Dependency3)
                .Raise("Click")
        );
        Then("the event should not be handled", () => AssertEventHandlerCalled(false, false, false, false));
        When("the event is raised asynchronously", async () =>
            await EventHandlerBase.GetBy("Element4")
                .Resolve<TestControllers.IDependency1>(() => Dependency1)
                .Resolve<TestControllers.IDependency2>(() => Dependency2)
                .Resolve<TestControllers.IDependency3>(() => Dependency3)
                .RaiseAsync("Click")
        );
        Then("the event should not be handled", () => AssertEventHandlerCalled(false, false, false, false));

        Args = new object();
        When("the event is raised with an event data", () =>
            EventHandlerBase.GetBy("Element2")
                .With(Args)
                .Resolve<TestControllers.IDependency1>(() => Dependency1)
                .Resolve<TestControllers.IDependency2>(() => Dependency2)
                .Resolve<TestControllers.IDependency3>(() => Dependency3)
                .Raise("Click")
        );
        Then("the event should not be handled", () => AssertEventHandlerCalled(false, false, false, false));
        When("the event is raised with an event data asynchronously", async () =>
            await EventHandlerBase.GetBy("Element5")
                .With(Args)
                .Resolve<TestControllers.IDependency1>(() => Dependency1)
                .Resolve<TestControllers.IDependency2>(() => Dependency2)
                .Resolve<TestControllers.IDependency3>(() => Dependency3)
                .RaiseAsync("Click")
        );
        Then("the event should not be handled", () => AssertEventHandlerCalled(false, false, false, false));

        Sender = new object();
        When("the event is raised with a sender object and event data", () =>
            EventHandlerBase.GetBy("Element3")
                .From(Sender)
                .With(Args)
                .Resolve<TestControllers.IDependency1>(() => Dependency1)
                .Resolve<TestControllers.IDependency2>(() => Dependency2)
                .Resolve<TestControllers.IDependency3>(() => Dependency3)
                .Raise("Click")
        );
        Then("the event should not be handled", () => AssertEventHandlerCalled(false, false, false, false));
        When("the event is raised with a sender object and event data asynchronously", async () =>
            await EventHandlerBase.GetBy("Element6")
                .From(Sender)
                .With(Args)
                .Resolve<TestControllers.IDependency1>(() => Dependency1)
                .Resolve<TestControllers.IDependency2>(() => Dependency2)
                .Resolve<TestControllers.IDependency3>(() => Dependency3)
                .RaiseAsync("Click")
        );
        Then("the event should not be handled", () => AssertEventHandlerCalled(false, false, false, false));
    }
}