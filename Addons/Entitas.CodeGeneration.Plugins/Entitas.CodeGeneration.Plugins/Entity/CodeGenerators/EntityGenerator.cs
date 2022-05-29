using System.IO;
using System.Linq;
using DesperateDevs.CodeGeneration;

namespace Entitas.CodeGeneration.Plugins {

    public class EntityGenerator : ICodeGenerator {

        public string name { get { return "Entity"; } }
        public int priority { get { return 0; } }
        public bool runInDryMode { get { return true; } }

#if TYPEDEF_CODEGEN
        const string TYPEDEF_TEMPLATE =
            @"public sealed partial class ${EntityType} : Entitas.DefEntity {
}
";
#endif

        const string TEMPLATE =
            @"public sealed partial class ${EntityType} : Entitas.Entity {
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

#if TYPEDEF_CODEGEN
            string fileContent;
            if (data.IsDefContext()) {
                fileContent = TYPEDEF_TEMPLATE;
            }
            else {
                fileContent = TEMPLATE;
            }
#else
            var fileContent = TEMPLATE;
#endif

            fileContent = fileContent
                .Replace(contextName);

            return new CodeGenFile(
                contextName + Path.DirectorySeparatorChar +
                contextName.AddEntitySuffix() + ".cs",
                fileContent,
                GetType().FullName
            );
        }
    }
}
