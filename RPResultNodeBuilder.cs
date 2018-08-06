using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace RoslynPath
{
    class RPResultNodeBuilder : IRPResultNodeBuilder
    {
        private static readonly Dictionary<Type, Type> _rpElementRPResultNodeBuilderPairs;

        static RPResultNodeBuilder()
        {
            _rpElementRPResultNodeBuilderPairs = new Dictionary<Type, Type>();

            IEnumerable<Type> concreteElementBuilders = Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => typeof(IRPResultNodeBuilder).IsAssignableFrom(t) && !t.IsAbstract && !t.IsInterface);

            foreach (Type concreteElementBuilder in concreteElementBuilders)
            {
                RPResultNodeBuilderAttribute attribute = concreteElementBuilder.GetCustomAttributes(typeof(RPResultNodeBuilderAttribute), false)
                    .Cast<RPResultNodeBuilderAttribute>()
                    // Only one RPResultNodeBuilderAttribute is allowed
                    .FirstOrDefault();

                if (attribute != null)
                    _rpElementRPResultNodeBuilderPairs[attribute.ElementType] = concreteElementBuilder;
            }
        }

        public virtual IRPResultNode EvaluateElement(IRPResultNode resultNode, IEnumerable<IRPElement> elements)
        {
            if (!elements.Any())
                return resultNode;

            IRPElement element = elements.First();

            if (!_rpElementRPResultNodeBuilderPairs.ContainsKey(element.GetType()))
                throw new Exception($"{element.GetType()} does not have an associated RPResultNodeBuilder");

            Type rpResultNodeBuilderType = _rpElementRPResultNodeBuilderPairs[element.GetType()];

            if (rpResultNodeBuilderType.GetMethod("EvaluateElement").DeclaringType == typeof(RPResultNodeBuilder))
                throw new Exception($"{rpResultNodeBuilderType} does not implement the EvaluateElement method.");

            IRPResultNodeBuilder resultNodeBuilder = (IRPResultNodeBuilder)Activator.CreateInstance(rpResultNodeBuilderType);

            return resultNodeBuilder.EvaluateElement(resultNode, elements);
        }
    }
}
