// Copyright (C) 2018 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System.Collections.Generic;

namespace Charites.Windows.Mvc
{
    public class TestElement
    {
        public string Name { get; }
        public IEnumerable<TestElement> Children => children.AsReadOnly();
        private readonly List<TestElement> children = new List<TestElement>();

        public bool IsLoaded { get; private set; }

        public TestElement(string name) => Name = name;

        public TestElement(string name, params TestElement[] children) : this(name)
        {
            this.children.AddRange(children);
        }

        public void Load() => IsLoaded = true;
    }
}
