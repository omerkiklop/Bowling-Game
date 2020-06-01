using System;
using System.Collections.Generic;
using System.Data;
using BusinessEntityServices.Entities.Interfaces;
using Log;

namespace BusinessEntityServices.Entities
{
    public class ScoreEntity : Entity , IScoreEntity
    {
        private readonly Dictionary<string, List<int>> _topScores;

        private DataTable _resultsTable;

        public ScoreEntity()
        {
            _topScores = new Dictionary<string, List<int>>();
        }

        public void Insert()
        {
            try
            {
                ExecuteNonQueryCommandTypeStoredProcedure();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void Get()
        {
            try
            {
                _resultsTable = ExecuteExecuteDataTableStoredProcedure();
            }
            catch (Exception e)
            {
                throw e;
            }
            
        }

        private void SetResults()
        {
            var dataRows = _resultsTable.AsEnumerable();

            foreach (var row in dataRows)
            {
                int input;

                if (!_topScores.ContainsKey(row[0].ToString()))
                {
                    if (int.TryParse(row[1].ToString(), out input))
                    {
                        _topScores.Add(row[0].ToString(), new List<int>() { input });
                    }
                }
                else
                {
                    List<int> val;
                    if (_topScores.TryGetValue(row[0].ToString(), out val)) ;
                    {
                        if (int.TryParse(row[1].ToString(), out input))
                        {
                            val.Add(input);
                           
                        }
                    }
                }
            }
        }

        public void Update()
        {
            throw new NotImplementedException();
        }

        public void Delete()
        {
            throw new NotImplementedException();
        }

        public event Action<string> WriteExternal;

        public void Insert(Guid gameId, Guid playerId , string gameType, int score)
        {
            Parameters.Clear();

            Command = "InsertUpdateScores";

            Parameters.Add(ParameterFactory.Create("@id", DbType.Guid, playerId));

            Parameters.Add(ParameterFactory.Create("@GameId", DbType.Guid, gameId));

            Parameters.Add(ParameterFactory.Create("@GameType", DbType.AnsiString, gameType));

            Parameters.Add(ParameterFactory.Create("@Score", DbType.Int32, score));

            ExecuteTask(Insert);
        }

        public Dictionary<string, List<int>>  GetTop5Scores()
        {
            try
            {
                _topScores.Clear();

                Command = "GetTop5Scores";

                ExecuteTask(Get);

                SetResults();

                return _topScores;
            }
            catch (Exception e)
            {
                Logger.Error(e);
                return null;
            }
           
        }
    }
}
