using System.Collections.Generic;
using System.Linq;
using DesperateDevs.CodeGeneration;
using DesperateDevs.Serialization;

namespace Entitas.CodeGeneration.Plugins {

    public class ContextDataProvider : IDataProvider, IConfigurable {

        public string name { get { return "Context"; } }
        public int priority { get { return 0; } }
        public bool runInDryMode { get { return true; } }

        public Dictionary<string, string> defaultProperties { get { return _contextNamesConfig.defaultProperties; } }

        readonly ContextNamesConfig _contextNamesConfig = new ContextNamesConfig();

#if TYPEDEF_CODEGEN
        readonly DefContextNamesConfig _defContextNamesConfig = new DefContextNamesConfig();
#endif

        public void Configure(Preferences preferences) {
            _contextNamesConfig.Configure(preferences);
#if TYPEDEF_CODEGEN
            _defContextNamesConfig.Configure(preferences);
#endif
        }

        public CodeGeneratorData[] GetData() {
#if TYPEDEF_CODEGEN
            var defContextNames = _defContextNamesConfig.contextNames;

            return _contextNamesConfig.contextNames
                .Concat(defContextNames)
                .Distinct()
                .Select(contextName => {
                    var data = new ContextData();
                    data.SetContextName(contextName);
                    data.IsDefContext(defContextNames.Contains(contextName));

                    return data;
                }).ToArray();
#else
            return _contextNamesConfig.contextNames
                .Select(contextName => {
                    var data = new ContextData();
                    data.SetContextName(contextName);

                    return data;
                }).ToArray();
#endif
        }
    }

    public static class ContextDataExtension {

        public const string CONTEXT_NAME = "Context.Name";

        public static string GetContextName(this ContextData data) {
            return (string)data[CONTEXT_NAME];
        }

        public static void SetContextName(this ContextData data, string contextName) {
            data[CONTEXT_NAME] = contextName;
        }
    }
}

#if TYPEDEF_CODEGEN

namespace Entitas.CodeGeneration.Plugins {
    public static class DefContextDataExtension {

        public const string CONTEXT_NAME = "Context.IsDefContext";

        public static bool IsDefContext(this ContextData data) {
            return (bool)data[CONTEXT_NAME];
        }

        public static void IsDefContext(this ContextData data, bool isDefContext) {
            data[CONTEXT_NAME] = isDefContext;
        }
    }
}
#endif