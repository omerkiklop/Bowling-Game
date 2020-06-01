using BusinessComponents.Interfaces;

namespace BusinessComponents.BusinessComponents
{
    public class RollScore : IRollScore
    {
        public int Score { get; set; }

        public RollScore( int score)
        {
            Score = score;
        }
    }
}
