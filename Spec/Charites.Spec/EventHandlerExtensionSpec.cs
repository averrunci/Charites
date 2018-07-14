﻿// Copyright (C) 2018 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System;
using System.Reflection;
using Carna;

namespace Charites.Windows.Mvc
{
    [Specification("EventHandlerExtension Spec")]
    class EventHandlerExtensionSpec : FixtureSteppable
    {
        IControllerExtension<TestElement> EventHandlerExtension { get; } = new EventHandlerExtensionTss();

        TestElement RootElement { get; } = new TestElement("RootElement");
        object Controller { get; set; }

        EventHandlerBase<TestElement, EventHandlerItemTss> EventHandlerBase { get; set; }

        Action NoArgumentAssertionHandler { get; }
        Action<object> OneArgumentAssertionHandler { get; }
        Action<object, object> TwoArgumentsAssertionHandler { get; }

        bool NoArgumentHandlerCalled { get; set; }
        bool OneArgumentHandlerCalled { get; set; }
        bool TwoArgumentsHandlerCalled { get; set; }

        object Sender { get; set; }
        object Args { get; set; }

        public EventHandlerExtensionSpec()
        {
            NoArgumentAssertionHandler = () => NoArgumentHandlerCalled = true;
            OneArgumentAssertionHandler = e => OneArgumentHandlerCalled = e == Args;
            TwoArgumentsAssertionHandler = (sender, e) => TwoArgumentsHandlerCalled = sender == Sender && e == Args;
        }

        bool AssertEventHandlerCalled(bool expectedNoArgumentHandlerCalled, bool expectedOneArgumentHandlerCalled, bool expectedTwoArgumentsHandlerCalled)
        {
            var result = NoArgumentHandlerCalled == expectedNoArgumentHandlerCalled && OneArgumentHandlerCalled == expectedOneArgumentHandlerCalled && TwoArgumentsHandlerCalled == expectedTwoArgumentsHandlerCalled;
            NoArgumentHandlerCalled = OneArgumentHandlerCalled = TwoArgumentsHandlerCalled = false;
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
            Then<TargetInvocationException>($"{typeof(InvalidOperationException)} should be thrown", exc => exc.InnerException.GetType() == typeof(InvalidOperationException));
        }
    }
}
