using System.Collections.Generic;

namespace BusinessComponents.Interfaces
{
    public interface IFrame
    {
        bool IsFull(int numberOfRolls);

        List<IRollScore> RollScore { get; set; }

         int Index { get; set; }

         bool IsStrike { get; set; }

         bool IsSpare { get; set; }

         int TotalScore { get; set; }

         int CumulativeTotalScore { get; set; }

         List<IRollScore> BonusScore { get; set; }

    }
}
