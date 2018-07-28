# Release note

## V1.0.1

### Bug fix

- Fixed the KeyFilter of the ControllerTypeFinder not to search the type name of the data context with the Key property of the ViewAttribute if it is specified.
- Fixed the RaiseAsync method in order to be able to handle the event asynchronously with not only the EventHandlerAction but also any action.