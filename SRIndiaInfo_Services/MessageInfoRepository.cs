﻿using System;
using System.Collections.Generic;
using System.Text;
using SRIndia_Repository;
using System.Linq;
using SRIndia_Models;
using Microsoft.EntityFrameworkCore;

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


        public void AddMessage(Message message)
        {
            _context.Messages.Add(message);
        }

        public void AddMessageReply(string strMessageId, MessageReply messageReply)
        {
            var Message = GetMessagesByMessageId(strMessageId,true).FirstOrDefault();
            Message.MessageReply.Add(messageReply);
        }

        public void DeleteMessage(Message message)
        {
            _context.Messages.Remove(message);
        }

        public void DeleteReply(MessageReply reply)
        {
            _context.MessageReply.Remove(reply);
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
                    objResult = GetMessagesByMessageId(strId, false);
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


        public IEnumerable<Message> GetMessagesByMessageId(string strMessageId, bool includeReplies)
        {
            if(includeReplies)
            {
                return _context.Messages.Include(c => c.MessageReply)
                    .Where(c => c.Id == strMessageId).ToList();
            }
            return _context.Messages.Where(c => c.Id == strMessageId).ToList();
        }

        public MessageReply GetMessagesReplyMessageId(string strMessageId, string strReplyId)
        {
                return _context.MessageReply.Where(m => m.Id == strReplyId && m.MessageId == strMessageId).FirstOrDefault();
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
