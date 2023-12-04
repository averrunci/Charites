﻿// Copyright (C) 2022-2023 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
namespace Charites.Windows.Mvc;

internal sealed class ControllerTypeFinderTss(
    IElementKeyFinder<TestElement> elementKeyFinder,
    IDataContextFinder<TestElement> dataContextFinder
) : ControllerTypeFinder<TestElement>(elementKeyFinder, dataContextFinder)
{
    protected override IEnumerable<Type> FindControllerTypeCandidates(TestElement view)
        => typeof(TestControllers).GetNestedTypes();
}