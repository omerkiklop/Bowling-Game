using System;
using System.Collections.Generic;
using BusinessComponents.Interfaces;

namespace BusinessComponents.BusinessComponents
{
    public class  Players : IPlayers
    {
        public Players()
        {
            players = new List<IPlayer>();
        }

        public List<IPlayer> players { get; set; }

        public Guid CurrentGameIdGuid { get; set; }
    }
}
