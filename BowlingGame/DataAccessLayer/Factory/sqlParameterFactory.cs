using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using DataAccessLayer.Interfaces;

namespace DataAccessLayer.Factory
{
    public class SqlParameterFactory : IParameterFactory
    {
        
        internal SqlDbType ConvertDbTypeToSqlDbType(DbType dbType)
        {
            SqlDbType sqlType;

            switch (dbType)
            {
                case DbType.AnsiString:
                    sqlType = SqlDbType.VarChar;
                    break;
                case DbType.Boolean:
                    sqlType = SqlDbType.Bit;
                    break;
                case DbType.DateTime:
                    sqlType = SqlDbType.DateTime;
                    break;
                case DbType.Guid:
                    sqlType = SqlDbType.UniqueIdentifier;
                    break;
                case DbType.Int32:
                    sqlType = SqlDbType.Int;
                    break;
                case DbType.Object:
                    sqlType = SqlDbType.Variant;
                    break;
                case DbType.String:
                    sqlType = SqlDbType.NVarChar;
                    break;
         
                default:
                    throw new ArgumentException(dbType.ToString() + " has no corresponding SQL Server datatype");
            }

            return sqlType;
        }

       
        public DbParameter Create(string paramName, DbType paramType, object value)
        {
            var  param = new SqlParameter(paramName, ConvertDbTypeToSqlDbType(paramType));
            if (value == null)
                param.Value = DBNull.Value;
            else
                param.Value = value;

            return param;
        }

        public DbParameter Create(string paramName, DbType paramType, object value, ParameterDirection direction)
        {
            DbParameter returnVal = Create(paramName, paramType, value);
            returnVal.Direction = direction;
            return returnVal;
        }

        public DbParameter Create(string paramName, DbType paramType, object value, int size, ParameterDirection direction)
        {
            DbParameter returnVal = Create(paramName, paramType, value);
            returnVal.Size = size;
            returnVal.Direction = direction;
            return returnVal;
            
        }
    }
}
