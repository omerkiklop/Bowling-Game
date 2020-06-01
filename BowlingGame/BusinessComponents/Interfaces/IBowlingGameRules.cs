using System.Collections.Generic;

namespace BusinessComponents.Interfaces
{
    public interface IBowlingGameRules : IGameRules
    {
        bool IsStrike(int score, IFrame currentFrame);

        bool IsSpare(IFrame currentFrame);

        bool IsLastFrame(int currentFrameCounter);

        bool IsGameOver(int framesCount, IFrame frame);

        void ValidateData(int score, RollResults rollResult, int currentFrameCounter, List<IFrame> frames);

        int FramesPerMatch { get;  set; }

        int MaxScore { get;  set; }

        int NumberOfRolls { get;  set; }

        int NumberOfBonusRolls { get; set; }

       
    }
}
