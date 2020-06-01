using System;
using System.Collections.Generic;
using System.Linq;
using BusinessComponents.Abstracts;
using BusinessComponents.Factory;
using BusinessComponents.Interfaces;
using BusinessEntityServices.Entities.Interfaces;
using Log;

namespace BusinessComponents.BusinessComponents
{
    public class BowlingGame : Game, IBowlingGame
    {
        public IMessageFactory MessageFactory { get; private set; }

        public IPlayerFactory PlayerFactory { get; private set; }

        public RollResults RollResults{ get; private set; }

        public event Action<string> WriteExternal;

        private IBowlingPlayer _currentPlayer;

        private readonly IBowlingGameRules _bowlingGameRules;

        private int _currentFrameCounter;

        private bool _exitGame = false;

        public BowlingGame(
            IPlayers players, 
            IGameCalculator gameCalculator, 
            IGameRules gameRules, 
            IGameEntity bowlingGamEntity, 
            IMessageFactory messageFactory,
            IPlayerFactory playerFactory) : 
            base(players, gameCalculator, gameRules, bowlingGamEntity)
        {
            MessageFactory = messageFactory;

            PlayerFactory = playerFactory;

            bowlingGamEntity.Insert("Bowling", DateTime.Now, Guid.NewGuid());
            
            _bowlingGameRules = gameRules as BowlingGameRules;

            Logger.Info("BowlingGame :: Create a new Game");
        }

        public void CreatePlayer(string name)
        {
            Logger.Info(string.Format("BowlingGame :: CreatePlayer {0} ", name));

            var player = PlayerFactory.CreatePlayer(name, "Bowling");

            if (IsPlayerExistsInPlayersList(Players.players, player))
                Players.players.Add(player);
        }

        private  bool IsPlayerExistsInPlayersList(IEnumerable<IPlayer> players, IPlayer player)
        {
            return players.All(bowler => bowler.PlayerId != player.PlayerId);
        }

        public void GetTop5Scores()
        {
            Logger.Info(string.Format("PlayGame :: GetTop5Scores"));

            var calculator = GameCalculator as BowlingGameCalculator;

            if (calculator == null) return;

            var res = calculator.ScoreEntity.GetTop5Scores();

            foreach (var kvp in res)
            {
                foreach (var val in kvp.Value)
                {
                    OnWriteExternal(string.Format("Player Name = {0}, Score = {1}", kvp.Key, val));
                }
            }
        }

        public override void PlayGame()
        {
            Logger.Info("BowlingGame :: PlayGame");

            try
            {
                InitEvents();

                while (_currentFrameCounter < _bowlingGameRules.FramesPerMatch)
                {
                    OnWriteExternal(string.Format("Play New Frame. The current Frame is {0} ", _currentFrameCounter + 1));

                    foreach (var player in Players.players.Cast<BowlingPlayer>())
                    {
                        _currentPlayer = player;

                        RollRound();
                    }

                    _currentFrameCounter++;
                }
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }
        }

        private void RollRound()
        {
            try
            {
                OnWriteExternal(string.Format("fill in the number of pins {0} knocked down ", _currentPlayer.Name));

                var line = Console.ReadLine();

                var input = 0;

                ValidateInput(line, input);

                if (line == null) return;

                var pins = int.Parse(line);

                _currentPlayer.Play(pins, GameCalculator, _currentFrameCounter, RollResults,Players.CurrentGameIdGuid);
            }
            catch (Exception e)
            {
                Logger.Error(e);
                if (!_exitGame)
                {
                    Logger.Error(string.Format(" {0} :: {1}", e.Message, e.StackTrace));

                    OnWriteExternal(string.Format(e.Message));

                    RollRound();
                }
                else
                    throw e;
            }
        }

        private void ValidateInput(string line, int input)
        {
            
            if (line.ToLower().Equals("exit"))
            {
                _exitGame = true;
                ThrowException(MessageFactory.GetMessage("ExitGame"));
            }

            if (!int.TryParse(line, out input))
                ThrowException(string.Format(MessageFactory.GetMessage("IssueWithInput"), line));
            

            if (string.IsNullOrEmpty(line))
                ThrowException(MessageFactory.GetMessage("LineIsEmpty"));
        }

        private void ThrowException(string msg)
        {
            var e = new Exception(msg);

            Logger.Error(e);

            throw e;
        }

        private void InitEvents()
        {
            foreach (var player in Players.players.Cast<BowlingPlayer>())
            {
                player.OnFramesCompleted -= PlayerOnFramesCompleted;
                player.OnFramesCompleted += PlayerOnFramesCompleted;

                player.OnFrameComplete -= PlayerOnFrameComplete;
                player.OnFrameComplete += PlayerOnFrameComplete;

                player.BonusLastFramesEvent -= PlayerOnBonusLastFramesEvent;
                player.BonusLastFramesEvent += PlayerOnBonusLastFramesEvent;

                player.RunAdditionalRoll -= PlayerOnRunAdditionalRoll;
                player.RunAdditionalRoll += PlayerOnRunAdditionalRoll;

                player.RunBonusMessage -= PlayerOnRunBonusMessage;
                player.RunBonusMessage += PlayerOnRunBonusMessage;

            }
        }

        private void PlayerOnFrameComplete(IFrame obj)
        {
            RollResults = RollResults.FrameIsFull;
        }

        private void PlayerOnRunBonusMessage()
        {
            Logger.Info(string.Format("BowlingGame :: PlayerOnRunBonusMessage Bonus"));

            OnWriteExternal(string.Format(MessageFactory.GetMessage("Bonus")));
        }

        private void PlayerOnRunAdditionalRoll()
        {
            RollRound();
            RollResults = RollResults.PlayAdditional;
        }

        private void PlayerOnBonusLastFramesEvent()
        {
            RollResults = RollResults.Bonus;
            RollRound();
            RollResults = RollResults.None;
        }

        private void PlayerOnFramesCompleted()
        {
            GameCalculator.CalculateTotalScore(_currentPlayer.PlayerId);
            RollResults = RollResults.GameOver;
        }

        public void PrintScores()
        {
            foreach (var player in Players.players.Cast<BowlingPlayer>())
            {
                OnWriteExternal(string.Format("Total Score for {0} is {1}", player.Name,
                    player.Frames.LastOrDefault().CumulativeTotalScore));
            }
        }

        public bool IsGameOver()
        {
            if (_currentPlayer == null) return false;

            if (!_bowlingGameRules.IsGameOver(_currentFrameCounter, _currentPlayer.CurrentFrame)) 
                return _exitGame;

            PrintScores();
            return true;
        }

        protected virtual void OnWriteExternal(string obj)
        {
            var handler = WriteExternal;

            if (handler != null)
                handler(obj);
        }

    }
    
}
