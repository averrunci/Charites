// Copyright (C) 2022 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using Carna;
using NSubstitute;

namespace Charites.Windows.Mvc;

[Specification("ControllerCollection Spec")]
class ControllerCollectionSpec : FixtureSteppable
{
    ControllerCollectionTss Controllers { get; }

    IDataContextFinder<TestElement> DataContextFinder { get; } = Substitute.For<IDataContextFinder<TestElement>>();
    IDataContextInjector DataContextInjector { get; } = Substitute.For<IDataContextInjector>();
    IElementInjector<TestElement> ElementInjector { get; } = Substitute.For<IElementInjector<TestElement>>();
    IControllerExtension<TestElement>[] Extensions { get; } = {
        Substitute.For<IControllerExtension<TestElement>>(),
        Substitute.For<IControllerExtension<TestElement>>(),
        Substitute.For<IControllerExtension<TestElement>>()
    };

    TestElement Element1 { get; } = new("Element1");
    object DataContext { get; } = new();

    object[] ActualControllers { get; } = { new(), new(), new() };
    TestControllers.DisposableController DisposableController { get; } = new();

    public ControllerCollectionSpec()
    {
        Controllers = new ControllerCollectionTss(DataContextFinder, DataContextInjector, ElementInjector, Extensions);

        DataContextFinder.Find(Element1).Returns(DataContext);
    }

    void ClearReceivedCalls()
    {
        Controllers.ClearSubscribedUnsubscribed();
        DataContextFinder.ClearReceivedCalls();
        DataContextInjector.ClearReceivedCalls();
        ElementInjector.ClearReceivedCalls();
        Extensions.ForEach(extension => extension.ClearReceivedCalls());
    }

    [Example("Attaches a controller to an element that is not loaded")]
    void Ex01()
    {
        When("a controller is added", () => Controllers.Add(ActualControllers[0]));
        Then("the controller collection should not subscribe to events of the associated element", () => !Controllers.AssociatedElementEventsSubscribed);
        Then("the DataContext should not be found", () => DataContextFinder.DidNotReceive().Find(Arg.Any<TestElement>()));
        Then("the DataContext should not be injected", () => DataContextInjector.DidNotReceive().Inject(Arg.Any<object?>(), Arg.Any<object>()));
        Then("the Element should not be injected", () => ElementInjector.DidNotReceive().Inject(Arg.Any<TestElement?>(), Arg.Any<object>()));
        Then("the Extensions should not be attached", () => Extensions.ForEach(extension => extension.DidNotReceive().Attach(Arg.Any<object>(), Arg.Any<TestElement>())));
        When("the controller is attached to an element", () => Controllers.AttachTo(Element1));
        Then("the controller collection should subscribe to events of the associated element", () => Controllers.AssociatedElementEventsSubscribed);
        Then("the DataContext should be found", () => DataContextFinder.Received().Find(Element1));
        Then("the DataContext should be injected", () => DataContextInjector.Received().Inject(DataContext, ActualControllers[0]));
        Then("the Element should not be injected", () => ElementInjector.DidNotReceive().Inject(Arg.Any<TestElement?>(), Arg.Any<object>()));
        Then("the Extensions should not be attached", () => Extensions.ForEach(extension => extension.DidNotReceive().Attach(Arg.Any<object>(), Arg.Any<TestElement>())));
    }

    [Example("Attaches a controller to an element that is loaded")]
    void Ex02()
    {
        When("a controller is added", () => Controllers.Add(ActualControllers[0]));
        Then("the controller collection should not subscribe to events of the associated element", () => !Controllers.AssociatedElementEventsSubscribed);
        Then("the DataContext should not be found", () => DataContextFinder.DidNotReceive().Find(Arg.Any<TestElement>()));
        Then("the DataContext should not be injected", () => DataContextInjector.DidNotReceive().Inject(Arg.Any<object?>(), Arg.Any<object>()));
        Then("the Element should not be injected", () => ElementInjector.DidNotReceive().Inject(Arg.Any<TestElement?>(), Arg.Any<object>()));
        Then("the Extensions should not be attached", () => Extensions.ForEach(extension => extension.DidNotReceive().Attach(Arg.Any<object>(), Arg.Any<TestElement>())));
        When("an element is loaded", () => Element1.Load());
        When("the controller is attached to the element", () => Controllers.AttachTo(Element1));
        Then("the controller collection should subscribe to events of the associated element", () => Controllers.AssociatedElementEventsSubscribed);
        Then("the DataContext should be found", () => DataContextFinder.Received().Find(Element1));
        Then("the DataContext should be injected", () => DataContextInjector.Received().Inject(DataContext, ActualControllers[0]));
        Then("the Element should be injected", () => ElementInjector.Received().Inject(Element1, ActualControllers[0]));
        Then("the Extensions should be attached", () => Extensions.ForEach(extension => extension.Received().Attach(ActualControllers[0], Element1)));
    }

