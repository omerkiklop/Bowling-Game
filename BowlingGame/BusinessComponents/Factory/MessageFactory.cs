using System.Collections.Generic;
using BusinessComponents.Interfaces;
using BusinessEntityServices.Entities.Interfaces;

namespace BusinessComponents.Factory
{
    public class MessageFactory : IMessageFactory
    {
        public MessageFactory(IMessagesEntity messageEntity)
        {
            _messageEntity = messageEntity;

            _messagesContainer = new Dictionary<string, string>();
        }

        public string GetMessage(string key)
        {
            if (_messagesContainer.ContainsKey(key))
            {
                string output;

                var res = _messagesContainer.TryGetValue(key,out output);

                return res ? output : string.Empty;
            }

            _messageEntity.Get(key);

            _messagesContainer.Add(key, _messageEntity.MessageValue);

            return _messageEntity.MessageValue;
        }
            
        private readonly Dictionary<string, string> _messagesContainer;

        private readonly IMessagesEntity _messageEntity;
    }
}
