// Copyright (C) 2022 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
namespace Charites.Windows.Mvc;

internal sealed class ElementInjectorTss : ElementInjector<TestElement>
{
    protected override object? FindElement(TestElement? rootElement, string elementName)
        => rootElement?.Name == elementName ? rootElement : rootElement?.Children.FirstOrDefault(element => element.Name == elementName);
}