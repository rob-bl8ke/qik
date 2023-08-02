
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace CygSoft.Qik.Functions
{
    public interface IPluginLoader
    {
        void Load();
        IFunction GetFunction(string name, List<IFunction> functionArguments);
    }

    public class PluginLoader : IPluginLoader
    {
        Dictionary<string, System.Type> functionTypes = new Dictionary<string, Type>();

        public IFunction GetFunction(string name, List<IFunction> functionArguments)
        {
            var success = functionTypes.TryGetValue(name, out Type functionType);
            if (success)
            {
                var function = (IFunction)Activator.CreateInstance(functionType, new object[] { name, functionArguments });
                return function;
            }
            return null;
        }

        public void Load()
        {
            var pluginFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Plugins"); 

            if (!Directory.Exists(pluginFolder))
                return;

            string[] files = Directory.GetFiles(pluginFolder, "*.dll", SearchOption.TopDirectoryOnly);

            foreach (var file in files)
            {
                var assembly = Assembly.LoadFile(file);      

                foreach (var type in assembly.GetTypes())
                {
                    if (type.BaseType == typeof(BaseFunction))
                    {
                        var attrs = System.Attribute.GetCustomAttributes(type);
                        foreach (var attr in attrs)
                        {
                            if (attr is QikFunctionAttribute)
                            {
                                var functionAttribute = (QikFunctionAttribute)attr;
                                functionTypes.Add(functionAttribute.Symbol, type);
                            }
                        }
                    }
                }
            }
        }
    }
}