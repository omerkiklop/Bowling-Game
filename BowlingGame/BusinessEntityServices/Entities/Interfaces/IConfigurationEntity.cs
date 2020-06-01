

using System.Collections.Generic;

namespace BusinessEntityServices.Entities.Interfaces
{
    public interface IConfigurationEntity : IEntity
    {
        string Get(string EntryKey);

        Dictionary<string, string> Get(IEnumerable<string> EntryKeys);
    }
}
