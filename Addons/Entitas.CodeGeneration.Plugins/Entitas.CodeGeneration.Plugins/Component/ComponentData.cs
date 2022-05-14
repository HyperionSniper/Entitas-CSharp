using DesperateDevs.CodeGeneration;
using DesperateDevs.Utils;

namespace Entitas.CodeGeneration.Plugins {

    public class ComponentData : CodeGeneratorData {

        public ComponentData() {
        }

        public ComponentData(CodeGeneratorData data) : base(data) {
        }
    }

    // ComponentDataExtension moved to Entitas.csproj => Entitas/Extensions/Generics
}
