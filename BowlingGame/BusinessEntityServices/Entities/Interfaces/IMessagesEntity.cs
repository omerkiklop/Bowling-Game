namespace BusinessEntityServices.Entities.Interfaces
{
    public interface IMessagesEntity : IEntity
    {
        string MessageValue { get; set; }
        void Get(string messageName);
    }
}
