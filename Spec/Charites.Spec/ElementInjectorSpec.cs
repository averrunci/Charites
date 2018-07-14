// Copyright (C) 2018 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using Carna;

namespace Charites.Windows.Mvc
{
    [Specification("ElementInjector Spec")]
    class ElementInjectorSpec : FixtureSteppable
    {
        IElementInjector<TestElement> ElementInjector { get; } = new ElementInjectorTss();

        TestControllers.IElementAssertController Controller { get; set; }
        TestElement RootElement { get; }
        TestElement ChildElement1 { get; } = new TestElement("childElement1");
        TestElement ChildElement2 { get; } = new TestElement("ChildElement2");
        TestElement ChildElement3 { get; } = new TestElement("ChildElement3");

        public ElementInjectorSpec()
        {
            RootElement = new TestElement("Root", ChildElement1, ChildElement2, ChildElement3);
        }

        [Example("When elements are attributed to the field")]
        void Ex01()
        {
            Given("a controller that has elements attributed to the field", () => Controller = new TestControllers.ElementAttributedToFieldController());
            When("elements are injected to the controller", () => ElementInjector.Inject(RootElement, Controller));
            Then("the specified elements should be injected to the field of the controller", () => Controller.AssertElements(RootElement));
        }

        [Example("When elements are not attributed to the field")]
        void Ex02()
        {
            Given("a controller that does not have elements attributed to the field", () => Controller = new TestControllers.ElementNotAttributedToFieldController());
            When("elements are injected to the controller", () => ElementInjector.Inject(RootElement, Controller));
            Then("the specified elements should not be injected to the field of the controller", () => Controller.AssertElements(null));
        }

        [Example("When elements are attributed to the field and each element is injected separately")]
        void Ex03()
        {
            Given("a controller that has elements attributed to the field", () => Controller = new TestControllers.ElementAttributedToFieldController());
            When("each element is injected to the controller separately by setting foundElementOnly parameter to true", () =>
            {
                ElementInjector.Inject(ChildElement1, Controller, true);
                ElementInjector.Inject(ChildElement2, Controller, true);
                ElementInjector.Inject(ChildElement3, Controller, true);
            });
            Then("the specified elements should be injected to the field of the controller", () => Controller.AssertElements(RootElement));
        }

        [Example("When elements are attributed to the property")]
        void Ex04()
        {
            Given("a controller that has elements attributed to the property", () => Controller = new TestControllers.ElementAttributedToPropertyController());
            When("elements are injected to the controller", () => ElementInjector.Inject(RootElement, Controller));
            Then("the specified elements should be injected to the property of the controller", () => Controller.AssertElements(RootElement));
        }

        [Example("When elements are not attributed to the property")]
        void Ex05()
        {
            Given("a controller that does not have elements attributed to the property", () => Controller = new TestControllers.ElementNotAttributedToPropertyController());
            When("elements are injected to the controller", () => ElementInjector.Inject(RootElement, Controller));
            Then("the specified elements should not be injected to the property of the controller", () => Controller.AssertElements(null));
        }

        [Example("When elements are attributed to the read only property")]
        void Ex06()
        {
            Given("a controller that has elements attributed to the read only property", () => Controller = new TestControllers.ElementAttributedToReadOnlyPropertyController());
            When("elements are injected to the controller", () => ElementInjector.Inject(RootElement, Controller));
            Then<ElementInjectionException>($"{typeof(ElementInjectionException)} should be thrown");
        }

        [Example("When elements are attributed to the property and each element is injected separately")]
        void Ex07()
        {
            Given("a controller that has elements attributed to the property", () => Controller = new TestControllers.ElementAttributedToPropertyController());
            When("each element is injected to the controller separately by setting foundElementOnly parameter to true", () =>
            {
                ElementInjector.Inject(ChildElement1, Controller, true);
                ElementInjector.Inject(ChildElement2, Controller, true);
                ElementInjector.Inject(ChildElement3, Controller, true);
            });
            Then("the specified elements should be injected to the property of the controller", () => Controller.AssertElements(RootElement));
        }

        [Example("When elements are attributed to the method")]
        void Ex08()
        {
            Given("a controller that has elements attributed to the method", () => Controller = new TestControllers.ElementAttributedToMethodController());
            When("elements are injected to the controller", () => ElementInjector.Inject(RootElement, Controller));
            Then("the specified elements should be injected to the method of the controller", () => Controller.AssertElements(RootElement));
        }

        [Example("When elements are not attributed to the method")]
        void Ex09()
        {
            Given("a controller that does not have elements attributed to the method", () => Controller = new TestControllers.ElementNotAttributedToMethodController());
            When("elements are injected to the controller", () => ElementInjector.Inject(RootElement, Controller));
            Then("the specified elements should not be injected to the method of the controller", () => Controller.AssertElements(null));
        }

        [Example("When elements are attributed to the method whose parameter is wrong")]
        void Ex10()
        {
            Given("a controller that has elements attributed to the method whose parameter is wrong", () => Controller = new TestControllers.ElementAttributedToWrongArgumentMethodController());
            When("elements are injected to the controller", () => ElementInjector.Inject(RootElement, Controller));
            Then<ElementInjectionException>($"{typeof(ElementInjectionException)} should be thrown");
        }

        [Example("When elements are attributed to the method and each element is injected separately")]
        void Ex11()
        {
            Given("a controller that has elements attributed to the method", () => Controller = new TestControllers.ElementAttributedToMethodController());
            When("each element is injected to the controller separately by setting foundElementOnly parameter to true", () =>
            {
                ElementInjector.Inject(ChildElement1, Controller, true);
                ElementInjector.Inject(ChildElement2, Controller, true);
                ElementInjector.Inject(ChildElement3, Controller, true);
            });
            Then("the specified elements should be injected to the method of the controller", () => Controller.AssertElements(RootElement));
        }
    }
}