    [Example("Attaches a controller that has already been attached to an element")]
    void Ex03()
    {
        When("a controller is added", () => Controllers.Add(ActualControllers[0]));
        When("the controller is attached to the element", () => Controllers.AttachTo(Element1));
        When("the controller is attached to same element again", () => Controllers.AttachTo(Element1));
        Then("the exception should not be thrown", () => true);
        When("the controller is attached to another element", () => Controllers.AttachTo(new TestElement("Element2")));
        Then<InvalidOperationException>($"{typeof(InvalidOperationException)} should be thrown");
    }

    [Example("Detaches a controller from the attached element")]
    void Ex04()
    {
        When("a controller is added", () => Controllers.Add(ActualControllers[0]));
        When("the controller is attached to the element", () => Controllers.AttachTo(Element1));

        ClearReceivedCalls();

        When("the controller is detached from the element", () => Controllers.Detach());
        Then("the controller collection should unsubscribe from events of the associated element", () => Controllers.AssociatedElementEventsUnsubscribed);
        Then("the DataContext should be initialized", () => DataContextInjector.Received().Inject(null, ActualControllers[0]));
        Then("the Element should be initialized", () => ElementInjector.Received().Inject(null, ActualControllers[0]));
        Then("the Extensions should be detached", () => Extensions.ForEach(extension => extension.Received().Detach(ActualControllers[0], Element1)));

        ClearReceivedCalls();

        When("the controller is detached again", () => Controllers.Detach());
        Then("the exception should not be thrown", () => true);
        Then("the controller collection should not be unsubscribed from events of the associated element", () => !Controllers.AssociatedElementEventsUnsubscribed);
        Then("the DataContext should not be initialized", () => DataContextInjector.DidNotReceive().Inject(Arg.Any<object?>(), Arg.Any<object>()));
        Then("the Element should not be initialized", () => ElementInjector.DidNotReceive().Inject(Arg.Any<TestElement?>(), Arg.Any<object>()));
        Then("the Extensions should not be detached", () => Extensions.ForEach(extension => extension.DidNotReceive().Detach(Arg.Any<object>(), Arg.Any<TestElement>())));

        When("the controller is attached to the element again", () => Controllers.AttachTo(Element1));
        Then("the controller collection should subscribe to events of the associated element", () => Controllers.AssociatedElementEventsSubscribed);
        Then("the DataContext should be found", () => DataContextFinder.Received().Find(Element1));
        Then("the DataContext should be injected", () => DataContextInjector.Received().Inject(DataContext, ActualControllers[0]));
        Then("the Element should not be injected", () => ElementInjector.DidNotReceive().Inject(Arg.Any<TestElement?>(), Arg.Any<object>()));
        Then("the Extensions should not be attached", () => Extensions.ForEach(extension => extension.DidNotReceive().Attach(Arg.Any<object>(), Arg.Any<TestElement>())));
    }

    [Example("Detaches a controller that implements IDisposable")]
    void Ex05()
    {
        When("a controller that implements IDisposable is added", () => Controllers.Add(DisposableController));
        When("the controller is attached to an element", () => Controllers.AttachTo(Element1));
        When("the controller is detached from the element", () => Controllers.Detach());
        Then("the controller should be disposed", () => DisposableController.IsDisposed);
    }

