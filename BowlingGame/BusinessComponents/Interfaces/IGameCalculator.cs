using System;
using System.Collections.Generic;

namespace BusinessComponents.Interfaces
{
    public interface IGameCalculator
    {
        void CalculateScore(int score, int index, List<IFrame> frames );

        void CalculateTotalScore(Guid playerId);
        
        Guid GameId { get; set; }
    }
}
