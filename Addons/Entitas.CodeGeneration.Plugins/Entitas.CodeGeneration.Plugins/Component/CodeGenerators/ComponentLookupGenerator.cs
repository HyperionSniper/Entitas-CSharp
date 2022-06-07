using System.Collections.Generic;
using System.Linq;
using System.IO;
using DesperateDevs.CodeGeneration;

namespace Entitas.CodeGeneration.Plugins {

    public class ComponentLookupGenerator : AbstractGenerator {

        public override string name { get { return "Component (Lookup)"; } }

        const string TEMPLATE =
            @"public static class ${Lookup} {

${componentConstantsList}

${totalComponentsConstant}

    private static readonly string[] _componentNames = {
${componentNamesList}
    };

    private static readonly System.Type[] _componentTypes = {
${componentTypesList}
    };

    static ${Lookup}() { Reset(); }

    private static readonly System.Collections.Generic.Dictionary<System.Type, int> _componentDict = new System.Collections.Generic.Dictionary<System.Type, int>();
    private static readonly System.Collections.Generic.List<string> _nameBuffer = new System.Collections.Generic.List<string>();
    private static readonly System.Collections.Generic.List<System.Type> _typeBuffer = new System.Collections.Generic.List<System.Type>();

    public static int TotalComponents { get { return _nameBuffer.Count; } }
    public static string[] componentNames { get { return _nameBuffer.ToArray(); } }
    public static System.Type[] componentTypes { get { return _typeBuffer.ToArray(); } }

    public static int GetComponentIndex<T>() where T : Entitas.IComponent {
        System.Type type = typeof(T);

        return GetComponentIndex(type);
    }

    public static int GetComponentIndex(System.Type type) {
        if(!_componentDict.ContainsKey(type))
            throw new Entitas.ComponentIsNotRegisteredException(""${ContextName}"", type);

        return _componentDict[type];
    }

    public static int[] GetComponentIndices(System.Type[] types) {
        return System.Linq.Enumerable.ToArray(
            System.Linq.Enumerable.Select(types, t => GetComponentIndex(t)));
    }

    public static void Reset() {
        _componentDict.Clear();
        _nameBuffer.Clear();
        _typeBuffer.Clear();
        for (int i = 0; i < _componentNames.Length; i++) {
            registerComponentType(_componentNames[i], _componentTypes[i]);
        }
    }

    public static void RegisterComponentType<T>() where T : Entitas.IComponent {
        System.Type type = typeof(T);
        string componentName = Entitas.Extensions.Typed.ComponentExtension.ComponentName(type);
        registerComponentType(componentName, type);
    }

    private static void registerComponentType(string componentName, System.Type type) {
        if(_componentDict.ContainsKey(type)) return;

        _componentDict.Add(type, _nameBuffer.Count);
        _nameBuffer.Add(componentName);
        _typeBuffer.Add(type);
    }
}
";

        const string COMPONENT_CONSTANT_TEMPLATE = @"    public const int ${ComponentName} = ${Index};";
        const string TOTAL_COMPONENTS_CONSTANT_TEMPLATE = @"    private const int _totalComponents = ${totalComponents};";
        const string COMPONENT_NAME_TEMPLATE = @"        ""${ComponentName}""";
        const string COMPONENT_TYPE_TEMPLATE = @"        typeof(${ComponentType})";

        public override CodeGenFile[] Generate(CodeGeneratorData[] data) {
            var lookups = generateLookups(data
                .OfType<ComponentData>()
                .Where(d => d.ShouldGenerateIndex())
                .ToArray());

            var existingFileNames = new HashSet<string>(lookups.Select(file => file.fileName));

            var emptyLookups = generateEmptyLookups(data
                    .OfType<ContextData>()
                    .ToArray())
                .Where(file => !existingFileNames.Contains(file.fileName))
                .ToArray();

            return lookups.Concat(emptyLookups).ToArray();
        }

        CodeGenFile[] generateEmptyLookups(ContextData[] data) {
            var emptyData = new ComponentData[0];
            return data
                .Select(d => generateComponentsLookupClass(d.GetContextName(), emptyData))
                .ToArray();
        }

