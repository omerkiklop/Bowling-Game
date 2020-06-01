using System.Collections.Generic;
using BusinessComponents.Interfaces;

namespace BusinessComponents.BusinessComponents
{
    public class Frame : IFrame
    {
        public List<IRollScore> RollScore { get; set; }

        public int Index { get;  set; }

        public bool IsStrike { get;  set; }

        public bool IsSpare{ get;  set; }

        public int TotalScore { get; set; }

        public int CumulativeTotalScore { get; set; }

        public List<IRollScore> BonusScore { get; set; }

        public Frame(int index)
        {
            Index = index;

            RollScore = new List<IRollScore>();

            IsStrike = false;

            IsSpare = false;
        }

        public bool IsFull(int numberOfRolls)
        {
            return RollScore.Count >= numberOfRolls;
        }
    }
}