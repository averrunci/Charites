// Copyright (C) 2022 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
namespace Charites.Windows.Mvc;

internal static class TestControllers
{
    public interface IDataContextAssertController
    {
        bool AssertDataContext(object? expectedDataContext);
    }

    public class DataContextAttributedToFieldController : IDataContextAssertController
    {
        [DataContext]
        private object? dataContext;

        public bool AssertDataContext(object? expectedDataContext) => dataContext == expectedDataContext;
    }

    public class DataContextNotAttributedToFieldController : IDataContextAssertController
    {
        private object? dataContext;

        public bool AssertDataContext(object? expectedDataContext) => dataContext == expectedDataContext;
    }

    public class DataContextAttributedToPropertyController : IDataContextAssertController
    {
        [DataContext]
        public object? DataContext { get; set; }

        public bool AssertDataContext(object? expectedDataContext) => DataContext == expectedDataContext;
    }

    public class DataContextNotAttributedToPropertyController : IDataContextAssertController
    {
        public object? DataContext { get; set; }

        public bool AssertDataContext(object? expectedDataContext) => DataContext == expectedDataContext;
    }

    public class DataContextAttributedToReadOnlyPropertyController : IDataContextAssertController
    {
        [DataContext]
        public object? DataContext { get; }

        public bool AssertDataContext(object? expectedDataContext) => DataContext == expectedDataContext;
    }

    public class DataContextAttributedToMethodController : IDataContextAssertController
    {
        private object? dataContext;

        [DataContext]
        void SetDataContext(object? dataContext) => this.dataContext = dataContext;

        public bool AssertDataContext(object? expectedDataContext) => dataContext == expectedDataContext;
    }

    public class DataContextNotAttributedToMethodController : IDataContextAssertController
    {
        private object? dataContext;

        public void SetDataContextObject(object? dataContext) => this.dataContext = dataContext;

        public bool AssertDataContext(object? expectedDataContext) => dataContext == expectedDataContext;
    }

    public class DataContextAttributedToWrongArgumentMethodController : IDataContextAssertController
    {
        private object? dataContext;

        [DataContext]
        void SetDataContext() { }

        public bool AssertDataContext(object? expectedDataContext) => dataContext == expectedDataContext;
    }

    public class DataContextSpecifiedMethodUsingNamingConventionController : IDataContextAssertController
    {
        private object? dataContext;

        void SetDataContext(object? dataContext) => this.dataContext = dataContext;

        public bool AssertDataContext(object? expectedDataContext) => dataContext == expectedDataContext;
    }

    public class DataContextSpecifiedMethodUsingWrongNamingConventionController : IDataContextAssertController
    {
        private object? dataContext;

        public void Set_DataContext(object? dataContext) => this.dataContext = dataContext;

        public bool AssertDataContext(object? expectedDataContext) => dataContext == expectedDataContext;
    }

    public class DataContextSpecifiedWrongArgumentMethodUsingNamingConventionController : IDataContextAssertController
    {
        private object? dataContext;

        void SetDataContext() { }

        public bool AssertDataContext(object? expectedDataContext) => dataContext == expectedDataContext;
    }

    public interface IElementAssertController
    {
        bool AssertElements(TestElement? rootElement);
    }

    public class ElementAttributedToFieldController : IElementAssertController
    {
        [Element]
        private TestElement? childElement1;

        [Element(Name = "ChildElement2")]
        private TestElement? childElement2;

        [Element]
        private TestElement? ChildElement3;

        public bool AssertElements(TestElement? rootElement)
        {
            return childElement1 == rootElement?.Children.ElementAt(0) &&
                childElement2 == rootElement?.Children.ElementAt(1) &&
                ChildElement3 == rootElement?.Children.ElementAt(2);
        }
    }

    public class ElementNotAttributedToFieldController : IElementAssertController
    {
        private TestElement? childElement1;
        private TestElement? childElement2;
        private TestElement? ChildElement3;

        public bool AssertElements(TestElement? rootElement)
        {
            return childElement1 == rootElement &&
                childElement2 == rootElement &&
                ChildElement3 == rootElement;
        }
    }

    public class ElementAttributedToPropertyController : IElementAssertController
    {
        [Element(Name = "childElement1")]
        private TestElement? ChildElement1 { get; set; }

        [Element]
        public TestElement? ChildElement2 { get; set; }

        [Element]
        private TestElement? ChildElement3 { get; set; }