        CodeGenFile[] generateLookups(ComponentData[] data) {
            var contextNameToComponentData = data
                .Aggregate(new Dictionary<string, List<ComponentData>>(), (dict, d) => {
                    var contextNames = d.GetContextNames();
                    foreach (var contextName in contextNames) {
                        if (!dict.ContainsKey(contextName)) {
                            dict.Add(contextName, new List<ComponentData>());
                        }

                        dict[contextName].Add(d);
                    }

                    return dict;
                });

            foreach (var key in contextNameToComponentData.Keys.ToArray()) {
                contextNameToComponentData[key] = contextNameToComponentData[key]
                    .OrderBy(d => d.GetTypeName())
                    .ToList();
            }

            return contextNameToComponentData
                .Select(kv => generateComponentsLookupClass(kv.Key, kv.Value.ToArray()))
                .ToArray();
        }

        CodeGenFile generateComponentsLookupClass(string contextName, ComponentData[] data) {
            var componentConstantsList = string.Join("\n", data
                .Select((d, index) => COMPONENT_CONSTANT_TEMPLATE
                    .Replace("${ComponentName}", d.ComponentName())
                    .Replace("${Index}", index.ToString())).ToArray());

            var totalComponentsConstant = TOTAL_COMPONENTS_CONSTANT_TEMPLATE
                .Replace("${totalComponents}", data.Length.ToString());

            var componentNamesList = string.Join(",\n", data
                .Select(d => COMPONENT_NAME_TEMPLATE
                    .Replace("${ComponentName}", d.ComponentName())
                ).ToArray());

            var componentTypesList = string.Join(",\n", data
                .Select(d => COMPONENT_TYPE_TEMPLATE
                    .Replace("${ComponentType}", d.GetTypeName())
                ).ToArray());

            var fileContent = TEMPLATE
                .Replace("${Lookup}", contextName + CodeGeneratorExtentions.LOOKUP)
                .Replace("${componentConstantsList}", componentConstantsList)
                .Replace("${totalComponentsConstant}", totalComponentsConstant)
                .Replace("${componentNamesList}", componentNamesList)
                .Replace("${componentTypesList}", componentTypesList);

            return new CodeGenFile(
                contextName + Path.DirectorySeparatorChar +
                contextName + "ComponentsLookup.cs",
                fileContent,
                GetType().FullName
            );
        }
    }
}

//public static class ExampleLookup {
//    private static readonly string[] _componentNames;
//    private static readonly System.Type[] _componentTypes;

//    //////
//    static ExampleLookup() { Reset(); }

//    private static readonly System.Collections.Generic.Dictionary<System.Type, int> _componentDict = new Dictionary<System.Type, int>();
//    private static readonly System.Collections.Generic.List<string> _nameBuffer = new List<string>();
//    private static readonly System.Collections.Generic.List<System.Type> _typeBuffer = new List<System.Type>();

//    public static int TotalComponents { get { return _nameBuffer.Count; } }
//    public static string[] componentNames { get { return _nameBuffer.ToArray(); } }
//    public static System.Type[] componentTypes { get { return _typeBuffer.ToArray(); } }

//    public static int GetComponentIndex<T>() where T : Entitas.IComponent {
//        System.Type type = typeof(T);

//        return GetComponentIndex(type);
//    }

//    public static int GetComponentIndex(System.Type type) {
//        return _componentDict[type];
//    }

//    public static void Reset() {
//        for (int i = 0; i < _componentNames.Length; i++) {
//            registerComponentType(_componentNames[i], _componentTypes[i]);
//        }
//    }

//    public static void RegisterComponentType<T>() where T : Entitas.IComponent {
//        System.Type type = typeof(T);
//        string componentName = Entitas.Extensions.Typed.ComponentExtension.ComponentName(type);
//        registerComponentType(componentName, type);
//    }

//    private static void registerComponentType(string componentName, System.Type type) {
//        _componentDict.Add(type, _nameBuffer.Count);
//        _nameBuffer.Add(componentName);
//        _typeBuffer.Add(type);
//    }
//}