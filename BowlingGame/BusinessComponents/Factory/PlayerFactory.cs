using System;
using BusinessComponents.BusinessComponents;
using BusinessComponents.Interfaces;
using BusinessEntityServices.Entities;
using BusinessEntityServices.Entities.Interfaces;

namespace BusinessComponents.Factory
{
    public class PlayerFactory : IPlayerFactory
    {
        public IGameRules GameRules { get; private set; }

        public IFrameEntity FrameEntity { get; private set; }

        public IFrameMapper FrameMapper { get; private set; }

        public IPlayerEntity PlayerEntity { get; private set; }

        public PlayerFactory(IPlayerEntity playerEntity, IGameRules gameRules, IFrameEntity frameEntity, IFrameMapper frameMapper)
        {
            GameRules = gameRules;

            FrameEntity = frameEntity;

            PlayerEntity = playerEntity;

            FrameMapper = frameMapper;
        }

        public IPlayer CreatePlayer(string name, string playType)
        {
            if (playType == "Bowling")
            {
                PlayerEntity.Get(name);

                return new BowlingPlayer(name, GameRules as IBowlingGameRules, FrameEntity, PlayerEntity, FrameMapper);
            }

            return null;
        }
    }
}
