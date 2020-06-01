using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using DataAccessLayer.Componenets;
using DataAccessLayer.Factory;
using DataAccessLayer.Interfaces;
using Log;

namespace BusinessEntityServices.Entities
{
    public abstract class Entity
    {
        protected string Command;

        public IDataAccess DataAccessObject { get; private set; }

        public IParameterFactory ParameterFactory { get; private set; }

        protected List<DbParameter> Parameters;

        public Entity()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();

            DataAccessObject = new SqlDataAccess(connectionString);

            ParameterFactory = new SqlParameterFactory();

            Parameters = new List<DbParameter>();
        }

        public virtual void ExecuteTask(Action entityAction)
        {
            try
            {
                var task = new Task(entityAction);

                task.Start();

                task.Wait();
            }
            catch (Exception e)
            {
                throw e;
            }
           
        }

        public virtual void ExecuteNonQueryCommandTypeText()
        {
            try
            {
                DataAccessObject.ExecuteNonQuery(Command, Parameters.ToArray(), CommandType.Text);
            }
            catch (Exception e)
            {
                Logger.Error(string.Format("Entity :: ExecuteNonQueryCommandTypeText ERROR {0}", e.Message));
                throw e;
            }
        }

        public virtual void ExecuteNonQueryCommandTypeStoredProcedure()
        {
            try
            {
                DataAccessObject.ExecuteNonQuery(Command, Parameters.ToArray(), CommandType.StoredProcedure);
            }
            catch (Exception e)
            {
                Logger.Error(e);
                throw e;
            }
        }

        public virtual DataTable ExecuteExecuteDataTableStoredProcedure()
        {
            try
            {
                var res = DataAccessObject.ExecuteDataTable(Command, Parameters.ToArray(), CommandType.StoredProcedure);

                return res;
            }
            catch (Exception e)
            {
                Logger.Error(e);
                throw e;
            }
        }

    }
}
