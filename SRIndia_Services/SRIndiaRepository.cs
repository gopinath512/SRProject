using System;
using System.Collections.Generic;
using System.Text;
using SRIndia_Repository;


namespace SRIndia_Services
{
    public class SRIndiaRepository : ISRIndiaRepository
    {
        private SRIndiaContext _context;

        public SRIndiaRepository(SRIndiaContext context)
        {
            _context = context;
        }

        public void AddMessage(Message message)
        {
            throw new NotImplementedException();
        }

        public void DeleteMessage(Message message)
        {
            throw new NotImplementedException();
        }

        public Message GetMessage(int messageId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Message> GetMessages()
        {
            throw new NotImplementedException();
        }

        public bool Save()
        {
            throw new NotImplementedException();
        }
    }
}
