// Copyright (C) 2022-2023 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
namespace Charites.Windows.Mvc;

internal static class EventHandlerParameterResolvers
{
    public class EventHandlerParameterFromDIResolverTss(IDictionary<Type, Func<object?>> resolver) : EventHandlerParameterFromDIResolver(null)
    {
        protected override object? CreateParameter(Type parameterType) => resolver.ContainsKey(parameterType) ? resolver[parameterType]() : null;
    }

    public class EventHandlerParameterFromElementResolverTss() : EventHandlerParameterFromElementResolver(null)
    {
        protected override object FindElement(string name) => new TestElement(name);
    }

    public class EventHandlerParameterFromDataContextResolverTss(object? dataContext) : EventHandlerParameterFromDataContextResolver(null)
    {
        protected override object? FindDataContext() => dataContext;
    }
}