        public bool AssertElements(TestElement? rootElement)
        {
            return ChildElement1 == rootElement?.Children.ElementAt(0) &&
                ChildElement2 == rootElement?.Children.ElementAt(1) &&
                ChildElement3 == rootElement?.Children.ElementAt(2);
        }
    }

    public class ElementNotAttributedToPropertyController : IElementAssertController
    {
        private TestElement? ChildElement1 { get; set; }
        public TestElement? ChildElement2 { get; set; }
        private TestElement? ChildElement3 { get; set; }

        public bool AssertElements(TestElement? rootElement)
        {
            return ChildElement1 == rootElement &&
                ChildElement2 == rootElement &&
                ChildElement3 == rootElement;
        }
    }

    public class ElementAttributedToReadOnlyPropertyController : IElementAssertController
    {
        [Element(Name = "childElement1")]
        private TestElement? ChildElement1 { get; }

        [Element]
        public TestElement? ChildElement2 { get; }

        [Element]
        private TestElement? ChildElement3 { get; }

        public bool AssertElements(TestElement? rootElement)
        {
            return ChildElement1 == rootElement?.Children.ElementAt(0) &&
                ChildElement2 == rootElement?.Children.ElementAt(1) &&
                ChildElement3 == rootElement?.Children.ElementAt(2);
        }
    }

    public class ElementAttributedToMethodController : IElementAssertController
    {
        [Element(Name = "childElement1")]
        private void SetChildElement1(TestElement? element) => childElement1 = element;
        private TestElement? childElement1;

        [Element]
        public void SetChildElement2(TestElement? element) => childElement2 = element;
        private TestElement? childElement2;

        [Element]
        private void ChildElement3(TestElement? element) => childElement3 = element;
        private TestElement? childElement3;

        public bool AssertElements(TestElement? rootElement)
        {
            return childElement1 == rootElement?.Children.ElementAt(0) &&
                childElement2 == rootElement?.Children.ElementAt(1) &&
                childElement3 == rootElement?.Children.ElementAt(2);
        }
    }

    public class ElementNotAttributedToMethodController : IElementAssertController
    {
        private void SetChildElement1(TestElement? element) => childElement1 = element;
        private TestElement? childElement1;

        public void SetChildElement2(TestElement? element) => childElement2 = element;
        private TestElement? childElement2;

        private void ChildElement3(TestElement? element) => childElement3 = element;
        private TestElement? childElement3;

        public bool AssertElements(TestElement? rootElement)
        {
            return childElement1 == rootElement &&
                childElement2 == rootElement &&
                childElement3 == rootElement;
        }
    }

    public class ElementAttributedToWrongArgumentMethodController : IElementAssertController
    {
        [Element(Name = "childElement1")]
        private void SetChildElement1(string element) { }
        private TestElement? childElement1;

        [Element]
        public void SetChildElement2() { }
        private TestElement? childElement2;

        [Element]
        private void ChildElement3() { }
        private TestElement? childElement3;

        public bool AssertElements(TestElement? rootElement)
        {
            return childElement1 == rootElement?.Children.ElementAt(0) &&
                childElement2 == rootElement?.Children.ElementAt(1) &&
                childElement3 == rootElement?.Children.ElementAt(2);
        }
    }

    [View(Key = "TestElement1")]
    public class TestElement1Controller
    {

    }

    [View(ViewType = typeof(TestElement))]
    public class TestElementController
    {
    }

    [View(ViewType = typeof(DerivedTestElement))]
    public class DerivedTestElementController
    {
    }

    [View(ViewType = typeof(DerivedTestElement), Key = "TestElement1")]
    public class DerivedTestElement1Controller
    {
    }

    [View(Key = "TestElement1")]
    [View(ViewType = typeof(DerivedTestElement))]
    public class ElementController
    {
    }

    [View]
    public class ViewController
    {
    }

    public class EventHandlerAttributedToFieldController
    {
        [EventHandler(ElementName = "Element1", Event = "Click")]
        private readonly Action noArgumentHandler;

        [EventHandler(ElementName = "Element1", Event = "Click")]
        [EventHandler(ElementName = "Element2", Event = "Click")]
        private readonly Action<object> oneArgumentHandler;

        [EventHandler(ElementName = "Element1", Event = "Click")]
        [EventHandler(ElementName = "Element2", Event = "Click")]
        [EventHandler(ElementName = "Element3", Event = "Click")]
        private readonly Action<object, object> twoArgumentsHandler;

