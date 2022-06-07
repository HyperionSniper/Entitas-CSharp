using Hyperion.Defs;

namespace Entitas {

    public class EntityAlreadyHasDefException : EntitasException {

        public EntityAlreadyHasDefException(TypeDef def, string message, string hint)
            : base(message + "\nEntity already has a TypeDef with id " + def.Id + "!", hint) {
        }
    }
}
