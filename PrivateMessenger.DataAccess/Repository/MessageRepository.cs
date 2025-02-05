﻿using PrivateMessenger.DataAccess.Data;
using PrivateMessenger.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrivateMessenger.DataAccess.Repository.IRepository;

namespace PrivateMessenger.DataAccess.Repository
{
    public class MessageRepository : Repository<Message>, IMessageRepository 
    {
        private readonly ApplicationDbContext _db;

        public MessageRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task UpdateAsync(Message message)
        {
            _db.Messages.Update(message);
            // Update only text or relevant info that is being updated
        }
    }
}
