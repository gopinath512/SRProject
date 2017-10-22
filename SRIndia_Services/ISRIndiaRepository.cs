using System;
using System.Collections.Generic;
using System.Text;
using SRIndia_Repository;

namespace SRIndia_Services
{
    public interface ISRIndiaRepository
    {
        IEnumerable<Message> GetMessages();

        Message GetMessage(int messageId);

        void AddMessage(Message message);

        void DeleteMessage(Message message);

        bool Save();
    }
}
