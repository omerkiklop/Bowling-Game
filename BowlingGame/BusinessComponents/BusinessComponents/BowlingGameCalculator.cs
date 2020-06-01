using System;
using System.Collections.Generic;
using System.Linq;
using BusinessComponents.Interfaces;
using BusinessEntityServices.Entities.Interfaces;
using Log;


namespace BusinessComponents.BusinessComponents
{
    public class BowlingGameCalculator : IGameCalculator
    {
        private IFrame _currentFrame;
       
        public IScoreEntity ScoreEntity { get; private set; }

        public Guid GameId { get; set; }

        public BowlingGameCalculator(IScoreEntity scoreEntity)
        {
            ScoreEntity = scoreEntity;
        }

        public void CalculateScore(int score, int index, List<IFrame> frames)
        {
            try
            {
                _currentFrame = frames[index] as Frame;
               
                if (_currentFrame != null) 
                    _currentFrame.TotalScore = _currentFrame.RollScore.Sum(x => x.Score);

                HandleAdditionalCalculation(index, frames);

                CalculateCumulativeTotalScore(index, frames);
            }
            catch (Exception e)
            {
                Logger.Error(e);
                throw e;
            }
        }

        public void CalculateTotalScore(Guid playerId)
        {
            ScoreEntity.Insert(GameId, playerId, "Bowling", _currentFrame.CumulativeTotalScore);
        }

        private void HandleAdditionalCalculation(int index, List<IFrame> frames)
        {
            if (index <= 0) return;
            
            if (frames[index - 1].IsStrike)
            {
                frames[index - 1].TotalScore += _currentFrame.RollScore.Sum(x => x.Score);
            }

            if (frames[index - 1].IsSpare)
            {
                frames[index - 1].TotalScore += _currentFrame.RollScore.FirstOrDefault().Score;
            }
            
        }

        private void CalculateCumulativeTotalScore(int index, List<IFrame> frames)
        {
            if (index <= 0)
            {
                _currentFrame.CumulativeTotalScore = _currentFrame.TotalScore;
                return;
            }

            if (frames[index - 1].IsStrike)
            {
                frames[index - 1].CumulativeTotalScore += _currentFrame.RollScore.Sum(x => x.Score);

            }

            if (frames[index - 1].IsSpare)
            {
                frames[index - 1].CumulativeTotalScore += _currentFrame.RollScore.FirstOrDefault().Score;
            }

            var bonus = _currentFrame.BonusScore == null ? 0 : _currentFrame.BonusScore.Sum(x => x.Score);

            _currentFrame.CumulativeTotalScore =
                frames[index - 1].CumulativeTotalScore + _currentFrame.TotalScore + bonus;
        }
    }
}
