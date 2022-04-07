using System;
using System.Linq;
using DesperateDevs.Utils;
using Entitas.CodeGeneration.Attributes;

namespace Entitas.CodeGeneration.Plugins {

    public class ShouldGenerateMethodsComponentDataProvider : IComponentDataProvider {

        public void Provide(Type type, ComponentData data) {
            var generate = !Attribute
                    .GetCustomAttributes(type)
                    .OfType<DontGenerateAttribute>()
                    .Any();
            var generateEntity = generate && !type.ImplementsInterface<IContextComponent>();

            data.ShouldGenerateContextMethods(generate);
            data.ShouldGenerateEntityMethods(generateEntity);
        }
    }

    public static class ShouldGenerateMethodsComponentDataExtension {

        public const string COMPONENT_GENERATE_CONTEXT_METHODS = "Component.Generate.ContextMethods";
        public const string COMPONENT_GENERATE_ENTITY_METHODS = "Component.Generate.EntityMethods";

        public static bool ShouldGenerateContextMethods(this ComponentData data) {
            return (bool)data[COMPONENT_GENERATE_CONTEXT_METHODS];
        }

        public static void ShouldGenerateContextMethods(this ComponentData data, bool generate) {
            data[COMPONENT_GENERATE_CONTEXT_METHODS] = generate;
        }
        public static bool ShouldGenerateEntityMethods(this ComponentData data) {
            return (bool)data[COMPONENT_GENERATE_ENTITY_METHODS];
        }

        public static void ShouldGenerateEntityMethods(this ComponentData data, bool generate) {
            data[COMPONENT_GENERATE_ENTITY_METHODS] = generate;
        }
    }
}
