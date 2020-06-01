using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessEntityServices.Entities.Interfaces;

namespace BusinessEntityServices.Entities
{
    public class MappedFrame : IMapedFrame
    {
        public int Try1 { get; set; }

        public int Try2 { get; set; }

        public int Index { get; set; }

        public bool IsStrike { get; set; }

        public bool IsSpare { get; set; }

        public int TotalScore { get; set; }

        public int CumulativeTotalScore { get; set; }

        public int BonusScore { get; set; }
    }
}
