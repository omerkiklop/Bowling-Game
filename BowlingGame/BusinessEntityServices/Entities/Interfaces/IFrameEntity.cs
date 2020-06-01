using System;

namespace BusinessEntityServices.Entities.Interfaces
{
    public interface IFrameEntity : IEntity
    {
        void Insert(IMapedFrame frame, Guid playerId, Guid gameId);
        void UpdateFrame(IMapedFrame frame, Guid playerId, Guid gameId);
    }
}
