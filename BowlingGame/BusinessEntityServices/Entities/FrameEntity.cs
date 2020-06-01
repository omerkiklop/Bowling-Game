using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using BusinessEntityServices.Entities.Interfaces;

namespace BusinessEntityServices.Entities
{
    public class FrameEntity : Entity, IFrameEntity
    {
       
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
            throw new NotImplementedException();
        }

        public void Update()
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

        public void Delete()
        {
            throw new NotImplementedException();
        }

        public event Action<string> WriteExternal;

        public void Insert(IMapedFrame frame, Guid playerId, Guid gameId)
        {
            Parameters.Clear();

            Command = "InsertFrame";

            Parameters.Add(ParameterFactory.Create("PlayerId", DbType.Guid, playerId));

            Parameters.Add(ParameterFactory.Create("GameId", DbType.Guid, gameId));

            Parameters.Add(ParameterFactory.Create("FrameId", DbType.Int32, frame.Index));

            Parameters.Add(ParameterFactory.Create("Try1", DbType.Int32, frame.Try1));

            Parameters.Add(ParameterFactory.Create("Try2", DbType.Int32, frame.Try2));

            Parameters.Add(ParameterFactory.Create("Bonus", DbType.Int32, frame.BonusScore));
            
            Parameters.Add(ParameterFactory.Create("IsStrike", DbType.Boolean, frame.IsStrike));

            Parameters.Add(ParameterFactory.Create("IsSpare", DbType.Boolean, frame.IsSpare));

            Parameters.Add(ParameterFactory.Create("TotalScore", DbType.Int32, frame.TotalScore));

            Parameters.Add(ParameterFactory.Create("CumulativeScore", DbType.Int32, frame.CumulativeTotalScore));

            ExecuteTask(Insert);
        }

        public void UpdateFrame(IMapedFrame frame, Guid playerId, Guid gameId)
        {
            Parameters.Clear();

            Command = "UpdateFrame";

            Parameters.Add(ParameterFactory.Create("PlayerId", DbType.Guid, playerId));

            Parameters.Add(ParameterFactory.Create("GameId", DbType.Guid, gameId));

            Parameters.Add(ParameterFactory.Create("FrameId", DbType.Int32, frame.Index));

            Parameters.Add(ParameterFactory.Create("Bonus", DbType.Int32, frame.BonusScore));

            Parameters.Add(ParameterFactory.Create("TotalScore", DbType.Int32, frame.TotalScore));

            Parameters.Add(ParameterFactory.Create("CumulativeScore", DbType.Int32, frame.CumulativeTotalScore));

            ExecuteTask(Update);
        }

       
    }
}
