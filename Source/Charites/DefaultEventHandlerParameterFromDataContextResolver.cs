﻿// Copyright (C) 2022 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
namespace Charites.Windows.Mvc;

internal sealed class DefaultEventHandlerParameterFromDataContextResolver : EventHandlerParameterFromDataContextResolver
{
    private readonly object? dataContext;

    public DefaultEventHandlerParameterFromDataContextResolver(object? dataContext) : base(null)
    {
        this.dataContext = dataContext;
    }

    protected override object? FindDataContext() => dataContext;
}