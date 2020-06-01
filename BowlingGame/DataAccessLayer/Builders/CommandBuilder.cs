using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using DataAccessLayer.Interfaces;

namespace DataAccessLayer.Builders
{
    public class CommandBuilder : ICommandBuilder
    {
        public SqlCommand BuildCommand(SqlConnection connection, string commandText,  DbParameter[] parameters,
            CommandType commandType)
        {
            var newCommand = new SqlCommand(commandText, connection)
            {
                CommandType = commandType
            };

            if (parameters != null)
                newCommand.Parameters.AddRange(parameters);

            return newCommand;
        }
    }
}
