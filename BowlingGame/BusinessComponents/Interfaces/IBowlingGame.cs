using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessComponents.Interfaces
{
    public interface IBowlingGame : IGame
    {
        event Action<string> WriteExternal;
    }
}
