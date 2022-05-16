#if TYPEDEF_CODEGEN

namespace Entitas {
    using Hyperion.Defs;

    public interface IDefEntity : IEntity {

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

        private DefWrapperBase _def;
        public DefWrapperBase def {
            get {
                if (_def?.typeDef == null)
                    return null;
                else
                    return _def;
            }
            private set { _def = value; }
        }

        public bool hasDef => _def?.typeDef != null;

        public void AddDef(Hyperion.Defs.TypeDef def) {
            if (hasDef)
                throw new Entitas.EntitasException("Cannot add TypeDef '" + def.Id + "' to " + this?.ToString() + "!", "You should check if an entity already has the TypeDef before adding it or use entity.ReplaceComponent().");

            _def = CreateDefWrapper(this, def);
        }

        public void ReplaceDef(Hyperion.Defs.TypeDef def) {
            _def = CreateDefWrapper(this, def);
        }

        public void RemoveDef() {
            _def = null;
        }

        protected abstract DefWrapperBase CreateDefWrapper(DefEntity entity, TypeDef typeDef);

        public class DefWrapperBase {
            Hyperion.Defs.TypeDef _typeDef;
            DefEntity _entity;

            public Hyperion.Defs.TypeDef typeDef { get { return _typeDef; } }

            protected DefWrapperBase(DefEntity entity, Hyperion.Defs.TypeDef typeDef) {
                _entity = entity;
                _typeDef = typeDef;
            }

            // defwrapper will have component access methods added to it 
        }
    }
}

#if DEBUG
namespace test {
public sealed partial class _EntityType0 : Entitas.DefEntity {
    public sealed partial class DefWrapper : DefWrapperBase {
        public DefWrapper(Entitas.DefEntity entity, Hyperion.Defs.TypeDef typeDef) : base(entity, typeDef) { }

    }

    protected override DefWrapperBase CreateDefWrapper(Entitas.DefEntity entity, Hyperion.Defs.TypeDef typeDef) {
        return new DefWrapper(entity, typeDef);
    }
}
}
#endif

#endif