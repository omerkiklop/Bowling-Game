using System;
using System.Collections.Generic;
using System.Linq;
using BusinessComponents.Abstracts;
using BusinessComponents.Interfaces;
using BusinessComponents.Mappers;
using BusinessEntityServices.Entities.Interfaces;
using Log;

namespace BusinessComponents.BusinessComponents
{
    public class BowlingPlayer : Player, IBowlingPlayer
    {
        public List<IFrame> Frames { get;  set; }

        public event Action<IFrame> OnFrameComplete;

        public event Action OnFramesCompleted;

        public event Action BonusLastFramesEvent;

        public event Action RunAdditionalRoll;

        public event Action RunBonusMessage;

        public IFrame CurrentFrame { get; set; }

        private readonly IBowlingGameRules _gameRules ;

        private readonly IFrameEntity _frameEntity;

        private readonly IFrameMapper _frameMapper;

        private int _currentFrameCounter;

        public BowlingPlayer(string name, IBowlingGameRules gameRules, IFrameEntity frameEntity, IPlayerEntity playerEntity, IFrameMapper frameMapper)
            : base(name, playerEntity)
        {
            _gameRules = gameRules;

            _frameEntity = frameEntity;

            _frameMapper = frameMapper;

            Frames = new List<IFrame>();
        }

        private void RunAdditionalRollEvent()
        {
            if (RunAdditionalRoll == null || CurrentFrame.IsFull(_gameRules.NumberOfRolls)) return;

            Logger.Info("BowlingPlayer :: RunAdditionalRollEvent");

            RunAdditionalRoll.Invoke();
        }

        private void RunBonusMessageEvent()
        {
            if (RunBonusMessage == null) return;

            Logger.Info("BowlingPlayer :: RunBonusMessageEvent");

            RunBonusMessage.Invoke();
        }

        private bool ShouldCreateFrame( )
        {
            return CurrentFrame == null || CurrentFrame.IsFull(_gameRules.NumberOfRolls);
        }

        private void NextPlayerRollEvent( )
        {
            Logger.Info("BowlingPlayer :: NextPlayerRollEvent");

            if (_gameRules.IsLastFrame(_currentFrameCounter))
            {
                if (CurrentFrame.IsSpare)
                {
                    if (BonusLastFramesEvent != null)
                    {
                        Logger.Info("BowlingPlayer :: BonusLastFramesEvent");

                        BonusLastFramesEvent.Invoke();
                    }
                } 
                if (CurrentFrame.IsStrike)
                {
                    Logger.Info("BowlingPlayer :: HandleStrikeBonus");

                    HandleStrikeBonus();
                }

                if (OnFramesCompleted == null) return;

                Logger.Info("BowlingPlayer :: OnFramesCompleted");

                OnFramesCompleted.Invoke();

                return;
            }

            if (OnFrameComplete != null)
            {
                Logger.Info("BowlingPlayer :: OnFrameComplete");

                OnFrameComplete.Invoke(CurrentFrame);
            }
        }

        private void HandleStrikeBonus()
        {
            Logger.Info("BowlingPlayer :: HandleStrikeBonus");

            if (BonusLastFramesEvent == null) return;

            BonusLastFramesEvent.Invoke();

            if (CurrentFrame.BonusScore.Count < _gameRules.NumberOfBonusRolls &&
                CurrentFrame.BonusScore.Sum(roll => roll.Score) < _gameRules.MaxScore)
                NextPlayerRollEvent();
        }

        private void HandleBonusRoll(int score, IGameCalculator gameCalculator, Guid currentGameId)
        {
            Logger.Info("BowlingPlayer :: HandleBonusRoll");

            if (CurrentFrame.BonusScore == null)
                CurrentFrame.BonusScore = new List<IRollScore> {new RollScore(score)};
            else
                CurrentFrame.BonusScore.Add(new RollScore(score));

            gameCalculator.CalculateScore(score, _currentFrameCounter, Frames);

            if(CurrentFrame.IsStrike)
                HandleStrikeBonus(currentGameId);
            else
                HandleFrameEntity(currentGameId);
        }

        private void HandleFrameEntity(Guid currentGameId)
        {
            Logger.Info("BowlingPlayer :: HandleFrameEntity");

            _frameEntity.Insert(_frameMapper.Map(CurrentFrame), PlayerId, currentGameId);

            if (CurrentFrame.Index > 0)
            {
                _frameEntity.UpdateFrame(_frameMapper.Map(Frames[CurrentFrame.Index - 1]), PlayerId, currentGameId);
            }
        }

        private void HandleStrikeBonus(Guid currentGameId)
        {
            Logger.Info("BowlingPlayer :: HandleFrameEntity");

            if (CurrentFrame.Index <= 0) return;

            _frameEntity.UpdateFrame(_frameMapper.Map(Frames[CurrentFrame.Index]), PlayerId, currentGameId);

        }

        public void ValidateData(int score , RollResults rollResult)
        {
            Logger.Info("BowlingPlayer :: ValidateData");

            _gameRules.ValidateData(score,rollResult,_currentFrameCounter,Frames);
        }

        public void Play(int score, IGameCalculator gameCalculator ,int currentFrameCounter ,RollResults rollResult, Guid currentGameId)
        {
            Logger.Info("BowlingPlayer :: Play");

            _currentFrameCounter = currentFrameCounter;

            ValidateData(score, rollResult);

            try
            {
                if (rollResult == RollResults.Bonus)
                {
                    HandleBonusRoll(score, gameCalculator, currentGameId);
                    return;
                }

                if (ShouldCreateFrame())
                    Frames.Add(new Frame(_currentFrameCounter));

                CurrentFrame = Frames[_currentFrameCounter];

                if (HandleStrikeFrame(currentGameId,gameCalculator,score))
                    return;

                CurrentFrame.RollScore.Add(new RollScore(score));

                if (HandleSpareFrame(currentFrameCounter))
                    return;

                HandleFullFrame(currentGameId, gameCalculator, score);
            }
            catch (Exception e)
            {
                Logger.Error(e);
                throw e;
            }
        }

        private void HandleFullFrame(Guid currentGameId, IGameCalculator gameCalculator, int score)
        {
            if (CurrentFrame.IsFull(_gameRules.NumberOfRolls))
            {
                gameCalculator.CalculateScore(score, _currentFrameCounter, Frames);

                HandleFrameEntity(currentGameId);

                NextPlayerRollEvent();
            }
            else
            {
                RunAdditionalRollEvent();
            }
        }

        private bool HandleStrikeFrame(Guid currentGameId, IGameCalculator gameCalculator, int score)
        {
            if (!_gameRules.IsStrike(score, CurrentFrame)) return false;

            RunBonusMessageEvent();

            CurrentFrame.IsStrike = true;

            CurrentFrame.RollScore.Add(new RollScore(score));

            CurrentFrame.RollScore.Add(new RollScore(0));

            gameCalculator.CalculateScore(score, _currentFrameCounter, Frames);

            HandleFrameEntity(currentGameId);

            NextPlayerRollEvent();

            return true;

        }

        private bool HandleSpareFrame(int currentFrameCounter)
        {
            if (!_gameRules.IsSpare(CurrentFrame)) return false;

            RunBonusMessageEvent();

            CurrentFrame.IsSpare = true;

            NextPlayerRollEvent();

            return Frames.Count == _gameRules.FramesPerMatch && Frames[currentFrameCounter].IsFull(_gameRules.NumberOfRolls);
        }

        
    }
}
