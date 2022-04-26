//#if TYPEDEF_CODEGEN

//using System;
//using Hyperion.Defs;

//namespace Entitas {
//    public class DefContext<TEntity> : Context<TEntity> where TEntity : DefEntity {
//        public DefContext(int totalComponents, Func<TEntity> entityFactory) : base(totalComponents, entityFactory)
//        {
//        }

//        public DefContext(int totalComponents, int startCreationIndex, ContextInfo contextInfo, Func<IEntity, IAERC> aercFactory, Func<TEntity> entityFactory) : base(totalComponents, startCreationIndex, contextInfo, aercFactory, entityFactory)
//        {
//        }

//        public TEntity CreateDefEntity(TypeDef def)
//        {
//            bool instantiate = false;
//            if (instantiate)
//            {
//                def = TypeDef.Instantiate(def);
//            }

//            var entity = CreateEntity();

//        }
//    }
//}

//#endif