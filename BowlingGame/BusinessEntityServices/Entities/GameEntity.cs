using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using BusinessEntityServices.Entities.Interfaces;

namespace BusinessEntityServices.Entities
{
    public class GameEntity : Entity, IGameEntity
    {
        public void Insert()
        {
            try
            {
                ExecuteNonQueryCommandTypeText();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void Insert(string gameType, DateTime currentDateTime, Guid Id)
        {
            Command = "INSERT INTO Games (Id, GameType, GameDateTime) VALUES (@Id, @GameType, @GameDateTime)";

            Parameters.Add(ParameterFactory.Create("Id", DbType.Guid, Id));

            Parameters.Add(ParameterFactory.Create("GameType", DbType.AnsiString, gameType));

            Parameters.Add(ParameterFactory.Create("GameDateTime", DbType.DateTime, currentDateTime));

            ExecuteTask(Insert);
        }

        public void Get()
        {
            throw new NotImplementedException();
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
    }
}
