using System;

namespace BusinessComponents.Interfaces
{
    public interface IBowlingPlayer : IPlayer
    {
        void ValidateData(int score , RollResults rollResult);

        string Name { get; set; }

        IFrame CurrentFrame { get; set; }

        void Play(int score, IGameCalculator gameCalculator , int currentFrameCounter, RollResults rollResult, Guid currentGameId);

        event Action<IFrame> OnFrameComplete;

        event Action OnFramesCompleted;

        event Action BonusLastFramesEvent;

        event Action RunAdditionalRoll;

        event Action RunBonusMessage;
        
    }

   
}
