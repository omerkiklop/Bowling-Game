using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Interfaces
{
    public interface ICommandBuilder
    {
        SqlCommand BuildCommand(SqlConnection connection, string commandText, DbParameter[] parameters,CommandType commandType);
    }
}
