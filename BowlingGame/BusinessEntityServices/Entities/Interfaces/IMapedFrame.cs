using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntityServices.Entities.Interfaces
{
    public interface IMapedFrame
    {
       
        int Try1 { get; set; }

        int Try2 { get; set; }

        int Index { get; set; }

        bool IsStrike { get; set; }

        bool IsSpare { get; set; }

        int TotalScore { get; set; }

        int CumulativeTotalScore { get; set; }

        int BonusScore { get; set; }

    }
}
