﻿// Copyright (C) 2018 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System;
using System.Collections.Generic;

namespace Charites.Windows.Mvc
{
    internal sealed class ControllerTypeFinderTss : ControllerTypeFinder<TestElement>
    {
        public ControllerTypeFinderTss(IElementKeyFinder<TestElement> elementKeyFinder, IDataContextFinder<TestElement> dataContextFinder) : base(elementKeyFinder, dataContextFinder)
        {
        }

        protected override IEnumerable<Type> FindControllerTypeCandidates(TestElement view)
            => typeof(TestControllers).GetNestedTypes();
    }
}
