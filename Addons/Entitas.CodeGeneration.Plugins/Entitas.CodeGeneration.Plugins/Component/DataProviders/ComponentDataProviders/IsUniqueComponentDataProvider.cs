﻿using System;
using System.Linq;
using DesperateDevs.Utils;
using Entitas.CodeGeneration.Attributes;

namespace Entitas.CodeGeneration.Plugins {

    public class IsUniqueComponentDataProvider : IComponentDataProvider {

        public void Provide(Type type, ComponentData data) {
            var isUnique = type.ImplementsInterface<IContextComponent>() ||
                Attribute
                    .GetCustomAttributes(type)
                    .OfType<UniqueAttribute>()
                    .Any();

            data.IsUnique(isUnique);
        }
    }

    public static class IsUniqueComponentDataExtension {

        public const string COMPONENT_IS_UNIQUE = "Component.Unique";

        public static bool IsUnique(this ComponentData data) {
            return (bool)data[COMPONENT_IS_UNIQUE];
        }

        public static void IsUnique(this ComponentData data, bool isUnique) {
            data[COMPONENT_IS_UNIQUE] = isUnique;
        }
    }
}
