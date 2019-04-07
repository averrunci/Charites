# Release note

## V1.2.0

### Add

- Add the function to set event handlers using a naming convention (ElementName_EventName).
- Add the function to set a data context using a naming convention (its method name is SetDataContext).

## V1.1.0

### Add

- Add the virtual method (HandleUnhandledException) to the EventHandlerAction class in order to handle an unhandled exception that occurred in an event handler.

## V1.0.1

### Bug fix

- Fixed the KeyFilter of the ControllerTypeFinder not to search the type name of the data context with the Key property of the ViewAttribute if it is specified.
- Fixed the RaiseAsync method in order to be able to handle the event asynchronously with not only the EventHandlerAction but also any action.