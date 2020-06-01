using System;
using BusinessComponents.Interfaces;
using BusinessEntityServices.Entities.Interfaces;

namespace BusinessComponents.Abstracts
{
    public abstract class Game 
    {
        public IPlayers Players { get; set; }

        public IGameCalculator GameCalculator { get; private set; }

        public IGameRules GameRules { get; private set; }

        public Guid Id { get; set; }

        public IGameEntity GamEntity { get; set; }

        public Game(IPlayers players, IGameCalculator gameCalculator, IGameRules gameRules, IGameEntity gamEntity)
        {
            Players = players;

            GameCalculator = gameCalculator;

            GameCalculator.GameId = Id;

            GameRules = gameRules;

            GamEntity = gamEntity;

            Id = Guid.NewGuid();

            players.CurrentGameIdGuid = Id;

            gameCalculator.GameId = Id;
        }

        public abstract void PlayGame();

        public void CreatePlayer(IPlayerFactory playerFactory, string name)
        {
            throw new NotImplementedException();
        }
    }
}
