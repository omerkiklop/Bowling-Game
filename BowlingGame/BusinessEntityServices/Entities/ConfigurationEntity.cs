using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using BusinessEntityServices.Entities.Interfaces;


namespace BusinessEntityServices.Entities
{
    public class ConfigurationEntity : Entity, IConfigurationEntity
    {
        private DataSet ResultSet;

        public void Insert()
        {
          
        }

        public void Get()
        {
            try
            {
                ResultSet = DataAccessObject.ExecuteDataSet(Command, null);
            }
            catch (Exception e)
            {
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

        public string Get(string entryKey)
        {
            throw new NotImplementedException();
        }

        public Dictionary<string,string> Get(IEnumerable<string> entryKeys)
        {
            var sb = new StringBuilder();

           foreach (var key in entryKeys.ToList())
           {
               sb.Append("'" + key.ToString()+ "',");
           }

           var entries = sb.ToString().TrimEnd(',');

           Command = "Select EntryKey, EntryValue from [dbo].[Configuration] where EntryKey In (" + entries + ")";

           ExecuteTask(Get);
            
           return HandleResults();
        }

        private Dictionary<string, string> HandleResults()
        {
            try
            {
                return ResultSet.Tables[0].AsEnumerable()
                    .ToDictionary<DataRow, string, string>(row => row[0].ToString(),
                        row => row[1].ToString()); 
            }
            catch (Exception e)
            {
                throw e;
            }
            
        }
    }
}
