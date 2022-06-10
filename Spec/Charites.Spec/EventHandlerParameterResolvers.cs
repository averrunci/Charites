// Copyright (C) 2022 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
namespace Charites.Windows.Mvc;

internal static class EventHandlerParameterResolvers
{
    public class EventHandlerParameterFromDIResolverTss : EventHandlerParameterFromDIResolver
    {
        private readonly IDictionary<Type, Func<object?>> resolver;

        public EventHandlerParameterFromDIResolverTss(IDictionary<Type, Func<object?>> resolver) : base(null)
        {
            this.resolver = resolver;
        }

        protected override object? CreateParameter(Type parameterType) => resolver.ContainsKey(parameterType) ? resolver[parameterType]() : null;
    }

    public class EventHandlerParameterFromElementResolverTss : EventHandlerParameterFromElementResolver
    {
        public EventHandlerParameterFromElementResolverTss() : base(null)
        {
        }

        protected override object FindElement(string name) => new TestElement(name);
    }

    public class EventHandlerParameterFromDataContextResolverTss : EventHandlerParameterFromDataContextResolver
    {
        private readonly object? dataContext;

        public EventHandlerParameterFromDataContextResolverTss(object? dataContext) : base(null)
        {
            this.dataContext = dataContext;
        }

        protected override object? FindDataContext() => dataContext;
    }
}