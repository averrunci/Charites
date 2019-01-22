// Copyright (C) 2018-2019 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using Carna;

namespace Charites.Windows.Mvc
{
    [Specification("DataContextInjector Spec")]
    class DataContextInjectorSpec : FixtureSteppable
    {
        IDataContextInjector DataContextInjector { get; } = new DataContextInjector();

        TestControllers.IDataContextAssertController Controller { get; set; }
        object DataContext { get; } = new object();

        [Example("When a data context is attributed to the field")]
        void Ex01()
        {
            Given("a controller that has a data context attributed to the field", () => Controller = new TestControllers.DataContextAttributedToFieldController());
            When("a data context is injected to the controller", () => DataContextInjector.Inject(DataContext, Controller));
            Then("the specified data context should be injected to the field of the controller", () => Controller.AssertDataContext(DataContext));
        }

        [Example("When a data context is not attributed to the field")]
        void Ex02()
        {
            Given("a controller that does not have a data context attributed to the field", () => Controller = new TestControllers.DataContextNotAttributedToFieldController());
            When("a data context is injected to the controller", () => DataContextInjector.Inject(DataContext, Controller));
            Then("the specified data context should not be injected to the field of the controller", () => Controller.AssertDataContext(null));
        }

        [Example("When a data context is attributed to the property")]
        void Ex03()
        {
            Given("a controller that has a data context attributed to the property", () => Controller = new TestControllers.DataContextAttributedToPropertyController());
            When("a data context is injected to the controller", () => DataContextInjector.Inject(DataContext, Controller));
            Then("the specified data context should be injected to the property of the controller", () => Controller.AssertDataContext(DataContext));
        }

        [Example("When a data context is not attributed to the property")]
        void Ex04()
        {
            Given("a controller that does not have a data context attributed to the property", () => Controller = new TestControllers.DataContextNotAttributedToPropertyController());
            When("a data context is injected to the controller", () => DataContextInjector.Inject(DataContext, Controller));
            Then("the specified data context should not be injected to the property of the controller", () => Controller.AssertDataContext(null));
        }

        [Example("When a data context is attributed to the read only property")]
        void Ex05()
        {
            Given("a controller that has a data context attributed to the read only property", () => Controller = new TestControllers.DataContextAttributedToReadOnlyPropertyController());
            When("a data context is injected to the controller", () => DataContextInjector.Inject(DataContext, Controller));
            Then<DataContextInjectionException>($"{typeof(DataContextInjectionException)} should be thrown");
        }

        [Example("When a data context is attributed to the method")]
        void Ex06()
        {
            Given("a controller that has a data context attributed to the method", () => Controller = new TestControllers.DataContextAttributedToMethodController());
            When("a data context is injected to the controller", () => DataContextInjector.Inject(DataContext, Controller));
            Then("the specified data context should be injected to the method of the controller", () => Controller.AssertDataContext(DataContext));
        }

        [Example("When a data context is not attributed to the method")]
        void Ex07()
        {
            Given("a controller that does not have a data context attributed to the method", () => Controller = new TestControllers.DataContextNotAttributedToMethodController());
            When("a data context is injected to the controller", () => DataContextInjector.Inject(DataContext, Controller));
            Then("the specified data context should not be injected to the method of the controller", () => Controller.AssertDataContext(null));
        }

        [Example("When a data context is attributed to the method whose parameter is wrong")]
        void Ex08()
        {
            Given("a controller that has a data context attributed to the method whose parameter is wrong", () => Controller = new TestControllers.DataContextAttributedToWrongArgumentMethodController());
            When("a data context is injected to the controller", () => DataContextInjector.Inject(DataContext, Controller));
            Then<DataContextInjectionException>($"{typeof(DataContextInjectionException)} should be thrown");
        }

        [Example("When a data context is specified by the method using a naming convention")]
        void Ex09()
        {
            Given("a controller that has a data context specified by the method using a naming convention", () => Controller = new TestControllers.DataContextSpecifiedMethodUsingNamingConventionController());
            When("a data context is injected to the controller", () => DataContextInjector.Inject(DataContext, Controller));
            Then("the specified data context should be injected to the method of the controller", () => Controller.AssertDataContext(DataContext));
        }

        [Example("When a data context is specified by the method using a wrong naming convention")]
        void Ex10()
        {
            Given("a controller that has a data context specified by the method using a wrong naming convention", () => Controller = new TestControllers.DataContextSpecifiedMethodUsingWrongNamingConventionController());
            When("a data context is injected to the controller", () => DataContextInjector.Inject(DataContext, Controller));
            Then("the specified data context should not be injected to the method of the controller", () => Controller.AssertDataContext(null));
        }

        [Example("When a data context is specified by the method whose parameter is wrong using a naming convention")]
        void Ex11()
        {
            Given("a controller that has a data context specified by the method whose parameter is wrong using a naming convention", () => Controller = new TestControllers.DataContextSpecifiedWrongArgumentMethodUsingNamingConventionController());
            When("a data context is injected to the controller", () => DataContextInjector.Inject(DataContext, Controller));
            Then<DataContextInjectionException>($"{typeof(DataContextInjectionException)} should be thrown");
        }
    }
}
