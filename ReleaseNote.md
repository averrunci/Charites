# Release note

## v2.2.0

### Add

- Add the ObjectExtensions class that has the following extension methods.
  - IfPresent&lt;T&gt;(this T?, Action&lt;T&gt;)
  - IfPresent&lt;T1, T2&gt;(this T1?, T2?, Action&lt;T1, T2&gt;)
  - IfPresent&lt;T1, T2, T3&gt;(this T1?, T2?, T3?, Action&lt;T1, T2, T3&gt;)
  - IfPresent&lt;T1, T2, T3, T4&gt;(this T1?, T2?, T3?, T4?, Action&lt;T1, T2, T3, T4&gt;)
  - IfPresent&lt;T1, T2, T3, T4, T5&gt;(this T1?, T2?, T3?, T4?, T5?, Action&lt;T1, T2, T3, T4, T5&gt;)
  - IfPresent&lt;T1, T2, T3, T4, T5, T6&gt;(this T1?, T2?, T3?, T4?, T5?, T6?, Action&lt;T1, T2, T3, T4, T5, T6&gt;)
  - IfPresent&lt;T1, T2, T3, T4, T5, T6, T7&gt;(this T1?, T2?, T3?, T4?, T5?, T6?, T7?, Action&lt;T1, T2, T3, T4, T5, T6, T7&gt;)
  - IfPresent&lt;T1, T2, T3, T4, T5, T6, T7, T8&gt;(this T1?, T2?, T3?, T4?, T5?, T6?, T7?, T8?, Action&lt;T1, T2, T3, T4, T5, T6, T7, T8&gt;)
  - IfPresentAsync&lt;T&gt;(this T?, Action&lt;T, Task&gt;)
  - IfPresentAsync&lt;T1, T2&gt;(this T1?, T2?, Func&lt;T1, T2, Task&gt;)
  - IfPresentAsync&lt;T1, T2, T3&gt;(this T1?, T2?, T3?, Func&lt;T1, T2, T3, Task&gt;)
  - IfPresentAsync&lt;T1, T2, T3, T4&gt;(this T1?, T2?, T3?, T4?, Func&lt;T1, T2, T3, T4, Task&gt;)
  - IfPresentAsync&lt;T1, T2, T3, T4, T5&gt;(this T1?, T2?, T3?, T4?, T5?, Func&lt;T1, T2, T3, T4, T5, Task&gt;)
  - IfPresentAsync&lt;T1, T2, T3, T4, T5, T6&gt;(this T1?, T2?, T3?, T4?, T5?, T6?, Func&lt;T1, T2, T3, T4, T5, T6, Task&gt;)
  - IfPresentAsync&lt;T1, T2, T3, T4, T5, T6, T7&gt;(this T1?, T2?, T3?, T4?, T5?, T6?, T7?, Func&lt;T1, T2, T3, T4, T5, T6, T7, Task&gt;)
  - IfPresentAsync&lt;T1, T2, T3, T4, T5, T6, T7, T8&gt;(this T1?, T2?, T3?, T4?, T5?, T6?, T7?, T8?, Func&lt;T1, T2, T3, T4, T5, T6, T7, T8, Task&gt;)
- Add the ControllerBase class that represents a base class for a controller associated with a view that has a data context of the specified type.

## v2.1.0

### Add

- Add the IElementFinder interface to find an element.
- Add the EventHandlerParameterAttribute attribute that is specified to the parameter of an event handler to inject a value.
- Add the FromElementAttribute attribute that is specified to the parameter to inject from the element in the view.
- Add the FromDataContextAttribute attribute that is specified to the parameter to inject from the data context in the view.
- Add the IEventHandlerParameterResolver interface to resolve a parameter specified by the attribute.
- Add the EventHandlerParameterResolver class that is a base class to implement the IEventHandlerParameterResolver interface.
- Add the EventHandlerParameterFromDIResolver class that is a base class to resolve a parameter specified by the FromDIAttribute attribute.
- Add the EventHandlerParameterFromElementResolver class that is a base class to resolve a parameter specified by the FromElementAttribute attribute.
- Add the EventHandlerParameterFromDataContextResolver class that is a base class to resolve a parameter specified by the FromDataContextAttribute attribute.
- Add the ResolveFromDI method to the EventHandlerBase.Executor class.
- Add the ResolveFromElement method to the EventHandlerBase.Executor class.
- Add the ResolveFromDataContext method to the EventHandlerBase.Executor class.
- Add the following method in the EventHandlerExtension class so that the EventHandlerExtension uses the IEventHandlerParameterResolver interface:
  - void Add&lt;TResolver&gt;() where TResolver : IEventHandlerParameterResolver
  - void Add(Type resolverType)
  - void Remove&lt;TResolver&gt;() where TResolver : IEventHandlerParameterResolver
  - void Remove(Type resolverType)

