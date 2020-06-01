using System;

namespace BusinessEntityServices.Entities.Interfaces
{
    public interface IPlayerEntity : IEntity
    {
        void Insert(string playerName, Guid playerId);

        void Get(string playerName);

        Guid PlayerId { get; set; }
    }
}
