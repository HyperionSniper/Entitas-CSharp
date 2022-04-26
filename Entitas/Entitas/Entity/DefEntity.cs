#if TYPEDEF_CODEGEN
using Hyperion.Defs;

namespace Entitas {
    public interface IDefEntity : IEntity {

    }

    /// Use context.CreateDefEntity() to create a new entity and
    /// entity.Destroy() to destroy it.
    /// You can add, replace and remove IComponent to an entity.
    /// DefEntities contain wrapper classes for managing components 
    /// created from objects within the TypeDef ecosystem.
    public abstract class DefEntity<T> : Entity, IDefEntity where T : TypeDef {
        protected DefWrapperBase _def;
        public DefWrapperBase def {
            get
            {
                if (_def.typeDef == null)
                    return null;
                else
                    return _def;
            }
        }

        // TODO: Def events ?

        /// Occurs when a component gets added.
        /// All event handlers will be removed when
        /// the entity gets destroyed by the context.
        //public event EntityComponentChanged OnComponentAdded;

        public abstract class DefWrapperBase {
            TypeDef _typeDef;
            Entity _entity;

            public TypeDef typeDef { get { return _typeDef; } }

            protected DefWrapperBase(Entity entity, TypeDef typeDef)
            {
                _entity = entity;
                _typeDef = typeDef;
            }
        }
    }



    public sealed partial class _EntityType0<T> : Entitas.DefEntity<T> where T : TypeDef {
        public sealed partial class DefWrapper : DefWrapperBase {
            public DefWrapper(Entity entity, TypeDef typeDef) : base(entity, typeDef) { }

        }

        public _EntityType0(TypeDef typeDef)
        {
            _def = new T(this, typeDef);
        }
    }
}
#endif