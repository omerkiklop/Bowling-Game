using System;

namespace BusinessEntityServices.Entities.Interfaces
{
    public interface IEntity
    {
        void Insert();

        void Get();

        void Update();

        void Delete();

         event Action<string> WriteExternal;
    }
}
