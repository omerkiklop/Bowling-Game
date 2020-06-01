using System;
using System.Collections.Generic;
using System.Linq;
using BusinessComponents.Abstracts;
using BusinessComponents.Interfaces;
using BusinessEntityServices.Entities;
using BusinessEntityServices.Entities.Interfaces;
using Log;

namespace BusinessComponents.BusinessComponents
{
    public class BowlingGameRules : GameRules, IBowlingGameRules
    {
        public IMessageFactory MessageFactory { get; private set; }

        public int FramesPerMatch { get; set; }

        public int MaxScore { get;   set; }

        public int NumberOfRolls { get;   set; }

        public int NumberOfBonusRolls { get; set; }

        private Dictionary<string, string> _configurationEntityResults;

        public BowlingGameRules(IMessageFactory messageFactory, IConfigurationEntity configurationEntity)
            : base(configurationEntity)
        {
            MessageFactory = messageFactory;
        }

        public bool IsStrike(int score, IFrame currentFrame)
        {
            return score == MaxScore && currentFrame.RollScore.Count == 0;
        }

        public bool IsSpare(IFrame currentFrame)
        {
            return currentFrame.RollScore.Sum(roll => roll.Score) == MaxScore;
        }

        public bool IsLastFrame(int currentFrameCounter)
        {
            return currentFrameCounter == FramesPerMatch - 1;
        }

        public bool IsGameOver(int framesCount, IFrame frame)
        {
            return framesCount == FramesPerMatch && frame.IsFull(NumberOfRolls);
        }

        public void ValidateData(int score, RollResults rollResult, int currentFrameCounter, List<IFrame> frames)
        {
            if (score > MaxScore)
                ThrowException(string.Format(MessageFactory.GetMessage("WrongInput"), MaxScore));

            if (currentFrameCounter >= FramesPerMatch)
                ThrowException((MessageFactory.GetMessage("NoMoreFrames")));
            
            if (currentFrameCounter >= frames.Count) return;

            if (frames.Count <= 0 || frames[currentFrameCounter].RollScore == null ||
                rollResult == RollResults.Bonus) return;

            if (frames[currentFrameCounter].RollScore.Sum(x => x.Score) + score > MaxScore)
                ThrowException(string.Format(MessageFactory.GetMessage("TotalScoreError"), MaxScore));
        }

        private void ThrowException(string msg)
        {
            var e = new Exception(msg);

            Logger.Error(e);

            throw e;
        }

        public override void GetRulesConfiguration()
        {
            Logger.Info("BowlingGameRules :: GetRulesConfiguration");

            var configList = AddParamsToConfigList();

            _configurationEntityResults = ConfigurationEntity.Get(configList.ToArray());

            FramesPerMatch = Convert.ToInt32(SetConfigProperty("FramesPerMatch")); 

            MaxScore = Convert.ToInt32(SetConfigProperty("MaxScore"));

            NumberOfRolls = Convert.ToInt32(SetConfigProperty("NumberOfRolls"));

            NumberOfBonusRolls = Convert.ToInt32(SetConfigProperty("NumberOfBonusRolls"));
        }

        private List<string> AddParamsToConfigList()
        {
            var configList = new List<string>();

            configList.Add("FramesPerMatch");

            configList.Add("MaxScore");

            configList.Add("NumberOfRolls");

            configList.Add("NumberOfBonusRolls");
            
            return configList;
        }

        private object SetConfigProperty(string key)
        {
            string val;
 
            if (_configurationEntityResults.TryGetValue(key, out val));
                return val;
        }

       
    }
}