    [Example("Adds a controller after attached to an element")]
    void Ex06()
    {
        When("attached to an element", () => Controllers.AttachTo(Element1));
        When("a controller is added", () => Controllers.Add(ActualControllers[0]));
        Then("the controller collection should subscribe to events of the associated element", () => Controllers.AssociatedElementEventsSubscribed);
        Then("the DataContext should be found", () => DataContextFinder.Received().Find(Element1));
        Then("the DataContext should be injected", () => DataContextInjector.Received().Inject(DataContext, ActualControllers[0]));
        Then("the Element should not be injected", () => ElementInjector.DidNotReceive().Inject(Arg.Any<TestElement?>(), Arg.Any<object>()));
        Then("the Extensions should not be attached", () => Extensions.ForEach(extension => extension.DidNotReceive().Attach(Arg.Any<object>(), Arg.Any<TestElement>())));
    }

    [Example("Removes a controller after attached to an element")]
    void Ex07()
    {
        When("a controller is added", () => Controllers.Add(ActualControllers[0]));
        When("the controller is attached to an element", () => Controllers.AttachTo(Element1));

        ClearReceivedCalls();

        When("the controller is removed", () => Controllers.RemoveAt(0));
        Then("the controller collection should unsubscribe from events of the associated element", () => Controllers.AssociatedElementEventsUnsubscribed);
        Then("the DataContext should be initialized", () => DataContextInjector.Received().Inject(null, ActualControllers[0]));
        Then("the Element should be initialized", () => ElementInjector.Received().Inject(null, ActualControllers[0]));
        Then("the Extensions should be detached", () => Extensions.ForEach(extension => extension.Received().Detach(ActualControllers[0], Element1)));
    }

    [Example("Removes a controller before attached to an element")]
    void Ex08()
    {
        When("a controller is added", () => Controllers.Add(ActualControllers[0]));
        When("the controller is removed", () => Controllers.RemoveAt(0));
        Then("the controller collection should not be unsubscribed from events of the associated element", () => !Controllers.AssociatedElementEventsUnsubscribed);
        Then("the DataContext should not be initialized", () => DataContextInjector.DidNotReceive().Inject(Arg.Any<object?>(), Arg.Any<object>()));
        Then("the Element should not be initialized", () => ElementInjector.DidNotReceive().Inject(Arg.Any<TestElement?>(), Arg.Any<object>()));
        Then("the Extensions should not be detached", () => Extensions.ForEach(extension => extension.DidNotReceive().Detach(Arg.Any<object>(), Arg.Any<TestElement>())));
    }

    [Example("Clears all controllers after attached to an element")]
    void Ex09()
    {
        When("controllers are added", () => ActualControllers.ForEach(Controllers.Add));
        When("controllers are attached to an element", () => Controllers.AttachTo(Element1));

        ClearReceivedCalls();

        When("the controllers is cleared", () => Controllers.Clear());
        Then("the controller collection should unsubscribe from events of the associated element", () => Controllers.AssociatedElementEventsUnsubscribed);
        Then("the DataContext should be initialized", () => ActualControllers.ForEach(controller => DataContextInjector.Received().Inject(null, controller)));
        Then("the Element should be initialized", () => ActualControllers.ForEach(controller => ElementInjector.Received().Inject(null, controller)));
        Then("the Extensions should be detached", () => Extensions.ForEach(extension => ActualControllers.ForEach(controller => extension.Received().Detach(controller, Element1))));
    }

    [Example("Clears all controllers before attached to an element")]
    void Ex10()
    {
        When("controllers are added", () => ActualControllers.ForEach(Controllers.Add));
        When("the controllers is cleared", () => Controllers.Clear());
        Then("the controller collection should not unsubscribe from events of the associated element", () => !Controllers.AssociatedElementEventsUnsubscribed);
        Then("the DataContext should not be initialized", () => DataContextInjector.DidNotReceive().Inject(Arg.Any<object?>(), Arg.Any<object>()));
        Then("the Element should not be initialized", () => ElementInjector.DidNotReceive().Inject(Arg.Any<TestElement?>(), Arg.Any<object>()));
        Then("the Extensions should not be detached", () => Extensions.ForEach(extension => extension.DidNotReceive().Detach(Arg.Any<object>(), Arg.Any<TestElement>())));
    }
}