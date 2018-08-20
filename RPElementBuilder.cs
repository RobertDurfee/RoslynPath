using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace RoslynPath
{
    internal class RPElementBuilder : IRPElementBuilder
    {
        protected Dictionary<string, string> Options { get; set; }
        public IRPElement Element { get; protected set; }

        private static readonly Dictionary<Type, Type> _rpTokenTypeRPElementBuilderPairs;

        static RPElementBuilder()
        {
            _rpTokenTypeRPElementBuilderPairs = new Dictionary<Type, Type>();

            IEnumerable<Type> concreteElementBuilders = Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => typeof(IRPElementBuilder).IsAssignableFrom(t) && !t.IsAbstract && !t.IsInterface);

            foreach (Type concreteElementBuilder in concreteElementBuilders)
            {
                RPElementBuilderAttribute attribute = concreteElementBuilder.GetCustomAttributes(typeof(RPElementBuilderAttribute), false)
                    .Cast<RPElementBuilderAttribute>()
                    // Only one RPElementBuilderAttribute is allowed
                    .FirstOrDefault();

                if (attribute != null)
                    _rpTokenTypeRPElementBuilderPairs[attribute.TokenType] = concreteElementBuilder;
            }
        }

        public RPElementBuilder() { }

        protected RPElementBuilder(Dictionary<string, string> options, IRPElement element)
        {
            Options = options;
            Element = element;
        }

        public void Clean()
        {
            Options = new Dictionary<string, string>();
            Element = null;
        }

        public virtual int ConvertTokens(IEnumerable<RPToken> tokens)
        {
            RPToken token = tokens.First();

            if (!_rpTokenTypeRPElementBuilderPairs.ContainsKey(token.TokenType))
                throw new Exception($"{token.TokenType} does not have an associated RPElementBuilder");

            Type rpElementBuilderType = _rpTokenTypeRPElementBuilderPairs[token.TokenType];

            if (rpElementBuilderType.GetMethod("ConvertTokens").DeclaringType == typeof(RPElementBuilder))
                throw new Exception($"{rpElementBuilderType} does not implement the ConvertTokens method.");

            IRPElementBuilder elementBuilder;

            if (Options.ContainsKey("IsComplex") && Options["IsComplex"] == "True")
                elementBuilder = new RPComplexElementBuilder(Options, Element);
            else
                elementBuilder = (IRPElementBuilder)Activator.CreateInstance(rpElementBuilderType, new object[] { Options, Element });

            int tokensConsumed = elementBuilder.ConvertTokens(tokens);

            Element = elementBuilder.Element;

            return tokensConsumed;
        }
    }
}
