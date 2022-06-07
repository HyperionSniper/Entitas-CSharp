namespace Entitas {

    public class ComponentIsNotRegisteredException : EntitasException {

        public ComponentIsNotRegisteredException(string contextName, System.Type componentType)
            : base("Cannot get component index of type '" + componentType.Name + "' from context '" +
                   contextName + "'!", "This type has not been registered in the context's lookup.") {
        }
    }
}
