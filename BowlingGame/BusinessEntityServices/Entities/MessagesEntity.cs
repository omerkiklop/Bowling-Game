using System;
using System.Data;
using System.Linq;
using BusinessEntityServices.Entities.Interfaces;
using Log;

namespace BusinessEntityServices.Entities
{
    public class MessagesEntity : Entity,IMessagesEntity
    {
        public void Insert()
        {
            throw new NotImplementedException();
        }

        public void Get()
        {
            try
            {
                ExecuteNonQueryCommandTypeStoredProcedure();

                var ret = Parameters.LastOrDefault();
                if (ret.Value == DBNull.Value)
                    MessageValue = null;
                else
                    MessageValue = (string) ret.Value;
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

        public void Get(string messageName)
        {
            Parameters.Clear();

            Command = "GetMessage";

            Parameters.Add(ParameterFactory.Create("@MessageName", DbType.AnsiString, messageName));

            Parameters.Add(ParameterFactory.Create("@RetId", DbType.AnsiString, MessageValue,50, ParameterDirection.Output));

            ExecuteTask(Get);
        }

        public string MessageValue { get; set; }

    }
}