        public EventHandlerAttributedToFieldController(Action noArgumentAssertionHandler, Action<object> oneArgumentAssertionHandler, Action<object, object> twoArgumentsAssertionHandler)
        {
            noArgumentHandler = noArgumentAssertionHandler;
            oneArgumentHandler = oneArgumentAssertionHandler;
            twoArgumentsHandler = twoArgumentsAssertionHandler;
        }
    }

    public class EventHandlerNotAttributedToFieldController
    {
        private readonly Action noArgumentHandler;
        private readonly Action<object> oneArgumentHandler;
        private readonly Action<object, object> twoArgumentsHandler;

        public EventHandlerNotAttributedToFieldController(Action noArgumentAssertionHandler, Action<object> oneArgumentAssertionHandler, Action<object, object> twoArgumentsAssertionHandler)
        {
            noArgumentHandler = noArgumentAssertionHandler;
            oneArgumentHandler = oneArgumentAssertionHandler;
            twoArgumentsHandler = twoArgumentsAssertionHandler;
        }
    }

    public class EventHandlerAttributedToPropertyController
    {
        [EventHandler(ElementName = "Element1", Event = "Click")]
        public Action NoArgumentHandler { get; set; }

        [EventHandler(ElementName = "Element1", Event = "Click")]
        [EventHandler(ElementName = "Element2", Event = "Click")]
        private Action<object> OneArgumentHandler { get; set; }

        [EventHandler(ElementName = "Element1", Event = "Click")]
        [EventHandler(ElementName = "Element2", Event = "Click")]
        [EventHandler(ElementName = "Element3", Event = "Click")]
        private Action<object, object> TwoArgumentsHandler { get; set; }

        public EventHandlerAttributedToPropertyController(Action noArgumentAssertionHandler, Action<object> oneArgumentAssertionHandler, Action<object, object> twoArgumentsAssertionHandler)
        {
            NoArgumentHandler = noArgumentAssertionHandler;
            OneArgumentHandler = oneArgumentAssertionHandler;
            TwoArgumentsHandler = twoArgumentsAssertionHandler;
        }
    }

    public class EventHandlerNotAttributedToPropertyController
    {
        public Action NoArgumentHandler { get; set; }
        private Action<object> OneArgumentHandler { get; set; }
        private Action<object, object> TwoArgumentsHandler { get; set; }

        public EventHandlerNotAttributedToPropertyController(Action noArgumentAssertionHandler, Action<object> oneArgumentAssertionHandler, Action<object, object> twoArgumentsAssertionHandler)
        {
            NoArgumentHandler = noArgumentAssertionHandler;
            OneArgumentHandler = oneArgumentAssertionHandler;
            TwoArgumentsHandler = twoArgumentsAssertionHandler;
        }
    }

    public class EventHandlerAttributedToWriteOnlyPropertyController
    {
        [EventHandler(ElementName = "Element1", Event = "Click")]
        public Action NoArgumentHandler { set => noArgumentHandler = value; }
        private Action noArgumentHandler;

        [EventHandler(ElementName = "Element1", Event = "Click")]
        [EventHandler(ElementName = "Element2", Event = "Click")]
        private Action<object> OneArgumentHandler { set => oneArgumentHandler = value; }
        private Action<object> oneArgumentHandler;

        [EventHandler(ElementName = "Element1", Event = "Click")]
        [EventHandler(ElementName = "Element2", Event = "Click")]
        [EventHandler(ElementName = "Element3", Event = "Click")]
        private Action<object, object> TwoArgumentsHandler { set => twoArgumentsHandler = value; }
        private Action<object, object> twoArgumentsHandler;

        public EventHandlerAttributedToWriteOnlyPropertyController(Action noArgumentAssertionHandler, Action<object> oneArgumentAssertionHandler, Action<object, object> twoArgumentsAssertionHandler)
        {
            noArgumentHandler = noArgumentAssertionHandler;
            oneArgumentHandler = oneArgumentAssertionHandler;
            twoArgumentsHandler = twoArgumentsAssertionHandler;
        }
    }

    public class EventHandlerAttributedToMethodController
    {
        [EventHandler(ElementName = "Element1", Event = "Click")]
        public void HandleNoArgument()
        {
            noArgumentHandler();
        }
        private readonly Action noArgumentHandler;

        [EventHandler(ElementName = "Element1", Event = "Click")]
        [EventHandler(ElementName = "Element2", Event = "Click")]
        private void HandleOneArgument(object e)
        {
            oneArgumentHandler(e);
        }
        private readonly Action<object> oneArgumentHandler;

