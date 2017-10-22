using System;
using System.Collections.Generic;
using System.Text;
using SRIndia_Repository;
using System.Linq;
using SRIndia_Models;

namespace SRIndiaInfo_Services
{
    public class MessageInfoRepository : IMessageInfoRepository
    {
        private SRIndiaContext _context;

        public MessageInfoRepository(SRIndiaContext context)
        {
            _context = context;
        }


        public bool MessageExists(string messageId)
        {
            return _context.Messages.Any(c => c.Id == messageId);
        }

        public Message AddMessage(Message message)
        {
           var obj= _context.Messages.Add(message).Entity;
            _context.SaveChanges();
            return obj;
        }

        public void DeleteMessage(Message message)
        {
            _context.Messages.Remove(message);
        }

        public IEnumerable<Message> GetMessagesByTypes(MessageTypes objMessageTypes, string strId = null)
        {
            IEnumerable<Message> objResult ;

            switch (objMessageTypes)
            {
                case MessageTypes.All:
                    objResult = GetMessages();
                    break;
                case MessageTypes.MessageId:
                    objResult = GetMessagesByMessageId(strId);
                    break;
                case MessageTypes.UserId:
                    objResult = GetMessagesByUserId(strId);
                    break;
                default:
                    objResult = GetMessages();
                    break;
            }

            return objResult;
        }


        public IEnumerable<Message> GetMessagesByMessageId(string strMessageId)
        {
            return _context.Messages.Where(c => c.Id == strMessageId).ToList();
        }

        public IEnumerable<Message> GetMessagesByUserId(string strUserId)
        {
            return _context.Messages.Where(c => c.UserId == strUserId).ToList();
        }

        public IEnumerable<Message> GetMessages()
        {
            return _context.Messages.OrderBy(c => c.Id).ToList();
        }

        public bool Save()
        {
            return (_context.SaveChanges() >= 0);
        }
    }
}
