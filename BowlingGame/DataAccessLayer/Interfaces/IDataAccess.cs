using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Xml;

namespace DataAccessLayer.Interfaces
{
    public interface IDataAccess
    {
        DataSet ExecuteDataSet(string commandText, params DbParameter[] parameters);

        DataTable ExecuteDataTable(string commandText, DbParameter[] parameters, CommandType commandType);

        int ExecuteNonQuery(string commandText, DbParameter[] parameters,CommandType commandType);

        object ExecuteScalar(string commandText, params DbParameter[] parameters);

    }
}
