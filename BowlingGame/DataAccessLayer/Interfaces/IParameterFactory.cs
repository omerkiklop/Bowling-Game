using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Interfaces
{
    public interface IParameterFactory
    {
        DbParameter Create(string paramName, DbType paramType, object value);

        DbParameter Create(string paramName, DbType paramType, object value, ParameterDirection direction);

        DbParameter Create(string paramName, DbType paramType, object value, int size ,ParameterDirection direction);

    }   
}
