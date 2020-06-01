using System;
using BusinessEntityServices.Entities;
using BusinessEntityServices.Entities.Interfaces;

namespace BusinessComponents.Abstracts
{
    public abstract class Player 
    {
        public string Name { get; set; }

        public Player(string name, IPlayerEntity playerEntity)
        {
            Name = name;

            if (playerEntity.PlayerId != Guid.Empty)
            {
                PlayerId = playerEntity.PlayerId;

                return;
            }

            PlayerId = Guid.NewGuid();

            playerEntity.Insert(name, PlayerId);
        }

        public Guid PlayerId { get; set; }
    }
}
