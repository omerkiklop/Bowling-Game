using System;

namespace BusinessEntityServices.Entities.Interfaces
{
    public interface IGameEntity : IEntity
    {
        void Insert(string gameType, DateTime currentDateTime, Guid Id);
    }
}
