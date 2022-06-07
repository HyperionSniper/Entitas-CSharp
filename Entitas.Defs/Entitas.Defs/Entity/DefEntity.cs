#if TYPEDEF_CODEGEN

//namespace Hyperion.Defs {
//    public class TypeDeff { public string Id;  }
//}

namespace Entitas {
    using Hyperion.Defs;

    public interface IDefEntity : IEntity {
        bool hasDef { get; }

        Hyperion.Defs.TypeDef typeDef { get; }

        void AddDef(Hyperion.Defs.TypeDef def);
        void ReplaceDef(Hyperion.Defs.TypeDef def);
        void RemoveDef();
    }

    /// Use context.CreateDefEntity() to create a new entity and
    /// entity.Destroy() to destroy it.
    /// You can add, replace and remove IComponent to an entity.
    /// DefEntities contain wrapper classes for managing components 
    /// created from objects within the TypeDef ecosystem.
    public abstract class DefEntity : Entity, IDefEntity {

        // TODO: Def events ?

        /// Occurs when a component gets added.
        /// All event handlers will be removed when
        /// the entity gets destroyed by the context.
        //public event EntityComponentChanged OnComponentAdded;

        private TypeDef _def;

        public bool hasDef => _def != null;
        public TypeDef typeDef => _def;

        public void AddDef(Hyperion.Defs.TypeDef def) {
            if (hasDef)
                throw new Entitas.EntityAlreadyHasDefException(def, "Cannot add TypeDef '" + def.Id + "' to " + this?.ToString() + "!",
                    "You should check if an entity already has the TypeDef before adding it or use entity.ReplaceDef().");

            _def = def;
        }

        public void ReplaceDef(Hyperion.Defs.TypeDef def) {
            _def = def;
        }

        public void RemoveDef() {
            _def = null;
        }
    }
}

//#if DEBUG
//namespace test {
//public sealed partial class _EntityType0 : Entitas.DefEntity {
//    public sealed partial class DefWrapper : DefWrapperBase {
//        public DefWrapper(Entitas.DefEntity entity, Hyperion.Defs.TypeDef typeDef) : base(entity, typeDef) { }

//    }

//    protected override DefWrapperBase CreateDefWrapper(Entitas.DefEntity entity, Hyperion.Defs.TypeDef typeDef) {
//        return new DefWrapper(entity, typeDef);
//    }
//}
//}
//#endif

#endif