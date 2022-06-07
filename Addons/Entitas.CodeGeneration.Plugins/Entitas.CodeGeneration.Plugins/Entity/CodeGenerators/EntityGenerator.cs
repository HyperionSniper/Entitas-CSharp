using System.IO;
using System.Linq;
using DesperateDevs.CodeGeneration;

namespace Entitas.CodeGeneration.Plugins {

    public class EntityGenerator : ICodeGenerator {

        public string name { get { return "Entity"; } }
        public int priority { get { return 0; } }
        public bool runInDryMode { get { return true; } }

        const string TEMPLATE =
            @"public sealed partial class ${EntityType} : Entitas.${EntityBaseName} {
    public void AddComponent<T>(T component) where T : Entitas.IComponent {
        AddComponent(${Lookup}.GetComponentIndex<T>(), component);
    }

    public void RemoveComponent<T>() where T : Entitas.IComponent {
        RemoveComponent(${Lookup}.GetComponentIndex<T>());
    }

    public void ReplaceComponent<T>(T component) where T : Entitas.IComponent {
        ReplaceComponent(${Lookup}.GetComponentIndex<T>(), component);
    }

    public T GetComponent<T>() where T : Entitas.IComponent {
        return (T)GetComponent(${Lookup}.GetComponentIndex<T>());
    }

    public void AddComponent(System.Type type, Entitas.IComponent component) {
        AddComponent(${Lookup}.GetComponentIndex(type), component);
    }

    public void RemoveComponent(System.Type type) {
        RemoveComponent(${Lookup}.GetComponentIndex(type));
    }

    public void ReplaceComponent(System.Type type, Entitas.IComponent component) {
        ReplaceComponent(${Lookup}.GetComponentIndex(type), component);
    }

    public Entitas.IComponent GetComponent(System.Type type) {
        return GetComponent(${Lookup}.GetComponentIndex(type));
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

#if TYPEDEF_CODEGEN
            var fileContent = TEMPLATE.Replace("${EntityBaseName}", (data.IsDefContext() ? "DefEntity" : "Entity"));
#else
            var fileContext = TEMPLATE;
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
