using System.IO;
using System.Linq;
using DesperateDevs.CodeGeneration;

namespace Entitas.CodeGeneration.Plugins {

    public class ComponentContextApiGenerator : AbstractGenerator {

        public override string name { get { return "Component (Context API)"; } }

        const string STANDARD_COMPONENT_PROPERTY_TEMPLATE = @"${componentName}Entity.${componentName}";
        const string INLINED_COMPONENT_PROPERTY_TEMPLATE = @"(${ComponentType})${componentName}Entity.GetComponent(${Index})";

        const string STANDARD_COMPONENT_ADD_TEMPLATE = @"        entity.Add${ComponentName}(${newMethodArgs});";
        const string INLINED_COMPONENT_ADD_TEMPLATE  = @"        var index = ${Index};
        var component = (${ComponentType})entity.CreateComponent(index, typeof(${ComponentType}));
        entity.AddComponent(index, component);";

        const string STANDARD_COMPONENT_REPLACE_TEMPLATE = @"            entity.Replace${ComponentName}(${newMethodArgs});";
        const string INLINED_COMPONENT_REPLACE_TEMPLATE  = @"            var index = ${Index};
            var component = (${ComponentType})entity.CreateComponent(index, typeof(${ComponentType}));
            entity.ReplaceComponent(index, component);";

        const string STANDARD_TEMPLATE =
            @"public partial class ${ContextType} {

    public ${EntityType} ${componentName}Entity { get { return GetGroup(${MatcherType}.${ComponentName}).GetSingleEntity(); } }
    public ${ComponentType} ${validComponentName} { get { return ${ComponentProperty}; } }
    public bool has${ComponentName} { get { return ${componentName}Entity != null; } }

    public ${EntityType} Set${ComponentName}(${newMethodParameters}) {
        if (has${ComponentName}) {
            throw new Entitas.EntitasException(""Could not set ${ComponentName}!\n"" + this + "" already has an entity with ${ComponentType}!"",
                ""You should check if the context already has a ${componentName}Entity before setting it or use context.Replace${ComponentName}()."");
        }
        var entity = CreateEntity();
${ComponentAdd}
        return entity;
    }

    public void Replace${ComponentName}(${newMethodParameters}) {
        var entity = ${componentName}Entity;
        if (entity == null) {
            entity = Set${ComponentName}(${newMethodArgs});
        } else {
${ComponentReplace}
        }
    }

    public void Remove${ComponentName}() {
        ${componentName}Entity.Destroy();
    }
}
";

        const string FLAG_TEMPLATE =
            @"public partial class ${ContextType} {

    public ${EntityType} ${componentName}Entity { get { return GetGroup(${MatcherType}.${ComponentName}).GetSingleEntity(); } }

    public bool ${prefixedComponentName} {
        get { return ${componentName}Entity != null; }
        set {
            var entity = ${componentName}Entity;
            if (value != (entity != null)) {
                if (value) {
                    CreateEntity().${prefixedComponentName} = true;
                } else {
                    entity.Destroy();
                }
            }
        }
    }
}
";

        public override CodeGenFile[] Generate(CodeGeneratorData[] data) {
            return data
                .OfType<ComponentData>()
                .Where(d => d.ShouldGenerateContextMethods())
                .Where(d => d.IsUnique())
                .SelectMany(generate)
                .ToArray();
        }

        CodeGenFile[] generate(ComponentData data) {
            return data.GetContextNames()
                .Select(contextName => generate(contextName, data))
                .ToArray();
        }

        CodeGenFile generate(string contextName, ComponentData data) {
            var template = data.GetMemberData().Length == 0
                ? FLAG_TEMPLATE
                : STANDARD_TEMPLATE;

            bool useInline = !data.ShouldGenerateEntityMethods(); // inline if not generating entity methods

            var fileContent = template
                .Replace("${ComponentProperty}", useInline ? INLINED_COMPONENT_PROPERTY_TEMPLATE : STANDARD_COMPONENT_PROPERTY_TEMPLATE)
                .Replace("${ComponentAdd}", useInline ? INLINED_COMPONENT_ADD_TEMPLATE : STANDARD_COMPONENT_ADD_TEMPLATE)
                .Replace("${ComponentReplace}", useInline ? INLINED_COMPONENT_REPLACE_TEMPLATE : STANDARD_COMPONENT_REPLACE_TEMPLATE)
                .Replace(data, contextName);

            return new CodeGenFile(
                contextName + Path.DirectorySeparatorChar +
                "Components" + Path.DirectorySeparatorChar +
                data.ComponentNameWithContext(contextName).AddComponentSuffix() + ".cs",
                fileContent,
                GetType().FullName
            );
        }
    }
}
