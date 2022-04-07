namespace Entitas
{
    /// Implement this interface if you want to create a component which
    /// you can add to a context. 
    /// These components act like [Unique] contexts.
    /// [MyContextName, MyOtherContextName]: You can make this component to be
    /// available only in the specified contexts.
    /// The code generator can generate these attributes for you.
    /// More available Attributes can be found in Entitas.CodeGeneration.Attributes/Attributes.
    public interface IContextComponent : IComponent {
    }
}
