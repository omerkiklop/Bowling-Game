using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using BusinessEntityServices.Entities.Interfaces;
using Log;

namespace BusinessEntityServices.Entities
{
    public class PlayerEntity : Entity, IPlayerEntity
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
            try
            {
                ExecuteNonQueryCommandTypeStoredProcedure();

                var ret = Parameters.LastOrDefault();
                if (ret.Value == DBNull.Value)
                    PlayerId = Guid.Empty;
                else
                    PlayerId = (Guid)ret.Value;
                
            }
            catch (Exception e)
            {
                Logger.Error(e);
                throw e;
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

        public void Insert(string playerName, Guid PlayerId)
        {
            Parameters.Clear();

            Command = "InsertPlayer";

            Parameters.Add(ParameterFactory.Create("@id", DbType.Guid, PlayerId));

            Parameters.Add(ParameterFactory.Create("@PlayerName", DbType.AnsiString, playerName));

            ExecuteTask(Insert);
        }

        public void Get(string playerName)
        {
            Parameters.Clear();

           Command = "GetPlayer";

            Parameters.Add(ParameterFactory.Create("@PlayerName", DbType.AnsiString, playerName));

            Parameters.Add(ParameterFactory.Create("@RetId", DbType.Guid, PlayerId, ParameterDirection.Output));

            ExecuteTask(Get);
        }

        public Guid PlayerId { get; set; }
    }
}
