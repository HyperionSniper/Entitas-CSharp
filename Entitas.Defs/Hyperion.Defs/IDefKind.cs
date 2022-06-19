namespace Hyperion.Defs {
    public interface IDefEntityKind<TEntity, TDef> where TEntity : Entitas.DefEntity where TDef : TypeDef {
        void GenerateComponentsFrom(TEntity entity, TDef def);
    }

    public interface IDefEntityKind {
        void GenerateComponents(Entitas.DefEntity entity) { }
    }
}