### Change

- Change to use the IElementFinder interface to find an element in the ElementInjector class.
- Obsolete the following method:
  - EventHandlerAction class
    - object? Handle(object?, object?, IDictionary&lt;Type, Func&lt;object?&gt;&gt;?)
    - IParameterDependencyResolver CreateParameterDependencyResolver(IDictionary&lt;Type, Func&lt;object?&gt;&gt;?)
  - EventHandlerBase class
    - Executor Resolve&lt;T&gt;(Func&lt;object?&gt;)
  - EventHandlerExtension class
    - Delegate? CreateEventHandler(Delegate?, Type?)
    - Delegate? CreateEventHandler(MethodInfo, object?, Type?)
    - EventHandlerAction CreateEventHandlerAction(MethodInfo, object?)
  - EventHandlerItem class
    - void Raise(string, object?, object?, IDictionary&lt;Type, Func&lt;object?&gt;&gt;)
    - Task RaiseAsync(string, object?, object?, IDictionary&lt;Type, Func&lt;object?&gt;&gt;?)
    - IParameterDependencyResolver CreateParameterDependencyResolver(IDictionary&lt;Type, Func&lt;object?&gt;&gt;?)
    - object? Handle(Delegate, object?, object?, IDictionary&lt;Type, Func&lt;object?&gt;&gt;?)
  - ParameterDependencyResolver class
    - .ctor(IDictionary&lt;Type, Func&lt;object?&gt;&gt;)
    - .ctor(IEnumerable&lt;IEventHandlerParameterResolver&gt;, IDictionary&lt;Type, Func&lt;object?&gt;&gt;)

## v2.0.0

### Change

- Update the target framework version to .NET 6.0.
- Enable Nullable reference types.

## v1.3.2

### Change

- Modify how to retrieve an event name from a method that represents an event handler using naming convention. If its name ends with "Async", it is ignored.

## v1.3.1

### Add

- Add the IContentNavigator interface and its implementation class that navigates to the content and records the navigation in the ForwardStack or BackwardStack.

## v1.3.0

### Add

- Add the FromDIAttribute attribute that is specified to the parameter to inject from the DI container.
- Add the IParameterDependencyResolver interface and its implementation class that resolves dependencies of parameters.
- Add the Resolve method to the EventHandlerBase.Executor class.
- Add the Raise and RaiseAsync method that have a parameter whose type is the IDictionary<Type, Func&lt;object&gt;> to the EventHandlerItem class.
- Add the Handle method that has a parameter whose type is the IDictionary<Type, Func&lt;object&gt;> to the EventHandlerAction class.
- Add the CreateParameterDependencyResolver virtual method that creates an instance that implements the IParameterDependencyResolver to the EventHandlerAction class.

## v1.2.0

### Add

- Add the function to set event handlers using a naming convention (ElementName_EventName).
- Add the function to set a data context using a naming convention (its method name is SetDataContext).

## v1.1.0

### Add

- Add the virtual method (HandleUnhandledException) to the EventHandlerAction class in order to handle an unhandled exception that occurred in an event handler.

## v1.0.1

### Bug fix

- Fixed the KeyFilter of the ControllerTypeFinder not to search the type name of the data context with the Key property of the ViewAttribute if it is specified.
- Fixed the RaiseAsync method in order to be able to handle the event asynchronously with not only the EventHandlerAction but also any action.