        [EventHandler(ElementName = "Element1", Event = "Click")]
        [EventHandler(ElementName = "Element2", Event = "Click")]
        [EventHandler(ElementName = "Element3", Event = "Click")]
        private void HandleTwoArguments(object sender, object e)
        {
            twoArgumentsHandler(sender, e);
        }
        private readonly Action<object, object> twoArgumentsHandler;

        public EventHandlerAttributedToMethodController(Action noArgumentAssertionHandler, Action<object> oneArgumentAssertionHandler, Action<object, object> twoArgumentsAssertionHandler)
        {
            noArgumentHandler = noArgumentAssertionHandler;
            oneArgumentHandler = oneArgumentAssertionHandler;
            twoArgumentsHandler = twoArgumentsAssertionHandler;
        }
    }

    public class EventHandlerNotAttributedToMethodController
    {
        public void HandleNoArgument()
        {
            noArgumentHandler();
        }
        private readonly Action noArgumentHandler;

        private void HandleOneArgument(object e)
        {
            oneArgumentHandler(e);
        }
        private readonly Action<object> oneArgumentHandler;

        private void HandleTwoArguments(object sender, object e)
        {
            twoArgumentsHandler(sender, e);
        }
        private readonly Action<object, object> twoArgumentsHandler;

        public EventHandlerNotAttributedToMethodController(Action noArgumentAssertionHandler, Action<object> oneArgumentAssertionHandler, Action<object, object> twoArgumentsAssertionHandler)
        {
            noArgumentHandler = noArgumentAssertionHandler;
            oneArgumentHandler = oneArgumentAssertionHandler;
            twoArgumentsHandler = twoArgumentsAssertionHandler;
        }
    }

    public class EventHandlerAttributedToWrongArgumentMethodController
    {
        [EventHandler(ElementName = "Element1", Event = "Click")]
        public void HandleNoArgument(object parameter1, object parameter2, object parameter3)
        {
            noArgumentHandler();
        }
        private readonly Action noArgumentHandler;

        [EventHandler(ElementName = "Element1", Event = "Click")]
        [EventHandler(ElementName = "Element2", Event = "Click")]
        private void HandleOneArgument(object e)
        {
            oneArgumentHandler(e);
        }
        private readonly Action<object> oneArgumentHandler;

        [EventHandler(ElementName = "Element1", Event = "Click")]
        [EventHandler(ElementName = "Element2", Event = "Click")]
        [EventHandler(ElementName = "Element3", Event = "Click")]
        private void HandleTwoArguments(object sender, object e)
        {
            twoArgumentsHandler(sender, e);
        }
        private readonly Action<object, object> twoArgumentsHandler;

        public EventHandlerAttributedToWrongArgumentMethodController(Action noArgumentAssertionHandler, Action<object> oneArgumentAssertionHandler, Action<object, object> twoArgumentsAssertionHandler)
        {
            noArgumentHandler = noArgumentAssertionHandler;
            oneArgumentHandler = oneArgumentAssertionHandler;
            twoArgumentsHandler = twoArgumentsAssertionHandler;
        }
    }

    public class EventHandlerOfMethodUsingNamingConventionController
    {
        public void Element1_Click()
        {
            noArgumentHandler();
        }
        private readonly Action noArgumentHandler;

        private void Element1_Click(object e)
        {
            oneArgumentHandler(e);
        }
        private void Element2_Click(object e)
        {
            oneArgumentHandler(e);
        }
        private readonly Action<object> oneArgumentHandler;

        private void Element1_Click(object sender, object e)
        {
            twoArgumentsHandler(sender, e);
        }
        private void Element2_Click(object sender, object e)
        {
            twoArgumentsHandler(sender, e);
        }
        private void Element3_Click(object sender, object e)
        {
            twoArgumentsHandler(sender, e);
        }
        private readonly Action<object, object> twoArgumentsHandler;

        public EventHandlerOfMethodUsingNamingConventionController(Action noArgumentAssertionHandler, Action<object> oneArgumentAssertionHandler, Action<object, object> twoArgumentsAssertionHandler)
        {
            noArgumentHandler = noArgumentAssertionHandler;
            oneArgumentHandler = oneArgumentAssertionHandler;
            twoArgumentsHandler = twoArgumentsAssertionHandler;
        }
    }

    public class EventHandlerOfAsyncMethodUsingNamingConventionController
    {
        public Task Element1_ClickAsync()
        {
            noArgumentHandler();
            return Task.CompletedTask;
        }
        private readonly Action noArgumentHandler;

