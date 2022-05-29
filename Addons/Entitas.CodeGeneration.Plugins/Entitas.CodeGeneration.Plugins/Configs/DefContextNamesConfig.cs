using DesperateDevs.Serialization;
using DesperateDevs.Utils;
using System.Collections.Generic;

namespace Entitas.CodeGeneration.Plugins {
#if TYPEDEF_CODEGEN
    public class DefContextNamesConfig : AbstractConfigurableConfig {

        const string CONTEXTS_KEY = "Entitas.CodeGeneration.Plugins.DefContexts";

        public override Dictionary<string, string> defaultProperties {
            get {
                return new Dictionary<string, string> {
                    { CONTEXTS_KEY, "" }
                };
            }
        }

        public string[] contextNames {
            get { return _preferences[CONTEXTS_KEY].ArrayFromCSV(); }
            set { _preferences[CONTEXTS_KEY] = value.ToCSV(); }
        }
    }
#endif
}
