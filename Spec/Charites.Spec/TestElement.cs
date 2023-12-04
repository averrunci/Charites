// Copyright (C) 2022-2023 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
namespace Charites.Windows.Mvc;

public class TestElement(string name)
{
    public string Name { get; } = name;
    public IEnumerable<TestElement> Children => children.AsReadOnly();
    private readonly List<TestElement> children = new();

    public bool IsLoaded { get; private set; }

    public TestElement(string name, params TestElement[] children) : this(name)
    {
        this.children.AddRange(children);
    }

    public void Load() => IsLoaded = true;
}