using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace CygSoft.Qik.Functions
{
    public interface IFunctionFactory
    {
        IFunction GetFunction(string name, List<IFunction> functionArguments);
    }

    public class FunctionFactory : IFunctionFactory
    {
        private readonly IPluginLoader pluginLoader;
        Dictionary<string, System.Type> functionTypes = new Dictionary<string, Type>();
        
        public FunctionFactory(IPluginLoader pluginLoader)
        {
            this.pluginLoader = pluginLoader;
            this.pluginLoader.Load();
        }

        public IFunction GetFunction(string name, List<IFunction> functionArguments)
        {
            if (name is null) throw new ArgumentNullException($"{nameof(name)} cannot be null.");
            if (functionArguments is null) throw new ArgumentNullException($"{nameof(functionArguments)} cannot be null.");

            IFunction func;

            switch (name)
            {
                case "camelCase":
                    func = new CamelCaseFunction(name, functionArguments);
                    break;

                case "currentDate":
                    func = new CurrentDateFunction(name, functionArguments);
                    break;

                case "lowerCase":
                    func = new LowerCaseFunction(name, functionArguments);
                    break;

                case "upperCase":
                    func = new UpperCaseFunction(name, functionArguments);
                    break;

                case "properCase":
                    func = new ProperCaseFunction(name, functionArguments);
                    break;

                case "removeSpaces":
                    func = new RemoveSpacesFunction(name, functionArguments);
                    break;

                case "removePunctuation":
                    func = new RemovePunctuationFunction(name, functionArguments);
                    break;

                case "replace":
                    func = new ReplaceFunction(name, functionArguments);
                    break;

                case "indentLine":
                    func = new IndentFunction(name, functionArguments);
                    break;

                case "doubleQuote":
                    func = new DoubleQuoteFunction(name, functionArguments);
                    break;

                case "htmlEncode":
                    func = new HtmlEncodeFunction(name, functionArguments);
                    break;

                case "htmlDecode":
                    func = new HtmlDecodeFunction(name, functionArguments);
                    break;

                case "urlEncode":
                    func = new UrlEncodeFunction(name, functionArguments);
                    break;

                case "urlDecode":
                    func = new UrlDecodeFunction(name, functionArguments);
                    break;

                case "base64Encode":
                    func = new Base64EncodeFunction(name, functionArguments);
                    break;

                case "base64Decode":
                    func = new Base64DecodeFunction(name, functionArguments);
                    break;

                case "guid":
                    var guidFunction = new GuidFunction(name, functionArguments);
                    func = guidFunction;
                    break;
                case "padLeft":
                    func = new PadLeftFunction(name, functionArguments);
                    break;
                case "padRight":
                    func = new PadRightFunction(name, functionArguments);
                    break;
                case "abbreviate":
                    func = new AbbreviateFunction(name, functionArguments);
                    break;

                default:
                    func = pluginLoader.GetFunction(name, functionArguments);
                    break;
            }

            if (func == null)
                throw new NotSupportedException(string.Format("Function \"{0}\" is not supported in this context.", name));

            return func;
        }
    }
}
