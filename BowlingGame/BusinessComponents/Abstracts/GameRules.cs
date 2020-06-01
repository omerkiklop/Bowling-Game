using BusinessEntityServices.Entities.Interfaces;

namespace BusinessComponents.Abstracts
{
    public abstract class GameRules
    {
        public IConfigurationEntity ConfigurationEntity { get; private set; }

        protected GameRules(IConfigurationEntity configurationEntity)
        {
            ConfigurationEntity = configurationEntity;

            GetRulesConfiguration();
        }
        public abstract void GetRulesConfiguration();
    }
}