        private Task Element1_ClickAsync(object e)
        {
            oneArgumentHandler(e);
            return Task.CompletedTask;
        }
        private Task Element2_ClickAsync(object e)
        {
            oneArgumentHandler(e);
            return Task.CompletedTask;
        }
        private readonly Action<object> oneArgumentHandler;

        private Task Element1_ClickAsync(object sender, object e)
        {
            twoArgumentsHandler(sender, e);
            return Task.CompletedTask;
        }
        private Task Element2_ClickAsync(object sender, object e)
        {
            twoArgumentsHandler(sender, e);
            return Task.CompletedTask;
        }
        private Task Element3_ClickAsync(object sender, object e)
        {
            twoArgumentsHandler(sender, e);
            return Task.CompletedTask;
        }
        private readonly Action<object, object> twoArgumentsHandler;

        public EventHandlerOfAsyncMethodUsingNamingConventionController(Action noArgumentAssertionHandler, Action<object> oneArgumentAssertionHandler, Action<object, object> twoArgumentsAssertionHandler)
        {
            noArgumentHandler = noArgumentAssertionHandler;
            oneArgumentHandler = oneArgumentAssertionHandler;
            twoArgumentsHandler = twoArgumentsAssertionHandler;
        }
    }

    public class EventHandlerOfMethodUsingWrongNamingConventionController
    {
        public void _Element1Click()
        {
            noArgumentHandler();
        }
        private readonly Action noArgumentHandler;

        private void Element2_Click_(object e)
        {
            oneArgumentHandler(e);
        }
        private readonly Action<object> oneArgumentHandler;

        private void OnElement3Click(object sender, object e)
        {
            twoArgumentsHandler(sender, e);
        }
        private readonly Action<object, object> twoArgumentsHandler;

        public EventHandlerOfMethodUsingWrongNamingConventionController(Action noArgumentAssertionHandler, Action<object> oneArgumentAssertionHandler, Action<object, object> twoArgumentsAssertionHandler)
        {
            noArgumentHandler = noArgumentAssertionHandler;
            oneArgumentHandler = oneArgumentAssertionHandler;
            twoArgumentsHandler = twoArgumentsAssertionHandler;
        }
    }

    public class EventHandlerOfWrongArgumentMethodUsingNamingConventionController
    {
        public void Element1_Click(object parameter1, object parameter2, object parameter3)
        {
            noArgumentHandler();
        }
        private readonly Action noArgumentHandler;

        private void Element1_Click(object e)
        {
            oneArgumentHandler(e);
        }
        private void Element2_Click(object e)
        {
            oneArgumentHandler(e);
        }
        private readonly Action<object> oneArgumentHandler;

        private void Element1_Click(object sender, object e)
        {
            twoArgumentsHandler(sender, e);
        }
        private void Element2_Click(object sender, object e)
        {
            twoArgumentsHandler(sender, e);
        }
        private void Element3_Click(object sender, object e)
        {
            twoArgumentsHandler(sender, e);
        }
        private readonly Action<object, object> twoArgumentsHandler;

        public EventHandlerOfWrongArgumentMethodUsingNamingConventionController(Action noArgumentAssertionHandler, Action<object> oneArgumentAssertionHandler, Action<object, object> twoArgumentsAssertionHandler)
        {
            noArgumentHandler = noArgumentAssertionHandler;
            oneArgumentHandler = oneArgumentAssertionHandler;
            twoArgumentsHandler = twoArgumentsAssertionHandler;
        }
    }

    public class EventHandlerOfMethodWithParametersSpecifiedByAttributeController
    {
        [EventHandler(ElementName = "Element1", Event = "Click")]
        public void OnElement1Click([FromDI] IDependency1 dependency1, [FromDI] IDependency2 dependency2, [FromDI] IDependency3 dependency3, [FromElement] TestElement element)
        {
            noArgumentHandler();
            attributedArgumentsHandler(dependency1, dependency2, dependency3, element);
        }
        public async Task Element4_Click([FromDI] IDependency1 dependency1, [FromDI] IDependency2 dependency2, [FromDI] IDependency3 dependency3, [FromElement] TestElement element)
        {
            await Task.Delay(10);
            noArgumentHandler();
            attributedArgumentsHandler(dependency1, dependency2, dependency3, element);
        }
        private readonly Action noArgumentHandler;

