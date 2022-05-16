using System;

namespace Entitas.CodeGeneration.Plugins {
    public class IsDefComponentDataProvider : IComponentDataProvider {

        public void Provide(Type type, ComponentData data) {
            //var isDefComponent = typeof(TypeAspect).IsAssignableFrom(type);

            bool isDefComponent = false;
            var curr = type;
            while (curr != null) {
                if (curr.Name == "TypeAspect") {
                    isDefComponent = true;
                    break;
                }
                curr = curr.BaseType;
            }

            data.IsDefComponent(isDefComponent);
        }
    }

    public static class DefComponentDataProviderExtension {

        public const string COMPONENT_TYPE = "Component.IsDef";

        public static bool IsDefComponent(this ComponentData data) {
            return (bool)data[COMPONENT_TYPE];
        }

        public static void IsDefComponent(this ComponentData data, bool isDefComponent) {
            data[COMPONENT_TYPE] = isDefComponent;
        }
    }
}