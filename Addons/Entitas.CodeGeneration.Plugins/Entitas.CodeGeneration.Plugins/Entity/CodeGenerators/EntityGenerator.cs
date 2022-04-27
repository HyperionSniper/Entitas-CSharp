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
    public sealed partial class DefWrapper : DefWrapperBase {
        public DefWrapper(Entitas.DefEntity entity, Hyperion.Defs.TypeDef typeDef) : base(entity, typeDef) { }
    }

    protected override DefWrapperBase CreateDefWrapper(Entitas.DefEntity entity, Hyperion.Defs.TypeDef typeDef) {
        return new DefWrapper(entity, typeDef);
    }
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
            var fileContent = TYPEDEF_TEMPLATE;
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
