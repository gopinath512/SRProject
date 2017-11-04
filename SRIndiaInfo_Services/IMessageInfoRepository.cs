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

        void AddMessage(Message message);
        void AddMessageImage(Message message, ICollection<MessageImages> images);

        void AddMessageReply(string strMessageId, MessageReply messageReply);

        void DeleteMessage(Message message);

        void DeleteReply(MessageReply reply);

        IEnumerable<Message> GetMessagesByTypes(MessageTypes objMessageTypes, string strId = null);

        IEnumerable<Message> GetMessagesByMessageId(string strMessageId, bool includeReplies);

        MessageReply GetMessagesReplyByMessageId(string strMessageId, string strReplyId);

        IEnumerable<MessageImages> GetMessageImagesByMessageId(string strMessageId);

        IEnumerable<Message> GetMessagesByUserId(string strUserId);

        IEnumerable<Message> GetMessages();

        bool Save();
    }
}
