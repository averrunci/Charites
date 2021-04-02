# Release note

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