using System;
using System.Collections.Generic;
using System.Text;
using SRIndia_Repository;
using SRIndia_Models;

namespace SRIndiaInfo_Services
{
    public interface IMessageInfoRepository
    {
        bool MessageExists(string messageId);

        IEnumerable<Message> GetMessages();

        IEnumerable<Message> GetMessagesByMessageId(string strMessageId);

        IEnumerable<Message> GetMessagesByUserId(string strUserId);

        IEnumerable<Message> GetMessagesByTypes(MessageTypes objMessageTypes,string strId = null);

        Message AddMessage(Message message);
        
        void DeleteMessage(Message message);

        bool Save();
    }
}
