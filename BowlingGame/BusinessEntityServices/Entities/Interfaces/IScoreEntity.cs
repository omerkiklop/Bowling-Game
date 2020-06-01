using System;
using System.Collections.Generic;

namespace BusinessEntityServices.Entities.Interfaces
{
    public interface IScoreEntity : IEntity
    {
       void Insert(Guid gameId, Guid playerId , string gameType, int score);

       Dictionary<string, List<int>> GetTop5Scores();
    }
}
