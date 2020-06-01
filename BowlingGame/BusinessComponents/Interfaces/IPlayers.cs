using System;
using System.Collections.Generic;

namespace BusinessComponents.Interfaces
{
    public interface IPlayers
    {
        List<IPlayer> players { get; set; }

        Guid CurrentGameIdGuid { get; set; }
    }
}
