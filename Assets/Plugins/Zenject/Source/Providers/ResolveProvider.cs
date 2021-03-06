using System;
using System.Collections.Generic;
using System.Linq;
using ModestTree;

namespace Zenject
{
    public class ResolveProvider : IProvider
    {
        readonly object _identifier;
        readonly DiContainer _container;
        readonly Type _contractType;
        readonly bool _isOptional;
        readonly InjectSources _source;

        public ResolveProvider(
            Type contractType, DiContainer container, object identifier,
            bool isOptional, InjectSources source)
        {
            _contractType = contractType;
            _identifier = identifier;
            _container = container;
            _isOptional = isOptional;
            _source = source;
        }

        public Type GetInstanceType(InjectContext context)
        {
            return _contractType;
        }

        public IEnumerator<List<object>> GetAllInstancesWithInjectSplit(InjectContext context, List<TypeValuePair> args)
        {
            Assert.IsEmpty(args);
            Assert.IsNotNull(context);

            Assert.That(_contractType.DerivesFromOrEqual(context.MemberType));

            yield return _container.ResolveAll(GetSubContext(context)).Cast<object>().ToList();
        }

        InjectContext GetSubContext(InjectContext parent)
        {
            var subContext = parent.CreateSubContext(_contractType, _identifier);

            subContext.SourceType = _source;
            subContext.Optional = _isOptional;

            return subContext;
        }
    }
}