        private void Element2_Click([FromDI] IDependency1 dependency1, object e, [FromDI] IDependency2 dependency2, [FromDI] IDependency3 dependency3, [FromElement] TestElement element)
        {
            oneArgumentHandler(e);
            attributedArgumentsHandler(dependency1, dependency2, dependency3, element);
        }
        [EventHandler(ElementName = "Element5", Event = "Click")]
        private async Task OnElement5Click([FromDI] IDependency1 dependency1, object e, [FromDI] IDependency2 dependency2, [FromDI] IDependency3 dependency3, [FromElement] TestElement element)
        {
            await Task.Delay(10);
            oneArgumentHandler(e);
            attributedArgumentsHandler(dependency1, dependency2, dependency3, element);
        }
        private readonly Action<object> oneArgumentHandler;

        [EventHandler(ElementName = "Element3", Event = "Click")]
        private void OnElement3Click([FromDI] IDependency1 dependency1, object sender, [FromDI] IDependency2 dependency2, object e, [FromDI] IDependency3 dependency3, [FromElement] TestElement element)
        {
            twoArgumentsHandler(sender, e);
            attributedArgumentsHandler(dependency1, dependency2, dependency3, element);
        }
        private async Task Element6_Click([FromDI] IDependency1 dependency1, object sender, [FromDI] IDependency2 dependency2, object e, [FromDI] IDependency3 dependency3, [FromElement] TestElement element)
        {
            await Task.Delay(10);
            twoArgumentsHandler(sender, e);
            attributedArgumentsHandler(dependency1, dependency2, dependency3, element);
        }
        private readonly Action<object, object> twoArgumentsHandler;

        private readonly Action<IDependency1, IDependency2, IDependency3, TestElement> attributedArgumentsHandler;

        public EventHandlerOfMethodWithParametersSpecifiedByAttributeController(Action noArgumentAssertionHandler, Action<object> oneArgumentAssertionHandler, Action<object, object> twoArgumentsAssertionHandler, Action<IDependency1, IDependency2, IDependency3, TestElement> attributedArgumentsAssertionHandler)
        {
            noArgumentHandler = noArgumentAssertionHandler;
            oneArgumentHandler = oneArgumentAssertionHandler;
            twoArgumentsHandler = twoArgumentsAssertionHandler;
            attributedArgumentsHandler = attributedArgumentsAssertionHandler;
        }
    }

    public interface IDependency1 { }
    public interface IDependency2 { }
    public interface IDependency3 { }
    public class Dependency1 : IDependency1 { }
    public class Dependency2 : IDependency2 { }
    public class Dependency3 : IDependency3 { }

    public class DisposableController : IDisposable
    {
        public bool IsDisposed { get; private set; }

        public void Dispose()
        {
            IsDisposed = true;
        }
    }

    public interface ITestDataContext { }
    public interface ITestDataContextFullName { }
    public class TestDataContext { }
    public class TestDataContextFullName { }
    public class BaseTestDataContext { }
    public class DerivedBaseTestDataContext : BaseTestDataContext { }
    public class BaseTestDataContextFullName { }
    public class DerivedBaseTestDataContextFullName : BaseTestDataContextFullName { }
    public class GenericTestDataContext<T> { }
    public class GenericTestDataContextFullName<T> { }
    public class InterfaceImplementedTestDataContext : ITestDataContext { }
    public class InterfaceImplementedTestDataContextFullName : ITestDataContextFullName { }
    public class KeyTestDataContext { }

    [View(Key = "TestDataContext")]
    public class TestDataContextController { }

    [View(Key = "BaseTestDataContext")]
    public class BaseTestDataContextController { }

    [View(Key = "Charites.Windows.Mvc.TestControllers+TestDataContextFullName")]
    public class TestDataContextFullNameController { }

    [View(Key = "Charites.Windows.Mvc.TestControllers+BaseTestDataContextFullName")]
    public class BaseTestDataContextFullNameController { }

    [View(Key = "GenericTestDataContext`1")]
    public class GenericTestDataContextController { }

    [View(Key = "Charites.Windows.Mvc.TestControllers+GenericTestDataContextFullName`1[System.String]")]
    public class GenericTestDataContextFullNameController { }

    [View(Key = "Charites.Windows.Mvc.TestControllers+GenericTestDataContextFullName`1")]
    public class GenericTestDataContextFullNameWithoutParametersController { }

    [View(Key = "ITestDataContext")]
    public class InterfaceImplementedTestDataContextController { }

    [View(Key = "Charites.Windows.Mvc.TestControllers+ITestDataContextFullName")]
    public class InterfaceImplementedTestDataContextFullNameController { }
}