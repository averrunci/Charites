// Copyright (C) 2022 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
namespace Charites.Windows.Mvc;

internal sealed class EventHandlerItemTss : EventHandlerItem<TestElement>
{
    public bool AddEventHandlerCalled => addEventHandlerCalled && !removeEventHandlerCalled;
    public bool RemoveEventHandlerCalled => !addEventHandlerCalled && removeEventHandlerCalled;

    private readonly TestElement? element;
    private readonly Delegate? handler;
    private readonly bool handledEventsToo;

    private bool addEventHandlerCalled;
    private bool removeEventHandlerCalled;

    public EventHandlerItemTss(string elementName, TestElement? element, string eventName, Delegate? handler, bool handledEventsToo) : base(elementName, element, eventName, handler, handledEventsToo)
    {
        this.element = element;
        this.handler = handler;
        this.handledEventsToo = handledEventsToo;
    }
    public EventHandlerItemTss(string elementName, TestElement? element, string eventName, Delegate? handler, bool handledEventsToo, IEnumerable<IEventHandlerParameterResolver> parameterResolver) : base(elementName, element, eventName, handler, handledEventsToo, parameterResolver)
    {
        this.element = element;
        this.handler = handler;
        this.handledEventsToo = handledEventsToo;
    }

    public void ClearCalled()
    {
        addEventHandlerCalled = false;
        removeEventHandlerCalled = false;
    }

    protected override void AddEventHandler(TestElement element, Delegate handler, bool handledEventsToo)
    {
        addEventHandlerCalled = this.element == element && this.handler == handler && this.handledEventsToo == handledEventsToo;
    }

    protected override void RemoveEventHandler(TestElement element, Delegate handler)
    {
        removeEventHandlerCalled = this.element == element && this.handler == handler;
    }
}