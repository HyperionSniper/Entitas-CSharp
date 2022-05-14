using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entitas.Extensions.Typed {
    public static class ComponentExtension {
        public static string ComponentName(this Type type) {
            string componentName = DesperateDevs.Utils.SerializationTypeExtension.ToCompilableString(type);
            componentName = Entitas.CodeGeneration.Plugins.ComponentDataExtension.ToComponentName(componentName, false);

            return componentName;
        }
    }
}
