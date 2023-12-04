// Copyright (C) 2022-2023 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
namespace Charites.Windows.Mvc;

internal sealed class ControllerCollectionTss(
    IDataContextFinder<TestElement> dataContextFinder,
    IDataContextInjector dataContextInjector,
    IElementInjector<TestElement> elementInjector,
    IEnumerable<IControllerExtension<TestElement>> extensions
) : ControllerCollection<TestElement>(dataContextFinder, dataContextInjector, elementInjector, extensions)
{
    public bool AssociatedElementEventsSubscribed { get; private set; }
    public bool AssociatedElementEventsUnsubscribed { get; private set; }

    public void ClearSubscribedUnsubscribed()
    {
        AssociatedElementEventsSubscribed = false;
        AssociatedElementEventsUnsubscribed = false;
    }

    protected override bool IsAssociatedElementLoaded(TestElement associatedElement) => associatedElement.IsLoaded;
    protected override void SubscribeAssociatedElementEvents(TestElement associatedElement) => AssociatedElementEventsSubscribed = true;
    protected override void UnsubscribeAssociatedElementEvents(TestElement associatedElement) => AssociatedElementEventsUnsubscribed = true;
}