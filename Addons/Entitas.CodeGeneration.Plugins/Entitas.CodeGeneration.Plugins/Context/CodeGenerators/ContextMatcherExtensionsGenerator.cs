using System.IO;
using System.Linq;
using DesperateDevs.CodeGeneration;

namespace Entitas.CodeGeneration.Plugins {

    public class ContextMatcherExtensionsGenerator : ICodeGenerator {

        public string name { get { return "Context Extensions (Matcher API)"; } }
        public int priority { get { return 0; } }
        public bool runInDryMode { get { return true; } }

        const string TEMPLATE =
            @"public static partial class ${MatcherType}Extensions {
    public static Entitas.INoneOfMatcher<${EntityType}> NoneOf(this Entitas.IAnyOfMatcher<${EntityType}> matcher, params System.Type[] types) {
        return matcher.NoneOf(${Lookup}.GetComponentIndices(types));
    }

    public static Entitas.IAnyOfMatcher<${EntityType}> AnyOf(this Entitas.IAllOfMatcher<${EntityType}> matcher, params System.Type[] types) {
        return matcher.AnyOf(${Lookup}.GetComponentIndices(types));
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
                contextName.AddMatcherSuffix() + "Extensions.cs",
                TEMPLATE.Replace(contextName),
                GetType().FullName
            );
        }
    }
}
