using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using DataAccessLayer.Builders;
using DataAccessLayer.Interfaces;
using Log;

namespace DataAccessLayer.Componenets
{
    public class SqlDataAccess : IDataAccess
    {
        private SqlConnection _databaseConnection;

        private readonly ICommandBuilder _builder;
      
        private readonly string _connectionString;

        public SqlDataAccess(string connectionString)
        {
            _connectionString = connectionString;

            _builder = new CommandBuilder();
        }

        private void SafelyCloseConnection()
        {
            if (_databaseConnection != null || _databaseConnection.State != ConnectionState.Closed)
            {
                lock (this)
                {
                    if (_databaseConnection != null || _databaseConnection.State != ConnectionState.Closed)
                    {
                        _databaseConnection.Close();
                        _databaseConnection.Dispose();
                    }
                }
            }
        }

        private SqlConnection SafelyOpenConnection()
        {
            if (_databaseConnection == null || _databaseConnection.State != ConnectionState.Open)
            {
                lock (this)
                {
                    if (_databaseConnection == null || _databaseConnection.State != ConnectionState.Open)
                    {
                        _databaseConnection = new SqlConnection(_connectionString);
                        _databaseConnection.Open();
                    }
                }
            }

            return _databaseConnection;
        }

        public int ExecuteNonQuery(string commandText, DbParameter[] parameters, CommandType commandType)
        {
            var rowsAffected = 0;

            using (var connection = SafelyOpenConnection())
            {
                var cmdExecute = _builder.BuildCommand(connection, commandText, parameters, commandType);

                try
                {
                    cmdExecute.CommandType = commandType;
                    rowsAffected = cmdExecute.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    Logger.Error(e);
                    throw e;
                }
                finally
                {
                    SafelyCloseConnection();
                }
            }

            return rowsAffected;
        }


        public object ExecuteScalar(string commandText, params DbParameter[] parameters)
        {
            using (var connection = SafelyOpenConnection())
            {
                var commands = _builder.BuildCommand(connection, commandText, parameters,CommandType.Text);

                object data;

                try
                {
                     data = commands.ExecuteScalar();
                }
                catch (Exception e)
                {
                    Logger.Error(string.Format("SqlDataAccess :: ExecuteScalar ERROR {0}", e.Message));
                    throw e;
                }
                finally
                {
                    SafelyCloseConnection();
                }
                return data;
            }
        }

        public DataTable ExecuteDataTable(string commandText, DbParameter[] parameters, CommandType commandType)
        {
            var result = new DataTable();

            using (var connection = SafelyOpenConnection())
            {
                var cmdDataTable = _builder.BuildCommand(connection, commandText, parameters,commandType);

                using (var da = new SqlDataAdapter(cmdDataTable))
                {
                    try
                    {

                        da.Fill(result);
                    }
                    catch (Exception e)
                    {
                        Logger.Error(string.Format("SqlDataAccess :: ExecuteDataTable ERROR {0}", e.Message));
                        throw e;
                    }
                    finally
                    {
                        SafelyCloseConnection();
                    }
                }
            }

            return result;
        }

        public DataSet ExecuteDataSet(string commandText, params DbParameter[] parameters)
        {
            var result = new DataSet();

            using (var connection = SafelyOpenConnection())
            {
                var cmdDataSet = _builder.BuildCommand(connection,commandText, parameters,CommandType.Text);

                using (var adapter = new SqlDataAdapter(cmdDataSet))
                {
                    try
                    {
                        adapter.Fill(result);
                    }
                    catch (Exception e)
                    {
                        Logger.Error(string.Format("SqlDataAccess :: ExecuteDataSet ERROR {0}", e.Message));
                        throw e;
                    }
                    finally
                    {
                        SafelyCloseConnection();
                    }
                }
            }

            return result;
        }
    }
}
