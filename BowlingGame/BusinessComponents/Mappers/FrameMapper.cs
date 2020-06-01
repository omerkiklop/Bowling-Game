using System.Linq;
using BusinessComponents.Interfaces;
using BusinessEntityServices.Entities;
using BusinessEntityServices.Entities.Interfaces;

namespace BusinessComponents.Mappers
{
    public class FrameMapper : IFrameMapper
    {
        public IMapedFrame Map(IFrame frame)
        {
            return new MappedFrame()
            {
                IsStrike = frame.IsStrike,
                IsSpare = frame.IsSpare,
                Try1 = frame.RollScore.FirstOrDefault().Score,
                Try2 = frame.RollScore.LastOrDefault().Score,
                CumulativeTotalScore = frame.CumulativeTotalScore,
                TotalScore = frame.TotalScore,
                Index = frame.Index + 1,
                BonusScore = frame.BonusScore != null ? frame.BonusScore.Sum(x => x.Score) : 0
            };
        }
    }
}
