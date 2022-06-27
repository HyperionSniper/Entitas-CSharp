using System.IO;
using System.Linq;
using DesperateDevs.CodeGeneration;

namespace Entitas.CodeGeneration.Plugins {

    public class ContextAttributeGenerator : ICodeGenerator {

        public string name { get { return "Context (Attribute)"; } }
        public int priority { get { return 0; } }
        public bool runInDryMode { get { return true; } }

        const string TEMPLATE =
            @"public sealed class ${ContextName}ContextAttribute : Entitas.CodeGeneration.Attributes.ContextAttribute {

    public ${ContextName}ContextAttribute() : base(""${ContextName}"") {
    }
}
";

        public CodeGenFile[] Generate(CodeGeneratorData[] data) {
            return data
                .OfType<ContextData>()
                .Select(generate)
                .ToArray();
        }

        CodeGenFile generate(ContextData data) {
            var contextName = data.GetContextName();
            return new CodeGenFile(
                contextName + Path.DirectorySeparatorChar +
                contextName + "ContextAttribute.cs",
                TEMPLATE.Replace(contextName),
                GetType().FullName
            );
        }
    }
}
