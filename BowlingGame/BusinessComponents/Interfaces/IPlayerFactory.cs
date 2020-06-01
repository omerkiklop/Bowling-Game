using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessComponents.Interfaces
{
    public interface IPlayerFactory
    {
        IPlayer CreatePlayer(string name, string PlayType);
    }
}
